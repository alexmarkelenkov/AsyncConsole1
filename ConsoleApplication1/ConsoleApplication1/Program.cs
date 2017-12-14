using AngleSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                Console.WriteLine("AggregateException:\n" + e.Flatten().ToString());
            }


            try
            {
                int i = 0;
                List<Task> imageSaveTasks = new List<Task>();
                Task t;

                var sw = new Stopwatch();
                sw.Start();
                foreach (var uri in uris)
                {
                    Console.WriteLine(uri);

                    //WebClient webClient = new WebClient();
                    //try
                    //{
                    //    webClient.DownloadFile(uri, Directory.GetCurrentDirectory().ToString() + "\\img\\cat" + i.ToString() + ".jpg");
                    //}
                    //catch (Exception e)
                    //{

                    //    Console.WriteLine(e.Message);
                    //}

                    t = GetImageAsync(uri, Directory.GetCurrentDirectory().ToString() + "\\img\\cat" + i.ToString() + ".jpg");
                    imageSaveTasks.Add(t);

                    i++;
                }
                Task.WaitAll(imageSaveTasks.ToArray());
                sw.Stop();
                Console.WriteLine(sw.Elapsed.TotalMilliseconds);
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }


            

            Console.ReadLine();
        }

        static async Task ProcessImagesAsync()
        {    
            var config = Configuration.Default.WithDefaultLoader();
            var address = CatServer.Cutespaw;
            var document = await BrowsingContext.New(config).OpenAsync(address);            
            var imgSelector = CatServer.Selectors[address];             
            uris = document.QuerySelectorAll(imgSelector).Select(item => item.GetAttribute("src")).Take(50).ToList() ;   
            
        }

        static async Task GetImageAsync(string uri, string filename)
        {
            WebClient webClient = new WebClient();
            try
            {
                await webClient.DownloadFileTaskAsync(uri, filename);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        static void DetectCat(string image)
        {
            OpenCvSharp.CascadeClassifier cc = new OpenCvSharp.CascadeClassifier("haarcascade_frontalcatface.xml");
            var img = new OpenCvSharp.Mat(image);
            var img2 = new OpenCvSharp.Mat();
            img.ConvertTo(img2, OpenCvSharp.MatType.CV_8U);
            var cats = cc.DetectMultiScale(img2);
            Console.WriteLine(cats.Length);
        }

    }
}
