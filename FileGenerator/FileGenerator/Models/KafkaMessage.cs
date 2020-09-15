using System;

namespace FileGenerator.Models
{
    [Serializable]
    public class KafkaMessage
    {
        public DateTime UtcStartDateTime { get; set; }
        public string FilePath { get; set; }
    }
}