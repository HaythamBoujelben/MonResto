﻿using AutoMapper;
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

        // Get All User Profiles
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var userProfiles = await _unitOfWork.UserProfiles.GetAllAsync();
            var userProfileDtos = _mapper.Map<List<UserProfileDto>>(userProfiles);
            return Ok(userProfileDtos);
        }

        // Get User Profile by ID
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var userProfile = await _unitOfWork.UserProfiles.GetByIdAsync(id);
            if (userProfile == null)
            {
                return NotFound($"User Profile with ID {id} not found.");
            }

            var userProfileDto = _mapper.Map<UserProfileDto>(userProfile);
            return Ok(userProfileDto);
        }

        // Get User Profile by Email
        [HttpGet("GetByEmail")]
        public async Task<IActionResult> GetByEmailAsync(string email)
        {
            var userProfile = await _unitOfWork.UserProfiles.FindAsync(x => x.Email == email);
            if (userProfile == null)
            {
                return NotFound($"User Profile with Email {email} not found.");
            }

            var userProfileDto = _mapper.Map<UserProfileDto>(userProfile);
            return Ok(userProfileDto);
        }

        // Create User Profile
        [HttpPost("Create")]
        public async Task<IActionResult> Create(UserProfileDto userProfileDto)
        {
            // Check if a user with the same email already exists
            var existingUser = await _unitOfWork.UserProfiles.FindAsync(x => x.Email == userProfileDto.Email);
            if (existingUser != null)
            {
                return Conflict("A user profile with this email already exists.");
            }

            // Map the DTO to the UserProfile entity
            var newUserProfile = _mapper.Map<UserProfile>(userProfileDto);

            // Add to the repository and save changes
            await _unitOfWork.UserProfiles.AddAsync(newUserProfile);
            await _unitOfWork.SaveChangesAsync();

            return Ok(newUserProfile);
        }

        // Update User Profile
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(int id, UserProfileDto userProfileDto)
        {
            var existingUserProfile = await _unitOfWork.UserProfiles.GetByIdAsync(id);
            if (existingUserProfile == null)
            {
                return NotFound($"User Profile with ID {id} not found.");
            }

            // Update fields (or map all fields, depending on your scenario)
            _mapper.Map(userProfileDto, existingUserProfile);

            // Save changes
            _unitOfWork.UserProfiles.Update(existingUserProfile);
            await _unitOfWork.SaveChangesAsync();

            return Ok(existingUserProfile);
        }

        // Delete User Profile
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userProfile = await _unitOfWork.UserProfiles.GetByIdAsync(id);
            if (userProfile == null)
            {
                return NotFound($"User Profile with ID {id} not found.");
            }

            // Delete the user profile
            _unitOfWork.UserProfiles.Delete(userProfile);
            await _unitOfWork.SaveChangesAsync();

            return Ok();
        }
    }
}
