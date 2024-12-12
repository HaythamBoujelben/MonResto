using MonRestoAPI.Models;

namespace MonResto.API.Dto
{
    public class ArticleDto
    {
        public int ArticleId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public int? CategoryId { get; set; }
        public int? MenuId { get; set; }
    }
}
