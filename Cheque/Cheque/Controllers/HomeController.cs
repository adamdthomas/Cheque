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
using Newtonsoft.Json;
using Cheque.App_Start;

namespace Cheque.Controllers
{
    public class HomeController : Controller
    {
        private string garageBenchURL = @"http://192.168.1.18/";
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Video()
        {
            Dictionary<string, string> vidDictionary = Utilities.GetConfigData();

            ViewBag.URL = vidDictionary["cam1url"];
            ViewBag.Port = vidDictionary["cam1port"];
            ViewBag.User = vidDictionary["cam1user"];
            ViewBag.Pass = vidDictionary["cam1pass"];
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

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }

        public ActionResult Dashboard()
        {
            rest(garageBenchURL + "Update");
            ViewBag.Message = "";
            return View();
        }

        [HttpPost]
        public ActionResult Dashboard(string min)
        {
            rest(garageBenchURL + "addmin!" + min + "!");
            ViewBag.Message = min + " Minutes Added";
            return RedirectToAction("Dashboard");

        }

        public ActionResult AddTime()
        {
            rest(garageBenchURL + "H");
            ViewBag.Message = "15 Minutes Added";

            return RedirectToAction("Dashboard");
        }

        public ActionResult RemoveTime()
        {
            rest(garageBenchURL + "L");
            ViewBag.Message = "Time Reset";

            return RedirectToAction("Dashboard");
        }

        public ActionResult Update()
        {
            rest(garageBenchURL + "Update");
            return RedirectToAction("Dashboard");
        }

        private void rest(string uri)
        {
            string r = "";
            bool ok = false;

            if (Request.IsAuthenticated)
            {
                if (User.Identity.Name == "adamdthomas@gmail.com")
                {
                    string url = uri;
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    try
                    {
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        Stream resStream = response.GetResponseStream();

                        StreamReader reader = new StreamReader(resStream);

                        r = reader.ReadToEnd();
                        ok = true;
                    }
                    catch (Exception)
                    {

                        ok = false;
                    }


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

            if (ok)
            {
                Dictionary<string, string> JD = JsonConvert.DeserializeObject<Dictionary<string, string>>(r);

                ViewBag.Temp = JD["Temp"];
                ViewBag.Humidity = JD["Humidity"];
                ViewBag.Pressure = JD["Pressure"];
                ViewBag.Hour = JD["HourRemaining"];
                ViewBag.Min = JD["MinutesRemaining"];
                ViewBag.Sec = JD["SecondsRemaining"];
                ViewBag.RelayOne = JD["RelayOneStatus"];
            }


        }
    }
}