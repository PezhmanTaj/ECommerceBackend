using System;
using ECommerceSolution.BLL.Interfaces;

namespace ECommerceSolution.BLL.PasswordHashers
{
	public class BCryptPasswordHasher : IPasswordHasher
    { 
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string hashedPassword, string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}

