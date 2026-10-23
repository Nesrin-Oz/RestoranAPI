namespace RestoranAPI.Models
{
    public class Table
    {
        public int Id { get; set; }
        public int Kapasite { get; set; }
        public int RestaurantId { get; set; }
    }
}
