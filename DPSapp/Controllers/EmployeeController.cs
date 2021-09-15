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

        public ActionResult Index(string sortOrder)
        {

            if (Session["role"] != null)
            {
                ViewBag.CurrentSort = sortOrder;
                ViewBag.IdSortParm = String.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
                ViewBag.LoginSortParm = String.IsNullOrEmpty(sortOrder) ? "login_desc" : "";
                ViewBag.PasswordSortParm = String.IsNullOrEmpty(sortOrder) ? "password_desc" : "";
                ViewBag.PatientIdSortParm = String.IsNullOrEmpty(sortOrder) ? "patient_desc" : "";
                if (Session["role"].ToString() == "1")
                {
                    var users = db.Users.ToList();
                    switch (sortOrder)
                    {
                        case "login_desc":
                            users = users.OrderByDescending(s => s.Login).ToList(); // możliwość sortowania po loginie
                            break;
                        case "password_desc":
                            users = users.OrderByDescending(s => s.Password).ToList(); // możliwość sortowania po haśle (?)
                            break;
                        case "patient_desc":
                            users = users.OrderByDescending(s => s.PatientID).ToList(); // możliwość sortowania po roli
                            break;
                        case "id_desc":
                            users = users.OrderBy(s => s.UserId).ToList(); // możliwość sortowania po id
                            break;
                        default:
                            users = users.OrderBy(s => s.UserId).ToList(); // domyślnie sortuj po id
                            break;
                    }
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

        public ActionResult UserManagement(string sortOrder)
        {
            if (Session["role"] != null)
            {
                ViewBag.CurrentSort = sortOrder;
                ViewBag.IdSortParm = String.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
                ViewBag.LoginSortParm = String.IsNullOrEmpty(sortOrder) ? "login_desc" : "";
                    ViewBag.PasswordSortParm = String.IsNullOrEmpty(sortOrder) ? "password_desc" : "";
                    ViewBag.PatientIdSortParm = String.IsNullOrEmpty(sortOrder) ? "patient_desc" : "";
                if (Session["role"].ToString() == "1")
                { 
                        var users = db.Users.ToList();
                        switch (sortOrder)
                        {
                            case "login_desc":
                                users = users.OrderByDescending(s => s.Login).ToList(); // możliwość sortowania po loginie
                                break;
                            case "password_desc":
                                users = users.OrderByDescending(s => s.Password).ToList(); // możliwość sortowania po haśle (?)
                                break;
                            case "patient_desc":
                                users = users.OrderByDescending(s => s.PatientID).ToList(); // możliwość sortowania po roli
                                break;
                        case "id_desc":
                            users = users.OrderBy(s => s.UserId).ToList(); // możliwość sortowania po id
                            break;
                        default:
                                users = users.OrderBy(s => s.UserId).ToList(); // domyślnie sortuj po id
                                break;
                        }
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

        // GET: Recipe/Create
        public ActionResult CreateUser()
        {
            UserManager userManager = new UserManager();
            var patients = db.Patients.ToList();
            ViewBag.Patients =
            ViewBag.PatientID = ToSelectList(patients.ToList<Patient>());
            return View(userManager);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateUser([Bind(Include = "Name, Role, Surname, PatientId")] UserManager userManager)
        {
            if (Session["role"] != null)
            {
                if (Session["role"].ToString() == "1")
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
                        string pass = System.Web.Security.Membership.GeneratePassword(12, 2);
                        string firstPart = name.Substring(0, 3);
                        string secondPart = last.Substring(0, 2);
                        Random r = new Random();
                        bool isFree = false;
                        string login = "";
                        while (isFree == false)
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


                        User user = new User { Login = login, Password = pass, RoleId = role, PatientID = patientID };
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


        public ActionResult EditUser(int? id)
        {
            if (Session["role"] != null)
            {
                if (Session["role"].ToString() == "1")
                {
                    var user = db.Users.Where(x => x.UserId == id).FirstOrDefault();
                    var patients = db.Patients.ToList();
                    ViewBag.Patients =
                    ViewBag.PatientID = ToSelectList(patients.ToList<Patient>());

                    return View(user);
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
        public ActionResult EditUser([Bind(Include = "UserId, Login, Password, RoleId, PatientId")] User editeduser)
        {
            if (Session["role"] != null)
            {
                if (Session["role"].ToString() == "1")
                {
                    if (ModelState.IsValid)
                    {
                        var previoususer = db.Users.Where(x => x.UserId == editeduser.UserId).FirstOrDefault();
                        db.Users.Remove(previoususer);
                        db.Users.Add(editeduser);
                        db.SaveChanges();


                        return RedirectToAction("UserManagement", "Employee");
                    }

                    return View(editeduser.UserId);
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
        // Więcej info o użytkowniku
        public ActionResult DetailsUser(int? id)
        {
            if (Session["role"] != null)
            {
                if (Session["role"].ToString() == "1")
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    User user = db.Users.Find(id);
                    var patients = db.Patients.ToList();
                    if (user == null)
                    {
                        return HttpNotFound();
                    }
                    var patient = patients.Where(x => x.PatientId == user.PatientID).FirstOrDefault();
                    if (user.PatientID != 0)
                    {
                        ViewBag.PatientN = patient.PatientName.ToString();
                        ViewBag.PatientS = patient.PatientSurname.ToString();
                    }
                    else
                    {
                        ViewBag.PatientN = "Brak";
                    }
                    return View(user);


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
                    var isTagForaAllAlreadyExist = db.Tags.Any(x => x.TagName == "Dla wszystkich");
                    
                    Tag tagDlaWszystkich = new Tag();
                    if (!isTagForaAllAlreadyExist)
                    {
                        
                        tagDlaWszystkich.TagName = "Dla wszystkich";
                        db.Tags.Add(tagDlaWszystkich);
                        db.SaveChanges();
                    }
                    else
                    {
                        tagDlaWszystkich = db.Tags.Where(x=>x.TagName== "Dla wszystkich").FirstOrDefault();
                    }

                    var tags = (from s in db.Tags
                               select s).ToList();

                    List<Tag> tagsWithAll = new List<Tag>();
                    tagsWithAll.Add(tagDlaWszystkich);
                    foreach (var item in tags)
                    {
                        tagsWithAll.Add(item);
                    }

                    var model = new EmployeeSender
                    {

                        AvailableListOfTags = ToSelectListItemTags(tagsWithAll.ToList())
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
                    Message m = new Message { Image = adres, MessageText = komunikat, IsAnnouncement = true };

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

                    var messages = db.Messages.Include("Tags").Where(x=>x.IsAnnouncement==true).ToList();

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


        public ActionResult DeleteMessage(int? id)
        {
            if (Session["role"] != null)
            {
                if (Session["role"].ToString() == "1")
                {
                    Message message = db.Messages.Where(x => x.MessageId == id).FirstOrDefault();


                    return View(message);


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
        public ActionResult DeleteMessage(int id)
        {
            if (Session["role"] != null)
            {
                if (Session["role"].ToString() == "1")
                {

                    Message messagetemp = db.Messages.Where(x => x.MessageId == id).FirstOrDefault();



                    db.Messages.Remove(messagetemp);
                    db.SaveChanges();



                    return RedirectToAction("SendedMessages", "Employee");

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


        public ActionResult EditMessage(int? id)
        {
            if (Session["role"] != null)
            {
                if (Session["role"].ToString() == "1")
                {
                    var mess = db.Messages.Include("Tags").Where(x => x.MessageId == id).FirstOrDefault();

                    MessageEditor mEditor = new MessageEditor
                    {
                        message = mess
                    };
                    return View(mEditor);
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
        public ActionResult EditMessage([Bind(Include = "message,file")] MessageEditor mEditor)
        {
            if (Session["role"] != null)
            {
                if (Session["role"].ToString() == "1")
                {
                    if (ModelState.IsValid)
                    {



                        
                        string filename = Path.GetFileName(mEditor.file.FileName);
                        string filepath = Path.Combine(Server.MapPath("~/FilesUpload"), filename);
                        mEditor.file.SaveAs(filepath);
                        string userName = Session["UserName"].ToString();
                        
                        string komunikat = mEditor.message.MessageText;
                        string adres = "~/FilesUpload\\" + filename;
                        Message tempmess = db.Messages.Include("Tags").Where(x => x.MessageId == mEditor.message.MessageId).FirstOrDefault();

                        List<Tag> tagi = tempmess.Tags.ToList();
                        Message m = new Message { Image = adres, MessageText = komunikat, IsAnnouncement = true, Tags=tagi };

                        var previousmessage = db.Messages.Where(x => x.MessageId == mEditor.message.MessageId).FirstOrDefault();
                        db.Messages.Remove(previousmessage);
                        db.Messages.Add(m);
                        db.SaveChanges();


                        return RedirectToAction("SendedMessages", "Employee");
                    }

                    return View(mEditor.message.MessageId);
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
                    var rooms = db.Rooms.Include("Patients").ToList();

                    
                    //zbierz tagi do pacjentów w danym pokoju
                    foreach (var room in rooms)
                    {
                        List<Patient> patients = new List<Patient>();

                        foreach (var patient in room.Patients )
                        {
                        var patienttemp = db.Patients.Include("Tags").Where(c => c.PatientId == patient.PatientId).FirstOrDefault();
                        patients.Add(patienttemp);
                        }

                        List<Tag> ListofTags = new List<Tag>();
                        foreach (var patient in patients)
                        {
                            foreach (var tag in patient.Tags)
                            {
                                ListofTags.Add(tag);
                            }

                        }
                        room.Tags = ListofTags;

                    }

                    //połącz wszytkie tagi w jedną liste
                    
                    

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

        public ActionResult EditPassword()
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

        [HttpPost]
        public ActionResult EditPassword([Bind(Include = "OldPass, NewPass1, NewPass2")] PassEditAssistant assistant)
        {
            if (ModelState.IsValid)
            {
                string login = Session["UserName"].ToString();
                var user = db.Users.Where(a => a.Login == login).Select(a => a).FirstOrDefault();
                if (user.Password==assistant.OldPass)
                {
                    if (assistant.NewPass1==assistant.NewPass2)
                    {

                        var user2 = db.Users.Where(a => a.Login == login).Select(a => a).FirstOrDefault();
                        user2.Password = assistant.NewPass1;
                        db.SaveChanges();

                        ViewBag.Message = "Zmieniono hasło";
                    }
                    else
                    {
                        ViewBag.Message = "Nowe hasła nie zgadzają się ";
                    }
                }
                else
                {
                    ViewBag.Message = "Podane stare hasło nie jest poprawne";
                }
            }
            
            return View(assistant);
        }


        public ActionResult DeleteUser(int? id)
        {
            if (Session["role"] != null)
            {
                if (Session["role"].ToString() == "1")
                {
                    User user = db.Users.Where(x => x.UserId == id).FirstOrDefault();
                    var patients = db.Patients.ToList();
                    var patient = patients.Where(x => x.PatientId == user.PatientID).FirstOrDefault();
                    if (user.PatientID != 0)
                    {
                        ViewBag.PatientN = patient.PatientName.ToString();
                        ViewBag.PatientS = patient.PatientSurname.ToString();
                    }
                    else
                    {
                        ViewBag.PatientN = "Brak";
                    }

                    return View(user);


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
        public ActionResult DeleteUser(int id)
        {
            if (Session["role"] != null)
            {
                if (Session["role"].ToString() == "1")
                {
                    User usertemp = db.Users.Where(x => x.UserId == id).FirstOrDefault();




                    db.Users.Remove(usertemp);
                    db.SaveChanges();



                    return RedirectToAction("UserManagement", "Employee");

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





        [NonAction]
        public SelectList ToSelectList(List<Patient> patients)
        {

            var dictionary = new Dictionary<int, string>();
            foreach (Patient patient in patients)
            {
               
                dictionary.Add(patient.PatientId, String.Format(patient.PatientName + " " + patient.PatientSurname));
            }
            SelectList list2 = new SelectList(dictionary, "Key", "Value");
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