using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.IO;
using System.Threading;
using System.Diagnostics;

namespace Twater
{
    public delegate void SetResultEventHandler(bool ret);
    public delegate void SetWaitEventHandler(bool ret);

    public partial class Mainfrm : Form
    {
        /// <summary>
        /// �㷨
        /// </summary>

        private WaitingDlg waitDlg = new WaitingDlg();

        private Log moniterLog;

        double[] p = new double[] { 1, 100, 100, 1, 100, 1, 1, 1, 100, 50, 100 };
        public static double[,] dsvmdat;
        object y = 0;
        int tcountdat;//��������

        String fileName;
        String extendedName;
        String fileName1;

        private TabPage tbshowdata = null;
        private TabPage tbsearchdata = null;
        private TabPage tbreportdata = null;
        private ContextMenuStrip contextMenu = null;
        public Mainfrm()
        {
            InitializeComponent();
            Predic.InstanceObj.setRetEventHandler += new SetResultEventHandler(this.SetResult);
            Predic.InstanceObj.setWaitEventHandler += new SetWaitEventHandler(this.SetWait);

            string strPath = System.Windows.Forms.Application.StartupPath + "\\Log\\";
            if (Directory.Exists(strPath) == false)
            {
                Directory.CreateDirectory(strPath);
            }
            this.moniterLog = new Log(strPath, LogType.Daily);
        }

        private void CheckAutoPredicteTime()
        {
            bool result = Global.GetAutoPredicteTime();
            if (result == false)           
            {
                Global.autoPredicteTime = 30;
                Global.SetAutoPredicteTime();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Start.newform.ShowDialog(this);

            Global.connectsql();
            if (!Global.conet)
            {
                Application.Exit();
                moniterLog.Write("Connect Sql failed,Application Exit", MsgType.Success);
            }
            CheckAutoPredicteTime();

            timer1.Start();
            timer2.Interval = 5000;
            timer2.Start();
            groupBox1.Visible = false;
            FormInit();
            moniterLog.Write("The Mainfrm has been running", MsgType.Success);
            //////////////////
        }

        private void FormInit()
        {
            string AppPath = System.Windows.Forms.Application.StartupPath;
            axMap1.GeoSet = AppPath + @"\Tailake.gst"; //Tailake
            axMap1.TitleText = "��ˮˮԴ��ˮ�ʼ����Ԥ��Ԥ��ϵͳ";
            axMap1.Title.Visible = false;
            axMap1.CurrentTool = MapXLib.ToolConstants.miPanTool;//�Ϸ�
            axMap1.Show();
            //����Ҽ��˵�
            contextMenu = new ContextMenuStrip(this.components);
            contextMenu.Items.Add("��С");
            contextMenu.Items.Add("�Ŵ�");
            contextMenu.Items.Add("�϶�");
            contextMenu.Items.Add("�Դ�Ϊ����");
            contextMenu.Items.Add("����ı�");

            contextMenu.Items[0].Click += new System.EventHandler(this.tab1_Click);
            contextMenu.Items[1].Click += new System.EventHandler(this.tab2_Click);
            contextMenu.Items[2].Click += new System.EventHandler(this.tab3_Click);
            contextMenu.Items[3].Click += new System.EventHandler(this.tab4_Click);
            contextMenu.Items[4].Click += new System.EventHandler(this.tab5_Click);
            tabControl1.ContextMenuStrip = contextMenu;//ָ��������Ҽ��˵�

            axMap1.MousewheelSupport = MapXLib.MousewheelSupportConstants.miFullMousewheelSupport;
            ////////////////��Ҫ����һ����ʱ��ˢ����ʾ

            double MapX = 120.37956, mapY = 31.37842;
            float screenX = 0, screenY = 0;
            axMap1.ConvertCoord(ref screenX, ref screenY, ref MapX, ref mapY,
                                MapXLib.ConversionConstants.miMapToScreen);

            label1.Left = (int)screenX;
            label1.Top = (int)screenY;
            label1.Text = "O";
        }

        /// <summary>
        /// �����ǵ�ͼ�Ҽ���ݼ���Ӧ�¼�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tab1_Click(object sender, EventArgs e)
        {
            axMap1.CurrentTool = MapXLib.ToolConstants.miZoomOutTool;//��С
        }
        private void tab2_Click(object sender, EventArgs e)
        {
            axMap1.CurrentTool = MapXLib.ToolConstants.miZoomInTool;//�Ŵ�
        }
        private void tab3_Click(object sender, EventArgs e)
        {
            axMap1.CurrentTool = MapXLib.ToolConstants.miPanTool;//�Ϸ�
        }
        private void tab4_Click(object sender, EventArgs e)
        {
            axMap1.CurrentTool = MapXLib.ToolConstants.miCenterTool;//�Դ�Ϊ����
        }
        private void tab5_Click(object sender, EventArgs e)
        {
            axMap1.CurrentTool = MapXLib.ToolConstants.miTextTool;//����ı�
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            while (this.Opacity < 1)
            {
                this.Opacity += 0.1;
            }
            timer1.Stop();
            timer1.Enabled = false;
            moniterLog.Write("Timer1 is disabled", MsgType.Information);
        }

        private void showdata_Click(object sender, EventArgs e)
        {
            try
            {
                if (ErgodicModiForm("tbshowdata", tabControl1))
                {
                    tbshowdata = new TabPage("������ʾ");
                    tbshowdata.Name = "tbshowdata";
                    tabControl1.Controls.Add(tbshowdata);

                    Showdata form = new Showdata();
                    form.TopLevel = false;
                    //form.BackColor = Color.White;
                    form.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
                    form.StartPosition = FormStartPosition.CenterScreen;
                    form.FormBorderStyle = FormBorderStyle.None;
                    form.WindowState = FormWindowState.Maximized;

                    //����Ҽ��˵�
                    contextMenu = new ContextMenuStrip(this.components);
                    contextMenu.Items.Add("�رմ���");
                    contextMenu.Items[0].Click += new System.EventHandler(this.contextMenuItem_Click);

                    form.Show();
                    tbshowdata.Controls.Add(form);

                    form.ContextMenuStrip = contextMenu;  //ָ��������Ҽ��˵�        
                }

                tabControl1.SelectedTab = tbshowdata;
                moniterLog.Write("������ʾ", MsgType.Information);
            }
            catch
            {
                moniterLog.Write("������ʾ Error", MsgType.Error);
            }
        }

        private void searchdata_Click(object sender, EventArgs e)
        {
            try
            {
                if (ErgodicModiForm("tbsearchdata", tabControl1))
                {
                    tbsearchdata = new TabPage("���ݲ�ѯ");
                    tbsearchdata.Name = "tbsearchdata";
                    tabControl1.Controls.Add(tbsearchdata);

                    Searchdata form = new Searchdata();
                    form.TopLevel = false;
                    //form.BackColor = Color.White;
                    form.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
                    form.StartPosition = FormStartPosition.CenterScreen;
                    form.FormBorderStyle = FormBorderStyle.None;
                    form.WindowState = FormWindowState.Maximized;

                    //����Ҽ��˵�
                    contextMenu = new ContextMenuStrip(this.components);
                    contextMenu.Items.Add("�رմ���");
                    contextMenu.Items[0].Click += new System.EventHandler(this.contextMenuItem_Click);
                    form.Show();
                    tbsearchdata.Controls.Add(form);

                    form.ContextMenuStrip = contextMenu;  //ָ��������Ҽ��˵�    
                }

                tabControl1.SelectedTab = tbsearchdata;
                moniterLog.Write("���ݲ�ѯ", MsgType.Information);
            }
            catch
            {
                moniterLog.Write("���ݲ�ѯ Error", MsgType.Error);
            }
        }

        private Boolean ErgodicModiForm(string MainTabControlKey, TabControl objTabControl)
        {
            //����ѡ��ж��Ƿ���ڸ��Ӵ���  
            foreach (Control con in objTabControl.Controls)
            {
                TabPage tab = (TabPage)con;
                if (tab.Name == MainTabControlKey)
                {
                    return false;//����  
                }
            }
            return true;//������  
        }

        /// <summary>
        /// �Ҽ��˵������رմ��壬�رյ�ǰѡ�ҳ��
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

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Prediction dlg = new Prediction();
            dlg.ShowDialog();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            ExitApp();
        }

        private void ExitApp()
        {
            timer2.Stop();
            timer3.Stop();
            timer2.Enabled = false;
            timer3.Enabled = false;
            timer1.Dispose();
            timer2.Dispose();
            timer3.Dispose();
            this.waitDlg = null;
            this.axMap1 = null;
            if (moniterLog != null)
                moniterLog.Write("Applicatioin Exited", MsgType.Information);
            this.moniterLog = null;
            GC.Collect(0);
            Process.GetCurrentProcess().Kill();
        }

        private void Reportdat_Click(object sender, EventArgs e)
        {
            try
            {
                if (ErgodicModiForm("tbreportdata", tabControl1))
                {
                    tbreportdata = new TabPage("���ݱ���");
                    tbreportdata.Name = "tbreportdata";
                    tabControl1.Controls.Add(tbreportdata);

                    Reportdata form = new Reportdata();
                    form.TopLevel = false;
                    //form.BackColor = Color.White;
                    form.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
                    form.StartPosition = FormStartPosition.CenterScreen;
                    form.FormBorderStyle = FormBorderStyle.None;
                    form.WindowState = FormWindowState.Maximized;

                    //����Ҽ��˵�
                    contextMenu = new ContextMenuStrip(this.components);
                    contextMenu.Items.Add("�رմ���");
                    contextMenu.Items[0].Click += new System.EventHandler(this.contextMenuItem_Click);
                    form.Show();
                    tbreportdata.Controls.Add(form);

                    form.ContextMenuStrip = contextMenu;  //ָ��������Ҽ��˵�    
                }

                tabControl1.SelectedTab = tbreportdata;
                moniterLog.Write("���ݱ���r", MsgType.Information);
            }
            catch
            {
                moniterLog.Write("���ݱ��� Error", MsgType.Error);
            }
        }
        /// <summary>
        /// ���¶�λվ��λ��
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
            label1.Text = " ";

        }
        /// <summary>
        /// ��ʱˢ��վ������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer2_Tick(object sender, EventArgs e)
        {
            try
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
                //sqlcommand.Dispose();
                //sqlcon.Dispose();
                sqlcommand = null;
                sqlcon = null;
                GC.Collect(0);
                moniterLog.Write("time2 has been done,Update data to map station", MsgType.Information);
            }
            catch
            {
                moniterLog.Write("time2 Update data Error", MsgType.Error);
            }

        }
        /// <summary>
        /// ���ŵ�վ��������ʾ�������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void label1_MouseHover(object sender, EventArgs e)
        {
            groupBox1.Left = label1.Right + 1;
            groupBox1.Top = label1.Top + 9;
            groupBox1.Visible = true;
            moniterLog.Write("Now data has shown", MsgType.Information);
        }

        private void label1_MouseLeave(object sender, EventArgs e)
        {
            groupBox1.Visible = false;
        }
        /// <summary>
        /// ��״̬��ʵʱ��ʾ���ľ�γ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void axMap1_MouseMoveEvent(object sender, AxMapXLib.CMapXEvents_MouseMoveEvent e)
        {
            try
            {
                double MapX = 0, mapY = 0;
                axMap1.ConvertCoord(ref e.x, ref e.y, ref MapX, ref mapY,
                                   MapXLib.ConversionConstants.miScreenToMap);
                toolStripStatusLabel1.Text = MapX.ToString() + "," + mapY.ToString();
            }
            catch
            {
                moniterLog.Write("Mouse move on the map,Error", MsgType.Error);
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            DataFit dlg = new DataFit();
            dlg.ShowDialog();
        }

        private void һ��У��ToolStripMenuItem_Click(object sender, EventArgs e)
        {
           // DataFit aa = new DataFit();
           // aa.fitalldata();//У���Զ�վ����
           // aa.fufitalldata();//У����������
            MessageBox.Show("����ʹ�ã�������");
        }

        private void ��ȡ��������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();                //newһ������
            ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments); //����򿪵�Ĭ���ļ���λ��
            ofd.Filter = "txt�ļ�|*.txt|�����ļ�|*.*";
            ofd.RestoreDirectory = true;
            ofd.FilterIndex = 1;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                fileName = ofd.FileName;                //���ѡ����ļ�·��
                extendedName = Path.GetExtension(fileName);       //����ļ���չ��
                fileName1 = Path.GetFileName(fileName);           //����ļ���
                fubiaodata.strtime = fileName1.Substring(0,fileName1.Length-4);
                fubiaodata.dt = DateTime.ParseExact(fubiaodata.strtime, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);
                ///////////////////////////////////////////////////
                int linNum = 0;
                string[] txtline = File.ReadAllLines(ofd.FileName, Encoding.GetEncoding("gb2312"));//�������Ĳ�������  
                linNum = txtline.Length;
                for (int i = 0; i < linNum; i++)
                {
                    Regex rx = new Regex(@"^qx,0R0,DM=\S{1,}W$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                    Match mth = rx.Match(txtline[i]);
                    if (mth.Success)
                    {
                        //MessageBox.Show("aaaaaa");
                        int istdm = txtline[i].IndexOf("=") + 1;
                        int ienddm = txtline[i].IndexOf("D", istdm);
                        fubiaodata.strdm = txtline[i].Substring(istdm, ienddm - istdm);

                        int istsm = txtline[i].IndexOf("=", ienddm + 1) + 1;
                        int iendsm = txtline[i].IndexOf("M", istsm);
                        fubiaodata.strsm = txtline[i].Substring(istsm, iendsm - istsm);

                        int istta = txtline[i].IndexOf("=", iendsm + 1) + 1;
                        int iendta = txtline[i].IndexOf("C", istta);
                        fubiaodata.strta = txtline[i].Substring(istta, iendta - istta);

                        int istua = txtline[i].IndexOf("=", iendta + 1) + 1;
                        int iendua = txtline[i].IndexOf("P", istua);
                        fubiaodata.strua = txtline[i].Substring(istua, iendua - istua);

                        int istpa = txtline[i].IndexOf("=", iendua + 1) + 1;
                        int iendpa = txtline[i].IndexOf("H", istpa);
                        fubiaodata.strpa = txtline[i].Substring(istpa, iendpa - istpa);

                        int istrc = txtline[i].IndexOf("=", iendpa + 1) + 1;
                        int iendrc = txtline[i].IndexOf("M", istrc);
                        fubiaodata.strrc = txtline[i].Substring(istrc, iendrc - istrc);

                        int istth = txtline[i].IndexOf("=", iendrc + 1) + 1;
                        int iendth = txtline[i].IndexOf("C", istth);
                        fubiaodata.strth = txtline[i].Substring(istth, iendth - istth);

                        int istvh = txtline[i].IndexOf("=", iendth + 1) + 1;
                        int iendvh = txtline[i].IndexOf("W", istvh);
                        fubiaodata.strvh = txtline[i].Substring(istvh, iendvh - istvh);
                    }

                    Regex rx1 = new Regex(@"^adc\S{1,}", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                    Match mth1 = rx1.Match(txtline[i]);
                    if (mth1.Success)
                    {
                        //MessageBox.Show("adc");
                        string[] stradc = txtline[i].Split(',');
                        string stradc1 = stradc[1];
                        string stradc2 = stradc[2];
                        string stradc3 = stradc[3];
                        string stradc4 = stradc[4];
                        string stradc5 = stradc[5];
                        string stradc6 = stradc[6];
                        string stradc7 = stradc[7];
                        int ik1 = Int32.Parse(stradc1);
                        int ik2 = Int32.Parse(stradc2);
                        int ik3 = Int32.Parse(stradc3);
                        int ik4 = Int32.Parse(stradc4);
                        int ik5 = Int32.Parse(stradc5);
                        int ik6 = Int32.Parse(stradc6);
                        int ik7 = Int32.Parse(stradc7);
                        fubiaodata.strlx = (ik1 * 146.48).ToString();
                        fubiaodata.strllv = (((ik2 + ik3 * 10 + ik4 * 100) * 39.0625) / 100).ToString();
                        fubiaodata.stryls = (((ik5 + ik6 * 10 + ik7 * 100) * 39.0625) / 100).ToString();
                        //int icout=stradc[0]

                    }

                    Regex rx2 = new Regex(@"^ds\S{1,}", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                    Match mth2 = rx2.Match(txtline[i]);
                    if (mth2.Success)
                    {
                        //MessageBox.Show("qx");
                        string[] strqx = txtline[i].Split(',');
                        fubiaodata.strwatertmp = strqx[1];
                        fubiaodata.strconduc = strqx[2];
                        fubiaodata.strntu = strqx[3];
                        fubiaodata.strph = strqx[4];
                        fubiaodata.strodiv = strqx[5];
                        fubiaodata.strsolt = strqx[7];
                    }

                }

                SqlConnection sqlcon = new SqlConnection(Global.sqlconstr);
                SqlCommand sqlcommand = sqlcon.CreateCommand();
                sqlcon.Open();
                //for (int k = 0; k < ddat; k++)
                {
                    string cmdtxt = "insert  into T_fubiao (LX,LLV,Chol,Ave_wind,Wind_spd,Temp,Rel_humid,Pres,Rain,Heat_temp,Heat_v,Water_temp,Conduc,NTU,PH,O_disv,Solt,time) values ";
                    cmdtxt += "('" + fubiaodata.strlx + "','" + fubiaodata.strllv + "','" + fubiaodata.stryls + "',";
                    cmdtxt += "'" + fubiaodata.strdm + "','" + fubiaodata.strsm + "','" + fubiaodata.strta + "',";
                    cmdtxt += "'" + fubiaodata.strua + "','" + fubiaodata.strpa + "','" + fubiaodata.strrc + "',";
                    cmdtxt += "'" + fubiaodata.strth + "','" + fubiaodata.strvh + "','" + fubiaodata.strwatertmp + "',";
                    cmdtxt += "'" + fubiaodata.strconduc + "','" + fubiaodata.strntu + "','" + fubiaodata.strph + "',";
                    cmdtxt += "'" + fubiaodata.strodiv + "','" + fubiaodata.strsolt + "',";
                    cmdtxt += "'" + fubiaodata.dt + "')";

                    sqlcommand.CommandText = cmdtxt;
                    sqlcommand.ExecuteNonQuery();
                }
                sqlcommand.Dispose();
                sqlcon.Close();

                MessageBox.Show("�������ݳɹ�");

            }


        }

        private void ������ʾToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ErgodicModiForm("tbshowdata", tabControl1))
            {
                tbshowdata = new TabPage("������ʾ");
                tbshowdata.Name = "tbshowdata";
                tabControl1.Controls.Add(tbshowdata);

                Showdata form = new Showdata();
                form.TopLevel = false;
                //form.BackColor = Color.White;
                form.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
                form.StartPosition = FormStartPosition.CenterScreen;
                form.FormBorderStyle = FormBorderStyle.None;
                form.WindowState = FormWindowState.Maximized;

                //����Ҽ��˵�
                contextMenu = new ContextMenuStrip(this.components);
                contextMenu.Items.Add("�رմ���");
                contextMenu.Items[0].Click += new System.EventHandler(this.contextMenuItem_Click);

                form.Show();
                tbshowdata.Controls.Add(form);

                form.ContextMenuStrip = contextMenu;  //ָ��������Ҽ��˵�        
            }

            tabControl1.SelectedTab = tbshowdata;
            moniterLog.Write("������ʾ", MsgType.Information);
        }

        private void ���ݲ�ѯToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ErgodicModiForm("tbsearchdata", tabControl1))
            {
                tbsearchdata = new TabPage("���ݲ�ѯ");
                tbsearchdata.Name = "tbsearchdata";
                tabControl1.Controls.Add(tbsearchdata);

                Searchdata form = new Searchdata();
                form.TopLevel = false;
                //form.BackColor = Color.White;
                form.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
                form.StartPosition = FormStartPosition.CenterScreen;
                form.FormBorderStyle = FormBorderStyle.None;
                form.WindowState = FormWindowState.Maximized;

                //����Ҽ��˵�
                contextMenu = new ContextMenuStrip(this.components);
                contextMenu.Items.Add("�رմ���");
                contextMenu.Items[0].Click += new System.EventHandler(this.contextMenuItem_Click);
                form.Show();
                tbsearchdata.Controls.Add(form);

                form.ContextMenuStrip = contextMenu;  //ָ��������Ҽ��˵�    
            }

            tabControl1.SelectedTab = tbsearchdata;
            moniterLog.Write("���ݲ�ѯr", MsgType.Information);
        }

        private void ���ݱ���ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ErgodicModiForm("tbreportdata", tabControl1))
            {
                tbreportdata = new TabPage("���ݱ���");
                tbreportdata.Name = "tbreportdata";
                tabControl1.Controls.Add(tbreportdata);

                Reportdata form = new Reportdata();
                form.TopLevel = false;
                //form.BackColor = Color.White;
                form.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
                form.StartPosition = FormStartPosition.CenterScreen;
                form.FormBorderStyle = FormBorderStyle.None;
                form.WindowState = FormWindowState.Maximized;

                //����Ҽ��˵�
                contextMenu = new ContextMenuStrip(this.components);
                contextMenu.Items.Add("�رմ���");
                contextMenu.Items[0].Click += new System.EventHandler(this.contextMenuItem_Click);
                form.Show();
                tbreportdata.Controls.Add(form);

                form.ContextMenuStrip = contextMenu;  //ָ��������Ҽ��˵�    
            }

            tabControl1.SelectedTab = tbreportdata;
            moniterLog.Write("���ݱ���r", MsgType.Information);
        }

        private void ����У��ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataFit dlg = new DataFit();
            dlg.ShowDialog();
        }

        private void Ԥ��Ԥ��ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Prediction dlg = new Prediction();
            dlg.ShowDialog();
        }

        //public static void Compute()
        //{
        //    WaitingDlg waitDlg = new WaitingDlg();
        //    waitDlg.Show();
        //}

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            try
            {
                toolStripTextBox1.Text = "ˮ��Ԥ����...";
                getdata("T_station", DateTime.Now.ToString());
                Thread oThread = new Thread(new ThreadStart(Predic.InstanceObj.GetResult));
                oThread.IsBackground = true;
                oThread.Start();
                while (!oThread.IsAlive)
                {
                    Thread.Sleep(5);
                }
                moniterLog.Write("Prediction thread has started", MsgType.Information);
            }
            catch
            {
                toolStripTextBox1.Text = "�������";
                moniterLog.Write("Prediction error occured", MsgType.Error);
            }
            if (timer3.Enabled == true)
            {
                timer3.Stop();
            }
            timer3.Interval = 1000 * 60 * Global.autoPredicteTime;
            timer3.Start();
            moniterLog.Write("timer3 started", MsgType.Information);
        }
        /// <summary>
        /// ��ȡ���ݿ�
        /// </summary>
        /// <param name="cmpdata"></param>
        /// <param name="datasource"></param>
        /// <param name="dattime"></param>
        private void getdata(string datasource, string endtime)
        {
            try
            {
                //select top 3 *from T_lmis where time<'2013-7-6 0:00:00'  order by ID desc 
                string sqlstr = "select top 200 *  from " + datasource + " where time< '" + endtime + "'";
                SqlConnection sql = new SqlConnection(Global.sqlconstr);
                sql.Open();
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(sqlstr, sql);
                da.Fill(dt);

                tcountdat = dt.Rows.Count > 190 ? 190 : dt.Rows.Count;
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
                        if (190 <= i)
                        {
                            ;// MessageBox.Show("��ֵΪ��ֵ");
                        }
                        else
                        {
                            // ddatx[i][0]=double.Parse(dr[11].ToString());///��һ����ʱ��ı仯

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
                //da.Dispose();
                dt.Clear();
                //dt.Dispose();
                da = null;
                dt = null;
                sql = null;
                GC.Collect(0);
                moniterLog.Write("get data from sql server", MsgType.Information);
            }
            catch
            {
                moniterLog.Write("��ȡ���ݿ� Error", MsgType.Error);
            }
        }

        /// <summary>
        /// ʵʱ����Ԥ���ļ��㣬�����������ź�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer3_Tick(object sender, EventArgs e)
        {
            try
            {
                toolStripTextBox1.Text = "ˮ��Ԥ����...";
                toolStripTextBox1.BackColor = Color.Empty;
                toolStripTextBox1.Text = "ˮ��Ԥ����...";
                getdata("T_station", DateTime.Now.ToString());
                Thread oThread = new Thread(new ThreadStart(Predic.InstanceObj.GetResult));
                oThread.IsBackground = true;
                oThread.Start();
                while (!oThread.IsAlive)
                {
                    Thread.Sleep(1);
                }
                Process proc = Process.GetCurrentProcess();
                long usedMemory = proc.PrivateMemorySize64;
                moniterLog.Write("Auto Predicte done", MsgType.Information);
                moniterLog.Write("Process Memory is "+usedMemory.ToString(), MsgType.Information);
            }
            catch
            {
                toolStripTextBox1.Text = "�������";
                moniterLog.Write("Auto Predicte Error", MsgType.Error);
            }
        }

        private void �Զ�Ԥ������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Setting setDlg = new Setting();
            setDlg.ShowDialog();
        }

        public void SetResult(bool ret)
        {
            if ((this.InvokeRequired) || (this.toolStrip1.InvokeRequired))
            {
                Delegate d = new SetResultEventHandler(SetResult);
                this.Invoke(d, new object[] { ret });
            }
            else
            {
                if (ret == true)
                {
                    toolStripTextBox1.Text = "ˮ���������";
                    toolStripTextBox1.BackColor = Color.Lime;
                }
                else
                {
                    toolStripTextBox1.Text = "ˮ������ϲ�";
                    toolStripTextBox1.BackColor = Color.Red;
                }
            }
        }

        public void SetWait(bool ret)
        {
            if ((this.InvokeRequired) || (this.waitDlg.InvokeRequired))
            {
                Delegate d = new SetWaitEventHandler(SetWait);
                this.Invoke(d, new object[] { ret });
            }
            else
            {
                if (ret == true)
                {
                    this.waitDlg.Show();
                    moniterLog.Write("waitDlg showed", MsgType.Information);
                }
                else
                {
                    this.waitDlg.Hide();
                    moniterLog.Write("waitDlg hided", MsgType.Information);
                }
            }
        }

        private void Mainfrm_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
            } 
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ShowMainFrm();
            }
            catch
            {
                moniterLog.Write("NotifyIcon,Open Form,Maximized Error", MsgType.Error);
            }
        }

        private void CloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExitApp();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            ExitApp();
        }

        private void ShowMainFrm()
        {
            this.Show();
            this.Visible = true;
            string AppPath = System.Windows.Forms.Application.StartupPath;
            this.axMap1.GeoSet = AppPath + @"\Tailake.gst"; //Tailake
            this.axMap1.Title.Visible = false;
            this.axMap1.Show();
            this.axMap1.Visible = true;
            this.WindowState = FormWindowState.Maximized;
        }

        private void notifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Left)
                {
                    ShowMainFrm();
                }
            }
            catch
            {
                moniterLog.Write("NotifyIcon mouseClick Error", MsgType.Error);
            }
        }

        private void MinToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            this.notifyIcon.Visible = true;
        }
    }

    public class Predic
    {
        public SetResultEventHandler setRetEventHandler;
        public SetWaitEventHandler setWaitEventHandler;
        private Predic()
        {

        }
        private static Predic instanceObj = new Predic();
        public static Predic InstanceObj {get{ return instanceObj; } }
        public void GetResult()
        {
            Log moniterLog = null;
            try
            {
                string strPath = System.Windows.Forms.Application.StartupPath + "\\Log\\";
                if (Directory.Exists(strPath) == false)
                {
                    Directory.CreateDirectory(strPath);
                }
                moniterLog = new Log(strPath, LogType.Daily);

                setWaitEventHandler(true);
                moniterLog.Write("Auto Predite, begin in GetResult", MsgType.Information);
                SVMpredictone.SVMpredictoneclassClass tmp1 = new SVMpredictone.SVMpredictoneclassClass();
                tmp1.SVMpredictone(1, ref Global.result, Mainfrm.dsvmdat, 1);
                moniterLog.Write("Auto Predite, end in GetResult", MsgType.Information);
                if (((double)Global.result) <= 40)
                {
                    setRetEventHandler(true);
                }
                else if (((double)Global.result) > 40)
                {
                    setRetEventHandler(false);
                }
                setWaitEventHandler(false);
                tmp1 = null;
                GC.Collect(0);

                moniterLog.Write("Auto Predite, done in GetResult", MsgType.Information);
            }
            catch
            {
                if (moniterLog != null)
                {
                    moniterLog.Write("Auto Predite, GetResult function error", MsgType.Error);
                }
            }
        }
    }

}