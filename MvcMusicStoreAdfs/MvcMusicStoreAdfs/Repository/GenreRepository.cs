using System.Linq;
using MvcMusicStoreAdfs.Models;
using Raven.Client;

namespace MvcMusicStoreAdfs.Repository
{
    class GenreRepository : RavenPersister<Genre>, IPersistGenres
    {
        public GenreRepository(IDocumentSession session) : base(session)
        {
        }

        public Genre LoadByName(string genreName)
        {
            return LoadAllAsIEnumerable().Where(genre => genre.Name == genreName).FirstOrDefault();
        }
    }
}