﻿namespace MonResto.API.Dto
{
    public class OrderItemDto
    {
        public int OrderId { get; set; }
        public int ArticleId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }


}