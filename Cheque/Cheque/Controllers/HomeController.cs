using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.IO;

namespace Cheque.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Time()
        {
            return Json(new { time = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss")}, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Shop()
        {
            string r;
           if(Request.IsAuthenticated)
           {
                if (User.Identity.Name == "adamdthomas@gmail.com")
                {
                    string url = "http://192.168.1.18/Update";
                    string responseText;

                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    Stream resStream = response.GetResponseStream();

                    StreamReader reader = new StreamReader(resStream);

                    r = reader.ReadToEnd();

                }
                else
                {
                    r = "You are unauthorized to view this service.";
                }

            }
            else
            {
                r = "You must be authenticated to view this service.";
            }
            return Content(r);

        }


        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Dashboard()
        {
            ViewBag.Message = "message from microcontroller 1";

            return View();
        }

        public ActionResult AddTime()
        {
            rest("http://192.168.1.18/H");
            ViewBag.Message = "15 Minutes Added";

            return View("Dashboard");
        }

        public ActionResult RemoveTime()
        {
            rest("http://192.168.1.18/L");
            ViewBag.Message = "Time Reset";

            return View("Dashboard");
        }

        public ActionResult Update()
        {
            rest("http://192.168.1.18/Update");
            ViewBag.Message = "Data pulled from controller";

            return View("Dashboard");
        }

        private string rest(string uri)
        {
            string r;
            if (Request.IsAuthenticated)
            {
                if (User.Identity.Name == "adamdthomas@gmail.com")
                {
                    string url = uri;
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    Stream resStream = response.GetResponseStream();

                    StreamReader reader = new StreamReader(resStream);

                    r = reader.ReadToEnd();

                }
                else
                {
                    r = "You are unauthorized to view this service.";
                }

            }
            else
            {
                r = "You must be authenticated to view this service.";
            }

            return "";
        }
    }
}