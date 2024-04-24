using System;
using ECommerceSolution.BLL.DTOs;
using FluentValidation;

namespace ECommerceSolution.BLL.Validators
{
	public class UserValidator : AbstractValidator<UserRegistrationDTO>
    {
		public UserValidator()
		{
            RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required")
            .Length(3, 20).WithMessage("Username must be between 3 and 20 characters");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("A valid email is required");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long")
                .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter")
                .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter")
                .Matches("[0-9]").WithMessage("Password must contain at least one number")
                .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character");
        }
    }
	
}

