using Microsoft.AspNetCore.Mvc;
using MonRestoAPI.Models;
using MonRestoAPI.Repositories;

namespace MonRestoAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArticlesController : ControllerBase
    {
        private readonly IRepository<Article> _articleRepository;

        public ArticlesController(IRepository<Article> articleRepository)
        {
            _articleRepository = articleRepository;
        }



        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(_articleRepository.GetAll());
        }
        [HttpGet("GetById")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            return Ok(await _articleRepository.GetByIdAsync(id));
        }
        [HttpGet("GetByName")]
        public IActionResult GetByName(string name)
        {
            var article = _articleRepository.Find(x => x.Name.Equals(name));
            return Ok(article);
        }
    }
}
