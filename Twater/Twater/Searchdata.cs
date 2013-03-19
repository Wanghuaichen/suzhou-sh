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
    public partial class Searchdata : Form
    {
        string datsours = null;
        string cmpdat = null;
        public Searchdata()
        {
            InitializeComponent();
        }

        private void Searchdata_Load(object sender, EventArgs e)
        {
            tChart1.Header.Text = "���ݲ�ѯ";
            this.tChart1.Aspect.View3D = false;
            this.tChart1.Axes.Bottom.Labels.DateTimeFormat = "yyyy/mm/dd hh mm ss";
            this.tChart1.Axes.Bottom.Labels.MultiLine = true;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    comboBox2.Items.Add("PH");
                    comboBox2.Items.Add("������");
                    comboBox2.Items.Add("ˮ��");
                    comboBox2.Items.Add("�Ƕ�");
                    comboBox2.Items.Add("����");
                    comboBox2.Items.Add("�ܵ�");
                    comboBox2.Items.Add("����");
                    comboBox2.Items.Add("�ܽ���");
                    comboBox2.Items.Add("Ҷ����");
                    comboBox2.Items.Add("���ܶ�");
                    break;
                case 1:
                    comboBox2.Items.Add("���ն�");
                    comboBox2.Items.Add("������");
                    comboBox2.Items.Add("Ҷ����");
                    comboBox2.Items.Add("ƽ������");
                    comboBox2.Items.Add("ƽ������");
                    comboBox2.Items.Add("����");
                    comboBox2.Items.Add("���ʪ��");
                    comboBox2.Items.Add("��ѹ");
                    comboBox2.Items.Add("�����ۼ�");
                    comboBox2.Items.Add("�����¶�");
                    comboBox2.Items.Add("���ȵ�ѹ");
                    comboBox2.Items.Add("ˮ��");
                    comboBox2.Items.Add("�絼��");
                    comboBox2.Items.Add("�Ƕ�");
                    comboBox2.Items.Add("PH");
                    comboBox2.Items.Add("�ܽ���");
                    comboBox2.Items.Add("�ζ�");
                    break;
                case 2:
                    comboBox2.Items.Add("PH");
                    comboBox2.Items.Add("������");
                    comboBox2.Items.Add("ˮ��");
                    comboBox2.Items.Add("�Ƕ�");
                    comboBox2.Items.Add("����");
                    comboBox2.Items.Add("�ܵ�");
                    comboBox2.Items.Add("����");
                    comboBox2.Items.Add("�ܽ���");
                    comboBox2.Items.Add("Ҷ����");
                    comboBox2.Items.Add("���ܶ�");
                    comboBox2.Items.Add("���ն�");
                    comboBox2.Items.Add("ƽ������");
                    comboBox2.Items.Add("ƽ������");
                    comboBox2.Items.Add("����");
                    comboBox2.Items.Add("���ʪ��");
                    comboBox2.Items.Add("��ѹ");
                    comboBox2.Items.Add("�����ۼ�");
                    comboBox2.Items.Add("�����¶�");
                    comboBox2.Items.Add("�絼��");
                    comboBox2.Items.Add("�ζ�");
                    break;
            }
        }

        private void chk()
        {
            switch (comboBox1.SelectedIndex)
            {
                #region
                case 0:
                    datsours = "T_station";
                    switch (comboBox2.SelectedIndex)
                    { 
                        case 0:
                            cmpdat = "PH";
                            break;
                        case 1:
                            cmpdat = "O_use";
                            break;
                        case 2:
                            cmpdat = "water_temp";
                            break;
                        case 3:
                            cmpdat = "NTU";
                            break;
                        case 4:
                            cmpdat = "AN_N";
                            break;
                        case 5:
                            cmpdat = "N_total";
                            break;
                        case 6:
                            cmpdat = "P_total";
                            break;
                        case 7:
                            cmpdat = "O_dis";
                            break;
                        case 8:
                            cmpdat = "Chol";
                            break;
                        case 9:
                            cmpdat = "Alg_den";
                            break;
                    }
                    break;
                #endregion
                #region
                case 1:
                    datsours = "T_fubiao";
                    switch (comboBox2.SelectedIndex)
                    {
                        case 0:
                            cmpdat = "LX";
                            break;
                        case 1:
                            cmpdat = "LLV";
                            break;
                        case 2:
                            cmpdat = "YLS";
                            break;
                        case 3:
                            cmpdat = "Ave_wind";
                            break;
                        case 4:
                            cmpdat = "Wind_spd";
                            break;
                        case 5:
                            cmpdat = "Temp";
                            break;
                        case 6:
                            cmpdat = "Rel_humid";
                            break;
                        case 7:
                            cmpdat = "Pres";
                            break;
                        case 8:
                            cmpdat = "Rain";
                            break;
                        case 9:
                            cmpdat = "Heat_temp";
                            break;
                        case 10:
                            cmpdat = "Heat_v";
                            break;
                        case 11:
                            cmpdat = "Water_temp";
                            break;
                        case 12:
                            cmpdat = "Conduc";
                            break;
                        case 13:
                            cmpdat = "NTU";
                            break;
                        case 14:
                            cmpdat = "PH";
                            break;
                        case 15:
                            cmpdat = "O_disv";
                            break;
                        case 16:
                            cmpdat = "Solt";
                            break;
                    }
                    break;
                #endregion
                #region
                case 2:
                    datsours = "T_lmis";
                    switch (comboBox2.SelectedIndex)
                    {

                        case 0:
                            cmpdat = "PH";
                            break;
                        case 1:
                            cmpdat = "O_use";
                            break;
                        case 2:
                            cmpdat = "water_temp";
                            break;
                        case 3:
                            cmpdat = "NTU";
                            break;
                        case 4:
                            cmpdat = "AN_N";
                            break;
                        case 5:
                            cmpdat = "N_total";
                            break;
                        case 6:
                            cmpdat = "P_total";
                            break;
                        case 7:
                            cmpdat = "O_dis";
                            break;
                        case 8:
                            cmpdat = "Chol";
                            break;
                        case 9:
                            cmpdat = "Alg_den";
                            break;
                        case 10:
                            cmpdat = "LX";
                            break;
                        case 11:
                            cmpdat = "Ave_wind";
                            break;
                        case 12:
                            cmpdat = "Wind_spd";
                            break;
                        case 13:
                            cmpdat = "Temp";
                            break;
                        case 14:
                            cmpdat = "Rel_humid";
                            break;
                        case 15:
                            cmpdat = "Pres";
                            break;
                        case 16:
                            cmpdat = "Rain";
                            break;
                        case 17:
                            cmpdat = "Heat_temp";
                            break;
                        case 18:
                            cmpdat = "Conduc";
                            break;
                        case 19:
                            cmpdat = "Solt";
                            break;
                    }
                    break;
                #endregion
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(comboBox1.Text) == true || string.IsNullOrEmpty(comboBox2.Text) == true)
            {
                MessageBox.Show("������Դ�����ݲ�������Ϊ��");
            }
            else
            {
                tChart1.Series.Clear();
                tChart1.Header.Text = "���ݲ�ѯ";
                tChart1.Aspect.View3D = false;
                tChart1.Axes.Bottom.Labels.DateTimeFormat = "yyyy/MM/dd HH:mm";
                tChart1.Axes.Bottom.Labels.MultiLine = true;

                Steema.TeeChart.Styles.Line line0 = new Steema.TeeChart.Styles.Line();
                line0.Title = "  ";
                line0.Color = Color.White;
                line0.XValues.DateTime = true;
                tChart1.Series.Add(line0);

                chk();
                Steema.TeeChart.Styles.Line line1 = new Steema.TeeChart.Styles.Line();
                paintline(line1, cmpdat, datsours, Color.Red, comboBox1.Text.ToString().Trim() + cmpdat);

            }
        }

        /// <summary>
        /// �����ߣ���������ʱ��
        /// </summary>
        /// <param name="line33"></param>
        /// <param name="cmpdata"></param>
        /// <param name="datasource"></param>
        /// <param name="tcolor"></param>
        /// <param name="tname"></param>
        private void paintline(Steema.TeeChart.Styles.Line line33, string cmpdata, string datasource, Color tcolor, string tname)
        {
            string sqlstr = "select " + cmpdata + ",time  from " + datasource + "";
            SqlConnection sql = new SqlConnection(Global.sqlconstr);
            sql.Open();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(sqlstr, sql);
            da.Fill(dt);
           
            if (0 == dt.Rows.Count)
            {
                MessageBox.Show("û������");
            }
            else
            {
                line33.Title = tname;
                line33.Color = tcolor;
                line33.DataSource = dt;
                line33.XValues.DateTime = true;
                line33.XValues.DataMember = "time";
                line33.YValues.DataMember = cmpdata;
                tChart1.Series.Add(line33);
            }
            sql.Close();
        }
    }
}