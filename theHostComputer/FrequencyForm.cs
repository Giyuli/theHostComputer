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
    public partial class FrequencyForm : Form
    {
        public FrequencyForm()
        {
            InitializeComponent();
            Shows();
        }

        private void Shows()
        {
            zedGraphControl1.GraphPane.CurveList.Clear();
            zedGraphControl1.GraphPane.GraphObjList.Clear();
            // clearing not teste


            GraphPane myPane = zedGraphControl1.GraphPane;
            // 画图面版标题
            myPane.Title.Text = "频谱分析";
            // 画图面版X标题
            myPane.XAxis.Title.Text = "频率";


            myPane.XAxis.Scale.Min = 0;
            //初始化数据(频率)
            PointPairList list1 = new PointPairList();

            Random random = new Random();
            double number;

            for (int i = 0; i < 24; i++)////这里的数量要和lable的一致，比如横坐标显示了5个lable，这里就要给5个
            {
                number = random.Next(60, 100);
                list1.Add(i, number);
            }



            // 画图面版Y标题
            myPane.YAxis.Title.Text = "信号强度";
            //柱的画笔
            //    public BarItem AddBar(string 名称, IPointList 数据, Color 颜色);

            BarItem myCurve = myPane.AddBar("频率", list1, Color.Blue);

            //myCurve.Bar.Fill = new Fill(Color.Blue, Color.White, Color.Blue);//渐变
            // BarItem myCurve2 = myPane.AddBar("买农药", list2, Color.Red);
            // myCurve2.Bar.Fill = new Fill(Color.Red, Color.White, Color.Red);
            //  BarItem myCurve3 = myPane.AddBar("买化肥", list3, Color.Green);
            // myCurve3.Bar.Fill = new Fill(Color.Green, Color.White, Color.Green);

            //YAxis标注
            string[] YLabels = { "0", "10", "20", "30", "40", "50", "60", "70", "80", "90", "100" };
            myPane.YAxis.Scale.TextLabels = YLabels;

            //myPane.XAxis.MajorTic.IsBetweenLabels = true;
            //XAxis标注
            string[] XLabels = { "1Hz", "70Hz", "100Hz", "150Hz", "200Hz", "250Hz", "300Hz", "350Hz", "400Hz",
                              "450Hz", "500Hz", "550Hz", "600Hz", "700Hz", "800Hz", "900Hz", "1000Hz", "1100Hz",
                              "1300Hz", "1500Hz", "2000Hz", "3000Hz", "4000Hz", "5000Hz"};
            myPane.XAxis.Scale.TextLabels = XLabels;
            myPane.XAxis.Type = AxisType.Text;

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
