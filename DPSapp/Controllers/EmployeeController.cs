using DPSapp.DAL;
using DPSapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Net;
using System.Dynamic;

namespace DPSapp.Controllers
{
    public class EmployeeController : Controller
    {
        private DPSContext db = new DPSContext();
        // GET: Employee
       
        public ActionResult Index()
        {

            if (Session["role"]!=null)
            {
                if (Session["role"].ToString() == "1")
                {
                    var users = from u in db.Users
                                   select u;
                    return View(users.ToList());
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
        public ActionResult Patient()
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
        public ActionResult Tag()
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

        public ActionResult UserManagement()
        {
            if (Session["role"] != null)
            {
                if (Session["role"].ToString() == "1")
                {
                    var patients = from s in db.Patients
                                   select s;
                    ViewBag.PatientID = ToSelectListID(patients.ToList<Patient>());
                    ViewBag.Patient = patients.ToList(); 
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
        [ValidateAntiForgeryToken]
        public ActionResult UserManagement([Bind(Include ="Name, Role, Surname, PatientId")] UserManager userManager)
        {
            if (ModelState.IsValid)
            {
                string name = userManager.Name;
                string last = userManager.Surname;
                bool isFamily = userManager.Role;
               // int Pacjentid = userManager.PatientId;
                int role = 1;
                int patientID = 0;
                if (isFamily)
                {
                    role = 2;
                    patientID = int.Parse(userManager.PatientId);
                }
                string pass = System.Web.Security.Membership.GeneratePassword(12,2);
                string firstPart = name.Substring(0, 3);
                string secondPart = last.Substring(0, 2);
                Random r = new Random();
                bool isFree = false;
                string login = "";
                while (isFree==false)
                {
                    string thirdPart = r.Next(100, 1000).ToString();
                    login = string.Concat(firstPart, secondPart, thirdPart);
                    //To do - wprowadzić warunek braku loginu w bazie
                    if (true)
                    {
                        isFree = true;
                        TempData["login"] = login;
                        TempData["pass"] = pass;
                    }
                }


                User user = new User { Login = login, Password = pass, RoleId = role, PatientID =  patientID};
                //if (isFamily)
                //{
                //    user.PatientID= userManager.PatientId;
                //    User user = new User { Login = login, Password = pass, RoleId = role, PatientID =  pa};
                //}
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("LoginInfo");

            }

            return View(userManager);
        }
        public ActionResult LoginInfo()
        {

            if (Session["role"] != null)
            {
                if (Session["role"].ToString() == "1")
                {
                    LogInfo info = new LogInfo();
                    info.login = TempData["login"].ToString();
                    info.pass = TempData["pass"].ToString();
                    return View(info);
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
        public SelectList ToSelectListID(List<Patient> patients)
        {
            List<SelectListItem> list = new List<SelectListItem>();

            foreach (Patient patient in patients)
            {
                var Temp = patient.PatientId.ToString();
                list.Add(new SelectListItem()
                {
                    //Value = "",
                    //Text = patient.PatientId.ToString(),
                    Text = patient.PatientName.ToString(),
                    Value = patient.PatientId.ToString()
                });
            }
            SelectList list2 = new SelectList(list, "Value", "Text");
            return list2;
        }

    }

}