namespace RestoranAPI.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public int TableId { get; set; }
        public int CustomerId { get; set; }
        public DateTime RezervasyonTarihi { get; set; }
        public DateTime BaslangicSaati { get; set; }
        public DateTime BitisSaati { get; set; }

        public override string ToString()
        {
            return $"ID:{Id} | Masa:{TableId} | {BaslangicSaati:HH:mm}-{BitisSaati:HH:mm}";
        }

    }


}
