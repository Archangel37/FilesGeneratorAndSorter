using System;

namespace FileSorter.Models
{
    [Serializable]
    public class KafkaConfig
    {
        public string BootstrapServers { get; set; }
        public string Topic { get; set; }
        public string GroupId { get; set; }
        public string ClientId { get; set; }
    }
}