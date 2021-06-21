using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DPSapp.Models
{
    public class EmployeeSender
    {
        public HttpPostedFileBase file { get; set; }
        public string Komunikat { get; set; }

        public List<string> SelectedTags { get; set; }
       
        public IList<SelectListItem> AvailableListOfTags { get; set; }

        public EmployeeSender()
        {
            SelectedTags = new List<string>();
            AvailableListOfTags = new List<SelectListItem>();
        }
    }
}