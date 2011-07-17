using System.Collections.Generic;
using MvcMusicStoreAdfs.Models;

namespace MvcMusicStoreAdfs.Repository
{
    public interface IPersistCarts : IPersistModel<Cart>
    {
        Cart LoadByAlbumAndShoppingCartId(string albumId, string shoppingCartId);
        Cart LoadById(string id, string shoppingCartId);
        IEnumerable<Cart> LoadByCartId(string shoppingCartId);
    }
}