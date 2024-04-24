using System.Collections.Generic;
using System.Threading.Tasks;
using ECommerceSolution.BLL.DTOs;

public interface ICategoryService
{
    Task<IEnumerable<CategoryListDTO>> GetAllCategoriesAsync(bool includeInactive = false);
    Task<CategoryDetailDTO> GetCategoryByIdAsync(string id);
    Task CreateCategoryAsync(CategoryCreateDTO categoryDTO);
    Task<bool> UpdateCategoryAsync(string id, CategoryUpdateDTO categoryDTO);
    Task<bool> DeleteCategoryAsync(string id);
}
