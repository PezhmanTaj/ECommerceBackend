using System;
using System.Security.Claims;
using ECommerceSolution.BLL.Interfaces;
using Microsoft.AspNetCore.Http;

namespace ECommerceSolution.BLL.Services
{
	public class UserContextService : IUserContext
	{
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContextService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string CurrentUserId => _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        public string CurrentUserRole => _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Role)?.Value;
    }
}

