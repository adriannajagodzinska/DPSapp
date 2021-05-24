using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DPSapp.Models
{
    public class ListOfUsers
    {
        public List<string> login { get; set; }
        public List<string> password { get; set; }
        public List<string> role { get; set; }
    }
}