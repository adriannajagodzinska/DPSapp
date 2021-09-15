using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DPSapp.Models
{
    public class AddTagToMessageHelper
    {
        public Message message { get; set; }

        public int TagIdToAdd { get; set; }


        public List<Tag> ListOfTags { get; set; }
    }
}