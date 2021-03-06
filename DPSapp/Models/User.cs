using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DPSapp.Models
{
    public class User
    {
        public int UserId { get; set; }
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [Range(1,2)]
        public int RoleId { get; set; }
        public int PatientID { get; set; }
        public string PatientName { get; set; }
    }
}