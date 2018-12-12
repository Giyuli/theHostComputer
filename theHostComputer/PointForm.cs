using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZedGraph;

namespace theHostComputer
{
    public partial class PointForm : Form
    {
        public PointForm()
        {
            InitializeComponent();
            Show();
        }

        private void Show()
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
            myPane.XAxis.Scale.MajorStep = 2;//X轴大步长为5，也就是显示文字的大间隔  

            //初始化数据(频率)
            PointPairList list1 = new PointPairList();
            PointPairList list2 = new PointPairList();
            PointPairList list3 = new PointPairList();
            PointPairList list4 = new PointPairList();

            Random random = new Random();
            double number1,number2,number3,number4;

            for (int i = 0; i < 200; i++)////这里的数量要和lable的一致，比如横坐标显示了5个lable，这里就要给5个
            {
                number1 = random.Next(60, 100);
                number2 = random.Next(60, 100);
                number3 = random.Next(60, 100);
                number4 = random.Next(60, 100);
                list1.Add(i, number1);
                list2.Add(i, number2);
                list3.Add(i, number3);
                list4.Add(i, number4);
            }

            BarItem myCurve1 = myPane.AddBar("", list1, Color.Blue);
            BarItem myCurve2 = myPane.AddBar("", list2, Color.Blue);
            BarItem myCurve3 = myPane.AddBar("", list3, Color.Blue);
            BarItem myCurve4 = myPane.AddBar("", list4, Color.Blue);

            //myCurve.Bar.Fill = new Fill(Color.Blue, Color.White, Color.Blue);//渐变
            // BarItem myCurve2 = myPane.AddBar("买农药", list2, Color.Red);
            // myCurve2.Bar.Fill = new Fill(Color.Red, Color.White, Color.Red);
            //  BarItem myCurve3 = myPane.AddBar("买化肥", list3, Color.Green);
            // myCurve3.Bar.Fill = new Fill(Color.Green, Color.White, Color.Green);

            ////YAxis标注
            //string[] YLabels = { "0", "10", "20", "30", "40", "50", "60", "70", "80", "90", "100" };
            //myPane.YAxis.Scale.TextLabels = YLabels;

            ////myPane.XAxis.MajorTic.IsBetweenLabels = true;
            ////XAxis标注
            //string[] XLabels = { "1Hz", "70Hz", "100Hz", "150Hz", "200Hz", "250Hz", "300Hz", "350Hz", "400Hz",
            //                  "450Hz", "500Hz", "550Hz", "600Hz", "700Hz", "800Hz", "900Hz", "1000Hz", "1100Hz",
            //                  "1300Hz", "1500Hz", "2000Hz", "3000Hz", "4000Hz", "5000Hz"};
            //myPane.XAxis.Scale.TextLabels = XLabels;
            //myPane.XAxis.Type = AxisType.Text;

            //图区以外的颜色
            // myPane.Fill = new Fill(Color.White, Color.FromArgb(200, 200, 255), 45.0f);
            //背景颜色
            // myPane.Chart.Fill = new Fill(Color.Red, Color.LightGoldenrodYellow, 45.0f);


            zedGraphControl1.AxisChange();
            zedGraphControl1.Refresh();
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            EndingForm eForm = new EndingForm();
            eForm.Show();
            this.Close();
        }
    }
}
