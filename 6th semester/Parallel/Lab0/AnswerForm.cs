using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class AnswerForm : Form
    {
        public AnswerForm(string answer)
        {
            InitializeComponent();
            this.label1.Text = answer;
        }
    }
}
