using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace DPSapp.Controllers
{
    public class UploadFilesController : Controller
    {
        // GET: UploadFiles
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file)
        {
            try
            {
                if(file.ContentLength>0)
                {
                    string filename = Path.GetFileName(file.FileName);
                    string filepath = Path.Combine(Server.MapPath("~/FilesUpload"),filename);
                    file.SaveAs(filepath);
                }
                ViewBag.Message = "Plik poprawnie załadowany!";
                return View();
            }
            catch (Exception)
            {
                ViewBag.Message = "Plik nie załadował się!";
                return View();
            }
            
        }
    }
}