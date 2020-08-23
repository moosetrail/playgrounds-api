using System.Collections.Generic;
using System.Threading.Tasks;
using Moosetrail.Playgrounds.Dataclasses;

namespace Moosetrail.Playgrounds.WebScrapers.Sweden
{
    public class GothenburgScraper
    {
        private static string entryPoint =
            "https://goteborg.se/wps/portal/start/kultur-och-fritid/fritid-och-natur/parker-lekplatser/lekplatser/hitta-lekplatser";

        public GothenburgScraper()
        {

        }

        public Task<IEnumerable<Playground>> GetPlaygrounds()
        {

        }
    }
}