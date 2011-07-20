using System.Web.Mvc;
using MvcMusicStoreAdfs.ActionFilters;
using MvcMusicStoreAdfs.Models;
using MvcMusicStoreAdfs.Repository;

namespace MvcMusicStoreAdfs.Controllers
{
    [RequiresAdminUser]
    public class StoreManagerController : Controller
    {
        private readonly IPersistAlbums _albumPersister;
        private readonly IPersistGenres _genreReader;
        private readonly IPersistArtists _artistReader;

        public StoreManagerController(IPersistAlbums albumPersister, IPersistGenres genreReader, IPersistArtists artistReader)
        {
            _albumPersister = albumPersister;
            _genreReader = genreReader;
            _artistReader = artistReader;
        }

        public ViewResult Index()
        {
            var albums = _albumPersister.LoadAll();
            return View(albums);
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

        public ActionResult Create()
        {
            ViewBag.Genres = new SelectList(_genreReader.LoadAll(), "Id", "Name");
            ViewBag.Artists = new SelectList(_artistReader.LoadAll(), "Id", "Name");
            return View();
        } 


        [HttpPost]
        public ActionResult Create(Album album, string genres, string artists)
        {
            var artist = _artistReader.Load(artists);
            var genre = _genreReader.Load(genres);
            if (artist.HasValue && genre.HasValue)
            {
                album.Artist = artist.Value;
                album.Genre = genre.Value;
                _albumPersister.Store(album);
                return RedirectToAction("Index");
            }

            ViewBag.Genres = new SelectList(_genreReader.LoadAll(), "Id", "Name");
            ViewBag.Artists = new SelectList(_artistReader.LoadAll(), "Id", "Name");
            return View(album);
        }
 
        public ActionResult Edit(string id)
        {
            var album = _albumPersister.Load(id);
            if (album.ValueMissing)
            {
                RedirectToAction("Index");
            }
            ViewBag.Genres = new SelectList(_genreReader.LoadAll(), "Id", "Name", album.Value.Genre.Id);
            ViewBag.Artists = new SelectList(_artistReader.LoadAll(), "Id", "Name", album.Value.Artist.Id);
            return View(album.Value);
        }

        [HttpPost]
        public ActionResult Edit(Album album, string genres, string artists)
        {
            var artist = _artistReader.Load(artists);
            var genre = _genreReader.Load(genres);
            if (artist.HasValue && genre.HasValue)
            {
                album.Artist = artist.Value;
                album.Genre = genre.Value;
                _albumPersister.Store(album);
                return RedirectToAction("Index");
            }
            ViewBag.Genres = new SelectList(_genreReader.LoadAll(), "Id", "Name");
            ViewBag.Artists = new SelectList(_artistReader.LoadAll(), "Id", "Name");
            return View(album);
        }

        public ActionResult Delete(string id)
        {
            var album = _albumPersister.Load(id);
            if (album.ValueMissing)
            {
                RedirectToAction("Index");
            }
            return View(album.Value);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {            
            _albumPersister.Delete(id);
            return RedirectToAction("Index");
        }
    }
}