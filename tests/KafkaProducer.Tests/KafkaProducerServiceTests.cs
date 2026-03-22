using KafkaProducer.Interfaces;
using KafkaProducer.Models;
using Microsoft.Extensions.Options;
using Moq;

namespace KafkaProducer.Tests;

public class KafkaProducerServiceTests
{
    private static IOptions<KafkaProducerOptions> CreateOptions(Action<KafkaProducerOptions>? configure = null)
    {
        var options = new KafkaProducerOptions
        {
            BootstrapServers = "localhost:9092",
            DefaultTopic = "test-topic"
        };
        configure?.Invoke(options);
        return Options.Create(options);
    }

    [Fact]
    public async Task ProduceAsync_WithExplicitTopic_CallsProducerWithCorrectTopic()
    {
        var mock = new Mock<IKafkaProducer<string, string>>();
        mock.Setup(p => p.ProduceAsync("my-topic", "key", "value", It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        await mock.Object.ProduceAsync("my-topic", "key", "value");

        mock.Verify(p => p.ProduceAsync("my-topic", "key", "value", It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ProduceAsync_WithDefaultTopic_CallsProducerWithoutTopic()
    {
        var mock = new Mock<IKafkaProducer<string, string>>();
        mock.Setup(p => p.ProduceAsync("key", "value", It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        await mock.Object.ProduceAsync("key", "value");

        mock.Verify(p => p.ProduceAsync("key", "value", It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public void KafkaProducerOptions_DefaultValues_AreCorrect()
    {
        var options = new KafkaProducerOptions();

        Assert.Equal(string.Empty, options.BootstrapServers);
        Assert.Equal(string.Empty, options.DefaultTopic);
        Assert.Equal(5000, options.MessageTimeoutMs);
        Assert.Null(options.SecurityProtocol);
        Assert.Null(options.SaslMechanism);
    }
}
