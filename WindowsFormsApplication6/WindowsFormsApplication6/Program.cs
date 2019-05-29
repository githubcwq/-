using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ServerSocket
{

    public partial class Form1 : Form
    {

        //定义Socket对象

        Socket serverSocket;

        //定义监听线程

        Thread listenThread;

        //定义接收客户端数据线程

        Thread threadReceive;

        //定义双方通信

        Socket socket;

        string str;



        public Form1()
        {

            InitializeComponent();

        }



        private void button1_Click(object sender, EventArgs e)
        {

            IPAddress ip = IPAddress.Parse(this.text_ip.Text.Trim());

            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {

                //绑定ip和端口

                serverSocket.Bind(new IPEndPoint(ip, Convert.ToInt32(this.text_port.Text.Trim())));

                //设置最多10个排队连接请求

                serverSocket.Listen(10);

                //开启线程循环监听

                listenThread = new Thread(ListenClientConnect);

                listenThread.Start();

                this.button_start.Enabled = false;



            }
            catch
            {

                MessageBox.Show("监听异常", "监听异常");

            }



        }



        //监听

        private void ListenClientConnect()
        {

            while (true)
            {

                //监听到客户端的连接，获取双方通信socket

                socket = serverSocket.Accept();

                //创建线程循环接收客户端发送的数据

                threadReceive = new Thread(Receive);

                //传入双方通信socket

                threadReceive.Start(socket);

            }

        }



        //接收客户端数据

        private void Receive(object socket)
        {

            try
            {

                Socket myClientSocket = (Socket)socket;

                while (true)
                {

                    byte[] buff = new byte[20000];

                    int r = myClientSocket.Receive(buff);

                    str = Encoding.Default.GetString(buff, 0, r);

                    this.Invoke(new Action(() => { this.text_log1.Text = str; }));

                }

            }
            catch
            {

                MessageBox.Show("接收数据失败", "接收数据失败");

            }

        }



        //关闭

        private void button_close_Click(object sender, EventArgs e)
        {

            //socket关闭

            serverSocket.Close();

            //线程关闭

            listenThread.Abort();

            threadReceive.Abort();

        }



        //发送

        private void button_send_Click(object sender, EventArgs e)
        {

            try
            {

                string strMsg = this.text_log2.Text.Trim();

                byte[] buffer = new byte[20000];

                buffer = Encoding.Default.GetBytes(strMsg);

                socket.Send(buffer);

            }

            catch
            {

                MessageBox.Show("发送数据失败", "发送数据失败");

            }

        }

    }

}
