using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DPSapp.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
        public int PatientID { get; set; }
    }
}