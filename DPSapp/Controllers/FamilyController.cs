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
        public ActionResult Send([Bind(Include = "Komunikat")] FamilySender fSender, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (file.ContentLength > 0)
                    {
                        string filename = Path.GetFileName(file.FileName);
                        string filepath = Path.Combine(Server.MapPath("~/FilesUpload"), filename);
                        file.SaveAs(filepath);
                        string userName = Session["UserName"].ToString();
                        int pID = db.Users.Where(a => a.Login == userName).Select(a => a.PatientID).FirstOrDefault();
                        var Tagi = db.Patients.Where(a => a.PatientId == pID).Select(a => a.Tags);
                        Tag tID = (Tag)Tagi.Select(a => a).FirstOrDefault();
                        string komunikat = fSender.Komunikat;
                        string adres = filepath;
                        Message m = new Message { Image = adres, MessageText = komunikat, Tags = (ICollection<Tag>)tID };
                        db.Messages.Add(m);
                        db.SaveChanges();
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