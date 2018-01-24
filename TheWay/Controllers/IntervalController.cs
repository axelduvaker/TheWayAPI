using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using TheWay.Logic;

namespace TheWay.Controllers
{
    public class IntervalController : Controller
    {
        int wordCount = 0;

        public async Task CheckAftonbladetBoy(TimeSpan interval, CancellationToken cancellationToken)
        {
            while (true)
            {
                interval = TimeSpan.FromSeconds(10);
                await WordCountOnPage("www.aftonbladet.se","trumP");
                await Task.Delay(interval, cancellationToken);
            }
        }
        
        private async Task<int> WordCountOnPage(string url, string word)
        {
            string validUrl = WebScraperLogic.UrlHttpFix(url);
            if (WebScraperLogic.CheckURLValid(validUrl))
            {
                string sourceCode = WebScraperLogic.getSourceCode(validUrl);
                int count = WebScraperLogic.countWord(sourceCode, word);
                wordCount = count;
                return count;
            }
            else
            {
                wordCount = 1337;
                return 0;
            }
        }
    }
}