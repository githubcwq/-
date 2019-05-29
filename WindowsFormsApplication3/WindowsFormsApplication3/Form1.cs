using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;


namespace Socket_TcpListener
{
    class Program
    {
        static void Main(string[] args)
        {
            string content = "今天又是元气满满的一天";     //需要发送的消息内容
            byte[] data = Encoding.GetEncoding("GBK").GetBytes(content);    //转换为byte数组

            ClientSocketStart(data);        //开启socket通信
            Console.ReadKey();
        }


        /// <summary>
        /// Tcp客户端
        /// </summary>
        /// <param name="data"></param>
        public static void ClientSocketStart(byte[] data)
        {
            TcpClient tcpClient; 
            int localPort = 11821;
            IPAddress localIp = IPAddress.Parse("192.168.11.83");
            IPEndPoint localEndPoint = new IPEndPoint(localIp, localPort);      //需要连接的本地客户端ip和端口号

            int remotePort = 11810;
            IPAddress remoteIp = IPAddress.Parse("192.168.11.83");
            IPEndPoint remoteEndPoint = new IPEndPoint(remoteIp, remotePort);   //需要连接的远端服务端ip和端口号

            tcpClient = new TcpClient(localEndPoint);
            tcpClient.Connect(remoteEndPoint);
            NetworkStream stream = tcpClient.GetStream();

            try
            {
                stream.Write(data, 0, data.Length);
                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 向服务端发送消息成功 ");
            }
            catch
            {
                Console.WriteLine("连接中断");
            }
        }
    }
}
