using System;
using System.Collections.Generic;
using Moosetrail.Playgrounds.Dataclasses;

namespace Moosetrail.Playgrounds.WebScrapers
{
    public interface WebScraper
    {
        IEnumerable<Tuple<Playground, ScrapeData>> GetPlaygrounds();
    }
}