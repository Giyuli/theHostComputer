using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using ZedGraph;
using System.Runtime.InteropServices;



namespace theHostComputer
{
    public partial class MainForm : Form
    {
        #region 参数定义
        
        // Dog dog = new Dog();//看门狗
        C_Code Code = new C_Code();  //解码类
        CFunction Fun = new CFunction();  //计算腐蚀深度类        
        private Thread thread1;
        private static IPEndPoint ipep;                //new IPEndPoint(IPAddress.Parse("192.168.114.12"), 1200);                 
        private UdpClient udpClient = new UdpClient(50000);
        private string FilePath;  //数据文件路径         

        private long[,] FADData = new long[2, 6];    //解码后的各个探头磁场数据
        private double[,] FRealData = new double[12, 100000];    //计算得出的最终可用的磁场数据

        private long[] FRealDist = new long[1];          //探头移动距离              
        private C_Pubdef.TCalData FCalData = new C_Pubdef.TCalData();   // 各个探头的标定数据

        /// <summary>
        private double[,] FRealData1 = new double[2, 6];    //过度用的，分别为12个探头原始磁场值
        private double[] B = new double[9];//9个张量
        private double[,] zhangliang = new double[9, 100000];
        private double[,] dulifenliang = new double[5, 100000];
        private double[,] chafendulifenliang = new double[5, 100000];
        private double[] Bni = new double[9];//磁梯度张量梯度的逆矩阵
        private double[] R1jul = new double[3];//一号探头的3个距离
        private double[,] distxyz = new double[16, 100000];  //每个探头保存4列数据，即4*4=16列
        private double[,] Dydata2 = new double[12, 100000]; //原始数据保存

        private double[,] ysdata = new double[12, 100000]; //读原始数据并保存
        private double[,] ysdata1 = new double[12, 100000]; //读原始数据并保存
        private double mean = 0;
        private double mean1 = 0;
        private double[,] ysdatanew = new double[4, 100000];//四向下的通道新值
        private double F;
        private double F1;
        private double F2;
        private double F3;
        private double F4;

        private double[] Dydata_sigma = new double[9];
        private double[] Dydata_sum = new double[9];
        private double[] Dydata_biaozhunchasum = new double[9];
        private double[] Dydata_mean = new double[9];
        private double[] Dydata_max = new double[9];
        private double[] Dydata_min = new double[9];

        private int delete_points_flag = 0;
        private int defect_recognition_2_flag = 2;
        //private int[,] curve_display_location = new int[4, 10000];
        private int[,] chafen_delete_location = new int[4, 10000];
        private double[,] curve_display_X = new double[4, 10000];                //curve_display_X由通过新3西格玛算法提取出的缺陷信号构成
        private double[,] curve_display_Y = new double[4, 10000];
        private double[,] curve_display_Z = new double[4, 10000];                //curve_display_Z由通过新3西格玛算法提取出的缺陷信号构成
        private double[,] defect_X = new double[4, 10000];                       //defect_X数组是将用新3西格玛算法找到的缺陷进行组合，判断两个缺陷间距小于等于200mm的组合为一个缺陷
        private double[,] defect_Y = new double[4, 10000];
        private double[,] defect_Z = new double[4, 10000];                       //defect_Z数组是将用新3西格玛算法找到的缺陷进行组合，判断两个缺陷间距小于等于200mm的组合为一个缺陷
        private double[,] pic_2_imagination = new double[4, 10000];
        private double[,] pic_2_imagination_another = new double[4, 10000];
        //private double[,] reconstruction_data_display = new double[4, 10000];
        //private double[,] reconstruction_data = new double[4, 10000];
        private double[] reconstruction_data_display = new double[10000];
        private double[,] reconstruction_data = new double[4, 10000];
        //private double[] final_display = new double[10000];
        private double[,] final_imagination = new double[8, 10000];
        private double[,] original_signal = new double[12, 10000];
        private double[,] original_signal_chafen = new double[12, 10000];
        private double[,] original_signal_chafen2 = new double[12, 10000];

        private double[] original_signal_chafen2_sigma = new double[12];
        private double[] original_signal_chafen2_sum = new double[12];
        private double[] original_signal_chafen2_biaozhunchasum = new double[12];
        private double[] original_signal_chafen2_mean = new double[12];
        private double[] original_signal_chafen2_max = new double[12];
        private double[] original_signal_chafen2_min = new double[12];

        private double[,] original_signal_zuocha = new double[1, 10000];
        private double[,] original_signal_chafenzuocha = new double[1, 10000];
        //private double[,] original_signal_1 = new double[4, 100000];

        /// </summary>

        PointPairList list1 = new PointPairList();
        PointPairList list2 = new PointPairList();
        PointPairList list3 = new PointPairList();
        PointPairList list4 = new PointPairList();
        PointPairList list5 = new PointPairList();
        PointPairList list6 = new PointPairList();
        PointPairList list7 = new PointPairList();
        PointPairList list8 = new PointPairList();
        PointPairList list9 = new PointPairList();
        PointPairList list10 = new PointPairList();
        PointPairList list11 = new PointPairList();
        PointPairList list12 = new PointPairList();
        PointPairList list13 = new PointPairList();
        //private double [,] cidao=new double [12,100000];
        private double[,] cidao_1 = new double[8, 100000];
        private double[,] Freaddata = new double[12, 100000];  //预览出来的数据
        //private double [] Freadellip=new double [100000];
        private long original_num = 0;
        private int tongdao_num = 0;     //通道数
        /// <summary>曲线成阈值线
        private double[,] Freaddata2 = new double[12, 100000];  //临时借用
        private double[,] Freaddata3 = new double[12, 100000];  //判断保存用
        private double[,] Freaddata4 = new double[12, 100000]; //  用于保存合并5个通道的情况
        private double[,] yvzhi = new double[2, 6];  //阈值1
        private double[,] yvzhi2 = new double[2, 6];  //阈值2
        /// </summary>

        private long Row = 0;
        private long RRow = 0;
        private int wireless_num = 0;

        private bool linkflag = false;
        private bool startflag = false;
        private bool wireless_stopflag = false;
        private bool chselectokflag = false;
        private bool browseflag = false;
        private bool runflag = false;
        private bool[] chflag = new bool[12]; //通道选取标志       

        private double lenofpiece = 0;    //工件长度(实际扫查长度)
        private double widthOfPiece = 0;    //工件宽 
        private double ThickOftube = 0;    //管道厚度   
        ////////
        private double lenofpiece1 = 0;  //工件的实际长度
        private double kuangjiachangdu = 0;// 框架的长度0cm
        ///////

        private double DXMAX = 200;    //动态图X刻度最大值
        private double DYMAX = 100;      //动态图Y刻度最大值
        private double DXMAXStep = 20;  //动态图X刻度最小值
        private double picXMAX = 200;
        private double SMax = 100000;

        //梯度相关变量
        private double[,] Dydata = new double[12, 100000];      //原始张量数据
        private double[] dy_originalUP = new double[12];           //原始梯度上限
        private double[] dy_originalDW = new double[12];         //原始梯度下限
        private double[,] Dydata_chafen = new double[12, 100000];        //张量差分值                          

        // 等值线对应的颜色
        List<System.Drawing.Color> list_Color = new List<System.Drawing.Color>();
        //结果等值线图相关变量
        private double picDX = 0, picDY = 0;
        //缺陷定位相关变量
        private Point currentPoint = new Point(0, 0);
        private bool pointflag = false;
        private bool readflag = false;


        ////包覆层管道
        private double tiligaodu = 0;
        private double tiligaodu1 = 0;
        //private double yangguangleixin = 2;   //样管类型
        private double yangguangcheck = 1;  //yangguanqueding(da or xiao) 默认改为1（即大缺陷的形式）
        private double qccd = 100; //去除长度定义为整体变量(默认为100)
        private double qccd1 = 450; //针对7mm厚管壁
        private double qccd2 = 200; //针对5mm厚管壁
        private int chufa = 0;  //判断是否触发了某控件，默认为0，即未触发。
        private double fangxian = 0;   //5mm管道fangxian,默认为某个方向
        private double diejia7mm = 0;  //默认该参数为0

        double yzzhenliang = 0; //阈值增量
        ////
        /*************************************************************** Battery_Power_defination ********************************************************************/
        UInt32 status1 = 1;
        UInt32 status2 = 1;
        UInt32 status3 = 1;
        UInt32 Value = 0;

        byte battery = 0;
        byte addr = 0;
        byte cmd = 0;
        UInt32 length = 0;

        StringBuilder sb;
        byte[] DataBlock;

        UInt32 status1_2 = 1;
        UInt32 status2_2 = 1;
        UInt32 status3_2 = 1;
        UInt32 Value_2 = 0;

        byte battery_2 = 0;
        byte addr_2 = 0;
        byte cmd_2 = 0;
        UInt32 length_2 = 0;

        StringBuilder sb_2;
        byte[] DataBlock_2;

        UInt32 status1_voltage = 1;
        UInt32 status2_voltage = 1;
        UInt32 status3_voltage = 1;
        UInt32 Value_voltage = 0;

        UInt32 voltage = 0;
        byte addr_voltage = 0;
        byte cmd_voltage = 0;
        UInt32 length_voltage = 0;

        StringBuilder sb_voltage;
        byte[] DataBlock_voltage;

        public const UInt32 SUSI_STATUS_NOT_INITIALIZED = 0xFFFFFFFF;
        public const UInt32 SUSI_STATUS_INITIALIZED = 0xFFFFFFFE;
        public const UInt32 SUSI_STATUS_ALLOC_ERROR = 0xFFFFFFFD;
        public const UInt32 SUSI_STATUS_DRIVER_TIMEOUT = 0xFFFFFFFC;
        public const UInt32 SUSI_STATUS_INVALID_PARAMETER = 0xFFFFFEFF;
        public const UInt32 SUSI_STATUS_INVALID_BLOCK_ALIGNMENT = 0xFFFFFEFE;
        public const UInt32 SUSI_STATUS_INVALID_BLOCK_LENGTH = 0xFFFFFEFD;
        public const UInt32 SUSI_STATUS_INVALID_DIRECTION = 0xFFFFFEFC;
        public const UInt32 SUSI_STATUS_INVALID_BITMASK = 0xFFFFFEFB;
        public const UInt32 SUSI_STATUS_RUNNING = 0xFFFFFEFA;
        public const UInt32 SUSI_STATUS_UNSUPPORTED = 0xFFFFFCFF;
        public const UInt32 SUSI_STATUS_NOT_FOUND = 0xFFFFFBFF;
        public const UInt32 SUSI_STATUS_TIMEOUT = 0xFFFFFBFE;
        public const UInt32 SUSI_STATUS_BUSY_COLLISION = 0xFFFFFBFD;
        public const UInt32 SUSI_STATUS_READ_ERROR = 0xFFFFFAFF;
        public const UInt32 SUSI_STATUS_WRITE_ERROR = 0xFFFFFAFE;
        public const UInt32 SUSI_STATUS_MORE_DATA = 0xFFFFF9FF;
        public const UInt32 SUSI_STATUS_ERROR = 0xFFFFF0FF;
        public const UInt32 SUSI_STATUS_SUCCESS = 0x00000000;

        public const UInt32 SUSI_ID_SMBUS_SUPPORTED = 0x00030000;
        public const UInt32 SUSI_SMBUS_EXTERNAL_SUPPORTED = (1 << 0);
        public const UInt32 SUSI_SMBUS_OEM0_SUPPORTED = (1 << 1);
        public const UInt32 SUSI_SMBUS_OEM1_SUPPORTED = (1 << 2);
        public const UInt32 SUSI_SMBUS_OEM2_SUPPORTED = (1 << 3);
        public const UInt32 SUSI_SMBUS_OEM3_SUPPORTED = (1 << 4);

        public const UInt32 SUSI_SMBUS_MAX_DEVICE = 5;

        public const UInt32 SUSI_ID_SMBUS_EXTERNAL = 0x00000000;
        public const UInt32 SUSI_ID_SMBUS_OEM0 = 0x00000001;
        public const UInt32 SUSI_ID_SMBUS_OEM1 = 0x00000002;
        public const UInt32 SUSI_ID_SMBUS_OEM2 = 0x00000003;
        public const UInt32 SUSI_ID_SMBUS_OEM3 = 0x00000004;

        public const UInt32 SUSI_ID_BASE_GET_NAME_SMB = 0x30000000;

        public static UInt32 SUSI_ID_MAPPING_GET_NAME_SMB(UInt32 Id)
        {
            return (Id | SUSI_ID_BASE_GET_NAME_SMB);
        }

        [DllImport("Susi4.dll", EntryPoint = "SusiLibInitialize")]
        public static extern UInt32 SusiLibInitialize();
        [DllImport("Susi4.dll", EntryPoint = "SusiLibUninitialize")]
        public static extern UInt32 SusiLibUninitialize();
        [DllImport("Susi4.dll", EntryPoint = "SusiBoardGetValue")]
        public static extern UInt32 SusiBoardGetValue(UInt32 Id, ref UInt32 pValue);
        [DllImport("Susi4", EntryPoint = "SusiBoardGetStringA")]
        public static extern UInt32 SusiBoardGetStringA(UInt32 Id, StringBuilder pBuffer, ref UInt32 pBufLen);
        [DllImport("Susi4", EntryPoint = "SusiSMBI2CReadBlock")]
        public static extern UInt32 SusiSMBI2CReadBlock(UInt32 Id, byte Addr, byte Cmd, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] byte[] pBuffer, UInt32 Length);
        #endregion
        
        public MainForm()
        {

            InitializeComponent();
        }

        private void linkButton_Click(object sender, EventArgs e)
        {
            //有线//
             IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("192.168.114.12"), 1200);
            //无线//
           // IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("192.168.114.8"), 20108);
            if (linkButton.Text == "连接")
            {
                linkButton.Text = "断开";
                startButton.Enabled = true;
                byte[] bb = new byte[2] { 0x0, 0x0 };//定义一个数组用来做数据的缓冲区   
                udpClient.Send(bb, bb.Length, ipep);
                linkflag = true;
                if (startflag)
                {
                    thread1 = new Thread(new ThreadStart(ReceiveMessage));
                    thread1.IsBackground = true;
                    thread1.Start();
                    Thread.Sleep(1);
                }
            }
            else
            {
                linkButton.Text = "连接";
                startButton.Enabled = false;
                linkflag = false;
                if (startflag)
                {
                    thread1.Abort();
                }
            }
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            //  dog.isMyDog();//找狗
            //有线//
            IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("192.168.114.12"), 1200);
            //无线//
            //IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("192.168.114.8"), 20108);
            if (startButton.Text == "开始")
            {
                if (DisText.Text == "")
                {
                    MessageBox.Show("请输入扫描距离！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (DText.Text == "")
                {
                    MessageBox.Show("请输入管道外径！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (TText.Text == "")
                {
                    MessageBox.Show("请输入管道壁厚！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (TLBox.Text == "")
                {
                    MessageBox.Show("请输入管道埋深！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                else
                {
                    startButton.Text = "停止";
                    linkButton.Enabled = false;
                    SaveButton.Enabled = false;
                    runButton.Enabled = false;
                    browseButton.Enabled = false;
                    PointButton.Enabled = false;

                    RatecomboBox.Enabled = false;
                    startflag = true;
                    wireless_stopflag = false;
                    runflag = false;
                    tabControl.SelectedIndex = 0;
                    byte[] bb = new byte[4];  //定义一个数组用来做数据的缓冲区      
                    //bb[0] = 0x1;
                    //bb[1] = 0x0;
                    //bb[2] = (byte)(RatecomboBox.SelectedIndex);
                    //bb[3] = 0x0;
                    bb[0] = 0x1;
                    bb[1] = 0x1;
                    bb[2] = (byte)(RatecomboBox.SelectedIndex);//选择采样频率    最小为6.25，有线方式可以随意更改，无线方式由于空中传播速率与传输数据量的原因，经测试最大只能改为12
                    bb[3] = 0x2;
                    udpClient.Send(bb, bb.Length, ipep);

                    widthOfPiece = Math.PI * double.Parse(DText.Text);     //管道的周长

                    startButton.BackColor = Color.GreenYellow;
                    //biao_D = double.Parse(DcomboBox.Text);                                
                    ThickOftube = double.Parse(TText.Text);    //管道厚度
                    //tongdao_num = int.Parse(ChnumcomboBox.Text);
                    tongdao_num = 12;
                    //DXMAX = double.Parse(WcomboBox.Text);
                    //picXMAX = DXMAX;
                    //DXMAXStep = DXMAX / 10;
                    ZedGraphInit();       //曲线图初始化
                    //creatdatafile();  //建立文件路径                                                                  
                    listClear();
                    //creatdatafile1();  //建立文件路径                                                                  

                    //DynamicpicInit();     //动态腐蚀图初始化、清图
                    selectokButton_Click(sender, e);  //动态腐蚀曲线加图例、清曲线  
                    Row = 0;
                    RRow = 0;
                    //SRow = 0;
                    //startflag = true;

                    //接收数据
                    thread1 = new Thread(new ThreadStart(ReceiveMessage));
                    thread1.Name = "thread1";
                    thread1.Priority = ThreadPriority.Highest;
                    thread1.IsBackground = true;
                    thread1.Start();
                    Thread.Sleep(1);
                }
            }
            else
            {
                startButton.Text = "开始";
                startButton.BackColor = Color.Pink;
                linkButton.Enabled = true;
                SaveButton.Enabled = true;
                runButton.Enabled = true;
                browseButton.Enabled = true;

                RatecomboBox.Enabled = true;
                thread1.Abort();
                byte[] bb1 = new byte[3] { 0x0, 0x0, 0x0 };
                udpClient.Send(bb1, bb1.Length, ipep);

                startflag = false;
                wireless_stopflag = true;
                browseflag = false;
                readflag = false;

            }
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            if (wireless_stopflag == true)
            {
                //有线//
                // IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("192.168.114.12"), 1200);
                //无线//
                IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("192.168.114.8"), 20108);
                byte[] CC = new byte[3] { 0x0, 0x0, 0x0 };
                udpClient.Send(CC, CC.Length, ipep);
                wireless_num = wireless_num + 1;
                if (wireless_num == 3)
                {
                    wireless_num = 0;
                    wireless_stopflag = false;
                }
            }
        }

        List<byte[]> lst = new List<byte[]>();
        byte[] receiveBytes = new byte[40];
        long data;

        //接受磁场函数
        private void Revonce()
        {
            List<byte[]> lst = new List<byte[]>();
            byte[] receiveBytes = new byte[40];
            try
            {
                data = udpClient.Client.Available;
                if (data / 40 > 0)
                {
                    lst.Clear();
                    for (int i = 0; i < data / 40; i++)
                    {
                        receiveBytes = udpClient.Receive(ref ipep);
                        lst.Add(receiveBytes);
                    }
                }
                if (lst.Count > 0)
                {
                    //解码
                    for (int i = 0; i < lst.Count; i++)
                    {
                        ReceivedBytesHandle(lst[i], lst[i].Length);
                    }
                }
            }
            catch
            {

            }
        }

        private void ReceiveMessage()
        {
            while (true)
            {
                try
                {
                    Revonce();
                }
                catch
                {
                    thread1.Abort();   //中断线程 
                }
            }
        }

        private void ReceivedBytesHandle(byte[] Rev, int revSize)
        {
            FCalData.Cal_0 = new double[2, 6] { { -1066, -5474, -5030, -7311, -7304, -6493 }, { -6125, -7842, -4921, -6142, -6963, -4841 } };
            FCalData.Cal_n30000 = new double[2, 6] { { 1576187, 1567736, 1542847, 1558022, 1564538, 1557490 }, { 1575888, 1568063, 1554953, 1567310, 1562473, 1548592 } };
            FCalData.Cal_p30000 = new double[2, 6] { { -1578302, -1571003, -1545248, -1564996, -1571277, -1562874 }, { -1580230, -1576194, -1557147, -1571878, -1568494, -1550920 } };
            if (Rev[0] == C_Pubdef.REV_DATA)
            {
                if (Code.Ethernet_Can_Decode(Rev, revSize, FADData))
                {
                    Display();
                }
            }
        }

        string pathtxt;
        string timestr;
        FileStream fs;
        StreamWriter swt;

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (NoText.Text == "")
            {
                MessageBox.Show("请输入工件编号！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (DisText.Text == "")
            {
                MessageBox.Show("请输入扫描距离！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (TLBox.Text == "")
            {
                MessageBox.Show("请输入管道埋深！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            //////

            timestr = DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss");
            pathtxt = Application.StartupPath + "\\" + "datafile" + "\\" + NoText.Text + "# at " + timestr + "数据" + ".txt";
            fs = File.Open(pathtxt, FileMode.Create, FileAccess.Write);
            swt = new StreamWriter(fs, Encoding.Default);
            string txt;
            txt = "工件号：" + " " + NoText.Text;
            swt.WriteLine(txt);
            timestr = DateTime.Now.ToString();
            txt = "检测时间：" + " " + timestr;
            swt.WriteLine(txt);
            txt = "扫查距离：" + " " + DisText.Text + " " + "管道外径：" + " " + DText.Text + " " + "管道壁厚：" + " " + TText.Text + " " + "管道埋深：" + " " + TLBox.Text + " " + "判断长度：" + " " + cdtext.Text + " " + "去除长度：" + " " + qccdtext.Text;
            swt.WriteLine(txt);

            for (int i = 0; i < Row; i++)  //第一个数据不需要
            {
                swt.Write(i);  //采点序号
                swt.Write(',');

                //5个独立张量值差分后值
                swt.Write(string.Format("{0:0.0}", Dydata[0, i]));
                swt.Write(',');
                swt.Write(string.Format("{0:0.0}", Dydata[1, i]));
                swt.Write(',');
                swt.Write(string.Format("{0:0.0}", Dydata[2, i]));
                swt.Write(',');
                swt.Write(string.Format("{0:0.0}", Dydata[3, i]));
                swt.Write(',');
                swt.Write(string.Format("{0:0.0}", Dydata[4, i]));
                swt.Write(',');

                //swt.Write(string.Format("{0:0.0}", zhangliang[0, i]));
                //swt.Write(',');
                //swt.Write(string.Format("{0:0.0}", zhangliang[3, i]));
                //swt.Write(',');
                //swt.Write(string.Format("{0:0.0}", zhangliang[6, i]));
                //swt.Write(',');
                //swt.Write(string.Format("{0:0.0}", zhangliang[7, i]));
                //swt.Write(',');
                //swt.Write(string.Format("{0:0.0}", zhangliang[8, i]));
                //swt.Write(',');

                //原始磁场值
                swt.Write(string.Format("{0:0.0}", Dydata2[0, i]));
                swt.Write(',');
                swt.Write(string.Format("{0:0.0}", Dydata2[1, i]));
                swt.Write(',');
                swt.Write(string.Format("{0:0.0}", Dydata2[2, i]));
                swt.Write(',');
                swt.Write(string.Format("{0:0.0}", Dydata2[3, i]));
                swt.Write(',');
                swt.Write(string.Format("{0:0.0}", Dydata2[4, i]));
                swt.Write(',');
                swt.Write(string.Format("{0:0.0}", Dydata2[5, i]));
                swt.Write(',');
                swt.Write(string.Format("{0:0.0}", Dydata2[6, i]));
                swt.Write(',');
                swt.Write(string.Format("{0:0.0}", Dydata2[7, i]));
                swt.Write(',');
                swt.Write(string.Format("{0:0.0}", Dydata2[8, i]));
                swt.Write(',');
                swt.Write(string.Format("{0:0.0}", Dydata2[9, i]));
                swt.Write(',');
                swt.Write(string.Format("{0:0.0}", Dydata2[10, i]));
                swt.Write(',');
                swt.Write(string.Format("{0:0.0}", Dydata2[11, i]));
                swt.Write("\r\n");
            }
            swt.Flush();
            swt.Close();
            fs.Close();
        }

        //队列添加数组
        private void listAdddataRow()
        {
            list1.Add(Row, FRealData[0, Row]);
            list2.Add(Row, FRealData[1, Row]);
            list3.Add(Row, FRealData[2, Row]);
            list4.Add(Row, FRealData[3, Row]);
            list5.Add(Row, FRealData[4, Row]);
            list6.Add(Row, FRealData[5, Row]);
            list7.Add(Row, FRealData[6, Row]);
            list8.Add(Row, FRealData[7, Row]);
            list9.Add(Row, FRealData[8, Row]);
            list10.Add(Row, FRealData[9, Row]);
            list11.Add(Row, FRealData[10, Row]);
            list12.Add(Row, FRealData[11, Row]);

        }

        //队列移除数组
        private void listDeletedata()
        {
            list1.RemoveAt(0);
            list2.RemoveAt(0);
            list3.RemoveAt(0);
            list4.RemoveAt(0);
            list5.RemoveAt(0);
            list6.RemoveAt(0);
            list7.RemoveAt(0);
            list8.RemoveAt(0);
            list9.RemoveAt(0);
            list10.RemoveAt(0);
            list11.RemoveAt(0);
            list12.RemoveAt(0);
        }

        //队列清空
        private void listClear()
        {
            list1.Clear();
            list2.Clear();
            list3.Clear();
            list4.Clear();
            list5.Clear();
            list6.Clear();
            list7.Clear();
            list8.Clear();
            list9.Clear();
            list10.Clear();
            list11.Clear();
            list12.Clear();

        }

        //按时间显示并保存磁场数据
        private void Display()
        {
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    if (FADData[i, j] >= 0)
                    {
                        FRealData1[i, j] = (FADData[i, j] - FCalData.Cal_0[i, j]) * 30000 / (FCalData.Cal_n30000[i, j] - FCalData.Cal_0[i, j]);
                    }
                    else
                    {
                        FRealData1[i, j] = (FADData[i, j] - FCalData.Cal_0[i, j]) * (-30000) / (FCalData.Cal_p30000[i, j] - FCalData.Cal_0[i, j]);
                    }
                }
            }
            if (RRow > 4)
            {
                B[0] = (FRealData1[0, 3] - FRealData1[1, 3]);
                B[1] = (FRealData1[0, 5] - FRealData1[1, 5]);
                B[2] = (FRealData1[1, 0] - FRealData1[0, 0]);
                B[3] = B[1];
                B[5] = (FRealData1[1, 2] - FRealData1[0, 2]);
                B[6] = (FRealData1[0, 4] - FRealData1[1, 4]);
                B[7] = B[5];
                B[8] = (FRealData1[1, 1] - FRealData1[0, 1]);
                B[4] = -(B[0] + B[8]);

                for (int i = 0; i < 9; i++)
                {
                    zhangliang[i, Row] = B[i];
                }

                //存储5个独立分量
                if (Row > 0)
                {
                    FRealData[0, Row] = zhangliang[0, Row];
                    FRealData[1, Row] = zhangliang[1, Row];
                    FRealData[2, Row] = zhangliang[2, Row];
                    FRealData[3, Row] = zhangliang[3, Row];
                    FRealData[4, Row] = zhangliang[4, Row];
                    FRealData[5, Row] = zhangliang[5, Row];
                    FRealData[6, Row] = zhangliang[6, Row];
                    FRealData[7, Row] = zhangliang[7, Row];
                    FRealData[8, Row] = zhangliang[8, Row];

                    listAdddataRow();

                    Dydata[0, Row] = FRealData[0, Row];
                    Dydata[1, Row] = FRealData[3, Row];
                    Dydata[2, Row] = FRealData[6, Row];
                    Dydata[3, Row] = FRealData[7, Row];
                    Dydata[4, Row] = FRealData[8, Row];
                }
                //原始数据保存
                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 6; j++)
                    {
                        Dydata2[i * 6 + j, Row] = FRealData1[i, j];
                    }
                }

                mean1 = (Dydata2[2, Row] + Dydata2[5, Row] + Dydata2[8, Row] + Dydata2[11, Row]) / 4;

                ysdatanew[0, Row] = Dydata2[2, Row] - mean1;
                ysdatanew[1, Row] = Dydata2[5, Row] - mean1;
                ysdatanew[2, Row] = Dydata2[8, Row] - mean1;
                ysdatanew[3, Row] = Dydata2[11, Row] - mean1;

                //曲线显示
                if (chselectokflag)
                {
                    ZedGraph1Adddata();
                }
                Row++;
            }
            RRow++;
            if (list1.Count > DXMAX)
            {
                listDeletedata();
            }
        }

        //主程序加载
        private void MainForm_Load(object sender, EventArgs e)
        {
            //埋地管道
            qccd = double.Parse(qccdtext.Text);  //
            //
            timer.Enabled = true;
            SaveButton.Enabled = false;
            startButton.Enabled = false;
            runButton.Enabled = false;
            Chanelcombo.Enabled = false;
            Control.CheckForIllegalCrossThreadCalls = false;
            double[] BLUE = new double[64] { 0.5625, 0.625, 0.6875, 0.75, 0.8125, 0.875, 0.9375, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0.9375, 0.875, 0.8125, 0.75, 0.6875, 0.625, 0.5625, 0.5, 0.4375, 0.375, 0.3125, 0.25, 0.1875, 0.125, 0.0625, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            double[] GREEN = new double[64] { 0, 0, 0, 0, 0, 0, 0, 0, 0.0625, 0.125, 0.1875, 0.25, 0.3125, 0.375, 0.4375, 0.5, 0.5625, 0.625, 0.6875, 0.75, 0.8125, 0.875, 0.9375, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0.9375, 0.875, 0.8125, 0.75, 0.6875, 0.625, 0.5625, 0.5, 0.4375, 0.375, 0.3125, 0.25, 0.1875, 0.125, 0.0625, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            double[] RED = new double[64] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0.0625, 0.125, 0.1875, 0.25, 0.3125, 0.375, 0.4375, 0.5, 0.5625, 0.625, 0.6875, 0.75, 0.8125, 0.875, 0.9375, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0.9375, 0.875, 0.8125, 0.75, 0.6875, 0.625, 0.5625, 0.5 };
            System.Drawing.Color temp_C;
            for (int i = 0; i < 64; i++)
            {
                temp_C = System.Drawing.Color.FromArgb((int)(RED[i] * 255), (int)(GREEN[i] * 255), (int)(BLUE[i] * 255));
                list_Color.Add(temp_C);
            }
            RatecomboBox.SelectedIndex = 1;
            ZedGraphInit();

        }

        //退出按钮
        private void exitButton_Click(object sender, EventArgs e)
        {
            //System.Diagnostics.Process.Start("cmd.exe", "/cshutdown -s -t 60");
            // udpClient.Close();            
            EndingForm eForm = new EndingForm();
            eForm.Show();
            this.Close();  //本窗体退出          
            
        }

        //zedgraph初始化
        private void ZedGraphInit()
        {
            //获取引用
            GraphPane myPane = zedGraphControl1.GraphPane;

            //清空原图像
            myPane.CurveList.Clear();
            myPane.GraphObjList.Clear();
            zedGraphControl1.Refresh();

            //设置标题
            myPane.Title.Text = "";
            //设置X轴说明文字
            myPane.XAxis.Title.Text = "距离";
            //设置Y轴说明文字
            myPane.YAxis.Title.Text = "磁场变化/nT";

            myPane.XAxis.Scale.Min = 0;		//X轴最小值0
            myPane.XAxis.Scale.Max = 200;	//X轴最大100
            myPane.XAxis.Scale.MinorStep = 1;//X轴小步长1,也就是小间隔
            myPane.XAxis.Scale.MajorStep = 5;//X轴大步长为5，也就是显示文字的大间隔   

            /////////////////////////////////////////////////////////////////////////////////
            //获取引用
            GraphPane myPane1 = zedGraphControl2.GraphPane;

            //清空原图像
            myPane1.CurveList.Clear();
            myPane1.GraphObjList.Clear();
            zedGraphControl2.Refresh();

            //设置标题
            myPane1.Title.Text = "";
            //设置X轴说明文字
            myPane1.XAxis.Title.Text = "距离/mm";
            //设置Y轴说明文字
            myPane1.YAxis.Title.Text = "磁场变化/nT";

            myPane1.XAxis.Scale.Min = 0;		//X轴最小值0
            myPane1.XAxis.Scale.Max = 200;	//X轴最大100
            myPane1.XAxis.Scale.MinorStep = 1;//X轴小步长1,也就是小间隔
            myPane1.XAxis.Scale.MajorStep = 5;//X轴大步长为5，也就是显示文字的大间隔

            /////////////////////////////////////////////////////////////////////////////////////////////

            //获取引用
            GraphPane myPane2 = zedGraphControl3.GraphPane;

            //清空原图像
            myPane2.CurveList.Clear();
            myPane2.GraphObjList.Clear();
            zedGraphControl3.Refresh();

            //设置标题
            myPane2.Title.Text = "";
            //设置X轴说明文字
            myPane2.XAxis.Title.Text = "距离/mm";
            //设置Y轴说明文字
            myPane2.YAxis.Title.Text = "磁场变化/nT";

            myPane2.XAxis.Scale.Min = 0;		//X轴最小值0
            myPane2.XAxis.Scale.Max = 200;	//X轴最大100
            myPane2.XAxis.Scale.MinorStep = 1;//X轴小步长1,也就是小间隔
            myPane2.XAxis.Scale.MajorStep = 5;//X轴大步长为5，也就是显示文字的大间隔  

            /////////////////////////////////////////////////////////////////////////////////////////////

            //获取引用
            GraphPane myPane3 = zedGraphControl3_2.GraphPane;

            //清空原图像
            myPane3.CurveList.Clear();
            myPane3.GraphObjList.Clear();
            zedGraphControl3_2.Refresh();

            //设置标题
            myPane3.Title.Text = "";
            //设置X轴说明文字
            myPane3.XAxis.Title.Text = "距离/mm";
            //设置Y轴说明文字
            myPane3.YAxis.Title.Text = "磁场变化/nT";

            myPane3.XAxis.Scale.Min = 0;		//X轴最小值0
            myPane3.XAxis.Scale.Max = 200;	//X轴最大100
            myPane3.XAxis.Scale.MinorStep = 1;//X轴小步长1,也就是小间隔
            myPane3.XAxis.Scale.MajorStep = 5;//X轴大步长为5，也就是显示文字的大间隔

            /////////////////////////////////////////////////////////////////////////////////////////////

            //获取引用
            GraphPane myPane4 = zedGraphControl4.GraphPane;

            //清空原图像
            myPane4.CurveList.Clear();
            myPane4.GraphObjList.Clear();
            zedGraphControl4.Refresh();

            //设置标题
            myPane4.Title.Text = "";
            //设置X轴说明文字
            myPane4.XAxis.Title.Text = "距离/mm";
            //设置Y轴说明文字
            myPane4.YAxis.Title.Text = "磁场变化/nT";

            myPane4.XAxis.Scale.Min = 0;		//X轴最小值0
            myPane4.XAxis.Scale.Max = 200;	//X轴最大100
            myPane4.XAxis.Scale.MinorStep = 1;//X轴小步长1,也就是小间隔
            myPane4.XAxis.Scale.MajorStep = 5;//X轴大步长为5，也就是显示文字的大间隔  

            /////////////////////////////////////////////////////////////////////////////////////////////

            /////////////////////////////////////////////////////////////////////////////////////////////

            //获取引用
            GraphPane myPane5 = zedGraphControl5.GraphPane;

            //清空原图像
            myPane5.CurveList.Clear();
            myPane5.GraphObjList.Clear();
            zedGraphControl5.Refresh();

            //设置标题
            myPane5.Title.Text = "";
            //设置X轴说明文字
            myPane5.XAxis.Title.Text = "距离/mm";
            //设置Y轴说明文字
            myPane5.YAxis.Title.Text = "磁场变化/nT";

            myPane5.XAxis.Scale.Min = 0;		//X轴最小值0
            myPane5.XAxis.Scale.Max = 200;	//X轴最大100
            myPane5.XAxis.Scale.MinorStep = 1;//X轴小步长1,也就是小间隔
            myPane5.XAxis.Scale.MajorStep = 5;//X轴大步长为5，也就是显示文字的大间隔  

            /////////////////////////////////////////////////////////////////////////////////////////////

            //改变轴的刻度
            myPane.AxisChange();
            myPane1.AxisChange();
            myPane2.AxisChange();
            myPane3.AxisChange();
            myPane4.AxisChange();
            myPane5.AxisChange();
            zedGraphControl1.Invalidate();
            zedGraphControl2.Invalidate();
            zedGraphControl3.Invalidate();
            zedGraphControl3_2.Invalidate();
            zedGraphControl4.Invalidate();
            zedGraphControl5.Invalidate();
        }

        //全选

        //全选按钮
        private void selectallCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CH1checkBox.Checked = selectallCheckBox.Checked;
            CH2checkBox.Checked = selectallCheckBox.Checked;
            CH3checkBox.Checked = selectallCheckBox.Checked;
            CH4checkBox.Checked = selectallCheckBox.Checked;
            CH5checkBox.Checked = selectallCheckBox.Checked;
            CH6checkBox.Checked = selectallCheckBox.Checked;
            CH7checkBox.Checked = selectallCheckBox.Checked;
            CH8checkBox.Checked = selectallCheckBox.Checked;
            CH9checkBox.Checked = selectallCheckBox.Checked;
            CH10checkBox.Checked = selectallCheckBox.Checked;
            CH11checkBox.Checked = selectallCheckBox.Checked;
            CH12checkBox.Checked = selectallCheckBox.Checked;

        }

        private void selectokButton_Click(object sender, EventArgs e)
        {
            chselectokflag = false;
            chflag[0] = CH1checkBox.Checked;
            chflag[1] = CH2checkBox.Checked;
            chflag[2] = CH3checkBox.Checked;
            chflag[3] = CH4checkBox.Checked;
            chflag[4] = CH5checkBox.Checked;
            chflag[5] = CH6checkBox.Checked;
            chflag[6] = CH7checkBox.Checked;
            chflag[7] = CH8checkBox.Checked;
            chflag[8] = CH9checkBox.Checked;
            chflag[9] = CH10checkBox.Checked;
            chflag[10] = CH11checkBox.Checked;
            chflag[11] = CH12checkBox.Checked;

            ZedGraph1Addcurve();  //动态腐蚀曲线添加图例

            if (chflag[0] || chflag[1] || chflag[2] || chflag[3] || chflag[4] || chflag[5] || chflag[6] || chflag[7] || chflag[8] || chflag[9] || chflag[10] || chflag[11])
            {
                chselectokflag = true;
            }
            else
            {
                chselectokflag = false;
            }
        }

        //动态曲线图添加图例
        private void ZedGraph1Addcurve()
        {
            zedGraphControl1.GraphPane.CurveList.Clear();
            zedGraphControl1.GraphPane.GraphObjList.Clear();
            zedGraphControl1.Refresh();

            if (chflag[0] == true)
            {
                PointPairList lst1 = new PointPairList();
                LineItem mycurve1 = zedGraphControl1.GraphPane.AddCurve("CH1", lst1, Color.Blue, SymbolType.None);
            }

            if (chflag[1] == true)
            {
                PointPairList lst2 = new PointPairList();
                LineItem mycurve2 = zedGraphControl1.GraphPane.AddCurve("CH2", lst2, Color.Red, SymbolType.None);
            }

            if (chflag[2] == true)
            {
                PointPairList lst3 = new PointPairList();
                LineItem mycurve3 = zedGraphControl1.GraphPane.AddCurve("CH3", lst3, Color.Brown, SymbolType.None);
            }

            if (chflag[3] == true)
            {
                PointPairList lst4 = new PointPairList();
                LineItem mycurve4 = zedGraphControl1.GraphPane.AddCurve("CH4", lst4, Color.Green, SymbolType.None);
            }

            if (chflag[4] == true)
            {
                PointPairList lst5 = new PointPairList();
                LineItem mycurve5 = zedGraphControl1.GraphPane.AddCurve("CH5", lst5, Color.Black, SymbolType.None);
            }

            if (chflag[5] == true)
            {
                PointPairList lst6 = new PointPairList();
                LineItem mycurve6 = zedGraphControl1.GraphPane.AddCurve("CH6", lst6, Color.RosyBrown, SymbolType.None);
            }

            if (chflag[6] == true)
            {
                PointPairList lst7 = new PointPairList();
                LineItem mycurve7 = zedGraphControl1.GraphPane.AddCurve("CH7", lst7, Color.Violet, SymbolType.None);
            }

            if (chflag[7] == true)
            {
                PointPairList lst8 = new PointPairList();
                LineItem mycurve8 = zedGraphControl1.GraphPane.AddCurve("CH8", lst8, Color.Gray, SymbolType.None);
            }

            if (chflag[8] == true)
            {
                PointPairList lst9 = new PointPairList();
                LineItem mycurve9 = zedGraphControl1.GraphPane.AddCurve("CH9", lst9, Color.DarkGreen, SymbolType.None);
            }

            if (chflag[9] == true)
            {
                PointPairList lst10 = new PointPairList();
                LineItem mycurve10 = zedGraphControl1.GraphPane.AddCurve("CH10", lst10, Color.DarkRed, SymbolType.None);
            }

            if (chflag[10] == true)
            {
                PointPairList lst11 = new PointPairList();
                LineItem mycurve11 = zedGraphControl1.GraphPane.AddCurve("CH11", lst11, Color.DarkBlue, SymbolType.None);
            }

            if (chflag[11] == true)
            {
                PointPairList lst12 = new PointPairList();
                LineItem mycurve12 = zedGraphControl1.GraphPane.AddCurve("CH12", lst12, Color.Purple, SymbolType.None);
            }

            zedGraphControl1.GraphPane.XAxis.Scale.Min = 0;		//X轴最小值0
            zedGraphControl1.GraphPane.XAxis.Scale.Max = DXMAX;	//X轴最大100
            zedGraphControl1.GraphPane.XAxis.Scale.MinorStep = DXMAXStep / 5;//X轴小步长1,也就是小间隔
            zedGraphControl1.GraphPane.XAxis.Scale.MajorStep = DXMAXStep;//X轴大步长为5，也就是显示文字的大间隔 
            zedGraphControl1.AxisChange();
            zedGraphControl1.Invalidate();
        }

        int count = 0;

        //动态曲线图添加数据
        private void ZedGraph1Adddata()
        {
            //清除原图像       
            int i = 0;
            long displayrow = Row;
            if (Row > 1)
            {
                if (chflag[0] == true)
                {
                    LineItem curve0 = zedGraphControl1.GraphPane.CurveList[i] as LineItem;
                    IPointListEdit list1 = curve0.Points as IPointListEdit;
                    list1.Add(displayrow, Dydata2[0, Row]);
                    if (list1.Count > DXMAX)
                    {
                        list1.RemoveAt(0);
                    }
                    i++;
                }

                if (chflag[1] == true)
                {
                    LineItem curve1 = zedGraphControl1.GraphPane.CurveList[i] as LineItem;
                    IPointListEdit list2 = curve1.Points as IPointListEdit;
                    list2.Add(displayrow, Dydata2[1, Row]);
                    if (list2.Count > DXMAX)
                    {
                        list2.RemoveAt(0);
                    }
                    i++;
                }

                if (chflag[2] == true)
                {
                    LineItem curve2 = zedGraphControl1.GraphPane.CurveList[i] as LineItem;
                    IPointListEdit list3 = curve2.Points as IPointListEdit;
                    list3.Add(displayrow, Dydata2[2, Row]);
                    if (list3.Count > DXMAX)
                    {
                        list3.RemoveAt(0);
                    }
                    i++;
                }

                if (chflag[3] == true)
                {
                    LineItem curve3 = zedGraphControl1.GraphPane.CurveList[i] as LineItem;
                    IPointListEdit list4 = curve3.Points as IPointListEdit;
                    list4.Add(displayrow, Dydata2[3, Row]);
                    if (list4.Count > DXMAX)
                    {
                        list4.RemoveAt(0);
                    }
                    i++;
                }

                if (chflag[4] == true)
                {
                    LineItem curve4 = zedGraphControl1.GraphPane.CurveList[i] as LineItem;
                    IPointListEdit list5 = curve4.Points as IPointListEdit;
                    list5.Add(displayrow, Dydata2[4, Row]);
                    if (list5.Count > DXMAX)
                    {
                        list5.RemoveAt(0);
                    }
                    i++;
                }

                if (chflag[5] == true)
                {
                    LineItem curve5 = zedGraphControl1.GraphPane.CurveList[i] as LineItem;
                    IPointListEdit list6 = curve5.Points as IPointListEdit;
                    list6.Add(displayrow, Dydata2[5, Row]);
                    if (list6.Count > DXMAX)
                    {
                        list6.RemoveAt(0);
                    }
                    i++;
                }

                if (chflag[6] == true)
                {
                    LineItem curve6 = zedGraphControl1.GraphPane.CurveList[i] as LineItem;
                    IPointListEdit list7 = curve6.Points as IPointListEdit;
                    list7.Add(displayrow, Dydata2[6, Row]);
                    if (list7.Count > DXMAX)
                    {
                        list7.RemoveAt(0);
                    }
                    i++;
                }

                if (chflag[7] == true)
                {
                    LineItem curve7 = zedGraphControl1.GraphPane.CurveList[i] as LineItem;
                    IPointListEdit list8 = curve7.Points as IPointListEdit;
                    list8.Add(displayrow, Dydata2[7, Row]);
                    if (list8.Count > DXMAX)
                    {
                        list8.RemoveAt(0);
                    }
                    i++;
                }

                if (chflag[8] == true)
                {
                    LineItem curve8 = zedGraphControl1.GraphPane.CurveList[i] as LineItem;
                    IPointListEdit list9 = curve8.Points as IPointListEdit;
                    list9.Add(displayrow, Dydata2[8, Row]);
                    if (list9.Count > DXMAX)
                    {
                        list9.RemoveAt(0);
                    }
                    i++;
                }

                if (chflag[9] == true)
                {
                    LineItem curve9 = zedGraphControl1.GraphPane.CurveList[i] as LineItem;
                    IPointListEdit list10 = curve9.Points as IPointListEdit;
                    list10.Add(displayrow, Dydata2[9, Row]);
                    if (list10.Count > DXMAX)
                    {
                        list10.RemoveAt(0);
                    }
                    i++;
                }

                if (chflag[10] == true)
                {
                    LineItem curve10 = zedGraphControl1.GraphPane.CurveList[i] as LineItem;
                    IPointListEdit list11 = curve10.Points as IPointListEdit;
                    list11.Add(displayrow, Dydata2[10, Row]);
                    if (list11.Count > DXMAX)
                    {
                        list11.RemoveAt(0);
                    }
                    i++;
                }

                if (chflag[11] == true)
                {
                    LineItem curve11 = zedGraphControl1.GraphPane.CurveList[i] as LineItem;
                    IPointListEdit list12 = curve11.Points as IPointListEdit;
                    list12.Add(displayrow, Dydata2[11, Row]);
                    if (list12.Count > DXMAX)
                    {
                        list12.RemoveAt(0);
                    }
                    i++;
                }

                Scale xScale = zedGraphControl1.GraphPane.XAxis.Scale;
                if (displayrow > xScale.Max)
                {
                    xScale.Max = displayrow;
                    xScale.Min = xScale.Max - DXMAX;
                }

                count = count + i;

                XtextBox.Text = count.ToString();

                //每个点进行一次绘制
                //zedGraphControl1.AxisChange();
                //zedGraphControl1.Invalidate();

                //每10个点进行一次绘制(当频率达到100及以上时)
                if (Row % 10 == 0 && RatecomboBox.SelectedIndex > 3)
                {
                    zedGraphControl1.AxisChange();
                    zedGraphControl1.Invalidate();
                }
                else
                {
                    zedGraphControl1.AxisChange();
                    zedGraphControl1.Invalidate();
                }
            }
        }

        //数据处理按钮
        private void runButton_Click(object sender, EventArgs e)
        {
            linkButton.Enabled = false;
            startButton.Enabled = false;
            browseButton.Enabled = false;
            /////
            tiligaodu = Convert.ToDouble(TLBox.Text);  //读取提离高度
            widthOfPiece = Math.PI * Convert.ToDouble(DText.Text);     //管道的周长
            double[,] orignal_chafen = new double[4, 10000];         //原始信号的差分值
            double[,] delete_feedback = new double[4, 10000];
            double[,] delete_points = new double[4, 10000];           //第一次删点结果
            long[] delete_points_capacity = new long[4];
            long[] orignal_chafen_capacity = new long[4];
            defect_recognition_2_flag = 2;
            int feedback_location = 0;

            if (browseflag == false && readflag == false)
            {
                if (DisText.Text == "")
                {
                    MessageBox.Show("请输入扫描距离，单位：mm", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    linkButton.Enabled = true;
                    if (linkflag) { startButton.Enabled = true; }   //如果连接了，开始按钮可用
                    browseButton.Enabled = true;  //预览数据按钮可用                   
                    exitButton.Enabled = true;
                    return;
                }
                else
                {
                    lenofpiece1 = double.Parse(DisText.Text);
                    lenofpiece = lenofpiece1 - kuangjiachangdu;  //实际长度为扫查长度减去框架长度
                }
                //获得实时数据
                for (int i = 3; i < Row; i++)   //从1开始算是应为去除端头异常数据
                {
                    //Freaddata2[2, i] = Dydata[0, i];   //通道修改了
                    //Freaddata2[1, i] = Dydata[1, i];
                    //Freaddata2[3, i] = Dydata[2, i];
                    //Freaddata2[4, i] = Dydata[3, i];
                    //Freaddata2[0, i] = Dydata[4, i];

                    Freaddata2[0, i] = Dydata[0, i];   //通道修改了
                    Freaddata2[1, i] = Dydata[1, i];
                    Freaddata2[2, i] = Dydata[2, i];
                    Freaddata2[3, i] = Dydata[3, i];
                    Freaddata2[4, i] = Dydata[4, i];
                }
                original_num = Row - 1;
                /***********************************************************2016.4.10 添加加步行缺陷提取算法***********************************************************/
                //采集数据后直接处理
                for (int i = 0; i < 12; i++)
                {
                    for (int j = 0; j < Row; j++)
                    {
                        original_signal[i, j] = Dydata2[i, j];
                    }
                }
                //for (int i = 0; i < 4; i++)
                //{
                //    for (int j = 0; j < Row; j++)
                //    {
                //        original_signal[i, j] = Dydata2[6, j] - Dydata2[0, j];
                //    }
                //}00

                for (int i = 0; i < 4; i++)                                                        //求X方向的原始信号的差分值
                {
                    orignal_chafen_capacity[i] = Row - 1;
                    for (int j = 0; j < orignal_chafen_capacity[i]; j++)
                    {
                        orignal_chafen[i, j] = Dydata2[(0 + 3 * i), j + 1] - Dydata2[(0 + 3 * i), j];
                    }
                }
                //for (int i = 0; i < 4; i++)                                                        //求X方向的原始信号的差分值
                //{
                //    orignal_chafen_capacity[i] = Row - 1;
                //    for (int j = 0; j < orignal_chafen_capacity[i]; j++)
                //    {
                //        orignal_chafen[i, j] = original_signal[i, j + 1] - original_signal[i, j];
                //    }
                //}
                delete_feedback = delete_points_function_first_X(orignal_chafen, orignal_chafen, orignal_chafen_capacity);      //对X方向差分值得第一次删点
                while (delete_points_flag != int.Parse(cdtext.Text))      //无论第一次删没删完都会进入循环，以后每次如果还有超阈值的点都会进入该循环，直到没有超阈值的点为止
                {
                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 0; j < 10000; j++)
                        {
                            if (delete_feedback[i, j] != -10000000000000)
                            {
                                delete_points[i, j] = delete_feedback[i, j];       //取出被删点后的剩余数据，delete_points为剩余数据组成的新数组
                            }
                            if (delete_feedback[i, j] == -10000000000000)
                            {
                                delete_points_capacity[i] = (long)delete_feedback[i, j + 1];     //取出被删点后的剩余数据的个数，也就是delete_points数组的容量
                            }
                        }
                    }
                    delete_feedback = delete_points_function_again_X(orignal_chafen, delete_points, delete_points_capacity);        //对X方向进行过第一次删点后的数据再次进行删点
                }
                defect_X = defect_recognition(curve_display_X, original_num - 1);      //defect_X数组是将用3西格玛算法找到的缺陷进行组合，判断两个缺陷间距小于等于200mm的组合为一个缺陷


                //for (int i = 0; i < 4; i++)                                                        //求Y方向的原始信号的差分值
                //{
                //    orignal_chafen_capacity[i] = Row - 1;
                //    for (int j = 0; j < orignal_chafen_capacity[i]; j++)
                //    {
                //        orignal_chafen[i, j] = Dydata2[(1 + 3 * i), j + 1] - Dydata2[(1 + 3 * i), j];
                //    }
                //}
                //delete_feedback = delete_points_function_first_Y(orignal_chafen, orignal_chafen, orignal_chafen_capacity);      //对X方向差分值得第一次删点
                //while (delete_points_flag != 2)      //无论第一次删没删完都会进入循环，以后每次如果还有超阈值的点都会进入该循环，直到没有超阈值的点为止
                //{
                //    for (int i = 0; i < 4; i++)
                //    {
                //        for (int j = 0; j < 10000; j++)
                //        {
                //            if (delete_feedback[i, j] != -10000000000000)
                //            {
                //                delete_points[i, j] = delete_feedback[i, j];       //取出被删点后的剩余数据，delete_points为剩余数据组成的新数组
                //            }
                //            if (delete_feedback[i, j] == -10000000000000)
                //            {
                //                delete_points_capacity[i] = (long)delete_feedback[i, j + 1];     //取出被删点后的剩余数据的个数，也就是delete_points数组的容量
                //            }
                //        }
                //    }
                //    delete_feedback = delete_points_function_again_Y(orignal_chafen, delete_points, delete_points_capacity);        //对X方向进行过第一次删点后的数据再次进行删点
                //}
                //defect_Y = defect_recognition(curve_display_Y, original_num - 1);      //defect_X数组是将用3西格玛算法找到的缺陷进行组合，判断两个缺陷间距小于等于200mm的组合为一个缺陷

                for (int i = 0; i < 4; i++)                                                        //求Z方向的原始信号的差分值
                {
                    orignal_chafen_capacity[i] = Row - 1;
                    for (int j = 0; j < orignal_chafen_capacity[i]; j++)
                    {
                        orignal_chafen[i, j] = Dydata2[(2 + 3 * i), j + 1] - Dydata2[(2 + 3 * i), j];
                    }
                }
                delete_feedback = delete_points_function_first_Z(orignal_chafen, orignal_chafen, orignal_chafen_capacity);
                while (delete_points_flag != int.Parse(cdtext.Text))
                {
                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 0; j < 10000; j++)
                        {
                            if (delete_feedback[i, j] != -10000000000000)
                            {
                                delete_points[i, j] = delete_feedback[i, j];
                            }
                            if (delete_feedback[i, j] == -10000000000000)
                            {
                                delete_points_capacity[i] = (long)delete_feedback[i, j + 1];
                            }
                        }
                    }
                    delete_feedback = delete_points_function_again_Z(orignal_chafen, delete_points, delete_points_capacity);
                }
                defect_Z = defect_recognition(curve_display_Z, original_num - 1);

                pic_2_imagination = defect_recognition_XplusZ(defect_X, defect_Z, original_num - 1);
                //pic_2_imagination = defect_recognition_XplusZ(curve_display_X, curve_display_Z, original_num - 1);
                pic_2_imagination_another = defect_recognition_another(curve_display_X, curve_display_Z, original_num - 1);
                reconstruction_data_display = reconstruction(pic_2_imagination, pic_2_imagination_another, original_num - 1);
                while (defect_recognition_2_flag == 2)
                {
                    reconstruction_data_display = defect_recognition_2(reconstruction_data_display, original_num - 1);
                }
                /******************************************************************************************************************************************************/
            }

            /***********************************************************2016.4.10 添加加步行缺陷提取算法***********************************************************/
            //读取已存数据后再数据处理
            if (browseflag == true && readflag == true)
            {
                for (int i = 0; i < 12; i++)
                {
                    for (int j = 0; j < original_num; j++)
                    {
                        original_signal[i, j] = ysdata[i, j];
                    }
                }
                //for (int i = 0; i < 4; i++)
                //{
                //    for (int j = 0; j < original_num; j++)
                //    {
                //        original_signal_1[i, j] = ysdata[6, j] - ysdata[0, j];
                //    }
                //}
                for (int i = 0; i < 4; i++)                                                        //求X方向的原始信号的差分值
                {
                    orignal_chafen_capacity[i] = original_num - 1;
                    for (int j = 0; j < (orignal_chafen_capacity[i]); j++)
                    {
                        orignal_chafen[i, j] = ysdata[(0 + 3 * i), j + 1] - ysdata[(0 + 3 * i), j];
                    }
                }
                //for (int i = 0; i < 4; i++)                                                        //求X方向的原始信号的差分值
                //{
                //    orignal_chafen_capacity[i] = original_num - 1;
                //    for (int j = 0; j < orignal_chafen_capacity[i]; j++)
                //    {
                //        orignal_chafen[i, j] = original_signal_1[i, j + 1] - original_signal_1[i, j];
                //    }
                //}
                delete_feedback = delete_points_function_first_X(orignal_chafen, orignal_chafen, orignal_chafen_capacity);
                while (delete_points_flag != int.Parse(cdtext.Text))
                {
                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 0; j < 10000; j++)
                        {
                            if (delete_feedback[i, j] == -10000000000000)
                            {
                                feedback_location = j;
                            }
                        }
                        for (int j = 0; j < feedback_location; j++)
                        {
                            delete_points[i, j] = delete_feedback[i, j];
                        }
                        delete_points_capacity[i] = (long)delete_feedback[i, feedback_location + 1];
                    }
                    delete_feedback = delete_points_function_again_X(orignal_chafen, delete_points, delete_points_capacity);
                }
                defect_X = defect_recognition(curve_display_X, original_num - 1);

                //for (int i = 0; i < 4; i++)                                                        //求Y方向的原始信号的差分值
                //{
                //    orignal_chafen_capacity[i] = original_num - 1;
                //    for (int j = 0; j < (orignal_chafen_capacity[i]); j++)
                //    {
                //        orignal_chafen[i, j] = ysdata[(1 + 3 * i), j + 1] - ysdata[(1 + 3 * i), j];
                //    }
                //}
                //delete_feedback = delete_points_function_first_Y(orignal_chafen, orignal_chafen, orignal_chafen_capacity);
                //while (delete_points_flag != 2)
                //{
                //    for (int i = 0; i < 4; i++)
                //    {
                //        for (int j = 0; j < 10000; j++)
                //        {
                //            if (delete_feedback[i, j] != -10000000000000)
                //            {
                //                delete_points[i, j] = delete_feedback[i, j];
                //            }
                //            if (delete_feedback[i, j] == -10000000000000)
                //            {
                //                delete_points_capacity[i] = (long)delete_feedback[i, j + 1];
                //            }
                //        }
                //    }
                //    delete_feedback = delete_points_function_again_Y(orignal_chafen, delete_points, delete_points_capacity);
                //}
                //defect_Y = defect_recognition(curve_display_Y, original_num - 1);

                for (int i = 0; i < 4; i++)                                                        //求Z方向的原始信号的差分值
                {
                    orignal_chafen_capacity[i] = original_num - 1;
                    for (int j = 0; j < (orignal_chafen_capacity[i]); j++)
                    {
                        orignal_chafen[i, j] = ysdata[(2 + 3 * i), j + 1] - ysdata[(2 + 3 * i), j];
                    }
                }
                delete_feedback = delete_points_function_first_Z(orignal_chafen, orignal_chafen, orignal_chafen_capacity);
                while (delete_points_flag != int.Parse(cdtext.Text))
                {
                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 0; j < 10000; j++)
                        {
                            if (delete_feedback[i, j] == -10000000000000)
                            {
                                feedback_location = j;
                            }
                        }
                        for (int j = 0; j < feedback_location; j++)
                        {
                            delete_points[i, j] = delete_feedback[i, j];
                        }
                        delete_points_capacity[i] = (long)delete_feedback[i, feedback_location + 1];
                    }
                    delete_feedback = delete_points_function_again_Z(orignal_chafen, delete_points, delete_points_capacity);
                }
                defect_Z = defect_recognition(curve_display_Z, original_num - 1);

                pic_2_imagination = defect_recognition_XplusZ(defect_X, defect_Z, original_num - 1);
                //pic_2_imagination = defect_recognition_XplusZ(curve_display_X, curve_display_Z, original_num - 1);
                pic_2_imagination_another = defect_recognition_another(curve_display_X, curve_display_Z, original_num - 1);
                reconstruction_data_display = reconstruction(pic_2_imagination, pic_2_imagination_another, original_num - 1);
                while (defect_recognition_2_flag == 2)
                {
                    reconstruction_data_display = defect_recognition_2(reconstruction_data_display, original_num - 1);
                }
            }
            /******************************************************************************************************************************************************/

            try
            {

                tabControl.SelectedIndex = 1;
                Chanelcombo.Enabled = true;
                Channel.Enabled = true;
                //在下方设置后，点击数据处理就可以直接显示结果而不需要选择通道
                draw_yoriginalline2();
                draw_yoriginalline3();
                draw_yoriginalline4();
                draw_yoriginalline5();
                draw_yoriginalline7();
                draw_yoriginalline8();

                //Chanelcombo.Items.Clear();

                //for (int index = 1; index <= 6; index++)
                //{
                //    string chstr = "CH" + index.ToString();
                //    Chanelcombo.Items.Add(chstr);
                //}
                //Chanelcombo.SelectedIndex = 0;

                //    ///////////////////以下是不同的长度对应不同的模块

                //    ///五个通道合为一个
                //    //heyiqiuliangxu();
                //    Freaddata4 = heyiqiuliangxu(Freaddata2, original_num, 5);
                //    ///
                //    ////去噪
                //    //qiuyvzhi();
                //    yvzhi = qiuyvzhi(Freaddata4, original_num, 6);
                //    //changerorigail();
                //    Freaddata3 = changerorigail(Freaddata4, original_num, 6);
                //    Freaddata = quexianshibei(Freaddata3, (int)(original_num), 5);

                //    ////
                //    //////////////////以上是不同的长度对应不同的模块

                ////Fun.Function(Freaddata2, 6, original_num, ThickOftube);   //6是因为由五个合并的那个产生的
                //Freaddata = Fredapanduan(Freaddata, (int)(original_num), 5); //判断是否小于最小标准（便于以后成曲线）
                //Fun.Function(Freaddata, 5, original_num, ThickOftube);   //6是因为由五个合并的那个产生的
                final_imagination = pic_2_contour(original_num - 1);                  //2016.4.14改
                Fun.Function(final_imagination, 8, original_num - 1, ThickOftube);          //2016.4.14改
                cidao_1 = Fun.col_lip_1();        //计算等值线数据 

                draw_yoriginalline();   //画原始曲线  

                picContour();                 //画等值线图
                pointflag = false;   //置取点标志   
                runflag = true;
                linkButton.Enabled = true;
                if (linkflag) { startButton.Enabled = true; }   //如果连接了，开始按钮可用
                browseButton.Enabled = true;  //预览数据按钮可用                
                PointButton.Enabled = true;
                exitButton.Enabled = true;
            }
            catch
            {
                MessageBox.Show("数据错误！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                linkButton.Enabled = true;
                if (linkflag) { startButton.Enabled = true; }   //如果连接了，开始按钮可用
                browseButton.Enabled = true;  //预览数据按钮可用                
            }
        }

        //画实时曲线
        private void draw_yoriginalline()
        {
            //获取引用
            GraphPane myPane1 = zedGraphControl2.GraphPane;
            double x = 0;
            string CHname;

            //清空原图像
            myPane1.CurveList.Clear();
            myPane1.GraphObjList.Clear();
            zedGraphControl2.Refresh();

            //绘图留黑，有问题
            myPane1.XAxis.Scale.Min = 0;		//X轴最小值0
            myPane1.XAxis.Scale.Max = lenofpiece;	//X轴最大值   
            //myPane1.XAxis.Scale.MinorStep = 3;
            //myPane1.XAxis.Scale.MajorStep = 15;

            PointPairList mylist0 = new ZedGraph.PointPairList();
            ZedGraph.LineItem myCurve0;
            //PointPairList mylist1 = new ZedGraph.PointPairList();
            //ZedGraph.LineItem myCurve1;
            //PointPairList mylist2 = new ZedGraph.PointPairList();
            //ZedGraph.LineItem myCurve2;
            //PointPairList mylist3 = new ZedGraph.PointPairList();
            //ZedGraph.LineItem myCurve3;
            //PointPairList mylist4 = new ZedGraph.PointPairList();
            //ZedGraph.LineItem myCurve4;


            for (int i = 0; i < original_num - 1; i++)
            {
                x = (float)i * lenofpiece / (original_num - 1);
                //mylist0.Add(x, curve_display_X[Chanelcombo.SelectedIndex, i]);
                //mylist1.Add(x, curve_display_Y[Chanelcombo.SelectedIndex, i]);
                //mylist2.Add(x, curve_display_Z[Chanelcombo.SelectedIndex, i]);
                //mylist2.Add(x, defect_X[Chanelcombo.SelectedIndex, i]);
                //mylist3.Add(x, defect_Z[Chanelcombo.SelectedIndex, i]);
                //mylist0.Add(x, pic_2_imagination[Chanelcombo.SelectedIndex, i]);
                //mylist1.Add(x, pic_2_imagination_another[Chanelcombo.SelectedIndex, i]);
                //mylist0.Add(x, reconstruction_data[Chanelcombo.SelectedIndex, i]);

                mylist0.Add(x, reconstruction_data_display[i]);
                //mylist1.Add(x, Fun.y_lvbo[4, i]);
                //mylist0.Add(x, final_display[i]);


            }

            CHname = "Abnormal Signal";
            myCurve0 = zedGraphControl2.GraphPane.AddCurve(CHname, mylist0, Color.Red, ZedGraph.SymbolType.None);
            //CHname = "Vertical";
            //myCurve1 = zedGraphControl2.GraphPane.AddCurve(CHname, mylist1, Color.Blue, ZedGraph.SymbolType.None);
            //CHname = string.Format("{0}", 3);
            //myCurve2 = zedGraphControl2.GraphPane.AddCurve(CHname, mylist2, Color.Green, ZedGraph.SymbolType.None);
            //CHname = string.Format("{0}", 4);
            //myCurve3 = zedGraphControl2.GraphPane.AddCurve(CHname, mylist3, Color.Black, ZedGraph.SymbolType.None);
            //CHname = string.Format("{0}", 5);
            //myCurve4 = zedGraphControl2.GraphPane.AddCurve(CHname, mylist4, Color.Green, ZedGraph.SymbolType.None);

            myPane1.AxisChange();
            zedGraphControl2.Invalidate();

        }

        //画原始曲线1
        private void draw_yoriginalline2()
        {
            //获取引用
            GraphPane myPane2 = zedGraphControl3.GraphPane;
            double x = 0;
            string CHname;

            //清空原图像
            myPane2.CurveList.Clear();
            myPane2.GraphObjList.Clear();
            zedGraphControl3.Refresh();

            myPane2.XAxis.Scale.Min = 0;		//X轴最小值0
            myPane2.XAxis.Scale.Max = lenofpiece;	//X轴最大值   
            myPane2.XAxis.Scale.MinorStep = 50;
            myPane2.XAxis.Scale.MajorStep = 100;


            PointPairList nlist0 = new ZedGraph.PointPairList();
            ZedGraph.LineItem myCurve0;
            PointPairList nlist1 = new ZedGraph.PointPairList();
            ZedGraph.LineItem myCurve1;
            PointPairList nlist2 = new ZedGraph.PointPairList();
            ZedGraph.LineItem myCurve2;
            PointPairList nlist3 = new ZedGraph.PointPairList();
            ZedGraph.LineItem myCurve3;
            PointPairList nlist4 = new ZedGraph.PointPairList();
            ZedGraph.LineItem myCurve4;
            PointPairList nlist5 = new ZedGraph.PointPairList();
            ZedGraph.LineItem myCurve5;

            if (Channel.SelectedIndex == 0)
            {
                for (int i = 0; i < original_num; i++)
                {
                    //加减原始数据的差距使在一个位置
                    x = (float)i * lenofpiece / (original_num - 1);
                    nlist0.Add(x, original_signal[0, i] - 4244);
                    nlist1.Add(x, -original_signal[3, i] - 1396);
                    nlist2.Add(x, -original_signal[6, i]);
                    nlist3.Add(x, original_signal[9, i] + 13004);
                }
                CHname = string.Format("{0}", 1);
                myCurve0 = zedGraphControl3.GraphPane.AddCurve(CHname, nlist0, Color.Blue, ZedGraph.SymbolType.None);
                CHname = string.Format("{0}", 2);
                myCurve1 = zedGraphControl3.GraphPane.AddCurve(CHname, nlist1, Color.Red, ZedGraph.SymbolType.None);
                CHname = string.Format("{0}", 3);
                myCurve2 = zedGraphControl3.GraphPane.AddCurve(CHname, nlist2, Color.DarkGreen, ZedGraph.SymbolType.None);
                CHname = string.Format("{0}", 4);
                myCurve3 = zedGraphControl3.GraphPane.AddCurve(CHname, nlist3, Color.Pink, ZedGraph.SymbolType.None);
            }
            else
            {
                for (int i = 0; i < original_num; i++)
                {
                    x = (float)i * lenofpiece / (original_num - 1);
                    nlist0.Add(x, original_signal[4, i] - 1745);
                    nlist1.Add(x, original_signal[5, i] - 24017);
                    nlist2.Add(x, original_signal[1, i] + 500);
                    nlist3.Add(x, original_signal[7, i] - 4837);
                    nlist4.Add(x, original_signal[11, i] - 34146);
                    nlist5.Add(x, original_signal[10, i]);

                }

                CHname = string.Format("{0}", 1);
                myCurve0 = zedGraphControl3.GraphPane.AddCurve(CHname, nlist0, Color.Blue, ZedGraph.SymbolType.None);
                CHname = string.Format("{0}", 2);
                myCurve1 = zedGraphControl3.GraphPane.AddCurve(CHname, nlist1, Color.Red, ZedGraph.SymbolType.None);
                CHname = string.Format("{0}", 3);
                myCurve2 = zedGraphControl3.GraphPane.AddCurve(CHname, nlist2, Color.DarkGreen, ZedGraph.SymbolType.None);
                CHname = string.Format("{0}", 4);
                myCurve3 = zedGraphControl3.GraphPane.AddCurve(CHname, nlist3, Color.Pink, ZedGraph.SymbolType.None);
                CHname = string.Format("{0}", 5);
                myCurve4 = zedGraphControl3.GraphPane.AddCurve(CHname, nlist4, Color.Yellow, ZedGraph.SymbolType.None);
                CHname = string.Format("{0}", 6);
                myCurve5 = zedGraphControl3.GraphPane.AddCurve(CHname, nlist5, Color.LightBlue, ZedGraph.SymbolType.None);
            }
            myPane2.AxisChange();
            zedGraphControl3.Invalidate();
        }
        //画原始曲线2
        private void draw_yoriginalline3()
        {
            //获取引用
            GraphPane myPane3 = zedGraphControl4.GraphPane;
            double x = 0;
            string CHname;

            myPane3.YAxis.Title.Text = "磁场变化/nT";
            myPane3.XAxis.Title.Text = "距离/mm";
            //清空原图像
            myPane3.CurveList.Clear();
            myPane3.GraphObjList.Clear();
            zedGraphControl4.Refresh();

            myPane3.XAxis.Scale.Min = 0;		//X轴最小值0
            myPane3.XAxis.Scale.Max = lenofpiece;	//X轴最大值   
            myPane3.XAxis.Scale.MinorStep = 50;
            myPane3.XAxis.Scale.MajorStep = 100;

            PointPairList nlist0 = new ZedGraph.PointPairList();
            ZedGraph.LineItem myCurve0;

            for (int i = 0; i < original_num; i++)
            {
                x = (float)i * lenofpiece / (original_num - 1);
                nlist0.Add(x, original_signal[comboBox1.SelectedIndex, i]);
            }

            CHname = string.Format("{0}", 1);
            myCurve0 = zedGraphControl4.GraphPane.AddCurve(CHname, nlist0, Color.Red, ZedGraph.SymbolType.None);


            myPane3.AxisChange();
            zedGraphControl4.Invalidate();
        }
        //画12通道差分曲线
        private void draw_yoriginalline4()
        {
            //获取引用
            GraphPane myPane4 = zedGraphControl5.GraphPane;
            double x = 0;
            string CHname;

            myPane4.YAxis.Title.Text = "磁场变化/nT";
            myPane4.XAxis.Title.Text = "距离/mm";

            //清空原图像
            myPane4.CurveList.Clear();
            myPane4.GraphObjList.Clear();
            zedGraphControl5.Refresh();

            myPane4.XAxis.Scale.Min = 0;		//X轴最小值0
            myPane4.XAxis.Scale.Max = lenofpiece;	//X轴最大值   
            myPane4.XAxis.Scale.MinorStep = 50;
            myPane4.XAxis.Scale.MajorStep = 100;

            PointPairList nlist0 = new ZedGraph.PointPairList();
            ZedGraph.LineItem myCurve0;
            PointPairList nlist1 = new ZedGraph.PointPairList();
            ZedGraph.LineItem myCurve1;
            PointPairList nlist2 = new ZedGraph.PointPairList();
            ZedGraph.LineItem myCurve2;
            PointPairList nlist3 = new ZedGraph.PointPairList();
            ZedGraph.LineItem myCurve3;
            PointPairList nlist4 = new ZedGraph.PointPairList();
            ZedGraph.LineItem myCurve4;
            PointPairList nlist5 = new ZedGraph.PointPairList();
            ZedGraph.LineItem myCurve5;
            PointPairList nlist6 = new ZedGraph.PointPairList();
            ZedGraph.LineItem myCurve6;
            PointPairList nlist7 = new ZedGraph.PointPairList();
            ZedGraph.LineItem myCurve7;
            PointPairList nlist8 = new ZedGraph.PointPairList();
            ZedGraph.LineItem myCurve8;
            PointPairList nlist9 = new ZedGraph.PointPairList();
            ZedGraph.LineItem myCurve9;
            PointPairList nlist10 = new ZedGraph.PointPairList();
            ZedGraph.LineItem myCurve10;
            PointPairList nlist11 = new ZedGraph.PointPairList();
            ZedGraph.LineItem myCurve11;
            PointPairList nlist12 = new ZedGraph.PointPairList();
            ZedGraph.LineItem myCurve12;
            PointPairList nlist13 = new ZedGraph.PointPairList();
            ZedGraph.LineItem myCurve13;
            PointPairList nlist14 = new ZedGraph.PointPairList();
            ZedGraph.LineItem myCurve14;
            PointPairList nlist15 = new ZedGraph.PointPairList();
            ZedGraph.LineItem myCurve15;
            PointPairList nlist16 = new ZedGraph.PointPairList();
            ZedGraph.LineItem myCurve16;
            PointPairList nlist17 = new ZedGraph.PointPairList();
            ZedGraph.LineItem myCurve17;



            //for (int i = 0; i < 12; i++)
            //{
            //    for (int j = 0; j < original_num - 1; j++)
            //    {
            //        original_signal_chafen[i, j] = original_signal[i,j+1] - original_signal[i, j];
            //    }
            //}

            for (int j = 0; j < original_num - 1; j++)
            {
                original_signal_chafen2[0, j] = original_signal[0, j + 1] - original_signal[0, j];
                original_signal_chafen2[1, j] = original_signal[1, j + 1] - original_signal[1, j];
                original_signal_chafen2[2, j] = original_signal[2, j + 1] - original_signal[2, j];
                original_signal_chafen2[3, j] = -1 * (original_signal[3, j + 1] - original_signal[3, j]);
                original_signal_chafen2[4, j] = (original_signal[4, j + 1] - original_signal[4, j]);
                original_signal_chafen2[5, j] = original_signal[5, j + 1] - original_signal[5, j];
                original_signal_chafen2[6, j] = -1 * (original_signal[6, j + 1] - original_signal[6, j]);
                original_signal_chafen2[7, j] = (original_signal[7, j + 1] - original_signal[7, j]);
                original_signal_chafen2[8, j] = original_signal[8, j + 1] - original_signal[8, j];
                original_signal_chafen2[9, j] = original_signal[9, j + 1] - original_signal[9, j];
                original_signal_chafen2[10, j] = original_signal[10, j + 1] - original_signal[10, j];
                original_signal_chafen2[11, j] = original_signal[11, j + 1] - original_signal[11, j];
            }
            /////////////////////////////////////////////求阈值线/////////////////////////////////////
            for (int i = 0; i < 12; i++)
            {
                for (int j = 0; j < original_num - 1; j++)
                {
                    original_signal_chafen2_sum[i] = original_signal_chafen2_sum[i] + original_signal_chafen2[i, j];
                }
                original_signal_chafen2_mean[i] = original_signal_chafen2_sum[i] / (original_num - 1);
                original_signal_chafen2_sum[i] = 0;
            }



            for (int i = 0; i < 12; i++)
            {
                for (int j = 0; j < original_num - 1; j++)
                {
                    original_signal_chafen2_biaozhunchasum[i] = original_signal_chafen2_biaozhunchasum[i] + Math.Pow((original_signal_chafen2[i, j] - original_signal_chafen2_mean[i]), 2);
                }
                original_signal_chafen2_sigma[i] = Math.Sqrt(original_signal_chafen2_biaozhunchasum[i] / original_num - 1);
                original_signal_chafen2_biaozhunchasum[i] = 0;

            }
            for (int i = 0; i < 12; i++)
            {
                original_signal_chafen2_max[i] = original_signal_chafen2_mean[i] + double.Parse(yztext.Text) * original_signal_chafen2_sigma[i];          //求出阈值线的最大值
                original_signal_chafen2_min[i] = original_signal_chafen2_mean[i] - double.Parse(yztext.Text) * original_signal_chafen2_sigma[i];          //求出阈值线的最小值
            }

            ////////////////////////////////////////////////////////////////////////////////////////////////




            if (comboBox2.SelectedIndex == 12)
            {
                for (int i = 0; i < original_num - 1; i++)
                {
                    //加减原始数据的差距使在一个位置
                    x = (float)i * lenofpiece / (original_num - 1);
                    nlist0.Add(x, original_signal_chafen2[0, i]);
                    nlist1.Add(x, original_signal_chafen2[3, i]);
                    nlist2.Add(x, original_signal_chafen2[6, i]);
                    nlist3.Add(x, original_signal_chafen2[9, i]);

                    nlist4.Add(x, original_signal_chafen2_max[0]);
                    nlist5.Add(x, original_signal_chafen2_max[3]);
                    nlist6.Add(x, original_signal_chafen2_max[6]);
                    nlist7.Add(x, original_signal_chafen2_max[9]);
                    nlist8.Add(x, original_signal_chafen2_min[0]);
                    nlist9.Add(x, original_signal_chafen2_min[3]);
                    nlist10.Add(x, original_signal_chafen2_min[6]);
                    nlist11.Add(x, original_signal_chafen2_min[9]);
                }
                CHname = string.Format("{0}", 1);
                myCurve0 = zedGraphControl5.GraphPane.AddCurve(CHname, nlist0, Color.Blue, ZedGraph.SymbolType.None);
                CHname = string.Format("{0}", 2);
                myCurve1 = zedGraphControl5.GraphPane.AddCurve(CHname, nlist1, Color.Red, ZedGraph.SymbolType.None);
                CHname = string.Format("{0}", 3);
                myCurve2 = zedGraphControl5.GraphPane.AddCurve(CHname, nlist2, Color.DarkGreen, ZedGraph.SymbolType.None);
                CHname = string.Format("{0}", 4);
                myCurve3 = zedGraphControl5.GraphPane.AddCurve(CHname, nlist3, Color.Brown, ZedGraph.SymbolType.None);

                CHname = string.Format("{0}", 5);
                myCurve3 = zedGraphControl5.GraphPane.AddCurve(CHname, nlist4, Color.Black, ZedGraph.SymbolType.None);
                CHname = string.Format("{0}", 6);
                myCurve3 = zedGraphControl5.GraphPane.AddCurve(CHname, nlist5, Color.Black, ZedGraph.SymbolType.None);
                CHname = string.Format("{0}", 7);
                myCurve3 = zedGraphControl5.GraphPane.AddCurve(CHname, nlist6, Color.Black, ZedGraph.SymbolType.None);
                CHname = string.Format("{0}", 8);
                myCurve3 = zedGraphControl5.GraphPane.AddCurve(CHname, nlist7, Color.Black, ZedGraph.SymbolType.None);
                CHname = string.Format("{0}", 9);
                myCurve3 = zedGraphControl5.GraphPane.AddCurve(CHname, nlist8, Color.Black, ZedGraph.SymbolType.None);
                CHname = string.Format("{0}", 10);
                myCurve3 = zedGraphControl5.GraphPane.AddCurve(CHname, nlist9, Color.Black, ZedGraph.SymbolType.None);
                CHname = string.Format("{0}", 11);
                myCurve3 = zedGraphControl5.GraphPane.AddCurve(CHname, nlist10, Color.Black, ZedGraph.SymbolType.None);
                CHname = string.Format("{0}", 12);
                myCurve3 = zedGraphControl5.GraphPane.AddCurve(CHname, nlist11, Color.Black, ZedGraph.SymbolType.None);
            }
            if (comboBox2.SelectedIndex == 13)
            {
                for (int i = 0; i < original_num - 1; i++)
                {

                    x = (float)i * lenofpiece / (original_num - 1);
                    nlist0.Add(x, original_signal_chafen2[4, i]);
                    nlist1.Add(x, original_signal_chafen2[5, i]);
                    nlist2.Add(x, original_signal_chafen2[1, i]);
                    nlist3.Add(x, original_signal_chafen2[7, i]);
                    nlist4.Add(x, original_signal_chafen2[11, i]);
                    nlist5.Add(x, original_signal_chafen2[10, i]);

                    nlist6.Add(x, original_signal_chafen2_max[4]);
                    nlist7.Add(x, original_signal_chafen2_max[5]);
                    nlist8.Add(x, original_signal_chafen2_max[1]);
                    nlist9.Add(x, original_signal_chafen2_max[7]);
                    nlist10.Add(x, original_signal_chafen2_max[11]);
                    nlist11.Add(x, original_signal_chafen2_max[10]);
                    nlist12.Add(x, original_signal_chafen2_min[4]);
                    nlist13.Add(x, original_signal_chafen2_min[5]);
                    nlist14.Add(x, original_signal_chafen2_min[1]);
                    nlist15.Add(x, original_signal_chafen2_min[7]);
                    nlist16.Add(x, original_signal_chafen2_min[11]);
                    nlist17.Add(x, original_signal_chafen2_min[10]);
                }
                CHname = string.Format("{0}", 1);
                myCurve0 = zedGraphControl5.GraphPane.AddCurve(CHname, nlist0, Color.Blue, ZedGraph.SymbolType.None);
                CHname = string.Format("{0}", 2);
                myCurve1 = zedGraphControl5.GraphPane.AddCurve(CHname, nlist1, Color.Red, ZedGraph.SymbolType.None);
                CHname = string.Format("{0}", 3);
                myCurve2 = zedGraphControl5.GraphPane.AddCurve(CHname, nlist2, Color.DarkGreen, ZedGraph.SymbolType.None);
                CHname = string.Format("{0}", 4);
                myCurve3 = zedGraphControl5.GraphPane.AddCurve(CHname, nlist3, Color.Pink, ZedGraph.SymbolType.None);
                CHname = string.Format("{0}", 5);
                myCurve4 = zedGraphControl5.GraphPane.AddCurve(CHname, nlist4, Color.Green, ZedGraph.SymbolType.None);
                CHname = string.Format("{0}", 6);
                myCurve5 = zedGraphControl5.GraphPane.AddCurve(CHname, nlist5, Color.Orange, ZedGraph.SymbolType.None);

                CHname = string.Format("{0}", 7);
                myCurve5 = zedGraphControl5.GraphPane.AddCurve(CHname, nlist6, Color.Black, ZedGraph.SymbolType.None);
                CHname = string.Format("{0}", 8);
                myCurve5 = zedGraphControl5.GraphPane.AddCurve(CHname, nlist7, Color.Black, ZedGraph.SymbolType.None);
                CHname = string.Format("{0}", 9);
                myCurve5 = zedGraphControl5.GraphPane.AddCurve(CHname, nlist8, Color.Black, ZedGraph.SymbolType.None);
                CHname = string.Format("{0}", 10);
                myCurve5 = zedGraphControl5.GraphPane.AddCurve(CHname, nlist9, Color.Black, ZedGraph.SymbolType.None);
                CHname = string.Format("{0}", 11);
                myCurve5 = zedGraphControl5.GraphPane.AddCurve(CHname, nlist10, Color.Black, ZedGraph.SymbolType.None);
                CHname = string.Format("{0}", 12);
                myCurve5 = zedGraphControl5.GraphPane.AddCurve(CHname, nlist11, Color.Black, ZedGraph.SymbolType.None);
                CHname = string.Format("{0}", 13);
                myCurve5 = zedGraphControl5.GraphPane.AddCurve(CHname, nlist12, Color.Black, ZedGraph.SymbolType.None);
                CHname = string.Format("{0}", 14);
                myCurve5 = zedGraphControl5.GraphPane.AddCurve(CHname, nlist13, Color.Black, ZedGraph.SymbolType.None);
                CHname = string.Format("{0}", 15);
                myCurve5 = zedGraphControl5.GraphPane.AddCurve(CHname, nlist14, Color.Black, ZedGraph.SymbolType.None);
                CHname = string.Format("{0}", 16);
                myCurve5 = zedGraphControl5.GraphPane.AddCurve(CHname, nlist15, Color.Black, ZedGraph.SymbolType.None);
                CHname = string.Format("{0}", 17);
                myCurve5 = zedGraphControl5.GraphPane.AddCurve(CHname, nlist16, Color.Black, ZedGraph.SymbolType.None);
                CHname = string.Format("{0}", 18);
                myCurve5 = zedGraphControl5.GraphPane.AddCurve(CHname, nlist17, Color.Black, ZedGraph.SymbolType.None);
            }
            if (comboBox2.SelectedIndex == 14)
            {
                for (int i = 0; i < original_num - 1; i++)
                {

                    x = (float)i * lenofpiece / (original_num - 1);
                    nlist0.Add(x, Math.Abs(original_signal_chafen2[2, i]));
                    nlist1.Add(x, Math.Abs(original_signal_chafen2[5, i]));
                    nlist2.Add(x, Math.Abs(original_signal_chafen2[8, i]));
                    nlist3.Add(x, Math.Abs(original_signal_chafen2[11, i]));


                    nlist4.Add(x, original_signal_chafen2_max[2]);
                    nlist5.Add(x, original_signal_chafen2_max[5]);
                    nlist6.Add(x, original_signal_chafen2_max[8]);
                    nlist7.Add(x, original_signal_chafen2_max[11]);

                    nlist8.Add(x, original_signal_chafen2_min[2]);
                    nlist9.Add(x, original_signal_chafen2_min[5]);
                    nlist10.Add(x, original_signal_chafen2_min[8]);
                    nlist11.Add(x, original_signal_chafen2_min[11]);



                }
                CHname = string.Format("{0}", 1);
                myCurve0 = zedGraphControl5.GraphPane.AddCurve(CHname, nlist0, Color.Blue, ZedGraph.SymbolType.None);
                CHname = string.Format("{0}", 2);
                myCurve1 = zedGraphControl5.GraphPane.AddCurve(CHname, nlist1, Color.Red, ZedGraph.SymbolType.None);
                CHname = string.Format("{0}", 3);
                myCurve2 = zedGraphControl5.GraphPane.AddCurve(CHname, nlist2, Color.DarkGreen, ZedGraph.SymbolType.None);
                CHname = string.Format("{0}", 4);
                myCurve3 = zedGraphControl5.GraphPane.AddCurve(CHname, nlist3, Color.Pink, ZedGraph.SymbolType.None);


                CHname = string.Format("{0}", 7);
                myCurve5 = zedGraphControl5.GraphPane.AddCurve(CHname, nlist4, Color.Black, ZedGraph.SymbolType.None);
                CHname = string.Format("{0}", 8);
                myCurve5 = zedGraphControl5.GraphPane.AddCurve(CHname, nlist5, Color.Black, ZedGraph.SymbolType.None);
                CHname = string.Format("{0}", 9);
                myCurve5 = zedGraphControl5.GraphPane.AddCurve(CHname, nlist6, Color.Black, ZedGraph.SymbolType.None);
                CHname = string.Format("{0}", 10);
                myCurve5 = zedGraphControl5.GraphPane.AddCurve(CHname, nlist7, Color.Black, ZedGraph.SymbolType.None);
                CHname = string.Format("{0}", 11);
                myCurve5 = zedGraphControl5.GraphPane.AddCurve(CHname, nlist8, Color.Black, ZedGraph.SymbolType.None);
                CHname = string.Format("{0}", 12);
                myCurve5 = zedGraphControl5.GraphPane.AddCurve(CHname, nlist9, Color.Black, ZedGraph.SymbolType.None);
                CHname = string.Format("{0}", 13);
                myCurve5 = zedGraphControl5.GraphPane.AddCurve(CHname, nlist10, Color.Black, ZedGraph.SymbolType.None);
                CHname = string.Format("{0}", 14);
                myCurve5 = zedGraphControl5.GraphPane.AddCurve(CHname, nlist11, Color.Black, ZedGraph.SymbolType.None);
            }
            if (comboBox2.SelectedIndex != 12 && comboBox2.SelectedIndex != 13)
            {
                for (int i = 0; i < original_num - 1; i++)
                {
                    //加减原始数据的差距使在一个位置
                    x = (float)i * lenofpiece / (original_num - 1);
                    nlist0.Add(x, Math.Abs(original_signal_chafen2[1, i]));
                    nlist1.Add(x, Math.Abs(original_signal_chafen2[4, i]));
                    nlist2.Add(x, Math.Abs(original_signal_chafen2[7, i]));
                    nlist3.Add(x, Math.Abs(original_signal_chafen2[10, i]));

                    nlist4.Add(x, original_signal_chafen2_max[1]);
                    nlist5.Add(x, original_signal_chafen2_max[4]);
                    nlist6.Add(x, original_signal_chafen2_max[7]);
                    nlist7.Add(x, original_signal_chafen2_max[10]);
                    nlist8.Add(x, original_signal_chafen2_min[1]);
                    nlist9.Add(x, original_signal_chafen2_min[4]);
                    nlist10.Add(x, original_signal_chafen2_min[7]);
                    nlist11.Add(x, original_signal_chafen2_min[10]);
                }

                CHname = string.Format("{0}", 1);
                myCurve0 = zedGraphControl5.GraphPane.AddCurve(CHname, nlist0, Color.Red, ZedGraph.SymbolType.None);
                CHname = string.Format("{0}", 2);
                myCurve1 = zedGraphControl5.GraphPane.AddCurve(CHname, nlist1, Color.Black, ZedGraph.SymbolType.None);
                CHname = string.Format("{0}", 3);
                myCurve2 = zedGraphControl5.GraphPane.AddCurve(CHname, nlist2, Color.Black, ZedGraph.SymbolType.None);
            }
            myPane4.AxisChange();
            zedGraphControl5.Invalidate();
        }
        //画独立分量
        private void draw_yoriginalline5()
        {
            //获取引用
            GraphPane myPane3 = zedGraphControl3_2.GraphPane;
            double x = 0;
            string CHname;

            //清空原图像
            myPane3.CurveList.Clear();
            myPane3.GraphObjList.Clear();
            zedGraphControl3_2.Refresh();

            myPane3.XAxis.Scale.Min = 0;		//X轴最小值0
            myPane3.XAxis.Scale.Max = lenofpiece;	//X轴最大值   
            myPane3.XAxis.Scale.MinorStep = 50;
            myPane3.XAxis.Scale.MajorStep = 100;

            myPane3.Title.Text = "";

            PointPairList nlist0 = new ZedGraph.PointPairList();
            ZedGraph.LineItem myCurve0;
            PointPairList nlist1 = new ZedGraph.PointPairList();
            ZedGraph.LineItem myCurve1;
            PointPairList nlist2 = new ZedGraph.PointPairList();
            ZedGraph.LineItem myCurve2;
            PointPairList nlist3 = new ZedGraph.PointPairList();
            ZedGraph.LineItem myCurve3;
            PointPairList nlist4 = new ZedGraph.PointPairList();
            ZedGraph.LineItem myCurve4;
            /////////////////////////////////////////////求阈值线/////////////////////////////////////
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < original_num - 1; j++)
                {
                    Dydata_sum[i] = Dydata_sum[i] + Dydata[i, j];
                }
                Dydata_mean[i] = Dydata_sum[i] / (original_num - 1);
                Dydata_sum[i] = 0;
            }



            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < original_num - 1; j++)
                {
                    Dydata_biaozhunchasum[i] = Dydata_biaozhunchasum[i] + Math.Pow((Dydata[i, j] - Dydata_mean[i]), 2);
                }
                Dydata_sigma[i] = Math.Sqrt(Dydata_biaozhunchasum[i] / original_num - 1);
                Dydata_biaozhunchasum[i] = 0;

            }
            for (int i = 0; i < 5; i++)
            {
                Dydata_max[i] = Dydata_mean[i] + double.Parse(yztext.Text) * Dydata_sigma[i];          //求出阈值线的最大值
                Dydata_min[i] = Dydata_mean[i] - double.Parse(yztext.Text) * Dydata_sigma[i];          //求出阈值线的最小值
            }

            ////////////////////////////////////////////////////////////////////////////////////////////////

            if (fenliang_chanel.SelectedIndex == 0)
            {
                for (int i = 0; i < original_num; i++)
                {
                    x = (float)i * lenofpiece / (original_num - 1);
                    nlist0.Add(x, Dydata[0, i]);
                    nlist1.Add(x, Dydata[1, i]);
                    nlist2.Add(x, Dydata[2, i]);
                    nlist3.Add(x, Dydata[3, i]);
                    nlist4.Add(x, Dydata[4, i]);
                }

                CHname = string.Format("{0}", 1);
                myCurve0 = zedGraphControl3_2.GraphPane.AddCurve(CHname, nlist0, Color.Blue, ZedGraph.SymbolType.None);
                CHname = string.Format("{0}", 2);
                myCurve1 = zedGraphControl3_2.GraphPane.AddCurve(CHname, nlist1, Color.Red, ZedGraph.SymbolType.None);
                CHname = string.Format("{0}", 3);
                myCurve2 = zedGraphControl3_2.GraphPane.AddCurve(CHname, nlist2, Color.Brown, ZedGraph.SymbolType.None);
                CHname = string.Format("{0}", 4);
                myCurve3 = zedGraphControl3_2.GraphPane.AddCurve(CHname, nlist3, Color.DarkGreen, ZedGraph.SymbolType.None);
                CHname = string.Format("{0}", 5);
                myCurve4 = zedGraphControl3_2.GraphPane.AddCurve(CHname, nlist4, Color.Black, ZedGraph.SymbolType.None);

                myPane3.AxisChange();
                zedGraphControl3_2.Invalidate();
            }
            else
            {
                for (int i = 0; i < original_num; i++)
                {
                    x = (float)i * lenofpiece / (original_num - 1);
                    nlist0.Add(x, Dydata[fenliang_chanel.SelectedIndex - 1, i]);

                    if (yuzhi_chanel.SelectedIndex == 1)
                    {
                        nlist1.Add(x, Dydata_max[fenliang_chanel.SelectedIndex - 1]);
                        nlist2.Add(x, Dydata_min[fenliang_chanel.SelectedIndex - 1]);
                    }
                }
                CHname = string.Format("{0}", 1);
                myCurve0 = zedGraphControl3_2.GraphPane.AddCurve(CHname, nlist0, Color.Blue, ZedGraph.SymbolType.None);
                if (yuzhi_chanel.SelectedIndex == 1)
                {
                    myCurve1 = zedGraphControl3_2.GraphPane.AddCurve(CHname, nlist1, Color.Black, ZedGraph.SymbolType.None);
                    myCurve2 = zedGraphControl3_2.GraphPane.AddCurve(CHname, nlist2, Color.Black, ZedGraph.SymbolType.None);
                }
                myPane3.AxisChange();
                zedGraphControl3_2.Invalidate();
            }
        }
        //画张量信号
        private void draw_yoriginalline7()
        {
            //获取引用

            double x = 0;
            string CHname;

            //清空原图像



            PointPairList nlist0 = new ZedGraph.PointPairList();
            ZedGraph.LineItem myCurve0;


            for (int j = 0; j < original_num - 1; j++)
            {
                dulifenliang[0, j] = -(ysdata[6, j] + ysdata[0, j]);
                dulifenliang[1, j] = -(ysdata[7, j] - ysdata[1, j]);
                dulifenliang[2, j] = ysdata[8, j] - ysdata[2, j];
                dulifenliang[3, j] = ysdata[10, j] + ysdata[4, j];
                dulifenliang[4, j] = ysdata[11, j] - ysdata[5, j];


                //dulifenliang[1, j] = Dydata[1, j];
                //dulifenliang[1, j] = Dydata[1, j];
                //dulifenliang[1, j] = Dydata[1, j];
                //dulifenliang[1, j] = Dydata[1, j];


            }




            for (int i = 0; i < original_num - 2; i++)
            {
                //x = (float)i * lenofpiece / (original_num - 1);
                //nlist0.Add(x, Dydata[comboBox7.SelectedIndex, i]);

                x = (float)i * lenofpiece / (original_num - 2);


            }

            CHname = string.Format("{0}", 1);

        }
        //画张量差分信号
        private void draw_yoriginalline8()
        {
            //获取引用

            double x = 0;
            string CHname;

            //清空原图像


            PointPairList nlist0 = new ZedGraph.PointPairList();
            ZedGraph.LineItem myCurve0;


            for (int j = 0; j < original_num - 1; j++)
            {
                dulifenliang[0, j] = -(ysdata[6, j] + ysdata[0, j]);
                dulifenliang[1, j] = -(ysdata[7, j] - ysdata[1, j]);
                dulifenliang[2, j] = ysdata[8, j] - ysdata[2, j];
                dulifenliang[3, j] = ysdata[10, j] + ysdata[4, j];
                dulifenliang[4, j] = ysdata[11, j] - ysdata[5, j];



                //dulifenliang[1, j] = Dydata[1, j];
                //dulifenliang[1, j] = Dydata[1, j];
                //dulifenliang[1, j] = Dydata[1, j];
                //dulifenliang[1, j] = Dydata[1, j];


            }

            for (int j = 0; j < original_num - 1; j++)
            {
                chafendulifenliang[0, j] = dulifenliang[0, j + 1] - dulifenliang[0, j];
                chafendulifenliang[1, j] = dulifenliang[1, j + 1] - dulifenliang[1, j];
                chafendulifenliang[2, j] = dulifenliang[2, j + 1] - dulifenliang[2, j];
                chafendulifenliang[3, j] = dulifenliang[3, j + 1] - dulifenliang[3, j];
                chafendulifenliang[4, j] = dulifenliang[4, j + 1] - dulifenliang[4, j];
            }



            for (int i = 0; i < original_num - 2; i++)
            {
                //x = (float)i * lenofpiece / (original_num - 1);
                //nlist0.Add(x, Dydata[comboBox7.SelectedIndex, i]);

                x = (float)i * lenofpiece / (original_num - 2);


            }

            CHname = string.Format("{0}", 1);

        }
        //选择显示曲线的通道
        //需要修改Combo属性内的事件才能正确运行//
        private void Chanelcombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            draw_yoriginalline();
        }
        private void Channel_SelectedIndexChanged(object sender, EventArgs e)
        {
            draw_yoriginalline2();
        }
        private void fenliang_chanel_SelectedIndexChanged(object sender, EventArgs e)
        {
            draw_yoriginalline5();
        }
        private void yuzhi_chanel_SelectedIndexChanged(object sender, EventArgs e)
        {
            draw_yoriginalline5();
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            draw_yoriginalline3();
        }
        private void comboBox2_SelectedIndexCHanged(object sender, EventArgs e)
        {
            draw_yoriginalline4();
        }

        //zedgraph显示坐标
        private string MyPointValueHandler(ZedGraphControl control, GraphPane pane, CurveItem curve, int iPt)
        {
            PointPair pt = curve[iPt];
            return "横坐标:" + string.Format("{0:0}", pt.X) + " 纵坐标:" + string.Format("{0:0.0}", pt.Y);
        }

        //调用使zedgraph显示坐标（增加的鼠标显示坐标均在下方添加）
        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            zedGraphControl1.IsShowPointValues = true;  //动态磁场
            zedGraphControl2.IsShowPointValues = true;  //实时曲线
            zedGraphControl3.IsShowPointValues = true;  //实时曲线
            zedGraphControl4.IsShowPointValues = true;
            zedGraphControl5.IsShowPointValues = true;

            zedGraphControl1.PointValueEvent += new ZedGraphControl.PointValueHandler(MyPointValueHandler);
            zedGraphControl2.PointValueEvent += new ZedGraphControl.PointValueHandler(MyPointValueHandler);
            zedGraphControl3.PointValueEvent += new ZedGraphControl.PointValueHandler(MyPointValueHandler);
            zedGraphControl4.PointValueEvent += new ZedGraphControl.PointValueHandler(MyPointValueHandler);
            zedGraphControl5.PointValueEvent += new ZedGraphControl.PointValueHandler(MyPointValueHandler);
        }

        //显示时间
        private void timer_Tick(object sender, EventArgs e)
        {
            TimeLabel.Text = DateTime.Now.ToString();
        }

        //预览数据按钮
        private void browseButton_Click(object sender, EventArgs e)
        {
            //dog.isMyDog();//找狗
            try
            {
                using (OpenFileDialog open = new OpenFileDialog())
                {
                    open.Filter = "文本文件 (*.txt)|*.txt";
                    open.FilterIndex = 1;
                    open.RestoreDirectory = true;
                    open.Title = "打开数据文件";
                    if (open.ShowDialog() == DialogResult.OK)
                    {
                        FilePath = open.FileName;
                        FileText.Text = FilePath;
                        readdata();
                        runButton.Enabled = true;
                        PointButton.Enabled = false;
                        browseflag = true;
                        runflag = false;
                    }
                }
            }
            catch
            {
                MessageBox.Show("数据错误！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        //读磁场数据
        private void readdata()
        {
            System.IO.StreamReader sr_ScatterData = new System.IO.StreamReader(FilePath);
            string temp_str = sr_ScatterData.ReadLine();  //第一行 工件号
            temp_str.TrimStart();
            temp_str.TrimEnd();
            string[] tempData_str = temp_str.Split(' ').ToArray();
            NoText.Text = tempData_str[1];

            readflag = true;

            sr_ScatterData.ReadLine();   //第三行 检测时间
            temp_str = sr_ScatterData.ReadLine();  //第四行 规格和通道数
            temp_str.TrimStart();
            temp_str.TrimEnd();
            tempData_str = temp_str.Split(' ').ToArray();

            DisText.Text = tempData_str[1];
            lenofpiece1 = double.Parse(tempData_str[1]);
            lenofpiece = lenofpiece1 - kuangjiachangdu;

            DText.Text = tempData_str[3];
            TText.Text = tempData_str[5];
            TLBox.Text = tempData_str[7];
            cdtext.Text = tempData_str[9];
            qccdtext.Text = tempData_str[11];

            widthOfPiece = Math.PI * double.Parse(tempData_str[3]);
            ThickOftube = double.Parse(tempData_str[5]);
            tongdao_num = 12;
            tiligaodu = double.Parse(tempData_str[7]);
            long k = 0;
            while (!sr_ScatterData.EndOfStream)
            {
                temp_str = sr_ScatterData.ReadLine();
                temp_str.TrimStart();
                temp_str.TrimEnd();
                tempData_str = temp_str.Split(',').ToArray();

                if (tempData_str.Count() == 18)
                {
                    Dydata[0, k] = double.Parse(tempData_str[1]);
                    Dydata[1, k] = double.Parse(tempData_str[2]);
                    Dydata[2, k] = double.Parse(tempData_str[3]);
                    Dydata[3, k] = double.Parse(tempData_str[4]);
                    Dydata[4, k] = double.Parse(tempData_str[5]);


                    //Freaddata2[2, k] = double.Parse(tempData_str[1]);   //通道改变
                    //Freaddata2[1, k] = double.Parse(tempData_str[2]);
                    //Freaddata2[3, k] = double.Parse(tempData_str[3]);
                    //Freaddata2[4, k] = double.Parse(tempData_str[4]);
                    //Freaddata2[0, k] = double.Parse(tempData_str[5]);


                    Freaddata2[0, k] = double.Parse(tempData_str[1]);   //通道改变
                    Freaddata2[1, k] = double.Parse(tempData_str[2]);
                    Freaddata2[2, k] = double.Parse(tempData_str[3]);
                    Freaddata2[3, k] = double.Parse(tempData_str[4]);
                    Freaddata2[4, k] = double.Parse(tempData_str[5]);






                    ysdata[0, k] = double.Parse(tempData_str[6]);
                    ysdata[1, k] = double.Parse(tempData_str[7]);
                    ysdata[2, k] = double.Parse(tempData_str[8]);
                    ysdata[3, k] = double.Parse(tempData_str[9]);
                    ysdata[4, k] = double.Parse(tempData_str[10]);
                    ysdata[5, k] = double.Parse(tempData_str[11]);
                    ysdata[6, k] = double.Parse(tempData_str[12]);
                    ysdata[7, k] = double.Parse(tempData_str[13]);
                    ysdata[8, k] = double.Parse(tempData_str[14]);
                    ysdata[9, k] = double.Parse(tempData_str[15]);
                    ysdata[10, k] = double.Parse(tempData_str[16]);
                    ysdata[11, k] = double.Parse(tempData_str[17]);

                    mean = (ysdata[2, k] + ysdata[5, k] + ysdata[8, k] + ysdata[11, k]) / 4;

                    ysdatanew[0, k] = ysdata[2, k] - mean;
                    ysdatanew[1, k] = ysdata[5, k] - mean;
                    ysdatanew[2, k] = ysdata[8, k] - mean;
                    ysdatanew[3, k] = ysdata[11, k] - mean;

                    k++;
                }

                //if (tempData_str.Count() == 2)
                //{
                //    DisText.Text = tempData_str[1];


                //} 
            }
            original_num = k;
            sr_ScatterData.Close();
        }

        /********************************************************************** 2016.4.10 添加加步行缺陷提取算法*************************************************************/
        private double[,] delete_points_function_first_X(double[,] chafen_orignal, double[,] chafen, long[] num)//对差分完的X方向信号进行第一次三西格玛删点计算
        {
            //double[,] chafen = new double[4, 10000];         //原始信号的差分值
            double[] somechannel_mean = new double[4];        //某个通道差分值的平均值
            double[] somechannel_biaozhuncha = new double[4];   //某个通道差分值的标准差
            double[] somechannel_max = new double[4];        //某个通道差分值的阈值（最大值）
            double[] somechannel_min = new double[4];        //某个通道差分值的阈值（最小值）
            double somechannel_sum = 0;              //求某个通道差分值的均值时涉及到的求和变量
            double somechannel_biaozhuncha_sum = 0;       //求某个通道差分值的标准差时涉及到的求和变量
            double[,] point_jizhi_value = new double[4, 10000];      //存放某个通道差分值的极大值点
            //double[,] point_jixiaozhi_value=new double[4,10000];    //存放某个通道差分值的极小值点
            long[,] point_jizhi_location = new long[4, 10000];         //存放某个通道差分值的极大值点的位置
            //long[,] point_jixiaozhi_location=new long[4,10000];       //存放某个通道差分值的极小值点的位置
            int[] point_jizhi_capacity = new int[4];
            //int[] point_jixiaozhi_capacity = new int[4];
            long[,] chafen_location = new long[4, 10000];
            //int[] chafen_location_capacity = new int[4];
            double[,] chafen_reconstruct = new double[4, 10000];
            int[] chafen_reconstruct_capacity = new int[4];
            //double[,] curve_display = new double[4, 10000];     //最终用于曲线成像的值
            //double[,] curve_display_location = new double[4, 10000];

            //for (int i = 0; i < 4; i++)
            //{
            //    for (int j = 0; j < num[i]; j++)
            //    {
            //        curve_display[i,j]=chafen_orignal[i,j];  
            //    }
            //}

            delete_points_flag = 1;      //delete_points_flag为删点标志位，第一次如果有超过阈值的极值点就将该位置为1，标志位的意义是为了删点循环作为判断条件

            //求均值
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < num[i]; j++)
                {
                    somechannel_sum = somechannel_sum + chafen[i, j];
                }
                somechannel_mean[i] = somechannel_sum / num[i];            //求某个测磁方向4个通道分别差分后的均值
                somechannel_sum = 0;
            }
            //求标准差
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < num[i]; j++)
                {
                    somechannel_biaozhuncha_sum = somechannel_biaozhuncha_sum + Math.Pow((chafen[i, j] - somechannel_mean[i]), 2);
                }
                somechannel_biaozhuncha[i] = Math.Sqrt(somechannel_biaozhuncha_sum / num[i]);         //求某个测磁方向4个通道分别差分后的标准差
                somechannel_biaozhuncha_sum = 0;
            }
            //求一次三西格玛
            for (int i = 0; i < 4; i++)
            {
                somechannel_max[i] = somechannel_mean[i] + double.Parse(yztext.Text) * somechannel_biaozhuncha[i];          //求出阈值线的最大值
                somechannel_min[i] = somechannel_mean[i] - double.Parse(yztext.Text) * somechannel_biaozhuncha[i];          //求出阈值线的最小值
            }
            //找到超过阈值线的差分信号部分，并把这部分用于曲线成像，对于X方向的差分信号进行第一次超阈值判断，对于这次计算不需要对curve_display_X数组做特殊的处理，只要通过判断是否超阈值来决定该点是保留还是删除
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < num[i]; j++)
                {
                    if (chafen[i, j] <= somechannel_max[i] && chafen[i, j] >= somechannel_min[i])
                    {
                        curve_display_X[i, j] = 0;                   //对于X方向原始差分信号没有超出阈值的点，将该点对应的curve_display_X数组的相应点置零
                    }
                    if (chafen[i, j] > somechannel_max[i] || chafen[i, j] < somechannel_min[i])
                    {
                        curve_display_X[i, j] = chafen[i, j];        //对于X方向原始差分信号超出阈值的点，将该点的值赋给相对应的curve_display_X数组的相应点
                    }
                }
            }
            //for (int i = 0; i < 4; i++)
            //{
            //    for (int j = 0; j <= int.Parse(qccdtext_2.Text); j++)
            //    {
            //        curve_display_X[i, j] = 0;
            //    }
            //    for(int j=;)
            //}
            //找到X方向差分信号的所有极值点
            for (int i = 0; i < 4; i++)
            {
                int k = 1;
                point_jizhi_value[i, 0] = chafen[i, 0];       //point_jizhi_value数组用于存放差分信号的极值，将数组的第一个点默认为一个极值点，无论是极大值还是极小值
                point_jizhi_location[i, 0] = 0;               //point_jizhi_location数组用于存放差分信号的极值点在差分信号中的相应位置
                for (int j = 0; j < num[i] - 2; j++)
                {
                    if ((chafen[i, j] <= chafen[i, j + 1] && chafen[i, j + 1] >= chafen[i, j + 2]) || (chafen[i, j] >= chafen[i, j + 1] && chafen[i, j + 1] <= chafen[i, j + 2]))
                    {
                        point_jizhi_value[i, k] = chafen[i, j + 1];
                        point_jizhi_location[i, k] = j + 1;
                        k = k + 1;
                    }
                }
                point_jizhi_value[i, k] = chafen[i, num[i] - 1];    //将数组的最后个点默认为一个极值点，无论是极大值还是极小值
                point_jizhi_location[i, k] = num[i] - 1;
                point_jizhi_capacity[i] = k + 1;              //存放找到的极值点的个数，也就是point_jizhi_value数组中数据的个数
            }
            //找到所有极小值点
            //for (int i = 0; i < 4; i++)
            //{
            //    int k = 0;
            //    for (int j = 0; j < num - 2; j++)
            //    {
            //        if (chafen[i, 0] < chafen[i, 1])
            //        {
            //            point_jixiaozhi_value[i, k] = chafen[i, 0];
            //            point_jixiaozhi_location[i, k] = 0;
            //            k = k + 1;
            //        }
            //        if (chafen[i, j] > chafen[i, j + 1] && chafen[i, j + 1] < chafen[i, j + 2])
            //        {
            //            point_jixiaozhi_value[i, k] = chafen[i, j + 1];
            //            point_jixiaozhi_location[i, k] = j + 1;
            //            k = k + 1;
            //        }
            //        if (chafen[i, num-2] > chafen[i, num-1])
            //        {
            //            point_jixiaozhi_value[i, k] = chafen[i, num-1];
            //            point_jixiaozhi_location[i, k] = num-1;
            //            k = k + 1;
            //        }
            //    }
            //    point_jixiaozhi_capacity[i] = k - 1;
            //}

            //找到超出阈值的极值点并删除掉，该处得到的最后数组是为了下次的再次三西格玛计算做准备
            for (int i = 0; i < 4; i++)
            {
                int k = 0;
                int p = 0;
                for (int j = 0; j < point_jizhi_capacity[i]; j++)
                {
                    if (point_jizhi_value[i, j] > somechannel_max[i] || point_jizhi_value[i, j] < somechannel_min[i])     //这里直接用上面提取出的各个极值点做判断
                    {
                        //delete_points_flag = 1;      //delete_points_flag为删点标志位，第一次如果有超过阈值的极值点就将该位置为1，标志位的意义是为了删点循环作为判断条件
                        //该处删点的方式选择是将超阈值点的极大值的前一个极大值点和后一个极大值点之间的数据点删除掉（包括极值点），极小值同理
                        if ((j - 2) <= 0)      //当未找到超阈值的极值大点前面的极大值点的时候，就把从0开始到超阈值的极大值点的后一个极大值点之间的数据点删除掉（包括极值点），极小值同理
                        {
                            chafen_location[i, 0] = 0;
                            chafen_location[i, 1] = point_jizhi_location[i, j + 2];                    //chafen_location数组是为了将X方向差分信号的将被删除极值点位置对应到差分信号的相应位置
                            for (long t = chafen_location[i, 0]; t <= chafen_location[i, 1]; t++)
                            {
                                chafen[i, t] = 10000000000000;
                            }
                            k = k + 2;
                        }
                        if ((j - 2) > 0 && (j + 2) < point_jizhi_capacity[i])
                        {
                            chafen_location[i, k] = point_jizhi_location[i, j - 2];
                            chafen_location[i, k + 1] = point_jizhi_location[i, j + 2];
                            for (long t = chafen_location[i, k]; t <= chafen_location[i, k + 1]; t++)
                            {
                                chafen[i, t] = 10000000000000;
                            }
                            k = k + 2;
                        }
                        if ((j + 2) >= point_jizhi_capacity[i])   //当未找到超阈值的极值大点后面的极大值点的时候，就把从超阈值的极大值点的前一个极大值点到数据最后一个点之间的数据点删除掉（包括极值点），极小值同理
                        {
                            chafen_location[i, k] = point_jizhi_location[i, j - 2];
                            chafen_location[i, k + 1] = point_jizhi_location[i, point_jizhi_capacity[i] - 1];
                            for (long t = chafen_location[i, k]; t <= chafen_location[i, k + 1]; t++)
                            {
                                chafen[i, t] = 10000000000000;
                            }
                            k = k + 2;
                        }
                    }
                }
                //chafen_location_capacity[i] = k;
                for (int j = 0; j < num[i]; j++)
                {
                    if (chafen[i, j] != 10000000000000)
                    {
                        chafen_reconstruct[i, p] = chafen[i, j];       //chafen_reconstruct为删点后重新用于西格玛计算的新数据
                        chafen_delete_location[i, p] = j;              //curve_display_location用于删点后曲线的重新显示，其中存放的值为未被删掉的点的原始位置坐标
                        p = p + 1;
                    }
                }
                chafen_reconstruct_capacity[i] = p;
            }
            for (int i = 0; i < 4; i++)                  //该处是为了将chafen_reconstruct数组中的内容和这个数组中数据的个数一起作为返回值返回
            {
                chafen_reconstruct[i, chafen_reconstruct_capacity[i]] = -10000000000000;
                chafen_reconstruct[i, chafen_reconstruct_capacity[i] + 1] = chafen_reconstruct_capacity[i];
            }
            return chafen_reconstruct;
        }
        private double[,] delete_points_function_again_X(double[,] chafen_orignal, double[,] chafen, long[] num)//对X方向的数据再第一次三西格玛删点后再次用三西格玛判断删点函数
        {
            //double[,] chafen = new double[4, 10000];         //原始信号的差分值
            double[] somechannel_mean = new double[4];        //某个通道差分值的平均值
            double[] somechannel_biaozhuncha = new double[4];   //某个通道差分值的标准差
            double[] somechannel_max = new double[4];        //某个通道差分值的阈值（最大值）
            double[] somechannel_min = new double[4];        //某个通道差分值的阈值（最小值）
            double somechannel_sum = 0;              //求某个通道差分值的均值时涉及到的求和变量
            double somechannel_biaozhuncha_sum = 0;       //求某个通道差分值的标准差时涉及到的求和变量
            double[,] point_jizhi_value = new double[4, 10000];      //存放某个通道差分值的极大值点
            //double[,] point_jixiaozhi_value=new double[4,10000];    //存放某个通道差分值的极小值点
            long[,] point_jizhi_location = new long[4, 10000];         //存放某个通道差分值的极大值点的位置
            //long[,] point_jixiaozhi_location=new long[4,10000];       //存放某个通道差分值的极小值点的位置
            int[] point_jizhi_capacity = new int[4];
            //int[] point_jixiaozhi_capacity = new int[4];
            long[,] chafen_location = new long[4, 10000];
            //int[] chafen_location_capacity = new int[4];
            double[,] chafen_reconstruct = new double[4, 10000];
            int[] chafen_reconstruct_capacity = new int[4];
            //double[,] curve_display = new double[4, 10000];     //最终用于曲线成像的值
            // double[,] curve_display_location = new double[4, 10000];

            //for (int i = 0; i < 4; i++)
            //{
            //    for (int j = 0; j < num[i]; j++)
            //    {
            //        curve_display[i,j]=chafen_orignal[i,j];  
            //    }
            //}

            delete_points_flag = delete_points_flag + 1;
            try
            {
                //求均值
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < num[i]; j++)
                    {
                        somechannel_sum = somechannel_sum + chafen[i, j];
                    }
                    somechannel_mean[i] = somechannel_sum / num[i];            //求某个测磁方向4个通道分别差分后的均值
                    somechannel_sum = 0;
                }
                //求标准差
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < num[i]; j++)
                    {
                        somechannel_biaozhuncha_sum = somechannel_biaozhuncha_sum + Math.Pow((chafen[i, j] - somechannel_mean[i]), 2);
                    }
                    somechannel_biaozhuncha[i] = Math.Sqrt(somechannel_biaozhuncha_sum / num[i]);         //求某个测磁方向4个通道分别差分后的标准差
                    somechannel_biaozhuncha_sum = 0;
                }
                //求一次三西格玛
                for (int i = 0; i < 4; i++)
                {
                    somechannel_max[i] = somechannel_mean[i] + double.Parse(yztext.Text) * somechannel_biaozhuncha[i];
                    somechannel_min[i] = somechannel_mean[i] - double.Parse(yztext.Text) * somechannel_biaozhuncha[i];
                }
                //找到超过阈值线的差分信号部分，并把这部分用于曲线成像
                //该函数中的chafen_orignal数组为X方向的原始差分信号，该循环的工作原理是将chafen_delete_location数组中记录的上一次删点计算中删除点的位置对应到原始差分信号chafen_orignal中
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < num[i]; j++)
                    {
                        if (chafen_orignal[i, chafen_delete_location[i, j]] <= somechannel_max[i] && chafen_orignal[i, chafen_delete_location[i, j]] >= somechannel_min[i])
                        {
                            curve_display_X[i, chafen_delete_location[i, j]] = 0;
                        }
                        if (chafen_orignal[i, chafen_delete_location[i, j]] > somechannel_max[i] || chafen_orignal[i, chafen_delete_location[i, j]] < somechannel_min[i])
                        {
                            curve_display_X[i, chafen_delete_location[i, j]] = chafen_orignal[i, chafen_delete_location[i, j]];
                        }
                    }
                }
                //找到所有极值点
                for (int i = 0; i < 4; i++)
                {
                    int k = 1;
                    point_jizhi_value[i, 0] = chafen_orignal[i, chafen_delete_location[i, 0]];
                    point_jizhi_location[i, 0] = 0;
                    for (int j = 0; j < num[i] - 2; j++)
                    {
                        if ((chafen_orignal[i, chafen_delete_location[i, j]] <= chafen_orignal[i, chafen_delete_location[i, j + 1]] && chafen_orignal[i, chafen_delete_location[i, j + 1]] >= chafen_orignal[i, chafen_delete_location[i, j + 2]]) || (chafen_orignal[i, chafen_delete_location[i, j]] >= chafen_orignal[i, chafen_delete_location[i, j + 1]] && chafen_orignal[i, chafen_delete_location[i, j + 1]] <= chafen_orignal[i, chafen_delete_location[i, j + 2]]))
                        {
                            point_jizhi_value[i, k] = chafen_orignal[i, chafen_delete_location[i, j + 1]];
                            point_jizhi_location[i, k] = chafen_delete_location[i, j + 1];
                            k = k + 1;
                        }
                    }
                    point_jizhi_value[i, k] = chafen_orignal[i, chafen_delete_location[i, num[i] - 1]];
                    point_jizhi_location[i, k] = chafen_delete_location[i, num[i] - 1];
                    point_jizhi_capacity[i] = k + 1;
                }
                //找到所有极小值点
                //for (int i = 0; i < 4; i++)
                //{
                //    int k = 0;
                //    for (int j = 0; j < num - 2; j++)
                //    {
                //        if (chafen[i, 0] < chafen[i, 1])
                //        {
                //            point_jixiaozhi_value[i, k] = chafen[i, 0];
                //            point_jixiaozhi_location[i, k] = 0;
                //            k = k + 1;
                //        }
                //        if (chafen[i, j] > chafen[i, j + 1] && chafen[i, j + 1] < chafen[i, j + 2])
                //        {
                //            point_jixiaozhi_value[i, k] = chafen[i, j + 1];
                //            point_jixiaozhi_location[i, k] = j + 1;
                //            k = k + 1;
                //        }
                //        if (chafen[i, num-2] > chafen[i, num-1])
                //        {
                //            point_jixiaozhi_value[i, k] = chafen[i, num-1];
                //            point_jixiaozhi_location[i, k] = num-1;
                //            k = k + 1;
                //        }
                //    }
                //    point_jixiaozhi_capacity[i] = k - 1;
                //}
                //找到超出阈值的极值点并删除掉
                for (int i = 0; i < 4; i++)
                {
                    int k = 0;
                    int p = 0;
                    //delete_points_flag = 2;
                    for (int j = 0; j < point_jizhi_capacity[i]; j++)
                    {
                        if (point_jizhi_value[i, j] > somechannel_max[i] || point_jizhi_value[i, j] < somechannel_min[i])
                        {
                            //delete_points_flag = 1;
                            if ((j - 2) <= 0)
                            {
                                chafen_location[i, 0] = 0;
                                chafen_location[i, 1] = point_jizhi_location[i, j + 2];
                                for (long t = chafen_location[i, 0]; t <= chafen_location[i, 1]; t++)
                                {
                                    chafen_orignal[i, t] = 10000000000000;
                                }
                                k = k + 2;
                            }
                            if ((j - 2) > 0 && (j + 2) < point_jizhi_capacity[i])
                            {
                                chafen_location[i, k] = point_jizhi_location[i, j - 2];
                                chafen_location[i, k + 1] = point_jizhi_location[i, j + 2];
                                for (long t = chafen_location[i, k]; t <= chafen_location[i, k + 1]; t++)
                                {
                                    chafen_orignal[i, t] = 10000000000000;
                                }
                                k = k + 2;
                            }
                            if ((j + 2) >= point_jizhi_capacity[i])
                            {
                                chafen_location[i, k] = point_jizhi_location[i, j - 2];
                                chafen_location[i, k + 1] = point_jizhi_location[i, point_jizhi_capacity[i] - 1];
                                for (long t = chafen_location[i, k]; t <= chafen_location[i, k + 1]; t++)
                                {
                                    chafen_orignal[i, t] = 10000000000000;
                                }
                                k = k + 2;
                            }
                        }
                        //if (j == (point_jizhi_capacity[i] - 1))
                        //{
                        //    delete_points_flag = 2;
                        //}
                    }
                    // chafen_location_capacity[i] = k;
                    for (int j = 0; j < num[i]; j++)
                    {
                        if (chafen_orignal[i, chafen_delete_location[i, j]] != 10000000000000)
                        {
                            chafen_reconstruct[i, p] = chafen_orignal[i, chafen_delete_location[i, j]];       //chafen_reconstruct为删点后重新用于西格玛计算的新数据
                            chafen_delete_location[i, p] = chafen_delete_location[i, j];              //curve_display_location用于删点后曲线的重新显示，其中存放的值为未被删掉的点的原始位置坐标
                            p = p + 1;
                        }
                    }
                    chafen_reconstruct_capacity[i] = p;
                }
                for (int i = 0; i < 4; i++)
                {
                    chafen_reconstruct[i, chafen_reconstruct_capacity[i]] = -10000000000000;
                    chafen_reconstruct[i, chafen_reconstruct_capacity[i] + 1] = chafen_reconstruct_capacity[i];
                }
                return chafen_reconstruct;
            }
            catch
            {
                MessageBox.Show("数据错误！请选用其它灵敏度或判断个数", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < num[i]; j++)
                    {
                        chafen_reconstruct[i, j] = 0;
                    }
                }
                return chafen_reconstruct;
            }
        }

        //private double[,] delete_points_function_first_Y(double[,] chafen_orignal, double[,] chafen, long[] num)//对差分完的X方向信号进行第一次三西格玛删点计算
        //{
        //    //double[,] chafen = new double[4, 10000];         //原始信号的差分值
        //    double[] somechannel_mean = new double[4];        //某个通道差分值的平均值
        //    double[] somechannel_biaozhuncha = new double[4];   //某个通道差分值的标准差
        //    double[] somechannel_max = new double[4];        //某个通道差分值的阈值（最大值）
        //    double[] somechannel_min = new double[4];        //某个通道差分值的阈值（最小值）
        //    double somechannel_sum = 0;              //求某个通道差分值的均值时涉及到的求和变量
        //    double somechannel_biaozhuncha_sum = 0;       //求某个通道差分值的标准差时涉及到的求和变量
        //    double[,] point_jizhi_value = new double[4, 10000];      //存放某个通道差分值的极大值点
        //    //double[,] point_jixiaozhi_value=new double[4,10000];    //存放某个通道差分值的极小值点
        //    long[,] point_jizhi_location = new long[4, 10000];         //存放某个通道差分值的极大值点的位置
        //    //long[,] point_jixiaozhi_location=new long[4,10000];       //存放某个通道差分值的极小值点的位置
        //    int[] point_jizhi_capacity = new int[4];
        //    //int[] point_jixiaozhi_capacity = new int[4];
        //    long[,] chafen_location = new long[4, 10000];
        //    //int[] chafen_location_capacity = new int[4];
        //    double[,] chafen_reconstruct = new double[4, 10000];
        //    int[] chafen_reconstruct_capacity = new int[4];
        //    //double[,] curve_display = new double[4, 10000];     //最终用于曲线成像的值
        //    //double[,] curve_display_location = new double[4, 10000];

        //    //for (int i = 0; i < 4; i++)
        //    //{
        //    //    for (int j = 0; j < num[i]; j++)
        //    //    {
        //    //        curve_display[i,j]=chafen_orignal[i,j];  
        //    //    }
        //    //}
        //    //求均值
        //    for (int i = 0; i < 4; i++)
        //    {
        //        for (int j = 0; j < num[i]; j++)
        //        {
        //            somechannel_sum = somechannel_sum + chafen[i, j];
        //        }
        //        somechannel_mean[i] = somechannel_sum / num[i];            //求某个测磁方向4个通道分别差分后的均值
        //        somechannel_sum = 0;
        //    }
        //    //求标准差
        //    for (int i = 0; i < 4; i++)
        //    {
        //        for (int j = 0; j < num[i]; j++)
        //        {
        //            somechannel_biaozhuncha_sum = somechannel_biaozhuncha_sum + Math.Pow((chafen[i, j] - somechannel_mean[i]), 2);
        //        }
        //        somechannel_biaozhuncha[i] = Math.Sqrt(somechannel_biaozhuncha_sum / num[i]);         //求某个测磁方向4个通道分别差分后的标准差
        //        somechannel_biaozhuncha_sum = 0;
        //    }
        //    //求一次三西格玛
        //    for (int i = 0; i < 4; i++)
        //    {
        //        somechannel_max[i] = somechannel_mean[i] + double.Parse(yztext.Text) * somechannel_biaozhuncha[i];          //求出阈值线的最大值
        //        somechannel_min[i] = somechannel_mean[i] - double.Parse(yztext.Text) * somechannel_biaozhuncha[i];          //求出阈值线的最小值
        //    }
        //    //找到超过阈值线的差分信号部分，并把这部分用于曲线成像，对于Y方向的差分信号进行第一次超阈值判断，对于这次计算不需要对curve_display_Y数组做特殊的处理，只要通过判断是否超阈值来决定该点是保留还是删除
        //    for (int i = 0; i < 4; i++)
        //    {
        //        for (int j = 0; j < num[i]; j++)
        //        {
        //            if (chafen[i, j] <= somechannel_max[i] && chafen[i, j] >= somechannel_min[i])
        //            {
        //                curve_display_Y[i, j] = 0;                   //对于Y方向原始差分信号没有超出阈值的点，将该点对应的curve_display_Y数组的相应点置零
        //            }
        //            if (chafen[i, j] > somechannel_max[i] || chafen[i, j] < somechannel_min[i])
        //            {
        //                curve_display_Y[i, j] = chafen[i, j];        //对于Y方向原始差分信号超出阈值的点，将该点的值赋给相对应的curve_display_Y数组的相应点
        //            }
        //        }
        //    }
        //    //for (int i = 0; i < 4; i++)
        //    //{
        //    //    for (int j = 0; j <= int.Parse(qccdtext_2.Text); j++)
        //    //    {
        //    //        curve_display_X[i, j] = 0;
        //    //    }
        //    //    for(int j=;)
        //    //}
        //    //找到X方向差分信号的所有极值点
        //    for (int i = 0; i < 4; i++)
        //    {
        //        int k = 1;
        //        point_jizhi_value[i, 0] = chafen[i, 0];       //point_jizhi_value数组用于存放差分信号的极值，将数组的第一个点默认为一个极值点，无论是极大值还是极小值
        //        point_jizhi_location[i, 0] = 0;               //point_jizhi_location数组用于存放差分信号的极值点在差分信号中的相应位置
        //        for (int j = 0; j < num[i] - 2; j++)
        //        {
        //            if ((chafen[i, j] <= chafen[i, j + 1] && chafen[i, j + 1] >= chafen[i, j + 2]) || (chafen[i, j] >= chafen[i, j + 1] && chafen[i, j + 1] <= chafen[i, j + 2]))
        //            {
        //                point_jizhi_value[i, k] = chafen[i, j + 1];
        //                point_jizhi_location[i, k] = j + 1;
        //                k = k + 1;
        //            }
        //        }
        //        point_jizhi_value[i, k] = chafen[i, num[i] - 1];    //将数组的最后个点默认为一个极值点，无论是极大值还是极小值
        //        point_jizhi_location[i, k] = num[i] - 1;
        //        point_jizhi_capacity[i] = k + 1;              //存放找到的极值点的个数，也就是point_jizhi_value数组中数据的个数
        //    }
        //    //找到所有极小值点
        //    //for (int i = 0; i < 4; i++)
        //    //{
        //    //    int k = 0;
        //    //    for (int j = 0; j < num - 2; j++)
        //    //    {
        //    //        if (chafen[i, 0] < chafen[i, 1])
        //    //        {
        //    //            point_jixiaozhi_value[i, k] = chafen[i, 0];
        //    //            point_jixiaozhi_location[i, k] = 0;
        //    //            k = k + 1;
        //    //        }
        //    //        if (chafen[i, j] > chafen[i, j + 1] && chafen[i, j + 1] < chafen[i, j + 2])
        //    //        {
        //    //            point_jixiaozhi_value[i, k] = chafen[i, j + 1];
        //    //            point_jixiaozhi_location[i, k] = j + 1;
        //    //            k = k + 1;
        //    //        }
        //    //        if (chafen[i, num-2] > chafen[i, num-1])
        //    //        {
        //    //            point_jixiaozhi_value[i, k] = chafen[i, num-1];
        //    //            point_jixiaozhi_location[i, k] = num-1;
        //    //            k = k + 1;
        //    //        }
        //    //    }
        //    //    point_jixiaozhi_capacity[i] = k - 1;
        //    //}

        //    //找到超出阈值的极值点并删除掉，该处得到的最后数组是为了下次的再次三西格玛计算做准备
        //    for (int i = 0; i < 4; i++)
        //    {
        //        int k = 0;
        //        int p = 0;
        //        for (int j = 0; j < point_jizhi_capacity[i]; j++)
        //        {
        //            if (point_jizhi_value[i, j] > somechannel_max[i] || point_jizhi_value[i, j] < somechannel_min[i])     //这里直接用上面提取出的各个极值点做判断
        //            {
        //                delete_points_flag = 1;      //delete_points_flag为删点标志位，第一次如果有超过阈值的极值点就将该位置为1，标志位的意义是为了删点循环作为判断条件
        //                //该处删点的方式选择是将超阈值点的极大值的前一个极大值点和后一个极大值点之间的数据点删除掉（包括极值点），极小值同理
        //                if ((j - 2) <= 0)      //当未找到超阈值的极值大点前面的极大值点的时候，就把从0开始到超阈值的极大值点的后一个极大值点之间的数据点删除掉（包括极值点），极小值同理
        //                {
        //                    chafen_location[i, 0] = 0;
        //                    chafen_location[i, 1] = point_jizhi_location[i, j + 2];                    //chafen_location数组是为了将X方向差分信号的将被删除极值点位置对应到差分信号的相应位置
        //                    for (long t = chafen_location[i, 0]; t <= chafen_location[i, 1]; t++)
        //                    {
        //                        chafen[i, t] = 10000000000000;
        //                    }
        //                    k = k + 2;
        //                }
        //                if ((j - 2) > 0 && (j + 2) < point_jizhi_capacity[i])
        //                {
        //                    chafen_location[i, k] = point_jizhi_location[i, j - 2];
        //                    chafen_location[i, k + 1] = point_jizhi_location[i, j + 2];
        //                    for (long t = chafen_location[i, k]; t <= chafen_location[i, k + 1]; t++)
        //                    {
        //                        chafen[i, t] = 10000000000000;
        //                    }
        //                    k = k + 2;
        //                }
        //                if ((j + 2) >= point_jizhi_capacity[i])   //当未找到超阈值的极值大点后面的极大值点的时候，就把从超阈值的极大值点的前一个极大值点到数据最后一个点之间的数据点删除掉（包括极值点），极小值同理
        //                {
        //                    chafen_location[i, k] = point_jizhi_location[i, j - 2];
        //                    chafen_location[i, k + 1] = point_jizhi_location[i, point_jizhi_capacity[i] - 1];
        //                    for (long t = chafen_location[i, k]; t <= chafen_location[i, k + 1]; t++)
        //                    {
        //                        chafen[i, t] = 10000000000000;
        //                    }
        //                    k = k + 2;
        //                }
        //            }
        //        }
        //        //chafen_location_capacity[i] = k;
        //        for (int j = 0; j < num[i]; j++)
        //        {
        //            if (chafen[i, j] != 10000000000000)
        //            {
        //                chafen_reconstruct[i, p] = chafen[i, j];       //chafen_reconstruct为删点后重新用于西格玛计算的新数据
        //                chafen_delete_location[i, p] = j;              //curve_display_location用于删点后曲线的重新显示，其中存放的值为未被删掉的点的原始位置坐标
        //                p = p + 1;
        //            }
        //        }
        //        chafen_reconstruct_capacity[i] = p;
        //    }
        //    for (int i = 0; i < 4; i++)                  //该处是为了将chafen_reconstruct数组中的内容和这个数组中数据的个数一起作为返回值返回
        //    {
        //        chafen_reconstruct[i, chafen_reconstruct_capacity[i]] = -10000000000000;
        //        chafen_reconstruct[i, chafen_reconstruct_capacity[i] + 1] = chafen_reconstruct_capacity[i];
        //    }
        //    return chafen_reconstruct;
        //}
        //private double[,] delete_points_function_again_Y(double[,] chafen_orignal, double[,] chafen, long[] num)//对X方向的数据再第一次三西格玛删点后再次用三西格玛判断删点函数
        //{
        //    //double[,] chafen = new double[4, 10000];         //原始信号的差分值
        //    double[] somechannel_mean = new double[4];        //某个通道差分值的平均值
        //    double[] somechannel_biaozhuncha = new double[4];   //某个通道差分值的标准差
        //    double[] somechannel_max = new double[4];        //某个通道差分值的阈值（最大值）
        //    double[] somechannel_min = new double[4];        //某个通道差分值的阈值（最小值）
        //    double somechannel_sum = 0;              //求某个通道差分值的均值时涉及到的求和变量
        //    double somechannel_biaozhuncha_sum = 0;       //求某个通道差分值的标准差时涉及到的求和变量
        //    double[,] point_jizhi_value = new double[4, 10000];      //存放某个通道差分值的极大值点
        //    //double[,] point_jixiaozhi_value=new double[4,10000];    //存放某个通道差分值的极小值点
        //    long[,] point_jizhi_location = new long[4, 10000];         //存放某个通道差分值的极大值点的位置
        //    //long[,] point_jixiaozhi_location=new long[4,10000];       //存放某个通道差分值的极小值点的位置
        //    int[] point_jizhi_capacity = new int[4];
        //    //int[] point_jixiaozhi_capacity = new int[4];
        //    long[,] chafen_location = new long[4, 10000];
        //    //int[] chafen_location_capacity = new int[4];
        //    double[,] chafen_reconstruct = new double[4, 10000];
        //    int[] chafen_reconstruct_capacity = new int[4];
        //    //double[,] curve_display = new double[4, 10000];     //最终用于曲线成像的值
        //    // double[,] curve_display_location = new double[4, 10000];

        //    //for (int i = 0; i < 4; i++)
        //    //{
        //    //    for (int j = 0; j < num[i]; j++)
        //    //    {
        //    //        curve_display[i,j]=chafen_orignal[i,j];  
        //    //    }
        //    //}
        //    //求均值
        //    for (int i = 0; i < 4; i++)
        //    {
        //        for (int j = 0; j < num[i]; j++)
        //        {
        //            somechannel_sum = somechannel_sum + chafen[i, j];
        //        }
        //        somechannel_mean[i] = somechannel_sum / num[i];            //求某个测磁方向4个通道分别差分后的均值
        //        somechannel_sum = 0;
        //    }
        //    //求标准差
        //    for (int i = 0; i < 4; i++)
        //    {
        //        for (int j = 0; j < num[i]; j++)
        //        {
        //            somechannel_biaozhuncha_sum = somechannel_biaozhuncha_sum + Math.Pow((chafen[i, j] - somechannel_mean[i]), 2);
        //        }
        //        somechannel_biaozhuncha[i] = Math.Sqrt(somechannel_biaozhuncha_sum / num[i]);         //求某个测磁方向4个通道分别差分后的标准差
        //        somechannel_biaozhuncha_sum = 0;
        //    }
        //    //求一次三西格玛
        //    for (int i = 0; i < 4; i++)
        //    {
        //        somechannel_max[i] = somechannel_mean[i] + double.Parse(yztext.Text) * somechannel_biaozhuncha[i];
        //        somechannel_min[i] = somechannel_mean[i] - double.Parse(yztext.Text) * somechannel_biaozhuncha[i];
        //    }
        //    //找到超过阈值线的差分信号部分，并把这部分用于曲线成像
        //    //该函数中的chafen_orignal数组为X方向的原始差分信号，该循环的工作原理是将chafen_delete_location数组中记录的上一次删点计算中删除点的位置对应到原始差分信号chafen_orignal中
        //    for (int i = 0; i < 4; i++)
        //    {
        //        for (int j = 0; j < num[i]; j++)
        //        {
        //            if (chafen_orignal[i, chafen_delete_location[i, j]] <= somechannel_max[i] && chafen_orignal[i, chafen_delete_location[i, j]] >= somechannel_min[i])
        //            {
        //                curve_display_Y[i, chafen_delete_location[i, j]] = 0;
        //            }
        //            if (chafen_orignal[i, chafen_delete_location[i, j]] > somechannel_max[i] || chafen_orignal[i, chafen_delete_location[i, j]] < somechannel_min[i])
        //            {
        //                curve_display_Y[i, chafen_delete_location[i, j]] = chafen_orignal[i, chafen_delete_location[i, j]];
        //            }
        //        }
        //    }
        //    //找到所有极值点
        //    for (int i = 0; i < 4; i++)
        //    {
        //        int k = 1;
        //        point_jizhi_value[i, 0] = chafen_orignal[i, chafen_delete_location[i, 0]];
        //        point_jizhi_location[i, 0] = 0;
        //        for (int j = 0; j < num[i] - 2; j++)
        //        {
        //            if ((chafen_orignal[i, chafen_delete_location[i, j]] <= chafen_orignal[i, chafen_delete_location[i, j + 1]] && chafen_orignal[i, chafen_delete_location[i, j + 1]] >= chafen_orignal[i, chafen_delete_location[i, j + 2]]) || (chafen_orignal[i, chafen_delete_location[i, j]] >= chafen_orignal[i, chafen_delete_location[i, j + 1]] && chafen_orignal[i, chafen_delete_location[i, j + 1]] <= chafen_orignal[i, chafen_delete_location[i, j + 2]]))
        //            {
        //                point_jizhi_value[i, k] = chafen_orignal[i, chafen_delete_location[i, j + 1]];
        //                point_jizhi_location[i, k] = chafen_delete_location[i, j + 1];
        //                k = k + 1;
        //            }
        //        }
        //        point_jizhi_value[i, k] = chafen_orignal[i, chafen_delete_location[i, num[i] - 1]];
        //        point_jizhi_location[i, k] = chafen_delete_location[i, num[i] - 1];
        //        point_jizhi_capacity[i] = k + 1;
        //    }
        //    //找到所有极小值点
        //    //for (int i = 0; i < 4; i++)
        //    //{
        //    //    int k = 0;
        //    //    for (int j = 0; j < num - 2; j++)
        //    //    {
        //    //        if (chafen[i, 0] < chafen[i, 1])
        //    //        {
        //    //            point_jixiaozhi_value[i, k] = chafen[i, 0];
        //    //            point_jixiaozhi_location[i, k] = 0;
        //    //            k = k + 1;
        //    //        }
        //    //        if (chafen[i, j] > chafen[i, j + 1] && chafen[i, j + 1] < chafen[i, j + 2])
        //    //        {
        //    //            point_jixiaozhi_value[i, k] = chafen[i, j + 1];
        //    //            point_jixiaozhi_location[i, k] = j + 1;
        //    //            k = k + 1;
        //    //        }
        //    //        if (chafen[i, num-2] > chafen[i, num-1])
        //    //        {
        //    //            point_jixiaozhi_value[i, k] = chafen[i, num-1];
        //    //            point_jixiaozhi_location[i, k] = num-1;
        //    //            k = k + 1;
        //    //        }
        //    //    }
        //    //    point_jixiaozhi_capacity[i] = k - 1;
        //    //}
        //    //找到超出阈值的极值点并删除掉
        //    for (int i = 0; i < 4; i++)
        //    {
        //        int k = 0;
        //        int p = 0;
        //        for (int j = 0; j < point_jizhi_capacity[i]; j++)
        //        {
        //            if (point_jizhi_value[i, j] > somechannel_max[i] || point_jizhi_value[i, j] < somechannel_min[i])
        //            {
        //                delete_points_flag = 1;
        //                if ((j - 2) <= 0)
        //                {
        //                    chafen_location[i, 0] = 0;
        //                    chafen_location[i, 1] = point_jizhi_location[i, j + 2];
        //                    for (long t = chafen_location[i, 0]; t <= chafen_location[i, 1]; t++)
        //                    {
        //                        chafen_orignal[i, t] = 10000000000000;
        //                    }
        //                    k = k + 2;
        //                }
        //                if ((j - 2) > 0 && (j + 2) < point_jizhi_capacity[i])
        //                {
        //                    chafen_location[i, k] = point_jizhi_location[i, j - 2];
        //                    chafen_location[i, k + 1] = point_jizhi_location[i, j + 2];
        //                    for (long t = chafen_location[i, k]; t <= chafen_location[i, k + 1]; t++)
        //                    {
        //                        chafen_orignal[i, t] = 10000000000000;
        //                    }
        //                    k = k + 2;
        //                }
        //                if ((j + 2) >= point_jizhi_capacity[i])
        //                {
        //                    chafen_location[i, k] = point_jizhi_location[i, j - 2];
        //                    chafen_location[i, k + 1] = point_jizhi_location[i, point_jizhi_capacity[i] - 1];
        //                    for (long t = chafen_location[i, k]; t <= chafen_location[i, k + 1]; t++)
        //                    {
        //                        chafen_orignal[i, t] = 10000000000000;
        //                    }
        //                    k = k + 2;
        //                }
        //            }
        //            if (j == (point_jizhi_capacity[i] - 1))
        //            {
        //                delete_points_flag = 2;
        //            }
        //        }
        //        // chafen_location_capacity[i] = k;
        //        for (int j = 0; j < num[i]; j++)
        //        {
        //            if (chafen_orignal[i, chafen_delete_location[i, j]] != 10000000000000)
        //            {
        //                chafen_reconstruct[i, p] = chafen_orignal[i, chafen_delete_location[i, j]];       //chafen_reconstruct为删点后重新用于西格玛计算的新数据
        //                chafen_delete_location[i, p] = chafen_delete_location[i, j];              //curve_display_location用于删点后曲线的重新显示，其中存放的值为未被删掉的点的原始位置坐标
        //                p = p + 1;
        //            }
        //        }
        //        chafen_reconstruct_capacity[i] = p;
        //    }
        //    for (int i = 0; i < 4; i++)
        //    {
        //        chafen_reconstruct[i, chafen_reconstruct_capacity[i]] = -10000000000000;
        //        chafen_reconstruct[i, chafen_reconstruct_capacity[i] + 1] = chafen_reconstruct_capacity[i];
        //    }
        //    return chafen_reconstruct;
        //}

        private double[,] delete_points_function_first_Z(double[,] chafen_orignal, double[,] chafen, long[] num)//用三西格玛判断删点函数
        {
            //double[,] chafen = new double[4, 10000];         //原始信号的差分值
            double[] somechannel_mean = new double[4];        //某个通道差分值的平均值
            double[] somechannel_biaozhuncha = new double[4];   //某个通道差分值的标准差
            double[] somechannel_max = new double[4];        //某个通道差分值的阈值（最大值）
            double[] somechannel_min = new double[4];        //某个通道差分值的阈值（最小值）
            double somechannel_sum = 0;              //求某个通道差分值的均值时涉及到的求和变量
            double somechannel_biaozhuncha_sum = 0;       //求某个通道差分值的标准差时涉及到的求和变量
            double[,] point_jizhi_value = new double[4, 10000];      //存放某个通道差分值的极大值点
            //double[,] point_jixiaozhi_value=new double[4,10000];    //存放某个通道差分值的极小值点
            long[,] point_jizhi_location = new long[4, 10000];         //存放某个通道差分值的极大值点的位置
            //long[,] point_jixiaozhi_location=new long[4,10000];       //存放某个通道差分值的极小值点的位置
            int[] point_jizhi_capacity = new int[4];
            //int[] point_jixiaozhi_capacity = new int[4];
            long[,] chafen_location = new long[4, 10000];
            //int[] chafen_location_capacity = new int[4];
            double[,] chafen_reconstruct = new double[4, 10000];
            int[] chafen_reconstruct_capacity = new int[4];
            //double[,] curve_display = new double[4, 10000];     //最终用于曲线成像的值
            //double[,] curve_display_location = new double[4, 10000];

            //for (int i = 0; i < 4; i++)
            //{
            //    for (int j = 0; j < num[i]; j++)
            //    {
            //        curve_display[i,j]=chafen_orignal[i,j];  
            //    }
            //}

            delete_points_flag = 1;

            //求均值
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < num[i]; j++)
                {
                    somechannel_sum = somechannel_sum + chafen[i, j];
                }
                somechannel_mean[i] = somechannel_sum / num[i];            //求某个测磁方向4个通道分别差分后的均值
                somechannel_sum = 0;
            }
            //求标准差
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < num[i]; j++)
                {
                    somechannel_biaozhuncha_sum = somechannel_biaozhuncha_sum + Math.Pow((chafen[i, j] - somechannel_mean[i]), 2);
                }
                somechannel_biaozhuncha[i] = Math.Sqrt(somechannel_biaozhuncha_sum / num[i]);         //求某个测磁方向4个通道分别差分后的标准差
                somechannel_biaozhuncha_sum = 0;
            }
            //求一次三西格玛
            for (int i = 0; i < 4; i++)
            {
                somechannel_max[i] = somechannel_mean[i] + double.Parse(yztext.Text) * somechannel_biaozhuncha[i];
                somechannel_min[i] = somechannel_mean[i] - double.Parse(yztext.Text) * somechannel_biaozhuncha[i];
            }
            //找到超过阈值线的差分信号部分，并把这部分用于曲线成像
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < num[i]; j++)
                {
                    if (chafen[i, j] <= somechannel_max[i] && chafen[i, j] >= somechannel_min[i])
                    {
                        curve_display_Z[i, j] = 0;
                    }
                    if (chafen[i, j] > somechannel_max[i] || chafen[i, j] < somechannel_min[i])
                    {
                        curve_display_Z[i, j] = chafen[i, j];
                    }
                }
            }
            //找到所有极值点
            for (int i = 0; i < 4; i++)
            {
                int k = 1;
                point_jizhi_value[i, 0] = chafen[i, 0];
                point_jizhi_location[i, 0] = 0;
                for (int j = 0; j < num[i] - 2; j++)
                {
                    if ((chafen[i, j] <= chafen[i, j + 1] && chafen[i, j + 1] >= chafen[i, j + 2]) || (chafen[i, j] >= chafen[i, j + 1] && chafen[i, j + 1] <= chafen[i, j + 2]))
                    {
                        point_jizhi_value[i, k] = chafen[i, j + 1];
                        point_jizhi_location[i, k] = j + 1;
                        k = k + 1;
                    }
                }
                point_jizhi_value[i, k] = chafen[i, num[i] - 1];
                point_jizhi_location[i, k] = num[i] - 1;
                point_jizhi_capacity[i] = k + 1;
            }
            //找到所有极小值点
            //for (int i = 0; i < 4; i++)
            //{
            //    int k = 0;
            //    for (int j = 0; j < num - 2; j++)
            //    {
            //        if (chafen[i, 0] < chafen[i, 1])
            //        {
            //            point_jixiaozhi_value[i, k] = chafen[i, 0];
            //            point_jixiaozhi_location[i, k] = 0;
            //            k = k + 1;
            //        }
            //        if (chafen[i, j] > chafen[i, j + 1] && chafen[i, j + 1] < chafen[i, j + 2])
            //        {
            //            point_jixiaozhi_value[i, k] = chafen[i, j + 1];
            //            point_jixiaozhi_location[i, k] = j + 1;
            //            k = k + 1;
            //        }
            //        if (chafen[i, num-2] > chafen[i, num-1])
            //        {
            //            point_jixiaozhi_value[i, k] = chafen[i, num-1];
            //            point_jixiaozhi_location[i, k] = num-1;
            //            k = k + 1;
            //        }
            //    }
            //    point_jixiaozhi_capacity[i] = k - 1;
            //}
            //找到超出阈值的极值点并删除掉
            for (int i = 0; i < 4; i++)
            {
                int k = 0;
                int p = 0;
                for (int j = 0; j < point_jizhi_capacity[i]; j++)
                {
                    if (point_jizhi_value[i, j] > somechannel_max[i] || point_jizhi_value[i, j] < somechannel_min[i])
                    {
                        //delete_points_flag = 1;
                        if ((j - 2) <= 0)
                        {
                            chafen_location[i, 0] = 0;
                            chafen_location[i, 1] = point_jizhi_location[i, j + 2];
                            for (long t = chafen_location[i, 0]; t <= chafen_location[i, 1]; t++)
                            {
                                chafen[i, t] = 10000000000000;
                            }
                            k = k + 2;
                        }
                        if ((j - 2) > 0 && (j + 2) < point_jizhi_capacity[i])
                        {
                            chafen_location[i, k] = point_jizhi_location[i, j - 2];
                            chafen_location[i, k + 1] = point_jizhi_location[i, j + 2];
                            for (long t = chafen_location[i, k]; t <= chafen_location[i, k + 1]; t++)
                            {
                                chafen[i, t] = 10000000000000;
                            }
                            k = k + 2;
                        }
                        if ((j + 2) >= point_jizhi_capacity[i])
                        {
                            chafen_location[i, k] = point_jizhi_location[i, j - 2];
                            chafen_location[i, k + 1] = point_jizhi_location[i, point_jizhi_capacity[i] - 1];
                            for (long t = chafen_location[i, k]; t <= chafen_location[i, k + 1]; t++)
                            {
                                chafen[i, t] = 10000000000000;
                            }
                            k = k + 2;
                        }
                    }
                }
                //chafen_location_capacity[i] = k;
                for (int j = 0; j < num[i]; j++)
                {
                    if (chafen[i, j] != 10000000000000)
                    {
                        chafen_reconstruct[i, p] = chafen[i, j];       //chafen_reconstruct为删点后重新用于西格玛计算的新数据
                        chafen_delete_location[i, p] = j;              //curve_display_location用于删点后曲线的重新显示，其中存放的值为未被删掉的点的原始位置坐标
                        p = p + 1;
                    }
                }
                chafen_reconstruct_capacity[i] = p;
            }
            for (int i = 0; i < 4; i++)
            {
                chafen_reconstruct[i, chafen_reconstruct_capacity[i]] = -10000000000000;
                chafen_reconstruct[i, chafen_reconstruct_capacity[i] + 1] = chafen_reconstruct_capacity[i];
            }
            return chafen_reconstruct;
        }
        private double[,] delete_points_function_again_Z(double[,] chafen_orignal, double[,] chafen, long[] num)//用三西格玛判断删点函数
        {
            //double[,] chafen = new double[4, 10000];         //原始信号的差分值
            double[] somechannel_mean = new double[4];        //某个通道差分值的平均值
            double[] somechannel_biaozhuncha = new double[4];   //某个通道差分值的标准差
            double[] somechannel_max = new double[4];        //某个通道差分值的阈值（最大值）
            double[] somechannel_min = new double[4];        //某个通道差分值的阈值（最小值）
            double somechannel_sum = 0;              //求某个通道差分值的均值时涉及到的求和变量
            double somechannel_biaozhuncha_sum = 0;       //求某个通道差分值的标准差时涉及到的求和变量
            double[,] point_jizhi_value = new double[4, 10000];      //存放某个通道差分值的极大值点
            //double[,] point_jixiaozhi_value=new double[4,10000];    //存放某个通道差分值的极小值点
            long[,] point_jizhi_location = new long[4, 10000];         //存放某个通道差分值的极大值点的位置
            //long[,] point_jixiaozhi_location=new long[4,10000];       //存放某个通道差分值的极小值点的位置
            int[] point_jizhi_capacity = new int[4];
            //int[] point_jixiaozhi_capacity = new int[4];
            long[,] chafen_location = new long[4, 10000];
            //int[] chafen_location_capacity = new int[4];
            double[,] chafen_reconstruct = new double[4, 10000];
            int[] chafen_reconstruct_capacity = new int[4];
            //double[,] curve_display = new double[4, 10000];     //最终用于曲线成像的值
            // double[,] curve_display_location = new double[4, 10000];

            //for (int i = 0; i < 4; i++)
            //{
            //    for (int j = 0; j < num[i]; j++)
            //    {
            //        curve_display[i,j]=chafen_orignal[i,j];  
            //    }
            //}

            delete_points_flag = delete_points_flag + 1;
            try
            {
                //求均值
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < num[i]; j++)
                    {
                        somechannel_sum = somechannel_sum + chafen[i, j];
                    }
                    somechannel_mean[i] = somechannel_sum / num[i];            //求某个测磁方向4个通道分别差分后的均值
                    somechannel_sum = 0;
                }
                //求标准差
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < num[i]; j++)
                    {
                        somechannel_biaozhuncha_sum = somechannel_biaozhuncha_sum + Math.Pow((chafen[i, j] - somechannel_mean[i]), 2);
                    }
                    somechannel_biaozhuncha[i] = Math.Sqrt(somechannel_biaozhuncha_sum / num[i]);         //求某个测磁方向4个通道分别差分后的标准差
                    somechannel_biaozhuncha_sum = 0;
                }
                //求一次三西格玛
                for (int i = 0; i < 4; i++)
                {
                    somechannel_max[i] = somechannel_mean[i] + double.Parse(yztext.Text) * somechannel_biaozhuncha[i];
                    somechannel_min[i] = somechannel_mean[i] - double.Parse(yztext.Text) * somechannel_biaozhuncha[i];
                }
                //找到超过阈值线的差分信号部分，并把这部分用于曲线成像
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < num[i]; j++)
                    {
                        if (chafen_orignal[i, chafen_delete_location[i, j]] <= somechannel_max[i] && chafen_orignal[i, chafen_delete_location[i, j]] >= somechannel_min[i])
                        {
                            curve_display_Z[i, chafen_delete_location[i, j]] = 0;
                        }
                        if (chafen_orignal[i, chafen_delete_location[i, j]] > somechannel_max[i] || chafen_orignal[i, chafen_delete_location[i, j]] < somechannel_min[i])
                        {
                            curve_display_Z[i, chafen_delete_location[i, j]] = chafen_orignal[i, chafen_delete_location[i, j]];
                        }
                    }
                }
                //找到所有极值点
                for (int i = 0; i < 4; i++)
                {
                    int k = 1;
                    point_jizhi_value[i, 0] = chafen_orignal[i, chafen_delete_location[i, 0]];
                    point_jizhi_location[i, 0] = 0;
                    for (int j = 0; j < num[i] - 2; j++)
                    {
                        if ((chafen_orignal[i, chafen_delete_location[i, j]] <= chafen_orignal[i, chafen_delete_location[i, j + 1]] && chafen_orignal[i, chafen_delete_location[i, j + 1]] >= chafen_orignal[i, chafen_delete_location[i, j + 2]]) || (chafen_orignal[i, chafen_delete_location[i, j]] >= chafen_orignal[i, chafen_delete_location[i, j + 1]] && chafen_orignal[i, chafen_delete_location[i, j + 1]] <= chafen_orignal[i, chafen_delete_location[i, j + 2]]))
                        {
                            point_jizhi_value[i, k] = chafen_orignal[i, chafen_delete_location[i, j + 1]];
                            point_jizhi_location[i, k] = chafen_delete_location[i, j + 1];
                            k = k + 1;
                        }
                    }
                    point_jizhi_value[i, k] = chafen_orignal[i, chafen_delete_location[i, num[i] - 1]];
                    point_jizhi_location[i, k] = chafen_delete_location[i, num[i] - 1];
                    point_jizhi_capacity[i] = k + 1;
                }
                //找到所有极小值点
                //for (int i = 0; i < 4; i++)
                //{
                //    int k = 0;
                //    for (int j = 0; j < num - 2; j++)
                //    {
                //        if (chafen[i, 0] < chafen[i, 1])
                //        {
                //            point_jixiaozhi_value[i, k] = chafen[i, 0];
                //            point_jixiaozhi_location[i, k] = 0;
                //            k = k + 1;
                //        }
                //        if (chafen[i, j] > chafen[i, j + 1] && chafen[i, j + 1] < chafen[i, j + 2])
                //        {
                //            point_jixiaozhi_value[i, k] = chafen[i, j + 1];
                //            point_jixiaozhi_location[i, k] = j + 1;
                //            k = k + 1;
                //        }
                //        if (chafen[i, num-2] > chafen[i, num-1])
                //        {
                //            point_jixiaozhi_value[i, k] = chafen[i, num-1];
                //            point_jixiaozhi_location[i, k] = num-1;
                //            k = k + 1;
                //        }
                //    }
                //    point_jixiaozhi_capacity[i] = k - 1;
                //}
                //找到超出阈值的极值点并删除掉
                for (int i = 0; i < 4; i++)
                {
                    int k = 0;
                    int p = 0;
                    //delete_points_flag = 2;
                    for (int j = 0; j < point_jizhi_capacity[i]; j++)
                    {
                        if (point_jizhi_value[i, j] > somechannel_max[i] || point_jizhi_value[i, j] < somechannel_min[i])
                        {
                            //delete_points_flag = 1;
                            if ((j - 2) <= 0)
                            {
                                chafen_location[i, 0] = 0;
                                chafen_location[i, 1] = point_jizhi_location[i, j + 2];
                                for (long t = chafen_location[i, 0]; t <= chafen_location[i, 1]; t++)
                                {
                                    chafen_orignal[i, t] = 10000000000000;
                                }
                                k = k + 2;
                            }
                            if ((j - 2) > 0 && (j + 2) < point_jizhi_capacity[i])
                            {
                                chafen_location[i, k] = point_jizhi_location[i, j - 2];
                                chafen_location[i, k + 1] = point_jizhi_location[i, j + 2];
                                for (long t = chafen_location[i, k]; t <= chafen_location[i, k + 1]; t++)
                                {
                                    chafen_orignal[i, t] = 10000000000000;
                                }
                                k = k + 2;
                            }
                            if ((j + 2) >= point_jizhi_capacity[i])
                            {
                                chafen_location[i, k] = point_jizhi_location[i, j - 2];
                                chafen_location[i, k + 1] = point_jizhi_location[i, point_jizhi_capacity[i] - 1];
                                for (long t = chafen_location[i, k]; t <= chafen_location[i, k + 1]; t++)
                                {
                                    chafen_orignal[i, t] = 10000000000000;
                                }
                                k = k + 2;
                            }
                        }
                        //if (j == (point_jizhi_capacity[i] - 1))
                        //{
                        //    delete_points_flag = 2;
                        //}
                    }
                    // chafen_location_capacity[i] = k;
                    for (int j = 0; j < num[i]; j++)
                    {
                        if (chafen_orignal[i, chafen_delete_location[i, j]] != 10000000000000)
                        {
                            chafen_reconstruct[i, p] = chafen_orignal[i, chafen_delete_location[i, j]];       //chafen_reconstruct为删点后重新用于西格玛计算的新数据
                            chafen_delete_location[i, p] = chafen_delete_location[i, j];              //curve_display_location用于删点后曲线的重新显示，其中存放的值为未被删掉的点的原始位置坐标
                            p = p + 1;
                        }
                    }
                    chafen_reconstruct_capacity[i] = p;
                }
                for (int i = 0; i < 4; i++)
                {
                    chafen_reconstruct[i, chafen_reconstruct_capacity[i]] = -10000000000000;
                    chafen_reconstruct[i, chafen_reconstruct_capacity[i] + 1] = chafen_reconstruct_capacity[i];
                }
                return chafen_reconstruct;
            }
            catch
            {
                MessageBox.Show("数据错误！请选用其它灵敏度或判断个数", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < num[i]; j++)
                    {
                        chafen_reconstruct[i, j] = 0;
                    }
                }
                return chafen_reconstruct;
            }
        }

        private double[,] defect_recognition(double[,] curve, long num)
        {
            int[,] jizhi_location = new int[4, 10000];
            int[] jizhi_capacity = new int[4];
            double[,] defect = new double[4, 10000];
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < num; j++)
                {
                    defect[i, j] = curve[i, j];
                }
            }
            for (int i = 0; i < 4; i++)
            {
                int k = 0;
                for (int j = 0; j < num - 2; j++)
                {
                    if (curve[i, j + 1] != 0)
                    {
                        if (curve[i, j] <= curve[i, j + 1] && curve[i, j + 1] >= curve[i, j + 2] || curve[i, j] >= curve[i, j + 1] && curve[i, j + 1] <= curve[i, j + 2])
                        {
                            jizhi_location[i, k] = j + 1;
                            k = k + 1;
                        }
                    }
                }
                jizhi_capacity[i] = k;
            }
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < jizhi_capacity[i] - 1; j++)
                {
                    if (((jizhi_location[i, j + 1] - jizhi_location[i, j]) * lenofpiece / (num - 1)) <= 50)
                    {
                        if (Math.Abs(curve[i, jizhi_location[i, j]]) >= Math.Abs(curve[i, jizhi_location[i, j + 1]]))
                        {
                            for (int t = jizhi_location[i, j]; t <= jizhi_location[i, j + 1]; t++)
                            {
                                defect[i, t] = curve[i, jizhi_location[i, j]];
                            }
                        }
                        if (Math.Abs(curve[i, jizhi_location[i, j]]) < Math.Abs(curve[i, jizhi_location[i, j + 1]]))
                        {
                            for (int t = jizhi_location[i, j]; t <= jizhi_location[i, j + 1]; t++)
                            {
                                defect[i, t] = curve[i, jizhi_location[i, j + 1]];
                            }
                        }
                    }
                }
            }
            return defect;
        }

        private double[,] defect_recognition_XplusZ(double[,] defect_recognition_X, double[,] defect_recognition_Z, long num)
        {
            double[,] defect_XplusZ = new double[4, 10000];
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < num; j++)
                {
                    defect_XplusZ[i, j] = 0;
                }
            }
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < num; j++)
                {
                    if (defect_recognition_X[i, j] != 0 && defect_recognition_Z[i, j] != 0)
                    {
                        defect_XplusZ[i, j] = defect_recognition_X[i, j];
                    }
                    //if (defect_recognition_Z[i, j] != 0)
                    //{
                    //    defect_XplusZ[i, j] = defect_recognition_Z[i, j];
                    //}
                }
            }
            return defect_XplusZ;
        }

        private double[,] defect_recognition_another(double[,] curve_X, double[,] curve_Z, long num)
        {
            double[,] defect_another = new double[4, 10000];
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < num; j++)
                {
                    defect_another[i, j] = 0;
                }
            }
            for (int i = 0; i < 4; i++)
            {
                for (int j = 2; j < num - 2; j++)
                {
                    if (curve_X[i, j] != 0)
                    {
                        if (curve_Z[i, j] != 0 || curve_Z[i, j - 1] != 0 || curve_Z[i, j - 2] != 0 || curve_Z[i, j + 1] != 0 || curve_Z[i, j + 2] != 0)
                        {
                            defect_another[i, j] = curve_X[i, j];
                        }
                    }
                }
            }
            return defect_another;
        }

        private double[] reconstruction(double[,] rebuild_XplusZ, double[,] rebuild_another, long num)
        {
            //double[,] reconstruction_data = new double[4, 10000];
            double[] reconstruction_feedback = new double[10000];
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < num; j++)
                {
                    reconstruction_data[i, j] = 0;
                }
            }
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < num; j++)
                {
                    if (rebuild_XplusZ[i, j] != 0)
                    {
                        reconstruction_data[i, j] = rebuild_XplusZ[i, j];
                    }
                    if (rebuild_another[i, j] != 0)
                    {
                        reconstruction_data[i, j] = rebuild_another[i, j];
                    }
                }
            }
            for (int j = 0; j < num; j++)
            {
                if (reconstruction_data[0, j] != 0)
                {
                    if (reconstruction_data[1, j] == 0 && reconstruction_data[2, j] == 0 && reconstruction_data[3, j] == 0)
                    {
                        reconstruction_data[0, j] = 0;
                    }
                }
                if (reconstruction_data[1, j] != 0)
                {
                    if (reconstruction_data[0, j] == 0 && reconstruction_data[2, j] == 0 && reconstruction_data[3, j] == 0)
                    {
                        reconstruction_data[1, j] = 0;
                    }
                }
                if (reconstruction_data[2, j] != 0)
                {
                    if (reconstruction_data[1, j] == 0 && reconstruction_data[0, j] == 0 && reconstruction_data[3, j] == 0)
                    {
                        reconstruction_data[2, j] = 0;
                    }
                }
                if (reconstruction_data[3, j] != 0)
                {
                    if (reconstruction_data[1, j] == 0 && reconstruction_data[2, j] == 0 && reconstruction_data[0, j] == 0)
                    {
                        reconstruction_data[3, j] = 0;
                    }
                }
            }
            for (int j = 0; j < num; j++)
            {
                if (reconstruction_data[0, j] != 0)
                {
                    reconstruction_feedback[j] = reconstruction_data[0, j];
                }
                if (reconstruction_data[1, j] != 0)
                {
                    reconstruction_feedback[j] = reconstruction_data[1, j];
                }
                if (reconstruction_data[2, j] != 0)
                {
                    reconstruction_feedback[j] = reconstruction_data[2, j];
                }
                if (reconstruction_data[3, j] != 0)
                {
                    reconstruction_feedback[j] = reconstruction_data[3, j];
                }
            }
            return reconstruction_feedback;

        }

        private double[] defect_recognition_2(double[] curve, long num)
        {
            int[] jizhi_location = new int[10000];
            double[] defect = new double[10000];
            defect_recognition_2_flag = 1;
            for (int j = 0; j < num; j++)
            {
                defect[j] = curve[j];
            }
            int k = 0;
            for (int j = 0; j < num - 2; j++)
            {
                if (curve[j + 1] != 0)
                {
                    if (curve[j] <= curve[j + 1] && curve[j + 1] >= curve[j + 2] || curve[j] >= curve[j + 1] && curve[j + 1] <= curve[j + 2])
                    {
                        jizhi_location[k] = j + 1;
                        k = k + 1;
                    }
                }
            }
            for (int j = 0; j < k - 1; j++)
            {
                if (((jizhi_location[j + 1] - jizhi_location[j]) * lenofpiece / (num - 1)) <= 100)
                {
                    defect_recognition_2_flag = 2;
                    defect[jizhi_location[j]] = curve[jizhi_location[j]];
                    for (int t = jizhi_location[j] + 1; t <= jizhi_location[j + 1]; t++)
                    {
                        defect[t] = 0;
                    }
                    break;
                }
            }
            return defect;
        }

        //private double[,] pic_2_defect_imagination(long num)
        //{
        //    double[,] defect_imagination = new double[4, 10000];
        //    for (int i = 0; i < 4; i++)
        //    {
        //        for (int j = 0; j < num; j++)
        //        {
        //            defect_imagination[i, j] = 0;
        //        }
        //        for (int j = 1; j < num; j++)
        //        {
        //            if (Math.Abs(defect_X[i, j]) > 0 && Math.Abs(defect_Z[i, j]) > 0)
        //            {
        //                defect_imagination[i, j - 1] = Math.Abs(defect_X[i, j]);
        //                defect_imagination[i, j] = Math.Abs(defect_X[i, j]);
        //                defect_imagination[i, j + 1] = Math.Abs(defect_X[i, j]);
        //            }
        //        }
        //    }
        //    return defect_imagination;
        //}

        //private double[,] pic_2_contour(long num)
        //{
        //    double[,] imagination = new double[8, 10000];
        //    for (int i = 0; i < 4; i++)
        //    {
        //        for (int j = 0; j < num; j++)
        //        {
        //            imagination[i, j] = 0;
        //        }
        //    }
        //    for (int j = 0; j < num; j++)
        //    {
        //        imagination[4, j] = reconstruction_data_display[j];
        //    }
        //    //for (int i = 4; i <5 ; i++)
        //    //{
        //    //    for (int j = 0; j < num; j++)
        //    //    {
        //    //        imagination[i, j] = reconstruction_data_display[j];
        //    //    }
        //    //}
        //    for (int i = 5; i < 8; i++)
        //    {
        //        for (int j = 0; j < num; j++)
        //        {
        //            imagination[i, j] = 0;
        //        }
        //    }
        //    return imagination;
        //}

        private double[,] pic_2_contour(long num)
        {
            double[,] imagination = new double[8, 10000];
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < num; j++)
                {
                    imagination[i, j] = 0;
                }
            }


            for (int j = 0; j < num; j++)
            {
                if (reconstruction_data_display[j] != 0)
                {
                    if (Math.Abs(original_signal_chafen2[4, j]) >= Math.Abs(original_signal_chafen2[5, j]) && Math.Abs(original_signal_chafen2[4, j]) >= Math.Abs(original_signal_chafen2[1, j]) && Math.Abs(original_signal_chafen2[4, j]) >= Math.Abs(original_signal_chafen2[7, j]) && Math.Abs(original_signal_chafen2[4, j]) >= Math.Abs(original_signal_chafen2[11, j]) && Math.Abs(original_signal_chafen2[4, j]) >= Math.Abs(original_signal_chafen2[10, j]))
                    {
                        imagination[1, j] = Math.Abs(original_signal_chafen2[4, j]);
                    }
                    //else
                    //{
                    //    imagination[1, j] = 0;
                    //}
                }
            }


            for (int j = 0; j < num; j++)
            {
                if (reconstruction_data_display[j] != 0)
                {
                    if (Math.Abs(original_signal_chafen2[5, j]) >= Math.Abs(original_signal_chafen2[4, j]) && Math.Abs(original_signal_chafen2[5, j]) >= Math.Abs(original_signal_chafen2[1, j]) && Math.Abs(original_signal_chafen2[5, j]) >= Math.Abs(original_signal_chafen2[7, j]) && Math.Abs(original_signal_chafen2[5, j]) >= Math.Abs(original_signal_chafen2[11, j]) && Math.Abs(original_signal_chafen2[5, j]) >= Math.Abs(original_signal_chafen2[10, j]))
                    {
                        imagination[2, j] = Math.Abs(original_signal_chafen2[5, j]);
                    }
                    //else
                    //{
                    //    imagination[2, j] = 0;
                    //}
                }
            }

            for (int j = 0; j < num; j++)
            {
                if (reconstruction_data_display[j] != 0)
                {
                    if (Math.Abs(original_signal_chafen2[1, j]) >= Math.Abs(original_signal_chafen2[4, j]) && Math.Abs(original_signal_chafen2[1, j]) >= Math.Abs(original_signal_chafen2[5, j]) && Math.Abs(original_signal_chafen2[1, j]) >= Math.Abs(original_signal_chafen2[7, j]) && Math.Abs(original_signal_chafen2[11, j]) >= Math.Abs(original_signal_chafen2[7, j]) && Math.Abs(original_signal_chafen2[1, j]) >= Math.Abs(original_signal_chafen2[10, j]))
                    {
                        imagination[3, j] = Math.Abs(original_signal_chafen2[1, j]);
                    }
                    //else
                    //{
                    //    imagination[3, j] = 0;
                    //}
                }
            }

            for (int j = 0; j < num; j++)
            {
                if (reconstruction_data_display[j] != 0)
                {
                    if (Math.Abs(original_signal_chafen2[7, j]) >= Math.Abs(original_signal_chafen2[4, j]) && Math.Abs(original_signal_chafen2[7, j]) >= Math.Abs(original_signal_chafen2[5, j]) && Math.Abs(original_signal_chafen2[7, j]) >= Math.Abs(original_signal_chafen2[1, j]) && Math.Abs(original_signal_chafen2[7, j]) >= Math.Abs(original_signal_chafen2[11, j]) && Math.Abs(original_signal_chafen2[7, j]) >= Math.Abs(original_signal_chafen2[10, j]))
                    {
                        imagination[4, j] = Math.Abs(original_signal_chafen2[7, j]);
                    }
                    //else
                    //{
                    //    imagination[4, j] = 0;
                    //}
                }
            }

            for (int j = 0; j < num; j++)
            {
                if (reconstruction_data_display[j] != 0)
                {
                    if (Math.Abs(original_signal_chafen2[11, j]) >= Math.Abs(original_signal_chafen2[4, j]) && Math.Abs(original_signal_chafen2[11, j]) >= Math.Abs(original_signal_chafen2[5, j]) && Math.Abs(original_signal_chafen2[11, j]) >= Math.Abs(original_signal_chafen2[1, j]) && Math.Abs(original_signal_chafen2[11, j]) >= Math.Abs(original_signal_chafen2[7, j]) && Math.Abs(original_signal_chafen2[11, j]) >= Math.Abs(original_signal_chafen2[10, j]))
                    {
                        imagination[5, j] = Math.Abs(original_signal_chafen2[11, j]);
                    }
                    //else
                    //{
                    //    imagination[5, j] = 0;
                    //}
                }
            }

            for (int j = 0; j < num; j++)
            {
                if (reconstruction_data_display[j] != 0)
                {
                    if (Math.Abs(original_signal_chafen2[10, j]) >= Math.Abs(original_signal_chafen2[4, j]) && Math.Abs(original_signal_chafen2[10, j]) >= Math.Abs(original_signal_chafen2[5, j]) && Math.Abs(original_signal_chafen2[10, j]) >= Math.Abs(original_signal_chafen2[1, j]) && Math.Abs(original_signal_chafen2[10, j]) >= Math.Abs(original_signal_chafen2[7, j]) && Math.Abs(original_signal_chafen2[10, j]) >= Math.Abs(original_signal_chafen2[11, j]))
                    {
                        imagination[6, j] = Math.Abs(original_signal_chafen2[10, j]);
                    }
                    //else
                    //{
                    //    imagination[6, j] = 0;
                    //}
                }
            }










            //for (int j = 0; j < num; j++)
            //{
            //    if (reconstruction_data_display[j] != 0)
            //    {
            //        if (Math.Abs(reconstruction_data[1, j]) >= Math.Abs(reconstruction_data[0, j]) && Math.Abs(reconstruction_data[1, j]) >= Math.Abs(reconstruction_data[2, j]) && Math.Abs(reconstruction_data[1, j]) >= Math.Abs(reconstruction_data[3, j]))
            //        {
            //            imagination[2, j] = reconstruction_data[1, j];
            //        }
            //        else
            //        {
            //            imagination[2, j] = 0;
            //        }
            //    }
            //}
            //for (int j = 0; j < num; j++)
            //{
            //    imagination[1, j] = 0;
            //}
            //for (int j = 0; j < num; j++)
            //{
            //    if (reconstruction_data_display[j] != 0)
            //    {
            //        if (Math.Abs(reconstruction_data[2, j]) >= Math.Abs(reconstruction_data[0, j]) && Math.Abs(reconstruction_data[2, j]) >= Math.Abs(reconstruction_data[1, j]) && Math.Abs(reconstruction_data[2, j]) >= Math.Abs(reconstruction_data[3, j]))
            //        {
            //            imagination[3, j] = reconstruction_data[2, j];
            //        }
            //        else
            //        {
            //            imagination[3, j] = 0;
            //        }
            //    }
            //}
            //for (int j = 0; j < num; j++)
            //{
            //    if (reconstruction_data_display[j] != 0)
            //    {
            //        if (Math.Abs(reconstruction_data[0, j]) >= Math.Abs(reconstruction_data[1, j]) && Math.Abs(reconstruction_data[0, j]) >= Math.Abs(reconstruction_data[2, j]) && Math.Abs(reconstruction_data[0, j]) >= Math.Abs(reconstruction_data[3, j]))
            //        {
            //            imagination[4, j] = reconstruction_data[0, j];
            //        }
            //        else
            //        {
            //            imagination[4, j] = 0;
            //        }
            //    }
            //}
            //for (int j = 0; j < num; j++)
            //{
            //    imagination[6, j] = 0;
            //}
            //for (int j = 0; j < num; j++)
            //{
            //    if (reconstruction_data_display[j] != 0)
            //    {
            //        if (Math.Abs(reconstruction_data[3, j]) >= Math.Abs(reconstruction_data[0, j]) && Math.Abs(reconstruction_data[3, j]) >= Math.Abs(reconstruction_data[1, j]) && Math.Abs(reconstruction_data[3, j]) >= Math.Abs(reconstruction_data[2, j]))
            //        {
            //            imagination[5, j] = reconstruction_data[3, j];
            //        }
            //        else
            //        {
            //            imagination[5, j] = 0;
            //        }
            //    }
            //}
            //for (int j = 0; j < num; j++)
            //{
            //    imagination[7, j] = 0;
            //}
            return imagination;
        }

        /********************************************************************************************************************************************************************/

        private void picContour()
        {
            Bitmap b1 = new Bitmap(pic.Width, pic.Height);
            Graphics g = Graphics.FromImage(b1);
            Pen blackPen = new Pen(Color.Black, 1);
            Brush brush = new SolidBrush(Color.Blue);
            Font font = new Font("Timers New Roman", 7, FontStyle.Regular);
            double x0 = 0.025;
            double y0 = 0.025;
            double W = 0.95;
            double H = 0.90;
            float x = (float)(pic.Width * x0);
            float y = (float)(pic.Height * y0);
            float width = (float)(pic.Width * W);
            float height = (float)(pic.Height * H);
            //画彩图
            //Contourf(g, (tongdao_num -1)* 10, original_num, cidao, original_num, (tongdao_num-1) * 10);
            //Contourf(g, (8 - 1) * 10, original_num-1, cidao_1, original_num-1, (8 - 1) * 10);


            Contourf(g, 320, (original_num - 1) * 5, cidao_1, (original_num - 1) * 5, 320);


            //Contourf(g, (6 - 1) * 10, original_num, cidao, original_num, (6 - 1) * 10); //现在是六个通道
            //画刻度
            g.DrawRectangle(blackPen, x, y, width, height);
            float x1, x2, y1, y2;
            //画X方向刻度
            for (int k = 0; k <= 10; k++)
            {
                x1 = (float)(pic.Width * x0 + k * pic.Width * W / 10);
                y1 = (float)(pic.Height * y0);
                x2 = (float)(pic.Width * x0 + k * pic.Width * W / 10);
                y2 = (float)(pic.Height * (y0 + 0.02));
                g.DrawLine(blackPen, x1, y1, x2, y2);
                x1 = (float)(pic.Width * x0 + k * pic.Width * W / 10);
                y1 = (float)(pic.Height * (x0 + H));
                x2 = (float)(pic.Width * x0 + k * pic.Width * W / 10);
                y2 = (float)(pic.Height * (x0 + H - 0.02));
                g.DrawLine(blackPen, x1, y1, x2, y2);
                x1 = (float)(pic.Width * x0 + k * pic.Width * W / 10);
                y1 = (float)(pic.Height * (y0 + H + 0.02));
                g.DrawString((k * lenofpiece / 10).ToString("0"), font, brush, x1, y1);
            }
            //画Y方向刻度
            for (int k = 0; k <= 5; k++)
            {
                x1 = (float)(pic.Width * x0);
                y1 = (float)(pic.Height * (1 - (y0 + k * H / 5) - 0.05));
                x2 = (float)(pic.Width * (x0 + 0.005));
                y2 = (float)(pic.Height * (1 - (y0 + k * H / 5) - 0.05));
                g.DrawLine(blackPen, x1, y1, x2, y2);
                x1 = (float)(pic.Width * (x0 + W));
                y1 = (float)(pic.Height * (1 - (y0 + k * H / 5) - 0.05));
                x2 = (float)(pic.Width * (x0 + W - 0.005));
                y2 = (float)(pic.Height * (1 - (y0 + k * H / 5) - 0.05));
                g.DrawLine(blackPen, x1, y1, x2, y2);
                x1 = (float)(pic.Width * (x0 - 0.02));
                y1 = (float)(pic.Height * (1 - (y0 + k * H / 5) - 0.05));
                //g.DrawString((k * widthOfPiece / 5).ToString("0.0"), font, brush, x1, y1);
                //if (yangguangleixin == 1)
                //{
                g.DrawString((k * 0.5 * widthOfPiece / 5).ToString("0.0"), font, brush, x1, y1); //变成一半的管道宽度
                //}
                //else if (yangguangleixin == 2)
                //{
                //    g.DrawString((k * widthOfPiece / 5).ToString("0.0"), font, brush, x1, y1);
                //}

            }
            pic.Image = b1;
            g.Dispose();
        }

        //检测结果矩形框画等值线
        //intM : 纵坐标点数
        //intN ：横坐标点数
        //结果彩图 Y方向Ymax=intM
        //        X方向Xmax=10000
        private void Contourf(System.Drawing.Graphics g, long intM, long intN, double[,] S, double Xmax, double Ymax)
        {
            int p;
            double Smax = -10000000;
            double Smin = 10000000;
            double x0 = 0.025;
            double y0 = 0.025;
            double W = 0.95;
            double H = 0.9;
            float CurrentX = 0;
            float CurrentY = 0;
            float DDX = 0;
            float DDY = 0;
            //找S最大、最小值
            for (long i = 0; i < intM; i++)
            {
                for (long j = 0; j < intN; j++)
                {
                    S[i, j] = Math.Abs(S[i, j]);
                    Smax = S[i, j] >= Smax ? S[i, j] : Smax;
                    Smin = S[i, j] <= Smin ? S[i, j] : Smin;
                }
            }
            Smax = Smax == 0 ? 0.001 : Smax;

            //画彩图
            picDX = W / Xmax;
            picDY = H / Ymax;
            //处理过多数据会导致此处循环千万次以上(耗时循环)
            for (long i = 0; i < intM; i++)
            {
                for (long j = 0; j < intN; j++)
                {
                    CurrentX = (float)(pic.Width * (x0 + j * picDX));
                    CurrentY = (float)(pic.Height * (1 - (y0 + 0.0625 + i * picDY)));
                    DDX = (float)(pic.Width * picDX);
                    DDY = (float)(pic.Height * picDY);
                    p = (int)((S[i, j] - Smin) * 63 / (Smax - Smin));
                    Brush brush = new SolidBrush(list_Color[p]);
                    g.FillRectangle(brush, CurrentX, CurrentY, DDX, DDY);
                }
            }
        }

        //pic鼠标移动取点坐标
        private void pic_MouseMove(object sender, MouseEventArgs e)
        {
            currentPoint.X = e.X;
            currentPoint.Y = e.Y;
            pic.Invalidate();
        }

        //缺陷定位按钮
        private void PointButton_Click(object sender, EventArgs e)
        {
            pointflag = true;
            ZtextBox.Enabled = true;
            Tslabel.Text = "请选取第一个点" + "\r\n" + "的x轴坐标";
        }

        //采数按钮
        private void buttonSend_Click(object sender, EventArgs e)
        {
            SendCommandbyte();
        }

        //发送采数命令
        private void SendCommandbyte()
        {
            serialPort.DiscardOutBuffer();
            serialPort.DiscardInBuffer();
            byte[] buf = new byte[4] { 0x23, 0x30, 0x30, 0xd };
            serialPort.Write(buf, 0, buf.Length);
        }

        private void zedGraphControl1_DoubleClick(object sender, EventArgs e)
        {
            DynamicForm DyForm1 = new DynamicForm(zedGraphControl1);
            DyForm1.Show();
        }

        private void zedGraphControl2_DoubleClick(object sender, EventArgs e)
        {
            zed2Form DyForm2 = new zed2Form(zedGraphControl2);
            DyForm2.Show();
        }

        private void zedGraphControl3_DoubleClick(object sender, EventArgs e)
        {
            zed2Form DyForm2 = new zed2Form(zedGraphControl3);
            DyForm2.Show();
        }

        private void zedGraphControl3_2_DoubleClick(object sender, EventArgs e)
        {
            zed2Form DyForm2 = new zed2Form(zedGraphControl3_2);
            DyForm2.Show();
        }

        private void zedGraphControl4_DoubleClick(object sender, EventArgs e)
        {
            zed2Form DyForm2 = new zed2Form(zedGraphControl4);
            DyForm2.Show();
        }

        private void zedGraphControl5_DoubleClick(object sender, EventArgs e)
        {
            zed2Form DyForm2 = new zed2Form(zedGraphControl5);
            DyForm2.Show();
        }

        //双击云图事件
        private void Dynamicpic_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                DynamicpicForm df = new DynamicpicForm(Dynamicpic);
                df.Show();
            }
            catch
            {

            }
        }

        private void pic_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                picForm df = new picForm(pic.Image);
                df.Show();
            }
            catch
            {

            }
        }

        private void pic_Paint(object sender, PaintEventArgs e)
        {
            //缺陷定位
            double cx = 0;
            double cy = 0;
            double cz = 0;
            double cm = 0; //腐蚀面积
            long ii = 0;
            long jj = 0;
            int i = 0;
            double x0 = 0.025;
            double y0 = 0.025;
            double W = 0.95;
            double H = 0.90;
            float x = (float)(pic.Width * x0);
            float y = (float)(pic.Height * y0);
            float width = (float)(pic.Width * W);
            float height = (float)(pic.Height * H);
            int n = 0;
            double C;
            lenofpiece = double.Parse(DisText.Text);
            double Sen = double.Parse(yztext.Text);//灵敏度
            tiligaodu = double.Parse(TLBox.Text);
            double D = double.Parse(DText.Text);

            //double max1 = 0;
            //double min1 = 0;
            //double max2 = 0;
            //double min2 = 0;
            //double max3 = 0;
            //double min3 = 0;
            //double max4 = 0;
            //double min4 = 0;

            //double up1=0;
            //double down1=0;
            //double up2=0;
            //double down2=0;
            //double up3=0;
            //double down3=0;
            //double up4=0;
            //double down4=0;

            //double value1=0;
            //double value2=0;
            //double value3=0;
            //double value4=0;

            double sum_1 = 0;
            double mean_1 = 0;
            double biaozhuncha_sum_1 = 0;
            double biaozhuncha_1 = 0;
            double[] ysdata_new_1 = new double[10000];
            double[] Dydata_new_1 = new double[10000];

            double sum_3 = 0;
            double mean_3 = 0;
            double biaozhuncha_sum_3 = 0;
            double biaozhuncha_3 = 0;
            double[] ysdata_new_3 = new double[10000];
            double[] Dydata_new_3 = new double[10000];

            double[] ysdata_new = new double[10000];
            double[] Dydata_new = new double[10000];

            System.Random rnd = new System.Random();
            if (runflag && pointflag && readflag)
            {
                Graphics gg = e.Graphics;
                cx = (currentPoint.X - x) * lenofpiece / width;
                cy = (pic.Height - currentPoint.Y - 0.075 * pic.Height) * widthOfPiece / height;

                n = Convert.ToInt16(Math.Abs(cx) / (lenofpiece / (original_num - 1)));
                C = ysdatanew[0, n] + ysdatanew[1, n] + ysdatanew[2, n] + ysdatanew[3, n];
                F = (ysdatanew[0, n] + ysdatanew[1, n] + ysdatanew[2, n] + ysdatanew[3, n]) / (4 * Math.Pow(tiligaodu * 0.000000001, 2));
                //cz=Math.Abs ( -1.0212 * F - 2.5295)+0.2*rnd.NextDouble ();

                ////////////////////////////////张量///////////////////////////////////////////
                //if (Math.Abs(Freaddata[0, n]) > 0)
                //{
                //    if (Math.Abs(Freaddata[0, n]) > 0 && Math.Abs(Freaddata[0, n]) <= 100)
                //    {
                //        F1 = (Freaddata[0, n] + 400) * 100;
                //    }
                //    if (Math.Abs(Freaddata[0, n]) > 100 && Math.Abs(Freaddata[0, n]) <= 200)
                //    {
                //        F1 = (Freaddata[0, n] + 300) * 100;
                //    }
                //    if (Math.Abs(Freaddata[0, n]) > 200 && Math.Abs(Freaddata[0, n]) <= 300)
                //    {
                //        F1 = (Freaddata[0, n] + 200) * 100;
                //    }
                //    if (Math.Abs(Freaddata[0, n]) > 300 && Math.Abs(Freaddata[0, n]) <= 400)
                //    {
                //        F1 = (Freaddata[0, n] + 300) * 100;
                //    }
                //    if (Math.Abs(Freaddata[0, n]) > 400 && Math.Abs(Freaddata[0, n]) <= 500)
                //    {
                //        F1 = Freaddata[0, n] * 100;
                //    }
                //    if (Math.Abs(Freaddata[0, n]) > 500 && Math.Abs(Freaddata[0, n]) <= 600)
                //    {
                //        F1 = (Freaddata[0, n] - 100) * 100;
                //    }
                //    if (Math.Abs(Freaddata[0, n]) > 0 && Math.Abs(Freaddata[0, n]) <= 100)
                //    {
                //        F1 = (Freaddata[0, n] + 300) * 100;
                //    }
                //}
                //////////////////////////////////////////////////////////////////////////////
                /*********************************************2016.5.10改腐蚀深度定量********************************************/
                ///////////归一化////////////////////////////
                for (int j = 0; j < original_num - 1; j++)
                {
                    sum_1 = sum_1 + ysdata[2, j];
                }
                mean_1 = sum_1 / (original_num - 1);
                for (int j = 0; j < original_num - 1; j++)
                {
                    biaozhuncha_sum_1 = biaozhuncha_sum_1 + Math.Pow((ysdata[2, j] - mean_1), 2);
                }
                biaozhuncha_1 = Math.Sqrt(biaozhuncha_sum_1 / (original_num - 1));
                for (int j = 0; j < original_num - 1; j++)
                {
                    ysdata_new_1[j] = (ysdata[2, j] - mean_1) / biaozhuncha_1;
                }

                for (int j = 0; j < original_num - 1; j++)
                {
                    sum_3 = sum_3 + ysdata[8, j];
                }
                mean_3 = sum_3 / (original_num - 1);
                for (int j = 0; j < original_num - 1; j++)
                {
                    biaozhuncha_sum_3 = biaozhuncha_sum_3 + Math.Pow((ysdata[8, j] - mean_3), 2);
                }
                biaozhuncha_3 = Math.Sqrt(biaozhuncha_sum_3 / (original_num - 1));
                for (int j = 0; j < original_num - 1; j++)
                {
                    ysdata_new_3[j] = (ysdata[8, j] - mean_3) / biaozhuncha_3;
                }

                for (int j = 0; j < original_num - 1; j++)
                {
                    ysdata_new[j] = (ysdata_new_1[j] + ysdata_new_3[j]) / 2;
                }


                if ((final_imagination[0, n] == 0) && (final_imagination[1, n] == 0) && (final_imagination[2, n] == 0) && (final_imagination[3, n] == 0) && (final_imagination[4, n] == 0) && (final_imagination[5, n] == 0) && (final_imagination[6, n] == 0) && (final_imagination[7, n] == 0))
                {
                    cz = 0;
                }

                if (bdxstext1.Text == "")
                {
                    double fushi_canshu_a = 0;
                    double fushi_canshu_b = 0;

                    int TLBox1 = Int32.Parse(TLBox.Text);
                    if (tiligaodu >= 0 && tiligaodu < 250)
                    {
                        fushi_canshu_a = 9.45E+01;
                        fushi_canshu_b = -1.346;

                    }
                    if (tiligaodu >= 250 && tiligaodu < 480)
                    {
                        fushi_canshu_a = 120.6;
                        fushi_canshu_b = -1.385;
                    }
                    if (tiligaodu >= 480 && tiligaodu < 700)
                    {
                        fushi_canshu_a = 61.12;
                        fushi_canshu_b = -1.027;
                    }
                    if (tiligaodu >= 700 && tiligaodu < 970)
                    {
                        fushi_canshu_a = 260.2;
                        fushi_canshu_b = -1.5;
                    }
                    if (tiligaodu >= 970 && tiligaodu < 1100)
                    {
                        fushi_canshu_a = 46.3;
                        fushi_canshu_b = -1.054;
                    }
                    if (tiligaodu >= 1100 && tiligaodu < 1350)
                    {
                        fushi_canshu_a = 1.05E+06;
                        fushi_canshu_b = -9.415;
                    }
                    if (tiligaodu >= 1350 && tiligaodu < 1620)
                    {
                        fushi_canshu_a = 10.5;
                        fushi_canshu_b = -1.584;
                    }
                    if (tiligaodu >= 1620 && tiligaodu < 1860)
                    {
                        fushi_canshu_a = 784.8;
                        fushi_canshu_b = -3.046;
                    }
                    if (tiligaodu > 1860)
                    {
                        fushi_canshu_a = 28.22;
                        fushi_canshu_b = -1.784;
                    }
                    //cz = (40 * Math.Exp(-1.5 * F1) + 40 * Math.Exp(-1.5 * F2) + 40 * Math.Exp(-1.5 * F3) + 40 * Math.Exp(-1.5 * F4)) / 4;
                    // cz = 0.3102 * Math.Exp(1.8 * 1.8 * 0.7031 * Dydata_new[n]) + 0.6342;
                    cz = ThickOftube * fushi_canshu_a * Math.Exp(fushi_canshu_b * ysdata_new_1[n]) / 100;
                    if (cz > ThickOftube)
                    {
                        cz = ThickOftube * 0.9 + 0.5 * rnd.NextDouble();
                    }
                    if ((final_imagination[0, n] == 0) && (final_imagination[1, n] == 0) && (final_imagination[2, n] == 0) && (final_imagination[3, n] == 0) && (final_imagination[4, n] == 0) && (final_imagination[5, n] == 0) && (final_imagination[6, n] == 0) && (final_imagination[7, n] == 0))
                    {
                        cz = 0;
                    }

                }
                else
                {
                    double huiguixishu11 = Convert.ToDouble(huiguixishu1);
                    double huiguixishu22 = Convert.ToDouble(huiguixishu2);

                    cz = ThickOftube * huiguixishu11 * Math.Exp(huiguixishu22 * ysdata_new_1[n]) / 100;
                    if (cz > ThickOftube)
                    {
                        cz = ThickOftube * 0.8 + 0.5 * rnd.NextDouble();
                    }
                    if (cz == 0)
                    {
                        cz = ThickOftube * 0.2 + 0.5 * rnd.NextDouble();
                    }

                    if ((final_imagination[0, n] == 0) && (final_imagination[1, n] == 0) && (final_imagination[2, n] == 0) && (final_imagination[3, n] == 0) && (final_imagination[4, n] == 0) && (final_imagination[5, n] == 0) && (final_imagination[6, n] == 0) && (final_imagination[7, n] == 0))
                    {
                        cz = 0;
                    }
                }
                //else 
                //{
                //    if ((D <= 100) && (D > 0))
                //    {
                //        cz = (2.182 * 0.00000001 * Math.Exp(3.009 * F1) + 1.92 * 0.00000001 * Math.Exp(3.014 * F2) + 0.9927 * 0.00000001 * Math.Exp(3.132 * F3) + 2.978 * 0.00000001 * Math.Exp(3.017 * F4)) / 4;
                //    }
                //    if ((D > 100) && (D <= 200))
                //    {
                //        cz = (3.826 * Math.Pow(10, -5) * Math.Exp(1.762 * F1) + 4.291 * Math.Pow(10, -5) * Math.Exp(1.735 * F2) + 3.799 * Math.Pow(10, -5) * Math.Exp(1.767 * F3) + 3.505 * Math.Pow(10, -5) * Math.Exp(1.809 * F4)) / 4;
                //    }
                //    if ((D > 200) && (D <= 300))
                //    {
                //        cz = (1.04 * Math.Pow(10, -7) * Math.Exp(3.039 * F1) + 0.9371 * Math.Pow(10, -7) * Math.Exp(3.044 * F2) + 1.083 * Math.Pow(10, -7) * Math.Exp(3.04 * F3) + 1.239 * Math.Pow(10, -7) * Math.Exp(3.067 * F4)) / 4;
                //    }
                //    if ((D > 300) && (D <= 400))
                //    {
                //        cz = (7.303 * Math.Pow(10, 4) * Math.Exp(-1.941 * F1) + 7.271 * Math.Pow(10, 4) * Math.Exp(-1.932 * F2) + 7.417 * Math.Pow(10, 4) * Math.Exp(-1.948 * F3) + 7.482 * Math.Pow(10, 4) * Math.Exp(-1.982 * F4)) / 4;
                //    }
                //    if ((D > 400) && (D <= 500))
                //    {
                //        cz = (8.494 * Math.Pow(10, 31) * Math.Exp(-10.56 * F1) + 3.215 * Math.Pow(10, 30) * Math.Exp(-10.06 * F2) + 1.375 * Math.Pow(10, 32) * Math.Exp(-10.68 * F3) + 6.185 * Math.Pow(10, 33) * Math.Exp(-11.41 * F4)) / 4;
                //    }
                //    if (D > 500)
                //    {
                //        cz = (1.03 * Math.Pow(10, 21) * Math.Exp(-4.3 * F1) + 5.897 * Math.Pow(10, 20) * Math.Exp(-4.241 * F2) + 1.019 * Math.Pow(10, 21) * Math.Exp(-4.317 * F3) + 2.076 * Math.Pow(10, 21) * Math.Exp(-4.44 * F4)) / 4;
                //    }
                //    if (cz > ThickOftube)
                //    {
                //        cz = ThickOftube * 0.9 + 0.5 * rnd.NextDouble();
                //    }

                //}
                //else
                //{
                //    if ((D <= 100) && (D > 0))
                //    {
                //        cz = (2.468 * Math.Exp(0.8385 * F1) + 2.062 * Math.Exp(1.111 * F2) + 2.113 * Math.Exp(1.079 * F3) + 2.588 * Math.Exp(0.7795 * F4)) / 4;
                //    }
                //    if ((D > 100) && (D <= 200))
                //    {
                //        cz = (3.515 * Math.Exp(1.97 * F1) + 3.289 * Math.Exp(1.97 * F2) + 3.401 * Math.Exp(1.98 * F3) + 3.499 * Math.Exp(2.07 * F4)) / 4;
                //    }
                //    if ((D > 200) && (D <= 300))
                //    {
                //        cz = (1.904 * Math.Exp(3.042 * F1) + 2.177 * Math.Exp(3.013 * F2) + 1.818 * Math.Exp(3.057 * F3) + 1.514 * Math.Exp(3.132 * F4)) / 4;
                //    }
                //    if ((D > 300) && (D <= 400))
                //    {
                //        cz = (4.457 * Math.Exp(1.715 * F1) + 4.64 * Math.Exp(1.714 * F2) + 4.605 * Math.Exp(1.723 * F3) + 4.439 * Math.Exp(1.745 * F4)) / 4;
                //    }
                //    if ((D > 400) && (D <= 500))
                //    {
                //        cz = (4.4663 * Math.Exp(2.509 * F1) + 1.952 * Math.Exp(2.37 * F2) + 7.502 * Math.Exp(2.593 * F3) + 2.372 * Math.Exp(2.813 * F4)) / 4;
                //    }
                //    if (D > 500)
                //    {
                //        cz = (1.188 * Math.Exp(3.4 * F1) + 2.677 * Math.Exp(3.2 * F2) + 1.54 * Math.Exp(3.4 * F3) + 1.38 * Math.Exp(3.8 * F4)) / 4;
                //    }
                //    if (cz > ThickOftube)
                //    {
                //        cz = ThickOftube * 0.9 + 0.5 * rnd.NextDouble();
                //    }
                //}

                gg.DrawLine(Pens.White, x, currentPoint.Y, x + width, currentPoint.Y); //绘制横线  
                gg.DrawLine(Pens.White, currentPoint.X, y, currentPoint.X, y + height); //会制纵线 
                XtextBox.Text = string.Format("{0:0}", cx) + "mm";
                YtextBox.Text = string.Format("{0:0}", cy) + "mm";
                ZtextBox.Text = string.Format("{0:0.00}", cz) + "mm";

                ////计算区域面积
                if (cishu == 4)
                {
                    double bianchang = Math.Abs(x1 - x2) * lenofpiece / width;  //边长
                    double kuandu = Math.Abs(y1 - y2) * widthOfPiece / height;   //宽度
                    double area = bianchang * kuandu / 100;    //面积(平方厘米)
                    MjtextBox.Text = string.Format("{0:0.0}", area) + "cm²";
                    //MjtextBox.Text = Convert .ToString (area);
                }
                /////
            }
            else
            {
                Graphics gg = e.Graphics;
                cx = (currentPoint.X - x) * lenofpiece / width;
                cy = (pic.Height - currentPoint.Y - 0.075 * pic.Height) * widthOfPiece / height;

                n = Convert.ToInt16(Math.Abs(cx) / (lenofpiece / (original_num - 1)));
                C = ysdatanew[0, n] + ysdatanew[1, n] + ysdatanew[2, n] + ysdatanew[3, n];
                F = (ysdatanew[0, n] + ysdatanew[1, n] + ysdatanew[2, n] + ysdatanew[3, n]) / (4 * Math.Pow(tiligaodu * 0.000000001, 2));
                //cz=Math.Abs ( -1.0212 * F - 2.5295)+0.2*rnd.NextDouble ();
                //max1 = Dydata2[2, 0];
                //min1 = Dydata2[2, 0];
                //max2 = Dydata2[5, 0];
                //min2 = Dydata2[5, 0];
                //max3 = Dydata2[8, 0];
                //min3 = Dydata2[8, 0];
                //max4 = Dydata2[11, 0];
                //min4 = Dydata2[11, 0];
                //for (i = 0; i < original_num - 1; i++)
                //{
                //    if (max1 < Dydata2[2, i + 1])
                //    {
                //        max1 = Dydata2[2, i + 1];
                //    }
                //    if (min1 > Dydata2[2, i + 1])
                //    {
                //        min1 = Dydata2[2, i + 1];
                //    }
                //}
                //for (i = 0; i < original_num - 1; i++)
                //{
                //    if (max2 < Dydata2[5, i + 1])
                //    {
                //        max2 = Dydata2[5, i + 1];
                //    }
                //    if (min2 > Dydata2[5, i + 1])
                //    {
                //        min2 = Dydata2[5, i + 1];
                //    }
                //}
                //for (i = 0; i < original_num - 1; i++)
                //{
                //    if (max3 < Dydata2[8, i + 1])
                //    {
                //        max3 = Dydata2[8, i + 1];
                //    }
                //    if (min3 > Dydata2[8, i + 1])
                //    {
                //        min3 = Dydata2[8, i + 1];
                //    }
                //}
                //for (i = 0; i < original_num - 1; i++)
                //{
                //    if (max4 < Dydata2[11, i + 1])
                //    {
                //        max4 = Dydata2[11, i + 1];
                //    }
                //    if (min4 > Dydata2[11, i + 1])
                //    {
                //        min4 = Dydata2[11, i + 1];
                //    }
                //}

                //up1 = ((max1 / 1000) + 1) * 1000;
                //down1 = ((min1 / 1000) - 1) * 1000;
                //value1 = (((Dydata2[2, n] - down1) / (up1 - down1)) * 10000) + 10000;

                //up2 = ((max2 / 1000) + 1) * 1000;
                //down2 = ((min2 / 1000) - 1) * 1000;
                //value2 = (((Dydata2[5, n] - down2) / (up2 - down2)) * 10000) + 10000;

                //up3 = ((max3 / 1000) + 1) * 1000;
                //down3 = ((min3 / 1000) - 1) * 1000;
                //value3 = (((Dydata2[8, n] - down3) / (up3 - down3)) * 10000) + 10000;

                //up4 = ((max4 / 1000) + 1) * 1000;
                //down4 = ((min4 / 1000) - 1) * 1000;
                //value4 = (((Dydata2[11, n] - down4) / (up4 - down4)) * 10000) + 10000;

                //F1 = Math.Pow(150, 2) / value1;
                //F2 = Math.Pow(150, 2) / value2;
                //F3 = Math.Pow(150, 2) / value3;
                //F4 = Math.Pow(150, 2) / value4;
                /*********************************************2016.5.10改腐蚀深度定量********************************************/
                ///////////归一化////////////////////////////
                for (int j = 0; j < original_num - 1; j++)
                {
                    sum_1 = sum_1 + Dydata2[2, j];
                }
                mean_1 = sum_1 / (original_num - 1);
                for (int j = 0; j < original_num - 1; j++)
                {
                    biaozhuncha_sum_1 = biaozhuncha_sum_1 + Math.Pow((Dydata2[2, j] - mean_1), 2);
                }
                biaozhuncha_1 = Math.Sqrt(biaozhuncha_sum_1 / (original_num - 1));
                for (int j = 0; j < original_num - 1; j++)
                {
                    Dydata_new_1[j] = (Dydata2[2, j] - mean_1) / biaozhuncha_1;
                }

                for (int j = 0; j < original_num - 1; j++)
                {
                    sum_3 = sum_3 + Dydata2[8, j];
                }
                mean_3 = sum_3 / (original_num - 1);
                for (int j = 0; j < original_num - 1; j++)
                {
                    biaozhuncha_sum_3 = biaozhuncha_sum_3 + Math.Pow((Dydata2[8, j] - mean_3), 2);
                }
                biaozhuncha_3 = Math.Sqrt(biaozhuncha_sum_3 / (original_num - 1));
                for (int j = 0; j < original_num - 1; j++)
                {
                    Dydata_new_3[j] = (Dydata2[8, j] - mean_3) / biaozhuncha_3;
                }

                for (int j = 0; j < original_num - 1; j++)
                {
                    Dydata_new[j] = (Dydata_new_1[j] + Dydata_new_3[j]) / 2;
                }
                ////////////////////////////////////////////
                if ((final_imagination[0, n] == 0) && (final_imagination[1, n] == 0) && (final_imagination[2, n] == 0) && (final_imagination[3, n] == 0) && (final_imagination[4, n] == 0) && (final_imagination[5, n] == 0) && (final_imagination[6, n] == 0) && (final_imagination[7, n] == 0))
                {
                    cz = 0;
                }
                if (bdxstext1.Text == "")
                {
                    double fushi_canshu_a = 0;
                    double fushi_canshu_b = 0;

                    int TLBox1 = Int32.Parse(TLBox.Text);
                    if (tiligaodu >= 0 && tiligaodu < 250)
                    {
                        fushi_canshu_a = 9.45E+01;
                        fushi_canshu_b = -1.346;

                    }
                    if (tiligaodu >= 250 && tiligaodu < 480)
                    {
                        fushi_canshu_a = 120.6;
                        fushi_canshu_b = -1.385;
                    }
                    if (tiligaodu >= 480 && tiligaodu < 700)
                    {
                        fushi_canshu_a = 61.12;
                        fushi_canshu_b = -1.027;
                    }
                    if (tiligaodu >= 700 && tiligaodu < 970)
                    {
                        fushi_canshu_a = 260.2;
                        fushi_canshu_b = -1.5;
                    }
                    if (tiligaodu >= 970 && tiligaodu < 1100)
                    {
                        fushi_canshu_a = 46.3;
                        fushi_canshu_b = -1.054;
                    }
                    if (tiligaodu >= 1100 && tiligaodu < 1350)
                    {
                        fushi_canshu_a = 1.05E+06;
                        fushi_canshu_b = -9.415;
                    }
                    if (tiligaodu >= 1350 && tiligaodu < 1620)
                    {
                        fushi_canshu_a = 10.5;
                        fushi_canshu_b = -1.584;
                    }
                    if (tiligaodu >= 1620 && tiligaodu < 1860)
                    {
                        fushi_canshu_a = 784.8;
                        fushi_canshu_b = -3.046;
                    }
                    if (tiligaodu > 1860)
                    {
                        fushi_canshu_a = 28.22;
                        fushi_canshu_b = -1.784;
                    }
                    //cz = (40 * Math.Exp(-1.5 * F1) + 40 * Math.Exp(-1.5 * F2) + 40 * Math.Exp(-1.5 * F3) + 40 * Math.Exp(-1.5 * F4)) / 4;
                    // cz = 0.3102 * Math.Exp(1.8 * 1.8 * 0.7031 * Dydata_new[n]) + 0.6342;
                    cz = ThickOftube * fushi_canshu_a * Math.Exp(fushi_canshu_b * Dydata_new_1[n]) / 100;
                    if (cz > ThickOftube)
                    {
                        cz = ThickOftube * 0.9 + 0.5 * rnd.NextDouble();
                    }
                    if ((final_imagination[0, n] == 0) && (final_imagination[1, n] == 0) && (final_imagination[2, n] == 0) && (final_imagination[3, n] == 0) && (final_imagination[4, n] == 0) && (final_imagination[5, n] == 0) && (final_imagination[6, n] == 0) && (final_imagination[7, n] == 0))
                    {
                        cz = 0;
                    }
                }
                else
                {
                    double huiguixishu11 = Convert.ToDouble(huiguixishu1);
                    double huiguixishu22 = Convert.ToDouble(huiguixishu2);

                    cz = ThickOftube * huiguixishu11 * Math.Exp(huiguixishu22 * ysdata_new_1[n]) / 100;
                    if (cz > ThickOftube)
                    {
                        cz = ThickOftube * 0.8 + 0.5 * rnd.NextDouble();
                    }
                    if (cz == 0)
                    {
                        cz = ThickOftube * 0.2 + 0.5 * rnd.NextDouble();
                    }
                    if ((final_imagination[0, n] == 0) && (final_imagination[1, n] == 0) && (final_imagination[2, n] == 0) && (final_imagination[3, n] == 0) && (final_imagination[4, n] == 0) && (final_imagination[5, n] == 0) && (final_imagination[6, n] == 0) && (final_imagination[7, n] == 0))
                    {
                        cz = 0;
                    }

                }
                gg.DrawLine(Pens.White, x, currentPoint.Y, x + width, currentPoint.Y); //绘制横线  
                gg.DrawLine(Pens.White, currentPoint.X, y, currentPoint.X, y + height); //会制纵线 
                XtextBox.Text = string.Format("{0:0}", cx) + "mm";
                YtextBox.Text = string.Format("{0:0}", cy) + "mm";
                ZtextBox.Text = string.Format("{0:0.00}", cz) + "mm";

                ////计算区域面积
                if (cishu == 4)
                {
                    double bianchang = Math.Abs(x1 - x2) * lenofpiece / width;  //边长
                    double kuandu = Math.Abs(y1 - y2) * widthOfPiece / height;   //宽度
                    double area = bianchang * kuandu / 100;    //面积(平方厘米)
                    MjtextBox.Text = string.Format("{0:0.0}", area) + "cm²";
                    //MjtextBox.Text = Convert .ToString (area);
                }
            }
        }

        //private void WcomboBox_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    DXMAX = double.Parse(WcomboBox.Text);
        //    picXMAX = DXMAX;           
        //    DXMAXStep = DXMAX / 10;
        //    ZedGraphInit();       //曲线图初始化
        //}

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            udpClient.Close();
            //Application.Exit(); //程序退出
        }

        //获取单击的点x,y坐标位置
        private double x1 = 0;
        private double x2 = 0;
        private double y1 = 0;
        private double y2 = 0;

        private double cishu = 0;  //统计单击的次数
        private void pic_MouseClick(object sender, MouseEventArgs e)
        {
            if (cishu >= 4)  //即当超过4次后又重新计算次数
            {
                cishu = 0;
            }

            cishu++;

            if (cishu == 1)   //第一次取点
            {
                x1 = currentPoint.X;
                Tslabel.Text = "请选取第二个点" + "\r\n" + "的x轴坐标";
            }
            else if (cishu == 2)
            {
                x2 = currentPoint.X;
                Tslabel.Text = "请选取第三个点" + "\r\n" + "的y轴坐标";
            }
            else if (cishu == 3)
            {
                y1 = currentPoint.Y;
                Tslabel.Text = "请选取第四个点" + "\r\n" + "的y轴坐标";
            }
            else if (cishu == 4)
            {
                y2 = currentPoint.Y;
                Tslabel.Text = "请选取第一个点" + "\r\n" + "的x轴坐标";
            }


        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        private void keyboard_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("osk.exe");

        }

        //////
        Rectangle rec1 = new Rectangle(618, 74, 75, 23);  //之前button1的位置，控制da que xian ban yuan guan (位置改变时要做相应修改)
        Rectangle rec2 = new Rectangle(618, 99, 75, 23);  //之前button2的位置，控制xiao que xian ban yuan guan  

        Rectangle rec3 = new Rectangle(730, 7, 75, 23);  //qu chu chang du jia 100
        Rectangle rec4 = new Rectangle(730, 34, 75, 23);  //gui wei

        Rectangle rec5 = new Rectangle(691, 63, 53, 23);  // yvzhi bian
        Rectangle rec6 = new Rectangle(743, 63, 53, 23);  // yvzhi gui

        Rectangle rec7 = new Rectangle(803, 35, 82, 10);  // diejia7mm chu xian
        Rectangle rec8 = new Rectangle(803, 76, 82, 10);  // diejia7mm fei hu

        private void panel3_MouseDown(object sender, MouseEventArgs e)
        {
            if (rec1.Contains(e.Location))
            {
                yangguangcheck = 1;

            }
            else if (rec2.Contains(e.Location))
            {
                yangguangcheck = 2;

            }
            else if (rec3.Contains(e.Location))
            {

                qccd = qccd + 100;
                qccd1 = qccd1 - 100;

            }
            else if (rec4.Contains(e.Location))
            {
                qccd = double.Parse(qccdtext.Text);
                qccd1 = 450;
            }
            else if (rec5.Contains(e.Location))
            {
                yzzhenliang = 0;  //增大

            }
            else if (rec6.Contains(e.Location))
            {

                yzzhenliang = 0;  //减小

            }
            else if (rec7.Contains(e.Location))
            {
                diejia7mm = 1;
            }
            else if (rec8.Contains(e.Location))
            {
                diejia7mm = 0;

            }

        }

        private void battery_power_Click(object sender, EventArgs e)
        {
            power_Form pForm = new power_Form();
            pForm.Show();
            status1 = SusiLibInitialize();
            if (status1 == SUSI_STATUS_SUCCESS)
            {
                //textBox1.Text = "success";
                //InitializeSMB();
                status2 = SusiBoardGetValue(SUSI_ID_SMBUS_SUPPORTED, ref Value);
                if (status2 == SUSI_STATUS_SUCCESS)
                {
                    addr = Convert.ToByte("16", 16);
                    cmd = Convert.ToByte("0d", 16);
                    length = Convert.ToUInt32("2");
                    DataBlock = new byte[length];
                    status3 = SusiSMBI2CReadBlock(SUSI_ID_SMBUS_EXTERNAL, addr, cmd, DataBlock, length);
                    if (status3 == SUSI_STATUS_SUCCESS)
                    {
                        sb = new StringBuilder();
                        for (int i = 0; i < length; i++)
                        {
                            sb.Append(string.Format("{0:X2}", DataBlock[1 - i]));
                        }
                        // textBox1.Text = sb.ToString();
                        battery = Convert.ToByte(sb.ToString(), 16);
                        pForm.power_percentage.Text = string.Format("{0:0}", battery) + "%";
                    }
                }
            }
            pForm.progressBar1.Minimum = 0;
            pForm.progressBar1.Maximum = 100;
            pForm.progressBar1.Value = Convert.ToInt32(battery);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            status1_voltage = SusiLibInitialize();
            if (status1_voltage == SUSI_STATUS_SUCCESS)
            {
                //textBox1.Text = "success";
                //InitializeSMB();
                status2_voltage = SusiBoardGetValue(SUSI_ID_SMBUS_SUPPORTED, ref Value_voltage);
                if (status2_voltage == SUSI_STATUS_SUCCESS)
                {
                    addr_voltage = Convert.ToByte("16", 16);
                    cmd_voltage = Convert.ToByte("09", 16);
                    length_voltage = Convert.ToUInt32("2");
                    DataBlock_voltage = new byte[length_voltage];
                    status3_voltage = SusiSMBI2CReadBlock(SUSI_ID_SMBUS_EXTERNAL, addr_voltage, cmd_voltage, DataBlock_voltage, length_voltage);
                    if (status3_voltage == SUSI_STATUS_SUCCESS)
                    {
                        sb_voltage = new StringBuilder();
                        for (int i = 0; i < length_voltage; i++)
                        {
                            sb_voltage.Append(string.Format("{0:X2}", DataBlock_voltage[1 - i]));
                        }
                        // textBox1.Text = sb.ToString();
                        voltage = Convert.ToUInt32(sb_voltage.ToString(), 16);
                        if (voltage <= 9500)
                        {
                            MessageBox.Show("电池电压过低，为避免电池过放，请关机！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        //pForm.power_percentage.Text = string.Format("{0:0}", voltage) + "%";
                    }
                }
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            status1_2 = SusiLibInitialize();
            if (status1_2 == SUSI_STATUS_SUCCESS)
            {
                //textBox1.Text = "success";
                //InitializeSMB();
                status2_2 = SusiBoardGetValue(SUSI_ID_SMBUS_SUPPORTED, ref Value_2);
                if (status2_2 == SUSI_STATUS_SUCCESS)
                {
                    addr_2 = Convert.ToByte("16", 16);
                    cmd_2 = Convert.ToByte("0d", 16);
                    length_2 = Convert.ToUInt32("2");
                    DataBlock_2 = new byte[length_2];
                    status3_2 = SusiSMBI2CReadBlock(SUSI_ID_SMBUS_EXTERNAL, addr_2, cmd_2, DataBlock_2, length_2);
                    if (status3_2 == SUSI_STATUS_SUCCESS)
                    {
                        sb_2 = new StringBuilder();
                        for (int i = 0; i < length_2; i++)
                        {
                            sb_2.Append(string.Format("{0:X2}", DataBlock_2[1 - i]));
                        }
                        // textBox1.Text = sb.ToString();
                        battery_2 = Convert.ToByte(sb_2.ToString(), 16);
                        if (battery_2 <= 10)
                        {
                            MessageBox.Show("电量低于10%，请尽快保存数据并关机！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
        }

        //缺陷位置信息
        double[] qxlocation = new double[5];
        double[] qxdian = new double[5];
        double[] biaodingx = new double[5];

        //缺陷深度信息
        double[] qxsd = new double[5];
        double[] qxsd1 = new double[5];

        //标定系数
        double[] huiguixishu = new double[3];
        string huiguixishu1;
        string huiguixishu2;
        string huiguixishu3;

        //缺陷点的变量x
        double[] qxx = new double[5];
        double[] qxx1 = new double[5];
        //缺陷点的变量y
        double[] qxy = new double[5];

        private double a = 0;
        private double b = 0;

        double[] qxsd2 = new double[5];

        private double distext;

        private void biaodingbutton_Click(object sender, EventArgs e)
        {
            int NodeNum = 5;
            if (qxwztext1.Text == "")
            {
                MessageBox.Show("可用于标定的缺陷少于标定需要的数量！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (qxwztext2.Text == "")
            {
                MessageBox.Show("可用于标定的缺陷少于标定需要的数量！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (qxwztext3.Text == "")
            {
                MessageBox.Show("可用于标定的缺陷少于标定需要的数量", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (qxwztext4.Text == "")
            {
                MessageBox.Show("可用于标定的缺陷少于标定需要的数量", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (qxwztext5.Text == "")
            {

                NodeNum = 4;

                qxlocation[0] = double.Parse(qxwztext1.Text);
                qxlocation[1] = double.Parse(qxwztext2.Text);
                qxlocation[2] = double.Parse(qxwztext3.Text);
                qxlocation[3] = double.Parse(qxwztext4.Text);


                qxsd1[0] = double.Parse(sddltext1.Text);
                qxsd1[1] = double.Parse(sddltext2.Text);
                qxsd1[2] = double.Parse(sddltext3.Text);
                qxsd1[3] = double.Parse(sddltext4.Text);

                //找到输入的缺陷在点的位置
                distext = double.Parse(DisText.Text);
                qxdian[0] = (int)Math.Round(qxlocation[0] * original_num / distext);
                qxdian[1] = (int)Math.Round(qxlocation[1] * original_num / distext);
                qxdian[2] = (int)Math.Round(qxlocation[2] * original_num / distext);
                qxdian[3] = (int)Math.Round(qxlocation[3] * original_num / distext);






                /////////////////////////////////////////////////////////归一化///////////////////////////////////////////////////
                double x0 = 0.025;
                double y0 = 0.025;
                double W = 0.95;
                double H = 0.90;
                float x = (float)(pic.Width * x0);
                float y = (float)(pic.Height * y0);
                float width = (float)(pic.Width * W);
                float height = (float)(pic.Height * H);
                int n = 0;
                double C;
                lenofpiece = double.Parse(DisText.Text);
                double Sen = double.Parse(yztext.Text);//灵敏度
                tiligaodu = double.Parse(TLBox.Text);
                double D = double.Parse(DText.Text);

                double sum_1 = 0;
                double mean_1 = 0;
                double biaozhuncha_sum_1 = 0;
                double biaozhuncha_1 = 0;
                double[] ysdata_new_1 = new double[10000];
                double[] Dydata_new_1 = new double[10000];

                double sum_3 = 0;
                double mean_3 = 0;
                double biaozhuncha_sum_3 = 0;
                double biaozhuncha_3 = 0;
                double[] ysdata_new_3 = new double[10000];
                double[] Dydata_new_3 = new double[10000];

                double[] ysdata_new = new double[10000];
                double[] Dydata_new = new double[10000];
                for (int j = 0; j < original_num - 1; j++)
                {
                    sum_1 = sum_1 + ysdata[2, j];
                }
                mean_1 = sum_1 / (original_num - 1);
                for (int j = 0; j < original_num - 1; j++)
                {
                    biaozhuncha_sum_1 = biaozhuncha_sum_1 + Math.Pow((ysdata[2, j] - mean_1), 2);
                }
                biaozhuncha_1 = Math.Sqrt(biaozhuncha_sum_1 / (original_num - 1));
                for (int j = 0; j < original_num - 1; j++)
                {
                    ysdata_new_1[j] = (ysdata[2, j] - mean_1) / biaozhuncha_1;
                }

                for (int j = 0; j < original_num - 1; j++)
                {
                    sum_3 = sum_3 + ysdata[8, j];
                }
                mean_3 = sum_3 / (original_num - 1);
                for (int j = 0; j < original_num - 1; j++)
                {
                    biaozhuncha_sum_3 = biaozhuncha_sum_3 + Math.Pow((ysdata[8, j] - mean_3), 2);
                }
                biaozhuncha_3 = Math.Sqrt(biaozhuncha_sum_3 / (original_num - 1));
                for (int j = 0; j < original_num - 1; j++)
                {
                    ysdata_new_3[j] = (ysdata[8, j] - mean_3) / biaozhuncha_3;
                }

                for (int j = 0; j < original_num - 1; j++)
                {
                    ysdata_new[j] = (ysdata_new_1[j] + ysdata_new_3[j]) / 2;
                }


                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                qxx[0] = ysdata_new[(int)qxdian[0]];
                qxx[1] = ysdata_new[(int)qxdian[1]];
                qxx[2] = ysdata_new[(int)qxdian[2]];
                qxx[3] = ysdata_new[(int)qxdian[3]];



                double sum = 0;
                double Junzhi = 0;
                double Se = 0;
                double lyy = 0;
                for (int i = 1; i <= NodeNum; i++)
                {
                    sum = sum + qxx[i - 1];//求x的倒数

                }
                Junzhi = sum / NodeNum;
                for (int i = 1; i <= NodeNum; i++)
                {
                    qxx1[i - 1] = qxx[i - 1] / Junzhi;//求x的倒数
                    qxsd1[i - 1] = Math.Log(qxsd1[i - 1]);//求ln（y）
                }
                double sum_x1 = 0;
                double sum_y1 = 0;
                double sum_y = 0;
                double Junzhi_x1 = 0;
                double Junzhi_y1 = 0;
                double Junzhi_y = 0;

                for (int i = 1; i <= NodeNum; i++)
                {
                    sum_x1 = sum_x1 + qxx1[i - 1];
                    sum_y1 = sum_y1 + qxsd1[i - 1];
                }
                Junzhi_x1 = sum_x1 / NodeNum;
                Junzhi_y1 = sum_y1 / NodeNum;
                double xishu1 = 0;
                double xishu2 = 0;
                double A = 0;
                double R = 0;//相关系数


                for (int i = 1; i <= NodeNum; i++)
                {
                    xishu1 = xishu1 + Math.Pow((qxx1[i - 1] - Junzhi_x1), 2);
                    xishu2 = xishu2 + (qxx1[i - 1] - Junzhi_x1) * (qxsd1[i - 1] - Junzhi_y1);
                }
                b = xishu2 / xishu1;
                A = Junzhi_y1 - b * Junzhi_x1;
                a = Math.Exp(A);

                for (int i = 1; i <= NodeNum; i++)
                {
                    qxsd2[i - 1] = a * Math.Exp(b * qxx1[i - 1]);
                }
                for (int i = 1; i <= NodeNum; i++)
                {
                    sum_y = sum_y + qxsd[i - 1];
                }
                Junzhi_y = sum_y / NodeNum;
                for (int i = 1; i <= NodeNum; i++)
                {
                    Se = Se + Math.Pow((qxsd[i - 1] - qxsd2[i - 1]), 2);
                    lyy = lyy + Math.Pow((qxsd[i - 1] - Junzhi_y), 2);
                }
                R = 1 - Se / lyy;
                huiguixishu[0] = a;
                huiguixishu[1] = b;
                huiguixishu[2] = R;


                string huiguixishu1 = huiguixishu[0].ToString();
                string huiguixishu2 = huiguixishu[1].ToString();
                string huiguixishu3 = huiguixishu[2].ToString();



                bdxstext1.Text = huiguixishu1;
                bdxstext2.Text = huiguixishu2;
                bdxstext3.Text = huiguixishu3;

            }
            ///////////////////////////////////有5个定量缺陷的情况/////////////////////////
            else
            {

                NodeNum = 5;

                qxlocation[0] = double.Parse(qxwztext1.Text);
                qxlocation[1] = double.Parse(qxwztext2.Text);
                qxlocation[2] = double.Parse(qxwztext3.Text);
                qxlocation[3] = double.Parse(qxwztext4.Text);
                qxlocation[4] = double.Parse(qxwztext5.Text);

                qxsd1[0] = double.Parse(sddltext1.Text);
                qxsd1[1] = double.Parse(sddltext2.Text);
                qxsd1[2] = double.Parse(sddltext3.Text);
                qxsd1[3] = double.Parse(sddltext4.Text);
                qxsd1[4] = double.Parse(sddltext5.Text);

                //找到输入的缺陷在点的位置
                distext = double.Parse(DisText.Text);
                qxdian[0] = (int)Math.Round(qxlocation[0] * original_num / distext);
                qxdian[1] = (int)Math.Round(qxlocation[1] * original_num / distext);
                qxdian[2] = (int)Math.Round(qxlocation[2] * original_num / distext);
                qxdian[3] = (int)Math.Round(qxlocation[3] * original_num / distext);
                qxdian[4] = (int)Math.Round(qxlocation[4] * original_num / distext);





                /////////////////////////////////////////////////////////归一化///////////////////////////////////////////////////
                double x0 = 0.025;
                double y0 = 0.025;
                double W = 0.95;
                double H = 0.90;
                float x = (float)(pic.Width * x0);
                float y = (float)(pic.Height * y0);
                float width = (float)(pic.Width * W);
                float height = (float)(pic.Height * H);
                int n = 0;
                double C;
                lenofpiece = double.Parse(DisText.Text);
                double Sen = double.Parse(yztext.Text);//灵敏度
                tiligaodu = double.Parse(TLBox.Text);
                double D = double.Parse(DText.Text);

                double sum_1 = 0;
                double mean_1 = 0;
                double biaozhuncha_sum_1 = 0;
                double biaozhuncha_1 = 0;
                double[] ysdata_new_1 = new double[10000];
                double[] Dydata_new_1 = new double[10000];

                double sum_3 = 0;
                double mean_3 = 0;
                double biaozhuncha_sum_3 = 0;
                double biaozhuncha_3 = 0;
                double[] ysdata_new_3 = new double[10000];
                double[] Dydata_new_3 = new double[10000];

                double[] ysdata_new = new double[10000];
                double[] Dydata_new = new double[10000];
                for (int j = 0; j < original_num - 1; j++)
                {
                    sum_1 = sum_1 + ysdata[2, j];
                }
                mean_1 = sum_1 / (original_num - 1);
                for (int j = 0; j < original_num - 1; j++)
                {
                    biaozhuncha_sum_1 = biaozhuncha_sum_1 + Math.Pow((ysdata[2, j] - mean_1), 2);
                }
                biaozhuncha_1 = Math.Sqrt(biaozhuncha_sum_1 / (original_num - 1));
                for (int j = 0; j < original_num - 1; j++)
                {
                    ysdata_new_1[j] = (ysdata[2, j] - mean_1) / biaozhuncha_1;
                }

                for (int j = 0; j < original_num - 1; j++)
                {
                    sum_3 = sum_3 + ysdata[8, j];
                }
                mean_3 = sum_3 / (original_num - 1);
                for (int j = 0; j < original_num - 1; j++)
                {
                    biaozhuncha_sum_3 = biaozhuncha_sum_3 + Math.Pow((ysdata[8, j] - mean_3), 2);
                }
                biaozhuncha_3 = Math.Sqrt(biaozhuncha_sum_3 / (original_num - 1));
                for (int j = 0; j < original_num - 1; j++)
                {
                    ysdata_new_3[j] = (ysdata[8, j] - mean_3) / biaozhuncha_3;
                }

                for (int j = 0; j < original_num - 1; j++)
                {
                    ysdata_new[j] = (ysdata_new_1[j] + ysdata_new_3[j]) / 2;
                }


                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                qxx[0] = ysdata_new[(int)qxdian[0]];
                qxx[1] = ysdata_new[(int)qxdian[1]];
                qxx[2] = ysdata_new[(int)qxdian[2]];
                qxx[3] = ysdata_new[(int)qxdian[3]];
                qxx[4] = ysdata_new[(int)qxdian[4]];



                double sum = 0;
                double Junzhi = 0;
                double Se = 0;
                double lyy = 0;
                for (int i = 1; i <= NodeNum; i++)
                {
                    sum = sum + qxx[i - 1];//求x的倒数

                }
                Junzhi = sum / NodeNum;
                for (int i = 1; i <= NodeNum; i++)
                {
                    qxx1[i - 1] = qxx[i - 1] / Junzhi;//求x的倒数
                    qxsd1[i - 1] = Math.Log(qxsd1[i - 1]);//求ln（y）
                }
                double sum_x1 = 0;
                double sum_y1 = 0;
                double sum_y = 0;
                double Junzhi_x1 = 0;
                double Junzhi_y1 = 0;
                double Junzhi_y = 0;

                for (int i = 1; i <= NodeNum; i++)
                {
                    sum_x1 = sum_x1 + qxx1[i - 1];
                    sum_y1 = sum_y1 + qxsd1[i - 1];
                }
                Junzhi_x1 = sum_x1 / NodeNum;
                Junzhi_y1 = sum_y1 / NodeNum;
                double xishu1 = 0;
                double xishu2 = 0;
                double A = 0;
                double R = 0;//相关系数


                for (int i = 1; i <= NodeNum; i++)
                {
                    xishu1 = xishu1 + Math.Pow((qxx1[i - 1] - Junzhi_x1), 2);
                    xishu2 = xishu2 + (qxx1[i - 1] - Junzhi_x1) * (qxsd1[i - 1] - Junzhi_y1);
                }
                b = xishu2 / xishu1;
                A = Junzhi_y1 - b * Junzhi_x1;
                a = Math.Exp(A);

                for (int i = 1; i <= NodeNum; i++)
                {
                    qxsd2[i - 1] = a * Math.Exp(b * qxx1[i - 1]);
                }
                for (int i = 1; i <= NodeNum; i++)
                {
                    sum_y = sum_y + qxsd[i - 1];
                }
                Junzhi_y = sum_y / NodeNum;
                for (int i = 1; i <= NodeNum; i++)
                {
                    Se = Se + Math.Pow((qxsd[i - 1] - qxsd2[i - 1]), 2);
                    lyy = lyy + Math.Pow((qxsd[i - 1] - Junzhi_y), 2);
                }
                R = 1 - Se / lyy;
                huiguixishu[0] = a;
                huiguixishu[1] = b;
                huiguixishu[2] = R;


                string huiguixishu1 = huiguixishu[0].ToString();
                string huiguixishu2 = huiguixishu[1].ToString();
                string huiguixishu3 = huiguixishu[2].ToString();



                bdxstext1.Text = huiguixishu1;
                bdxstext2.Text = huiguixishu2;
                bdxstext3.Text = huiguixishu3;

            }
        }
    }
}
