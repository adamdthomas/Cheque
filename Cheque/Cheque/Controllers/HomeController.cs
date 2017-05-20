﻿using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using HouseFly.App_Start;

namespace HouseFly.Controllers
{
    public class HomeController : Controller
    {
        private string garageBenchURL = @"http://192.168.1.18/";
        private string garageDoorURL = @"http://192.168.1.19/";

        //private string garageBenchURL = @"http://10.238.243.174/";
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Video()
        {
            Dictionary<string, string> vidDictionary = Utilities.GetConfigData();

            ViewBag.cam1URL = vidDictionary["cam1url"];
            ViewBag.cam1Port = vidDictionary["cam1port"];
            ViewBag.cam1User = vidDictionary["cam1user"];
            ViewBag.cam1Pass = vidDictionary["cam1pass"];

            ViewBag.cam2URL = vidDictionary["cam2url"];
            ViewBag.cam2Port = vidDictionary["cam2port"];
            ViewBag.cam2User = vidDictionary["cam2user"];
            ViewBag.cam2Pass = vidDictionary["cam2pass"];
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

        [Authorize(Users = "adamdthomas@gmail.com")]
        public ActionResult controlcam(string d, string camName)
        {
            Dictionary<string, string> vidDictionary = Utilities.GetConfigData();

            rest(vidDictionary[camName + "url"] + ":" + vidDictionary[camName + "port"] + @"/CGIProxy.fcgi?cmd=" + d + "&usr=" + vidDictionary[camName + "user"]  + "&pwd=" + vidDictionary[camName + "pass"], "none");
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
            ViewBag.timeone = 12;
            return View();
        }

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

            Dictionary<string, string> vidDictionary = Utilities.GetConfigData();

            ViewBag.cam1URL = vidDictionary["cam1url"];
            ViewBag.cam1Port = vidDictionary["cam1port"];
            ViewBag.cam1User = vidDictionary["cam1user"];
            ViewBag.cam1Pass = vidDictionary["cam1pass"];

            ViewBag.cam2URL = vidDictionary["cam2url"];
            ViewBag.cam2Port = vidDictionary["cam2port"];
            ViewBag.cam2User = vidDictionary["cam2user"];
            ViewBag.cam2Pass = vidDictionary["cam2pass"];

            rest(garageBenchURL + "Update", "GarageBench");
            rest(garageDoorURL + "Update", "GarageDoor");
            ViewBag.Message = "";
            return View();
        }

        [Authorize(Users = "adamdthomas@gmail.com")]
        [HttpPost]
        public ActionResult Dashboard(string min, string relay)
        {
            try
            {
                min = (int.Parse(min) * 60000).ToString();
                rest(garageBenchURL + "time!" + min + "!" + relay + "!", "GarageBench");
                ViewBag.Message = min + " Minutes Added";
               
            }
            catch (Exception)
            {

            }

            return RedirectToAction("Dashboard");
        }

        [Authorize(Users = "adamdthomas@gmail.com")]
        public ActionResult HandleTime(string relay, string time)
        {
            try
            {
                time = (int.Parse(time) * 60000).ToString();
                rest(garageBenchURL + "time!" + time + "!" + relay + "!", "GarageBench");
                ViewBag.Message = time + " Minutes Added";
                System.Threading.Thread.Sleep(1000);
            }
            catch (Exception)
            {

            }

            return RedirectToAction("Dashboard");
        }

        [Authorize(Users = "adamdthomas@gmail.com")]
        public ActionResult HandleDoor(string Direction, string Notes)
        {
            try
            {
                switch (Direction.ToUpper())
                {
                    case "OPEN":
                        rest(garageBenchURL + "GarageDoor/Open", "GarageDoor");
                        break;
                    case "CLOSE":
                        rest(garageBenchURL + "GarageDoor/Close", "GarageDoor");
                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {

            }

            return RedirectToAction("Dashboard");
        }


        [Authorize(Users = "adamdthomas@gmail.com")]
        public ActionResult Update()
        {
            rest(garageBenchURL + "Update", "GarageBench");
            rest(garageDoorURL + "Update", "GarageDoor");
            return RedirectToAction("Dashboard");
        }

        [Authorize(Users = "adamdthomas@gmail.com")]
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
                    case "GARAGEBENCH":
                        Dictionary<string, string> JD = JsonConvert.DeserializeObject<Dictionary<string, string>>(r);

                        ViewBag.Temp = JD["Temp"];
                        ViewBag.Humidity = JD["Humidity"];
                        ViewBag.Pressure = JD["Pressure"];
                        ViewBag.Hour = JD["HourRemaining"];
                        ViewBag.Min = JD["MinutesRemaining"];
                        ViewBag.Sec = JD["SecondsRemaining"];

                        ViewBag.rt1 = Utilities.ToSec(JD["r1t"]);
                        ViewBag.rt2 = Utilities.ToSec(JD["r2t"]);
                        ViewBag.rt3 = Utilities.ToSec(JD["r3t"]);
                        ViewBag.rt4 = Utilities.ToSec(JD["r4t"]);

                        break;
                    case "GARAGEDOOR":
                        Dictionary<string, string> JDD = JsonConvert.DeserializeObject<Dictionary<string, string>>(r);
                        string doorStat;
                        if (JDD["DoorOpened"].ToUpper() == "TRUE")
                        {
                            doorStat = "Opened";
                        }
                        else
                        {
                            doorStat = "Closed";
                        }

                        ViewBag.GarageDoor = doorStat;
                        ViewBag.GarageDoorNotes = JDD["Notes"];
                        break;
                    default:
                        break;
                }
               
            }


        }
    }
}