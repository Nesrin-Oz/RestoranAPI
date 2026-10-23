using Newtonsoft.Json;
using RestoranWinForm.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;




namespace RestoranWinForm
{


    public partial class Form1 : Form
    {

        HttpClient client = new HttpClient()
        {
            BaseAddress = new Uri("https://localhost:7100/")
        };

        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e) //BU kısmı Thread lerle yapmakda zorlandım. Bundan dolayı async ve await kullandım.
        {
            DateTime tarih = dateTimePicker1.Value.Date;

            DateTime baslangic = tarih.AddHours(dateTimePicker2.Value.Hour).AddMinutes(dateTimePicker2.Value.Minute);

            DateTime bitis =tarih.AddHours(dateTimePicker3.Value.Hour).AddMinutes(dateTimePicker3.Value.Minute);

            if (baslangic >= bitis)
            {
                MessageBox.Show(
                    "Bitiş saati başlangıç saatinden sonra olmalıdır.",
                    "Geçersiz Saat Aralığı",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            if (!int.TryParse(textBox1.Text, out int tableId) || !int.TryParse(textBox2.Text, out int customerId))
            {
                MessageBox.Show("Masa ID ve Müşteri ID tam sayı olmalıdır.");
                return;
            }

            Reservation r = new Reservation()
            {

                TableId = Convert.ToInt32(textBox1.Text),
                CustomerId = Convert.ToInt32(textBox2.Text),
                RezervasyonTarihi = tarih,
                BaslangicSaati = baslangic,
                BitisSaati = bitis
            };

            string bilgiMetni = JsonConvert.SerializeObject(r);
            StringContent content = new StringContent(bilgiMetni, Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage response = await client.PostAsync("api/Reservation", content);
                
                string sonuc = await response.Content.ReadAsStringAsync();

                if (sonuc.Contains("dolu"))
                {
                    MessageBox.Show(sonuc," Rezervasyonlar Çakışıyor ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show(sonuc," Başarılı ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private async void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null)
            {
                MessageBox.Show("Lütfen silinecek rezervasyonu seçin.");
                return;
            }

            Reservation secili = (Reservation)listBox1.SelectedItem;

            DialogResult durum = MessageBox.Show(
                "Seçili rezervasyonu silmek istiyor musunuz?",
                "Onay",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (durum != DialogResult.Yes)
                return;

            Thread thread = new Thread(() =>
            {
                try
                {
                    HttpResponseMessage response =client.DeleteAsync($"api/Reservation/{secili.Id}").Result;

                    string sonuc =response.Content.ReadAsStringAsync().Result;

                    this.Invoke(new Action(() =>
                    {
                        MessageBox.Show(sonuc);
                        button3.PerformClick();
                    }));
                }
                catch (Exception ex)
                {
                    this.Invoke(new Action(() =>
                    {
                        MessageBox.Show(ex.Message);
                    }));
                }
            });

            thread.Start();
        }


        private void button3_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(() =>
            {
                HttpResponseMessage response =client.GetAsync("api/Reservation").Result;

                if (response.IsSuccessStatusCode)
                {
                    string bilgiMetni =response.Content.ReadAsStringAsync().Result;

                    List<Reservation> liste =JsonConvert.DeserializeObject<List<Reservation>>(bilgiMetni);

                    listBox1.Invoke(new Action(() =>
                    {
                        listBox1.Items.Clear();
                        foreach (var i in liste)
                        {
                            listBox1.Items.Add(i);
                        }
                    }));
                }
                else
                {
                    listBox1.Invoke(new Action(() =>
                    {
                        MessageBox.Show("Veriler alınamadı");
                    }));
                }
            });

            thread.Start();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(textBox3.Text, out int tableId))
            {
                MessageBox.Show("Masa ID geçerli değil");
                return;
            }

            DateTime tarih = dateTimePicker1.Value.Date;

            Thread thread = new Thread(() =>
            {
                try
                {
                    HttpResponseMessage response =
                        client.GetAsync(
                            $"api/Reservation/BosSaatler?tableId={tableId}&tarih={tarih:yyyy-MM-dd}"
                        ).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        string json =
                            response.Content.ReadAsStringAsync().Result;

                        List<SaatAraligi> liste =
                            JsonConvert.DeserializeObject<List<SaatAraligi>>(json);

                        listBox2.Invoke(new Action(() =>
                        {
                            listBox2.Items.Clear();

                            if (liste.Count == 0)
                            {
                                listBox2.Items.Add("Boş saat yok");
                            }
                            else
                            {
                                foreach (var i in liste)
                                {
                                    listBox2.Items.Add(i);
                                }
                            }
                        }));
                    }
                    else
                    {
                        MessageBoxBosSaatlerHata();
                    }
                }
                catch (Exception ex)
                {
                    listBox2.Invoke(new Action(() =>
                    {
                        MessageBox.Show(ex.Message);
                    }));
                }
            });

            thread.Start();
        }

        void MessageBoxBosSaatlerHata()
        {
            listBox2.Invoke(new Action(() =>
            {
                MessageBox.Show("Boş saatler getirilemedi");
            }));
        }
    }
}
