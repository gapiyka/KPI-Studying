using System;
using System.Threading;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        public string A { get; set; }
        public string B { get; set; }
        public string MA { get; set; }
        public string ME { get; set; }

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.A = this.textBox1.Text;
            this.B = this.textBox2.Text;
            this.MA = this.textBox3.Text;
            this.ME = this.textBox4.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Name = Thread.CurrentThread.Name;
            this.label1.Text = "SIZE = " + Data.size;
        }
    }
}
