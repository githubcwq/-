using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int num = Convert.ToInt32(textBox1.Text); //文本转换
            string result="000";
            switch(    (int)(num/10)    )    
            {            
                case 9: result="优秀";break;            
                case 8:           
                case 7:result="良好";break;           
                case 6:result="及格";break;
                case 5:
                case 4:
                case 3:
                case 2:
                case 1:
                case 0:result="不及格"; break; 
                default:result="输入错误";break;   
            }
            MessageBox.Show("结果是" + result,"提醒",MessageBoxButtons.OKCancel,MessageBoxIcon.Information);//弹出计算结果
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

       

        
    }
}
