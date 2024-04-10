using System;
using ECommerceSolution.BLL.DTOs;
using ECommerceSolution.DAL.Models;

namespace ECommerceSolution.BLL.Interfaces
{
	public interface IProductService
	{
        Task<IEnumerable<ProductDTO>> GetAllProductsAsync();
        Task<ProductDTO> GetProductByIdAsync(string id);
        Task CreateProductAsync(ProductDTO product);
        Task<bool> UpdateProductAsync(string id, ProductDTO product);
        Task<bool> DeleteProductAsync(string id);
    }

}

