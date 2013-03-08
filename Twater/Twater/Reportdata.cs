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
                MessageBox.Show("��Դ����Ϊ��");
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
                        dataGridView1.Columns[0].HeaderText = "���";
                        dataGridView1.Columns[0].Width = 100;
                        dataGridView1.Columns[1].HeaderText = "PHֵ";
                        dataGridView1.Columns[1].Width = 140;
                        dataGridView1.Columns[2].HeaderText = "������(mg/L)";
                        dataGridView1.Columns[2].Width = 165;
                        dataGridView1.Columns[3].HeaderText = "ˮ��(C)";
                        dataGridView1.Columns[3].Width = 165;
                        dataGridView1.Columns[4].HeaderText = "�Ƕ�(NTU)";
                        dataGridView1.Columns[4].Width = 165;
                        dataGridView1.Columns[5].HeaderText = "����(mg/L)";
                        dataGridView1.Columns[5].Width = 165;
                        dataGridView1.Columns[6].HeaderText = "�ܵ�(mg/L)";
                        dataGridView1.Columns[6].Width = 165;
                        dataGridView1.Columns[7].HeaderText = "����(mg/L)";
                        dataGridView1.Columns[7].Width = 165;
                        dataGridView1.Columns[8].HeaderText = "�ܽ���(mg/L)";
                        dataGridView1.Columns[8].Width = 165;
                        dataGridView1.Columns[9].HeaderText = "Ҷ����(mg/L)";
                        dataGridView1.Columns[9].Width = 165;
                        dataGridView1.Columns[10].HeaderText = "���ܶ�(cell/L)";
                        dataGridView1.Columns[10].Width = 165;
                        dataGridView1.Columns[11].HeaderText = "����";
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
                        dataGridView1.Columns[0].HeaderText = "���";
                        dataGridView1.Columns[0].Width = 100;
                        dataGridView1.Columns[1].HeaderText = "���ն�";
                        dataGridView1.Columns[1].Width = 140;
                        dataGridView1.Columns[2].HeaderText = "������(cell/L)";
                        dataGridView1.Columns[2].Width = 165;
                        dataGridView1.Columns[3].HeaderText = "Ҷ����(mg/L)";
                        dataGridView1.Columns[3].Width = 165;
                        dataGridView1.Columns[4].HeaderText = "ƽ������(D)";
                        dataGridView1.Columns[4].Width = 165;
                        dataGridView1.Columns[5].HeaderText = "ƽ������(m/s)";
                        dataGridView1.Columns[5].Width = 165;
                        dataGridView1.Columns[6].HeaderText = "����(C)";
                        dataGridView1.Columns[6].Width = 165;
                        dataGridView1.Columns[7].HeaderText = "���ʪ��(%RH)";
                        dataGridView1.Columns[7].Width = 165;
                        dataGridView1.Columns[8].HeaderText = "��ѹ(hPa)";
                        dataGridView1.Columns[8].Width = 165;
                        dataGridView1.Columns[9].HeaderText = "����(mm)";
                        dataGridView1.Columns[9].Width = 165;
                        dataGridView1.Columns[10].HeaderText = "�����¶�(C)";
                        dataGridView1.Columns[10].Width = 165;
                        dataGridView1.Columns[11].HeaderText = "���ȵ�ѹ(V)";
                        dataGridView1.Columns[11].Width = 165;
                        dataGridView1.Columns[12].HeaderText = "ˮ��(C)";
                        dataGridView1.Columns[12].Width = 165;
                        dataGridView1.Columns[13].HeaderText = "�絼��(mS/cm)";
                        dataGridView1.Columns[13].Width = 165;
                        dataGridView1.Columns[14].HeaderText = "�Ƕ�(NTU)";
                        dataGridView1.Columns[14].Width = 165;
                        dataGridView1.Columns[15].HeaderText = "PH";
                        dataGridView1.Columns[15].Width = 165;
                        dataGridView1.Columns[16].HeaderText = "�ܽ���(mg/L)";
                        dataGridView1.Columns[16].Width = 165;
                        dataGridView1.Columns[17].HeaderText = "�ζ�(ppt)";
                        dataGridView1.Columns[17].Width = 165;
                        dataGridView1.Columns[18].HeaderText = "����";
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