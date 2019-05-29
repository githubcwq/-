using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;  
namespace client
{
    class Program
    {
        private static byte[] result = new byte[1024];

        static void Main(string[] args)
        {  
            //设定服务器IP地址  
            IPAddress ip = IPAddress.Parse("192.168.8.100");  
            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);  
            try  
            {
                clientSocket.Connect(new IPEndPoint(ip, 8888)); //配置服务器IP与端口  
                Console.WriteLine("连接服务器成功");  
            }  
            catch  
            {  
                Console.WriteLine("连接服务器失败，请按回车键退出！");  
                return;  
            }  

            //通过 clientSocket 发送数据  
            for (int i = 0; i < 10; i++)  
            {  
                try  
                {  
                    Thread.Sleep(1000);    //等待1秒钟  
                    string sendMessage = "client send Message Hellp" + DateTime.Now;  
                    clientSocket.Send(Encoding.ASCII.GetBytes(sendMessage));  
                    Console.WriteLine("向服务器发送消息：{0}" + sendMessage);

                    //通过clientSocket接收数据  
                    int receiveLength = clientSocket.Receive(result);
                    Console.WriteLine("接收服务器消息：{0}", Encoding.ASCII.GetString(result, 0, receiveLength)); 
                }  
                catch  
                {  
                    clientSocket.Shutdown(SocketShutdown.Both);  
                    clientSocket.Close();  
                    break;  
                }  
            }  
            Console.WriteLine("发送完毕，按回车键退出");  
            Console.ReadLine();  
        }  
    }
}