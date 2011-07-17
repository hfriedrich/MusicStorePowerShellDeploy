using MvcMusicStoreAdfs.Repository;
using Ninject.Modules;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Extensions;

namespace MvcMusicStoreAdfs
{
    public class RepositoryModule : NinjectModule
    {
        private readonly DocumentStore _documentStore;

        public RepositoryModule()
        {
            _documentStore = new DocumentStore {Url = Konfiguration.RavenDB};
            _documentStore.Initialize();
        }

        public override void Load()
        {
            Bind<IDocumentStore>().To<DocumentStore>();
            Bind<IDocumentSession>().ToMethod(ctx => OpenSession()).InRequestScope().OnDeactivation(session => session.Dispose());
            Bind<IPersistAlbums>().To<AlbumRepository>();
            Bind<IPersistCarts>().To<CartRepository>();
            Bind<IPersistOrders>().To<OrderRepository>();
            Bind<IPersistGenres>().To<GenreRepository>();
            Bind<IPersistArtists>().To<ArtistRepository>();

            Bind(typeof(IPersistModel<>)).To(typeof(RavenPersister<>));
        }

        private IDocumentSession OpenSession()
        {
            _documentStore.DatabaseCommands.EnsureDatabaseExists("MvcMusicStore");
            var documentSession = _documentStore.OpenSession("MvcMusicStore");
            return documentSession;
        }

    }
}