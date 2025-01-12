using EcommerceMVC.Models;
using EcommerceMVC.Repositories.Implementations;
using EcommerceMVC;
using Microsoft.EntityFrameworkCore;

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _context;

    public ProductRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        return await _context.Products
                             .Include(p => p.ProductCategories)
                             .ThenInclude(pc => pc.Category)
                             .ToListAsync();
    }

    public async Task<Product> GetProductByIdAsync(int productId)
    {
        return await _context.Products
                             .Include(p => p.ProductCategories)
                             .ThenInclude(pc => pc.Category)
                             .FirstOrDefaultAsync(p => p.Id == productId);
    }

    public async Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm, int? categoryId)
    {
        var query = _context.Products
                            .Include(p => p.ProductCategories)
                            .ThenInclude(pc => pc.Category)
                            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(p => p.Name.Contains(searchTerm) || p.Description.Contains(searchTerm));
        }

        if (categoryId.HasValue)
        {
            query = query.Where(p => p.ProductCategories.Any(pc => pc.CategoryId == categoryId));
        }

        return await query.ToListAsync();
    }
}
