using System;
namespace ECommerceSolution.BLL.DTOs
{
	public class CategoryCreateDTO
	{
        public string Name { get; set; }
        public string Description { get; set; }
        public string ParentCategoryId { get; set; }
        public string ImagePath { get; set; }
        public string SEOKeywords { get; set; }
    }
}

