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

            if (Session["role"] != null)
            {
                if (Session["role"].ToString() == "1")
                {

                    ViewBag.CurrentSort = sortOrder;
                    ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                    ViewBag.SurnameSortParm = String.IsNullOrEmpty(sortOrder) ? "surname_desc" : "";
                    var patients = db.Patients.Include("Tags").ToList();
                    //var patients = from s in db.Patients
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


          //  return View();
        }
        // Więcej info o pacjencie
        public ActionResult Details(int? id)
        {
            if (Session["role"] != null)
            {
                if (Session["role"].ToString() == "1")
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    Patient patient = db.Patients.Find(id);
                    if (patient == null)
                    {
                        return HttpNotFound();
                    }
                    var patients = db.Patients.Include("Tags").ToList();
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
       

        // GET: Recipe/Create
        public ActionResult Create()
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

        // POST: Recipe/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PatientId,PatientSurName,PatientName")] Patient patient)
        {
            if (Session["role"] != null)
            {
                if (Session["role"].ToString() == "1")
                {
                    if (ModelState.IsValid)
                    {
                        Tag tag = new Tag();
                        tag.TagName = "pacjent" + patient.PatientName+patient.PatientSurname;
                        db.Tags.Add(tag);
                        
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
                        var temp = db.Tags.Include("Patients").Where(s => s.TagId ==tag.TagId).FirstOrDefault();
                        temp.Patients.Add(patient);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
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


                    string tagimie = "pacjent" + patienttemp.PatientName + patienttemp.PatientSurname;
                    
                    var tag = db.Tags.Include("Patients").Include("Rooms").Include("Messages").Where(x => x.TagName == tagimie).FirstOrDefault();
                    var usertemp = db.Users.Where(x => x.PatientID == id).FirstOrDefault();

                    if (usertemp!=null)
                    {
                        usertemp.PatientID = 0;
                        usertemp.PatientName = null;
                    }
                   if(tag!=null)
                    {
                        db.Tags.Remove(tag);
                    }
                   
                    
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

        public ActionResult EditPatient(int? id)
        {
            if (Session["role"] != null)
            {
                if (Session["role"].ToString() == "1")
                {
                    var patient = db.Patients.Where(x => x.PatientId == id).FirstOrDefault();


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
        [ValidateAntiForgeryToken]
        public ActionResult EditPatient([Bind(Include = "PatientId, PatientName, PatientSurname")] Patient editpatient)
        {
            if (Session["role"] != null)
            {
                if (Session["role"].ToString() == "1")
                {
                    if (ModelState.IsValid)
                    {
                        var previouspatient = db.Patients.Include("Tags").Where(x => x.PatientId == editpatient.PatientId).FirstOrDefault();
                        
                        string tagimie = "pacjent" + previouspatient.PatientName + previouspatient.PatientSurname;
                        var tag = db.Tags.Include("Patients").Include("Rooms").Include("Messages").Where(x =>x.TagName==tagimie ).FirstOrDefault();
                       
                        tag.TagName = "pacjent" + editpatient.PatientName + editpatient.PatientSurname;
                       
                        editpatient.Tags = new List<Tag>();
                        if(previouspatient.Tags.Count==0)
                        {
                            editpatient.Tags.Add(tag);
                        }
                        else
                        {
                            foreach (var item in previouspatient.Tags)
                            {
                                editpatient.Tags.Add(item);
                            }
                        }
                        
                        var user = db.Users.Where(x =>x.PatientID == editpatient.PatientId).FirstOrDefault();
                        if(user!=null)
                        {
                            user.PatientName = editpatient.PatientName + " " + editpatient.PatientSurname;
                        }    
                        

                        db.Patients.Remove(previouspatient);
                        db.Patients.Add(editpatient);
                        db.SaveChanges();


                        return RedirectToAction("Index", "Patient");
                    }

                    return View(editpatient.PatientId);
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