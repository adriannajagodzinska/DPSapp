using DPSapp.DAL;
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

        private DPSContext _db = new DPSContext();

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
                                            "~/Images\\3.bmp",
            "~/Images\\4.jpg"};
            // ""
            ListOfAdresses.ListOfAdresses = list;

            return View(ListOfAdresses);
        }

        public ActionResult ChooseRoomSite()
        {
            List<Room> rooms = new List<Room>();
            Room room1 = new Room();
            room1.RoomNumber = 1;
            Room room2 = new Room();
            room2.RoomNumber = 2;
            rooms.Add(room1);
            rooms.Add(room2);
            //_db.Rooms.Add(room1);
            //_db.SaveChanges();
            return View(rooms);
        }

    }
}