using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace theHostComputer
{
    class CDigitalFilter
    {
        private double[] b;
        private double[] a;
        private double[] x;
        private int nfilt;

        public void DigitalFilter(double[] b, double[] a, double[] x)
        {
            this.b = b;
            this.a = a;
            this.x = x;
        }

        public double[] zeroFilter()
        {
            double[] y = new double[x.Length];
            int nb = b.Length;
            int na = a.Length;
            nfilt = Math.Max(na, nb);
            int nfact = 3 * (nfilt - 1);  // length of edge transients
            if (na < nfilt) a = new double[nfilt];
            if (nb < nfilt) b = new double[nfilt];
            //首尾添数
            double[] yTemp = new double[x.Length + 2 * nfact];
            for (int i = 0; i < nfact; i++)
            {
                yTemp[i] = 2 * x[0] - x[nfact - i];
            }
            for (int i = nfact; i < x.Length + nfact; i++)
            {
                yTemp[i] = x[i - nfact];
            }
            for (int i = x.Length + nfact; i < yTemp.Length; i++)
            {
                yTemp[i] = 2 * x[x.Length - 1] - x[yTemp.Length - 2 - i + x.Length - nfact];
            }
            //正向滤波           
            yTemp = zeroCalc(yTemp);
            //反序
            yTemp = this.reverse(yTemp);
            //反向滤波            
            yTemp = zeroCalc(yTemp);
            //反序
            yTemp = this.reverse(yTemp);
            for (int i = 0; i < x.Length; i++)
            {
                y[i] = yTemp[i + nfact];
            }            
            return y;
        }
        private double[] zeroCalc(double[] xx)
        {
            double[] yy = new double[xx.Length];
            for (int i = 0; i < yy.Length; i++)
            {
                for (int j = 0; j <= i && j < nfilt; j++)
                {
                    yy[i] = yy[i] + (b[j] * xx[i - j] - a[j] * yy[i - j]);
                }
            }
            return yy;
        }
        private double[] reverse(double[] data)
        {
            double tmp = 0;
            for (int i = 0; i < data.Length / 2; i++)
            {
                tmp = data[data.Length - i - 1];
                data[data.Length - i - 1] = data[i];
                data[i] = tmp;
            }
            return data;
        }
    }
}
