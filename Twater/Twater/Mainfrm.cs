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
    public partial class Mainfrm : Form
    {
        private TabPage tbshowdata = null;
        private TabPage tbsearchdata = null;
        private TabPage tbreportdata = null;
        private ContextMenuStrip contextMenu = null;
        public Mainfrm()
        {
            InitializeComponent();
           
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Start.newform.ShowDialog(this);
            Global.connectsql();
            if (!Global.conet)
            {
                Application.Exit();
            }
           
            timer1.Start();
            timer2.Interval = 5000;
            timer2.Start();
            groupBox1.Visible = false;
            string AppPath = System.Windows.Forms.Application.StartupPath;
            axMap1.GeoSet = AppPath + @"\Tailake.gst"; //Tailake
            axMap1.TitleText = "供水水源地水质监测与预测预警系统";
            axMap1.Title.Visible = false;
            axMap1.CurrentTool = MapXLib.ToolConstants.miPanTool;//拖放
            //添加右键菜单
            contextMenu = new ContextMenuStrip(this.components);
            contextMenu.Items.Add("缩小");
            contextMenu.Items.Add("放大");
            contextMenu.Items.Add("拖动");
            contextMenu.Items.Add("以此为中心");
            contextMenu.Items.Add("添加文本");

            contextMenu.Items[0].Click += new System.EventHandler(this.tab1_Click);
            contextMenu.Items[1].Click += new System.EventHandler(this.tab2_Click);
            contextMenu.Items[2].Click += new System.EventHandler(this.tab3_Click);
            contextMenu.Items[3].Click += new System.EventHandler(this.tab4_Click);
            contextMenu.Items[4].Click += new System.EventHandler(this.tab5_Click);
            tabControl1.ContextMenuStrip = contextMenu;//指定窗体的右键菜单

            axMap1.MousewheelSupport = MapXLib.MousewheelSupportConstants.miFullMousewheelSupport;
            ////////////////需要开启一个定时器刷新显示

            double MapX = 120.3653, mapY = 31.2555;
            float screenX = 0, screenY = 0;
            axMap1.ConvertCoord(ref screenX, ref screenY, ref MapX, ref mapY,
                                MapXLib.ConversionConstants.miMapToScreen);

            label1.Left = (int)screenX;
            label1.Top = (int)screenY;
            label1.Text = "okok";
            //////////////////
        }
     
        /// <summary>
        /// 以下是地图右键快捷键响应事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tab1_Click(object sender, EventArgs e)
        {
            axMap1.CurrentTool = MapXLib.ToolConstants.miZoomOutTool;//缩小
        }
        private void tab2_Click(object sender, EventArgs e)
        {
            axMap1.CurrentTool = MapXLib.ToolConstants.miZoomInTool;//放大
        }
        private void tab3_Click(object sender, EventArgs e)
        {
            axMap1.CurrentTool = MapXLib.ToolConstants.miPanTool;//拖放
        }
        private void tab4_Click(object sender, EventArgs e)
        {
            axMap1.CurrentTool = MapXLib.ToolConstants.miCenterTool;//以此为中心
        }
        private void tab5_Click(object sender, EventArgs e)
        {
            axMap1.CurrentTool = MapXLib.ToolConstants.miTextTool;//添加文本
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            while (this.Opacity < 1)
            {
                this.Opacity += 0.1;
            }
            timer1.Stop();
        }

        private void showdata_Click(object sender, EventArgs e)
        {
            if (ErgodicModiForm("tbshowdata", tabControl1))
            {
                tbshowdata = new TabPage("数据显示");
                tbshowdata.Name = "tbshowdata";
                tabControl1.Controls.Add(tbshowdata);

                Showdata form = new Showdata();
                form.TopLevel = false;
                //form.BackColor = Color.White;
                form.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
                form.StartPosition = FormStartPosition.CenterScreen;
                form.FormBorderStyle = FormBorderStyle.None;
                form.WindowState = FormWindowState.Maximized;

                //添加右键菜单
                contextMenu = new ContextMenuStrip(this.components);
                contextMenu.Items.Add("关闭窗口");
                contextMenu.Items[0].Click += new System.EventHandler(this.contextMenuItem_Click);

                form.Show();
                tbshowdata.Controls.Add(form);

                form.ContextMenuStrip = contextMenu;  //指定窗体的右键菜单        
            }

            tabControl1.SelectedTab = tbshowdata;  
        }

        private void searchdata_Click(object sender, EventArgs e)
        {
            if (ErgodicModiForm("tbsearchdata", tabControl1))
            {
                tbsearchdata = new TabPage("数据查询");
                tbsearchdata.Name = "tbsearchdata";
                tabControl1.Controls.Add(tbsearchdata);

                Searchdata form = new Searchdata();
                form.TopLevel = false;
                //form.BackColor = Color.White;
                form.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
                form.StartPosition = FormStartPosition.CenterScreen;
                form.FormBorderStyle = FormBorderStyle.None;
                form.WindowState = FormWindowState.Maximized;

                //添加右键菜单
                contextMenu = new ContextMenuStrip(this.components);
                contextMenu.Items.Add("关闭窗口");
                contextMenu.Items[0].Click += new System.EventHandler(this.contextMenuItem_Click);
                form.Show();
                tbsearchdata.Controls.Add(form);

                form.ContextMenuStrip = contextMenu;  //指定窗体的右键菜单    
            }

            tabControl1.SelectedTab = tbsearchdata; 
        }

        private Boolean ErgodicModiForm(string MainTabControlKey, TabControl objTabControl)
        {
            //遍历选项卡判断是否存在该子窗体  
            foreach (Control con in objTabControl.Controls)
            {
                TabPage tab = (TabPage)con;
                if (tab.Name == MainTabControlKey)
                {
                    return false;//存在  
                }
            }
            return true;//不存在  
        }

        /// <summary>
        /// 右键菜单单击关闭窗体，关闭当前选项卡页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void contextMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Control i in this.tabControl1.Controls)
            {
                if (i.GetType() == typeof(Form))
                {
                    Form form = (Form)i;
                    form.Close();
                }
            }
            this.tabControl1.TabPages.RemoveAt(this.tabControl1.SelectedIndex);
        }

        private void menuabout_Click(object sender, EventArgs e)
        {
            About dlg = new About();
            dlg.ShowDialog();
        }

        //private void yibiao()
        //{
        //    tChart1.Series.Clear();
        //    tChart1.Header.Text = "数据对比";
        //    tChart1.Aspect.View3D = false;
        //    Steema.TeeChart.Styles.Gauges gauges = new Steema.TeeChart.Styles.Gauges();
        //    gauges.Pen.Color = Color.Blue;
        //    gauges.TotalAngle = 180; //圆的弧度
        //    gauges.RotationAngle = 180; //圆弧旋转角度
        //    gauges.HandStyle = Steema.TeeChart.Styles.HandStyle.Triangle;//指针样式
        //    gauges.Center.Style = Steema.TeeChart.Styles.PointerStyles.Sphere; //中心圆样式
        //    gauges.Center.HorizSize = 5; //中心圆的水平大小
        //    gauges.Center.VertSize = 5;//中心圆的垂直大小
        //    gauges.ShowInLegend = true;//显示图例
        //    gauges.HandDistance = 23;//指针长度
        //    //gauges.LabelsInside = true;//数据的现实内部or外部
        //    gauges.Value = 250; //指针指向的值
        //    gauges.Value = 360;
        //    gauges.Minimum = 0;//最小值
        //    gauges.Maximum = 1000;//最大值
        //    gauges.MinorTickDistance = 0;
        //    gauges.Chart.Axes.Left.AxisPen.Width = 15; //画笔宽度
        //    gauges.Chart.Axes.Left.AxisPen.Color = Color.FromArgb(215, 215, 215);
        //    gauges.Chart.Axes.Left.MinorTickCount = 5; //刻度值之间的刻度线数量
        //    gauges.Chart.Axes.Left.MinorTicks.Length = 10;//刻度值之间的刻度线长短
        //    gauges.Chart.Axes.Left.Ticks.Length = 20; //显示值的刻度线长短
        //    gauges.Chart.Axes.Left.Increment = 100; //刻度值的间隔大小
        //    gauges.Add(30, "日销售数量");

        //    gauges.Add(80, "月销售数量");
        //    tChart1.Series.Add(gauges);
        //}

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Prediction dlg = new Prediction();
            dlg.ShowDialog();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            timer2.Stop();
            Application.Exit();
        }

        private void Reportdat_Click(object sender, EventArgs e)
        {
            if (ErgodicModiForm("tbreportdata", tabControl1))
            {
                tbreportdata = new TabPage("数据报表");
                tbreportdata.Name = "tbreportdata";
                tabControl1.Controls.Add(tbreportdata);

                Reportdata form = new Reportdata();
                form.TopLevel = false;
                //form.BackColor = Color.White;
                form.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
                form.StartPosition = FormStartPosition.CenterScreen;
                form.FormBorderStyle = FormBorderStyle.None;
                form.WindowState = FormWindowState.Maximized;

                //添加右键菜单
                contextMenu = new ContextMenuStrip(this.components);
                contextMenu.Items.Add("关闭窗口");
                contextMenu.Items[0].Click += new System.EventHandler(this.contextMenuItem_Click);
                form.Show();
                tbreportdata.Controls.Add(form);

                form.ContextMenuStrip = contextMenu;  //指定窗体的右键菜单    
            }

            tabControl1.SelectedTab = tbreportdata; 
        }
        /// <summary>
        /// 重新定位站点位置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void axMap1_MapViewChanged(object sender, EventArgs e)
        {
            double MapX = 120.3653, mapY = 31.2555;
            float screenX = 0, screenY = 0;
            axMap1.ConvertCoord(ref screenX, ref screenY, ref MapX, ref mapY,
                                MapXLib.ConversionConstants.miMapToScreen);

            label1.Left = (int)screenX;
            label1.Top = (int)screenY;
            label1.Text = "o";

        }
        /// <summary>
        /// 定时刷新站点数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer2_Tick(object sender, EventArgs e)
        {
            SqlConnection sqlcon = new SqlConnection(Global.sqlconstr);
            SqlCommand sqlcommand = sqlcon.CreateCommand();
            sqlcon.Open();
            SqlDataReader dr1;
            sqlcommand.CommandText = "select * from T_station where time=( select max(time) as maxvalue from T_station)";
            dr1 = sqlcommand.ExecuteReader();
            if (dr1.Read())
            {
                //comboBox1.Text = dr1.GetValue(0).ToString();
                label12.Text = dr1.GetValue(1).ToString();
                label13.Text = dr1.GetValue(2).ToString();
                label14.Text = dr1.GetValue(3).ToString();
                label15.Text = dr1.GetValue(4).ToString();
                label16.Text = dr1.GetValue(5).ToString();
                label17.Text = dr1.GetValue(6).ToString();
                label18.Text = dr1.GetValue(7).ToString();
                label19.Text = dr1.GetValue(8).ToString();
                label20.Text = dr1.GetValue(9).ToString();
                label21.Text = dr1.GetValue(10).ToString();

            }
            sqlcon.Close();
        }
        /// <summary>
        /// 鼠标放到站点上面显示数据情况
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void label1_MouseHover(object sender, EventArgs e)
        {
            groupBox1.Left = label1.Right + 1;
            groupBox1.Top = label1.Top + 9;
            groupBox1.Visible = true;
         
        }

        private void label1_MouseLeave(object sender, EventArgs e)
        {
            groupBox1.Visible = false;
        }
        /// <summary>
        /// 在状态栏实时显示鼠标的经纬度
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void axMap1_MouseMoveEvent(object sender, AxMapXLib.CMapXEvents_MouseMoveEvent e)
        {
            double MapX = 0, mapY = 0;
            axMap1.ConvertCoord(ref e.x, ref e.y, ref MapX, ref mapY,
                               MapXLib.ConversionConstants.miScreenToMap);
            toolStripStatusLabel1.Text = MapX.ToString() + "," + mapY.ToString();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            DataFit dlg = new DataFit();
            dlg.ShowDialog();
        }

    }
}