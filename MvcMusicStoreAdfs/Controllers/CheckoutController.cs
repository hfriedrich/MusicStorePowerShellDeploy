﻿using System;
using System.Linq;
using System.Web.Mvc;
using MvcMusicStoreAdfs.Models;
using MvcMusicStoreAdfs.Repository;

namespace MvcMusicStoreAdfs.Controllers
{
//    [Authorize]
    public class CheckoutController : Controller
    {
        private readonly IPersistOrders _orderPersister;

        public CheckoutController(IPersistOrders orderPersister)
        {
            _orderPersister = orderPersister;
        }

        const string PromoCode = "FREE";

        //
        // GET: /Checkout/AddressAndPayment

        public ActionResult AddressAndPayment()
        {
            return View();
        }

        //
        // POST: /Checkout/AddressAndPayment

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

                    //Save Order
//                    storeDB.Orders.Add(order);
//                    storeDB.SaveChanges();
                    _orderPersister.Store(order);

                    //Process the order
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

        //
        // GET: /Checkout/Complete

        public ActionResult Complete(string id)
        {
            // Validate customer owns this order
//            bool isValid = storeDB.Orders.Any(
//                o => o.Id == id &&
//                o.Username == User.Identity.Name);  
            bool isValid = _orderPersister.LoadAll().Any(
                o => o.Id == id &&
                o.Username == User.Identity.Name);

            if (isValid)
            {
                return View(id);
            }
            else
            {
                return View("Error");
            }
        }
    }
}