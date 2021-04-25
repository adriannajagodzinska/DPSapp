using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DPSapp.Models
{
    public class Patient
    {
        public int PatientId { get; set; }
        [Required]
        public string PatientName { get; set; }
        [Required]
        public string PatientSurname { get; set; }
        
        public ICollection<Tag> Tags { get; set; }
    }
}