using DPSapp.DAL;
using DPSapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Net;
using System.Dynamic;
using System.IO;

namespace DPSapp.Controllers
{
    public class EmployeeController : Controller
    {
        private DPSContext db = new DPSContext();
        // GET: Employee
       
        public ActionResult Index()
        {

            if (Session["role"]!=null)
            {
                if (Session["role"].ToString() == "1")
                {
                    var users = from u in db.Users
                                   select u;
                    return View(users.ToList());
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
        public ActionResult Patient()
        {
            if (Session["role"] != null)
            {
                if (Session["role"].ToString() == "1")
                {
                    return View();
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
        public ActionResult Tag()
        {
            if (Session["role"] != null)
            {
                if (Session["role"].ToString() == "1")
                {
                    return View();
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

        public ActionResult UserManagement()
        {
            if (Session["role"] != null)
            {
                if (Session["role"].ToString() == "1")
                {
                    var patients = from s in db.Patients
                                   select s;
                    ViewBag.PatientID = ToSelectListID(patients.ToList<Patient>());
                    ViewBag.Patient = patients.ToList(); 
                    return View();
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
        [ValidateAntiForgeryToken]
        public ActionResult UserManagement([Bind(Include ="Name, Role, Surname, PatientId")] UserManager userManager)
        {
            if (ModelState.IsValid)
            {
                string name = userManager.Name;
                string last = userManager.Surname;
                bool isFamily = userManager.Role;
               // int Pacjentid = userManager.PatientId;
                int role = 1;
                int patientID = 0;
                if (isFamily)
                {
                    role = 2;
                    patientID = int.Parse(userManager.PatientId);
                }
                string pass = System.Web.Security.Membership.GeneratePassword(12,2);
                string firstPart = name.Substring(0, 3);
                string secondPart = last.Substring(0, 2);
                Random r = new Random();
                bool isFree = false;
                string login = "";
                while (isFree==false)
                {
                    string thirdPart = r.Next(100, 1000).ToString();
                    login = string.Concat(firstPart, secondPart, thirdPart);
                    //To do - wprowadzić warunek braku loginu w bazie
                    if (true)
                    {
                        isFree = true;
                        TempData["login"] = login;
                        TempData["pass"] = pass;
                    }
                }


                User user = new User { Login = login, Password = pass, RoleId = role, PatientID =  patientID};
                //if (isFamily)
                //{
                //    user.PatientID= userManager.PatientId;
                //    User user = new User { Login = login, Password = pass, RoleId = role, PatientID =  pa};
                //}
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("LoginInfo");

            }

            return View(userManager);
        }
        public ActionResult LoginInfo()
        {

            if (Session["role"] != null)
            {
                if (Session["role"].ToString() == "1")
                {
                    LogInfo info = new LogInfo();
                    info.login = TempData["login"].ToString();
                    info.pass = TempData["pass"].ToString();
                    return View(info);
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
        public ActionResult Send()
        {
            if (Session["role"] != null)
            {
                if (Session["role"].ToString() == "1")
                {
                    var tags = from s in db.Tags
                               select s;
                    var model = new EmployeeSender
                    {

                        AvailableListOfTags = ToSelectListItemTags(tags.ToList())
                    };
                    return View(model);

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
        public ActionResult Send([Bind(Include = "Komunikat, file, SelectedTags")] EmployeeSender fSender)
        {
            if (ModelState.IsValid)
            {
                //try
                //{
                    if (fSender.file.ContentLength > 0)
                    {
                        string filename = Path.GetFileName(fSender.file.FileName);
                        string filepath = Path.Combine(Server.MapPath("~/FilesUpload"), filename);
                        fSender.file.SaveAs(filepath);
                        string userName = Session["UserName"].ToString();
                        List<Tag> tagsSelected=new List<Tag>();
                        for (int i = 0; i < fSender.SelectedTags.Count; i++)
                        {
                            var nametagtemp = fSender.SelectedTags[i];
                            var tagtemp= (from s in db.Tags
                                         where s.TagName == nametagtemp             
                                         select s);

                        Tag taglist = tagtemp.ToList().First(); ;
                         tagsSelected.Add(taglist);
                          
                           
                        }
                     string komunikat = fSender.Komunikat;
                    string adres = "~/FilesUpload\\" + filename;
                        //string adres = filepath;
                        Message m = new Message { Image = adres, MessageText = komunikat };

                    m.Tags = tagsSelected;
                      

                        //foreach (var tag in tagsSelected)
                        //{
                        //    m.Tags.Add(tag);
                          
                        //}


                        db.Messages.Add(m);
                        db.SaveChanges();
                    }
                    ViewBag.Message = "Plik poprawnie załadowany!";
                    return RedirectToAction("Index", "Employee"); ;
                //}
                //catch (Exception)
                //{
                //    ViewBag.Message = "Plik nie załadował się!";
                //    return View();
                //}

            }
            return View(fSender);
        }



        public ActionResult SendedMessages()
        {
            if (Session["role"] != null)
            {
                if (Session["role"].ToString() == "1")
                {
                //    var messages = (from s in db.Messages
                //               select s).ToList();

                    var messages = db.Messages.Include("Tags").ToList();

                    return View(messages);

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


        public ActionResult Rooms()
        {
            if (Session["role"] != null)
            {
                if (Session["role"].ToString() == "1")
                {
                    var rooms = db.Rooms.ToList();
                    
                    return View(rooms);

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

        public ActionResult RoomsManager(int? id)
        {
            if (Session["role"] != null)
            {
                if (Session["role"].ToString() == "1")
                {

                   
                    //Zbierz pacjentów z danego pokoju
                    var room = db.Rooms.Include("Patients").Where(c => c.RoomNumber == id).First();
                    List<Patient> patients = new List<Patient>();
                    //zbierz tagi do pacjentów w danym pokoju
                    foreach (var item in room.Patients)
                    {
                        var patient = db.Patients.Include("Tags").Where(c => c.PatientId == item.PatientId).FirstOrDefault();
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
                    List<Message> messages = new List<Message>();
                    List<Tag> tagiost = new List<Tag>();
                    foreach (var tag in tagi)
                    {
                        //var temp = _db.Messages.Where(c => c.Tags == item).Select(c => c.Image);
                        var temp = db.Tags.Include("Messages").Where(c => c.TagId == tag.TagId).FirstOrDefault();
                        tagiost.Add(temp);
                        
                    }
                    foreach (var tag in tagiost)
                    {
                        foreach (var item in tag.Messages)
                        {
                            messages.Add(item);
                        }

                    }

                    return View(messages);

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
        public SelectList ToSelectListID(List<Patient> patients)
        {
            List<SelectListItem> list = new List<SelectListItem>();

            foreach (Patient patient in patients)
            {
                var Temp = patient.PatientId.ToString();
                list.Add(new SelectListItem()
                {
                    //Value = "",
                    //Text = patient.PatientId.ToString(),
                    Text = patient.PatientName.ToString(),
                    Value = patient.PatientId.ToString()
                });
            }
            SelectList list2 = new SelectList(list, "Value", "Text");
            return list2;
        }

        public List<SelectListItem> ToSelectListItemTags(List<Tag> tags)
        {
            List<SelectListItem> list = new List<SelectListItem>();

            foreach (Tag tag in tags)
            {
                var Temp = tag.TagId.ToString();
                list.Add(new SelectListItem()
                {
                    //Value = "",
                    //Text = patient.PatientId.ToString(),
                    Text = tag.TagId.ToString(),
                    Value = tag.TagName.ToString()
                });
            }
            //SelectList list2 = new SelectList(list, "Value", "Text");
            return list;
        }

    }

}