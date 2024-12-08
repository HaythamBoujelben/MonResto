using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MonResto.API.Dto;
using MonRestoAPI.Models;
using MonRestoAPI.Repositories;

namespace MonResto.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // Get all Categories
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _unitOfWork.Categorys.GetAllAsync();
            if (categories == null || !categories.Any())
            {
                return NotFound("No categories found.");
            }

            var categoryDtos = _mapper.Map<List<CategoryDto>>(categories);
            return Ok(categoryDtos);
        }

        // Get Category by ID
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _unitOfWork.Categorys.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound($"Category with ID {id} not found.");
            }

            var categoryDto = _mapper.Map<CategoryDto>(category);
            return Ok(categoryDto);
        }

        // Create a new Category
        [HttpPost("Create")]
        public async Task<IActionResult> Create(CategoryDto categoryDto)
        {
            // Check if a category with the same name already exists
            var existingCategory = await _unitOfWork.Categorys.FindAsync(x => x.Name == categoryDto.Name);
            if (existingCategory != null)
            {
                return Conflict("A category with this name already exists.");
            }

            // Map the DTO to the Category entity
            var newCategory = _mapper.Map<Category>(categoryDto);

            // Add the category to the database
            await _unitOfWork.Categorys.AddAsync(newCategory);
            await _unitOfWork.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = newCategory.CategoryId }, newCategory);
        }

        // Update a Category
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(int id, CategoryDto categoryDto)
        {
            var existingCategory = await _unitOfWork.Categorys.GetByIdAsync(id);
            if (existingCategory == null)
            {
                return NotFound($"Category with ID {id} not found.");
            }

            // Map updated values to the existing category
            _mapper.Map(categoryDto, existingCategory);

            // Update the category in the repository
            _unitOfWork.Categorys.Update(existingCategory);
            await _unitOfWork.SaveChangesAsync();

            return Ok(existingCategory);
        }

        // Delete a Category
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _unitOfWork.Categorys.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound($"Category with ID {id} not found.");
            }

            // Delete the category
            _unitOfWork.Categorys.Delete(category);
            await _unitOfWork.SaveChangesAsync();

            return Ok();
        }
    }
}
