using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DPSapp.Models
{
    public class AddPatientsToRoomHelper
    {
        public Room Room { get; set; }

        public int PatientIdToAdd { get; set; }


        public List<Patient> ListOfPatients { get; set; }
    }
}