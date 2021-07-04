using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DPSapp.Models
{
    public class LoginAssistant
    {
      // [Required(ErrorMessage = "Nieprawidłowy login")]
      // [DisplayFormat(NullDisplayText = "Należy wprowadzić login")]
        public string login { get; set; }
       // [Required(ErrorMessage = "Nieprawidłowe hasło")]
       // [DisplayFormat(NullDisplayText = "Należy wprowadzić hasło")]
        public string password { get; set; }
       
    }
}