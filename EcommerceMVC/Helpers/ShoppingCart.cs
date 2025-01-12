using EcommerceMVC.Models;

namespace EcommerceMVC.Helpers
{
    public class ShoppingCart
    {
        private readonly ISession _session;

        public ShoppingCart(IHttpContextAccessor httpContextAccessor)
        {
            _session = httpContextAccessor.HttpContext.Session;
        }

        private string CartSessionKey => "CartItems";

        public List<CartItem> GetCartItems()
        {
            var cartItems = _session.GetObjectFromJson<List<CartItem>>(CartSessionKey);
            return cartItems ?? new List<CartItem>();
        }

        public void AddToCart(CartItem cartItem)
        {
            var cartItems = GetCartItems();
            var existingItem = cartItems.FirstOrDefault(item => item.ProductId == cartItem.ProductId);

            if (existingItem != null)
            {
                existingItem.Quantity += cartItem.Quantity;
            }
            else
            {
                cartItems.Add(cartItem);
            }

            _session.SetObjectAsJson(CartSessionKey, cartItems);
        }

        public void RemoveFromCart(int productId)
        {
            var cartItems = GetCartItems();
            var itemToRemove = cartItems.FirstOrDefault(item => item.ProductId == productId);

            if (itemToRemove != null)
            {
                cartItems.Remove(itemToRemove);
                _session.SetObjectAsJson(CartSessionKey, cartItems);
            }
        }

        public void UpdateQuantity(int productId, int quantity)
        {
            var cartItems = GetCartItems();
            var itemToUpdate = cartItems.FirstOrDefault(item => item.ProductId == productId);

            if (itemToUpdate != null)
            {
                itemToUpdate.Quantity = quantity;
                _session.SetObjectAsJson(CartSessionKey, cartItems);
            }
        }

        public decimal GetTotalCost()
        {
            var cartItems = GetCartItems();
            return cartItems.Sum(item => item.TotalPrice);
        }

        public void ClearCart()
        {
            _session.Remove(CartSessionKey);
        }
    }

}
