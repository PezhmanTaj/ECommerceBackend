using System;
using ECommerceSolution.BLL.DTOs;

namespace ECommerceSolution.BLL.Interfaces
{
    public interface IUserService
    {
        Task RegisterUserAsync(UserRegistrationDTO registrationDto);
        Task<TokenDTO> AuthenticateAsync(UserLoginDTO loginDto);
        Task<bool> UpdateUserAsync(UserUpdateDTO updateDto);
        Task<bool> DeleteUserAsync(string userId);
        Task<UserDTO> GetUserByIdAsync(string userId);
        Task<UserDTO> GetUserByUsernameAsync(string username);
        Task<bool> ChangePasswordAsync(string userId, string oldPassword, string newPassword);
    }

}

