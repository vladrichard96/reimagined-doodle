using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Net.Http.Headers;
using System.Net.Http;
using Newtonsoft.Json;


namespace WindowsFormsApp1
{
    public partial class Form2 : Form
    {
        int access = Form1.useraccess;
        bool dataaccept = false;
        public static List<JSONAnswer> answers = new List<JSONAnswer>();
        public Form2()
        {
            InitializeComponent();
            label2.Text = "Пользователь " + access + " категории";
            comboBox1.Items.AddRange(new string[] { "Режим шифрования", "Режим дешифрования", "Режим приёма данных" });
            textBox2.ReadOnly = true;
            button4.Enabled = false;
            switch (access)
            {
                case 3:
                    {
                        button1.Enabled = true;
                        button2.Enabled = false;
                        button3.Enabled = false;
                        break;
                    }
                case 2:
                    {
                        button1.Enabled = true;
                        button2.Enabled = true;
                        button3.Enabled = false;
                        break;
                    }
                case 1:
                    {
                        button1.Enabled = true;
                        button2.Enabled = true;
                        button3.Enabled = true;
                        break;
                    }
            }
            
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0) textBox2.Text = Encrypt(textBox1.Text);
            if (comboBox1.SelectedIndex == 1) textBox2.Text = Decrypt(textBox1.Text);
            if (comboBox1.SelectedIndex == 2) textBox2.Text = Encrypt(textBox1.Text);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Файлы данных (*.dat)|*.dat|Все файлы (*.*)|*.*";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string path = saveFileDialog1.FileName;
                FileInfo f = new FileInfo(path);
                StreamWriter sw = new StreamWriter(f.FullName);
                sw.WriteLine(textBox2.Text);
                sw.Close();
                File.Encrypt(path);
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            Form3 f3 = new Form3();
            f3.Show();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            if (dataaccept == false) { 
                button4.Text = "Стоп";
                dataaccept = true;
                textBox1.Clear();
            }
            else
            {
                dataaccept = false;
                button4.Text = "Начать";
            }
            if (dataaccept)
            {
                using (var client = new HttpClient())
                {
                    var endpoint = new Uri("https://fish-text.ru/get");
                    var result = client.GetAsync(endpoint).Result;
                    var json = result.Content.ReadAsStringAsync().Result;
                    JSONAnswer answer = JsonConvert.DeserializeObject<JSONAnswer>(json);
                    if (answer.status == "success")
                    {
                        textBox1.Text += answer.text;
                    }
                    answers.Add(answer);
                }
            }
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 1) button1.Text = "Дешифровать";
            else button1.Text = "Зашифровать";
            textBox1.Clear();
            textBox2.Clear();
            if (comboBox1.SelectedIndex == 2)
                button4.Enabled = true;
            else button4.Enabled = false;
        }
        private static string Encrypt (string input)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(input);
            string[] h = bytes.Select(x => x.ToString("x2")).ToArray();
            string hex = string.Concat(h);
            return hex;
        }
        private static string Decrypt (string input)
        {
            string[] hexBytes = new string[input.Length / 2];
            for (int i = 0; i < hexBytes.Length; i++)
            {
                hexBytes[i] = input.Substring(i * 2, 2);
            }
            byte[] resultBytes = hexBytes.Select(value => Convert.ToByte(value, 16)).ToArray();
            string result = Encoding.UTF8.GetString(resultBytes);
            return result;
        }       
    }
}
