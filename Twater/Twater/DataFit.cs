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
    public partial class DataFit : Form
    {
        string datsours1 = null;
        string cmpdat1 = null;
        //自动站校正数据参量
        double[] dtph=new double[1024];
        double[] dtouse=new double[1024];
        double[] dtwatmp=new double[1024];
        double[] dtntu=new double[1024];
        double[] dtann=new double[1024];
        double[] dtntotal=new double[1024];
        double[] dtptotal=new double[1024];
        double[] dtodis=new double[1024];
        double[] dtchol=new double[1024];
        double[] dtalgden=new double[1024];
        double[] dttime=new double[1024];//建模时间取自limis
        double[] dtnewtim=new double[1024];//具体取值所有时间，作为x
        int ddat;//具体所有时间数值个数
        //浮标校正数据参量
        double[] dfuph=new double[1024];
        double[] dfuwatmp=new double[1024];
        double[] dfuntu=new double[1024];
        double[] dfuodis=new double[1024];

        double[] dfulx=new double[1024];
        double[] dfullv=new double[1024];
        double[] dfuyls=new double[1024];
        double[] dfuavewind=new double[1024];
        double[] dfuwindspd=new double[1024];
        double[] dfutmp=new double[1024];
        double[] dfurelhumid=new double[1024];
        double[] dfupres=new double[1024];
        double[] dfurain=new double[1024];
        double[] dfuheattmp=new double[1024];
        double[] dfuheatv=new double[1024];
        double[] dfuconduc=new double[1024];
        double[] dfusolt=new double[1024];
        double[] dfutimedat=new double[1024];
        List<string> strfutimelist = new List<string>();
        String[] strfudatnew = new String[1024];//存数据库所用的时间字符串中间变量
        /// <summary>
        /// //
        /// </summary>
        String[] strdatnew=new String[1024];//存数据库所用的时间字符串中间变量
        //中间数据x,y
        List<double> mList = new List<double>();
        List<string> timeList = new List<string>();
        List<string> newtimeList = new List<string>();
        double[] mlist=new double[1024];
        double[] timelist=new double[1024];
        double dnewdat;//最后结果
        double dnewtim;//时间的double值
        DateTime fdatetim;
        int tcountdat;
        int tallcoutdat;
       
        public DataFit()
        {
            InitializeComponent();
        }

        private void DataFit_Load(object sender, EventArgs e)
        {
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "yyyy-MM-dd:hh-mm";
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    comboBox2.Items.Add("PH");
                    comboBox2.Items.Add("耗氧量");
                    comboBox2.Items.Add("水温");
                    comboBox2.Items.Add("浊度");
                    comboBox2.Items.Add("氨氮");
                    comboBox2.Items.Add("总氮");
                    comboBox2.Items.Add("总磷");
                    comboBox2.Items.Add("溶解氧");
                    comboBox2.Items.Add("叶绿素");
                    comboBox2.Items.Add("藻密度");
                    break;
                case 1:
                    comboBox2.Items.Add("光照度");
                    comboBox2.Items.Add("蓝绿藻");
                    comboBox2.Items.Add("叶绿素");
                    comboBox2.Items.Add("平均风向");
                    comboBox2.Items.Add("平均风速");
                    comboBox2.Items.Add("气温");
                    comboBox2.Items.Add("相对湿度");
                    comboBox2.Items.Add("气压");
                    comboBox2.Items.Add("雨量累计");
                    comboBox2.Items.Add("加热温度");
                    comboBox2.Items.Add("加热电压");
                    comboBox2.Items.Add("水温");
                    comboBox2.Items.Add("电导率");
                    comboBox2.Items.Add("浊度");
                    comboBox2.Items.Add("PH");
                    comboBox2.Items.Add("溶解氧");
                    comboBox2.Items.Add("盐度");
                    break;
                case 2:
                    comboBox2.Items.Add("PH");
                    comboBox2.Items.Add("耗氧量");
                    comboBox2.Items.Add("水温");
                    comboBox2.Items.Add("浊度");
                    comboBox2.Items.Add("氨氮");
                    comboBox2.Items.Add("总氮");
                    comboBox2.Items.Add("总磷");
                    comboBox2.Items.Add("溶解氧");
                    comboBox2.Items.Add("叶绿素");
                    comboBox2.Items.Add("藻密度");
                    comboBox2.Items.Add("光照度");
                    comboBox2.Items.Add("平均风向");
                    comboBox2.Items.Add("平均风速");
                    comboBox2.Items.Add("气温");
                    comboBox2.Items.Add("相对湿度");
                    comboBox2.Items.Add("气压");
                    comboBox2.Items.Add("雨量累计");
                    comboBox2.Items.Add("加热温度");
                    comboBox2.Items.Add("电导率");
                    comboBox2.Items.Add("盐度");
                    break;
            }
        }

        private void chk()
        {
            switch (comboBox1.SelectedIndex)
            {
                #region
                case 0:
                    datsours1 = "T_station_correct";
                    switch (comboBox2.SelectedIndex)
                    {
                        case 0:
                            cmpdat1 = "PH";
                            break;
                        case 1:
                            cmpdat1 = "O_use";
                            break;
                        case 2:
                            cmpdat1 = "water_temp";
                            break;
                        case 3:
                            cmpdat1 = "NTU";
                            break;
                        case 4:
                            cmpdat1 = "AN_N";
                            break;
                        case 5:
                            cmpdat1 = "N_total";
                            break;
                        case 6:
                            cmpdat1 = "P_total";
                            break;
                        case 7:
                            cmpdat1 = "O_dis";
                            break;
                        case 8:
                            cmpdat1 = "Chol";
                            break;
                        case 9:
                            cmpdat1 = "Alg_den";
                            break;
                    }
                    break;
                #endregion
                #region
                case 1:
                    datsours1 = "T_fubiao_correct";
                    switch (comboBox2.SelectedIndex)
                    {
                        case 0:
                            cmpdat1 = "LX";
                            break;
                        case 1:
                            cmpdat1 = "LLV";
                            break;
                        case 2:
                            cmpdat1 = "YLS";
                            break;
                        case 3:
                            cmpdat1 = "Ave_wind";
                            break;
                        case 4:
                            cmpdat1 = "Wind_spd";
                            break;
                        case 5:
                            cmpdat1 = "Temp";
                            break;
                        case 6:
                            cmpdat1 = "Rel_humid";
                            break;
                        case 7:
                            cmpdat1 = "Pres";
                            break;
                        case 8:
                            cmpdat1 = "Rain";
                            break;
                        case 9:
                            cmpdat1 = "Heat_temp";
                            break;
                        case 10:
                            cmpdat1 = "Heat_v";
                            break;
                        case 11:
                            cmpdat1 = "Water_temp";
                            break;
                        case 12:
                            cmpdat1 = "Conduc";
                            break;
                        case 13:
                            cmpdat1 = "NTU";
                            break;
                        case 14:
                            cmpdat1 = "PH";
                            break;
                        case 15:
                            cmpdat1 = "O_disv";
                            break;
                        case 16:
                            cmpdat1 = "Solt";
                            break;
                    }
                    break;
                #endregion
                #region
                case 3:
                    datsours1 = "T_lmis";
                    switch (comboBox2.SelectedIndex)
                    {

                        case 0:
                            cmpdat1 = "PH";
                            break;
                        case 1:
                            cmpdat1 = "O_use";
                            break;
                        case 2:
                            cmpdat1 = "water_temp";
                            break;
                        case 3:
                            cmpdat1 = "NTU";
                            break;
                        case 4:
                            cmpdat1 = "AN_N";
                            break;
                        case 5:
                            cmpdat1 = "N_total";
                            break;
                        case 6:
                            cmpdat1 = "P_total";
                            break;
                        case 7:
                            cmpdat1 = "O_dis";
                            break;
                        case 8:
                            cmpdat1 = "Chol";
                            break;
                        case 9:
                            cmpdat1 = "Alg_den";
                            break;
                        case 10:
                            cmpdat1 = "LX";
                            break;
                        case 11:
                            cmpdat1 = "Ave_wind";
                            break;
                        case 12:
                            cmpdat1 = "Wind_spd";
                            break;
                        case 13:
                            cmpdat1 = "Temp";
                            break;
                        case 14:
                            cmpdat1 = "Rel_humid";
                            break;
                        case 15:
                            cmpdat1 = "Pres";
                            break;
                        case 16:
                            cmpdat1 = "Rain";
                            break;
                        case 17:
                            cmpdat1 = "Heat_temp";
                            break;
                        case 18:
                            cmpdat1 = "Conduc";
                            break;
                        case 19:
                            cmpdat1 = "Solt";
                            break;
                    }
                    break;
                #endregion
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(comboBox1.Text) == true  || string.IsNullOrEmpty(comboBox2.Text)==true)
            {
                MessageBox.Show("数据来源和数据参量不能为空");
            }
            else
            {
                chk();
                string strlims = "T_lmis";
                string dattim = dateTimePicker1.Value.ToString();
                getdata(cmpdat1, strlims, dattim);//加个时间
                if (changdat(mList, timeList))
                {
                    double[] fcan;
                    fcan = Datafiting.MultiLine(timelist, mlist, tcountdat, 2);
                    dnewdat = getresult(fcan, dattim);//时间必须是浮标或者自动站的时间数据，不能是limis的数据
                    textBox1.Text = comboBox1.Text + "-" + comboBox2.Text + "的校正值为：" + dnewdat.ToString();

                    if (MessageBox.Show((comboBox1.Text + "-" + comboBox2.Text + "的校正值为：" + dnewdat.ToString() + "，是否保存到数据库？"), "Confirm Message", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                    {
                        stroredat(cmpdat1, datsours1, dnewdat, dateTimePicker1.Value);
                        if (MessageBox.Show("是否退出？", "Confirm Message", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                        {
                            this.Close();
                        }
                    }
                }
            }
        }
        
        /// <summary>
        /// 从数据库得到数据
        /// </summary>
        /// <param name="cmpdata"></param>
        /// <param name="datasource"></param>
        private void getdata(string cmpdata,string datasource,string dattime)
        {
            //select top 3 *from T_lmis where time<'2013-7-6 0:00:00'  order by ID desc 
            string sqlstr = "select top 20 " + cmpdata + ",time  from " + datasource + " where time< '"+ dattime +"' order by ID desc";
            SqlConnection sql = new SqlConnection(Global.sqlconstr);
            sql.Open();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(sqlstr, sql);
            da.Fill(dt);
          
            tcountdat = dt.Rows.Count;
            if (0 == tcountdat)
            {
                ;
            }
            else
            {
                int i = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    if (string.IsNullOrEmpty(dr[0].ToString()))
                    {
                        ;// MessageBox.Show("该值为空值");
                    }
                    else
                    {
                        mList.Add(double.Parse(dr[0].ToString()));
                        timeList.Add(dr[1].ToString());
                        i++;
                    }
                }
            }
            sql.Close();
        }

        #region 自动站数据校正
        /// <summary>
        /// 从数据库得到所有数据
        /// </summary>建模数据
        /// <param name="cmpdata"></param>
        /// <param name="datasource"></param>
        public void getalldata(string datasource, string dattime)
        {
            string sqlstr = "select top 500 PH,water_temp,NTU,AN_N,N_total,O_dis,time  from " + datasource + " where time < '" + dattime + "' order by ID desc";//  order by ID desc
            SqlConnection sql = new SqlConnection(Global.sqlconstr);
            sql.Open();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(sqlstr, sql);
            da.Fill(dt);

            tcountdat = dt.Rows.Count>1024?1024:dt.Rows.Count;
            if (0 == tcountdat)
            {
                ;
            }
            else
            {
                int i = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    if (string.IsNullOrEmpty(dr[0].ToString()))
                    {
                        ;// MessageBox.Show("该值为空值");
                    }
                    else
                    {
                        if (tcountdat > i)
                        {
                            dtph[i] = double.Parse(dr[0].ToString());
                            dtwatmp[i] = double.Parse(dr[1].ToString());
                            dtntu[i] = double.Parse(dr[2].ToString());
                            dtann[i] = double.Parse(dr[3].ToString());
                            dtntotal[i] = double.Parse(dr[4].ToString());
                            dtodis[i] = double.Parse(dr[5].ToString());
                            timeList.Add(dr[6].ToString());
                            i++;
                        }
                    }
                }
            }
            sql.Close();
        }
        /// <summary>
        /// 一键校正所有数据
        /// </summary>
        public void fitalldata()
        {
            ////循环
            string dattim1 = DateTime.Now.ToString();
            getalldata("T_lmis", dattim1);//加个时间
            if (allchangdat(timeList))
            {
                double[] fcan;
                getalltimedata("T_station");
                allchangdattt(newtimeList);
                //循环
                fcan = Datafiting.MultiLine(dttime, dtph, tcountdat, 2);
                dtph = getallresult(fcan, dtnewtim);//时间必须是浮标或者自动站的时间数据，不能是limis的数据 //时间从数据库里面取得

                fcan = Datafiting.MultiLine(dttime, dtwatmp, tcountdat, 2);
                dtwatmp = getallresult(fcan, dtnewtim);//时间必须是浮标或者自动站的时间数据，不能是limis的数据 //时间从数据库里面取得

                fcan = Datafiting.MultiLine(dttime, dtntu, tcountdat, 2);
                dtntu = getallresult(fcan, dtnewtim);//时间必须是浮标或者自动站的时间数据，不能是limis的数据 //时间从数据库里面取得

                fcan = Datafiting.MultiLine(dttime, dtann, tcountdat, 2);
                dtann = getallresult(fcan, dtnewtim);//时间必须是浮标或者自动站的时间数据，不能是limis的数据 //时间从数据库里面取得

                fcan = Datafiting.MultiLine(dttime, dtntotal, tcountdat, 2);
                dtntotal = getallresult(fcan, dtnewtim);//时间必须是浮标或者自动站的时间数据，不能是limis的数据 //时间从数据库里面取得

                fcan = Datafiting.MultiLine(dttime, dtodis, tcountdat, 2);
                dtodis = getallresult(fcan, dtnewtim);//时间必须是浮标或者自动站的时间数据，不能是limis的数据 //时间从数据库里面取得

                allstroredat("T_station_correct");
                MessageBox.Show("校正完毕");
            }
        }
        /// <summary>
        /// 得到所有结果
        /// </summary>
        /// <param name="cand"></param>
        /// <param name="cmptim"></param>
        /// <returns></returns>
        private double[] getallresult(double[] cand, double[] dcmptim)
        {
            double[] ddddat = new double[ddat];
            for (int i = 0; i < ddat; i++)
            {
                ddddat[i] = cand[0] + cand[1] * dcmptim[i] + cand[2] * dcmptim[i] * dcmptim[i];
            }
            return ddddat;
        }
        /// <summary>
        /// 得到时间数据
        /// </summary>
        /// <param name="datasource"></param>
        /// <param name="dattime"></param>
        public void getalltimedata(string datasource)
        {
            string sqlstr = "select top 1024 time,O_use,P_total,Chol,Alg_den from " + datasource + "  order by ID desc";
            SqlConnection sql = new SqlConnection(Global.sqlconstr);
            sql.Open();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(sqlstr, sql);
            da.Fill(dt);
            ddat = dt.Rows.Count>1024?1024:dt.Rows.Count;
            if (0 == ddat)
            {
                ;
            }
            else
            {
                int i = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    if (string.IsNullOrEmpty(dr[0].ToString()))
                    {
                        ;// MessageBox.Show("该值为空值");
                    }
                    else
                    {
                        //dtnewtim[i] = double.Parse(dr[0].ToString());
                        if(ddat>i)
                        {
                            newtimeList.Add(dr[0].ToString());
                            dtouse[i] = double.Parse(dr[1].ToString());
                            dtptotal[i] = double.Parse(dr[2].ToString());
                            dtchol[i] = double.Parse(dr[3].ToString());
                            dtalgden[i] = double.Parse(dr[4].ToString());
                            i++;
                        }
                    }
                }
            }
            sql.Close();
        }
        /// <summary>
        /// 把时间换算成数值
        /// </summary>
        /// <param name="lmtimdat"></param>
        /// <returns></returns>
        public bool allchangdat(List<string> lmtimdat)
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
                    fdatetim = DateTime.Parse(strdat[i].ToString());
                    double y = fdatetim.Year;
                    double mt = fdatetim.Month;
                    double d = fdatetim.Day;
                    double h = fdatetim.Hour;
                    double mm = fdatetim.Minute;
                    if (1 == mt || 3 == mt || 5 == mt || 7 == mt || 8 == mt || 10 == mt || 12 == mt)
                        dttime[i] = (y * 365 + mt * 31 + d) * 24 + h + mm / 60;
                    else if (2 == mt)
                        dttime[i] = (y * 365 + mt * 28 + d) * 24 + h + mm / 60;
                    else
                        dttime[i] = (y * 365 + mt * 30 + d) * 24 + h + mm / 60;
                }
                return true;
            }

        }
        /// <summary>
        /// 换算时间,real data
        /// </summary>
        /// <param name="lmtimdat"></param>
        /// <returns></returns>
        public bool allchangdattt(List<string> lmtimdat)
        {
            if (0 == lmtimdat.Count)
            {
                MessageBox.Show("库中不存在指定数据");
                return false;
            }
            else
            {
                strdatnew = lmtimdat.ToArray();
                tallcoutdat = ddat > 1024 ? 1024 : ddat;
                for (int i = 0; i < tallcoutdat; i++)
                {
                    fdatetim = DateTime.Parse(strdatnew[i].ToString());
                    double y = fdatetim.Year;
                    double mt = fdatetim.Month;
                    double d = fdatetim.Day;
                    double h = fdatetim.Hour;
                    double mm = fdatetim.Minute;
                    if (1 == mt || 3 == mt || 5 == mt || 7 == mt || 8 == mt || 10 == mt || 12 == mt)
                        dtnewtim[i] = (y * 365 + mt * 31 + d) * 24 + h + mm / 60;
                    else if (2 == mt)
                        dtnewtim[i] = (y * 365 + mt * 28 + d) * 24 + h + mm / 60;
                    else
                        dtnewtim[i] = (y * 365 + mt * 30 + d) * 24 + h + mm / 60;
                }
                return true;
            }

        }
#endregion

        #region 浮标数据校正
        public void fufitalldata()
        {
            ////循环
            string dattim1 = DateTime.Now.ToString();
            fugetalldata("T_lmis", dattim1);//加个时间
            if (allchangdat(timeList))
            {
                double[] fcan;
                fugetalltimedata("T_fubiao");
                fuallchangdattt(strfutimelist);
                //循环
                fcan = Datafiting.MultiLine(dttime, dfuph, tcountdat, 2);
                dfuph = getallresult(fcan, dfutimedat);//时间必须是浮标或者自动站的时间数据，不能是limis的数据 //时间从数据库里面取得

                fcan = Datafiting.MultiLine(dttime, dfuwatmp, tcountdat, 2);
                dfuwatmp = getallresult(fcan, dfutimedat);//时间必须是浮标或者自动站的时间数据，不能是limis的数据 //时间从数据库里面取得

                fcan = Datafiting.MultiLine(dttime, dfuntu, tcountdat, 2);
                dfuntu = getallresult(fcan, dfutimedat);//时间必须是浮标或者自动站的时间数据，不能是limis的数据 //时间从数据库里面取得

                fcan = Datafiting.MultiLine(dttime, dfuodis, tcountdat, 2);
                dfuodis = getallresult(fcan, dfutimedat);//时间必须是浮标或者自动站的时间数据，不能是limis的数据 //时间从数据库里面取得

                fuallstroredat("T_fubiao_correct");
                MessageBox.Show("校正完毕");
            }
        }

        public void fugetalldata(string datasource, string dattime)
        {
            string sqlstr = "select top 500 PH,water_temp,NTU,O_dis,time  from " + datasource + " where time < '" + dattime + "' order by ID desc";//  order by ID desc
            SqlConnection sql = new SqlConnection(Global.sqlconstr);
            sql.Open();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(sqlstr, sql);
            da.Fill(dt);

            tcountdat = dt.Rows.Count > 1024 ? 1024 : dt.Rows.Count;
            if (0 == tcountdat)
            {
                ;
            }
            else
            {
                int i = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    if (string.IsNullOrEmpty(dr[0].ToString()))
                    {
                        ;// MessageBox.Show("该值为空值");
                    }
                    else
                    {
                        if (tcountdat > i)
                        {
                            dfuph[i] = double.Parse(dr[0].ToString());
                            dfuwatmp[i] = double.Parse(dr[1].ToString());
                            dfuntu[i] = double.Parse(dr[2].ToString());
                            dfuodis[i] = double.Parse(dr[3].ToString());
                            timeList.Add(dr[4].ToString());
                            i++;
                        }
                    }
                }
            }
            sql.Close();
        }

        public void fugetalltimedata(string datasource)
        {
            string sqlstr = "select top 1024 time,LX,LLV,YLS,Ave_wind,Wind_spd,Temp,Rel_humid,Pres,Rain,Heat_temp,Heat_v,Conduc,Solt from " + datasource + "  order by ID desc";
            SqlConnection sql = new SqlConnection(Global.sqlconstr);
            sql.Open();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(sqlstr, sql);
            da.Fill(dt);
            ddat = dt.Rows.Count > 1024 ? 1024 : dt.Rows.Count;
            if (0 == ddat)
            {
                ;
            }
            else
            {
                int i = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    if (string.IsNullOrEmpty(dr[0].ToString()))
                    {
                        ;// MessageBox.Show("该值为空值");
                    }
                    else
                    {
                        //dtnewtim[i] = double.Parse(dr[0].ToString());
                        if (ddat > i)
                        {
                            strfutimelist.Add(dr[0].ToString());
                            dfulx[i] = double.Parse(dr[1].ToString());
                            dfullv[i] = double.Parse(dr[2].ToString());
                            dfuyls[i] = double.Parse(dr[3].ToString());
                            dfuavewind[i] = double.Parse(dr[4].ToString());
                            dfuwindspd[i] = double.Parse(dr[5].ToString());
                            dfutmp[i] = double.Parse(dr[6].ToString());
                            dfurelhumid[i] = double.Parse(dr[7].ToString());
                            dfupres[i] = double.Parse(dr[8].ToString());
                            dfurain[i] = double.Parse(dr[9].ToString());
                            dfuheattmp[i] = double.Parse(dr[10].ToString());
                            dfuheatv[i] = double.Parse(dr[11].ToString());
                            dfuconduc[i] = double.Parse(dr[12].ToString());
                            dfusolt[i] = double.Parse(dr[13].ToString());
                            i++;
                        }
                    }
                }
            }
            sql.Close();
        }

        public bool fuallchangdattt(List<string> lmtimdat)
        {
            if (0 == lmtimdat.Count)
            {
                MessageBox.Show("库中不存在指定数据");
                return false;
            }
            else
            {
                strfudatnew = lmtimdat.ToArray();
                tallcoutdat = ddat > 1024 ? 1024 : ddat;
                for (int i = 0; i < tallcoutdat; i++)
                {
                    fdatetim = DateTime.Parse(strfudatnew[i].ToString());
                    double y = fdatetim.Year;
                    double mt = fdatetim.Month;
                    double d = fdatetim.Day;
                    double h = fdatetim.Hour;
                    double mm = fdatetim.Minute;
                    if (1 == mt || 3 == mt || 5 == mt || 7 == mt || 8 == mt || 10 == mt || 12 == mt)
                        dfutimedat[i] = (y * 365 + mt * 31 + d) * 24 + h + mm / 60;
                    else if (2 == mt)
                        dfutimedat[i] = (y * 365 + mt * 28 + d) * 24 + h + mm / 60;
                    else
                        dfutimedat[i] = (y * 365 + mt * 30 + d) * 24 + h + mm / 60;
                }
                return true;
            }

        }


        #endregion
        #region private_only
        /// <summary>
        /// 存储所有数据
        /// </summary>
        /// <param name="cmpdata"></param>
        /// <param name="datasource"></param>
        /// <param name="dnewdat"></param>
        /// <param name="newtm"></param>
        private void allstroredat(string datasource)
        {
            SqlConnection sqlcon = new SqlConnection(Global.sqlconstr);
            SqlCommand sqlcommand = sqlcon.CreateCommand();
            sqlcon.Open();
            for (int k = 0; k < ddat; k++)
            {
                sqlcommand.CommandText = "insert  into " + datasource + "(PH,O_use,water_temp,NTU,AN_N,N_total,P_total,O_dis,Chol,Alg_den,time) values ('" + dtph[k].ToString() + "','" + dtouse[k].ToString() + "','" + dtwatmp[k].ToString() + "','" + dtntu[k].ToString() + "','" + dtann[k].ToString() + "','" + dtntotal[k].ToString() + "','" + dtptotal[k].ToString() + "','" + dtodis[k].ToString() + "','" + dtchol[k].ToString() + "','" + dtalgden[k].ToString() + "','" + DateTime.Parse(strdatnew[k]) + "')";
                sqlcommand.ExecuteNonQuery();
            }
            sqlcommand.Dispose();
            sqlcon.Close();
        }

        private void fuallstroredat(string datasource)
        {
            SqlConnection sqlcon = new SqlConnection(Global.sqlconstr);
            SqlCommand sqlcommand = sqlcon.CreateCommand();
            sqlcon.Open();
            for (int k = 0; k < ddat; k++)
            {
                string cmdtxt= "insert  into " + datasource + "(LX,LLV,YLS,Ave_wind,Wind_spd,Temp,Rel_humid,Pres,Rain,Heat_temp,Heat_v,Water_temp,Conduc,NTU,PH,O_disv,Solt,time) values ";
                cmdtxt += "('" + dfulx[k].ToString() + "','" + dfullv[k].ToString() + "','" + dfuyls[k].ToString() + "',";
                cmdtxt += "'" + dfuavewind[k].ToString() + "','" + dfuwindspd[k].ToString() + "','" + dfutmp[k].ToString() + "',";
                cmdtxt += "'" + dfurelhumid[k].ToString() + "','" + dfupres[k].ToString() + "','" + dfurain[k].ToString() + "',";
                cmdtxt += "'" + dfuheattmp[k].ToString() + "','" + dfuheatv[k].ToString() + "','" + dfuwatmp[k].ToString() + "',";
                cmdtxt += "'" + dfuconduc[k].ToString() + "','" + dfuntu[k].ToString() + "','" + dfuph[k].ToString() + "',";
                cmdtxt += "'" + dfuodis[k].ToString() + "','" + dfusolt[k].ToString() + "',";
                cmdtxt += "'" + DateTime.Parse(strfudatnew[k]) + "')";

                sqlcommand.CommandText = cmdtxt;
                sqlcommand.ExecuteNonQuery();
            }
            sqlcommand.Dispose();
            sqlcon.Close();
        }
        /// <summary>
        /// 把数据都转成double型的数组，方便计算
        /// </summary>
        /// <param name="lmdat"></param>
        /// <param name="lmtimdat"></param>
        private bool changdat(List<double> lmdat, List<string> lmtimdat)
        {
            if (0 == lmdat.Count || 0 == lmtimdat.Count)
            {
                MessageBox.Show("库中不存在指定数据");
                return false;
            }
            else
            {
                mlist = lmdat.ToArray();
                String[] strdat = lmtimdat.ToArray();
                tcountdat = tcountdat > 1024 ? 1024 : tcountdat;
                for (int i = 0; i < tcountdat; i++)
                {
                    fdatetim = DateTime.Parse(strdat[i].ToString());
                    double y = fdatetim.Year;
                    double mt = fdatetim.Month;
                    double d = fdatetim.Day;
                    double h = fdatetim.Hour;
                    double mm = fdatetim.Minute;
                    if (1 == mt || 3 == mt || 5 == mt || 7 == mt || 8 == mt || 10 == mt || 12 == mt)
                        timelist[i] = (y * 365 + mt * 31 + d) * 24 + h + mm / 60;
                    else if (2 == mt)
                        timelist[i] = (y * 365 + mt * 28 + d) * 24 + h + mm / 60;
                    else
                        timelist[i] = (y * 365 + mt * 30 + d) * 24 + h + mm / 60;
                }
                return true;
            }
            
        }
        /// <summary>
        /// y=a0+a1*x+a2*x*x,求接Y值并存储到数组中
        /// </summary>
        /// <param name="cand"></param>
        /// <param name="cmpdata"></param>
        /// <returns></returns>
        private double getresult(double[] cand, string cmptim)
        {
            double dcptim;
            fdatetim = DateTime.Parse(cmptim.ToString());
            double y = fdatetim.Year;
            double mt = fdatetim.Month;
            double d = fdatetim.Day;
            double h = fdatetim.Hour;
            double mm = fdatetim.Minute;
            if (1 == mt || 3 == mt || 5 == mt || 7 == mt || 8 == mt || 10 == mt || 12 == mt)
                dcptim = (y * 365 + mt * 31 + d) * 24 + h + mm / 60;
            else if (2 == mt)
                dcptim = (y * 365 + mt * 28 + d) * 24 + h + mm / 60;
            else
                dcptim = (y * 365 + mt * 30 + d) * 24 + h + mm / 60;

            dnewtim=cand[0] + cand[1] * dcptim + cand[2] * dcptim * dcptim;
            return dnewtim;
        }
        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="cmpdata"></param>
        /// <param name="datasource"></param>
        /// <param name="dnewdat"></param>
        /// <param name="newtm"></param>
        private void stroredat(string cmpdata, string datasource, double dnewdat, DateTime newtm)
        {
            SqlConnection sqlcon = new SqlConnection(Global.sqlconstr);
            SqlCommand sqlcommand = sqlcon.CreateCommand();
            sqlcon.Open();
            sqlcommand.CommandText = "insert  into " + datasource + "(" + cmpdata + ",time) values ('" + dnewdat.ToString() + "','" + newtm + "')";
            sqlcommand.ExecuteNonQuery();
            sqlcommand.Dispose();
            sqlcon.Close();
        }
        #endregion
        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}