using System.Collections.Generic;

namespace Moosetrail.Playgrounds.Dataclasses
{
    public class Playground
    {
        public Playground(string name, string officialText = null, double latitude = 0, double longitude = 0,
            string website = null, params string[] tags)
        {
            Name = name;
            OfficialText = officialText;
            Latitude = latitude;
            Longitude = longitude;
            Website = website;
            Tags = tags; 
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public string OfficialText { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public string Website { get; set; }

        public IEnumerable<string> Tags { get; set; }
    }
}