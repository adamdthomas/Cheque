using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using HouseFly.App_Start;
using System.Drawing;
using System.Drawing.Imaging;
using static HouseFly.App_Start.Utilities;

namespace HouseFly.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Users = "adamdthomas@gmail.com")]
        public ActionResult Video()
        {
            return View();
        }

        //[Authorize(Users ="adamdthomas@gmail.com")]
        //[HttpPost]
        //public ActionResult Video(string str)
        //{
        //    return View();
        //}

        [Authorize(Users = "adamdthomas@gmail.com")]
        public ActionResult controlcam(string d, string camName)
        {
            //Dictionary<string, string> vidDictionary = Utilities.GetConfigData();

            rest(config[camName + "url"] + ":" + config[camName + "port"] + @"/CGIProxy.fcgi?cmd=" + d + "&usr=" + config[camName + "user"]  + "&pwd=" + config[camName + "pass"]);
            return RedirectToAction("Video");
        }

        [Authorize(Users = "adamdthomas@gmail.com")]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        [Authorize(Users = "adamdthomas@gmail.com")]
        public ActionResult Sandbox()
        {
            //ViewBag.isRunning = Utilities.backgroundProcessIsRunning;
            //ViewBag.runCount = Utilities.runCount.ToString();
            //ViewBag.errorCount = Utilities.errorCount.ToString();
            return View();
        }

        //[Authorize(Users = "adamdthomas@gmail.com")]
        //public ActionResult SandBoxHandler(string command, string arg)
        //{
        //    try
        //    {
        //        switch (command.ToUpper())
        //        {
        //            case "RUNCHECKER":
        //                if (arg.Equals("TRUE"))
        //                {
        //                    Utilities.backgroundProcess(true);
        //                }
        //                else
        //                {
        //                    Utilities.backgroundProcess(false);
        //                }
        //                break;
        //            case "GENERAL":
        //                if (arg.Equals("TRUE"))
        //                {
        //                    Utilities.runCount = 0;
        //                    Utilities.errorCount = 0;
        //                }
        //                    break;
        //            default:
        //                break;
        //        }
        //    }
        //    catch (Exception)
        //    {

        //    }
   
        //    return RedirectToAction("Sandbox", View());
        //}

        public ActionResult Time(int? type)
        {
            return Json(new { time = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss")}, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Users = "adamdthomas@gmail.com")]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }

        [Authorize(Users = "adamdthomas@gmail.com")]
        public ActionResult Dashboard()
        {
           // rest("Update", "GarageBench");
           // rest("Update", "GarageDoor");
            //ViewBag.Message = "";
            return View();
        }

        //[Authorize(Users = "adamdthomas@gmail.com")]
        //[HttpPost]
        //public ActionResult Dashboard(string min, string relay, string domain)
        //{
        //    switch (relay.ToLower())
        //    {
        //        case "openon":
        //            break;
        //        case "closeoff":
        //            break;
        //        default:
        //            try
        //            {
        //                min = (int.Parse(min) * 60000).ToString();
        //                rest("time!" + min + "!" + relay + "!", "GarageBench");
        //                ViewBag.Message = min + " Minutes Added";
        //            }
        //            catch (Exception) { }
        //            break;
        //    }


        //    return RedirectToAction("Dashboard");
        //}

        [Authorize(Users = "adamdthomas@gmail.com")]
        public ActionResult Porch()
        {
           // rest("Update", "Porch");
            //ViewBag.Message = "";
            return View();
        }

        //[Authorize(Users = "adamdthomas@gmail.com")]
        //[HttpPost]
        //public ActionResult Porch(string min, string relay, string domain)
        //{
        //    try
        //    {
        //        min = (int.Parse(min) * 60000).ToString();
        //        rest("time!" + min + "!" + relay + "!", domain);
        //        ViewBag.Message = min + " Minutes Added";

        //    }
        //    catch (Exception)
        //    {

        //    }

        //    return RedirectToAction("Porch");
        //}


        //[Authorize(Users = "adamdthomas@gmail.com")]
        //public ActionResult HandleTime(string relay, string time, string domain)
        //{
        //    string redir = "Dashboard";
        //    try
        //    {
        //        time = (int.Parse(time) * 60000).ToString();
        //        redir = rest("time!" + time + "!" + relay + "!", domain);
        //        ViewBag.Message = time + " Minutes Added";
        //        System.Threading.Thread.Sleep(1000);
        //    }
        //    catch (Exception)
        //    {

        //    }

        //    return RedirectToAction(redir);
        //}



        [Authorize(Users = "adamdthomas@gmail.com")]
        public ActionResult CallController(string command, string domain)
        {
            string r = Utilities.rest(Utilities.GetURL(domain) + command);
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Users = "adamdthomas@gmail.com")]
        public ActionResult GetWeather()
        {
            string r = Utilities.rest(@"http://api.openweathermap.org/data/2.5/weather?zip=" + Utilities.config["weatherzip"] + @",us&units=imperial&APPID=" + Utilities.config["weathertoken"]);
            return Json(r, JsonRequestBehavior.AllowGet);
        }

       


        //[Authorize(Users = "adamdthomas@gmail.com")]
        //public ActionResult HandleDoor(string Direction, string Notes)
        //{
        //    try
        //    {
        //        switch (Direction.ToUpper())
        //        {
        //            case "OPEN":
        //                rest("GarageDoor/Open", "GarageDoor");
        //                break;
        //            case "CLOSE":
        //                rest("GarageDoor/Close", "GarageDoor");
        //                break;
        //            default:
        //                break;
        //        }
        //    }
        //    catch (Exception)
        //    {

        //    }

        //    return RedirectToAction("Dashboard");
        //}


        //[Authorize(Users = "adamdthomas@gmail.com")]
        //public ActionResult Update()
        //{
        //    rest("Update", "GarageBench");
        //    rest("Update", "GarageDoor");
        //    return RedirectToAction("Dashboard");
        //}

        //[Authorize(Users = "adamdthomas@gmail.com")]
        //public ActionResult UpdatePorch()
        //{
        //    rest("Update", "Porch");
        //    return RedirectToAction("Porch");
        //}

        //[Authorize(Users = "adamdthomas@gmail.com")]
        //private string rest(string uri, string domain)
        //{
        //    Dictionary<string, string> JD = null;

        //    string pageRedirect = "";

        //    try
        //    {
        //        string r = "";
        //        switch (domain.ToUpper())
        //        {
        //            case "GARAGEBENCH":
        //                r = Utilities.rest(Utilities.GetURL("GarageBench") + uri);
        //                JD = JsonConvert.DeserializeObject<Dictionary<string, string>>(r);
        //                ViewBag.Temp = JD["Temp"];
        //                ViewBag.Humidity = JD["Humidity"];
        //                ViewBag.Pressure = JD["Pressure"];
        //                ViewBag.Hour = JD["HourRemaining"];
        //                ViewBag.Min = JD["MinutesRemaining"];
        //                ViewBag.Sec = JD["SecondsRemaining"];
        //                ViewBag.rt1 = Utilities.ToSec(JD["r1t"]);
        //                ViewBag.rt2 = Utilities.ToSec(JD["r2t"]);
        //                ViewBag.rt3 = Utilities.ToSec(JD["r3t"]);
        //                ViewBag.rt4 = Utilities.ToSec(JD["r4t"]);
        //                pageRedirect = "Dashboard";
        //                break;
        //            case "PORCH":
        //                r = Utilities.rest(Utilities.GetURL("PorchLights") + uri);
        //                JD = JsonConvert.DeserializeObject<Dictionary<string, string>>(r);
        //                ViewBag.rpt1 = Utilities.ToSec(JD["r1t"]);
        //                ViewBag.rpt2 = Utilities.ToSec(JD["r2t"]);
        //                pageRedirect = "Porch";
        //                break;
        //            case "GARAGEDOOR":
        //                r = Utilities.rest(Utilities.GetURL("GarageDoor") + uri);
        //                JD = JsonConvert.DeserializeObject<Dictionary<string, string>>(r);
        //                string doorStat;
        //                if (JD["DoorOpened"].ToUpper() == "TRUE")
        //                {
        //                    doorStat = "Opened";
        //                }
        //                else
        //                {
        //                    doorStat = "Closed";
        //                }

        //                ViewBag.GarageDoor = doorStat;
        //                ViewBag.GarageDoorNotes = JD["Notes"];
        //                pageRedirect = "Dashboard";
        //                break;
        //            default:
        //                r = Utilities.rest(uri);
        //                pageRedirect = "Dashboard";
        //                break;
        //        }
        //    }
        //    catch (Exception)
        //    {

        //    }
        //    return pageRedirect;
        //}

        [Authorize(Users = "adamdthomas@gmail.com")]
        public ActionResult BabyCam()
        {
            //Dictionary<string, string> vidDictionary = Utilities.GetConfigData();
            string url = config["cam1url"] + ":" + config["cam1port"] + @"/CGIProxy.fcgi?cmd=snapPicture2&usr=" + config["cam1user"] + "&pwd=" + config["cam1pass"];
            var dir = Server.MapPath("/Images");
            //var path = Path.Combine(dir, "campic.png");
            string path = @"C:\Temp\campicb.png";
            Utilities.SaveImage(path, ImageFormat.Png, url, false);
            return base.File(path, "image/jpeg");
        }

        [Authorize(Users = "adamdthomas@gmail.com")]
        public ActionResult GarageCam()
        {
            //Dictionary<string, string> vidDictionary = Utilities.GetConfigData();
            string url = config["cam2url"] + ":" + config["cam2port"] + @"/CGIProxy.fcgi?cmd=snapPicture2&usr=" + config["cam2user"] + "&pwd=" + config["cam2pass"];
            var dir = Server.MapPath("/Images");
            // var path = Path.Combine(dir, "campic.png");

            string path = @"C:\Temp\campic.png";

            SaveImage(path, ImageFormat.Png, url, false);
            return base.File(path, "image/jpeg");
        }

        [Authorize(Users = "adamdthomas@gmail.com")]
        public ActionResult GarageCamJPG()
        {
            //Dictionary<string, string> vidDictionary = Utilities.GetConfigData();
            string url = config["cam2url"] + ":" + config["cam2port"] + @"/CGIProxy.fcgi?cmd=snapPicture2&usr=" + config["cam2user"] + "&pwd=" + config["cam2pass"];
            var dir = Server.MapPath("/Images");
            // var path = Path.Combine(dir, "campic.png");

            string path = @"C:\Temp\campic.jpg";

            Utilities.SaveImage(path, ImageFormat.Png, url, true);
            return File(path, "image/jpeg");
        }




    }
}