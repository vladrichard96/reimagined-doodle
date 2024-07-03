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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
            treeView1.BeginUpdate();
            for (int i = 0; i < Form2.answers.Count; i++)
            {
                treeView1.Nodes.Add("Пакет "+i);
                treeView1.Nodes[i].Nodes.Add("errorCode: " + Form2.answers[i].errorCode);
                treeView1.Nodes[i].Nodes.Add("status: " + Form2.answers[i].status);
                treeView1.Nodes[i].Nodes.Add("text: " + Form2.answers[i].text);
            }
        }
    }
}
