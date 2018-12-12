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
    public partial class SpectrumFrom : Form
    {
        public SpectrumFrom()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label1.Text = "1Hz";
            this.progressBar1.Value = 87;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            label1.Text = "70Hz";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            label1.Text = "100Hz";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            label1.Text = "150Hz";
        }

        private void button8_Click(object sender, EventArgs e)
        {
            label1.Text = "200Hz";
        }

        private void button7_Click(object sender, EventArgs e)
        {
            label1.Text = "250Hz";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            label1.Text = "300Hz";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            label1.Text = "350Hz";
        }

        private void button12_Click(object sender, EventArgs e)
        {
            label1.Text = "400Hz";
        }

        private void button11_Click(object sender, EventArgs e)
        {
            label1.Text = "450Hz";
        }

        private void button10_Click(object sender, EventArgs e)
        {
            label1.Text = "500Hz";
        }

        private void button9_Click(object sender, EventArgs e)
        {
            label1.Text = "550Hz";
        }

        private void button16_Click(object sender, EventArgs e)
        {
            label1.Text = "600Hz";
        }

        private void button15_Click(object sender, EventArgs e)
        {
            label1.Text = "700Hz";
        }

        private void button14_Click(object sender, EventArgs e)
        {
            label1.Text = "800Hz";
        }

        private void button13_Click(object sender, EventArgs e)
        {
            label1.Text = "900Hz";
        }

        private void button20_Click(object sender, EventArgs e)
        {
            label1.Text = "1000Hz";
        }

        private void button19_Click(object sender, EventArgs e)
        {
            label1.Text = "1100Hz";
        }

        private void button18_Click(object sender, EventArgs e)
        {
            label1.Text = "1300Hz";
        }

        private void button17_Click(object sender, EventArgs e)
        {
            label1.Text = "1500Hz";
        }

        private void button24_Click(object sender, EventArgs e)
        {
            label1.Text = "2000Hz";
        }

        private void button23_Click(object sender, EventArgs e)
        {
            label1.Text = "3000Hz";
        }

        private void button22_Click(object sender, EventArgs e)
        {
            label1.Text = "4000Hz";
        }

        private void button21_Click(object sender, EventArgs e)
        {
            label1.Text = "5000Hz";
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            EndingForm eForm = new EndingForm();
            eForm.Show();
            this.Close();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            var sound = this.trackBar1.Value;
            switch (sound)
            {
                case 0:
                    label15.Text = "(000)";
                    break;
                case 100:
                    label15.Text = "(100)";
                    break;
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                    label15.Text = "(00" + sound + ")";
                    break;
                default:
                    label15.Text = "(0" + sound + ")";
                    break;
            }
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            var sound = this.trackBar2.Value;
            switch (sound)
            {
                case 0:
                    label17.Text = "0%";
                    break;
                case 100:
                    label17.Text = "100%";
                    break;
                default:
                    label17.Text = sound + "%";
                    break;
            }
        }
    }
}
