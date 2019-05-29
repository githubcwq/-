using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ClientSocket
{
    public partial class Form1 : Form
    {
        //定义Socket对象
        Socket clientSocket;
        //创建接收消息的线程
        Thread threadReceive;
        //接收服务端发送的数据
        string str;

        public Form1()
        {
            InitializeComponent();
        }

        private void button_connect_Click(object sender, EventArgs e)
        {
            IPAddress ip = IPAddress.Parse(this.textBox1.Text.Trim());    
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);     
            try {             
                //连接服务端        
                clientSocket.Connect(ip, Convert.ToInt32(this.textBox2.Text.Trim()));    
                //开启线程不停的接收服务端发送的数据      
                threadReceive = new Thread(new ThreadStart(Receive));      
                threadReceive.IsBackground = true;               
                threadReceive.Start();              
                //设置连接按钮在连接服务端后状态为不可点       
                this.button1.Enabled = false;          
            } 
            catch {    
                MessageBox.Show("连接服务端失败，请确认ip和端口是否填写正确", "连接服务端失败");     
            }
        }

        private void Receive()      
        {            
            try {          
                while (true){                
                    byte[] buff = new byte[20000];         
                    int r = clientSocket.Receive(buff);   
                    str = Encoding.Default.GetString(buff,0,r);   
                    this.Invoke(new Action(() => { this.richTextBox1.Text = str; })); 
                }         
            } 
            catch{        
                MessageBox.Show("获取服务端参数失败","获取服务端参数失败");     
            }      
        }

        private void button_close_Click(object sender, EventArgs e)
        {
            //clientSocket关闭
            clientSocket.Close();
            //threadReceive关闭
            threadReceive.Abort();
        }

        private void button_send_Click(object sender, EventArgs e)
        {
            try {      
                string strMsg = this.richTextBox2.Text.Trim();     
                byte[] buffer = new byte[20000];             
                buffer = Encoding.Default.GetBytes(strMsg);        
                clientSocket.Send(buffer);           
            }
            catch{     
                MessageBox.Show("发送数据失败", "发送数据失败");    
            }
        }
    }
}
