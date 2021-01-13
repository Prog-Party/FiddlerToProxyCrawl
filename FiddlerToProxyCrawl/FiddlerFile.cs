using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FiddlerToProxyCrawl
{
    public class FiddlerFile: HttpFile
    {
        private readonly string FilePath;
        private readonly List<string> FileLines;
        private Dictionary<string, string> _fileLineParts;
        private Dictionary<string, string> FileLineParts
        {
            get
            {
                if (_fileLineParts == null)
                {
                    _fileLineParts = new Dictionary<string, string>();
                    foreach (var fileLine in FileLines)
                    {
                        try
                        {
                            var httpMethods = new List<string> { "GET", "POST", "PUT", "DELETE" };
                            if(httpMethods.Any(method => fileLine.ToUpper().StartsWith(method)))
                            {
                                //process the method specific stylo
                                _fileLineParts["url"] = fileLine.Split(" ")[1];
                                _fileLineParts["method"] = fileLine.Split(" ")[0];
                            }
                            else
                            {
                                var split = fileLine.Split(":");
                                var key = split.First();
                                var value = split.Last().Trim();
                                if (key.Contains(" "))
                                    continue;

                                _fileLineParts[key.ToLower()] = value;
                            }
                        }
                        catch (Exception e)
                        {
                            //do nothing
                        }
                    }
                }
                return _fileLineParts;
            }
        }


        internal FiddlerFile(string filePath)
        {
            FilePath = filePath;
            FileLines = File.ReadLines(FilePath).ToList();
        }

        public override string Host => FileLineParts.GetValueOrDefault<string, string>("host");
        public override string Connection => FileLineParts.GetValueOrDefault<string, string>("connection");
        public override string UserAgent => FileLineParts.GetValueOrDefault<string, string>("user-agent");
        public override string Accept => FileLineParts.GetValueOrDefault<string, string>("accept");
        public override string AcceptLanguage => FileLineParts.GetValueOrDefault<string, string>("accept-language");
        public override string Url => FileLineParts.GetValueOrDefault<string, string>("url");
        public override string Method => FileLineParts.GetValueOrDefault<string, string>("method");
        public override string Referer => FileLineParts.GetValueOrDefault<string, string>("referer");
        public override string Origin => FileLineParts.GetValueOrDefault<string, string>("origin");
    }
}
