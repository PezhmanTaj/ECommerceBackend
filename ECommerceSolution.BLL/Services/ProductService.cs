using System;
using AutoMapper;
using ECommerceSolution.BLL.DTOs;
using ECommerceSolution.BLL.Interfaces;
using ECommerceSolution.DAL.Interfaces;
using ECommerceSolution.DAL.Models;
using FluentValidation;
using FluentValidation.Results;

namespace ECommerceSolution.BLL.Services
{
	public class ProductService : IProductService
	{
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<ProductDTO> _validator;

		public ProductService(IProductRepository productRepository, IMapper mapper, IValidator<ProductDTO> validator)
		{
            _productRepository = productRepository;
            _mapper = mapper;
            _validator = validator;
		}

        public async Task CreateProductAsync(ProductDTO productDTO)
        {
            ValidationResult validationResult = await _validator.ValidateAsync(productDTO);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            var product = _mapper.Map<Product>(productDTO);

            await _productRepository.AddProductAsync(product);
        }

        public async Task<bool> DeleteProductAsync(string id)
        {
           return await _productRepository.DeleteProductAsync(id);
           
        }

        public async Task<IEnumerable<ProductDTO>> GetAllProductsAsync()
        {
            IEnumerable<Product> products = await _productRepository.GetAllProductsAsync();
            var productDTOs = products.Select(item => _mapper.Map<ProductDTO>(item)).ToList();
            return productDTOs;
        }

        public async Task<ProductDTO> GetProductByIdAsync(string id)
        {
            Product product = await _productRepository.GetProductByIdAsync(id);
            return _mapper.Map<ProductDTO>(product);
        }

        public async Task<bool> UpdateProductAsync(string id, ProductDTO productDTO)
        {
            ValidationResult validationResult = await _validator.ValidateAsync(productDTO);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            return await _productRepository.UpdateProductAsync(id, _mapper.Map<Product>(productDTO));
        }
    }
}

