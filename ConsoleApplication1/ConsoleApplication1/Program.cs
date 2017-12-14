using AngleSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        private static IEnumerable<string> uris;

        static void Main(string[] args)
        {
            try
            {
                ProcessImagesAsync().Wait();
            }
            catch (AggregateException e)
            {

                Console.WriteLine(e.Flatten().ToString());
            }


            int i = 0;            

            foreach (var uri in uris)
            {
                Console.WriteLine(uri);
                GetImageAsync(uri, Directory.GetCurrentDirectory().ToString() + "\\img\\cat" + i.ToString() + ".jpg");
                i++;
            }



            Console.ReadLine();
        }

        static async Task ProcessImagesAsync()
        {
            // Setup the configuration to support document loading
            var config = Configuration.Default.WithDefaultLoader();
            
            var address = "https://imgur.com";
            // Asynchronously get the document in a new context using the configuration
            var document = await BrowsingContext.New(config).OpenAsync(address);
            // This CSS selector gets the desired content
            var imgSelector = "img";
            // Perform the query to get all imgs with the content
            uris = document.QuerySelectorAll(imgSelector).Select(item => item.GetAttribute("src")).Take(5).ToList() ;   
            
        }

        static async Task GetImageAsync(string uri, string filename)
        {
            WebClient webClient = new WebClient();
            try
            {
                await webClient.DownloadFileTaskAsync("https:" + uri, filename);
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
        }

    }
}
