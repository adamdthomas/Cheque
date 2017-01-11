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
    }
}