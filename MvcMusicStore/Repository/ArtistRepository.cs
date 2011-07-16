using MvcMusicStore.Models;
using Raven.Client;

namespace MvcMusicStore.Repository
{
    public class ArtistRepository : RavenPersister<Artist>, IPersistArtists
    {
        public ArtistRepository(IDocumentSession session) : base(session)
        {
        }
    }
}