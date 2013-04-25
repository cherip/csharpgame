using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace MyGameServer
{
    class MyFile
    {
        static string path = Environment.CurrentDirectory + "\\users.txt";
        public static bool Judge(string name,string pwd)
        {

            try
            {
                using (StreamReader sr = File.OpenText(path))
                {
                    string s = "";
                    while ((s = sr.ReadLine()) != null)
                    {
                        string[] ss = s.Split('|');
                        if (ss[0] == name && ss[1] == pwd)
                        {
                            return true;
                        }

                    }
                    return false;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("请在运行程序目录下创建users.txt!用户名和密码用‘|’字符隔开。");
                return false;
            }
          
        }
       
    }
}
