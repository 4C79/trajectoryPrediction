using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ROI
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        public void clearDlgtext()
        {
            richTextBox1.Text = "";
        }

        public void setDlgtext(String str)
        {
            richTextBox1.Text += str;
        }
    }
}
