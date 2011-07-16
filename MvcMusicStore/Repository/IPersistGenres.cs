using MvcMusicStore.Models;

namespace MvcMusicStore.Repository
{
    public interface IPersistGenres : IPersistModel<Genre>
    {
        Genre LoadByName(string genreName);
    }
}