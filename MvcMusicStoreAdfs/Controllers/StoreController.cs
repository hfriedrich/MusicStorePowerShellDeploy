using System.Linq;
using System.Web.Mvc;
using MvcMusicStoreAdfs.Repository;

namespace MvcMusicStoreAdfs.Controllers
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


        //
        // GET: /Store/

        public ActionResult Index()
        {
            var genres = _genreReader.LoadAll();

            return View(genres);
        }

        //
        // GET: /Store/Browse?genre=Disco

        public ActionResult Browse(string genre)
        {
            // Retrieve Genre and its Associated Albums from database
            var loadedGenre = _genreReader.LoadByName(genre);
            loadedGenre.Albums =_albumPersister.LoadAlbumsForGenre(loadedGenre.Id).ToList();

//            var genreModel = storeDB.Genres.Include("Albums")
//                .Single(g => g.Name == genre);

            return View(loadedGenre);
        }

        //
        // GET: /Store/Details/5

        public ActionResult Details(string id)
        {
//            var album = storeDB.Albums.Find(id);
            var album = _albumPersister.Load(id);
            if (album.ValueMissing)
            {
                return RedirectToAction("Index");
            }
            return View(album.Value);
        }

        //
        // GET: /Store/GenreMenu

        [ChildActionOnly]
        public ActionResult GenreMenu()
        {
//            var genres = storeDB.Genres.ToList();
            var genres = _genreReader.LoadAll();

            return PartialView(genres);
        }

    }
}