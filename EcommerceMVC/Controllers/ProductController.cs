using EcommerceMVC.Repositories.Implementations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
[Authorize]
public class ProductController : Controller
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;

    public ProductController(IProductRepository productRepository, ICategoryRepository categoryRepository)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<IActionResult> Index()
    {
        var products = await _productRepository.GetAllProductsAsync();
        var categories = await _categoryRepository.GetAllCategoriesAsync();
        ViewBag.Categories = categories;
        return View(products);
    }

    public async Task<IActionResult> Details(int id)
    {
        var product = await _productRepository.GetProductByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        return View(product);
    }

    public async Task<IActionResult> Search(string searchTerm, int? categoryId)
    {
        var products = await _productRepository.SearchProductsAsync(searchTerm, categoryId);
        var categories = await _categoryRepository.GetAllCategoriesAsync();
        ViewBag.Categories = categories;
        return View("Index", products);
    }
}
