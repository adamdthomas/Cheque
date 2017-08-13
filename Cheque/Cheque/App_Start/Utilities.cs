using HouseFly.Context;
using HouseFly.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HouseFly.Controllers;
using System.Drawing.Imaging;
using System.Drawing;
using nQuant;

namespace HouseFly.App_Start
{
    public static class Utilities
    {

        private static TempContext db;
        public static string backgroundProcessIsRunning = "";
        private static bool isRunning = false;
        public static int runCount = 0;
        public static int errorCount = 0;
        private static Thread bp;
        private static int pause = 500000;
        public static Dictionary<string, string> config = GetConfigData();


        public static string GetURL(string domain)
        {
            string url = "";
            switch (domain.ToUpper())
            {
                case "GARAGEBENCH":
                    url = @"http://192.168.1.18/";
                    break;
                case "PORCH":
                    url = @"http://192.168.1.20/";
                    break;
                case "GARAGEDOOR":
                    url = @"http://192.168.1.19/";
                    break;
                default:
                    break;
            }

            return url;
        }


        //public static void backgroundProcess(bool shouldRun)
        //{
        //    if (shouldRun)
        //    {
        //        backgroundProcessIsRunning = "True";
        //        isRunning = true;
        //        try
        //        {
        //            bp = new Thread(backgroundProcess);
        //            bp.Start();
        //        }
        //        catch (Exception)
        //        {
        //        }
        //    }
        //    else
        //    {
        //        isRunning = false;
        //        backgroundProcessIsRunning = "True";
        //    }
        //}

        public static string uniqueID()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmss");
        }

        public static void DeleteFile(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public static void SaveImage(string filename, ImageFormat format, string url, bool compress)
        {
            //<img src="@ViewBag.cam2URL:@ViewBag.cam2Port/CGIProxy.fcgi?cmd=snapPicture2&usr=@ViewBag.cam2User&pwd=@ViewBag.cam2Pass&t=" onload='setTimeout(function() {src = src.substring(0, (src.lastIndexOf("t=")+2))+(new Date()).getTime()}, 1000)' onerror='setTimeout(function() {src = src.substring(0, (src.lastIndexOf("t=")+2))+(new Date()).getTime()}, 5000)' img style='height: 100%; width: 100%; object-fit: contain' alt='' />
            try
            {
                DeleteFile(filename);

                WebClient client = new WebClient();
                Stream stream = client.OpenRead(url);
                Bitmap bitmap = new Bitmap(stream);


                if (compress)
                {
                    ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);
                    Encoder myEncoder = Encoder.Quality;
                    EncoderParameters myEncoderParameters = new EncoderParameters(1);
                    EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 50L);
                    myEncoderParameters.Param[0] = myEncoderParameter;
                    bitmap.Save(filename, jpgEncoder, myEncoderParameters);
                }
                else
                {
                    if (bitmap != null)
                        bitmap.Save(filename, format);
                }

                stream.Flush();
                stream.Close();
                client.Dispose();
            }
            catch (Exception e)
            {

              
            }
 

        }

        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }


        //static void backgroundProcess()
        //{
        //    Thread.CurrentThread.IsBackground = true;
        //    while (isRunning)
        //    {
        //        string r = rest(garageBenchURL + "update");
        //        Dictionary<string, string> JD = JsonConvert.DeserializeObject<Dictionary<string, string>>(r);
        //        TempModels tempModel = new TempModels();
                        

        //        tempModel.Domain = "Garage";
        //        tempModel.Humidity = decimal.Parse(JD["Humidity"]);
        //        tempModel.Temp = decimal.Parse(JD["Temp"]);
        //        tempModel.Pressure = decimal.Parse(JD["Pressure"]);
        //        tempModel.TimeStamp = DateTime.Now.ToString();

        //        TempModelsController tmc = new TempModelsController();
        //        tmc.CreateInternal(tempModel);
                


        //        Thread.Sleep(pause);
        //    }
        //}

        public static string rest(string url) {
 
            string r = "";
            bool ok = false;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream resStream = response.GetResponseStream();

                StreamReader reader = new StreamReader(resStream);

                r = reader.ReadToEnd();
                ok = true;
                runCount++;
            }
            catch (Exception)
            {
                errorCount++;
                ok = false;
            }

            return r;
        }
        public static Dictionary<string,string> GetConfigData()
                {
                    Dictionary<string, string> dicConfig = new Dictionary<string, string>();
                    System.Collections.Generic.IEnumerable<String> lines = File.ReadLines(@"C:\FTP\Config\camcreds.txt");

                    foreach (var item in lines)
                    {
                        string[] KeyPair = item.Split('=');
                        dicConfig.Add(KeyPair[0], KeyPair[1]);
                    }

                    return dicConfig;
                }

        public static int ToSec(string milliseconds)
        {
            int iMillis = int.Parse(milliseconds);
            int secLeft = iMillis / 1000;
            if (secLeft < 0 )
            {
                secLeft = 0;
            }
            return secLeft;
        }

        public static string MilisToClock(string milliseconds)
        {
            int iMillis = int.Parse(milliseconds);
            int hourLeft = iMillis / 3600000;
            int minLeft = iMillis / 60000;
            int secLeft = iMillis / 1000;
            secLeft = secLeft - (minLeft * 60);
            minLeft = minLeft - (hourLeft * 60);
            string sSecLeft = secLeft.ToString();
            string sMinLeft = minLeft.ToString();
            string sHourLeft = hourLeft.ToString();

            if (sSecLeft.Length < 2)
            {
                sSecLeft = "0" + sSecLeft;
            }

            if (sMinLeft.Length < 2)
            {
                sMinLeft = "0" + sMinLeft;
            }

            if (sHourLeft.Length < 2)
            {
                sHourLeft = "0" + sHourLeft;
            }

            string tl = sHourLeft + ":" + sMinLeft + ":" + sSecLeft;

          

            return tl;
        }
    }
}