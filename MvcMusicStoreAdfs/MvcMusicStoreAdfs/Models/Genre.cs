using System;
using System.Collections.Generic;

namespace MvcMusicStoreAdfs.Models
{
    public class Genre : ICanBeStored
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Album> Albums { get; set; }

        public Genre()
        {
            Id = Guid.NewGuid().ToString();
            Albums = new List<Album>();
        }
    }
}
