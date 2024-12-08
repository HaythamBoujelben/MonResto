using MonRestoAPI.Models;

namespace MonResto.API.Dto
{
    public class CartItemDto
    {
        public int CartItemId { get; set; }
        public int UserId { get; set; }
        public int CartId { get; set; }
        public int ArticleId { get; set; }
        public int Quantity { get; set; }
        public decimal? Price { get; set; }
    }

}
