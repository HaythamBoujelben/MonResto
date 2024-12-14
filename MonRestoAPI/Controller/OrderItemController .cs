using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MonResto.API.Dto;
using MonRestoAPI.Models;
using MonRestoAPI.Repositories;

[ApiController]
[Route("api/[controller]")]
public class OrderItemController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public OrderItemController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    // Get All OrderItems by OrderId
    [HttpGet("GetByOrderId/{orderId}")]
    public async Task<IActionResult> GetOrderItemsByOrderId(int orderId)
    {
        var orderItems =  _unitOfWork.OrderItems.Include(x =>x.Order).Include(x => x.Article).Include(x => x.Article.Menu).Include(x => x.Order.UserProfile).Include(x => x.Article.Category).GetAll().Where(x => x.OrderId == orderId).ToList();
        if (orderItems == null)
        {
            return NotFound($"No items found for Order with ID {orderId}.");
        }

        return Ok(orderItems);
    }

    // Get OrderItem by ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderItemById(int id)
    {
        var orderItem = await _unitOfWork.OrderItems.GetByIdAsync(id);
        if (orderItem == null)
        {
            return NotFound($"OrderItem with ID {id} not found.");
        }

        var orderItemDto = _mapper.Map<OrderItemDto>(orderItem);
        return Ok(orderItemDto);
    }

    // Create OrderItem (usually called when an order is placed)
    [HttpPost]
    public async Task<IActionResult> CreateOrderItem(OrderItemDto orderItemDto)
    {
        var order = await _unitOfWork.Orders.GetByIdAsync(orderItemDto.OrderId);
        if (order == null)
        {
            return NotFound($"Order with ID {orderItemDto.OrderId} not found.");
        }

        var article = await _unitOfWork.Articles.GetByIdAsync(orderItemDto.ArticleId);
        if (article == null)
        {
            return NotFound($"Article with ID {orderItemDto.ArticleId} not found.");
        }

        // Create new OrderItem from DTO
        var newOrderItem = _mapper.Map<OrderItem>(orderItemDto);
        newOrderItem.Price = article.Price * orderItemDto.Quantity; // Set price based on article price

        await _unitOfWork.OrderItems.AddAsync(newOrderItem);
        await _unitOfWork.SaveChangesAsync();

        return CreatedAtAction(nameof(GetOrderItemById), new { id = newOrderItem.Id }, newOrderItem);
    }

    // Update OrderItem (e.g., updating the quantity)
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateOrderItem(int id, OrderItemDto orderItemDto)
    {
        var orderItem = await _unitOfWork.OrderItems.GetByIdAsync(id);
        if (orderItem == null)
        {
            return NotFound($"OrderItem with ID {id} not found.");
        }

        // Update OrderItem with new quantity
        orderItem.Quantity = orderItemDto.Quantity;
        orderItem.Price = orderItemDto.Quantity * orderItem.Price; // Update the price based on new quantity

        _unitOfWork.OrderItems.Update(orderItem);
        await _unitOfWork.SaveChangesAsync();

        return Ok(orderItem);
    }

    // Delete OrderItem
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrderItem(int id)
    {
        var orderItem = await _unitOfWork.OrderItems.GetByIdAsync(id);
        if (orderItem == null)
        {
            return NotFound($"OrderItem with ID {id} not found.");
        }

        // Delete the OrderItem
        _unitOfWork.OrderItems.Delete(orderItem);
        await _unitOfWork.SaveChangesAsync();

        return Ok($"OrderItem with ID {id} deleted.");
    }
}
