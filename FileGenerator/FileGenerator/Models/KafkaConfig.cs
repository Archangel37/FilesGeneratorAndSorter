using System;

namespace FileGenerator.Models
{
    [Serializable]
    public class KafkaConfig
    {
        public bool Enabled { get; set; }
        public string BootstrapServers { get; set; }
        public string Topic { get; set; }
        public string ClientId { get; set; }
    }
}