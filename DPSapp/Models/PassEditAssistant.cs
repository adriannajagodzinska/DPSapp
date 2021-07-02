using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace DPSapp.Models
{
    public class PassEditAssistant
    {
        [DisplayName("Stare hasło")]
        public string OldPass { get; set; }
        [DisplayName("Nowe hasło")]
        public string NewPass1 { get; set; }
        [DisplayName("Powtórz nowe hasło")]
        public string NewPass2 { get; set; }
    }
}