using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;

namespace CSharpGame
{

    static class Program
    {
        [DllImport("kernel32.dll")]
        public static extern bool AllocConsole();

        public static string serverip;
       
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            //AllocConsole();
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            string apppath = Environment.CurrentDirectory;
            //using (StreamReader sr = File.OpenText(apppath + "\\ip.txt"))
            //{
            //    string s = "";
            //    while ((s = sr.ReadLine()) != null)
            //    {
            //        serverip = s;
            //    }            
            //}

            serverip = "127.0.0.1";

            MainLogic gameLogic = new MainLogic();
            
            // 启动界面
            Application.Run(gameLogic.hall);
        }
    }
}
