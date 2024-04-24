using System;
namespace ECommerceSolution.BLL.DTOs
{
	public class CategoryDTO
	{
        public string CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ParentCategoryId { get; set; }
        public List<CategoryDTO> Subcategories { get; set; }
        public string ImagePath { get; set; }
        public string SEOKeywords { get; set; }
        public bool IsActive { get; set; }
    }
}

