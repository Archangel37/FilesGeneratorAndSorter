namespace FileSorter.Models
{
    public class MainConfig
    {
        public byte SubstringLength { get; set; }
        public int WriteMemoryBufferBytes { get; set; }
        public int BufferDeltaBytes { get; set; }
        public string TemporaryFilesExtension { get; set; }
        public string OutputFolderPath { get; set; }
        public string OutputFileName { get; set; }

        public KafkaConfig Kafka { get; set; }
    }
}