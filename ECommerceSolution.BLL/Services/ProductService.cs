using System;
using AutoMapper;
using ECommerceSolution.BLL.DTOs;
using ECommerceSolution.BLL.Interfaces;
using ECommerceSolution.DAL.Interfaces;
using ECommerceSolution.DAL.Models;
using FluentValidation;
using FluentValidation.Results;
using MongoDB.Driver.Core.Servers;

namespace ECommerceSolution.BLL.Services
{
	public class ProductService : IProductService
	{
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<ProductDTO> _validator;
        private readonly IUserContext _userContext;

        public ProductService(IProductRepository productRepository, IMapper mapper, IValidator<ProductDTO> validator, IUserContext userContext)
		{
            _productRepository = productRepository;
            _mapper = mapper;
            _validator = validator;
            _userContext = userContext;
        }

        public async Task CreateProductAsync(ProductDTO productDTO)
        {
            ValidationResult validationResult = await _validator.ValidateAsync(productDTO);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            var product = _mapper.Map<Product>(productDTO);
            product.OwnerUserId = _userContext.CurrentUserId;
            await _productRepository.AddProductAsync(product);
        }

        public async Task<bool> DeleteProductAsync(string id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            CheckSellerAccess(product.OwnerUserId);
            return await _productRepository.DeleteProductAsync(id);
           
        }

        public async Task<IEnumerable<ProductDTO>> GetAllProductsAsync()
        {
            IEnumerable<Product> products;
            if (_userContext.CurrentUserRole == "Seller")
            {
                products = await _productRepository.GetProductsBySellerId(_userContext.CurrentUserId);
            }
            else
            {
                products = await _productRepository.GetAllProductsAsync();
            }

            var productDTOs = products.Select(product => _mapper.Map<ProductDTO>(product)).ToList();

            return productDTOs;
        }


        public async Task<ProductDTO> GetProductByIdAsync(string id)
        {
            Product product = await _productRepository.GetProductByIdAsync(id);
            CheckSellerAccess(product.OwnerUserId);
            return _mapper.Map<ProductDTO>(product);
        }

        public async Task<bool> UpdateProductAsync(string id, ProductDTO productDTO)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            if (product == null)
            {
                throw new KeyNotFoundException("Product not found."); // Or another appropriate exception
            }

            CheckSellerAccess(product.OwnerUserId);

            ValidationResult validationResult = await _validator.ValidateAsync(productDTO);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            return await _productRepository.UpdateProductAsync(id, _mapper.Map<Product>(productDTO));
        }


        public void CheckSellerAccess(string sellerId)
        {
            if (_userContext.CurrentUserRole == "Seller" && _userContext.CurrentUserId != sellerId)
            {
                throw new UnauthorizedAccessException("Access denied.");
            }
        }
    }
}

