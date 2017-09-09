using System.Collections.Generic;

namespace Moosetrail.Playgrounds.DataClasses
{
    public class Playground
    {
        public IEnumerable<PlaygroundInfo> Information { get; private set; }
    }
}