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
using System.Runtime.CompilerServices;

namespace WindowsFormsApp1
{
    public partial class Form2 : Form
    {
        public static byte[] key1, key2;
        public static List<JSONAnswer> answers = new List<JSONAnswer>();
        public static Form2 f2;
        public static CancellationTokenSource cts;
        public static Kuznechik kuznechik = new Kuznechik();
        public static bool canEncrypt, canDecrypt, canGetResponses, canReserveCopying, canTrafAnalytic, canManageCreate, canManageModify, canManageDelete;
        public Form2()
        {
            InitializeComponent();
            f2 = this;
            key1 = new byte[16] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            key2 = new byte[16] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            label2.Text = Form1.username;
            if (canEncrypt == true) comboBox1.Items.Add("Режим шифрования");
            if (canDecrypt == true) comboBox1.Items.Add("Режим дешифрования");
            if (canReserveCopying == true) comboBox1.Items.Add("Режим приёма данных");
            textBox2.ReadOnly = true;
            button1.Enabled = true;
            button4.Enabled = false;
            button5.Enabled = false;
            kuznechik.RoundKeys(key1, key2);
            cts = new CancellationTokenSource();
            if (canReserveCopying == true) button2.Enabled = true; else button2.Enabled = false;
            if (canTrafAnalytic == true) button3.Enabled = true; else button3.Enabled = false;
            if ((canManageCreate && canManageModify && canManageDelete) == false) toolStripButton1.Enabled = false; else toolStripButton1.Enabled = true;
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
        private async void button4_Click(object sender, EventArgs e)
        {
            var token = cts.Token;
            textBox1.Clear();
            button4.Enabled = false;
            button5.Enabled = true;
            for (int i = 0; i < 10; i++) {
                await DoRequest(token, textBox1);
            }
        }
        public static async Task DoRequest(CancellationToken canceltoken, TextBox tb)
        {
            if (!canceltoken.IsCancellationRequested)
            {
                string result = await Get(canceltoken);
                JSONAnswer answer = JsonConvert.DeserializeObject<JSONAnswer>(result);
                if (answer.status == "success")
                {
                    tb.Text += answer.text;
                }
                answers.Add(answer);
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            cts.Cancel();
            cts.Dispose();
            button4.Enabled = true;
            button5.Enabled = false;
            cts = new CancellationTokenSource();
        }
        public static async Task<string> Get(CancellationToken token)
        {
            while (true)
            {
                using (var client = new HttpClient())
                {
                    try 
                    { 
                    Uri endpoint = new Uri("https://fish-text.ru/get");
                    var result = await client.GetAsync(endpoint.ToString(), token).ConfigureAwait(false);
                    return await result.Content.ReadAsStringAsync();
                    }
                    catch (Exception ex) 
                    {
                        string answer = "{\"status\":\"cancelled\",\"text\":\"\"}";
                        return answer.ToString();
                    }
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
            {
                button4.Enabled = true;
            }
            else { button4.Enabled = false; button5.Enabled = false; }
        }
        private static string Encrypt (string input)
        {
            string res = "";
            byte[] bytes = Encoding.Unicode.GetBytes(input);
            int rnk = (int)Math.Ceiling((decimal)bytes.Length / 16);
            if (bytes.Length < 16 * rnk) Array.Resize(ref bytes, 16 * rnk);
            for (int i = 0; i < bytes.Length; i+=2) 
            { 
                if (bytes[i]==0 && bytes[i+1]==0) { bytes[i] = (byte)0x20; bytes[i + 1] = 0; }  
            }
            byte[][] data = new byte[rnk][];
            for (int i=0; i < rnk; i++)
            {
                data[i] = new byte[16];
                Array.Copy(bytes, i * 16, data[i], 0, 16);
            }
            for (int i = 0; i < rnk; i++)
            {
                data[i] = kuznechik.Encrypt(data[i]);
                res += Encoding.Unicode.GetString(data[i]);
            }
            return res;
        }
        private static string Decrypt (string input)
        {
            string res = "";
            byte[] bytes = Encoding.Unicode.GetBytes(input);
            int rnk = (int)Math.Ceiling((decimal)bytes.Length / 16);
            byte[][] data = new byte[rnk][];
            for (int i = 0; i < rnk; i++)
            {
                data[i] = new byte[16];
                Array.Copy(bytes, i * 16, data[i], 0, 16);
            }
            for (int i = 0; i < rnk; i++)
            {
                data[i] = kuznechik.Decrypt(data[i]);
                res += Encoding.Unicode.GetString(data[i]);
            }
            return res;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Form5 f5 = new Form5();
            f5.Show();
        }

        private void ключиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form4 f4 = new Form4();
            f4.Show();
        }
    }
}
