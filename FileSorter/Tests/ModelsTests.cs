using System;
using System.Diagnostics.CodeAnalysis;
using FileSorter.Models;
using Xunit;

namespace Tests
{
    [ExcludeFromCodeCoverage]
    public class ModelsTests
    {
        [Fact]
        //    integrity of KafkaConfig type
        public void Test_KafkaConfig_Model()
        {
           var kafkaConfig = new KafkaConfig
           {
               BootstrapServers = "", ClientId = "", GroupId = "", Topic = ""
           };
           Assert.True(kafkaConfig.BootstrapServers != null);
           Assert.True(kafkaConfig.ClientId != null);
           Assert.True(kafkaConfig.GroupId != null);
           Assert.True(kafkaConfig.Topic != null);
           
           Assert.True(kafkaConfig.BootstrapServers is string);
           Assert.True(kafkaConfig.ClientId is string);
           Assert.True(kafkaConfig.GroupId is string);
           Assert.True(kafkaConfig.Topic is string);
        }
        
        [Fact]
        //    integrity of MainConfig type
        public void Test_MainConfig_Model()
        {
            var mainConfig = new MainConfig
            {
                SubstringLength = 2,
                Kafka = new KafkaConfig {BootstrapServers = "", ClientId = "", GroupId = "", Topic = ""},
                OutputFileName = "test.txt",
                OutputFolderPath = "",
                TemporaryFilesExtension = "",
                WriteMemoryBufferBytes = 0L
            };
            
            
            Assert.True(mainConfig.SubstringLength is byte);
            Assert.True(mainConfig.OutputFileName is string);
            Assert.True(mainConfig.OutputFolderPath is string);
            Assert.True(mainConfig.TemporaryFilesExtension is string);
            Assert.True(mainConfig.WriteMemoryBufferBytes is long);
            Assert.True(mainConfig.Kafka.GetType() == typeof(KafkaConfig));
        }
        
        [Fact]
        //    integrity of MainConfig type
        public void Test_KafkaMessage_Model()
        {
            var kafkaMessage = new KafkaMessage
            {
               UtcStartDateTime = DateTime.Now.ToUniversalTime(),
               FilePath = Environment.CurrentDirectory //invalid for file, but for string
            };
            
            
            Assert.True(kafkaMessage.UtcStartDateTime is DateTime);
            Assert.True(kafkaMessage.FilePath is string);
            
            Assert.True(kafkaMessage.UtcStartDateTime != default);
            Assert.True(kafkaMessage.FilePath != default);
        }
    }
}