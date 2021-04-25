using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DPSapp.Models
{
    public class Tag
    {
        public int TagId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Room> Rooms { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
        public virtual ICollection<Patient> Patients { get; set; }
    }
}