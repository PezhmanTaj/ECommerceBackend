using NUnit.Framework;
using Moq;
using AutoMapper;
using FluentValidation;
using ECommerceSolution.BLL.Services;
using ECommerceSolution.DAL.Interfaces;
using ECommerceSolution.BLL.DTOs;
using ECommerceSolution.DAL.Models;
using FluentValidation.Results;
using ECommerceSolution.DAL.Repositories;

namespace ECommerceSolution.Tests
{
    [TestFixture]
    public class ProductServiceTests
    {
        private Mock<IProductRepository> _mockProductRepository;
        private Mock<IMapper> _mockMapper;
        private Mock<IValidator<ProductDTO>> _mockValidator;
        private ProductService _productService;

        [SetUp]
        public void SetUp()
        {
            _mockProductRepository = new Mock<IProductRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockValidator = new Mock<IValidator<ProductDTO>>();
            _productService = new ProductService(_mockProductRepository.Object, _mockMapper.Object, _mockValidator.Object);
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
        public async Task DeleteProductAsync_ValidId_ReturnsTrue()
        {
            // Arrange
            var testId = "validId";
            _mockProductRepository.Setup(repo => repo.DeleteProductAsync(testId))
                                  .ReturnsAsync(true);

            // Act
            var result = await _productService.DeleteProductAsync(testId);

            // Assert
            Assert.IsTrue(result);
            _mockProductRepository.Verify(repo => repo.DeleteProductAsync(testId), Times.Once());
        }

        [Test]
        public async Task DeleteProductAsync_ValidId_ReturnsFalse()
        {
            // Arrange
            var testId = "InvalidId";
            _mockProductRepository.Setup(repo => repo.DeleteProductAsync(testId))
                                  .ReturnsAsync(false);

            // Act
            var result = await _productService.DeleteProductAsync(testId);

            // Assert
            Assert.IsFalse(result);
            _mockProductRepository.Verify(repo => repo.DeleteProductAsync(testId), Times.Once());
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
        //public async Task<bool> UpdateProductAsync(string id, ProductDTO product)
        //{
        //    return await _productRepository.UpdateProductAsync(id, _mapper.Map<Product>(product));
        //}

        [Test]
        public async Task UpdateProductAsync_ReturnsTrue()
        {
            //Arrange
            string testId = "testId";
            Product expectedProduct = new() { };
            ProductDTO expectedProductDTO = new() {};
            _mockProductRepository.Setup(repo => repo.UpdateProductAsync(testId, It.IsAny<Product>())).ReturnsAsync(true);
            _mockMapper.Setup(m => m.Map<Product>(It.IsAny<ProductDTO>()))
                  .Returns(expectedProduct);
            //Act
            var result = await _productService.UpdateProductAsync(testId, expectedProductDTO);

            //Assert
            Assert.IsTrue(result);
        }

    }
}
