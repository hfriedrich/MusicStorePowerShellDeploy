using MvcMusicStoreAdfs.Models;
using Raven.Client;

namespace MvcMusicStoreAdfs.Repository
{
    public class OrderRepository : RavenPersister<Order>, IPersistOrders
    {
        public OrderRepository(IDocumentSession session) : base(session)
        {
        }
    }
}