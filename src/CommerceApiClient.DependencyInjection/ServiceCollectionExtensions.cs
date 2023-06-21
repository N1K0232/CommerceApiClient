using Microsoft.Extensions.DependencyInjection;

namespace CommerceApiClient.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCommerceApiClient(this IServiceCollection services, Action<CommerceClientOptions> configuration)
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        if (configuration == null)
        {
            throw new ArgumentNullException(nameof(configuration), "url is required");
        }

        var options = new CommerceClientOptions();
        configuration.Invoke(options);

        services.AddHttpClient();
        services.AddScoped(services =>
        {
            var httpClientFactory = services.GetRequiredService<IHttpClientFactory>();
            var httpClient = httpClientFactory.CreateClient();
            httpClient.BaseAddress = new Uri(options.BaseUrl);

            return httpClient;
        });

        services.AddScoped<ICommerceClient, CommerceClient>();
        return services;
    }
}