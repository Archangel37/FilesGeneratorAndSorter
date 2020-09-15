using Serilog.Core;
using Serilog.Events;

namespace FileGenerator
{
    public class UtcTimestampEnricher : ILogEventEnricher
    {
        /// <inheritdoc />
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory) =>
            logEvent.AddPropertyIfAbsent(
                propertyFactory.CreateProperty(
                    "UtcTimestamp",
                    logEvent.Timestamp.UtcDateTime));
    }
}