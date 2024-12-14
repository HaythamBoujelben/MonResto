namespace MonResto.API.Dto
{
    public class OrderDto
    {
        public int UserProfileId { get; set; } // User who placed the order
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }
    }
}
