using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 显示图片4._30
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBox1.Image = imageList1.Images[1];
        }
        private void button1_Click(object sender, EventArgs e)
        {
                int f = 0;
                int flag = int.Parse(richTextBox1.Text);
                if (flag >= 50)
                {
                    f = 0;
                }
                else
                {
                    f = 1;
                }
                switch (f)
                {
                    case 0: pictureBox1.Image = imageList1.Images[0];
                        break;
                    case 1: pictureBox1.Image = imageList1.Images[1];
                        break;
                }
        }
    }
}
