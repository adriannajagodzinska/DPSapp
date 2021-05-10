using DPSapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DPSapp.Controllers
{
    public class RoomsController : Controller
    {
        


        // GET: Rooms
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Slider()
        {
            ListOfAdressessToMedia ListOfAdresses = new ListOfAdressessToMedia();
            
            List<string> list = new List<string>{"~/Images\\1.jpg",
                                            "~/Images\\2.jpg",
                                            "~/Images\\3.bmp"};
            // ""
            ListOfAdresses.ListOfAdresses = list;

            return View(ListOfAdresses);
        }

    }
}