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
using System.Threading;
//using System.Windows.Forms.DataVisualization.Charting;

namespace 服务端版本三5._1
{
    public partial class Form1 : Form
    {
        ///////////定义随机数区///////////
        Random I = new Random();    //电流
        Random V = new Random();    //电压
        Random R = new Random();    //电阻
        Random L = new Random();    //电感
        Random H = new Random();    //湿度
        Random T = new Random();    //温度
        Random E = new Random();    //海拔
        Random P = new Random();    //气压
        private Queue<double> dataQueue = new Queue<double>(100);
        private int curValue = 0;
        private int num = 5;//每次删除增加几个点
        //////////////////////////////////

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
            IPAddress ip = IPAddress.Parse(this.textBox21.Text.Trim());
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                //绑定ip和端口           
                serverSocket.Bind(new IPEndPoint(ip, Convert.ToInt32(this.textBox22.Text.Trim())));
                //设置最多10个排队连接请求    
                serverSocket.Listen(10);
                //开启线程循环监听              
                listenThread = new Thread(ListenClientConnect);
                listenThread.Start();
                this.button1.Enabled = false;
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
                    this.Invoke(new Action(() => { this.richTextBox1.Text = str; }));
                }
            }
            catch
            {
                MessageBox.Show("接收数据失败", "接收数据失败");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //socket关闭
            serverSocket.Close();
            //线程关闭
            listenThread.Abort();
            threadReceive.Abort();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                string strMsg = this.richTextBox2.Text.Trim();
                byte[] buffer = new byte[20000];
                buffer = Encoding.Default.GetBytes(strMsg);
                socket.Send(buffer);
            }
            catch
            {
                MessageBox.Show("发送数据失败", "发送数据失败");
            }
        }

        /// <summary>
        /// 定时器事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            //UpdateQueueValue();
           /* this.textBox5.Text = String.Empty;
            this.textBox6.Text = String.Empty;
            this.textBox7.Text = String.Empty;
            this.textBox8.Text = String.Empty;
            this.textBox10.Text = String.Empty;
            this.textBox12.Text = String.Empty;
            this.textBox14.Text = String.Empty;
            this.textBox16.Text = String.Empty;
            Random I = new Random();    //电流
            Random V = new Random();    //电压
            Random R = new Random();    //电阻
            Random L = new Random();    //电感
            Random H = new Random();    //湿度
            Random T = new Random();    //温度
            Random E = new Random();    //海拔
            Random P = new Random();    //气压
            this.textBox5.Text = I.ToString();
            this.textBox6.Text = R.ToString();
            this.textBox7.Text = V.ToString();
            this.textBox8.Text = L.ToString();
            this.textBox10.Text = T.ToString();
            this.textBox12.Text = H.ToString();
            this.textBox14.Text = P.ToString();
            this.textBox16.Text = E.ToString();
            for (int i = 0; i < dataQueue.Count; i++)
              {
                  this.textBox5.Text = (dataQueue.ElementAt(i)).ToString();
              }*/
            this.richTextBox1.Text = String.Empty;
            Random I = new Random();    //电流
           // this.richTextBox1.Text = I.ToString();
        }

        private void UpdateQueueValue()
        {
            if (dataQueue.Count > 100)
            {
                //先出列
                for (int i = 0; i < num; i++)
                {
                    dataQueue.Dequeue();
                }
            }
            Random I = new Random();    //电流
            Random V = new Random();    //电压
            Random R = new Random();    //电阻
            Random L = new Random();    //电感
            Random H = new Random();    //湿度
            Random T = new Random();    //温度
            Random E = new Random();    //海拔
            Random P = new Random();    //气压
            for (int i = 0; i < num; i++)
            {
                dataQueue.Enqueue(I.Next(0, 100));
                dataQueue.Enqueue(V.Next(0, 100));
                dataQueue.Enqueue(R.Next(0, 100));
                dataQueue.Enqueue(L.Next(0, 100));
                dataQueue.Enqueue(H.Next(0, 100));
                dataQueue.Enqueue(T.Next(0, 100));
                dataQueue.Enqueue(E.Next(0, 100));
                dataQueue.Enqueue(P.Next(0, 100));
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.timer1.Start();
        }



    }
}
