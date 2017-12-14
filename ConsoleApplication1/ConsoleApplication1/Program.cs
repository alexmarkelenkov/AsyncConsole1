using AngleSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        private static IEnumerable<string> imgs;

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
            imgs = document.QuerySelectorAll(imgSelector).Select(item => item.GetAttribute("src")).ToList();            

            foreach (var image in imgs)
            {
                Console.WriteLine(image);
            }
        }

        static async Task GetImageAsync(Uri uri, WebClient webClient)
        {

        }

    }
}
