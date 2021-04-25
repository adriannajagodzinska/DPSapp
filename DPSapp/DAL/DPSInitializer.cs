using DPSapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DPSapp.DAL
{
    public class DPSInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<DPSContext>
    {
        protected override void Seed(DPSContext context)
        {
            var tags = new List<Tag>
            {
                new Tag{TagName="Kobieta"},
                new Tag{TagName="Demencja"},
                new Tag{TagName="Niedowidząca"},
                new Tag{TagName="Niewidoma"},
                new Tag{TagName="Pokój_1"},
                new Tag{TagName="Pokój_2"}
            };
            var tag2 = new List<Tag> {
            new Tag{TagName="Kobieta"},
                new Tag{TagName="Demencja"},
                new Tag{TagName="Niedowidząca"},
                new Tag{TagName="Pokój_1"}
            };
            var tag3 = new List<Tag>
            {
                 new Tag{TagName="Kobieta"},
                new Tag{TagName="Demencja"},
                 new Tag{TagName="Pokój_2"}
            };
            var Room = new List<Room>
            {
                new Room{RoomNumber=1},
                new Room{RoomNumber=2}
            };
            var patients = new List<Patient>
            {
                new Patient{PatientName="Karolina", PatientSurname="Cicha", Tags=tag2},
                new Patient{PatientName="Adrianna",PatientSurname="Jagodzińska", Tags=tag3}

            };



            
            context.SaveChanges();
        }
    }
}