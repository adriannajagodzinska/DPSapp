using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace DPSapp.Models
{
    public class AddTagToPatientHelper
    {
        public Patient Patient { get; set; }

        public int TagIdToAdd { get; set; }
        

        public List<Tag> ListOfTags { get; set; }

       
    }
}