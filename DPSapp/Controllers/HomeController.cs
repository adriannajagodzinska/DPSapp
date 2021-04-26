using DPSapp.DAL;
using DPSapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DPSapp.Controllers
{
    public class HomeController : Controller
    {
        private DPSContext db = new DPSContext();
        public ActionResult Index()
        {
        //   Patient patient = new Patient { PatientName = "Karolina", PatientSurname = "Cicha" };
        //    db.Patients.Add(patient);
         //   db.SaveChanges();
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}