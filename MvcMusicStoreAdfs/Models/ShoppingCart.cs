using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcMusicStoreAdfs.App_Start;
using MvcMusicStoreAdfs.Repository;
using Ninject;

namespace MvcMusicStoreAdfs.Models
{
    public class ShoppingCart
    {
        private readonly IPersistCarts _cartsPersister;
        private readonly IPersistOrders _orderPersister;

        public ShoppingCart(IPersistCarts cartsPersister, IPersistOrders orderPersister)
        {
            _cartsPersister = cartsPersister;
            _orderPersister = orderPersister;
        }

        string ShoppingCartId { get; set; }

        public const string CartSessionKey = "CartId";

        public static ShoppingCart GetCart(HttpContextBase context)
        {
//            var cart = new ShoppingCart();
            var cart = NinjectMVC3.GetKernel().Get<ShoppingCart>();
            cart.ShoppingCartId = cart.GetCartId(context);
            return cart;
        }

        // Helper method to simplify shopping cart calls
        public static ShoppingCart GetCart(Controller controller)
        {
            return GetCart(controller.HttpContext);
        }

        public void AddToCart(Album album)
        {
            // Get the matching cart and album instances
//            var cartItem = storeDB.Carts.SingleOrDefault(
            //c => c.CartId == ShoppingCartId
            //&& c.AlbumId == album.Id); 
            var cartItem = _cartsPersister.LoadByAlbumAndShoppingCartId(album.Id, ShoppingCartId);

            if (cartItem == null)
            {
                // Create a new cart item if no cart item exists
                cartItem = new Cart
                {
                    Album = album,
                    CartId = ShoppingCartId,
                    Count = 1,
                    DateCreated = DateTime.Now
                };

//                storeDB.Carts.Add(cartItem);
            }
            else
            {
                // If the item does exist in the cart, then add one to the quantity
                cartItem.Count++;
            }

            // Save changes
//            storeDB.SaveChanges();
            _cartsPersister.Store(cartItem);
        }

        public int RemoveFromCart(string id)
        {
            // Get the cart
//            var cartItem = storeDB.Carts.Single(
//cart => cart.CartId == ShoppingCartId
//&& cart.Id == id);
            var cartItem = _cartsPersister.LoadById(id, ShoppingCartId);

            var itemCount = 0;

            if (cartItem != null)
            {
                if (cartItem.Count > 1)
                {
                    cartItem.Count--;
                    itemCount = cartItem.Count;
                    _cartsPersister.Store(cartItem);
                }
                else
                {
                    _cartsPersister.Delete(cartItem.Id);
//                    storeDB.Carts.Remove(cartItem);
                }

                // Save changes
//                storeDB.SaveChanges();
            }

            return itemCount;
        }

        public void EmptyCart()
        {
//            var cartItems = storeDB.Carts.Where(cart => cart.CartId == ShoppingCartId);
            var cartItems = _cartsPersister.LoadByCartId(ShoppingCartId);

            foreach (var cartItem in cartItems)
            {
//                storeDB.Carts.Remove(cartItem);
                _cartsPersister.Delete(cartItem.Id);
            }

            // Save changes
//            storeDB.SaveChanges();
        }

        public List<Cart> GetCartItems()
        {
//            return storeDB.Carts.Where(cart => cart.CartId == ShoppingCartId).ToList();
            return _cartsPersister.LoadByCartId(ShoppingCartId).ToList();
        }

        public int GetCount()
        {
            // Get the count of each item in the cart and sum them up
//            int? count = (from cartItems in storeDB.Carts
//                          where cartItems.CartId == ShoppingCartId
//                          select (int?)cartItems.Count).Sum();


            var count = _cartsPersister.LoadByCartId(ShoppingCartId).Select(item => item.Count).Sum();
            // Return 0 if all entries are null
            return count;
        }

        public decimal GetTotal()
        {
            // Multiply album price by count of that album to get 
            // the current price for each of those albums in the cart
            // sum all album price totals to get the cart total
//            decimal? total = (from cartItems in storeDB.Carts
//                              where cartItems.CartId == ShoppingCartId
//                              select (int?)cartItems.Count * cartItems.Album.Price).Sum();

            var total = _cartsPersister.LoadByCartId(ShoppingCartId).Select(item => item.Count*item.Album.Price).Sum();
            return total;
        }

        public string CreateOrder(Order order)
        {
            decimal orderTotal = 0;

            var cartItems = GetCartItems();

            // Iterate over the items in the cart, adding the order details for each
            foreach (var item in cartItems)
            {
                var orderDetail = new OrderDetail
                {
                    Album = item.Album,
                    Order = order,
                    UnitPrice = item.Album.Price,
                    Quantity = item.Count
                };
                
                // Set the order total of the shopping cart
                orderTotal += (item.Count * item.Album.Price);

//                storeDB.OrderDetails.Add(orderDetail);
                order.OrderDetails.Add(orderDetail);
            }

            // Set the order's total to the orderTotal count
            order.Total = orderTotal;

            // Save the order
//            storeDB.SaveChanges();
            _orderPersister.Store(order);

            // Empty the shopping cart
            EmptyCart();

            // Return the Id as the confirmation number
            return order.Id;
        }

        // We're using HttpContextBase to allow access to cookies.
        public string GetCartId(HttpContextBase context)
        {
            if (context.Session[CartSessionKey] == null)
            {
                if (!string.IsNullOrWhiteSpace(context.User.Identity.Name))
                {
                    context.Session[CartSessionKey] = context.User.Identity.Name;
                }
                else
                {
                    // Send tempCartId back to client as a cookie
                    context.Session[CartSessionKey] = Guid.NewGuid().ToString();
                }
            }

            return context.Session[CartSessionKey].ToString();
        }

        // When a user has logged in, migrate their shopping cart to
        // be associated with their username
        public void MigrateCart(string userName)
        {
            var shoppingCart = _cartsPersister.LoadByCartId(ShoppingCartId);

            foreach (var cart in shoppingCart)
            {
                cart.CartId = userName;
                _cartsPersister.Store(cart);
            }
        }
    }
}