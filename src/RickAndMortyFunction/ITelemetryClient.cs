using System.Collections.Generic;
using System;

namespace RickAndMortyFunction
{
    public interface ITelemetryClient
    {
        void TrackEvent(string name, IDictionary<string, string> properties = null);
        void TrackDependency(string type, string name, string data, DateTime offset, TimeSpan duration, bool success);
        void TrackException(Exception exception);
    }
}
