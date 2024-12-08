using MonRestoAPI.Models;

namespace MonResto.API.Dto
{
    public class MenuDto
    {
        public int MenuId { get; set; }
        public string Name { get; set; }
        public List<ArticleDto> Articles { get; set; }
    }


}
