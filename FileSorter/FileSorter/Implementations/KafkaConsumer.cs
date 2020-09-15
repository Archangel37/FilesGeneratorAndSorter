using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using FileSorter.Models;
using Serilog;

namespace FileSorter.Implementations
{
    public static class KafkaConsumer
    {
        public static async Task ConsumeMessage(MainConfig cfg, CancellationToken cancellationToken, ILogger logger)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = cfg.Kafka.BootstrapServers,
                GroupId = cfg.Kafka.GroupId,
                ClientId = cfg.Kafka.ClientId
            };

            using (var consumer = new ConsumerBuilder<Ignore, string>(config).Build())
            {
                consumer.Subscribe(cfg.Kafka.Topic);

                while (!cancellationToken.IsCancellationRequested)
                    try
                    {
                        var consumeResult = consumer.Consume(cancellationToken);
                        if (consumeResult.Message != null)
                        {
                            //todo warning! "Application maximum poll interval (300000ms) exceeded by X ms"
                            //https://github.com/confluentinc/confluent-kafka-dotnet/issues/785
                            var message = consumeResult.Message.Value;
                            await KafkaMessageHandler.NotifyFromKafkaMessage(message, logger, cfg);
                        }
                    }
                    catch (ConsumeException e)
                    {
                        logger.Information($"Error occured while consuming: {e.Error.Reason}", e);
                        //throw;
                    }

                consumer.Close();
            }
        }
    }
}