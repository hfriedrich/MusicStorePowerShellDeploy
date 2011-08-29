using System.Collections.Generic;
using MvcMusicStoreAdfs.Models;

namespace MvcMusicStoreAdfs.ViewModels
{
    public class ShoppingCartViewModel
    {
        public List<Cart> CartItems { get; set; }
        public decimal CartTotal { get; set; }
    }
}