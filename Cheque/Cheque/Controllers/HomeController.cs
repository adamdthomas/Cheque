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

            string url = "http://192.168.1.18/Update";
            string responseText;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream resStream = response.GetResponseStream();

            StreamReader reader = new StreamReader(resStream);

            responseText = reader.ReadToEnd();

            return View(responseText);
        }


        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}