using System;
using Moosetrail.Playgrounds.WebScrapers.Sweden;

namespace Moosetrail.Playgrounds.WebScrapers.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var scraper = new GothenburgScraper();

            System.Console.WriteLine("Welcome to the scraper for Playgrounds in Sweden!");
            System.Console.WriteLine("Now using {0} to get data", scraper.GetType());

            var playgrounds = scraper.GetPlaygrounds().Result;
        }
    }
}
