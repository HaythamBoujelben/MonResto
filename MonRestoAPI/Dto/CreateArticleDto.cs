using MonRestoAPI.Models;

namespace MonResto.API.Dto
{
    public class CreateArticleDto
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
    }
}
