using NUnit.Framework;
using System.Security;
using Moq;
using AutoMapper;
using FluentValidation;
using ECommerceSolution.BLL.Services;
using ECommerceSolution.DAL.Interfaces;
using ECommerceSolution.BLL.DTOs;
using ECommerceSolution.DAL.Models;
using FluentValidation.Results;
using ECommerceSolution.DAL.Repositories;
using ECommerceSolution.BLL.Interfaces;

[TestFixture]
public class ProductServiceTests
{
    private Mock<IProductRepository> _mockProductRepository;
    private Mock<IMapper> _mockMapper;
    private Mock<IValidator<ProductDTO>> _mockValidator;
    private ProductService _productService;
    private Mock<IUserContext> _mockUserContext;


    [SetUp]
    public void SetUp()
    {
        _mockProductRepository = new Mock<IProductRepository>();
        _mockMapper = new Mock<IMapper>();
        _mockValidator = new Mock<IValidator<ProductDTO>>();
        _mockUserContext = new Mock<IUserContext>();
        _productService = new ProductService(_mockProductRepository.Object, _mockMapper.Object, _mockValidator.Object, _mockUserContext.Object);
    }

    [Test]
    public async Task CreateProductAsync_ValidInput_CallsRepositoryAddOnce()
    {
        // Arrange
        var productDTO = new ProductDTO { Title = "test", Price = 8.80};
        _mockValidator.Setup(v => v.ValidateAsync(It.IsAny<ProductDTO>(), default))
                        .ReturnsAsync(new ValidationResult());

        _mockMapper.Setup(m => m.Map<Product>(It.IsAny<ProductDTO>()))
                    .Returns(new Product());

        // Act
        await _productService.CreateProductAsync(productDTO);

        // Assert
        _mockProductRepository.Verify(x => x.AddProductAsync(It.IsAny<Product>()), Times.Once);
    }

    [Test]
    public async Task DeleteProductAsync_AccessGranted_ProductDeleted()
    {
        // Arrange
        string productId = "123";
        string ownerId = "seller123";
        _mockUserContext.Setup(x => x.CurrentUserId).Returns(ownerId);
        _mockUserContext.Setup(x => x.CurrentUserRole).Returns("Seller");
        _mockProductRepository.Setup(x => x.GetProductByIdAsync(productId))
                                .ReturnsAsync(new Product { OwnerUserId = ownerId });
        _mockProductRepository.Setup(x => x.DeleteProductAsync(productId))
                                .ReturnsAsync(true);

        // Act
        var result = await _productService.DeleteProductAsync(productId);

        // Assert
        Assert.IsTrue(result);
        _mockProductRepository.Verify(x => x.DeleteProductAsync(productId), Times.Once());
    }

    [Test]
    public void DeleteProductAsync_AccessDenied_ThrowsUnauthorizedAccessException()
    {
        // Arrange
        string productId = "123";
        string ownerId = "seller123";
        string currentUserId = "seller456";
        _mockUserContext.Setup(x => x.CurrentUserId).Returns(currentUserId);
        _mockUserContext.Setup(x => x.CurrentUserRole).Returns("Seller");
        _mockProductRepository.Setup(x => x.GetProductByIdAsync(productId))
                                .ReturnsAsync(new Product { OwnerUserId = ownerId });

        // Act & Assert
        var ex = Assert.ThrowsAsync<UnauthorizedAccessException>(() => _productService.DeleteProductAsync(productId));
        Assert.That(ex.Message, Is.EqualTo("Access denied."));
        _mockProductRepository.Verify(x => x.DeleteProductAsync(It.IsAny<string>()), Times.Never());
    }


    [Test]
    public async Task GetAllProductsAsync_ReturnsAllProducts()
    {
        //Arrange
        var expectedProducts = new List<Product> { new Product(), new Product() };
        _mockProductRepository.Setup(repo => repo.GetAllProductsAsync()).ReturnsAsync(expectedProducts);

        //Act
        var result = await _productService.GetAllProductsAsync();

        //Assert
        Assert.That(result.Count, Is.EqualTo(expectedProducts.Count));
    }

    [Test]
    public async Task GetProductByIdAsync_ReturnsMappedProductDTO()
    {
        // Arrange
        string testId = "testId";
        Product expectedProduct = new () { Id = testId };
        ProductDTO expectedProductDTO = new() { Id = testId };

        _mockProductRepository.Setup(repo => repo.GetProductByIdAsync(testId))
                                .ReturnsAsync(expectedProduct);

        _mockMapper.Setup(m => m.Map<ProductDTO>(It.IsAny<Product>()))
                .Returns(expectedProductDTO);

        // Act
        var resultDTO = await _productService.GetProductByIdAsync(testId);

        // Assert
        Assert.That(resultDTO, Is.Not.Null);
        Assert.That(resultDTO.Id, Is.EqualTo(expectedProductDTO.Id));

    }

    [Test]
    public async Task UpdateProductAsync_ValidConditions_ProductUpdated()
    {
        // Arrange
        string productId = "123";
        var productDTO = new ProductDTO { Title = "New Title" };
        var product = new Product { OwnerUserId = "seller123" };
        var validationResult = new ValidationResult();

        _mockUserContext.Setup(x => x.CurrentUserId).Returns("seller123");
        _mockProductRepository.Setup(x => x.GetProductByIdAsync(productId)).ReturnsAsync(product);
        _mockValidator.Setup(v => v.ValidateAsync(productDTO, default)).ReturnsAsync(validationResult);
        _mockMapper.Setup(m => m.Map<Product>(productDTO)).Returns(new Product());
        _mockProductRepository.Setup(x => x.UpdateProductAsync(productId, It.IsAny<Product>())).ReturnsAsync(true);

        // Act
        var result = await _productService.UpdateProductAsync(productId, productDTO);

        // Assert
        Assert.IsTrue(result);
        _mockProductRepository.Verify(x => x.UpdateProductAsync(productId, It.IsAny<Product>()), Times.Once());
    }


    [Test]
    public void UpdateProductAsync_AccessDenied_ThrowsUnauthorizedAccessException()
    {
        // Arrange
        string productId = "123";
        var productDTO = new ProductDTO();
        var product = new Product { OwnerUserId = "otherUserId" }; 

        _mockUserContext.Setup(x => x.CurrentUserId).Returns("seller123");
        _mockUserContext.Setup(x => x.CurrentUserRole).Returns("Seller");
        _mockProductRepository.Setup(x => x.GetProductByIdAsync(productId)).ReturnsAsync(product);

        // Act & Assert
        var ex = Assert.ThrowsAsync<UnauthorizedAccessException>(() => _productService.UpdateProductAsync(productId, productDTO));
        Assert.That(ex.Message, Is.EqualTo("Access denied."));
        _mockProductRepository.Verify(x => x.UpdateProductAsync(It.IsAny<string>(), It.IsAny<Product>()), Times.Never());
    }

    [Test]
    public void CheckSellerAccess_SameSellerId_NoExceptionThrown()
    {
        // Arrange
        var sellerId = "seller123";
        _mockUserContext.Setup(x => x.CurrentUserId).Returns(sellerId);
        _mockUserContext.Setup(x => x.CurrentUserRole).Returns("Seller");

        // Act & Assert
        Assert.DoesNotThrow(() => _productService.CheckSellerAccess(sellerId));
    }

    [Test]
    public void CheckSellerAccess_DifferentSellerId_ThrowsUnauthorizedAccessException()
    {
        // Arrange
        var sellerId = "seller123";
        _mockUserContext.Setup(x => x.CurrentUserId).Returns("anotherSellerId");
        _mockUserContext.Setup(x => x.CurrentUserRole).Returns("Seller");

        // Act & Assert
        Assert.Throws<UnauthorizedAccessException>(() => _productService.CheckSellerAccess(sellerId),
            "Access denied.");
    }

    [Test]
    public void CheckSellerAccess_NonSellerRole_NoExceptionThrown()
    {
        // Arrange
        var sellerId = "seller123";
        _mockUserContext.Setup(x => x.CurrentUserId).Returns(sellerId);
        _mockUserContext.Setup(x => x.CurrentUserRole).Returns("Admin");

        // Act & Assert
        Assert.DoesNotThrow(() => _productService.CheckSellerAccess(sellerId));
    }

}

