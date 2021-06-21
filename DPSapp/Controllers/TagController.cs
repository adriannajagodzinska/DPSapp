using DPSapp.DAL;
using DPSapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public ActionResult Create([Bind(Include = "TagId,TagName")] Tag tag)
        {
            if (ModelState.IsValid)
            {
                db.Tags.Add(tag);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tag);
        }

        public ActionResult AddTagToPatient()
        {
            ViewBag.TagName = new SelectList(db.Tags, "TagId", "TagName");
            return View();
        }
    }
}