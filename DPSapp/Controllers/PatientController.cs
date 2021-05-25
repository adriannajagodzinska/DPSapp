using DPSapp.DAL;
using DPSapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace DPSapp.Controllers
{
    public class PatientController : Controller
    {
        private DPSContext db = new DPSContext();
        // GET: Patient
        public ActionResult Index(string sortOrder)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.SurnameSortParm = String.IsNullOrEmpty(sortOrder) ? "surname_desc" : "";
            var patients = from s in db.Patients
                          select s;
            //if (!String.IsNullOrEmpty(searchString))
            //{
            //    recipes = recipes.Where(s => s.RecipeName.Contains(searchString)
            //                           || s.Sources.Contains(searchString));
            //}
            switch (sortOrder)
            {
                case "name_desc":
                    patients = patients.OrderByDescending(s => s.PatientName); // możliwość sortowania po imieniu
                    break;
                case "surname_desc":
                    patients = patients.OrderByDescending(s => s.PatientSurname); // możliwość sortowania po imieniu
                    break;
                default:
                    patients = patients.OrderBy(s => s.PatientSurname); // domyślnie sortuj po nazwiskach
                    break;
            }
            return View(patients.ToList());
          //  return View();
        }
        // Więcej info o pacjencie
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = db.Patients.Find(id);
            if(patient==null)
            {
                return HttpNotFound();
            }
            return View(patient);
        }

        // GET: Recipe/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Recipe/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PatientId,PatientSurName,PatientName")] Patient patient)
        {
            if (ModelState.IsValid)
            {
                db.Patients.Add(patient);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(patient);
        }
    }
}