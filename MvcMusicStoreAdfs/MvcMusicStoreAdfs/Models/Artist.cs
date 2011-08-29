using System;

namespace MvcMusicStoreAdfs.Models
{
    public class Artist : ICanBeStored
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public Artist()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}