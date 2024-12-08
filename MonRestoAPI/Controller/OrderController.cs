using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MonResto.API.Dto;
using MonRestoAPI.Models;
using MonRestoAPI.Repositories;

namespace MonResto.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // Get Orders by User (by UserProfile ID)
        [HttpGet("GetByUser/{userId}")]
        public async Task<IActionResult> GetByUserAsync(int userId)
        {
            var orders = await _unitOfWork.Orders.FindAsync(x => x.UserId == userId);
            if (orders == null)
            {
                return NotFound($"No orders found for User with ID {userId}.");
            }

            var orderDtos = _mapper.Map<List<OrderDto>>(orders);
            return Ok(orderDtos);
        }

        // Get Order by ID
        [HttpGet("GetById/{orderId}")]
        public async Task<IActionResult> GetByIdAsync(int orderId)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(orderId);
            if (order == null)
            {
                return NotFound($"Order with ID {orderId} not found.");
            }

            var orderDto = _mapper.Map<OrderDto>(order);
            return Ok(orderDto);
        }

        // Create a New Order (finalizing the cart)
        [HttpPost("Create")]
        public async Task<IActionResult> Create(OrderDto orderDto)
        {
            // Check if the User exists
            var userProfile = await _unitOfWork.UserProfiles.GetByIdAsync(orderDto.UserProfileId);
            if (userProfile == null)
            {
                return BadRequest("User Profile not found.");
            }

            // Calculate the total amount of the order (assuming you can get this info)
            decimal totalAmount = 0;
            foreach (var itemDto in orderDto.OrderItems)
            {
                var article = await _unitOfWork.Articles.GetByIdAsync(itemDto.ArticleId);
                if (article == null)
                {
                    return BadRequest($"Article with ID {itemDto.ArticleId} not found.");
                }
                totalAmount += article.Price * itemDto.Quantity;
            }

            // Map the OrderDto to the Order entity
            var newOrder = _mapper.Map<Order>(orderDto);
            newOrder.TotalPrice = totalAmount;
            newOrder.OrderDate = DateTime.UtcNow;

            // Add order to the repository and save changes
            await _unitOfWork.Orders.AddAsync(newOrder);
            await _unitOfWork.SaveChangesAsync();

            // Add the order items
            foreach (var itemDto in orderDto.OrderItems)
            {
                var newOrderItem = new OrderItem
                {
                    OrderId = newOrder.OrderId,
                    ArticleId = itemDto.ArticleId,
                    Quantity = itemDto.Quantity,
                    Price = itemDto.Price // You may use the price passed or retrieve it from the article
                };
                await _unitOfWork.OrderItems.AddAsync(newOrderItem);
            }

            await _unitOfWork.SaveChangesAsync();

            return CreatedAtAction(nameof(GetByIdAsync), new { orderId = newOrder.OrderId }, newOrder);
        }

        // Update Order (e.g., change status)
        [HttpPut("Update/{orderId}")]
        public async Task<IActionResult> Update(int orderId, OrderDto orderDto)
        {
            var existingOrder = await _unitOfWork.Orders.GetByIdAsync(orderId);
            if (existingOrder == null)
            {
                return NotFound($"Order with ID {orderId} not found.");
            }

            // Update Order fields (for example, status)
            _mapper.Map(orderDto, existingOrder);
            _unitOfWork.Orders.Update(existingOrder);
            await _unitOfWork.SaveChangesAsync();

            return Ok(existingOrder);
        }

        // Delete Order (cancel or remove)
        [HttpDelete("Delete/{orderId}")]
        public async Task<IActionResult> Delete(int orderId)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(orderId);
            if (order == null)
            {
                return NotFound($"Order with ID {orderId} not found.");
            }

            // Remove order and order items
            _unitOfWork.Orders.Delete(order);
            var orderItems = _unitOfWork.OrderItems.GetAll().Where(x => x.OrderId == orderId).ToList();
            foreach (var item in orderItems)
            {
                _unitOfWork.OrderItems.Delete(item);
            }

            await _unitOfWork.SaveChangesAsync();

            return Ok();
        }
    }
}
