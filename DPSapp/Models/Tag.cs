using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DPSapp.Models
{
    public class Tag
    {
        public int TagId { get; set; }
        [Required]
        public string TagName { get; set; }
        public bool IsAnnouncement { get; set; }
        public virtual ICollection<Room> Rooms { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
        public virtual ICollection<Patient> Patients { get; set; }
        
    }
}