using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TheWay.Logic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Net;
using System.Web.Http.Cors;

namespace TheWay.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        public IActionResult Index()
        {
            var file = ApplicationSettings.env.ContentRootPath + $"{Path.DirectorySeparatorChar}wwwroot{Path.DirectorySeparatorChar}index.html";
            return PhysicalFile(file, "text/html");
        }

        // GET api/values/word/page
        [HttpGet("{url}/{word}/{interval}")]
        public string WordCountOnPageAsync(string url, string word, int interval)
        {
            string validUrl = WebScraperLogic.UrlHttpFix(url);
            WebScraperLogic.FillList();
            if (WebScraperLogic.CheckURLValid(validUrl))
            {
                //anropar intervallanropen i "bakgrunden"
                WebScraperLogic.CheckPageWithIntervalAsync(validUrl, word, interval);
                string sourceCode = WebScraperLogic.GetSourceCode(validUrl);
                string count = WebScraperLogic.WordCountOnPage(validUrl, word);

                if(count != null) {
                    return "Söksträngen: '" + word + "' förekommer " + count + " gånger på " + validUrl;
                }
                else {
                    return validUrl + " verkar inte finnas, eller så är sidan nere!";
                }
            }
            else
            {
                return "Urlen: '" + url + "' är felaktig, försök igen med ex: aftonbladet.se eller www.google.com";
            }
        }
        [HttpGet("health/{url}/{interval}")]
        public string PageHealthAnalysisAsync(string url, int interval)
        {
            string validUrl = WebScraperLogic.UrlHttpFix(url);
            WebScraperLogic.FillList();
            if (WebScraperLogic.CheckURLValid(validUrl))
            {
                WebScraperLogic.CheckPageWithIntervalAsync(validUrl, interval);
                if (WebScraperLogic.IsThePageAlive(validUrl))
                {
                    return validUrl + ": Alive";
                }
                else
                {
                    return validUrl + ": Dead";
                }

            }
            else
            {
                return "Urlen: '" + url + "' är felaktig, försök igen med ex: aftonbladet.se eller www.google.com";
            }
        }
    }
}
