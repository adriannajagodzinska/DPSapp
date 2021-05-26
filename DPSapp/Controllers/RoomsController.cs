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


        public ActionResult Slider(int? id)
        {
            
            ListOfAdressessToMedia ListOfAdresses = new ListOfAdressessToMedia();



            var tags = from tag in _db.Tags
                       where tag.Rooms.Any(c => c.RoomNumber == id)
                       select tag;
            List<Tag> tagi = tags.ToList();
            //var tags = _db.Rooms.Where(c => c.RoomId == id).SelectMany(c => c.Tags);
            //var tags = (from t in _db.Rooms
            //                  where t.RoomNumber == id
            //                  select (from tag in _db.Tags
            //                          where t.Tags.
            //                          select tag.TagName
            //var libelles = _db.Rooms
            //            .Include(e => )
            //          .Single(e => e.EtudiantId == 1) // will throw exception if entity not found
            //          .cours.Select(c => c.libelle);
            //                          )).ToList();
            //List<string> tags = tagstemp.
            List<string> adresses = new List<string>();
            foreach (var tag in tagi)
            {
                //var temp = _db.Messages.Where(c => c.Tags == item).Select(c => c.Image);
               
                var temp = (from m in _db.Messages
                            where m.Tags.Any(c => c.TagId == tag.TagId)
                            select m);

                List<Message> wiadomosci = temp.ToList();
                foreach (var messaage in temp)
                {
                    adresses.Add(messaage.Image);
                }
            }
           ;

            //List<string> list = new List<string>{"~/Images\\1.jpg",
            //                                "~/Images\\2.jpg",
            //                                "~/Images\\3.bmp",
            //"~/Images\\4.mp4"};
            // ""
            ListOfAdresses.ListOfAdresses = adresses;

            return View(ListOfAdresses);
        }

        public ActionResult ChooseRoomSite()
        {
            var rooms = (from r in _db.Rooms
                         select r).ToList();
            //List<Room> rooms = new List<Room>();
            //Room room1 = new Room();
            //room1.RoomNumber = 1;
            //Room room2 = new Room();
            //room2.RoomNumber = 2;
            //rooms.Add(room1);
            //rooms.Add(room2);


            //Tag pokoj1 = new Tag();
            //Tag pokoj2 = new Tag();
            //pokoj1.TagName = "pokoj1";
            //pokoj2.TagName = "pokoj2";


            //room1.Tags = new List<Tag>();
            //room2.Tags = new List<Tag>();
            //room1.Tags.Add(pokoj1);
            //room2.Tags.Add(pokoj2);

            //Message wiad1 = new Message();
            //wiad1.Tags = new List<Tag>();
            //wiad1.Tags.Add(pokoj1);

            //Message wiad2 = new Message();
            //wiad2.Tags = new List<Tag>();
            //wiad2.Tags.Add(pokoj2);

            //wiad1.Image = "~/Images\\1.jpg";
            //wiad2.Image = "~/Images\\4.mp4";
            //_db.Tags.Add(pokoj1);
            //_db.Tags.Add(pokoj2);
            //_db.Messages.Add(wiad1);
            //_db.Messages.Add(wiad2);
            //_db.Rooms.Add(room1);
            //_db.Rooms.Add(room2);



            // _db.Rooms.Add(room2);
            //bool a = _db.ChangeTracker.HasChanges();
            //_db.SaveChanges();
            return View(rooms);
        }

    }
}