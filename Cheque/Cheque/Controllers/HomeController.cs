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
using HouseFly.App_Start;

namespace HouseFly.Controllers
{
    public class HomeController : Controller
    {
        private string garageBenchURL = @"http://192.168.1.18/";
        //private string garageBenchURL = @"http://10.238.243.174/";
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

        [Authorize(Users ="adamdthomas@gmail.com")]
        [HttpPost]
        public ActionResult Video(string str)
        {
            Dictionary<string, string> vidDictionary = Utilities.GetConfigData();

            ViewBag.URL = vidDictionary["cam1url"];
            ViewBag.Port = vidDictionary["cam1port"];
            ViewBag.User = vidDictionary["cam1user"];
            ViewBag.Pass = vidDictionary["cam1pass"];
            return View();
        }


        public ActionResult controlcam(string d)
        {
            Dictionary<string, string> vidDictionary = Utilities.GetConfigData();

            rest(vidDictionary["cam1url"] + ":" + vidDictionary["cam1port"] + @"/CGIProxy.fcgi?cmd=" + d + "&usr=" + vidDictionary["cam1user"]  + "&pwd=" + vidDictionary["cam1pass"], "none");
            return RedirectToAction("Video");
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
            rest(garageBenchURL + "Update", "Garage");
            ViewBag.Message = "";
            return View();
        }

        [HttpPost]
        public ActionResult Dashboard(string min, string relay)
        {
            try
            {
                min = (int.Parse(min) * 60000).ToString();
                rest(garageBenchURL + "time!" + min + "!" + relay + "!", "Garage");
                ViewBag.Message = min + " Minutes Added";
               
            }
            catch (Exception)
            {

            }

            return RedirectToAction("Dashboard");
        }


        public ActionResult HandleTime(string relay, string time)
        {
            try
            {
                time = (int.Parse(time) * 60000).ToString();
                rest(garageBenchURL + "time!" + time + "!" + relay + "!", "Garage");
                ViewBag.Message = time + " Minutes Added";

            }
            catch (Exception)
            {

            }

            return RedirectToAction("Dashboard");
        }

        public ActionResult AddTime(string rnum)
        {
            rest(garageBenchURL + "H", "Garage");
            ViewBag.Message = "15 Minutes Added";

            return RedirectToAction("Dashboard");
        }

        public ActionResult RemoveTime()
        {
            rest(garageBenchURL + "L", "Garage");
            ViewBag.Message = "Time Reset";

            return RedirectToAction("Dashboard");
        }

        public ActionResult Update()
        {
            rest(garageBenchURL + "Update", "Garage");
            return RedirectToAction("Dashboard");
        }

        private void rest(string uri, string domain)
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

                switch (domain.ToUpper())
                {
                    case "GARAGE":
                        Dictionary<string, string> JD = JsonConvert.DeserializeObject<Dictionary<string, string>>(r);

                        ViewBag.Temp = JD["Temp"];
                        ViewBag.Humidity = JD["Humidity"];
                        ViewBag.Pressure = JD["Pressure"];
                        ViewBag.Hour = JD["HourRemaining"];
                        ViewBag.Min = JD["MinutesRemaining"];
                        ViewBag.Sec = JD["SecondsRemaining"];

                        ViewBag.r1t = Utilities.MilisToClock(JD["r1t"]);
                        ViewBag.r2t = Utilities.MilisToClock(JD["r2t"]);
                        ViewBag.r3t = Utilities.MilisToClock(JD["r3t"]);
                        ViewBag.r4t = Utilities.MilisToClock(JD["r4t"]);



                        break;
                    default:
                        break;
                }
               
            }


        }
    }
}