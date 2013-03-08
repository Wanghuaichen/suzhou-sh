using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Twater
{
    public partial class Reportdata : Form
    {
        string sqlstr;
        DataTable dt1;
        public Reportdata()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(comboBox1.Text) == true)
            {
                MessageBox.Show("来源不能为空");
            }
            else
            {
                switch (comboBox1.SelectedIndex)
                {
                    case 0:
                        sqlstr = "select * from T_station";
                        dt1 = new DataTable();
                        SqlConnection sqlcon = new SqlConnection(Global.sqlconstr);
                        sqlcon.Open();
                        SqlDataAdapter da1 = new SqlDataAdapter(sqlstr, sqlcon);
                        da1.Fill(dt1);
                        dataGridView1.DataSource = dt1;
                        dataGridView1.Columns[0].HeaderText = "编号";
                        dataGridView1.Columns[0].Width = 100;
                        dataGridView1.Columns[1].HeaderText = "PH值";
                        dataGridView1.Columns[1].Width = 140;
                        dataGridView1.Columns[2].HeaderText = "耗氧量(mg/L)";
                        dataGridView1.Columns[2].Width = 165;
                        dataGridView1.Columns[3].HeaderText = "水温(C)";
                        dataGridView1.Columns[3].Width = 165;
                        dataGridView1.Columns[4].HeaderText = "浊度(NTU)";
                        dataGridView1.Columns[4].Width = 165;
                        dataGridView1.Columns[5].HeaderText = "氨氮(mg/L)";
                        dataGridView1.Columns[5].Width = 165;
                        dataGridView1.Columns[6].HeaderText = "总氮(mg/L)";
                        dataGridView1.Columns[6].Width = 165;
                        dataGridView1.Columns[7].HeaderText = "总磷(mg/L)";
                        dataGridView1.Columns[7].Width = 165;
                        dataGridView1.Columns[8].HeaderText = "溶解氧(mg/L)";
                        dataGridView1.Columns[8].Width = 165;
                        dataGridView1.Columns[9].HeaderText = "叶绿素(mg/L)";
                        dataGridView1.Columns[9].Width = 165;
                        dataGridView1.Columns[10].HeaderText = "藻密度(cell/L)";
                        dataGridView1.Columns[10].Width = 165;
                        dataGridView1.Columns[11].HeaderText = "日期";
                        dataGridView1.Columns[11].Width = 165;
                        sqlcon.Close();
                        break;
                    case 1:
                        sqlstr = "select * from T_fubiao";
                        dt1 = new DataTable();
                        SqlConnection sqlcon1 = new SqlConnection(Global.sqlconstr);
                        sqlcon1.Open();
                        SqlDataAdapter da2 = new SqlDataAdapter(sqlstr, sqlcon1);
                        da2.Fill(dt1);
                        dataGridView1.DataSource = dt1;
                        dataGridView1.Columns[0].HeaderText = "编号";
                        dataGridView1.Columns[0].Width = 100;
                        dataGridView1.Columns[1].HeaderText = "光照度";
                        dataGridView1.Columns[1].Width = 140;
                        dataGridView1.Columns[2].HeaderText = "蓝绿藻(cell/L)";
                        dataGridView1.Columns[2].Width = 165;
                        dataGridView1.Columns[3].HeaderText = "叶绿素(mg/L)";
                        dataGridView1.Columns[3].Width = 165;
                        dataGridView1.Columns[4].HeaderText = "平均风向(D)";
                        dataGridView1.Columns[4].Width = 165;
                        dataGridView1.Columns[5].HeaderText = "平均风速(m/s)";
                        dataGridView1.Columns[5].Width = 165;
                        dataGridView1.Columns[6].HeaderText = "气温(C)";
                        dataGridView1.Columns[6].Width = 165;
                        dataGridView1.Columns[7].HeaderText = "相对湿度(%RH)";
                        dataGridView1.Columns[7].Width = 165;
                        dataGridView1.Columns[8].HeaderText = "气压(hPa)";
                        dataGridView1.Columns[8].Width = 165;
                        dataGridView1.Columns[9].HeaderText = "雨量(mm)";
                        dataGridView1.Columns[9].Width = 165;
                        dataGridView1.Columns[10].HeaderText = "加热温度(C)";
                        dataGridView1.Columns[10].Width = 165;
                        dataGridView1.Columns[11].HeaderText = "加热电压(V)";
                        dataGridView1.Columns[11].Width = 165;
                        dataGridView1.Columns[12].HeaderText = "水温(C)";
                        dataGridView1.Columns[12].Width = 165;
                        dataGridView1.Columns[13].HeaderText = "电导率(mS/cm)";
                        dataGridView1.Columns[13].Width = 165;
                        dataGridView1.Columns[14].HeaderText = "浊度(NTU)";
                        dataGridView1.Columns[14].Width = 165;
                        dataGridView1.Columns[15].HeaderText = "PH";
                        dataGridView1.Columns[15].Width = 165;
                        dataGridView1.Columns[16].HeaderText = "溶解氧(mg/L)";
                        dataGridView1.Columns[16].Width = 165;
                        dataGridView1.Columns[17].HeaderText = "盐度(ppt)";
                        dataGridView1.Columns[17].Width = 165;
                        dataGridView1.Columns[18].HeaderText = "日期";
                        dataGridView1.Columns[18].Width = 165;
                        sqlcon1.Close();
                        break;
                    default: break;
                }
            }

           
            //if (dataGridView1.RowCount > 0)
            //{
            //    dataGridView1.SelectedRows[0].Selected = false;
            //}
        }

    }
}