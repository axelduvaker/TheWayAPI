using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TheWay.Logic;
using System.Diagnostics;

namespace TheWay.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        [HttpGet("{url}/{word}")]
        public string WordCountOnAftonbladet(string url, string word)
        {
            string sourceCode = WebScraperLogic.getSourceCode(url);
            int count = WebScraperLogic.countWord(sourceCode, word);
            return "Ordet " + word + " förekommer " + count + " gånger på " + url;
        }
    }
}
