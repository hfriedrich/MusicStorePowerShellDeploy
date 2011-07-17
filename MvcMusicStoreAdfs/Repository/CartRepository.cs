using System.Collections.Generic;
using System.Linq;
using MvcMusicStoreAdfs.Models;
using Raven.Client;

namespace MvcMusicStoreAdfs.Repository
{
    class CartRepository : RavenPersister<Cart>, IPersistCarts
    {
        public CartRepository(IDocumentSession session) : base(session)
        {
        }

        public Cart LoadByAlbumAndShoppingCartId(string albumId, string shoppingCartId)
        {
            return LoadAllAsIEnumerable().Where(cart => cart.Album.Id == albumId && cart.CartId == shoppingCartId).
                SingleOrDefault();
        }

        public Cart LoadById(string id, string shoppingCartId)
        {
            return LoadAllAsIEnumerable().Where(cart => cart.Id == id && cart.CartId == shoppingCartId).
                Single();
        }

        public IEnumerable<Cart> LoadByCartId(string shoppingCartId)
        {
            return LoadAllAsIEnumerable().Where(cart => cart.CartId == shoppingCartId);
        }
    }
}