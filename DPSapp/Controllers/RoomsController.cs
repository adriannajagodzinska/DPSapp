using DPSapp.DAL;
using DPSapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DPSapp.Controllers
{
    public class RoomsController : Controller
    {

        private DPSContext _db = new DPSContext();

        // GET: Rooms
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Slider(int? id)
        {
            
            ListOfAdressessToMedia ListOfAdresses = new ListOfAdressessToMedia();
            //Zbierz pacjentów z danego pokoju
            var room = _db.Rooms.Include("Patients").Where(c => c.RoomNumber == id).First();
            List<Patient> patients = new List<Patient>(); 
            //zbierz tagi do pacjentów
            foreach (var item in room.Patients)
            {
                 var patient = _db.Patients.Include("Tags").Where(c => c.PatientId == item.PatientId).FirstOrDefault();
                patients.Add(patient);
            }

            //połącz wszytkie tagi w jedną liste
            List<Tag> ListofTags = new List<Tag>(); 
            foreach (var patient in patients)
            {
                foreach (var tag in patient.Tags)
                {
                    ListofTags.Add(tag);
                }
                
            }



            List<Tag> tagi = ListofTags.ToList();
            
            //zbierz wszystkie wiadomości, które mają te tagi
            List<string> adresses = new List<string>();
            foreach (var tag in tagi)
            {
                //var temp = _db.Messages.Where(c => c.Tags == item).Select(c => c.Image);
               
                var temp = (from m in _db.Messages
                            where m.Tags.Any(c => c.TagId == tag.TagId)
                            select m);

                List<Message> wiadomosci = temp.ToList();
                foreach (var messaage in temp)
                {
                    adresses.Add(messaage.Image);
                }
            }
           ;

            ListOfAdresses.ListOfAdresses = adresses;

            return View(ListOfAdresses);
        }
        public ActionResult CreateRoom()
        {
            if (Session["role"] != null)
            {
                if (Session["role"].ToString() == "1")
                {
                    Room room = new Room();
                    return View(room);


                }
                else
                {
                    return RedirectToAction("Error401", "Home");
                }

            }
            else
            {
                return RedirectToAction("Error401", "Home");
            }

        }


        [HttpPost]
        public ActionResult CreateRoom([Bind(Include = "RoomNumber")] Room room)
        {
            if (Session["role"] != null)
            {
                if (Session["role"].ToString() == "1")
                {


                    _db.Rooms.Add(room);
                    _db.SaveChanges();



                    return RedirectToAction("Rooms", "Employee");

                }
                else
                {
                    return RedirectToAction("Error401", "Home");
                }

            }
            else
            {
                return RedirectToAction("Error401", "Home");
            }
        }



        public ActionResult DeleteRoom(int? id)
        {
            if (Session["role"] != null)
            {
                if (Session["role"].ToString() == "1")
                {
                    Room room = _db.Rooms.Where(x=>x.RoomId==id).FirstOrDefault();
                    
                  
                    return View(room);


                }
                else
                {
                    return RedirectToAction("Error401", "Home");
                }

            }
            else
            {
                return RedirectToAction("Error401", "Home");
            }

        }


        [HttpPost]
        public ActionResult DeleteRoom(int id)
        {
            if (Session["role"] != null)
            {
                if (Session["role"].ToString() == "1")
                {

                    Room roomtemp = _db.Rooms.Where(x => x.RoomId == id).FirstOrDefault();



                    _db.Rooms.Remove(roomtemp);
                    _db.SaveChanges();



                    return RedirectToAction("Rooms", "Employee");

                }
                else
                {
                    return RedirectToAction("Error401", "Home");
                }

            }
            else
            {
                return RedirectToAction("Error401", "Home");
            }
        }



        public ActionResult ChooseRoomSite()
        {
            var rooms = (from r in _db.Rooms
                         select r).ToList();
            //List<Room> rooms = new List<Room>();
            //Room room1 = new Room();
            //room1.RoomNumber = 1;
            //Room room2 = new Room();
            //room2.RoomNumber = 2;
            //rooms.Add(room1);
            //rooms.Add(room2);


            //Tag pokoj1 = new Tag();
            //Tag pokoj2 = new Tag();
            //pokoj1.TagName = "pokoj1";
            //pokoj2.TagName = "pokoj2";


            //room1.Tags = new List<Tag>();
            //room2.Tags = new List<Tag>();
            //room1.Tags.Add(pokoj1);
            //room2.Tags.Add(pokoj2);

            //Message wiad1 = new Message();
            //wiad1.Tags = new List<Tag>();
            //wiad1.Tags.Add(pokoj1);

            //Message wiad2 = new Message();
            //wiad2.Tags = new List<Tag>();
            //wiad2.Tags.Add(pokoj2);

            //wiad1.Image = "~/Images\\1.jpg";
            //wiad2.Image = "~/Images\\4.mp4";
            //_db.Tags.Add(pokoj1);
            //_db.Tags.Add(pokoj2);
            //_db.Messages.Add(wiad1);
            //_db.Messages.Add(wiad2);
            //_db.Rooms.Add(room1);
            //_db.Rooms.Add(room2);



            // _db.Rooms.Add(room2);
            //bool a = _db.ChangeTracker.HasChanges();
            //_db.SaveChanges();
            return View(rooms);
        }

    }
}