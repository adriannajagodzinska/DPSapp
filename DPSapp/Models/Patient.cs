using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DPSapp.Models
{
    public class Patient
    {
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public string PatientSurname { get; set; }
        public ICollection<Tag> Tags { get; set; }
    }
}