namespace RestoranAPI.Models
{
    public class SaatAraligi
    {
        public DateTime Baslangic { get; set; }
        public DateTime Bitis { get; set; }

        public override string ToString()
        {
            return $"{Baslangic:HH:mm} - {Bitis:HH:mm}";
        }
    }
}
