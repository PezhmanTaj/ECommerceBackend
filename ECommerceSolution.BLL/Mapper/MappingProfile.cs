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
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<UserRegistrationDTO, User>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());
            CreateMap<UserUpdateDTO, User>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => src.ModifiedDate));

            CreateMap<Category, CategoryListDTO>()
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));

            CreateMap<Category, CategoryDetailDTO>()
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.ParentCategoryId, opt => opt.MapFrom(src => src.ParentCategoryId))
                .ForMember(dest => dest.Subcategories, opt => opt.MapFrom(src => src.Subcategories))
                .ForMember(dest => dest.ImagePath, opt => opt.MapFrom(src => src.ImagePath))
                .ForMember(dest => dest.SEOKeywords, opt => opt.MapFrom(src => src.SEOKeywords))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));

            CreateMap<CategoryCreateDTO, Category>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.ParentCategoryId, opt => opt.MapFrom(src => src.ParentCategoryId))
                .ForMember(dest => dest.ImagePath, opt => opt.MapFrom(src => src.ImagePath))
                .ForMember(dest => dest.SEOKeywords, opt => opt.MapFrom(src => src.SEOKeywords));

            CreateMap<CategoryUpdateDTO, Category>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.ParentCategoryId, opt => opt.MapFrom(src => src.ParentCategoryId))
                .ForMember(dest => dest.ImagePath, opt => opt.MapFrom(src => src.ImagePath))
                .ForMember(dest => dest.SEOKeywords, opt => opt.MapFrom(src => src.SEOKeywords))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive)).ReverseMap();

        }
    }
}

