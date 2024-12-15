using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MonResto.API.Dto;
using MonRestoAPI.Models;
using MonRestoAPI.Repositories;

namespace MonResto.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartItemController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CartItemController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("GetByCart/{cartId}")]
        public async Task<IActionResult> GetByCartIdAsync(int cartId)
        {
            var cartItems = await _unitOfWork.CartItems.FindAsync(x => x.CartId == cartId);
            if (cartItems == null)
            {
                return NotFound($"No items found for Cart with ID {cartId}.");
            }
            var cartItemDtos = _mapper.Map<List<CartItemDto>>(cartItems);
            return Ok(cartItemDtos);
        }

        [HttpGet("GetByArticleInCart")]
        public async Task<IActionResult> GetByArticleInCart(int cartId, int articleId)
        {
            var cartItem = await _unitOfWork.CartItems.FindAsync(x => x.CartId == cartId && x.ArticleId == articleId);
            if (cartItem == null)
            {
                return NotFound($"CartItem with CartId {cartId} and ArticleId {articleId} not found.");
            }

            var cartItemDto = _mapper.Map<CartItemDto>(cartItem);
            return Ok(cartItemDto);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(CartItemDto cartItemDto)
        {
            var cart = await _unitOfWork.Carts.GetByIdAsync(cartItemDto.CartId);
            if (cart == null)
            {
                return BadRequest("Cart not found.");
            }
            var article = await _unitOfWork.Articles.GetByIdAsync(cartItemDto.ArticleId);
            if (article == null)
            {
                return BadRequest("Article not found.");
            }
            var newCartItem = _mapper.Map<CartItem>(cartItemDto);
            await _unitOfWork.CartItems.AddAsync(newCartItem);
            await _unitOfWork.SaveChangesAsync();

            return CreatedAtAction(nameof(GetByCartIdAsync), new { cartId = newCartItem.CartId }, newCartItem);
        }

        [HttpPut("Update/{cartItemId}")]
        public async Task<IActionResult> Update(int cartItemId, CartItemDto cartItemDto)
        {
            var existingCartItem = await _unitOfWork.CartItems.GetByIdAsync(cartItemId);
            if (existingCartItem == null)
            {
                return NotFound($"CartItem with ID {cartItemId} not found.");
            }

            _mapper.Map(cartItemDto, existingCartItem);

            _unitOfWork.CartItems.Update(existingCartItem);
            await _unitOfWork.SaveChangesAsync();

            return Ok(existingCartItem);
        }

        [HttpDelete("Delete/{cartItemId}")]
        public async Task<IActionResult> Delete(int cartItemId)
        {
            var cartItem = await _unitOfWork.CartItems.GetByIdAsync(cartItemId);
            if (cartItem == null)
            {
                return NotFound($"CartItem with ID {cartItemId} not found.");
            }

            _unitOfWork.CartItems.Delete(cartItem);
            await _unitOfWork.SaveChangesAsync();

            return Ok();
        }
    }
}
