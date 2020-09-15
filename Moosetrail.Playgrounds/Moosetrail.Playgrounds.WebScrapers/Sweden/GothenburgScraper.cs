using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using Moosetrail.Playgrounds.Dataclasses;

namespace Moosetrail.Playgrounds.WebScrapers.Sweden
{
    public class GothenburgScraper : WebScraperBase
    {
        public GothenburgScraper()
        {

        }

        protected override string EntryPoint()
        {
            return "https://goteborg.se/wps/portal/start/kultur-och-fritid/fritid-och-natur/parker-lekplatser/lekplatser/hitta-lekplatser"; ;
        }

        protected override string BaseUrl()
        {
            return "https://goteborg.se";
        }

        protected override IEnumerable<string> GetAllPlaygroundLinksOnPage(HtmlDocument page)
        {
            var playGroundList =
                page.DocumentNode.Descendants().Where(x => x.HasClass("c-list") && x.HasClass("c-list--large"));

            var playgroundLinks = playGroundList.First().Descendants().Where(x => x.Name == "a")
                .Select(x => x.GetAttributeValue("href", ""));
            return playgroundLinks;
        }

        protected override bool GetNextLink(HtmlNode x)
        {
            return x.HasClass("c-pagination__next");
        }

        protected override Playground GetPlayground(HtmlDocument page, string playgroundLink)
        {
            var infoCode = page.DocumentNode.Descendants()
                .FirstOrDefault(x => x.InnerText.Contains("coordinates") && !x.HasChildNodes);
            var coordMatch = Regex.Match(infoCode.InnerText, @"\[(\d*.\d*),(\d*.\d*)\]");
            var name = Regex.Match(infoCode.InnerText, "name\":\"(.*?)\"");

            var info = page.DocumentNode.Descendants().SingleOrDefault(x =>
                x.GetAttributeValue("xmlns:gbg", "") == "http://teik.goteborg.se/components" && x.Name == "p");

            var playground = new Playground(name.Groups[1].Value, info.InnerText,
                Convert.ToDouble(coordMatch.Groups[1].Value, System.Globalization.CultureInfo.InvariantCulture),
                Convert.ToDouble(coordMatch.Groups[2].Value, System.Globalization.CultureInfo.InvariantCulture), playgroundLink,
                GetTags(page));
            return playground;
        }

        private static string[] GetTags(HtmlDocument page)
        {
            var tags = page.DocumentNode.Descendants().SingleOrDefault(x => x.HasClass("c-heading__byline"));

            if (tags != null)
            {
                return tags.InnerText.Replace("\n", " ").Replace(", ", " ")
                    .Split(" ", StringSplitOptions.RemoveEmptyEntries);
            }

            return new string[0];
        }
    }
}