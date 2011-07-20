using System.Web.Mvc;
using MvcMusicStore.Models;
using MvcMusicStore.Repository;
using MvcMusicStore.ViewModels;

namespace MvcMusicStore.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IPersistAlbums _albumReader;
        private readonly IPersistCarts _cartReader;

        public ShoppingCartController(IPersistAlbums albumReader, IPersistCarts cartReader)
        {
            _albumReader = albumReader;
            _cartReader = cartReader;
        }

        public ActionResult Index()
        {
            var cart = ShoppingCart.GetCart(HttpContext);

            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal()
            };

            return View(viewModel);
        }

        public ActionResult AddToCart(string id)
        {

            var addedAlbum = _albumReader.Load(id);
            if (addedAlbum.ValueMissing)
            {
                return RedirectToAction("Index");
            }

            var cart = ShoppingCart.GetCart(HttpContext);

            cart.AddToCart(addedAlbum.Value);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult RemoveFromCart(string id)
        {
            var cart = ShoppingCart.GetCart(HttpContext);
            var loadedCart = _cartReader.Load(id);
            if(loadedCart.ValueMissing)
            {
                return RedirectToAction("Index");
            }

            var albumName = loadedCart.Value.Album.Title;

            var itemCount = cart.RemoveFromCart(id);

            var results = new ShoppingCartRemoveViewModel
            {
                Message = Server.HtmlEncode(albumName) +
                    " has been removed from your shopping cart.",
                CartTotal = cart.GetTotal(),
                CartCount = cart.GetCount(),
                ItemCount = itemCount,
                DeleteId = id
            };

            return Json(results);
        }

        [ChildActionOnly]
        public ActionResult CartSummary()
        {
            var cart = ShoppingCart.GetCart(HttpContext);

            ViewData["CartCount"] = cart.GetCount();

            return PartialView("CartSummary");
        }
    }
}