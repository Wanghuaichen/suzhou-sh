using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Threading;

namespace Twater
{
    public partial class Prediction : Form
    {

        public Prediction()
        {
            InitializeComponent();
        }

        private void Prediction_Load(object sender, EventArgs e)
        {

        }
        private void button1_Click(object sender, EventArgs e)
        {

            //if (string.IsNullOrEmpty(comboBox1.Text) == true)
            //{
            //    MessageBox.Show("数据来源和数据参量不能为空");
            //}
            //else
            //{
                double[,] dx = new double[10, 2] { { 1, 2 }, { 3, 4 }, { 5, 6 }, { 7, 8 }, { 9, 0 }, { 2, 3 }, { 4, 5 }, { 6, 7 }, { 8, 9 }, { 0, 1 } };
                object y = 0;
                double m = 10;
                mat.matclass p2 = new mat.matclass();

                p2.waterwlc1(1, ref y, dx,m);
                MessageBox.Show("qqqqqqqqqqq");
           // }
        }
    }
}