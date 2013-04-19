using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace CSharpGame
{
    class MyClientSoc
    {
        private TcpClient client;//基于TCP协议的客户端编程
        private int serverport = 8668;//服务器端端口号
        private NetworkStream ns;//用于获取和操作网络流
        private Thread receive = null;//用于启动用户聊天的线程
        private string clientname;//要加入聊天的客户姓名
        public bool connected = false; //判断用户是否已经与服务器取得连接        
        private bool privatemode = false; //判断用户之间的聊天是否属于私聊
        public delegate void GetWinCall(string id, string app1, string app2);//建立启动Windows控件的委托

        public void InitialSoc()
        {
            
            while (!connected)
            {
                try
                {
                    client = new TcpClient();
                    client.Connect("127.0.0.1", serverport);
                    ns = client.GetStream(); //用来获得答应的数据流
                    connected = true;

                }
                catch (Exception) 
                {
                    MessageBox.Show("不能连接到服务器！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    
                    return;
                }
            }
           
        }
        public void SendStr(string type, string strSend)
        {
            try
            {
                strSend = type + '|' + strSend + '|';
                Console.WriteLine(strSend);
                Byte[] outbytes = System.Text.Encoding.BigEndianUnicode.GetBytes(strSend.ToCharArray());
                ns.Write(outbytes, 0, outbytes.Length);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        public string[] RecieveStr()
        {
            try
            {
                byte[] byteMessage = new byte[1024];
                //ns = client.GetStream();
                ns.Read(byteMessage, 0, byteMessage.Length);
                string receiveStr = System.Text.Encoding.BigEndianUnicode.GetString(byteMessage);
                receiveStr.Trim();
                string[] contents = receiveStr.Split(new Char[] { '|' });
                return contents;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
           

        }

        public void CloseConn()
        {
           try
           {
               client.Close();
               connected = false;
           }
           catch (Exception ex)
           {
               MessageBox.Show("本来就没连接！");
           }
            
        }
    }
    
}
