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
    public partial class picForm : Form
    {
        public picForm(Image im)
        {            
            InitializeComponent();
            pic.Image = im;
        }

        private void pic_Click(object sender, EventArgs e)
        {

        }
    }
}
