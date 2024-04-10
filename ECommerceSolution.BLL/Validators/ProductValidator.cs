using System;
using ECommerceSolution.BLL.DTOs;
using FluentValidation;

namespace ECommerceSolution.BLL.Validators
{
	public class ProductValidator : AbstractValidator<ProductDTO>
    {
		public ProductValidator()
		{
			RuleFor(x => x.Title).NotEmpty().NotNull().WithMessage("Title is required");
			RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price should be more than 0");
		}
	}
}

