using System;
using System.Threading;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class Form3 : Form
    {
        public string P { get; set; }
        public string MR { get; set; }
        public string MS { get; set; }
        public Form3()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.P = this.textBox1.Text;
            this.MR = this.textBox2.Text;
            this.MS = this.textBox3.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            this.Name = Thread.CurrentThread.Name;
            this.label1.Text = "SIZE = " + Data.size;
        }
    }
}
