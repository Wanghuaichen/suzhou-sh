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
    public partial class Start : Form
    {   
        private static Start dg=new Start();
        public static Start newform
        { 
            get
            {
                if (dg == null || dg.IsDisposed)
                {
                    dg = new Start();
                }
                return dg;
            }
        }
        public Start()
        {
            InitializeComponent();
            timer1.Start();

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            while(this.Opacity>1)
            {
                this.Opacity-=0.25;
            }
            timer1.Stop();
            this.Close();
        }
    }
}