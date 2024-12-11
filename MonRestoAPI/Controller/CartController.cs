using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MonResto.API.Dto;
using MonRestoAPI.Models;
using MonRestoAPI.Repositories;

[ApiController]
[Route("api/[controller]")]
public class CartController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CartController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    // Get Cart by User ID
    [HttpGet("GetByUserId/{userId}")]
    public async Task<IActionResult> GetCartByUserId(int userId)
    {
        var cart = await _unitOfWork.Carts.FindAsync(x => x.UserId == userId);
        if (cart == null)
        {
            return NotFound($"Cart for user ID {userId} not found.");
        }

        var cartItems = await _unitOfWork.CartItems.FindAsync(x => x.CartId == cart.Id);
        var cartItemDtos = _mapper.Map<List<CartItemDto>>(cartItems);
        return Ok(cartItemDtos);
    }

    // Create or Update Cart (Add Item to Cart)
    [HttpPost("AddItem")]
    public async Task<IActionResult> AddItem(CartItemDto cartItemDto)
    {
        // Ensure the user exists
        var user = await _unitOfWork.UserProfiles.GetByIdAsync(cartItemDto.UserId);
        if (user == null)
        {
            return NotFound($"User with ID {cartItemDto.UserId} not found.");
        }

        // Find or create the user's cart
        var cart = await _unitOfWork.Carts.FindAsync(x => x.UserId == cartItemDto.UserId);
        if (cart == null)
        {
            cart = new Cart { UserId = cartItemDto.UserId };
            await _unitOfWork.Carts.AddAsync(cart);
            await _unitOfWork.SaveChangesAsync();
        }

        // Check if the item already exists in the cart
        var existingCartItem = await _unitOfWork.CartItems.FindAsync(x => x.CartId == cart.Id && x.ArticleId == cartItemDto.ArticleId);
        if (existingCartItem != null)
        {
            existingCartItem.Quantity += cartItemDto.Quantity;
            _unitOfWork.CartItems.Update(existingCartItem);
        }
        else
        {
            var newCartItem = _mapper.Map<CartItem>(cartItemDto);
            newCartItem.CartId = cart.Id;
            await _unitOfWork.CartItems.AddAsync(newCartItem);
        }

        await _unitOfWork.SaveChangesAsync();
        return Ok();
    }

    // Clear all items from the user's cart
    [HttpDelete("ClearCart/{userId}")]
    public async Task<IActionResult> ClearCart(int userId)
    {
        var cart = await _unitOfWork.Carts.FindAsync(x => x.UserId == userId);
        if (cart == null)
        {
            return NotFound($"Cart for user ID {userId} not found.");
        }

        var cartItems = _unitOfWork.CartItems.GetAll().Where(x => x.CartId == cart.Id);
        foreach (var item in cartItems)
        {
            _unitOfWork.CartItems.Delete(item);
        }

        await _unitOfWork.SaveChangesAsync();
        return Ok($"Cart cleared for user ID {userId}.");
    }
}
