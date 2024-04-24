using ECommerceSolution.DAL.Models;

namespace ECommerceSolution.BLL.DTOs
{
    public class UserRegistrationDTO
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }
    }
}