using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Threading;
using System.Data.SqlClient;

namespace Twater
{
    public partial class Prediction : Form
    {
        public const int LENGTH = 40960;
        double[,] ddatx = new double[Prediction.LENGTH, 11];
        double[,] dsvmdat;// = new double[Prediction.LENGTH, 10];
        List<string> timeList = new List<string>();
        object y = 0;
        double strdaynum;//Ԥ������
        int tcountdat;//��������
        string stragl;//�㷨
        double[] p = new double[] { 1, 100, 100, 1, 100, 1, 1, 1, 100, 50, 100 };
        public Prediction()
        {
            InitializeComponent();
        }

        private void Prediction_Load(object sender, EventArgs e)
        {
            //label5.Text = "";
            textBox1.Text = "";
            label7.Text = "";
        }
        private void button1_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(comboBox1.Text) == true || string.IsNullOrEmpty(comboBox2.Text) == true)
            {
                MessageBox.Show("Ԥ���������㷨����Ϊ��");
            }
            else if (dateTimePicker2.Value > DateTime.Now)
            {
                MessageBox.Show("��ֹʱ�䲻�ܳ�������");
            }
            else
            {
                //double[,] dx = new double[10, 2] { { 1, 2 }, { 3, 4 }, { 5, 6 }, { 7, 8 }, { 9, 0 }, { 2, 3 }, { 4, 5 }, { 6, 7 }, { 8, 9 }, { 0, 1 } };
                //��ȡ����

                getdata(dateTimePicker1.Value.ToString(), "T_station", dateTimePicker2.Value.ToString());
                bool result = changdat(timeList);
                if (result == true)
                {
                    label7.BackColor = Color.Transparent;
                    label7.Text = "������...";
                    if ("��ָ��Ԥ��" == stragl)
                    {
                        SVMpredictone.SVMpredictoneclassClass tmp1 = new SVMpredictone.SVMpredictoneclassClass();
                        tmp1.SVMpredictone(1, ref y, dsvmdat, strdaynum);
                        textBox1.Text = (((double)y / 2) * 2).ToString();
                        label7.Text = null;
                        if (10 >= (double)y)
                        {
                            label7.BackColor = Color.Lime;
                        }
                        else if (20 >= (double)y && 10 < (double)y)
                        {
                            label7.BackColor = Color.CornflowerBlue;
                        }
                        else if (40 >= (double)y && 20 < (double)y)
                        {
                            label7.BackColor = Color.Yellow;
                        }
                        else if (60 >= (double)y && 40 < (double)y)
                        {
                            label7.BackColor = Color.LightCoral;
                        }
                        else
                        {
                            label7.BackColor = Color.Red;
                        }
                    }
                    else
                    {
                        //waterwlc3.waterwlc3classClass tmp1 = new waterwlc3.waterwlc3classClass();
                        //tmp1.waterwlc3(1, ref y, ddatx, strdaynum, p);
                        label7.Text = null;
                        textBox1.Text = ((double)y).ToString();
                        if (10 >= (double)y)
                        {
                            label7.BackColor = Color.Lime;
                        }
                        else if (20 >= (double)y && 10 < (double)y)
                        {
                            label7.BackColor = Color.CornflowerBlue;
                        }
                        else if (40 >= (double)y && 20 < (double)y)
                        {
                            label7.BackColor = Color.Yellow;
                        }
                        else if (40 >= (double)y && 60 < (double)y)
                        {
                            label7.BackColor = Color.LightCoral;
                        }
                        else
                        {
                            label7.BackColor = Color.Red;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// �����ݶ�ת��double�͵����飬�������
        /// </summary>  
        /// <param name="lmdat"></param>
        /// <param name="lmtimdat"></param>
        /// <returns></returns>
        private bool changdat(List<string> lmtimdat)
        {
            if (0 == lmtimdat.Count)
            {
                MessageBox.Show("���в�����ָ������");
                return false;
            }
            else
            {
                String[] strdat = lmtimdat.ToArray();
                tcountdat = tcountdat > Prediction.LENGTH ? Prediction.LENGTH : tcountdat;
                for (int i = 0; i < tcountdat; i++)
                {
                    DateTime fdatetim;
                    fdatetim = DateTime.Parse(strdat[i].ToString());
                    double y = fdatetim.Year;
                    double mt = fdatetim.Month;
                    double d = fdatetim.Day;
                    double h = fdatetim.Hour;
                    if (1 == mt || 3 == mt || 5 == mt || 7 == mt || 8 == mt || 10 == mt || 12 == mt)
                        ddatx[i, 0] = (y) * 365 + mt * 31 + d + h / 24;
                    else if (2 == mt)
                        ddatx[i, 0] = (y) * 365 + mt * 28 + d + h / 24;
                    else
                        ddatx[i, 0] = (y) * 365 + mt * 30 + d + h / 24;
                }
                while (ddatx[0, 0] > 50)
                {
                    for (int k = 0; k < tcountdat; k++)
                    {
                        ddatx[k, 0] = ddatx[k, 0] - 50;
                    }
                }
                return true;
            }

        }
        /// <summary>
        /// ��ȡ���ݿ�
        /// </summary>
        /// <param name="cmpdata"></param>
        /// <param name="datasource"></param>
        /// <param name="dattime"></param>
        private void getdata(string startime, string datasource, string endtime)
        {
            string sqlstr = null;
            if ("��ָ��Ԥ��" == stragl)
            {
                sqlstr = "select top 200 *  from " + datasource + " where '" + startime + "'< time and time< '" + endtime + "' order by time";
            }
            else
            {
                sqlstr = "select *  from " + datasource + " where '" + startime + "'< time and time< '" + endtime + "' order by time";
            }
            SqlConnection sql = new SqlConnection(Global.sqlconstr);
            sql.Open();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(sqlstr, sql);
            da.Fill(dt);

            tcountdat = dt.Rows.Count > Prediction.LENGTH ? Prediction.LENGTH : dt.Rows.Count;
            if (0 == tcountdat)
            {
                MessageBox.Show("û������"); ;
            }
            else if (100 > tcountdat)
            {
                MessageBox.Show("��ȡ���ݲ���С��100�飬������ѡȡ");
            }
            else
            {
                dsvmdat = new double[tcountdat, 10];
                int i = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    if (Prediction.LENGTH <= i)
                    {
                        ;// MessageBox.Show("��ֵΪ��ֵ");
                    }
                    else
                    {
                        // ddatx[i][0]=double.Parse(dr[11].ToString());///��һ����ʱ��ı仯
                        ddatx[i, 1] = double.Parse(dr[1].ToString());
                        ddatx[i, 2] = double.Parse(dr[2].ToString());
                        ddatx[i, 3] = double.Parse(dr[3].ToString());
                        ddatx[i, 4] = double.Parse(dr[4].ToString());
                        ddatx[i, 5] = double.Parse(dr[5].ToString());
                        ddatx[i, 6] = double.Parse(dr[6].ToString());
                        ddatx[i, 7] = double.Parse(dr[7].ToString());
                        ddatx[i, 8] = double.Parse(dr[8].ToString());
                        ddatx[i, 9] = double.Parse(dr[9].ToString());
                        ddatx[i, 10] = double.Parse(dr[10].ToString());
                        timeList.Add(dr[11].ToString());

                        dsvmdat[i, 0] = double.Parse(dr[1].ToString());
                        dsvmdat[i, 1] = double.Parse(dr[2].ToString());
                        dsvmdat[i, 2] = double.Parse(dr[3].ToString());
                        dsvmdat[i, 3] = double.Parse(dr[4].ToString());
                        dsvmdat[i, 4] = double.Parse(dr[5].ToString());
                        dsvmdat[i, 5] = double.Parse(dr[6].ToString());
                        dsvmdat[i, 6] = double.Parse(dr[7].ToString());
                        dsvmdat[i, 7] = double.Parse(dr[8].ToString());
                        dsvmdat[i, 8] = double.Parse(dr[9].ToString());
                        dsvmdat[i, 9] = double.Parse(dr[10].ToString());

                        //mList.Add(double.Parse(dr[0].ToString()));
                        //timeList.Add(dr[1].ToString());
                        i++;
                    }
                }
            }
            sql.Close();
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            strdaynum = Convert.ToInt32(comboBox1.Text.ToString());
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            stragl = comboBox2.Text.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}