using System;
using System.Linq;
using System.Web.Mvc;
using MvcMusicStoreAdfs.Models;
using MvcMusicStoreAdfs.Repository;

namespace MvcMusicStoreAdfs.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly IPersistOrders _orderPersister;

        public CheckoutController(IPersistOrders orderPersister)
        {
            _orderPersister = orderPersister;
        }

        const string PromoCode = "FREE";

        public ActionResult AddressAndPayment()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddressAndPayment(FormCollection values)
        {
            var order = new Order();
            TryUpdateModel(order);

            try
            {
                if (string.Equals(values["PromoCode"], PromoCode,
                    StringComparison.OrdinalIgnoreCase) == false)
                {
                    return View(order);
                }
                else
                {
                    order.Username = User.Identity.Name;
                    order.OrderDate = DateTime.Now;
                    _orderPersister.Store(order);

                    var cart = ShoppingCart.GetCart(this.HttpContext);
                    cart.CreateOrder(order);

                    return RedirectToAction("Complete",
                        new { id = order.Id });
                }

            }
            catch
            {
                //Invalid - redisplay with errors
                return View(order);
            }
        }

        public ActionResult Complete(string id)
        {
            var isValid = _orderPersister.LoadAll().Any(
                o => o.Id == id &&
                o.Username == User.Identity.Name);

            return View(isValid ? id : "Error");
        }
    }
}
