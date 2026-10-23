using Microsoft.Data.SqlClient;
using RestoranAPI.Models;
using System.Data;

namespace RestoranAPI.Services
{
    public class ReservationService
    {
        private readonly string _connectionString;

        public ReservationService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public string RezervasyonEkle(Reservation r)
        {
            using SqlConnection con = new SqlConnection(_connectionString);
            con.Open();

            using SqlCommand kontrol = new SqlCommand(
                @"SELECT COUNT(*) FROM Reservations 
                  WHERE TableId = @TableId 
                  AND RezervasyonTarihi = @Tarih
                  AND @Baslangic < BitisSaati 
                  AND @Bitis > BaslangicSaati", con);

            kontrol.Parameters.AddWithValue("@TableId", r.TableId);
            kontrol.Parameters.AddWithValue("@Tarih", r.RezervasyonTarihi);
            kontrol.Parameters.AddWithValue("@Baslangic", r.BaslangicSaati);
            kontrol.Parameters.AddWithValue("@Bitis", r.BitisSaati);

            int count = (int)kontrol.ExecuteScalar();
            if (count > 0) return "Bu masa seçilen saat aralığında dolu.";

            using SqlCommand cmd = new SqlCommand(
                @"INSERT INTO Reservations 
                  (TableId, CustomerId, RezervasyonTarihi, BaslangicSaati, BitisSaati)
                  VALUES (@TableId, @CustomerId, @Tarih, @Baslangic, @Bitis)", con);

            cmd.Parameters.AddWithValue("@TableId", r.TableId);
            cmd.Parameters.AddWithValue("@CustomerId", r.CustomerId);
            cmd.Parameters.AddWithValue("@Tarih", r.RezervasyonTarihi);
            cmd.Parameters.AddWithValue("@Baslangic", r.BaslangicSaati);
            cmd.Parameters.AddWithValue("@Bitis", r.BitisSaati);

            cmd.ExecuteNonQuery();
            return "Rezervasyon başarılı";
        }

        public List<Reservation> RezervasyonListele()
        {
            List<Reservation> rezervasyonlar = new List<Reservation>();
            using SqlConnection con = new SqlConnection(_connectionString);
            using SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Reservations", con);

            DataTable dt = new DataTable();
            da.Fill(dt);

            foreach (DataRow row in dt.Rows)
            {
                rezervasyonlar.Add(new Reservation
                {
                    Id = Convert.ToInt32(row["Id"]),
                    TableId = Convert.ToInt32(row["TableId"]),
                    CustomerId = Convert.ToInt32(row["CustomerId"]),
                    RezervasyonTarihi = Convert.ToDateTime(row["RezervasyonTarihi"]),
                    BaslangicSaati = Convert.ToDateTime(row["BaslangicSaati"]),
                    BitisSaati = Convert.ToDateTime(row["BitisSaati"])
                });
            }
            return rezervasyonlar;
        }

        public string RezervasyonSil(int id)
        {
            using SqlConnection con = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("DELETE FROM Reservations WHERE Id = @Id", con);
            cmd.Parameters.AddWithValue("@Id", id);

            con.Open();
            int etkilenen = cmd.ExecuteNonQuery();
            return etkilenen == 0 ? "Silinecek kayıt bulunamadı." : "Rezervasyon silindi.";
        }

        public List<SaatAraligi> BosSaatleriHesapla(int tableId, DateTime tarih)
        {
            List<SaatAraligi> bosSaatler = new List<SaatAraligi>();
            List<Reservation> doluSaatler = new List<Reservation>();

            using SqlConnection con = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand(
                @"SELECT * FROM Reservations 
                  WHERE TableId = @TableId AND RezervasyonTarihi = @Tarih", con);

            cmd.Parameters.AddWithValue("@TableId", tableId);
            cmd.Parameters.AddWithValue("@Tarih", tarih);

            con.Open();
            using SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                doluSaatler.Add(new Reservation
                {
                    BaslangicSaati = Convert.ToDateTime(dr["BaslangicSaati"]),
                    BitisSaati = Convert.ToDateTime(dr["BitisSaati"])
                });
            }

            DateTime aktif = tarih.AddHours(10);
            DateTime gunBitis = tarih.AddHours(22);

            foreach (var r in doluSaatler.OrderBy(x => x.BaslangicSaati))
            {
                if (aktif < r.BaslangicSaati)
                {
                    bosSaatler.Add(new SaatAraligi { Baslangic = aktif, Bitis = r.BaslangicSaati });
                }
                aktif = r.BitisSaati;
            }

            if (aktif < gunBitis)
            {
                bosSaatler.Add(new SaatAraligi { Baslangic = aktif, Bitis = gunBitis });
            }

            return bosSaatler;
        }
    }
}