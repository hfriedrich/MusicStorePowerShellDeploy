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
            var cart = NinjectMVC3.GetKernel().Get<ShoppingCart>();
            cart.ShoppingCartId = cart.GetCartId(context);
            return cart;
        }

        public static ShoppingCart GetCart(Controller controller)
        {
            return GetCart(controller.HttpContext);
        }

        public void AddToCart(Album album)
        {
            var cartItem = _cartsPersister.LoadByAlbumAndShoppingCartId(album.Id, ShoppingCartId);

            if (cartItem == null)
            {
                cartItem = new Cart
                {
                    Album = album,
                    CartId = ShoppingCartId,
                    Count = 1,
                    DateCreated = DateTime.Now
                };
            }
            else
            {
                cartItem.Count++;
            }

            _cartsPersister.Store(cartItem);
        }

        public int RemoveFromCart(string id)
        {
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
                }
            }

            return itemCount;
        }

        public void EmptyCart()
        {
            var cartItems = _cartsPersister.LoadByCartId(ShoppingCartId);

            foreach (var cartItem in cartItems)
            {
                _cartsPersister.Delete(cartItem.Id);
            }
        }

        public List<Cart> GetCartItems()
        {
            return _cartsPersister.LoadByCartId(ShoppingCartId).ToList();
        }

        public int GetCount()
        {
            return _cartsPersister.LoadByCartId(ShoppingCartId).Select(item => item.Count).Sum();
        }

        public decimal GetTotal()
        {
            var total = _cartsPersister.LoadByCartId(ShoppingCartId).Select(item => item.Count*item.Album.Price).Sum();
            return total;
        }

        public string CreateOrder(Order order)
        {
            decimal orderTotal = 0;

            var cartItems = GetCartItems();

            foreach (var item in cartItems)
            {
                var orderDetail = new OrderDetail
                {
                    Album = item.Album,
                    Order = order,
                    UnitPrice = item.Album.Price,
                    Quantity = item.Count
                };
                
                orderTotal += (item.Count * item.Album.Price);

                order.OrderDetails.Add(orderDetail);
            }

            order.Total = orderTotal;

            _orderPersister.Store(order);

            EmptyCart();

            return order.Id;
        }

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
                    context.Session[CartSessionKey] = Guid.NewGuid().ToString();
                }
            }

            return context.Session[CartSessionKey].ToString();
        }

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