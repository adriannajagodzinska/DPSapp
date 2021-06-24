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
        public ActionResult Send([Bind(Include = "Komunikat, file")] FamilySender fSender )
        {
            if (ModelState.IsValid)
            {
                try
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
                        var pacjent = db.Patients.Where(a => a.PatientId == pID).Select(a => a).FirstOrDefault();
                        var Tagi = pacjent.Tags;
                        var tID = Tagi.Select(a => a).FirstOrDefault();
                        
                        string komunikat = fSender.Komunikat;
                        string adres = filepath;
                        Message m = new Message { Image = adres, MessageText = komunikat};
                        m.Tags = new List<Tag>();
                        m.Tags.Add(tID);
                        db.Messages.Add(m);
                        db.SaveChanges();
                        //int mID = db.Messages.Where(a => a.Image == adres).Select(a => a.MessageId).FirstOrDefault();
                        //db.
                    }
                    ViewBag.Message = "Plik poprawnie załadowany!";
                    return View();
                }
                catch (Exception)
                {
                    ViewBag.Message = "Plik nie załadował się!";
                    return View();
                }
                
            }
            return View(fSender);
        }

        
    }
}