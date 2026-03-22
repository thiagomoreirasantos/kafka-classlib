namespace KafkaProducer.Models;

public class KafkaProducerOptions
{
    public const string SectionName = "Kafka";

    public string BootstrapServers { get; set; } = string.Empty;
    public string DefaultTopic { get; set; } = string.Empty;
    public int MessageTimeoutMs { get; set; } = 5000;
    public string? SecurityProtocol { get; set; }
    public string? SaslMechanism { get; set; }
    public string? SaslUsername { get; set; }
    public string? SaslPassword { get; set; }
}
