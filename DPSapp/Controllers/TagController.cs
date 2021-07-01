﻿using DPSapp.DAL;
using DPSapp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;


namespace DPSapp.Controllers
{
    public class TagController : Controller
    {
        private DPSContext db = new DPSContext();
        // GET: Tag
        public ActionResult Index(string sortOrder)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.IdSortParm = String.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
            var tags = from s in db.Tags
                       select s;
            switch (sortOrder)
            {
                case "name_desc":
                    tags = tags.OrderByDescending(s => s.TagName); // możliwość sortowania po nazwie
                    break;
                default:
                    tags = tags.OrderBy(s => s.TagId); // domyślnie sortuj po id
                    break;
            }
            return View(tags.ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        // POST: Tag/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TagId,TagName,IsGlobal")] Tag tag)
        {
            if (ModelState.IsValid)
            {
                db.Tags.Add(tag);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tag);
        }
        // GET: Recipe/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tag tag = db.Tags.Find(id);
            if (tag == null)
            {
                return HttpNotFound();
            }
            return View(tag);
        }

        // POST: Recipe/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TagId,TagName")] Tag tag)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tag).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tag);
        }
        public ActionResult AddTagToPatient(int? id)
        {
            if (Session["role"] != null)
            {
                if (Session["role"].ToString() == "1")
                {
                    //var tags = from s in db.Tags
                    //           select s;
                    var tags = db.Tags.ToList();
                    ViewBag.TagID = ToSelectListID(tags.ToList<Tag>());
                    var patient = db.Patients.Include("Tags").Where(x => x.PatientId == id).First();
                    AddTagToPatientHelper helper = new AddTagToPatientHelper();
                    helper.Patient = patient;
                    tags.ToList<Tag>();
                    helper.ListOfTags = tags;
                   

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
        public ActionResult AddTagToPatient([Bind(Include = "TagIdToAdd")]AddTagToPatientHelper helper)
        {
            if (Session["role"] != null)
            {
                if (Session["role"].ToString() == "1")
                {
                   // int patientid = helper.Patient.PatientId;
                    int patientid = int.Parse(this.RouteData.Values["id"].ToString());
                   
                    var tag = db.Tags.Include("Patients").Where(s => s.TagId == helper.TagIdToAdd).FirstOrDefault();
                    //var patientTags = from tag in db.Tags
                    //                  where tag.Patients.Any(c => c.PatientId == patientid)
                    //                  select tag;

                    var patient = db.Patients.Include("Tags").Where(s => s.PatientId == patientid).FirstOrDefault();

                    tag.Patients.Add(patient);

                    db.SaveChanges();
                    return RedirectToAction("Index");
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
        public SelectList ToSelectListID(List<Tag> tags)
        {

           // List<SelectListItem> list = new List<SelectListItem>();
            var dictionary = new Dictionary<int, string>();
            foreach (Tag tag in tags)
            {
                // var Temp = tag.TagId.ToString();
                //list.Add(new SelectListItem()
                //{
                //    //Value = "",
                //    //Text = patient.PatientId.ToString(),
                //    Text = tag.TagName.ToString(),
                //    Value = tag.TagId.ToString()
                //}) ;
                dictionary.Add(tag.TagId, tag.TagName);
            }
            SelectList list2 = new SelectList(dictionary, "Key", "Value");
            return list2;
        }

        [NonAction]
        public List<SelectListItem> ToSelectListItemTags(IQueryable<Tag> tags)
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