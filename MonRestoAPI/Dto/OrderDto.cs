namespace MonResto.API.Dto
{
    public class OrderDto
    {
        public int UserProfileId { get; set; } 
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } // E.g., "Pending", "Completed", "Cancelled"
    }
}
