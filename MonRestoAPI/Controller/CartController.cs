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

    [HttpPost("create")]
    public async Task<IActionResult> AddCart()
    {
        var cart = new Cart();
        await _unitOfWork.Carts.AddAsync(cart);
        await _unitOfWork.SaveChangesAsync();

        return Ok(cart);
    }

    // Retrieve a Cart by its ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCartById(int id)
    {
        var cart = await _unitOfWork.Carts.GetByIdAsync(id);
        if (cart == null)
        {
            return NotFound("Cart not found.");
        }

        return Ok(cart);
    }

    // Add a CartItem to a Cart
    [HttpPost("{cartId}/items")]
    public async Task<IActionResult> CreateCartItem(int cartId, [FromBody] CartItemDto cartItemDto)
    {
        // Validate the input DTO
        if (cartItemDto == null || cartItemDto.ArticleId <= 0 || cartItemDto.Quantity <= 0)
        {
            return BadRequest("Invalid CartItem data.");
        }

        // Check if the Cart exists
        var cart = await _unitOfWork.Carts.GetByIdAsync(cartId);
        if (cart == null)
        {
            return NotFound("Cart not found.");
        }

        // Check if the Article exists
        var article = await _unitOfWork.Articles.GetByIdAsync(cartItemDto.ArticleId);
        if (article == null)
        {
            return NotFound("Article not found.");
        }

        // Create a new CartItem
        var cartItem = _mapper.Map<CartItem>(cartItemDto);
        cartItem.CartId = cartId;
        cartItem.Price = article.Price; // Set the price from the Article entity

        await _unitOfWork.CartItems.AddAsync(cartItem);
        await _unitOfWork.SaveChangesAsync();

        // Return the created CartItem as a response
        var resultDto = _mapper.Map<CartItemDto>(cartItem);
        return CreatedAtAction(nameof(GetCartById), new { id = cartItem.Id }, resultDto);
    }
}
