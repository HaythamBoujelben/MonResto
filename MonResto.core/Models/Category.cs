namespace MonRestoAPI.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public ICollection<Article> Articles { get; set; }
    }
}
