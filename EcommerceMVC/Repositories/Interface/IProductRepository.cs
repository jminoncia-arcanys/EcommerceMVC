using EcommerceMVC.Models;

namespace EcommerceMVC.Repositories.Implementations
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product> GetProductByIdAsync(int productId);
        Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm, int? categoryId);
    }
}
