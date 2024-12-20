﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ArticlesController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var articles =  _unitOfWork.Articles
                .Include(e => e.Menu)
                .Include(e => e.Category)
                .GetAll().ToList();
            return Ok(articles); 
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var article = await _unitOfWork.Articles.GetByIdAsync(id);
            if (article == null)
            {
                return NotFound();  
            }
            return Ok(article); 
        }

        [HttpGet("GetByName")]
        public IActionResult GetByName(string name)
        {
            var article = _unitOfWork.Articles.Find(x => x.Name.Equals(name));
            if (article == null)
            {
                return NotFound("Article not found with the provided name."); 
            }
            return Ok(article);  
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] ArticleDto artDto)
        {
            var existingArticle = _unitOfWork.Articles.Find(x => x.Name.Equals(artDto.Name));
            if (existingArticle != null)
            {
                return Conflict(new { Message = "An article with this name already exists." });
            }

            var newArticle = _mapper.Map<Article>(artDto);
            await _unitOfWork.Articles.AddAsync(newArticle);
            await _unitOfWork.SaveChangesAsync();
            return CreatedAtAction(nameof(Create), new { id = newArticle.Id }, newArticle);
        }


        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(int id, ArticleDto articleDto)
        {
            var existingArticle = await _unitOfWork.Articles.GetByIdAsync(id);
            if (existingArticle == null)
            {
                return NotFound($"Article with ID {id} not found.");
            }
            _mapper.Map(articleDto, existingArticle);
            _unitOfWork.Articles.Update(existingArticle);
            await _unitOfWork.SaveChangesAsync();

            return Ok(existingArticle);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var article = await _unitOfWork.Articles.GetByIdAsync(id);
            if (article == null)
            {
                return NotFound("Article not found.");  
            }

            _unitOfWork.Articles.Delete(article);
            await _unitOfWork.SaveChangesAsync();  

            return NoContent();  
        }
    }
}
