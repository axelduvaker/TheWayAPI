using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TheWay.Logic
{
    public static class WebScraperLogic
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
        public static string UrlHttpFix(string url)
        {
            try
            {
                Match httpwwwmatch = Regex.Match(url, "^(http|https)://www.", RegexOptions.IgnoreCase);
                if (httpwwwmatch.Success)
                {
                    return url;
                }
                else
                {
                    Match wwwmatch = Regex.Match(url, "^www.", RegexOptions.IgnoreCase);
                    if (wwwmatch.Success)
                    {
                        string newUrl = "http://" + url;
                        return newUrl;
                    }
                    else
                    {
                        string newUrl = "http://www." + url;
                        return newUrl;

                    }
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }


        }

        public static bool CheckURLValid(this string source)
        {


            if (source.Split('.').Length >= 3)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static int countWord(string sourceCode, string word)
        {
            int count = 0;

            //string webpage = sourceCode.ToLower();
            //string words = word.ToLower();

            foreach (Match match in Regex.Matches(sourceCode, word, RegexOptions.IgnoreCase))
            {
                count++;
            }

            return count;
        }
    }
}
