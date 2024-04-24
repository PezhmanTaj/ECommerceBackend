using System;
using ECommerceSolution.DAL.Models;
using System.Security.Claims;

namespace ECommerceSolution.BLL.Interfaces
{
	public interface ITokenService
	{
        TokenDTO GenerateToken(User user);
        bool ValidateToken(string token);
        ClaimsPrincipal DecodeToken(string token);
    }
}

