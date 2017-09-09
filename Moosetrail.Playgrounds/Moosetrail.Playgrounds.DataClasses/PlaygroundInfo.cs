using System;

namespace Moosetrail.Playgrounds.DataClasses
{
    public interface PlaygroundInfo
    {
        bool IsScraped { get; }
        string AddedBy { get; }
        DateTime Added { get; }
    }
}