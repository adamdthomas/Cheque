using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace HouseFly.App_Start
{
    public static class Utilities
    {
        public static Dictionary<string,string> GetConfigData()
        {
            Dictionary<string, string> dicConfig = new Dictionary<string, string>();
            System.Collections.Generic.IEnumerable<String> lines = File.ReadLines(@"C:\FTP\www\config.txt");

            foreach (var item in lines)
            {
                string[] KeyPair = item.Split('=');
                dicConfig.Add(KeyPair[0], KeyPair[1]);
 

            }

            return dicConfig;
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