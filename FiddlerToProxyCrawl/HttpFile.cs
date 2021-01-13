namespace FiddlerToProxyCrawl
{
    public class HttpFile
    {
        public virtual string Host { get; set; } 
        public virtual string Connection { get; set; }
        public virtual string UserAgent { get; set; }
        public virtual string Accept { get; set; }
        public virtual string AcceptLanguage { get; set; }
        public virtual string Url { get; set; }
        public virtual string Method { get; set; }
        public virtual string Referer { get; set; }
        public virtual string Origin { get; set; }
    }
}
