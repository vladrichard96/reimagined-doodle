using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        List<User> users = new List<User>()
        {
            new User { login = "user", password = "user", access = 3 },
            new User { login = "moder", password = "moder", access = 2 },
            new User { login = "admin", password = "admin", access = 1 }
        };
        public static int useraccess;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if ((textBox1.Text == string.Empty) || (textBox2.Text == string.Empty))
            {
                MessageBox.Show("Введите логин/пароль!");
            }
            else
            {
                for (int i = 0; i < users.Count; i++)
                {
                    if (textBox1.Text == users[i].login) useraccess = users[i].access;
                }
                if ((textBox1.Text == users[3 - useraccess].login) && (textBox2.Text == users[3 - useraccess].password) && (useraccess != 0))
                {
                    Form2 f2 = new Form2();
                    f2.FormClosed += new FormClosedEventHandler(Form2_FormClosed);
                    f2.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Неверный логин/пароль!");
                    textBox1.Clear();
                    textBox2.Clear();
                    textBox1.Focus();
                }
            }
        }
        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Close();
        }
    }
}
