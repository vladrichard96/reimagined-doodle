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
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Runtime.Remoting.Messaging;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public static string username;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string conn = "Data Source=DESKTOP-MGN38Q8\\SQLEXPRESS;Initial Catalog=GruzhinskasDB;Integrated Security=True";
            string sql = "SELECT * FROM Users WHERE userLogin = @ul AND userPassword = @up";
            SqlConnection sqlc = new SqlConnection(conn);
            sqlc.Open();
            DataTable table = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter();
            SqlCommand command = new SqlCommand(sql, sqlc);
            command.Parameters.Add("@ul", SqlDbType.VarChar, 20);
            command.Parameters.Add("@up", SqlDbType.VarChar, 20);
            command.Parameters["@ul"].Value = textBox1.Text;
            command.Parameters["@up"].Value = textBox2.Text;
            if ((textBox1.Text == string.Empty) || (textBox2.Text == string.Empty))
            {
                MessageBox.Show("Введите логин/пароль!");
            }
            else
            {
                adapter.SelectCommand = command;
                adapter.Fill(table);
                if (table.Rows.Count > 0)
                {
                    AssignUserRole(textBox1.Text);
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
            sqlc.Close();
        }
        public static void AssignUserRole(string value)
        {
            string UserLogin = value;
            string conn = "Data Source=DESKTOP-MGN38Q8\\SQLEXPRESS;Initial Catalog=GruzhinskasDB;Integrated Security=True";
            string sql = "SELECT userFirstName, userLastName, userPosition, userEncrypting, userDecrypting, userGetResponses, userReserveCopying, userTrafAnalytic, userManageCreate, userManageDelete, userManageModify FROM Users WHERE userLogin = @ul";
            SqlConnection sqlc = new SqlConnection(conn);
            sqlc.Open();
            SqlParameter param = new SqlParameter("@ul", UserLogin);
            SqlCommand command = new SqlCommand(sql, sqlc);
            command.Parameters.Add(param);
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    username = reader.GetString(0) + " " + reader.GetString(1) + ", " + reader.GetString(2);
                    Form2.canEncrypt = Convert.ToBoolean(reader.GetByte(3));
                    Form2.canDecrypt = Convert.ToBoolean(reader.GetByte(4));
                    Form2.canGetResponses = Convert.ToBoolean(reader.GetByte(5));
                    Form2.canReserveCopying = Convert.ToBoolean(reader.GetByte(6));
                    Form2.canTrafAnalytic = Convert.ToBoolean(reader.GetByte(7));
                    Form2.canManageCreate = Convert.ToBoolean(reader.GetByte(8));
                    Form2.canManageDelete = Convert.ToBoolean(reader.GetByte(9));
                    Form2.canManageModify = Convert.ToBoolean(reader.GetByte(10));
                }
            }
        }
        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Close();
        }
    }
}
