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
            return _albumReader.LoadAlbumsOrderedByDescendingDetailsNumber(count);
        }
    }
}