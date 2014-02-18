using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace Twater
{
    public partial class Setting : Form
    {
        public Setting()
        {
            InitializeComponent();
        }

        private void Setting_Load(object sender, EventArgs e)
        {
            label2.Text = null;
            label2.ForeColor = Color.Empty;
            bool result = Global.GetAutoPredicteTime();
            if (result == true)
            {
                this.textBox1.Text = Global.autoPredicteTime.ToString();
            }
            else
            {
                Global.autoPredicteTime = 30;
                this.textBox1.Text = Global.autoPredicteTime.ToString();
                Global.SetAutoPredicteTime();
            }
        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            string pattern = @"^[0-9]*$";
            Match m = Regex.Match(this.textBox1.Text, pattern);
            if (m.Success)
            {
                label2.Text = "";
                label2.ForeColor = Color.Empty;
                Global.autoPredicteTime = Convert.ToInt32(this.textBox1.Text.Trim());
                Global.SetAutoPredicteTime();
                this.Close();
            }
            else
            {
                label2.Text = "只能输入数字";
                label2.ForeColor = Color.Red;
            }
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
