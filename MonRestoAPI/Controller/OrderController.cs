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

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var orders =  _unitOfWork.Orders
                .Include(x => x.UserProfile)
                .GetAll().ToList();
            return Ok(orders);
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

            return Ok(order);
        }

        // Create a New Order (finalizing the cart)
        [HttpPost("Create")]
        public async Task<IActionResult> Create(OrderDto orderDto)
        {
            try
            {
                // Validate the user profile
                var userProfile = await _unitOfWork.UserProfiles.GetByIdAsync(orderDto.UserProfileId);
                if (userProfile == null)
                {
                    return BadRequest("User Profile not found.");
                }

                // Map OrderDto to Order entity
                var newOrder = new Order()
                {
                    OrderDate = orderDto.OrderDate,
                    UserProfileId = orderDto.UserProfileId,
                    TotalPrice = orderDto.TotalPrice,
                };
                var idOrder = await _unitOfWork.Orders.AddAndGetIdAsync(newOrder);
                await _unitOfWork.SaveChangesAsync();

                // Add OrderItems to the Order
                foreach (var itemDto in orderDto.OrderItems)
                {
                    var orderItem = new OrderItem
                    {
                        OrderId = idOrder,
                        ArticleId = itemDto.ArticleId,
                        Quantity = itemDto.Quantity,
                        Price = itemDto.Price
                    };
                    await _unitOfWork.OrderItems.AddAsync(orderItem);
                }

                await _unitOfWork.SaveChangesAsync();

                return Ok(newOrder);
            }
            catch (Exception ex)
            {
                // Log the exception (use a logging library like Serilog, NLog, or built-in logging)
                Console.Error.WriteLine($"Error creating order: {ex.Message}");

                // Return a meaningful error response
                return StatusCode(500, "An error occurred while creating the order. Please try again later.");
            }
        }


        // Update Order (e.g., change status)
        [HttpPut("Update/{Id}")]
        public async Task<IActionResult> Update(int Id, OrderDto orderDto)
        {
            var existingOrder = await _unitOfWork.Orders.GetByIdAsync(Id);
            if (existingOrder == null)
            {
                return NotFound($"Order with ID {Id} not found.");
            }

            // Update Order fields (for example, status)
            _mapper.Map(orderDto, existingOrder);
            _unitOfWork.Orders.Update(existingOrder);
            await _unitOfWork.SaveChangesAsync();

            return Ok(existingOrder);
        }

        // Delete Order (cancel or remove)
        [HttpDelete("Delete/{Id}")]
        public async Task<IActionResult> Delete(int Id)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(Id);
            if (order == null)
            {
                return NotFound($"Order with ID {Id} not found.");
            }

            // Remove order and order items
            _unitOfWork.Orders.Delete(order);
            var orderItems = _unitOfWork.OrderItems.GetAll().Where(x => x.OrderId == Id).ToList();
            foreach (var item in orderItems)
            {
                _unitOfWork.OrderItems.Delete(item);
            }

            await _unitOfWork.SaveChangesAsync();

            return Ok();
        }
    }
}
