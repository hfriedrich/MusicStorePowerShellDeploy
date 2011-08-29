using MvcMusicStore.Models;
using Raven.Client;

namespace MvcMusicStore.Repository
{
    public class OrderRepository : RavenPersister<Order>, IPersistOrders
    {
        public OrderRepository(IDocumentSession session) : base(session)
        {
        }
    }
}