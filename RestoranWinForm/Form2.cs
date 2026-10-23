using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RestoranWinForm
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "admin" && textBox2.Text == "123")
            {
                try
                {
                    RegistryKey rkey = Registry.CurrentUser.CreateSubKey(@"Software\RestoranRezervasyon");

                    rkey.SetValue("KullaniciAdi", textBox1.Text);
                    rkey.Close();
                }
                catch { }

                this.Hide();
                Form1 anaForm = new Form1();
                anaForm.Show();
            }
            else
            {
                MessageBox.Show("Kullanıcı adı veya şifre hatalı");
            }
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            try
            {
                RegistryKey rkey = Registry.CurrentUser.OpenSubKey(@"Software\RestoranRezervasyon");

                if (rkey != null)
                {
                    textBox1.Text = rkey.GetValue("KullaniciAdi")?.ToString();
                    rkey.Close();
                }
            }
            catch { }
        }
    }

}
