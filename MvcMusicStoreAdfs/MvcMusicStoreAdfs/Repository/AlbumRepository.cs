using System.Collections.Generic;
using System.Linq;
using MvcMusicStoreAdfs.Models;
using Raven.Client;

namespace MvcMusicStoreAdfs.Repository
{
    public class AlbumRepository : RavenPersister<Album>, IPersistAlbums
    {

        public AlbumRepository(IDocumentSession session) : base(session)
        {
        }

        public IEnumerable<Album> LoadAlbumsOrderedByDescendingDetailsNumber(int count)
        {
            return LoadAllAsIEnumerable().OrderByDescending(album => album.OrderDetails.Count()).Take(count);
        }

        public IEnumerable<Album> LoadAlbumsForGenre(string genreId)
        {
            return LoadAllAsIEnumerable().Where(album => album.Genre.Id == genreId);
        }
    }
}