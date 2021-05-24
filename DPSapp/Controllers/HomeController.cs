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
            //Role role = new Role { RoleId = 1, RoleName = "Administrator" };
            //db.Roles.Add(role);

            //User user = new User { Login = "admin", Password = "admin", RoleId = 1 };
            //db.Users.Add(user);
            //db.SaveChanges();
            //   Patient patient = new Patient { PatientName = "Karolina", PatientSurname = "Cicha" };
            //    db.Patients.Add(patient);
            //   db.SaveChanges();
            ListOfUsers list = new ListOfUsers();
           
            list.login = new List<string>();
            list.password = new List<string>();
            list.role = new List<string>();
            list.login.Add("admin");
            list.password.Add("admin");
            list.role.Add("1");
            list.login.Add("wnuczek");
            list.password.Add("wnuczek");
            list.role.Add("2");

            //To do - Pobieranie danych z bazy
            //for (int i = 0; i < db.Users.Select(a => a.UserId).Max(); i++)
            //{
            //    if (db.Users.Where(a => a.UserId == i).Select(a => a).Count() == 1)
            //    {
            //        list.login.Add(db.Users.Where(a => a.UserId == i).Select(a => a.Login).ToString());
            //        list.password.Add(db.Users.Where(a => a.UserId == i).Select(a => a.Password).ToString());
            //        list.role.Add(db.Users.Where(a => a.UserId == i).Select(a => a.RoleId).ToString());
            //    }

            //}
            return View(list);
        }
        //[HttpPost]
        //public ActionResult Index()
        //{
        //    string log = Request["log"].ToString();
        //    string pas = Resquest["pas"].ToString();
        //    return Content()
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Index(string log, string pas)
        //{
            
        //    return View(objUser);
        //}
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