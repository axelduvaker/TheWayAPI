using System;
using System.IO;
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
        public IActionResult Index()
        {
            var file = ApplicationSettings.env.ContentRootPath + $"{Path.DirectorySeparatorChar}wwwroot{Path.DirectorySeparatorChar}index.html";
            return PhysicalFile(file, "text/html");
        }

        // GET api/values/word/page
        [HttpGet("{url}/{word}")]
        public string WordCountOnPage(string url, string word)
        {
            string validUrl = WebScraperLogic.UrlHttpFix(url);
            if (WebScraperLogic.CheckURLValid(validUrl))
            {
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
