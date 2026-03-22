using Confluent.Kafka;
using KafkaProducer.Interfaces;
using KafkaProducer.Models;
using Microsoft.Extensions.Options;

namespace KafkaProducer;

public class KafkaProducerService<TKey, TValue> : IKafkaProducer<TKey, TValue>, IDisposable
{
    private readonly IProducer<TKey, TValue> _producer;
    private readonly KafkaProducerOptions _options;

    public KafkaProducerService(IOptions<KafkaProducerOptions> options)
    {
        _options = options.Value;
        _producer = new ProducerBuilder<TKey, TValue>(BuildConfig()).Build();
    }

    public async Task ProduceAsync(string topic, TKey key, TValue value, CancellationToken cancellationToken = default)
    {
        var message = new Message<TKey, TValue> { Key = key, Value = value };
        await _producer.ProduceAsync(topic, message, cancellationToken);
    }

    public Task ProduceAsync(TKey key, TValue value, CancellationToken cancellationToken = default)
        => ProduceAsync(_options.DefaultTopic, key, value, cancellationToken);

    public async Task ProduceAsync(string topic, TKey key, TValue value, IDictionary<string, string> headers, CancellationToken cancellationToken = default)
    {
        var kafkaHeaders = new Headers();
        foreach (var header in headers)
            kafkaHeaders.Add(header.Key, System.Text.Encoding.UTF8.GetBytes(header.Value));

        var message = new Message<TKey, TValue> { Key = key, Value = value, Headers = kafkaHeaders };
        await _producer.ProduceAsync(topic, message, cancellationToken);
    }

    public Task ProduceAsync(TKey key, TValue value, IDictionary<string, string> headers, CancellationToken cancellationToken = default)
        => ProduceAsync(_options.DefaultTopic, key, value, headers, cancellationToken);

    private ProducerConfig BuildConfig()
    {
        var config = new ProducerConfig
        {
            BootstrapServers = _options.BootstrapServers,
            MessageTimeoutMs = _options.MessageTimeoutMs
        };

        if (!string.IsNullOrEmpty(_options.SecurityProtocol))
            config.SecurityProtocol = Enum.Parse<SecurityProtocol>(_options.SecurityProtocol, ignoreCase: true);

        if (!string.IsNullOrEmpty(_options.SaslMechanism))
            config.SaslMechanism = Enum.Parse<SaslMechanism>(_options.SaslMechanism, ignoreCase: true);

        if (!string.IsNullOrEmpty(_options.SaslUsername))
            config.SaslUsername = _options.SaslUsername;

        if (!string.IsNullOrEmpty(_options.SaslPassword))
            config.SaslPassword = _options.SaslPassword;

        return config;
    }

    public void Dispose()
    {
        _producer.Flush(TimeSpan.FromSeconds(5));
        _producer.Dispose();
    }
}
