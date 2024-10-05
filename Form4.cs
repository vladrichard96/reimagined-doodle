using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2.key1 = Encoding.UTF8.GetBytes(textBox1.Text);
            Form2.key2 = Encoding.UTF8.GetBytes(textBox1.Text);
            Form2.kuznechik.RoundKeys(Form2.key1, Form2.key2);
            this.Close();
        }
    }
}
