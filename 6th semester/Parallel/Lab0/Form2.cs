using System;
using System.Threading;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class Form2 : Form
    {
        public string MH { get; set; }
        public string MK { get; set; }
        public string ML { get; set; }

        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.MH = this.textBox1.Text;
            this.MK = this.textBox2.Text;
            this.ML = this.textBox3.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            this.Name = Thread.CurrentThread.Name;
            this.label1.Text = "SIZE = " + Data.size;
        }
    }
}
