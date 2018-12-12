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
    public partial class zed2Form : Form
    {
        ZedGraphControl zedgraph;
        public zed2Form(ZedGraphControl gc)
        {
            zedgraph = gc;            
            InitializeComponent();
            DyfushizedGraph.GraphPane.Title.Text = gc.GraphPane.Title.Text;
            DyfushizedGraph.GraphPane.XAxis.Title = gc.GraphPane.XAxis.Title;
            DyfushizedGraph.GraphPane.YAxis.Title = gc.GraphPane.YAxis.Title;
            //底色画网格
            //DyfushizedGraph.GraphPane.XAxis.MajorGrid.IsVisible = true;
            //DyfushizedGraph.GraphPane.XAxis.MajorGrid.Color = Color.Green;
            //DyfushizedGraph.GraphPane.XAxis.MinorGrid.IsVisible = true;
            //DyfushizedGraph.GraphPane.XAxis.MinorGrid.Color = Color.Green;
            DyfushizedGraph.GraphPane.XAxis.Scale.Min = zedgraph.GraphPane.XAxis.Scale.Min;		//X轴最小值0
            DyfushizedGraph.GraphPane.XAxis.Scale.Max = zedgraph.GraphPane.XAxis.Scale.Max;	//X轴最大1000
            DyfushizedGraph.GraphPane.XAxis.Scale.MinorStep = gc.GraphPane.XAxis.Scale.MinorStep;//X轴小步长20,也就是小间隔
            DyfushizedGraph.GraphPane.XAxis.Scale.MajorStep = gc.GraphPane.XAxis.Scale.MajorStep;//X轴大步长为100，也就是显示文字的大间隔                     
            DyfushizedGraph.GraphPane.CurveList = gc.GraphPane.CurveList;
            DyfushizedGraph.GraphPane.AxisChange();
            DyfushizedGraph.Refresh();
            DyfushizedGraph.Invalidate();                               
        }

        //zedgraph显示坐标
        private string MyPointValueHandler(ZedGraphControl control, GraphPane pane, CurveItem curve, int iPt)
        {
            PointPair pt = curve[iPt];
            return "横坐标:" + string.Format("{0:0}", pt.X) + " 纵坐标:" + string.Format("{0:0.0}", pt.Y);
        }
        //调用使zedgraph显示坐标        
        private void zed2Form_MouseMove(object sender, MouseEventArgs e)
        {
            DyfushizedGraph.IsShowPointValues = true;  //动态磁场            
            DyfushizedGraph.PointValueEvent += new ZedGraphControl.PointValueHandler(MyPointValueHandler);
        }

        private void DyfushizedGraph_Load(object sender, EventArgs e)
        {

        }       
    }
}
