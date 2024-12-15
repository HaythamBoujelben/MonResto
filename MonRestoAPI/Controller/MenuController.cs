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

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var menus = _unitOfWork.Menus.GetAll().ToList();
            return Ok(menus);
        }

        [HttpGet("GetById/{menuId}")]
        public async Task<IActionResult> GetByIdAsync(int menuId)
        {
            var menu = await _unitOfWork.Menus.GetByIdAsync(menuId);
            if (menu == null)
            {
                return NotFound($"Menu with ID {menuId} not found.");
            }
            return Ok(menu);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(MenuDto menuDto)
        {
            var existingMenu = await _unitOfWork.Menus.FindAsync(x => x.Name == menuDto.Name);
            if (existingMenu != null)
            {
                return Conflict("A menu with this name already exists.");
            }
            var newMenu = _mapper.Map<Menu>(menuDto);
            await _unitOfWork.Menus.AddAsync(newMenu);
            await _unitOfWork.SaveChangesAsync();


            return Ok(newMenu);
        }

        [HttpPut("Update/{Id}")]
        public async Task<IActionResult> Update(int Id, MenuDto menuDto)
        {
            var existingMenu = await _unitOfWork.Menus.GetByIdAsync(Id);
            if (existingMenu == null)
            {
                return NotFound($"Menu with ID {Id} not found.");
            }

            _mapper.Map(menuDto, existingMenu);
            _unitOfWork.Menus.Update(existingMenu);
            await _unitOfWork.SaveChangesAsync();

            return Ok(existingMenu);
        }

        [HttpDelete("Delete/{Id}")]
        public async Task<IActionResult> Delete(int Id)
        {
            var menu = await _unitOfWork.Menus.GetByIdAsync(Id);
            if (menu == null)
            {
                return NotFound($"Menu with ID {Id} not found.");
            }

            var articles = _unitOfWork.Articles.GetAll().Where(x => x.MenuId == Id);
            foreach (var article in articles)
            {
                article.MenuId = null; 
                _unitOfWork.Articles.Update(article);
            }
            _unitOfWork.Menus.Delete(menu);
            await _unitOfWork.SaveChangesAsync();

            return Ok();
        }
    }
}
