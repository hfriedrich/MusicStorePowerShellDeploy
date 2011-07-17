using System.Web.Mvc;
using MvcMusicStoreAdfs.ActionFilters;
using MvcMusicStoreAdfs.Models;
using MvcMusicStoreAdfs.Repository;

namespace MvcMusicStoreAdfs.Controllers
{
//    [Authorize(Roles = "Administrator")]
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

        //
        // GET: /StoreManager/

        public ViewResult Index()
        {
//            var albums = db.Albums.Include(a => a.Genre).Include(a => a.Artist);
            var albums = _albumPersister.LoadAll();
            return View(albums);
        }

        //
        // GET: /StoreManager/Details/5

        public ActionResult Details(string id)
        {
//            Album album = db.Albums.Find(id);
            var album = _albumPersister.Load(id);
            if (album.ValueMissing)
            {
                return RedirectToAction("Index");
            }
            return View(album.Value);
        }

        //
        // GET: /StoreManager/Create

        public ActionResult Create()
        {
//            ViewBag.GenreId = new SelectList(db.Genres, "Id", "Name");
//            ViewBag.ArtistId = new SelectList(db.Artists, "Id", "Name");
            ViewBag.Genres = new SelectList(_genreReader.LoadAll(), "Id", "Name");
            ViewBag.Artists = new SelectList(_artistReader.LoadAll(), "Id", "Name");
            return View();
        } 

        //
        // POST: /StoreManager/Create

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
        
        //
        // GET: /StoreManager/Edit/5
 
        public ActionResult Edit(string id)
        {
//            Album album = db.Albums.Find(id);
            var album = _albumPersister.Load(id);
            if (album.ValueMissing)
            {
                RedirectToAction("Index");
            }
            ViewBag.Genres = new SelectList(_genreReader.LoadAll(), "Id", "Name", album.Value.Genre.Id);
            ViewBag.Artists = new SelectList(_artistReader.LoadAll(), "Id", "Name", album.Value.Artist.Id);
            return View(album.Value);
        }

        //
        // POST: /StoreManager/Edit/5

        [HttpPost]
        public ActionResult Edit(Album album, string genres, string artists)
        {
            var artist = _artistReader.Load(artists);
            var genre = _genreReader.Load(genres);
            if (artist.HasValue && genre.HasValue)
            {
//                db.Entry(album).State = EntityState.Modified;
//                db.SaveChanges();
                album.Artist = artist.Value;
                album.Genre = genre.Value;
                _albumPersister.Store(album);
                return RedirectToAction("Index");
            }
            ViewBag.Genres = new SelectList(_genreReader.LoadAll(), "Id", "Name");
            ViewBag.Artists = new SelectList(_artistReader.LoadAll(), "Id", "Name");
            return View(album);
        }

        //
        // GET: /StoreManager/Delete/5
 
        public ActionResult Delete(string id)
        {
//            Album album = db.Albums.Find(id);
            var album = _albumPersister.Load(id);
            if (album.ValueMissing)
            {
                RedirectToAction("Index");
            }
            return View(album.Value);
        }

        //
        // POST: /StoreManager/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {            
//            Album album = db.Albums.Find(id);
//            db.Albums.Remove(album);
//            db.SaveChanges();
            _albumPersister.Delete(id);
            return RedirectToAction("Index");
        }
    }
}