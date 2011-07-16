using System.Collections.Generic;
using System.Web.Mvc;
using MvcMusicStore.Models;
using MvcMusicStore.Repository;

namespace MvcMusicStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPersistAlbums _albumReader;

        public HomeController(IPersistAlbums albumReader)
        {
            _albumReader = albumReader;
        }


        public ActionResult Index()
        {
            var albums = GetTopSellingAlbums(5);

            return View(albums);
        }

        private IEnumerable<Album> GetTopSellingAlbums(int count)
        {
            // Group the order details by album and return
            // the albums with the highest count

//            return storeDB.Albums
//                .OrderByDescending(a => a.OrderDetails.Count())
//                .Take(count)
//                .ToList();
            return _albumReader.LoadAlbumsOrderedByDescendingDetailsNumber(count);
        }
    }
}