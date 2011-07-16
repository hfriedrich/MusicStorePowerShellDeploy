using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace MvcMusicStore.Models
{
    public class Album : ICanBeStored
    {
        public string Id { get; set; }

        [JsonIgnore]
        public string Name
        {
            get { return Title; }
        }

        [DisplayName("Genre")]
        public Genre Genre { get; set; }

        [DisplayName("Artist")]
        public Artist Artist { get; set; }

        [Required(ErrorMessage = "An Album Title is required")]
        [StringLength(160)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, 100.00,
            ErrorMessage = "Price must be between 0.01 and 100.00")]
        public decimal Price { get; set; }

        [DisplayName("Album Art URL")]
        [StringLength(1024)]
        public string AlbumArtUrl { get; set; }

        private List<OrderDetail> _orderDetails;
        public List<OrderDetail> OrderDetails
        {
            get
            {
                return _orderDetails ?? new List<OrderDetail>();
            }
            set { _orderDetails = value; }
        }

        public Album()
        {
            Id = Guid.NewGuid().ToString();
            OrderDetails = new List<OrderDetail>();
        }
    }
}