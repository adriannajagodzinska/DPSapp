using DPSapp.DAL;
using DPSapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Net;

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
                    return View();
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }

            }
            else
            {
                return RedirectToAction("Index", "Home");
            }



        }
        public ActionResult Patient()
        {
            return View();
        }
        public ActionResult Tag()
        {
            return View();
        }

        public ActionResult UserManagement()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserManagement([Bind(Include ="Name, Role, Surname")] UserManager userManager)
        {
            if (ModelState.IsValid)
            {
                string name = userManager.Name;
                string last = userManager.Surname;
                bool isFamily = userManager.Role;
                int role = 1;
                if (isFamily)
                {
                    role = 2;
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
                

                User user = new User { Login = login, Password = pass, RoleId = role };
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("LoginInfo");

            }

            return View(userManager);
        }
        public ActionResult LoginInfo()
        {
            LogInfo info = new LogInfo();
            info.login = TempData["login"].ToString();
            info.pass = TempData["pass"].ToString();
            return View(info);
        }
    }

}