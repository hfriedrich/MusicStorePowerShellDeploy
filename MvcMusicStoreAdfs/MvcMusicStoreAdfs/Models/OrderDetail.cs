using System;

namespace MvcMusicStoreAdfs.Models
{
    public class OrderDetail : ICanBeStored
    {
        public string Id { get; set; }

        public string Name
        {
            get { return Id; }
        }

        public Order Order { get; set; }
        public Album Album { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public OrderDetail()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
