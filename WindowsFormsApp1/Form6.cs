using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

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
            SqlDataAdapter adapter = new SqlDataAdapter();
            AddTextParameters(command, "@log", textBox1);
            AddTextParameters(command, "@pas", textBox2);
            AddTextParameters(command, "@nam", textBox3);
            AddTextParameters(command, "@sur", textBox4);
            AddTextParameters(command, "@pos", textBox5);
            AddBoolParameters(command, "@enc", checkBox1);
            AddBoolParameters(command, "@dec", checkBox2);
            AddBoolParameters(command, "@acc", checkBox3);
            AddBoolParameters(command, "@res", checkBox4);
            AddBoolParameters(command, "@trf", checkBox5);
            AddBoolParameters(command, "@cre", checkBox6);
            AddBoolParameters(command, "@del", checkBox7);
            AddBoolParameters(command, "@mod", checkBox8);
            if (Form5.mode == 1) { AddIDParameter(command, "@uid", Form5.uid); }
            adapter.SelectCommand = command;
            command.ExecuteNonQuery();
            sqlc.Close();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        public static void AddTextParameters (SqlCommand command, string pam, System.Windows.Forms.TextBox textbox)
        {
            command.Parameters.Add(pam, SqlDbType.VarChar, 20);
            command.Parameters[pam].Value = textbox.Text;
        }
        public static void AddBoolParameters (SqlCommand command, string pam, CheckBox checkBox)
        {
            command.Parameters.Add(pam, SqlDbType.TinyInt, 1);
            command.Parameters[pam].Value = GetValueCheckbox(checkBox);
        }
        public static void AddIDParameter(SqlCommand command, string pam, int id)
        {
            command.Parameters.Add(pam, SqlDbType.Int, 5); //исправить под базу данных
            command.Parameters[pam].Value = id;
        }
        public static int GetValueCheckbox (CheckBox checkBox)
        {
            int value;
            if (checkBox.Checked == true) value = 1; else value = 0;
            return value;
        }
    }
}

