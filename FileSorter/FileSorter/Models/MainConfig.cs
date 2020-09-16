namespace FileSorter.Models
{
    public class MainConfig
    {
        public byte SubstringLength { get; set; }
        public long WriteMemoryBufferBytes { get; set; }
        public string TemporaryFilesExtension { get; set; }
        public string OutputFolderPath { get; set; }
        public string OutputFileName { get; set; }

        public KafkaConfig Kafka { get; set; }
    }
}