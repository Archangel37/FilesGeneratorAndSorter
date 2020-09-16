using System;
using System.Threading.Tasks;
using FileSorter.Models;
using Newtonsoft.Json;
using Serilog;

namespace FileSorter.Implementations
{
    public static class KafkaMessageHandler
    {
        public static async Task NotifyFromKafkaMessage(string message, ILogger logger, MainConfig cfg)
        {
            Log.Information($"Got Message From Kafka: {message}");
            try
            {
                await SortWorker.ProcessSorting(cfg,
                    JsonConvert.DeserializeObject<KafkaMessage>(message), Log.Logger);
            }
            catch (Exception e)
            {
                logger.Error($"Can't Process Sorting:\n{e.Message}");
                throw;
            }
        }
    }
}