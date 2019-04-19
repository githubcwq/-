using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms.DataVisualization.Charting;

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
        //定义两个模块的一个中间量
        double z;
        private Queue<double> dataQueue = new Queue<double>(100);
        private int curValue = 0;
        //每次删除增加几个点
        private int num = 5;

        public void RealChart()
        {
            InitializeComponent();
        }
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

       private void button_start_Click(object sender, EventArgs e)
        {
            IPAddress ip = IPAddress.Parse(this.textBox1.Text.Trim());   
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);     
            try {              
                //绑定ip和端口           
                serverSocket.Bind(new IPEndPoint(ip, Convert.ToInt32(this.textBox2.Text.Trim())));       
                //设置最多10个排队连接请求    
                serverSocket.Listen(10);             
                //开启线程循环监听              
                listenThread = new Thread(ListenClientConnect);       
                listenThread.Start();            
                this.button1.Enabled = false;           
            }
            catch {            
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
                    z = double.Parse(Encoding.Default.GetString(buff, 0, r));
                    this.Invoke(new Action(() => { this.richTextBox1.Text = str; }));    
                }          
            } 
            catch 
            {   
                MessageBox.Show("接收数据失败", "接收数据失败");   
            }    
        }

        private void button_close_Click(object sender, EventArgs e)
        {
            //socket关闭
            serverSocket.Close();
            //线程关闭
            listenThread.Abort();
            threadReceive.Abort();
        }

        private void button_send_Click(object sender, EventArgs e)
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
        /// 初始化事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void btnInit_Click(object sender, EventArgs e)
        {
            InitChart();
        }

        /// <summary>
        /// 开始事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void btnStart_Click(object sender, EventArgs e)
        {
            this.timer1.Start();
        }

        /// <summary>
        /// 停止事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void btnStop_Click(object sender, EventArgs e)
        {
            this.timer1.Stop();
        }

        /// <summary>
        /// 定时器事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void timer1_Tick(object sender, EventArgs e)
        {
            UpdateQueueValue();
            this.chart1.Series[0].Points.Clear();
            for (int i = 0; i < dataQueue.Count; i++)
            {
                this.chart1.Series[0].Points.AddXY((i + 1), dataQueue.ElementAt(i));
            }     
        }

        /// <summary>
        /// 初始化图表
        /// </summary>

        private void InitChart()
        {
            //定义图表区域   
            this.chart1.ChartAreas.Clear();
            ChartArea chartArea1 = new ChartArea("C1");
            this.chart1.ChartAreas.Add(chartArea1);
            //定义存储和显示点的容器           
            this.chart1.Series.Clear();
            Series series1 = new Series("S1");
            series1.ChartArea = "C1";
            this.chart1.Series.Add(series1);
            //设置图表显示样式           
            this.chart1.ChartAreas[0].AxisY.Minimum = 0;
            this.chart1.ChartAreas[0].AxisY.Maximum = 100;
            this.chart1.ChartAreas[0].AxisX.Interval = 5;
            this.chart1.ChartAreas[0].AxisX.MajorGrid.LineColor = System.Drawing.Color.Silver;
            this.chart1.ChartAreas[0].AxisY.MajorGrid.LineColor = System.Drawing.Color.Silver;
            //设置标题           
            this.chart1.Titles.Clear();
            this.chart1.Titles.Add("S01");
            this.chart1.Titles[0].Text = "XXX显示";
            this.chart1.Titles[0].ForeColor = Color.RoyalBlue;
            this.chart1.Titles[0].Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            //设置图表显示样式          
            this.chart1.Series[0].Color = Color.Red;
            this.chart1.Titles[0].Text = string.Format("折线显示");
            this.chart1.Series[0].ChartType = SeriesChartType.Line;
            this.chart1.Series[0].Points.Clear();
        }

        //更新队列中的值    
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
            double r = z;
            for (int i = 0; i < num; i++)
            {
                dataQueue.Enqueue(r);
            }
        }
    }
}
