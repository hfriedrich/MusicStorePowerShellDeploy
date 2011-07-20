using System.Collections.Generic;
using System.Web.Mvc;
using MvcMusicStoreAdfs.Models;
using MvcMusicStoreAdfs.Repository;

namespace MvcMusicStoreAdfs.Controllers
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
            return _albumReader.LoadAlbumsOrderedByDescendingDetailsNumber(count);
        }
    }
}