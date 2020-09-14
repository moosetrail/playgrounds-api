using System;

namespace Moosetrail.Playgrounds.Dataclasses
{
    public class ScrapeData
    {
        public ScrapeData(string url, string code)
        {
            Url = url;
            Code = code; 
            DateStamp = DateTime.Now;
        }

        public string Url { get; set; }

        public DateTime DateStamp { get; set; }

        public string Code { get; set; }
    }
}