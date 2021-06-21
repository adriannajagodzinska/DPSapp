using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DPSapp.Controllers
{
    public class FamilyController : Controller
    {
        // GET: Family
        public ActionResult Index()
        {

            if (Session["role"] != null)
            {
                if (Session["role"].ToString() == "2")
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
    }
}