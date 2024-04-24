using NUnit.Framework;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ECommerceSolution.BLL.DTOs;
using ECommerceSolution.BLL.Services;
using ECommerceSolution.DAL.Interfaces;
using ECommerceSolution.DAL.Models;

[TestFixture]
public class CategoryServiceTests
{
    private CategoryService _categoryService;
    private Mock<ICategoryRepository> _mockRepo;
    private Mock<IMapper> _mockMapper;

    [SetUp]
    public void Setup()
    {
        _mockRepo = new Mock<ICategoryRepository>();
        _mockMapper = new Mock<IMapper>();
        _categoryService = new CategoryService(_mockRepo.Object, _mockMapper.Object);
    }

    [Test]
    public async Task GetAllCategoriesAsync_ReturnsAllCategories()
    {
        // Arrange
        var categories = new List<Category> { new Category { Id = "1", Name = "Leather Bags" } };
        _mockRepo.Setup(repo => repo.GetAllAsync(false)).ReturnsAsync(categories);
        _mockMapper.Setup(mapper => mapper.Map<IEnumerable<CategoryListDTO>>(It.IsAny<IEnumerable<Category>>()))
                   .Returns(new List<CategoryListDTO> { new CategoryListDTO { CategoryId = "1", Name = "Leather Bags" } });

        // Act
        var result = await _categoryService.GetAllCategoriesAsync(false);

        // Assert
        Assert.IsNotNull(result);
        Assert.That(result, Has.Exactly(1).Items);
        _mockRepo.Verify(repo => repo.GetAllAsync(false), Times.Once);
    }

    [Test]
    public async Task CreateCategoryAsync_CallsRepositoryWithCorrectData()
    {
        // Arrange
        var categoryDTO = new CategoryCreateDTO { Name = "Leather Shoes" };
        var category = new Category { Name = "Leather Shoes" };

        _mockMapper.Setup(m => m.Map<Category>(categoryDTO)).Returns(category);
        _mockRepo.Setup(r => r.CreateAsync(category)).Returns(Task.CompletedTask);

        // Act
        await _categoryService.CreateCategoryAsync(categoryDTO);

        // Assert
        _mockRepo.Verify(r => r.CreateAsync(It.IsAny<Category>()), Times.Once);
        _mockMapper.Verify(m => m.Map<Category>(categoryDTO), Times.Once);
    }

    [Test]
    public async Task GetCategoryByIdAsync_ReturnsCorrectCategory()
    {
        // Arrange
        var categoryId = "1";
        var category = new Category { Id = categoryId, Name = "Leather Bags" };
        _mockRepo.Setup(repo => repo.GetByIdAsync(categoryId)).ReturnsAsync(category);
        _mockMapper.Setup(mapper => mapper.Map<CategoryDetailDTO>(category)).Returns(new CategoryDetailDTO { CategoryId = categoryId, Name = "Leather Bags" });

        // Act
        var result = await _categoryService.GetCategoryByIdAsync(categoryId);

        // Assert
        Assert.IsNotNull(result);
        Assert.That(result.CategoryId, Is.EqualTo(categoryId));
        _mockRepo.Verify(repo => repo.GetByIdAsync(categoryId), Times.Once);
    }

    [Test]
    public async Task UpdateCategoryAsync_UpdatesExistingCategory()
    {
        // Arrange
        var categoryId = "1";
        var updatedCategoryDTO = new CategoryUpdateDTO { Name = "Updated Leather Bags" };
        var existingCategory = new Category { Id = categoryId, Name = "Leather Bags" };
        var updatedCategory = new Category { Id = categoryId, Name = "Updated Leather Bags" };

        _mockMapper.Setup(m => m.Map<Category>(updatedCategoryDTO)).Returns(updatedCategory);
        _mockRepo.Setup(r => r.UpdateAsync(categoryId, updatedCategory)).ReturnsAsync(true);

        // Act
        var result = await _categoryService.UpdateCategoryAsync(categoryId, updatedCategoryDTO);

        // Assert
          Assert.IsTrue(result);
        _mockRepo.Verify(r => r.UpdateAsync(categoryId, updatedCategory), Times.Once);
        _mockMapper.Verify(m => m.Map<Category>(updatedCategoryDTO), Times.Once);
    }

    [Test]
    public async Task DeleteCategoryAsync_DeletesCategory()
    {
        // Arrange
        var categoryId = "1";
        _mockRepo.Setup(r => r.DeleteAsync(categoryId)).ReturnsAsync(true);

        // Act
        var result = await _categoryService.DeleteCategoryAsync(categoryId);

        // Assert
        Assert.IsTrue(result);
        _mockRepo.Verify(r => r.DeleteAsync(categoryId), Times.Once);
    }




}
