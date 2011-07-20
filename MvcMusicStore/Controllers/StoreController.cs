using System.Linq;
using System.Web.Mvc;
using MvcMusicStore.Repository;

namespace MvcMusicStore.Controllers
{
    public class StoreController : Controller
    {
        private readonly IPersistGenres _genreReader;
        private readonly IPersistAlbums _albumPersister;

        public StoreController(IPersistGenres genreReader, IPersistAlbums albumPersister)
        {
            _genreReader = genreReader;
            _albumPersister = albumPersister;
        }

        public ActionResult Index()
        {
            var genres = _genreReader.LoadAll();

            return View(genres);
        }

        public ActionResult Browse(string genre)
        {
            var loadedGenre = _genreReader.LoadByName(genre);
            loadedGenre.Albums =_albumPersister.LoadAlbumsForGenre(loadedGenre.Id).ToList();

            return View(loadedGenre);
        }

        public ActionResult Details(string id)
        {
            var album = _albumPersister.Load(id);
            if (album.ValueMissing)
            {
                return RedirectToAction("Index");
            }
            return View(album.Value);
        }

        [ChildActionOnly]
        public ActionResult GenreMenu()
        {
            var genres = _genreReader.LoadAll();

            return PartialView(genres);
        }

    }
}