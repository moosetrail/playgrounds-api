using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Moosetrail.Playgrounds.Dataclasses;

namespace Moosetrail.Playgrounds.WebScrapers.Sweden
{
    public class StockholmScraper : WebScraperBase
    {
        public StockholmScraper()
        {

        }

        protected override string EntryPoint()
        {
            return "https://parker.stockholm/hitta-lekplatser-parklekar-plaskdammar/";
        }

        protected override string BaseUrl()
        {
            return "https://parker.stockholm";
        }

        protected override IEnumerable<string> GetAllPlaygroundLinksOnPage(HtmlDocument page)
        {
            var parks = page.DocumentNode.Descendants().Where(x => x.HasClass("card"));
            var parkLinks = parks.Select(x =>
                x.Descendants().Single(n => n.Name == "a").GetAttributeValue("href", ""));
            return parkLinks;
        }

        protected override bool GetNextLink(HtmlNode x)
        {
            return x.HasClass("paging-item--next");
        }

        protected override Playground GetPlayground(HtmlDocument page, string playgroundLink)
        {
            throw new NotImplementedException();
        }
    }
}