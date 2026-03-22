# KafkaProducer

A simple Kafka producer class library for .NET using [Confluent.Kafka](https://docs.confluent.io/kafka-clients/dotnet/current/overview.html).

## Installation

```bash
dotnet add package KafkaProducer
```

## Configuration

Add to `appsettings.json`:

```json
{
  "Kafka": {
    "BootstrapServers": "localhost:9092",
    "DefaultTopic": "my-topic",
    "MessageTimeoutMs": 5000
  }
}
```

## Usage

Register in `Program.cs`:

```csharp
builder.Services.AddKafkaProducer<string, string>(builder.Configuration);
```

Inject and use:

```csharp
public class MyService
{
    private readonly IKafkaProducer<string, string> _producer;

    public MyService(IKafkaProducer<string, string> producer)
    {
        _producer = producer;
    }

    public async Task SendAsync(string key, string message)
    {
        await _producer.ProduceAsync(key, message);
        // or with explicit topic:
        await _producer.ProduceAsync("other-topic", key, message);
    }
}
```

## Authentication (optional)

```json
{
  "Kafka": {
    "BootstrapServers": "broker:9092",
    "DefaultTopic": "my-topic",
    "SecurityProtocol": "SaslSsl",
    "SaslMechanism": "Plain",
    "SaslUsername": "user",
    "SaslPassword": "pass"
  }
}
```
