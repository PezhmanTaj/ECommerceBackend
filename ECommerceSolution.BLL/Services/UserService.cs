using System;
using System.Security.Authentication;
using AutoMapper;
using ECommerceSolution.BLL.DTOs;
using ECommerceSolution.BLL.Interfaces;
using ECommerceSolution.BLL.Validators;
using ECommerceSolution.DAL.Interfaces;
using ECommerceSolution.DAL.Models;
using ECommerceSolution.DAL.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace ECommerceSolution.BLL.Services
{
	public class UserService : IUserService
	{
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<UserRegistrationDTO> _validator;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenService _tokenService;
        public UserService(IUserRepository userRepository, IMapper mapper, IValidator<UserRegistrationDTO> validator, IPasswordHasher passwordHasher, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _validator = validator;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
        }

        public async Task<TokenDTO> AuthenticateAsync(UserLoginDTO loginDto)
        {
            User user = await _userRepository.GetByUsernameAsync(loginDto.Username);
            if (user == null || !_passwordHasher.VerifyPassword(user.PasswordHash, loginDto.Password))
            {
                throw new AuthenticationException("Username or password is incorrect.");
            }

            var token = _tokenService.GenerateToken(user);

            return new TokenDTO
            {
                AccessToken = token.AccessToken,
                RefreshToken = token.RefreshToken,
                ExpiresIn = token.ExpiresIn,
                TokenType = "Bearer"
            };
        }

        public async Task<bool> ChangePasswordAsync(string userId, string oldPassword, string newPassword)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new InvalidOperationException("User not found.");
            }

            var verificationResult = _passwordHasher.VerifyPassword(user.PasswordHash, oldPassword);
            if (verificationResult != true)
            {
                throw new ArgumentException("The old password is incorrect.");
            }

            var hashedNewPassword = _passwordHasher.HashPassword(newPassword);
            user.PasswordHash = hashedNewPassword;

            await _userRepository.UpdateAsync(user.UserId,user);

            return true;
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            return await _userRepository.DeleteAsync(userId);
        }

        public async Task<UserDTO> GetUserByIdAsync(string userId)
        {
            return _mapper.Map<UserDTO>(await _userRepository.GetByIdAsync(userId));
        }

        public async Task<UserDTO> GetUserByUsernameAsync(string username)
        {
            return _mapper.Map<UserDTO>(await _userRepository.GetByUsernameAsync(username));
        }

        public async Task RegisterUserAsync(UserRegistrationDTO registrationDto)
        {
            var validationResult = _validator.Validate(registrationDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            try
            {
                var user = _mapper.Map<User>(registrationDto);
                user.PasswordHash = _passwordHasher.HashPassword(registrationDto.Password);
                await _userRepository.AddAsync(user);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> UpdateUserAsync(UserUpdateDTO updateDto)
        {
            User user = _mapper.Map<User>(updateDto);
            try
            {
                User userForId = await _userRepository.GetByUsernameAsync(user.Username);
                string userId = userForId.UserId;
                return await _userRepository.UpdateAsync(userId, user);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("User not found!");
            }
            
        }
    }
}

