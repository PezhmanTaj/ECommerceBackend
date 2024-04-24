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
public class ProductRepositoryTests
{
    private readonly Mock<IMongoDBContext> _mockContext;
    private readonly Mock<IMongoCollection<Product>> _mockCollection;
    private readonly ProductRepository _repository;
    private readonly Mock<IAsyncCursor<Product>> _mockCursor;

    
    public ProductRepositoryTests()
    {
        _mockContext = new Mock<IMongoDBContext>();
        _mockCollection = new Mock<IMongoCollection<Product>>();
        _mockCursor = new Mock<IAsyncCursor<Product>>();
        _mockContext.Setup(c => c.GetCollection<Product>(It.IsAny<string>()))
            .Returns(_mockCollection.Object);

        _repository = new ProductRepository(_mockContext.Object);
    }

    [Test]
    public async Task AddProductAsync_CallsInsertOneAsync()
    {
        // Arrange
        Product testProduct = new Product { Title = "test", Price = 9.99 };
        _mockCollection.Setup(x => x.InsertOneAsync(testProduct, null, default))
            .Returns(Task.CompletedTask)
            .Verifiable(); 

        // Act
        await _repository.AddProductAsync(testProduct);

        // Assert
        _mockCollection.Verify(); 
    }

    [Test]
    public async Task DeleteProductAsync_ValidId_DeletesProduct()
    {
        // Arrange
        var testId = "testId";
        var deleteResultMock = new Mock<DeleteResult>();

        deleteResultMock.SetupGet(r => r.IsAcknowledged).Returns(true);
        deleteResultMock.SetupGet(r => r.DeletedCount).Returns(1);

        _mockCollection.Setup(x =>
            x.DeleteOneAsync(It.IsAny<FilterDefinition<Product>>(), default))
            .ReturnsAsync(deleteResultMock.Object);

        // Act
        var result = await _repository.DeleteProductAsync(testId);

        // Assert
        Assert.True(result);
    }

    [Test]
    public async Task GetAllProductsAsync_ReturnsAllProducts()
    {
        // Arrange
        var expectedProducts = new List<Product> { new Product(), new Product() };
        _mockCursor.SetupSequence(_ => _.MoveNextAsync(default))
                   .ReturnsAsync(true)
                   .ReturnsAsync(false);
        _mockCursor.Setup(_ => _.Current).Returns(expectedProducts);
        _mockCollection.Setup(x => x.FindAsync(It.IsAny<FilterDefinition<Product>>(),
                                               It.IsAny<FindOptions<Product, Product>>(),
                                               default))
                       .ReturnsAsync(_mockCursor.Object);

        // Act
        var products = await _repository.GetAllProductsAsync();

        // Assert
        Assert.That(products.Count(), Is.EqualTo(expectedProducts.Count));
        CollectionAssert.AreEquivalent(expectedProducts, products);
    }

    [Test]
    public async Task GetProductByIdAsync_ReturnsProduct()
    {
        // Arrange
        var expectedProduct = new Product { Id = "testId" };
        _mockCursor.SetupSequence(_ => _.MoveNextAsync(default))
                   .ReturnsAsync(true)
                   .ReturnsAsync(false);
        _mockCursor.Setup(_ => _.Current).Returns(new List<Product> { expectedProduct });
        _mockCollection.Setup(x => x.FindAsync(It.IsAny<FilterDefinition<Product>>(),
                                               It.IsAny<FindOptions<Product, Product>>(),
                                               default))
                       .ReturnsAsync(_mockCursor.Object);

        // Act
        var product = await _repository.GetProductByIdAsync("testId");

        // Assert
        Assert.That(product, Is.EqualTo(expectedProduct));
    }

    [Test]
    public async Task UpdateProductAsync_ProductExists_ReturnsTrue()
    {
        // Arrange
        var testId = "testId";
        var testProduct = new Product { Id = testId };
        var replaceResultMock = new Mock<ReplaceOneResult>();

        // Setting up the ReplaceOneAsync method to return a successful result
        replaceResultMock.SetupGet(r => r.IsAcknowledged).Returns(true);
        replaceResultMock.SetupGet(r => r.ModifiedCount).Returns(1);

        _mockCollection.Setup(x => x.ReplaceOneAsync(It.IsAny<FilterDefinition<Product>>(),
                                                     It.IsAny<Product>(),
                                                     It.IsAny<ReplaceOptions>(),
                                                     default))
                       .ReturnsAsync(replaceResultMock.Object);

        // Act
        var result = await _repository.UpdateProductAsync(testId, testProduct);

        // Assert
        Assert.IsTrue(result);
    }

    [Test]
    public async Task GetProductsBySellerId_ReturnSellerProduct()
    {

        // Arrange
        string sellerId = "sellerId";
        var expectedProducts = new List<Product> { new Product() { OwnerUserId = sellerId }, new Product() { OwnerUserId = sellerId } };
        _mockCursor.SetupSequence(_ => _.MoveNextAsync(default))
                   .ReturnsAsync(true)
                   .ReturnsAsync(false);
        _mockCursor.Setup(_ => _.Current).Returns(expectedProducts);
        _mockCollection.Setup(x => x.FindAsync(It.IsAny<FilterDefinition<Product>>(),
                                               It.IsAny<FindOptions<Product, Product>>(),
                                               default))
                       .ReturnsAsync(_mockCursor.Object);

        // Act
        var products = await _repository.GetAllProductsAsync();

        // Assert
        Assert.IsNotNull(products);
        Assert.That(products.Count(), Is.EqualTo(2));
        Assert.IsTrue(products.All(p => p.OwnerUserId == sellerId));
    }



}






