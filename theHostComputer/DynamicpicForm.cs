using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace theHostComputer
{
    public partial class DynamicpicForm : Form
    {
        PictureBox pic;
        public DynamicpicForm(PictureBox im)
        {
            pic = im;
            InitializeComponent();            
            Dynamicpic.Image = im.Image;            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Dynamicpic.Image = pic.Image;            
            Dynamicpic.Invalidate();
        }

        private void DynamicpicForm_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;
        }

        private void DynamicpicForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            timer1.Enabled = false;
        }
    }
}
