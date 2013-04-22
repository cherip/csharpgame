using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Windows.Forms;

namespace CSharpGame
{
    class MyClientSoc
    {
        
        private TcpClient client;//基于TCP协议的客户端编程
        private int serverport = 8668;//服务器端端口号
        private NetworkStream ns;//用于获取和操作网络流
        //private Thread receive = null;//用于启动用户聊天的线程
        //private string clientname;//要加入聊天的客户姓名
        public bool connected = false; //判断用户是否已经与服务器取得连接        
        //private bool privatemode = false; //判断用户之间的聊天是否属于私聊
        public delegate void GetWinCall(string id, string app1, string app2);//建立启动Windows控件的委托

        public void InitialSoc()
        {
            
            while (!connected)
            {
                try
                {
                    client = new TcpClient();
                    client.Connect(Program.serverip, serverport);
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

        public Message RecieveMsg()
        {
            try
            {
                byte[] byteMessage = new byte[1024];
                ns.Read(byteMessage, 0, byteMessage.Length);
                Message msg = (Message)(SerializationUnit.DeserializeObject(byteMessage));
                return msg;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        public bool SendMsg(MsgSys msg)
        {
            Message fullMsg = new Message(msg);
            return SendMsg(fullMsg);
        }

        public bool SendMsg(MsgGame msg)
        {
            Message fullMsg = new Message(msg);
            return SendMsg(fullMsg);
        }

        public bool SendMsg(MsgChat msg)
        {
            Message fullMsg = new Message(msg);
            return SendMsg(fullMsg);
        }

        public bool SendMsg(Message msg) {
            try
            {
                Byte[] outbytes = SerializationUnit.SerializeObject(msg);
                ns.Write(outbytes, 0, outbytes.Length);
                

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
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
               //MessageBox.Show("本来就没连接！");
           }
            
        }
    }

    public class SerializationUnit
    {
        /// <summary>
        /// 把对象序列化为字节数组
        /// </summary>
        public static byte[] SerializeObject(object obj)
        {
            try
            {
                if (obj == null)
                    return null;
                MemoryStream ms = new MemoryStream();
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;
                byte[] bytes = new byte[ms.Length];
                ms.Read(bytes, 0, bytes.Length);
                ms.Close();
                return bytes;
            }
            catch (System.Runtime.Serialization.SerializationException e)
            {
                return null;
            }
        }

        /// <summary>
        /// 把字节数组反序列化成对象
        /// </summary>
        public static object DeserializeObject(byte[] bytes)
        {
            object obj = null;
            if (bytes == null)
                return obj;
            MemoryStream ms = new MemoryStream(bytes);
            ms.Position = 0;
            BinaryFormatter formatter = new BinaryFormatter();
            obj = formatter.Deserialize(ms);
            ms.Close();
            return obj;
        }
    }

}
