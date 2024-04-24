using NUnit.Framework;
using Moq;
using AutoMapper;
using FluentValidation;
using ECommerceSolution.BLL.Services;
using ECommerceSolution.DAL.Interfaces;
using ECommerceSolution.BLL.DTOs;
using ECommerceSolution.DAL.Models;
using FluentValidation.Results;
using MongoDB.Driver;
using ECommerceSolution.DAL.Repositories;

[TestFixture]
public class CategoryRepositoryTests
{
    private readonly Mock<IMongoDBContext> _mockContext;
    private readonly Mock<IMongoCollection<Category>> _mockCollection;
    private readonly CategoryRepository _repository;
    private readonly Mock<IAsyncCursor<Category>> _mockCursor;


    public CategoryRepositoryTests()
    {
        _mockContext = new Mock<IMongoDBContext>();
        _mockCollection = new Mock<IMongoCollection<Category>>();
        _mockCursor = new Mock<IAsyncCursor<Category>>();
        _mockContext.Setup(c => c.GetCollection<Category>(It.IsAny<string>()))
            .Returns(_mockCollection.Object);

        _repository = new CategoryRepository(_mockContext.Object);
    }

    [Test]
    public async Task AddCategoryAsync_CallsCreateAsync()
    {
        // Arrange
        Category testCategory = new Category { Name = "test", SEOKeywords = "Seo"};
        _mockCollection.Setup(x => x.InsertOneAsync(testCategory, null, default))
            .Returns(Task.CompletedTask)
            .Verifiable();

        // Act
        await _repository.CreateAsync(testCategory);

        // Assert
        _mockCollection.Verify();
    }

    [Test]
    public async Task DeleteCategoryAsync_ValidId_DeletesCategory()
    {
        // Arrange
        var testId = "testId";
        var deleteResultMock = new Mock<DeleteResult>();

        deleteResultMock.SetupGet(r => r.IsAcknowledged).Returns(true);
        deleteResultMock.SetupGet(r => r.DeletedCount).Returns(1);

        _mockCollection.Setup(x =>
            x.DeleteOneAsync(It.IsAny<FilterDefinition<Category>>(), default))
            .ReturnsAsync(deleteResultMock.Object);

        // Act
        var result = await _repository.DeleteAsync(testId);

        // Assert
        Assert.True(result);
    }

    [Test]
    public async Task GetAllAsync_ReturnsAllCategories()
    {
        // Arrange
        var expectedCategories = new List<Category> { new Category(), new Category() };
        _mockCursor.SetupSequence(_ => _.MoveNextAsync(default))
                   .ReturnsAsync(true)
                   .ReturnsAsync(false);
        _mockCursor.Setup(_ => _.Current).Returns(expectedCategories);
        _mockCollection.Setup(x => x.FindAsync(It.IsAny<FilterDefinition<Category>>(),
                                               It.IsAny<FindOptions<Category, Category>>(),
                                               default))
                       .ReturnsAsync(_mockCursor.Object);

        // Act
        var categories = await _repository.GetAllAsync();

        // Assert
        Assert.That(categories.Count(), Is.EqualTo(expectedCategories.Count));
        CollectionAssert.AreEquivalent(expectedCategories, categories);
    }

    [Test]
    public async Task GetProductByIdAsync_ReturnsProduct()
    {
        // Arrange
        var expectedCategory = new Category { Id = "testId" };
        _mockCursor.SetupSequence(_ => _.MoveNextAsync(default))
                   .ReturnsAsync(true)
                   .ReturnsAsync(false);
        _mockCursor.Setup(_ => _.Current).Returns(new List<Category> { expectedCategory });
        _mockCollection.Setup(x => x.FindAsync(It.IsAny<FilterDefinition<Category>>(),
                                               It.IsAny<FindOptions<Category, Category>>(),
                                               default))
                       .ReturnsAsync(_mockCursor.Object);

        // Act
        var category = await _repository.GetByIdAsync("testId");

        // Assert
        Assert.That(category, Is.EqualTo(expectedCategory));
    }

    [Test]
    public async Task UpdateCategoryAsync_CategoryExists_ReturnsTrue()
    {
        // Arrange
        var testId = "testId";
        var testCategory = new Category { Id = testId };
        var replaceResultMock = new Mock<ReplaceOneResult>();
        replaceResultMock.SetupGet(r => r.IsAcknowledged).Returns(true);
        replaceResultMock.SetupGet(r => r.ModifiedCount).Returns(1);

        _mockCollection.Setup(x => x.ReplaceOneAsync(It.IsAny<FilterDefinition<Category>>(),
                                                     It.IsAny<Category>(),
                                                     It.IsAny<ReplaceOptions>(),
                                                     default))
                       .ReturnsAsync(replaceResultMock.Object);

        // Act
        var result = await _repository.UpdateAsync(testId, testCategory);

        // Assert
        Assert.IsTrue(result);
    }


}






