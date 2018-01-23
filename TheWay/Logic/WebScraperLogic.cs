using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TheWay.Logic
{
    public class WebScraperLogic
    {
        public static string getSourceCode(string url)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            StreamReader sr = new StreamReader(resp.GetResponseStream());
            string sourceCode = sr.ReadToEnd();
            sr.Close();
            resp.Close();
            return sourceCode;
        }

        public static int countWord(string sourceCode, string word)
        {
            int count = 0;

            foreach(Match match in Regex.Matches(sourceCode, word))
            {
                count++;
            }

            return count;
        }
    }
}
