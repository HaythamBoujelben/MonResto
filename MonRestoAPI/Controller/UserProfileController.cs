using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MonResto.API.Dto;
using MonRestoAPI.Models;
using MonRestoAPI.Repositories;

namespace MonResto.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserProfileController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserProfileController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var userProfiles =  _unitOfWork.UserProfiles.GetAll().ToList();
            return Ok(userProfiles);
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var userProfile = await _unitOfWork.UserProfiles.GetByIdAsync(id);
            if (userProfile == null)
            {
                return NotFound();
            }
            return Ok(userProfile);
        }

        [HttpGet("GetByEmail")]
        public async Task<IActionResult> GetByEmailAsync(string email)
        {
            var userProfile = await _unitOfWork.UserProfiles.FindAsync(x => x.Email == email);
            if (userProfile == null)
            {
                return NotFound($"User Profile with Email {email} not found.");
            }
            return Ok(userProfile);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(UserProfileDto userProfileDto)
        {
            var newUserProfile = _mapper.Map<UserProfile>(userProfileDto);

            await _unitOfWork.UserProfiles.AddAsync(newUserProfile);
            await _unitOfWork.SaveChangesAsync();

            return Ok(newUserProfile);
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(int id, UserProfileDto userProfileDto)
        {
            var existingUserProfile = await _unitOfWork.UserProfiles.GetByIdAsync(id);
            if (existingUserProfile == null)
            {
                return NotFound($"User Profile with ID {id} not found.");
            }

            _mapper.Map(userProfileDto, existingUserProfile);
            _unitOfWork.UserProfiles.Update(existingUserProfile);
            await _unitOfWork.SaveChangesAsync();

            return Ok(existingUserProfile);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userProfile = await _unitOfWork.UserProfiles.GetByIdAsync(id);
            if (userProfile == null)
            {
                return NotFound($"User Profile with ID {id} not found.");
            }

            _unitOfWork.UserProfiles.Delete(userProfile);
            await _unitOfWork.SaveChangesAsync();

            return Ok();
        }
    }
}
