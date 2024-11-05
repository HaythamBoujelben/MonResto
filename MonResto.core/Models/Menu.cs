namespace MonRestoAPI.Models
{
    public class Menu
    {
        public int MenuId { get; set; }
        public string Name { get; set; }
        public ICollection<Article> Articles { get; set; }
    }

}
