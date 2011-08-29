using System.Collections.Generic;
using MvcMusicStore.Models;

namespace MvcMusicStore.Repository
{
    public interface IPersistAlbums : IPersistModel<Album>
    {
        IEnumerable<Album> LoadAlbumsOrderedByDescendingDetailsNumber(int count);
        IEnumerable<Album> LoadAlbumsForGenre(string genreId);
    }
}