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
            var patients = db.Patients.Include("Tags").ToList();
            //var patients = from s in db.Patients

            //               select s;
            //if (!String.IsNullOrEmpty(searchString))
            //{
            //    recipes = recipes.Where(s => s.RecipeName.Contains(searchString)
            //                           || s.Sources.Contains(searchString));
            //}
          
            switch (sortOrder)
            {
                case "name_desc":
                     patients = patients.OrderByDescending(s => s.PatientName).ToList(); // możliwość sortowania po imieniu
                    break;
                case "surname_desc":
                     patients = patients.OrderByDescending(s => s.PatientSurname).ToList(); // możliwość sortowania po imieniu
                    break;
                default:
                     patients = patients.OrderBy(s => s.PatientSurname).ToList(); // domyślnie sortuj po nazwiskach
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
                //string name = patient.PatientName;
                //string last = patient.PatientSurname;
                //string pass = System.Web.Security.Membership.GeneratePassword(12, 2);
                //string firstPart = name.Substring(0, 3);
                //string secondPart = last.Substring(0, 2);
                //Random r = new Random();
                //string thirdPart = r.Next(100, 1000).ToString();
                //string patientTag = string.Concat(firstPart, secondPart, thirdPart);
                //var tags = db.Tags.ToList();
                //int new_tag_id = tags.Count() + 1;
                //var new_tag = new Tag() { TagId=new_tag_id, TagName = patientTag };
                //db.Tags.Add(new_tag);
                //db.SaveChanges();
                db.Patients.Add(patient);
                //db.SaveChanges();

                //Patient patient1 = db.Patients.FirstOrDefault(x => x.PatientId == patient.PatientId);
                //new_tag.Patients.Add(patient);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(patient);
        }


        public ActionResult DeletePatient(int? id)
        {
            if (Session["role"] != null)
            {
                if (Session["role"].ToString() == "1")
                {
                    Patient patient = db.Patients.Where(x => x.PatientId == id).FirstOrDefault();


                    return View(patient);


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
        public ActionResult DeletePatient(int id)
        {
            if (Session["role"] != null)
            {
                if (Session["role"].ToString() == "1")
                {
                    Patient patienttemp = db.Patients.Where(x => x.PatientId == id).FirstOrDefault();
                    



                    db.Patients.Remove(patienttemp);
                    db.SaveChanges();



                    return RedirectToAction("Index", "Patient");

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