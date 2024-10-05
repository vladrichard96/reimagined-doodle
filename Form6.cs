using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form6 : Form
    {
        public string login, password, name, surname, position;
        public static string conn, sql;
        public Form6()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            conn = "Data Source=DESKTOP-MGN38Q8\\SQLEXPRESS;Initial Catalog=GruzhinskasDB;Integrated Security=True";
            switch (Form5.mode)
            {
                case 0:
                    {
                        sql = "INSERT INTO Users VALUES (@log, @pas, @nam, @sur, @pos, @enc, @dec, @acc, @res, @trf, @cre, @mod, @del)";
                        break;
                    }
                case 1:
                    {
                        sql = "UPDATE Users SET userLogin = @log, userPassword = @pas, userFirstName = @nam, userLastName = @sur, userPosition = @pos, userEncrypting = @enc, userDecrypting = @dec, userGetResponses = @acc, userReserveCopying = @res, userTrafAnalytic = @trf, userManageCreate = @cre, userManageDelete = @del, userManageModify = @mod WHERE userID = @uid";
                        break;
                    }
            }
            SqlConnection sqlc = new SqlConnection(conn);
            sqlc.Open();
            SqlCommand command = new SqlCommand(sql, sqlc);
            command.Parameters.Add("@log", SqlDbType.VarChar, 20);
            command.Parameters["@log"].Value = textBox1.Text;
            command.Parameters.Add("@pas", SqlDbType.VarChar, 20);
            command.Parameters["@pas"].Value = textBox2.Text;
            command.Parameters.Add("@nam", SqlDbType.VarChar, 30);
            command.Parameters["@nam"].Value = textBox3.Text;
            command.Parameters.Add("@sur", SqlDbType.VarChar, 30);
            command.Parameters["@sur"].Value = textBox4.Text;
            command.Parameters.Add("@pos", SqlDbType.VarChar, 30);
            command.Parameters["@pos"].Value = textBox5.Text;

            sqlc.Close();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
