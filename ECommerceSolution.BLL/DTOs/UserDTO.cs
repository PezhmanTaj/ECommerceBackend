using System;
using ECommerceSolution.DAL.Models;

namespace ECommerceSolution.BLL.DTOs
{
	public class UserDTO
	{
        public String UserId { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public UserRole Role { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}

