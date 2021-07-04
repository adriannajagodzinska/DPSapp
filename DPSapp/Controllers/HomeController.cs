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
        //public ActionResult Index()
        //{
        //    //Role role = new Role { RoleId = 2, RoleName = "Rodzina" };
        //    //db.Roles.Add(role);
        //    //db.SaveChanges();
        //    //User user = new User { Login = "admin", Password = "admin", RoleId = 1 };
        //    //db.Users.Add(user);
        //    //db.SaveChanges();
        //    //   Patient patient = new Patient { PatientName = "Karolina", PatientSurname = "Cicha" };
        //    //    db.Patients.Add(patient);
        //    //   db.SaveChanges();
        //    ListOfUsers list = new ListOfUsers();

        //    list.login = new List<string>();
        //    list.password = new List<string>();
        //    list.role = new List<string>();
        //    list.login.Add("admin");
        //    list.password.Add("admin");
        //    list.role.Add("1");
        //    list.login.Add("wnuczek");
        //    list.password.Add("wnuczek");
        //    list.role.Add("2");

        //    //To do - Pobieranie danych z bazy
        //    //for (int i = 1; i <= db.Users.Select(a => a.UserId).Max(); i++)
        //    //{
        //    //    if (db.Users.Where(a => a.UserId == i).Select(a => a).Count() == 1)
        //    //    {
        //    //        list.login.Add(db.Users.Where(a => a.UserId == i).Select(a => a.Login).ToString());
        //    //        list.password.Add(db.Users.Where(a => a.UserId == i).Select(a => a.Password).ToString());
        //    //        list.role.Add(db.Users.Where(a => a.UserId == i).Select(a => a.RoleId).ToString());
        //    //    }

        //    //}
        //    return View(list);
        //}
        //public ActionResult Index()
        //{
        //    return View();
        //}
        
        public ActionResult Index([Bind] LoginAssistant assistant)
        {
            //Role role = new Role { RoleId = 1, RoleName = "Administrator" };
            //db.Roles.Add(role);
            //db.SaveChanges();
            //User user = new User { Login = "admin", Password = "admin", RoleId = 1 };
            //db.Users.Add(user);
            //db.SaveChanges();

            if (db.Users.Where(a => a.Login == assistant.login).Select(a => a).Count() > 0)
            {

                if (db.Users.Where(a => a.Login == assistant.login).Select(a => a.Password).FirstOrDefault().ToString() == assistant.password)
                {
                    Session["UserName"] = assistant.login;
                    if (db.Users.Where(a => a.Login == assistant.login).Select(a => a.RoleId).FirstOrDefault().ToString() == "1")
                    {
                        Session["Role"] = "1";
                        return RedirectToAction("Index", "Employee");
                    }
                    else
                    {
                        Session["Role"] = "2";
                        return RedirectToAction("Index", "Family");
                    }
                }
                else
                {
                    ModelState.AddModelError("password", "Podane hasło jest niepoprawne");
                }
            }
            else
            {
                ModelState.AddModelError("login", "Użytkownik o podanym loginie nie istnieje w bazie danych");
            }

            return View();
        }

        public ActionResult Wyloguj()
        {
            Session["Role"] = null;

            return RedirectToAction("Index", "Home");
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Error401()
        {
            return View();
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}