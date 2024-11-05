using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MonResto.API.Dto;
using MonRestoAPI.Models;
using MonRestoAPI.Repositories;

namespace MonRestoAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArticlesController : ControllerBase
    {
        private readonly IUnitOfWork _UnitOfWork;
        private  IMapper _mapper;

        public ArticlesController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _UnitOfWork = unitOfWork;
            _mapper = mapper;
        }



        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(_UnitOfWork.Articles.GetAll());
        }
        [HttpGet("GetById")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            return Ok(await _UnitOfWork.Articles.GetByIdAsync(id));
        }
        [HttpGet("GetByName")]
        public IActionResult GetByName(string name)
        {
            var article = _UnitOfWork.Articles.Find(x => x.Name.Equals(name));
            return Ok(article);
        }
        [HttpPost("Create")]
        public async Task<IActionResult> Create(CreateArticleDto artDto)
        {
            var existingArticle = _UnitOfWork.Articles.Find(x => x.Name == artDto.Name);
            if (existingArticle != null)
            {
                return Conflict("An article with this name already exists.");
            }

            var newArticle = _mapper.Map<Article>(artDto);

            await _UnitOfWork.Articles.AddAsync(newArticle);
            await _UnitOfWork.SaveChangesAsync();
            return CreatedAtAction(nameof(Create), new { id = newArticle.ArticleId }, newArticle);
        }
        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var article = await _UnitOfWork.Articles.GetByIdAsync(id);
            _UnitOfWork.Articles.Delete(article);
            await _UnitOfWork.SaveChangesAsync();
            return Ok();
        }

    }
}
