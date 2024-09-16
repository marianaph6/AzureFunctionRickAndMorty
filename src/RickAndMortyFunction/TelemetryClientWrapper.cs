using System.Collections.Generic;
using System;
using Microsoft.ApplicationInsights;

namespace RickAndMortyFunction
{
    public class TelemetryClientWrapper : ITelemetryClient
    {
        private readonly TelemetryClient _telemetryClient;

        public TelemetryClientWrapper(TelemetryClient telemetryClient)
        {
            _telemetryClient = telemetryClient;
        }

        public void TrackEvent(string name, IDictionary<string, string> properties = null)
        {
            _telemetryClient.TrackEvent(name, properties);
        }

        public void TrackDependency(string type, string name, string data, DateTime offset, TimeSpan duration, bool success)
        {
            _telemetryClient.TrackDependency(type, name, data, offset, duration, success);
        }

        public void TrackException(Exception exception)
        {
            _telemetryClient.TrackException(exception);
        }
    }
}
