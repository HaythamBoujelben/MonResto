namespace MonRestoAPI.Models
{
    public class Article
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public int? CategoryId { get; set; }
        public int? MenuId { get; set; }
        public Category Category { get; set; }
        public Menu Menu { get; set; }

        //public ICollection<CartItem> CartItems { get; set; }
        //public ICollection<OrderItem> OrderItems { get; set; }
    }

}
