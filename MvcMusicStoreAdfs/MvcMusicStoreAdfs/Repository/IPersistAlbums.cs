using System.Collections.Generic;
using MvcMusicStoreAdfs.Models;

namespace MvcMusicStoreAdfs.Repository
{
    public interface IPersistAlbums : IPersistModel<Album>
    {
        IEnumerable<Album> LoadAlbumsOrderedByDescendingDetailsNumber(int count);
        IEnumerable<Album> LoadAlbumsForGenre(string genreId);
    }
}