using System;

namespace MvcMusicStore.Models
{
    public class Cart : ICanBeStored
    {
        public string Id { get; set; }

        public string Name
        {
            get { return Id; }
        }

        public string CartId { get; set; }
        public Album Album { get; set; }
        public int Count { get; set; }
        public DateTime DateCreated { get; set; }

        public Cart()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}