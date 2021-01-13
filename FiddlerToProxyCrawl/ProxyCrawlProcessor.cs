using System;
using System.Net;

namespace FiddlerToProxyCrawl
{
    public class ProxyCrawlProcessor
    {
        private Random _random = new Random();
        private string _keepOpenSessionName = "";
        private bool _keepOpenSession = false;
        public bool KeepOpenSession
        {
            get => _keepOpenSession;
            set
            {
                _keepOpenSession = value;
                if (_keepOpenSession)
                    _keepOpenSessionName = "proxySessionName" + _random.Next(100000);
            }
        }
        private string _keepOpenCookieSessionName = "";
        private bool _keepOpenCookieSession = false;
        public bool KeepOpenCookieSession
        {
            get => _keepOpenCookieSession;
            set
            {
                _keepOpenCookieSession = value;
                if (_keepOpenCookieSession)
                    _keepOpenCookieSessionName = "cookieSessionName" + _random.Next(100000);
            }
        }

        //Javascript token
        private const string JavascriptToken = "<TOKEN>";
        private const string NormalToken = "<TOKEN>";


        internal void CrawlFile(HttpFile httpFile, FileCrawlOptions options = null)
        {
            if (options == null)
                options = new FileCrawlOptions();

            var url = ConstructUrl(httpFile, options);
            try
            {
                Console.WriteLine("Executing Proxy crawl url:");
                Console.WriteLine(url);
                var result = new WebClient().DownloadString(url);
                Console.WriteLine("First part of the result:");
                Console.WriteLine("______________________________________________________________________________");
                var length = 500;
                if (result.Length < length)
                    length = result.Length;
                Console.WriteLine(result.Substring(0, length));
                Console.WriteLine("______________________________________________________________________________");
            } catch(Exception e)
            {
                Console.WriteLine($"Something BAD went wrong: {e.Message}");
            }
        }

        private string ConstructUrl(HttpFile httpFile, FileCrawlOptions options)
        {
            var urlToScrape = System.Net.WebUtility.UrlDecode(httpFile.Url);
            var url = $"https://api.proxycrawl.com/";
            url += $"?token={(options.UseJavascriptToken ? JavascriptToken : NormalToken)}";
            url += $"&url={urlToScrape}";
            url += $"&device=mobile";
            url += $"&ajax_wait=true";
            url += $"&country=NL";

            if (options.AjaxWait)
                url += $"&ajax_wait={options.AjaxWait.ToString().ToLower()}";

            if (!string.IsNullOrEmpty(options.CssSelector))
                url += $"&css_click_selector={options.CssSelector}";

            if (KeepOpenSession)
                url += $"&proxy_session={_keepOpenSessionName}";
            if (KeepOpenCookieSession)
                url += $"&cookies_session={_keepOpenCookieSessionName}";

            return url;
        }
    }
}
