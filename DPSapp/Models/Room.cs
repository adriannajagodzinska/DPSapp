using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DPSapp.Models
{
    public class Room
    {
        public int RoomId { get; set; }
        [Required]
        public int RoomNumber { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
    }
}