using System.Linq;
using MvcMusicStore.Models;
using Raven.Client;

namespace MvcMusicStore.Repository
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