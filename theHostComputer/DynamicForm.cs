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
    public partial class DynamicForm : Form
    {

        ZedGraphControl zedgraph;
        public DynamicForm(ZedGraphControl  gc)
        {
            zedgraph = gc;            
            InitializeComponent();
            DyfushizedGraph.GraphPane.Title.Text = gc.GraphPane.Title.Text;
            DyfushizedGraph.GraphPane.XAxis.Title = gc.GraphPane.XAxis.Title;
            DyfushizedGraph.GraphPane.YAxis.Title = gc.GraphPane.YAxis.Title;
            DyfushizedGraph.GraphPane.XAxis.MajorGrid.IsVisible = true;//底色画网格
            DyfushizedGraph.GraphPane.XAxis.MajorGrid.Color = Color.Green;
            DyfushizedGraph.GraphPane.XAxis.MinorGrid.IsVisible = true;
            DyfushizedGraph.GraphPane.XAxis.MinorGrid.Color = Color.Green;          
            DyfushizedGraph.GraphPane.XAxis.Scale.MinorStep = gc.GraphPane.XAxis.Scale.MinorStep;//X轴小步长20,也就是小间隔
            DyfushizedGraph.GraphPane.XAxis.Scale.MajorStep = gc.GraphPane.XAxis.Scale.MajorStep;//X轴大步长为100，也就是显示文字的大间隔                     
            DyfushizedGraph.GraphPane.CurveList = gc.GraphPane.CurveList;                       
            timer1.Enabled = true;
            
        }                  

        private void timer1_Tick(object sender, EventArgs e)
        {
            DyfushizedGraph.GraphPane.XAxis.Scale.Min = zedgraph.GraphPane.XAxis.Scale.Min;		//X轴最小值0
            DyfushizedGraph.GraphPane.XAxis.Scale.Max = zedgraph.GraphPane.XAxis.Scale.Max;	//X轴最大1000
            DyfushizedGraph.GraphPane.AxisChange();
            DyfushizedGraph.Refresh();
            DyfushizedGraph.Invalidate();
        }

        private void DynamicForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            timer1.Enabled = false;
        }              
    }
}
