using EcommerceMVC.Helpers;
using EcommerceMVC.Models;
using EcommerceMVC.Repositories.Implementations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
public class CartController : Controller
{
    private readonly IProductRepository _productRepository;
    private readonly ShoppingCart _shoppingCart;

    public CartController(IProductRepository productRepository, ShoppingCart shoppingCart)
    {
        _productRepository = productRepository;
        _shoppingCart = shoppingCart;
    }

    public IActionResult Index()
    {
        var cartItems = _shoppingCart.GetCartItems();
        var totalCost = _shoppingCart.GetTotalCost();
        ViewBag.TotalCost = totalCost;
        return View(cartItems);
    }

    public async Task<IActionResult> AddToCart(int productId, int quantity)
    {
        var product = await _productRepository.GetProductByIdAsync(productId);
        if (product != null)
        {
            var cartItem = new CartItem
            {
                ProductId = product.Id,
                ProductName = product.Name,
                Price = product.Price,
                Quantity = quantity
            };
            _shoppingCart.AddToCart(cartItem);
        }
        return RedirectToAction("Index");
    }

    public IActionResult RemoveFromCart(int productId)
    {
        _shoppingCart.RemoveFromCart(productId);
        return RedirectToAction("Index");
    }

    public IActionResult UpdateQuantity(int productId, int quantity)
    {
        _shoppingCart.UpdateQuantity(productId, quantity);
        return RedirectToAction("Index");
    }

    public IActionResult ClearCart()
    {
        _shoppingCart.ClearCart();
        return RedirectToAction("Index");
    }
}
