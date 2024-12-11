namespace MonRestoAPI.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public UserProfile UserProfile { get; set; }
        public ICollection<CartItem> CartItems { get; set; }
    }
}
