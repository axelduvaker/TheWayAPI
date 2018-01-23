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
        // GET api/values/word
        [HttpGet("{word}")]
        public string Get(string word)
        {
            string uri = "https://www.aftonbladet.se/";
            string sourceCode = WebScraperLogic.getSourceCode(uri);
            int count = WebScraperLogic.countWord(sourceCode, word);
            return "Ordet " + word + " förekommer " + count + " gånger på Aftonbladet";
        }
    }
}
