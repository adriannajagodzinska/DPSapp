using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DPSapp.Models
{
    public class Message
    {
        public int MessageId { get; set; }
        public string Image { get; set; }
        public string MessageText { get; set; } //Komunikat tylko tekstowy

        public virtual ICollection<Tag> Tags { get; set; }
    }
}