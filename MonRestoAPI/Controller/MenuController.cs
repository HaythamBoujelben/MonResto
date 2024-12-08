using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MonResto.API.Dto;
using MonRestoAPI.Models;
using MonRestoAPI.Repositories;

namespace MonResto.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MenuController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MenuController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // Get All Menus
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var menus = await _unitOfWork.Menus.GetAllAsync();
            var menuDtos = _mapper.Map<List<MenuDto>>(menus);
            return Ok(menuDtos);
        }

        // Get Menu by ID
        [HttpGet("GetById/{menuId}")]
        public async Task<IActionResult> GetByIdAsync(int menuId)
        {
            var menu = await _unitOfWork.Menus.GetByIdAsync(menuId);
            if (menu == null)
            {
                return NotFound($"Menu with ID {menuId} not found.");
            }

            var menuDto = _mapper.Map<MenuDto>(menu);
            return Ok(menuDto);
        }

        // Create Menu
        [HttpPost("Create")]
        public async Task<IActionResult> Create(MenuDto menuDto)
        {
            // Check if the menu name already exists
            var existingMenu = await _unitOfWork.Menus.FindAsync(x => x.Name == menuDto.Name);
            if (existingMenu != null)
            {
                return Conflict("A menu with this name already exists.");
            }

            // Map the MenuDto to the Menu entity
            var newMenu = _mapper.Map<Menu>(menuDto);
            await _unitOfWork.Menus.AddAsync(newMenu);
            await _unitOfWork.SaveChangesAsync();

            // Add Articles to the Menu
            foreach (var articleDto in menuDto.Articles)
            {
                var article = await _unitOfWork.Articles.GetByIdAsync(articleDto.ArticleId);
                if (article != null)
                {
                    article.MenuId = newMenu.MenuId;
                    _unitOfWork.Articles.Update(article);
                }
            }
            await _unitOfWork.SaveChangesAsync();

            return CreatedAtAction(nameof(GetByIdAsync), new { menuId = newMenu.MenuId }, newMenu);
        }

        // Update Menu
        [HttpPut("Update/{menuId}")]
        public async Task<IActionResult> Update(int menuId, MenuDto menuDto)
        {
            var existingMenu = await _unitOfWork.Menus.GetByIdAsync(menuId);
            if (existingMenu == null)
            {
                return NotFound($"Menu with ID {menuId} not found.");
            }

            // Map updated data from MenuDto to Menu entity
            _mapper.Map(menuDto, existingMenu);
            _unitOfWork.Menus.Update(existingMenu);
            await _unitOfWork.SaveChangesAsync();

            // Update Articles associated with the Menu
            foreach (var articleDto in menuDto.Articles)
            {
                var article = await _unitOfWork.Articles.GetByIdAsync(articleDto.ArticleId);
                if (article != null)
                {
                    article.MenuId = existingMenu.MenuId;
                    _unitOfWork.Articles.Update(article);
                }
            }

            await _unitOfWork.SaveChangesAsync();
            return Ok(existingMenu);
        }

        // Delete Menu
        [HttpDelete("Delete/{menuId}")]
        public async Task<IActionResult> Delete(int menuId)
        {
            var menu = await _unitOfWork.Menus.GetByIdAsync(menuId);
            if (menu == null)
            {
                return NotFound($"Menu with ID {menuId} not found.");
            }

            // Delete Articles from this Menu
            var articles = _unitOfWork.Articles.GetAll().Where(x => x.MenuId == menuId);
            foreach (var article in articles)
            {
                article.MenuId = null; 
                _unitOfWork.Articles.Update(article);
            }

            // Delete the Menu
            _unitOfWork.Menus.Delete(menu);
            await _unitOfWork.SaveChangesAsync();

            return Ok();
        }
    }
}
