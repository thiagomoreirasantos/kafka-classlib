using KafkaProducer.Interfaces;
using KafkaProducer.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KafkaProducer.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddKafkaProducer<TKey, TValue>(
        this IServiceCollection services,
        IConfiguration configuration,
        string sectionName = KafkaProducerOptions.SectionName)
    {
        services.Configure<KafkaProducerOptions>(configuration.GetSection(sectionName));
        services.AddSingleton<IKafkaProducer<TKey, TValue>, KafkaProducerService<TKey, TValue>>();
        return services;
    }

    public static IServiceCollection AddKafkaProducer<TKey, TValue>(
        this IServiceCollection services,
        Action<KafkaProducerOptions> configure)
    {
        services.Configure(configure);
        services.AddSingleton<IKafkaProducer<TKey, TValue>, KafkaProducerService<TKey, TValue>>();
        return services;
    }
}
