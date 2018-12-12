using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms; 
using NT.Net.App;

namespace theHostComputer
{
    class Dog
    {

        private long Rtn = 0; //方法返回值
        private long[] keyHandles = new long[8];
        private long keyNum = 0;
        private string[] AppId = new string[] { "CT-PT-31600", "CT-PT-31600-PC" };//长度为2

        private void find_Dog()
        {
            //加密狗的处理；
            //找狗
            for (int i = 0; i < 2; ++i)
            {
                Rtn = NT158App.NT158Find(AppId[i], out keyHandles, ref keyNum);
                if (Rtn != 0)
                {
                    //MessageBox.Show("Not Find NT158Key Error = " + Rtn);
                }
                else
                {

                    int UserPin1 = -2087732841;
                    int UserPin2 = 1648569370;
                    int UserPin3 = 1489046975;
                    int UserPin4 = 676158292;

                    Rtn = NT158App.NT158Login(keyHandles[0], UserPin1, UserPin2, UserPin3, UserPin4);
                    if (Rtn != 0)
                    {
                        // MessageBox.Show("Login Error =" + Rtn);
                    }
                    else
                    {
                        return;//找到狗
                    }

                }

            }
        }

        public void isMyDog()
        {
            find_Dog();
            if (Rtn != 0)
            {
                MessageBox.Show("非法软件不能运行！");
                Application.Exit(); //程序退出
            }
        
        }



    }
    
}
