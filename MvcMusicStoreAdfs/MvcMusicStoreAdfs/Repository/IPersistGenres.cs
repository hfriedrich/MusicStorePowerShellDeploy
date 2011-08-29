using MvcMusicStoreAdfs.Models;

namespace MvcMusicStoreAdfs.Repository
{
    public interface IPersistGenres : IPersistModel<Genre>
    {
        Genre LoadByName(string genreName);
    }
}