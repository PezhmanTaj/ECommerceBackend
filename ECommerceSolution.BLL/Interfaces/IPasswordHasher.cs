using System;
namespace ECommerceSolution.BLL.Interfaces
{
	public interface IPasswordHasher
	{
        string HashPassword(string password);
        bool VerifyPassword(string hashedPassword, string password);
    }
}

