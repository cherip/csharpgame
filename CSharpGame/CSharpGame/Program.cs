using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace CSharpGame
{
    static class Program
    {
        //[DllImport("kernel32.dll")]
        //public static extern bool AllocConsole();
        

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            //AllocConsole();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            MainLogic gameLogic = new MainLogic();

            // 启动界面
            Application.Run(gameLogic.hall);
        }
    }
}
