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
    public partial class WelcomeForm : Form
    {
        public WelcomeForm()
        {
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e)
        {
       //     dog.isMyDog();

            MainForm a_Form = new MainForm();
            a_Form.Show();
            this.Hide();
        }

        private void buttoncancel_Click(object sender, EventArgs e)
        {
            this.Close();  //本窗体退出
            Application.Exit(); //程序退出
        }

        private void button2_Click(object sender, EventArgs e)
        {
           // dog.isMyDog();
            //walking wForm = new walking();
            //wForm.Show();
            //this.Hide();
            walking b_Form = new walking();
            b_Form.Show();
            this.Hide();
        }

        //private void button1_Click(object sender, EventArgs e)
        //{
        //    biaoding zForm = new biaoding();
        //    zForm.Show();
        //    //this.Hide();
        //}

        private void button3_Click(object sender, EventArgs e)
        {
            dealing c_Form = new dealing();
            c_Form.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            FrequencyForm frequencyForm = new FrequencyForm();
            frequencyForm.Show();
            this.Hide();
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            SpectrumFrom spectrumForm = new SpectrumFrom();
            spectrumForm.Show();
            this.Hide();
        }
        //private void button1_Click(object sender, EventArgs e)
        //{
        //    biaoding wForm = new biaoding();
        //    wForm.Show();
            
        //}    
    }
}
