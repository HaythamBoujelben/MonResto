namespace MonRestoAPI.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public int? UserProfileId { get; set; }
        public UserProfile UserProfile { get; set; }
        
    }
}
