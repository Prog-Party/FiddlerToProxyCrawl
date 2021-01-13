using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace FiddlerToProxyCrawl
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = ReadFiddlerFiles();
            Console.WriteLine("");
            Console.WriteLine("");
            ExecuteHttpFiles(path);

            Console.WriteLine("");
            Console.WriteLine("Program finished running");
            Console.ReadLine();
        }

        private static string ReadFiddlerFiles()
        {
            Console.WriteLine("Read all fiddler files");
            Console.WriteLine("Input the directory path with request files");
            var path = Console.ReadLine();

            var files = Directory.GetFiles(path, "*.txt", SearchOption.AllDirectories).ToList();
            files = files.OrderBy(f => f).ToList();
            Console.WriteLine($"Found {files.Count} files");

            foreach (var file in files)
            {
                Console.WriteLine($"Processing txt fiddler file: {file}");
                var fiddlerFile = new FiddlerFile(file);
                var jsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(fiddlerFile, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
                File.WriteAllText(file + ".json", jsonResult);
                Console.WriteLine("Processing file worked");
                Console.WriteLine("");
                Console.WriteLine("");
            }

            return path;
        }

        private static void ExecuteHttpFiles(string path = null)
        {
            Console.WriteLine("Read all http files");
            if (path == null)
            {
                Console.WriteLine("Input the directory path with request files");
                path = Console.ReadLine();
            }

            var pathsToFiles = Directory.GetFiles(path, "*.json", SearchOption.AllDirectories).ToList();
            pathsToFiles = pathsToFiles.OrderBy(f => f).ToList();
            Console.WriteLine($"Found {pathsToFiles.Count} files");

            var proxyCrawlProcessor = new ProxyCrawlProcessor();
            proxyCrawlProcessor.KeepOpenSession = true;
            proxyCrawlProcessor.KeepOpenCookieSession = true;
            int counter = 1;
            foreach (var filePath in pathsToFiles)
            {
                Console.WriteLine($"Processing http file: {filePath}");
                var content = File.ReadAllText(filePath);
                var httpFile = Newtonsoft.Json.JsonConvert.DeserializeObject<HttpFile>(content);

                var options = new FileCrawlOptions() { CssSelector = "#didomi-notice-agree-button", UseJavascriptToken = true, AjaxWait = true };
                if (counter != 1) //Bij dumpert willen we bij de eerste pagina op de akkoord knop drukken
                    options = null;

                proxyCrawlProcessor.CrawlFile(httpFile, options);
                Console.WriteLine("Processing done");
                Console.WriteLine("");
                Console.WriteLine("");
                counter++;
            }
        }
    }
}
