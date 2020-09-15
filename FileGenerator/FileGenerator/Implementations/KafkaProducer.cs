using System;
using System.Threading.Tasks;
using Confluent.Kafka;
using Confluent.Kafka.Admin;
using FileGenerator.Models;
using Newtonsoft.Json;
using Serilog;

namespace FileGenerator.Implementations
{
    public static class KafkaProducer
    {
        public static async Task CreateTopic(MainConfig cfg, ILogger logger)
        {
            using (var adminClient = new AdminClientBuilder(new AdminClientConfig
                {BootstrapServers = cfg.Kafka.BootstrapServers}).Build())
            {
                try
                {
                    await adminClient.CreateTopicsAsync(new[]
                    {
                        new TopicSpecification {Name = cfg.Kafka.Topic, ReplicationFactor = 1, NumPartitions = 1}
                    });
                    logger.Information($"Crated Kafka topic <{cfg.Kafka.Topic}>");
                }
                catch (CreateTopicsException e)
                {
                    logger.Error($"An error occured creating topic {e.Results[0].Topic}: {e.Results[0].Error.Reason}");
                }
            }
        }


        public static async Task ProduceMessage(MainConfig cfg, KafkaMessage kafkaMessage, ILogger logger)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = cfg.Kafka.BootstrapServers,
                ClientId = cfg.Kafka.ClientId
            };

            using (var producer = new ProducerBuilder<Null, string>(config).Build())
            {
                var produceAsync =
                    producer.ProduceAsync(cfg.Kafka.Topic, new Message<Null, string>
                    {
                        Value = JsonConvert.SerializeObject(kafkaMessage, Formatting.Indented)
                    });
                await produceAsync.ContinueWith(task =>
                {
                    if (task.IsFaulted)
                    {
                        logger.Error("Producing message faulted");
                        throw new Exception("FileGenerator was unable to send the message to Kafka");
                    }

                    logger.Information($"Wrote to offset: {task.Result.Offset}");
                });
            }
        }
    }
}