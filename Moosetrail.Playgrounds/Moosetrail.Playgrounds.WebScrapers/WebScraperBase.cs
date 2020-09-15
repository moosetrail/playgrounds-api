using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using HtmlAgilityPack;
using Moosetrail.Playgrounds.Dataclasses;

namespace Moosetrail.Playgrounds.WebScrapers
{
    public abstract class WebScraperBase : WebScraper
    {
        protected abstract string EntryPoint();

        protected abstract string BaseUrl();

        protected static HtmlDocument GetDocumentFromServer(string url)
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


        public IEnumerable<Tuple<Playground, ScrapeData>> GetPlaygrounds()
        {
            var playgroundLinks = GetAllPlaygroundLinks();

            var playgroundList = new List<Tuple<Playground, ScrapeData>>();

            foreach (var playgroundLink in playgroundLinks)
            {
                var page = GetDocumentFromServer(playgroundLink);

                var playground = GetPlayground(page, playgroundLink);
                var scrapeData = new ScrapeData(playgroundLink, page.DocumentNode.OuterHtml);
                playgroundList.Add(new Tuple<Playground, ScrapeData>(playground, scrapeData));
            }

            return playgroundList;
        }

        protected IEnumerable<string> GetAllPlaygroundLinks()
        {
            var url = EntryPoint();
            var playgroundLinks = new List<string>();

            while (url != null)
            {
                var results = GetListOfUrlsToPlaygrounds(url);
                playgroundLinks.AddRange(results.Item1.Select(x => BaseUrl() + x));

                if (results.Item2 != null)
                    url = BaseUrl() + results.Item2;
                else
                    url = null;
            }

            return playgroundLinks;
        }

        protected Tuple<IEnumerable<string>, string> GetListOfUrlsToPlaygrounds(string url)
        {
            var page = GetDocumentFromServer(url);

            var playgroundLinks = GetAllPlaygroundLinksOnPage(page);

            var nextLink = page.DocumentNode.Descendants().SingleOrDefault(GetNextLink);

            if (nextLink != null)
            {
                var nextPage = nextLink.Name == "a"
                    ? nextLink.GetAttributeValue("href", null)
                    : nextLink.Descendants().Single(x => x.Name == "a").GetAttributeValue("href", null);
                return new Tuple<IEnumerable<string>, string>(playgroundLinks, nextPage);
            }

            return new Tuple<IEnumerable<string>, string>(playgroundLinks, null);
        }

        protected abstract IEnumerable<string> GetAllPlaygroundLinksOnPage(HtmlDocument page); 

        protected abstract bool GetNextLink(HtmlNode x);

        protected abstract Playground GetPlayground(HtmlDocument page, string playgroundLink);
    }
}