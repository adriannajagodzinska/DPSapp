﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DPSapp.Models
{
    public class UserManager
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public bool Role { get; set; }
        public string PatientId { get; set; }

    }
}