namespace MonRestoAPI.Models
{
    public class Cart
    {
        public int CartId { get; set; }
        public int UserId { get; set; }
        public UserProfile User { get; set; }
        public ICollection<CartItem> CartItems { get; set; }
    }
}
