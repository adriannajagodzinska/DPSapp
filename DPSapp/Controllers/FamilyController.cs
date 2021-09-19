using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DPSapp.Models;
using DPSapp.DAL;
using System.IO;

namespace DPSapp.Controllers
{
    public class FamilyController : Controller
    {
        // GET: Family
        private DPSContext db = new DPSContext();
        public ActionResult Index()
        {

            if (Session["role"] != null)
            {
                if (Session["role"].ToString() == "2")
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

        public ActionResult Send()
        {
            if (Session["role"] != null)
            {
                if (Session["role"].ToString() == "2")
                {
                    var model = new FamilySender();
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
        public ActionResult Send([Bind(Include = "Komunikat, file")] FamilySender fSender )
        {
            if (ModelState.IsValid)
            {
                
                   if (fSender.file.ContentLength > 0)
                    {
                        //Patient p = db.Patients.Where(a=>a.PatientId==2).Select(a => a).FirstOrDefault();
                        //p.Tags = new List<Tag>();
                        //Tag t = db.Tags.Where(a => a.TagId == 1).Select(a => a).FirstOrDefault();
                        //p.Tags.Add(t);
                        //db.SaveChanges();
                        string filename = Path.GetFileName(fSender.file.FileName);
                        string filepath = Path.Combine(Server.MapPath("~/FilesUpload"), filename);
                        fSender.file.SaveAs(filepath);
                        string userName = Session["UserName"].ToString();
                        int pID = db.Users.Where(a => a.Login == userName).Select(a => a.PatientID).FirstOrDefault();
                        var pacjent = db.Patients.Include("Tags").Where(a => a.PatientId == pID).Select(a => a).FirstOrDefault();
                        var Tagi = pacjent.Tags;
                        var tID = Tagi.Select(a => a).FirstOrDefault();
                        
                        string komunikat = fSender.Komunikat;
                        string adres = "~/FilesUpload\\" + filename; 
                        Message m = new Message { Image = adres, MessageText = komunikat};
                        m.Tags = new List<Tag>();
                        m.Tags.Add(tID);
                        db.Messages.Add(m);
                        db.SaveChanges();
                        //int mID = db.Messages.Where(a => a.Image == adres).Select(a => a.MessageId).FirstOrDefault();
                        //db.
                    }
                    ViewBag.Message = "Plik poprawnie załadowany!";
                    return RedirectToAction("Index", "Family");
                
                
            }
            return View(fSender);
        }

        public ActionResult EditPassword()
        {
            if (Session["role"] != null)
            {
                if (Session["role"].ToString() == "2")
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
                if (user.Password == assistant.OldPass)
                {
                    if (assistant.NewPass1 == assistant.NewPass2)
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


        public ActionResult SendedMessages()
        {
            if (Session["role"] != null)
            {
                if (Session["role"].ToString() == "2")
                {
                    //    var messages = (from s in db.Messages
                    //               select s).ToList();
                    string userName = Session["UserName"].ToString();
                    int pID = db.Users.Where(a => a.Login == userName).Select(a => a.PatientID).FirstOrDefault();
                    var pacjent = db.Patients.Include("Tags").Where(a => a.PatientId == pID).Select(a => a).FirstOrDefault();
                    var Tagi = pacjent.Tags;
                    var tID = Tagi.Select(a => a).FirstOrDefault();
                    List<Message> messages = new List<Message>();
                    List<Message> allmessages = db.Messages.Include("Tags").ToList();
                    foreach (var tagpacjenta in Tagi)
                    {
                        foreach (var message in allmessages)
                        {
                            foreach (var tagwiadomosci in message.Tags)
                            {
                                if (tagpacjenta.TagId == tagwiadomosci.TagId)
                                {
                                    messages.Add(message);
                                    break;
                                }
                            }

                            
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
        public ActionResult DeleteMessage(int? id)
        {
            if (Session["role"] != null)
            {
                if (Session["role"].ToString() == "2")
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
                if (Session["role"].ToString() == "2")
                {

                    Message messagetemp = db.Messages.Where(x => x.MessageId == id).FirstOrDefault();



                    db.Messages.Remove(messagetemp);
                    db.SaveChanges();



                    return RedirectToAction("SendedMessages", "Family");

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
                if (Session["role"].ToString() == "2")
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
                if (Session["role"].ToString() == "2")
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
                        Message m = new Message { Image = adres, MessageText = komunikat, IsAnnouncement = true, Tags = tagi };

                        var previousmessage = db.Messages.Where(x => x.MessageId == mEditor.message.MessageId).FirstOrDefault();
                        db.Messages.Remove(previousmessage);
                        db.Messages.Add(m);
                        db.SaveChanges();


                        return RedirectToAction("SendedMessages", "Family");
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

    }
}