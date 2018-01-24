using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace TheWay.Logic
{
    public static class WebScraperLogic
    {
        public static List<string> domains = new List<string>();
        public static void FillList()
        {
            domains.Add(".com");
            domains.Add(".se");
            domains.Add(".nu");
            domains.Add(".co.uk");
            domains.Add(".org");
            domains.Add(".biz");
            domains.Add(".edu");
            domains.Add(".gov");
            domains.Add(".net");
            domains.Add(".eu");
            domains.Add(".ca");
            domains.Add(".de");
            domains.Add(".info");
            domains.Add(".us");
        }
        public static string getSourceCode(string url)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            try
            {
                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                StreamReader sr = new StreamReader(resp.GetResponseStream());
                string sourceCode = sr.ReadToEnd();
                sr.Close();
                resp.Close();
                return sourceCode;
            }
            catch
            {
                return null;
            }
            
        }
        public static bool IsThePageAlive(string url)
        {
            Uri uri = new Uri(url);
            WebRequest request = WebRequest.Create(uri);
            request.Method = "HEAD";
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                if (response == null || response.StatusCode != HttpStatusCode.OK)
                {
                    return false;
                    Debug.WriteLine("The page: '" + url + "' is down");
                }
                else
                {
                    return true;
                    Debug.WriteLine("The page: '" + url + "' is up!");
                }
                response.Close();
            }
            catch
            {
                return false;
                Debug.WriteLine("The page: '" + url + "' is down or doesn't exist!");
            }
            
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
                        string newUrl = "http://" + url;
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
            if (!source.Contains(".."))
            {
                foreach (string domain in domains)
                {
                    if (source.EndsWith(domain))
                    {
                        return true;
                    }

                }
                return false;
            }
            return false;

            //if (source.Split('.').Length >= 3)
            //{
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
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


        //metoden tar in urlen (färdigfixad), söksträngen och sedan intervallen i SEKUNDER
        public static async Task CheckPageWithIntervalAsync(string url, string word, int interval)
        {
            while (true)
            {
                Debug.WriteLine(WordCountOnPage(url, word));
                // intervallerna konverteras till millisekunder för delayen
                int intervalInMs = interval * 1000;
                await Task.Delay(intervalInMs);
            }
        }
        public static async Task CheckPageWithIntervalAsync(string url, int interval)
        {
            while (true)
            {
                if (IsThePageAlive(url))
                {
                    Debug.WriteLine(url + " is OK");
                }
                else
                {
                    Debug.WriteLine(url + " is DEAD");
                }
                // intervallerna konverteras till millisekunder för delayen
                int intervalInMs = interval * 1000;
                await Task.Delay(intervalInMs);
            }
        }

        private static string WordCountOnPage(string url, string word)
        {
            string sourceCode = WebScraperLogic.getSourceCode(url);
            if(sourceCode != null)
            {
                int count = WebScraperLogic.countWord(sourceCode, word);
                return count.ToString();
            }
            else
            {
                return null;
            }

        }

        class MyClient : WebClient
        {
            public bool HeadOnly { get; set; }
            protected override WebRequest GetWebRequest(Uri address)
            {
                WebRequest req = base.GetWebRequest(address);
                if (HeadOnly && req.Method == "GET")
                {
                    req.Method = "HEAD";
                }
                return req;
            }
        }
    }
}

