using System.Collections.Generic;
using MvcMusicStore.Models;

namespace MvcMusicStore.Repository
{
    public interface IPersistCarts : IPersistModel<Cart>
    {
        Cart LoadByAlbumAndShoppingCartId(string albumId, string shoppingCartId);
        Cart LoadById(string id, string shoppingCartId);
        IEnumerable<Cart> LoadByCartId(string shoppingCartId);
    }
}