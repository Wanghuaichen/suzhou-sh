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
        double[,] ddatx = new double[1024, 11];
        double[,] dsvmdat;// = new double[1024, 10];
        List<string> timeList = new List<string>();
        object y = 0;
        int strdaynum;//预测天数
        int tcountdat;//数据条数
        string stragl;//算法
        double[] p=new double[]{1,100,100,1,100,1,1,1,100,50,100};
        public Prediction()
        {
            InitializeComponent();
        }

        private void Prediction_Load(object sender, EventArgs e)
        {

        }
        private void button1_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(comboBox1.Text) == true || string.IsNullOrEmpty(comboBox2.Text) == true)
            {
                MessageBox.Show("预测天数和算法不能为空");
            }
            else if(dateTimePicker2.Value>DateTime.Now)
            {
                MessageBox.Show("终止时间不能超过今天");
            }
            else
            {
                //double[,] dx = new double[10, 2] { { 1, 2 }, { 3, 4 }, { 5, 6 }, { 7, 8 }, { 9, 0 }, { 2, 3 }, { 4, 5 }, { 6, 7 }, { 8, 9 }, { 0, 1 } };
                //获取数据

                getdata(dateTimePicker1.Value.ToString(), "T_station_correct", dateTimePicker2.Value.ToString());
                changdat(timeList);
                //if()

                if ("支持向量机" == stragl)
                {
                    svma.svmaclassClass aaa = new svma.svmaclassClass();
                    aaa.SVMpredict(1, ref y, dsvmdat, strdaynum);
                    //svmnew.svmnewclassClass matlb = new svmnew.svmnewclassClass();
                    //matlb.SVMpredict(1, ref y, dsvmdat, strdaynum);
                }
                else
                {
                    xulie.xulieclassClass dd = new xulie.xulieclassClass();
                    dd.waterwlc2(1,ref y,ddatx,strdaynum,p);
                    //finalone.finaloneclassClass pp = new finalone.finaloneclassClass();
                    //pp.waterwlc2(1, ref y, ddatx, strdaynum, p);
                }
               



                //MessageBox.Show("qqqqqqqqqqq");
            }
        }
        /// <summary>
        /// 把数据都转成double型的数组，方便计算
        /// </summary>
        /// <param name="lmdat"></param>
        /// <param name="lmtimdat"></param>
        /// <returns></returns>
        private bool changdat(List<string> lmtimdat)
        {
            if (0 == lmtimdat.Count)
            {
                MessageBox.Show("库中不存在指定数据");
                return false;
            }
            else
            {
                String[] strdat = lmtimdat.ToArray();
                tcountdat = tcountdat > 1024 ? 1024 : tcountdat;
                for (int i = 0; i < tcountdat; i++)
                {
                    DateTime fdatetim;
                    fdatetim = DateTime.Parse(strdat[i].ToString());
                    double y = fdatetim.Year;
                    double mt = fdatetim.Month;
                    double d = fdatetim.Day;
                    double h = fdatetim.Hour;
                    if (1 == mt || 3 == mt || 5 == mt || 7 == mt || 8 == mt || 10 == mt || 12 == mt)
                        ddatx[i,0] = (y) * 365 + mt * 31 + d + h/24 ;
                    else if (2 == mt)
                        ddatx[i,0] = (y) * 365 + mt * 28 + d + h/24 ;
                    else
                        ddatx[i,0] = (y) * 365 + mt * 30 + d + h/24 ;
                }
                while (ddatx[0,0] > 50)
                {
                    for (int k = 0; k < tcountdat; k++)
                    {
                        ddatx[k,0] = ddatx[k,0] - 50;
                    }
                }
                return true;
            }

        }
        /// <summary>
        /// 读取数据库
        /// </summary>
        /// <param name="cmpdata"></param>
        /// <param name="datasource"></param>
        /// <param name="dattime"></param>
        private void getdata(string startime, string datasource, string endtime)
        {
            //select top 3 *from T_lmis where time<'2013-7-6 0:00:00'  order by ID desc 
            string sqlstr = "select *  from " + datasource + " where '" + startime + "'< time and time< '" + endtime + "'";
            SqlConnection sql = new SqlConnection(Global.sqlconstr);
            sql.Open();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(sqlstr, sql);
            da.Fill(dt);

            tcountdat = dt.Rows.Count>1024?1024:dt.Rows.Count;
            if (0 == tcountdat)
            {
                MessageBox.Show("没有数据"); ;
            }
            else if(100>tcountdat)
            {
                MessageBox.Show("所取数据不能小于100组，请重新选取");
            }
            else
            {
                dsvmdat = new double[tcountdat, 10];
                int i = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    if (1024 <= i)
                    {
                        ;// MessageBox.Show("该值为空值");
                    }
                    else
                    {
                       // ddatx[i][0]=double.Parse(dr[11].ToString());///第一个是时间的变化
                        ddatx[i,1] = double.Parse(dr[1].ToString());
                        ddatx[i,2] = double.Parse(dr[2].ToString());
                        ddatx[i,3] = double.Parse(dr[3].ToString());
                        ddatx[i,4] = double.Parse(dr[4].ToString());
                        ddatx[i,5] = double.Parse(dr[5].ToString());
                        ddatx[i,6] = double.Parse(dr[6].ToString());
                        ddatx[i,7] = double.Parse(dr[7].ToString());
                        ddatx[i,8] = double.Parse(dr[8].ToString());
                        ddatx[i,9] = double.Parse(dr[9].ToString());
                        ddatx[i,10] = double.Parse(dr[10].ToString());
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