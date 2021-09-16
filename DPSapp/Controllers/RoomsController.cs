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
                    bool containsItem = ListofTags.Any(item => item.TagId == tag.TagId);
                    if(!containsItem)
                    {
                            ListofTags.Add(tag);
                    }
                    
                }
                
            }



            List<Tag> tagi = ListofTags.ToList();
            
            //zbierz wszystkie wiadomości, które mają te tagi
            List<string> adresses = new List<string>();
            foreach (var tag in tagi)
            {
            //    var temp3 = _db.Messages.Include("Tags").Where(c => c.Tags == tag).Select(x => x.Image).ToList();
            //    var temp2 = (from m in _db.Messages select m).ToList();
            //    var temp = (from m in _db.Messages
            //                where m.Tags.Any(c => c.TagId == tag.TagId)
            //                select m);
                var wszystkiewiadomosci = _db.Messages.Include("Tags").ToList();


                List<Message> wiadomosci = new List<Message>();
                foreach (var wiadomosc in wszystkiewiadomosci)
                {

                    foreach (var tagiwiadomosci in wiadomosc.Tags)
                    {
                        if(tagiwiadomosci.TagId==tag.TagId)
                        {
                            wiadomosci.Add(wiadomosc);
                        }
                    }

                }



                foreach (var messaage in wiadomosci)
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

        public ActionResult AddPatientToRoom(int? id)
        {
            if (Session["role"] != null)
            {
                if (Session["role"].ToString() == "1")
                {
                    //var tags = from s in db.Tags
                    //           select s;
                    var patients = _db.Patients.ToList();
                    ViewBag.Patients = ToSelectList(patients.ToList<Patient>());
                    var room = _db.Rooms.Where(x => x.RoomId == id).First();
                    AddPatientsToRoomHelper helper = new AddPatientsToRoomHelper();
                    helper.Room = room;
                   
                    helper.ListOfPatients = patients;

                    ViewBag.ListOfPatients = ToSelectListID(helper.ListOfPatients.ToList());
                    //helper.ListOfTags = tags;
                    //helper.ListOfTags = ToSelectListID(tags.ToList<Tag>()); 


                    return View(helper);
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
        // public ActionResult Send([Bind(Include = "Komunikat, file, SelectedTags")] EmployeeSender fSender)
        [HttpPost]
        public ActionResult AddPatientToRoom([Bind(Include = "PatientIdToAdd")] AddPatientsToRoomHelper helper)
        {
            if (Session["role"] != null)
            {
                if (Session["role"].ToString() == "1")
                {
                    // int patientid = helper.Patient.PatientId;
                    int roomid = int.Parse(this.RouteData.Values["id"].ToString());

                    var patient = _db.Patients.Where(s => s.PatientId == helper.PatientIdToAdd).FirstOrDefault();
                    //var patientTags = from tag in db.Tags
                    //                  where tag.Patients.Any(c => c.PatientId == patientid)
                    //                  select tag;

                    var room = _db.Rooms.Include("Patients").Where(s => s.RoomId == roomid).FirstOrDefault();

                    room.Patients.Add(patient);
                  
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

        public ActionResult RemovePatienFromRoom(int? id)
        {
            if (Session["role"] != null)
            {
                if (Session["role"].ToString() == "1")
                {
                    //var tags = from s in db.Tags
                    //           select s;
                    var room = _db.Rooms.Include("Patients").Where(x => x.RoomId == id).First();
                    var patients = room.Patients.ToList();
                    ViewBag.Patients = ToSelectList(patients.ToList<Patient>());
                    
                    AddPatientsToRoomHelper helper = new AddPatientsToRoomHelper();
                    helper.Room = room;

                    helper.ListOfPatients = patients;


                    //helper.ListOfTags = tags;
                    //helper.ListOfTags = ToSelectListID(tags.ToList<Tag>()); 


                    return View(helper);
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
        // public ActionResult Send([Bind(Include = "Komunikat, file, SelectedTags")] EmployeeSender fSender)
        [HttpPost]
        public ActionResult RemovePatienFromRoom([Bind(Include = "PatientIdToAdd")] AddPatientsToRoomHelper helper)
        {
            if (Session["role"] != null)
            {
                if (Session["role"].ToString() == "1")
                {
                    // int patientid = helper.Patient.PatientId;
                    int roomid = int.Parse(this.RouteData.Values["id"].ToString());

                    var patient = _db.Patients.Where(s => s.PatientId == helper.PatientIdToAdd).FirstOrDefault();
                    //var patientTags = from tag in db.Tags
                    //                  where tag.Patients.Any(c => c.PatientId == patientid)
                    //                  select tag;

                    var room = _db.Rooms.Include("Patients").Where(s => s.RoomId == roomid).FirstOrDefault();

                    room.Patients.Remove(patient);

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



        [NonAction]
        public SelectList ToSelectList(List<Patient> patients)
        {

            // List<SelectListItem> list = new List<SelectListItem>();
            var dictionary = new Dictionary<int, string>();
            foreach (Patient patient in patients)
            {
                // var Temp = tag.TagId.ToString();
                //list.Add(new SelectListItem()
                //{
                //    //Value = "",
                //    //Text = patient.PatientId.ToString(),
                //    Text = tag.TagName.ToString(),
                //    Value = tag.TagId.ToString()
                //}) ;
                dictionary.Add(patient.PatientId, String.Format(patient.PatientName+" "+patient.PatientSurname));
            }
            SelectList list2 = new SelectList(dictionary, "Key", "Value");
            return list2;
        }



        public ActionResult ChooseRoomSite()
        {
            var rooms = (from r in _db.Rooms
                         select r).ToList();
           
            return View(rooms);
        }

        [NonAction]
        public SelectList ToSelectListID(List<Patient> patients)
        {

            // List<SelectListItem> list = new List<SelectListItem>();
            var dictionary = new Dictionary<int, string>();
            foreach (Patient patient in patients)
            {
                // var Temp = tag.TagId.ToString();
                //list.Add(new SelectListItem()
                //{
                //    //Value = "",
                //    //Text = patient.PatientId.ToString(),
                //    Text = tag.TagName.ToString(),
                //    Value = tag.TagId.ToString()
                //}) ;
                dictionary.Add(patient.PatientId, patient.PatientName+" "+patient.PatientSurname);
            }
            SelectList list2 = new SelectList(dictionary, "Key", "Value");
            return list2;
        }

    }
}