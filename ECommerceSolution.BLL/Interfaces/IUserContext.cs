using System;
namespace ECommerceSolution.BLL.Interfaces
{
	public interface IUserContext
	{
        string CurrentUserId { get; }
        string CurrentUserRole { get; }
    }
}

