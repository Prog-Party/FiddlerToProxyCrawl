namespace FiddlerToProxyCrawl
{
    internal class FileCrawlOptions
    {
        public string CssSelector { get; set; } = null;
        public bool UseJavascriptToken { get; set; } = false;
        public bool AjaxWait { get; set; } = false;
    }
}