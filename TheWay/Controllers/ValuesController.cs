using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TheWay.Logic;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace TheWay.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {

        // GET api/values/word/page
        
        [HttpGet("{url}/{word}/{interval}")]
        public string WordCountOnPageAsync(string url, string word, int interval)
        {
            string validUrl = WebScraperLogic.UrlHttpFix(url);
            if (WebScraperLogic.CheckURLValid(validUrl))
            {
                //anropar intervallanropen i "bakgrunden"
                WebScraperLogic.CheckPageWithIntervalAsync(url, word, interval);
                string sourceCode = WebScraperLogic.getSourceCode(validUrl);
                int count = WebScraperLogic.countWord(sourceCode, word);
                return "Söksträngen: '" + word + "' förekommer " + count + " gånger på " + url;
            }
            else
            {
                return "Urlen: '" + url + "' är felaktig, försök igen med ex: aftonbladet.se eller www.google.com";
            }
        }
    }
}
