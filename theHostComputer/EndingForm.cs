using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Runtime.InteropServices;

namespace theHostComputer
{
    public partial class EndingForm : Form
    {

        /*************************************************************** Putoff_defination ********************************************************************/
        UInt32 status_off_1 = 1;
        UInt32 status_off_2 = 1;
        UInt32 status_off_3 = 1;
        UInt32 Value_off = 0;

        byte feedback = 0;
        byte flag = 0;
        byte addr_off = 0;
        byte cmd_off = 0;
        UInt32 length_off = 0;

        StringBuilder sb_off;
        byte[] DataBlock_off;

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
        public const UInt32 SUSI_ID_I2C_SUPPORTED = 0x00030100;
        public const UInt32 SUSI_I2C_EXTERNAL_SUPPORTED = (1 << 0);
        public const UInt32 SUSI_I2C_OEM0_SUPPORTED = (1 << 1);
        public const UInt32 SUSI_I2C_OEM1_SUPPORTED = (1 << 2);
        public const UInt32 SUSI_I2C_OEM2_SUPPORTED = (1 << 3);

        public const UInt32 SUSI_ID_I2C_MAXIMUM_BLOCK_LENGTH = 0x00000000;

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
        [DllImport("Susi4", EntryPoint = "SusiI2CReadTransfer")]
        public static extern UInt32 SusiI2CReadTransfer(UInt32 Id, UInt32 Addr, UInt32 Cmd, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] byte[] pBuffer, UInt32 ReadLen);
        /**************************************************************************************************************************************************/
        
        public EndingForm()
        {
            InitializeComponent();
        }

        private void back_to_desktop_Click(object sender, EventArgs e)
        {
            this.Close();  //本窗体退出
            Application.Exit(); //程序退出
        }

        private void close_pc_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("仪器将在60秒后自动关机！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //this.Close();  //本窗体退出
            //Application.Exit(); //程序退出
            //System.Diagnostics.Process.Start("cmd.exe", "/cshutdown -s -t 60");     //60秒后关闭计算机
            status_off_1 = SusiLibInitialize();
            if (status_off_1 == SUSI_STATUS_SUCCESS)
            {
                //textBox1.Text = "success";
                //InitializeSMB();
                status_off_2 = SusiBoardGetValue(SUSI_ID_I2C_SUPPORTED, ref Value_off);
                if (status_off_2 == SUSI_STATUS_SUCCESS)
                {
                    addr_off = Convert.ToByte("56", 16);//以前为26，后改成56
                    cmd_off = Convert.ToByte("12", 16);
                    length_off = Convert.ToUInt32("1");
                    DataBlock_off = new byte[length_off];
                    status_off_3 = SusiI2CReadTransfer(SUSI_ID_I2C_MAXIMUM_BLOCK_LENGTH, addr_off, cmd_off, DataBlock_off, length_off);
                    if (status_off_3 == SUSI_STATUS_SUCCESS)
                    {
                        sb_off = new StringBuilder();
                        for (int i = 0; i < length_off; i++)
                        {
                            sb_off.Append(string.Format("{0:X2}", DataBlock_off[i]));
                        }
                        feedback = Convert.ToByte(sb_off.ToString(), 16); 
                    }
                }
            }
            if (feedback == 136)
            {
                MessageBox.Show("仪器将在60秒后自动关机！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();  //本窗体退出
                Application.Exit(); //程序退出
                System.Diagnostics.Process.Start("cmd.exe", "/cshutdown -s -t 60");     //60秒后关闭计算机
            }
            if (feedback == 68)
            {
                flag++;
                if (flag != 3)
                {
                    MessageBox.Show("请再次点击关机按钮！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                if (flag == 3)
                {
                    MessageBox.Show("请采用传统软件关机方式关机，并手动长按仪器电源键，切断仪器电源！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.Close();  //本窗体退出
                    Application.Exit(); //程序退出
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            WelcomeForm c_Form = new WelcomeForm();
            c_Form.Show();
            this.Hide();
        }
    }
}
