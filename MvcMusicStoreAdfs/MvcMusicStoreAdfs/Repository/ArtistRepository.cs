using MvcMusicStoreAdfs.Models;
using Raven.Client;

namespace MvcMusicStoreAdfs.Repository
{
    public class ArtistRepository : RavenPersister<Artist>, IPersistArtists
    {
        public ArtistRepository(IDocumentSession session) : base(session)
        {
        }
    }
}