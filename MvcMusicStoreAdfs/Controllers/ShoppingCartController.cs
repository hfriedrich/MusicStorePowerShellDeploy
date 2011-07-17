using System.Web.Mvc;
using MvcMusicStoreAdfs.Models;
using MvcMusicStoreAdfs.Repository;
using MvcMusicStoreAdfs.ViewModels;

namespace MvcMusicStoreAdfs.Controllers
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

        //
        // GET: /ShoppingCart/

        public ActionResult Index()
        {
            var cart = ShoppingCart.GetCart(HttpContext);

            // Set up our ViewModel
            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal()
            };

            // Return the view
            return View(viewModel);
        }

        //
        // GET: /Store/AddToCart/5

        public ActionResult AddToCart(string id)
        {

            // Retrieve the album from the database
//            var addedAlbum = storeDB.Albums
//                .Single(album => album.Id == id);     
            var addedAlbum = _albumReader.Load(id);
            if (addedAlbum.ValueMissing)
            {
                return RedirectToAction("Index");
            }

            // Add it to the shopping cart
            var cart = ShoppingCart.GetCart(HttpContext);

            cart.AddToCart(addedAlbum.Value);

            // Go back to the main store page for more shopping
            return RedirectToAction("Index");
        }

        //
        // AJAX: /ShoppingCart/RemoveFromCart/5

        [HttpPost]
        public ActionResult RemoveFromCart(string id)
        {
            // Remove the item from the cart
            var cart = ShoppingCart.GetCart(HttpContext);

            // Get the name of the album to display confirmation
//            string albumName = storeDB.Carts
//                .Single(item => item.Id == id).Album.Title;         
            var loadedCart = _cartReader.Load(id);
            if(loadedCart.ValueMissing)
            {
                return RedirectToAction("Index");
            }

            var albumName = loadedCart.Value.Album.Title;

            // Remove from cart
            var itemCount = cart.RemoveFromCart(id);

            // Display the confirmation message
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

        //
        // GET: /ShoppingCart/CartSummary

        [ChildActionOnly]
        public ActionResult CartSummary()
        {
            var cart = ShoppingCart.GetCart(HttpContext);

            ViewData["CartCount"] = cart.GetCount();

            return PartialView("CartSummary");
        }
    }
}