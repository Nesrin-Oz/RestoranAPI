using System.Net.Http.Json; 

namespace RestoranWinFormClient
{
    public partial class Form1 : Form
    {
        private static readonly HttpClient _client = new HttpClient()
        {
            BaseAddress = new Uri("https://localhost:7100/api/")
        };

        public Form1()
        {
            InitializeComponent();
        }

        private async void btnKaydet_Click(object sender, EventArgs e)
        {
            var yeniRezervasyon = new Reservation
            {
                TableId = Convert.ToInt32(txtMasaId.Text),
                CustomerId = Convert.ToInt32(txtMusteriId.Text),
                RezervasyonTarihi = dtpTarih.Value,
                BaslangicSaati = dtpBaslangic.Value,
                BitisSaati = dtpBitis.Value
            };

            var response = await _client.PostAsJsonAsync("Reservation", yeniRezervasyon);
            string sonuc = await response.Content.ReadAsStringAsync();

            MessageBox.Show(sonuc);
        }

        private async void btnListele_Click(object sender, EventArgs e)
        {
            var liste = await _client.GetFromJsonAsync<List<Reservation>>("Reservation");

            listBox1.DataSource = liste;
        }
    }
}