namespace MonRestoAPI.Models
{
    public class Article
    {
        public int ArticleId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }

}
