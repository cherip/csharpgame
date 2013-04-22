using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MyGameServer
{
    class MyFile
    {
        static string  path = @"c:\users.txt";
        public static bool Judge(string name,string pwd)
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
       
    }
}
