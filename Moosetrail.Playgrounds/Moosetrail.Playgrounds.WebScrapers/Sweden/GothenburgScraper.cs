using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Moosetrail.Playgrounds.Dataclasses;

namespace Moosetrail.Playgrounds.WebScrapers.Sweden
{
    public class GothenburgScraper
    {
        private static string entryPoint =
            "https://goteborg.se/wps/portal/start/kultur-och-fritid/fritid-och-natur/parker-lekplatser/lekplatser/hitta-lekplatser";

        private static string baseUrl = "https://goteborg.se";

        public GothenburgScraper()
        {

        }

        public async Task<IEnumerable<Tuple<Playground, ScrapeData>>> GetPlaygrounds()
        {
            var url = entryPoint;
            var playgroundLinks = new List<string>();

            while (url != null)
            {
                var results = GetListOfUrlsToPlaygrounds(url);
                playgroundLinks.AddRange(results.Item1.Select(x => baseUrl + x));

                if (results.Item2 != null)
                    url = baseUrl + results.Item2;
                else
                    url = null;
            }

            var playgroundList = new List<Tuple<Playground, ScrapeData>>();

            foreach (var playgroundLink in playgroundLinks)
            {
                var page = getDocumentFromServer(playgroundLink);

                var infoCode = page.DocumentNode.Descendants().FirstOrDefault(x => x.InnerText.Contains("coordinates") && !x.HasChildNodes);
                var coordMatch = Regex.Match(infoCode.InnerText, @"\[(\d*.\d*),(\d*.\d*)\]");
                var name = Regex.Match(infoCode.InnerText, "name\":\"(.*?)\"");

                var info = page.DocumentNode.Descendants().SingleOrDefault(x =>
                    x.GetAttributeValue("xmlns:gbg", "") == "http://teik.goteborg.se/components" && x.Name == "p");

                var playground = new Playground(name.Groups[1].Value, info.InnerText, Convert.ToDouble(coordMatch.Groups[1].Value, System.Globalization.CultureInfo.InvariantCulture), Convert.ToDouble(coordMatch.Groups[2].Value, System.Globalization.CultureInfo.InvariantCulture), playgroundLink, GetTags(page));
                var scrapeData = new ScrapeData(playgroundLink, page.DocumentNode.OuterHtml);
                playgroundList.Add(new Tuple<Playground, ScrapeData>(playground, scrapeData));
            }

            return playgroundList;
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

        private static Tuple<IEnumerable<string>, string> GetListOfUrlsToPlaygrounds(string url)
        {
            var page = getDocumentFromServer(url);

            var playGroundList =
                page.DocumentNode.Descendants().Where(x => x.HasClass("c-list") && x.HasClass("c-list--large"));

            var playgroundLinks = playGroundList.First().Descendants().Where(x => x.Name == "a")
                .Select(x => x.GetAttributeValue("href", ""));

            var nextLink = page.DocumentNode.Descendants().SingleOrDefault(x => x.HasClass("c-pagination__next"));

            if (nextLink != null)
            {
                var nextPage = nextLink.Descendants().Single(x => x.Name == "a").GetAttributeValue("href", "");
                return new Tuple<IEnumerable<string>, string>(playgroundLinks, nextPage);
            }
            
            return new Tuple<IEnumerable<string>, string>(playgroundLinks, null);
        }

        private static HtmlDocument getDocumentFromServer(string url)
        {
            var request = WebRequest.Create(url);
            var response = request.GetResponse();
            using var dataStream = response.GetResponseStream();
            var reader = new StreamReader(dataStream);
            var responseFromServer = reader.ReadToEnd();

            var page = new HtmlDocument();
            page.LoadHtml(responseFromServer);
            return page;
        }
    }
}