﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace CSharpGame
{
    class  MyFormat
    {
        //随机生成图片数组
        public static void genPic(ref int[] a)
        {
            Random r = new Random();
            for(int i = 0; i < 64; i++)                             
            {    
                a[i] = r.Next(0,16);
            }           
        }
        //计算数组中有多少对可以消去的
        public static int countPairPic(int[] a)
        {
            int count = 0;
            for (int i = 0; i < a.Length; i++)
            {
               if (a[i] != -1)
               {
                   for (int j = i + 1; j < a.Length; j++)
                   {
                       if (a[j] != -1)
                       {
                           if (a[i] == a[j])
                           {
                               count ++;                           
                               a[j] = -1;
                               j = a.Length;
                           }
                       }
                   }
               }
            }
            return count;
        }
        //数组转换成字符串
        public static string arrayToStr(int[] array)
        {
            string str = null;
            foreach (int i in array)
            {
                str = str + i + ',';
            }
            str = str.Substring(0,str.Length -1);
            return str;
        }
        //字符串转换成数组
        public static int[] strToArray(string str)
        {
            string[] str_array = str.Split(',');
            int[] array = new int[64];
            for (int i = 0; i < str_array.Length; i++ )
            {
                array[i] = int.Parse(str_array[i]);
            }
            return array;
        }
    }
}
