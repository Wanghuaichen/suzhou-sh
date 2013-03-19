using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security.Permissions;
using System.Globalization;
using System.Data.SqlClient;
using System.Drawing;

namespace Twater
{
    public static class fubiaodata
    {
        public static string strlx;
        public static string strllv;
        public static string stryls;

        public static string strdm;
        public static string strsm;
        public static string strta;
        public static string strua;
        public static string strpa;
        public static string strrc;
        public static string strth;
        public static string strvh;

        public static string strwatertmp;
        public static string strconduc;
        public static string strntu;
        public static string strph;
        public static string strodiv;
        public static string strsolt;

        public static string strtime;
        public static DateTime dt;
    }

    public static class Global
    {
        [DllImport("kernel32")]
        public static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);//读取INI配置文件
        [DllImport("kernel32")]
        public static extern int WritePrivateProfileString(string section, string key, string val, string filePath);//写INI配置文件

        public static string AppPath;
        public static string sqlconstr;
        public static bool conet;
        public static void connectsql()
        {
            AppPath = System.Windows.Forms.Application.StartupPath;
            StringBuilder temp = new StringBuilder(255);
            string path = AppPath + @"\Myconfig.ini";
            try
            {
                GetPrivateProfileString("MyConnectionStr", "sqlstr", "error", temp, 255, path);//读取数据库连接字符串
                sqlconstr = temp.ToString();

                SqlConnection sqlcon = new SqlConnection(sqlconstr);
                sqlcon.Open();
                sqlcon.Close();
                conet = true;
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("连接数据库失败");
                conet = false;
            }
        }

     

    }

}
