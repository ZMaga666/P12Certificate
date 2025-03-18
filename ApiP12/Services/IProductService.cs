using ApiP12.Models;

namespace ApiP12.Services
{
    public interface IProductService
    {
        Task<List<Product>> GetAllasync();
        Task<Product> GetProductByIdAsync(int id);
        Task CreateProductAsync(Product product);
        Task DeleteProductAsync(int id);
        Task UpdateProductAsync(Product product);


    
    }
}
