using System;
using AutoMapper;
using ECommerceSolution.BLL.DTOs;
using ECommerceSolution.DAL.Models;

namespace ECommerceSolution.BLL.Mapper
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
            CreateMap<Product, ProductDTO>().ReverseMap();
        }
	}
}

