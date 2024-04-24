using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ECommerceSolution.BLL.DTOs;
using ECommerceSolution.BLL.Services;
using System.ComponentModel.DataAnnotations;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCategories([FromQuery] bool includeInactive = false)
    {
        var categories = await _categoryService.GetAllCategoriesAsync(includeInactive);
        return Ok(categories);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCategoryById(string id)
    {
        var category = await _categoryService.GetCategoryByIdAsync(id);
        if (category == null)
            return NotFound();
        return Ok(category);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory([FromBody] CategoryCreateDTO categoryDTO)
    {
        await _categoryService.CreateCategoryAsync(categoryDTO);
        return CreatedAtAction(nameof(GetCategoryById), new { id = categoryDTO.Name }, categoryDTO);  
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCategory(string id, [FromBody] CategoryUpdateDTO categoryDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var result = await _categoryService.UpdateCategoryAsync(id, categoryDTO);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { errors = ex.ValidationResult.ErrorMessage });
        }
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(string id)
    {
        var result = await _categoryService.DeleteCategoryAsync(id);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }

}
