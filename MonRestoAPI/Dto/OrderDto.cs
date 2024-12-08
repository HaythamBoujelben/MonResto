namespace MonResto.API.Dto
{
    public class OrderDto
    {
        public int UserProfileId { get; set; } // Linked to UserProfile
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } // E.g., "Pending", "Completed", "Cancelled"
        public List<OrderItemDto> OrderItems { get; set; } // Items in this order
    }
}
