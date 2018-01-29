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
        public static List<string> queueList = new List<string>();
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

        private static void addToQueue(string url)
        {
            //lägger till urlen till en kö
            //Det borde egentligen vara ett unikt ID, så man kan köra flera kontroller på samma sida
            if (!queueList.Contains(url))
            {
                queueList.Add(url);
            }
            else
            {
                Console.WriteLine(url + " Fanns redan i kön");
            }
            
        }
        private static bool queueStatus(string url)
        {
            //kollar om urlen är först i kön
            if (url.Equals(queueList.First(), StringComparison.InvariantCultureIgnoreCase))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private static void removeFromQueue(string url)
        {
            if(queueList.First().Equals(url, StringComparison.InvariantCultureIgnoreCase))
            {
                Console.WriteLine(url+" Är nu borttagen från kön!");
                queueList.Remove(url);
                if(queueList.Count > 0)
                    Console.WriteLine("Först i kön är nu: "+queueList.First());
            }
        }

            public static string GetSourceCode(string url)
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
            bool returnVal;
            Uri uri = new Uri(url);
            WebRequest request = WebRequest.Create(uri);
            request.Method = "HEAD";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            try
            {
                if (response == null || response.StatusCode != HttpStatusCode.OK)
                {
                    Console.WriteLine("The page: '" + url + "' is down");
                    returnVal = false;
                }
                else
                {
                    Console.WriteLine("The page: '" + url + "' is up!");
                    returnVal = true;
                }
            }
            catch
            {
                Console.WriteLine("The page: '" + url + "' is down or doesn't exist!");
                returnVal = false;
            }

            response.Close();
            return returnVal;
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
        }

        //metoden tar in urlen (färdigfixad), söksträngen och sedan intervallen i SEKUNDER
        public static async Task CheckPageWithIntervalAsync(string url, string word, int interval)
        {
            // intervallerna konverteras till millisekunder för delayen
            int intervalInMs = interval * 1000;
            addToQueue(url);
            while (true)
            {
                
                if (queueStatus(url))
                {
                    Console.WriteLine(WordCountOnPage(url, word));
                    PrintToLog(url, word);
                    removeFromQueue(url);

                    await Task.Delay(intervalInMs);

                    addToQueue(url);

                    Console.WriteLine(url + " har lagts till sist i kön");
                }
                else
                {
                    Console.WriteLine("väntar 2 sekund, kön är upptagen!");
                    await Task.Delay(2000);
                }
               
            }
        }

        public static async Task CheckPageWithIntervalAsync(string url, int interval)
        {
            while (true)
            {
                if (IsThePageAlive(url))
                {
                    Console.WriteLine(url + " is OK");
                }
                else
                {
                    Console.WriteLine(url + " is DEAD");
                }
                // intervallerna konverteras till millisekunder för delayen
                int intervalInMs = interval * 1000;
                await Task.Delay(intervalInMs);
            }
        }

        public static string WordCountOnPage(string url, string word)
        {
            int count = 0;
            string sourceCode = WebScraperLogic.GetSourceCode(url);
            if (sourceCode != null)
            {
                foreach (Match match in Regex.Matches(sourceCode, word, RegexOptions.IgnoreCase))
                {
                    count++;
                }
                return count.ToString();
            }
            else
            {
                return null;
            }
        }

        public static void PrintToLog(string url, string word)
        {
            string count = WordCountOnPage(url, word);
            String timeStamp = DateTime.Now.ToString();

            using (StreamWriter writer = new StreamWriter("logFile.txt", true))
            {
                writer.WriteLine(timeStamp + " - Söksträngen: '" + word + "' förekommer " + count + " gånger på " + url);
            }
        }
    }
}

