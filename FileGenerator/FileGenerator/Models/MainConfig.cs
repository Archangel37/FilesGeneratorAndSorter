using System.Collections.Generic;

namespace FileGenerator.Models
{
    public class MainConfig
    {
        public long FileSizeBytes { get; set; }
        public string ResultFilePath { get; set; }
        public IEnumerable<string> DictionaryFilesPaths { get; set; }
        public KafkaConfig Kafka { get; set; }
    }
}