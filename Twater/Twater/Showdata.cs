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
    public partial class Showdata : Form
    {
        public Showdata()
        {
            InitializeComponent();
        }
        Point mousePoint;
        string cmpdat = null;

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(comboBox1.Text) == true)
            {
                MessageBox.Show("数据来源和数据参量不能为空");
            }
            else
            {
                tChart1.Series.Clear();
                tChart1.Header.Text = "数据对比";
                tChart1.Aspect.View3D = false;
                tChart1.Axes.Bottom.Labels.DateTimeFormat = "yyyy/MM/dd HH mm";
                tChart1.Axes.Bottom.Labels.MultiLine = true;

                Steema.TeeChart.Styles.Line line0 = new Steema.TeeChart.Styles.Line();
                line0.Title = "  ";
                line0.Color = Color.White;
                line0.XValues.DateTime = true;
                tChart1.Series.Add(line0);
                switch (comboBox1.SelectedIndex)
                {
                    case 0:
                        cmpdat = "PH";
                        break;
                    case 1:
                        cmpdat = "water_temp";
                        break;
                    case 2:
                        cmpdat = "NTU";
                        break;
                    case 3:
                        cmpdat = "O_dis";
                        break;
                    case 4:
                        cmpdat = "Chol";
                        break;
                    default:
                        break;
                }
                if (checkBox1.Checked)
                {
                    Steema.TeeChart.Styles.Line line1 = new Steema.TeeChart.Styles.Line();
                    paintline(line1, cmpdat, "T_station", Color.Red, "自动站数据");//T_station
                }
                if (checkBox2.Checked)
                {
                    Steema.TeeChart.Styles.Line line2 = new Steema.TeeChart.Styles.Line();
                    paintline(line2, cmpdat, "T_fubiao", Color.Green, "浮标数据");
                }
                if (checkBox3.Checked)
                {
                    Steema.TeeChart.Styles.Line line3 = new Steema.TeeChart.Styles.Line();
                    paintline(line3, cmpdat, "T_lmis", Color.DarkBlue, "实验室数据");
                }
            }
            //extraaxis();
        }

        private void Showdata_Load(object sender, EventArgs e)
        {
            checkBox1.Checked = true;
            checkBox2.Checked = true;
            checkBox3.Checked = true;
            tChart1.Header.Text = "数据对比";
            this.tChart1.Aspect.View3D = false;
            this.tChart1.Axes.Bottom.Labels.DateTimeFormat = "yyyy/mm/dd hh mm ss";
            this.tChart1.Axes.Bottom.Labels.MultiLine = true;
        }
        /// <summary>
        /// 画曲线，横坐标是时间
        /// </summary>
        /// <param name="line33"></param>
        /// <param name="cmpdata"></param>
        /// <param name="datasource"></param>
        /// <param name="tcolor"></param>
        /// <param name="tname"></param>
        private void paintline(Steema.TeeChart.Styles.Line line33, string cmpdata, string datasource,Color tcolor,string tname)
        {
            string sqlstr = "select " + cmpdata + ",time  from "+ datasource +"";
            SqlConnection sql = new SqlConnection(Global.sqlconstr);
            sql.Open();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(sqlstr, sql);
            da.Fill(dt);
            sql.Close();
            line33.Title = tname;
            line33.Color = tcolor;
            line33.DataSource = dt;
            line33.XValues.DateTime = true;
            line33.XValues.DataMember = "time";
            line33.YValues.DataMember = cmpdata;
            tChart1.Series.Add(line33);
        }
        /// <summary>
        /// 自定义的坐标轴，但是这次没有用上。换了一种方式实现
        /// </summary>
        private void extraaxis()
        {
            Steema.TeeChart.Styles.Line extraline = new Steema.TeeChart.Styles.Line();
            Steema.TeeChart.Axis extrax = new Steema.TeeChart.Axis();
            extraline.CustomVertAxis = extrax;
            extrax.StartPosition = 0;
            extrax.EndPosition = 100;
            extrax.Title.Text = "test1";
            extrax.AxisPen.Color = Color.Black;
            extrax.PositionUnits = Steema.TeeChart.PositionUnits.Percent;
            extrax.RelativePosition = 20;

            tChart1.Series.Add(extraline);
        }

        private void tChart1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                //extrax.RelativePosition = Control.MousePosition.Y - mousePoint.Y;
                //this.Top = Control.MousePosition.Y - mousePoint.Y;
                ///this.Left = Control.MousePosition.X - mousePoint.X;
            }
        }

        private void tChart1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.mousePoint.X = e.X;
                this.mousePoint.Y = e.Y;
            }
        }
      
    }
}