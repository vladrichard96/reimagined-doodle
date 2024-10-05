using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using System.Runtime.Remoting.Contexts;

namespace WindowsFormsApp1
{
    public partial class Form5 : Form
    {
        public static int mode;
        public static int uid;
        public Form5()
        {
            InitializeComponent();
            if (Form2.canManageCreate == false) button1.Enabled = false;
            if (Form2.canManageModify == false) button2.Enabled = false;
            if (Form2.canManageDelete == false) button3.Enabled = false;
            string conn = "Data Source=DESKTOP-MGN38Q8\\SQLEXPRESS;Initial Catalog=GruzhinskasDB;Integrated Security=True";
            string sql = "SELECT * FROM Users";
            SqlConnection sqlc = new SqlConnection(conn);
            DataTable result = new DataTable();
            try
            {
                sqlc.Open();
                SqlCommand command = new SqlCommand(sql, sqlc);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(result);
            }
            catch (Exception ex) { MessageBox.Show("Ошибка!"); }
            finally
            {
                if (sqlc.State != ConnectionState.Closed)
                    sqlc.Close();
            }
            for (int i=6; i <= 13; i++)
            {
                result.Columns[i].DataType = typeof(bool);
            }
            dataGridView1.DataSource = result;
            dataGridView1.Columns[0].HeaderText = "ID";
            dataGridView1.Columns[1].HeaderText = "Логин";
            dataGridView1.Columns[2].HeaderText = "Пароль";
            dataGridView1.Columns[3].HeaderText = "Имя";
            dataGridView1.Columns[4].HeaderText = "Фамилия";
            dataGridView1.Columns[5].HeaderText = "Должность";
            dataGridView1.Columns[6].HeaderText = "Шифрование";
            dataGridView1.Columns[7].HeaderText = "Дешифрование";
            dataGridView1.Columns[8].HeaderText = "Запросы данных";
            dataGridView1.Columns[9].HeaderText = "Резервирование";
            dataGridView1.Columns[10].HeaderText = "Анализ данных";
            dataGridView1.Columns[11].HeaderText = "Создание польз.";
            dataGridView1.Columns[12].HeaderText = "Изменение польз.";
            dataGridView1.Columns[13].HeaderText = "Удаление польз.";
            uid = dataGridView1.CurrentCell.RowIndex + 1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            mode = 0;
            Form6 f6 = new Form6();
            f6.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (uid > 0)
            {
                mode = 1;
                Form6 f6 = new Form6();
                f6.Show();
            }
            else MessageBox.Show("Выберите пользователя для действия!");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (uid > 0)
            {
                if (MessageBox.Show("Вы действительно хотите удалить запись? Это действие нельзя будет отменить", "Удаление записи", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    string conn = "Data Source=DESKTOP-MGN38Q8\\SQLEXPRESS;Initial Catalog=GruzhinskasDB;Integrated Security=True";
                    SqlConnection sqlc = new SqlConnection(conn);
                    sqlc.Open();
                    string sql = "DELETE FROM Users WHERE userID = @uid";
                    SqlCommand command = new SqlCommand(sql, sqlc);
                    command.Parameters.Add("uid", uid);
                    command.ExecuteNonQuery();
                    sqlc.Close();
                    MessageBox.Show("Запись была успешно удалена из базы данных", "Удаление записи", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else MessageBox.Show("Выберите пользователя для действия!");
        }
    }
}
