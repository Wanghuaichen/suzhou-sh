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
    public static class Global
    {
        [DllImport("kernel32")]
        public static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);//��ȡINI�����ļ�
        [DllImport("kernel32")]
        public static extern int WritePrivateProfileString(string section, string key, string val, string filePath);//дINI�����ļ�

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
                GetPrivateProfileString("MyConnectionStr", "sqlstr", "error", temp, 255, path);//��ȡ���ݿ������ַ���
                sqlconstr = temp.ToString();

                SqlConnection sqlcon = new SqlConnection(sqlconstr);
                sqlcon.Open();
                sqlcon.Close();
                conet = true;
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("�������ݿ�ʧ��");
                conet = false;
            }
        }

     

    }

}
