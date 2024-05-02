using System.Collections.Generic;
using System.Threading.Tasks;
using ECommerceSolution.BLL.DTOs;
using ECommerceSolution.DAL.Repositories;
using AutoMapper;
using ECommerceSolution.DAL.Interfaces;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CategoryListDTO>> GetAllCategoriesAsync(bool includeInactive = false)
    {
        var categories = await _categoryRepository.GetAllAsync(includeInactive);
        return _mapper.Map<IEnumerable<CategoryListDTO>>(categories);
    }

    public async Task<CategoryDetailDTO> GetCategoryByIdAsync(string id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        return _mapper.Map<CategoryDetailDTO>(category);
    }

    public async Task CreateCategoryAsync(CategoryCreateDTO categoryDTO)
    {
        var category = _mapper.Map<Category>(categoryDTO);
        await _categoryRepository.CreateAsync(category);
    }

    public async Task<bool> UpdateCategoryAsync(string id, CategoryUpdateDTO categoryDTO)
    {
        var category = _mapper.Map<Category>(categoryDTO);
        category.Id = id;  // Ensure the ID is set correctly
        return await _categoryRepository.UpdateAsync(id, category);
    }

    public async Task<bool> DeleteCategoryAsync(string id)
    {
        return await _categoryRepository.DeleteAsync(id);
    }

}
