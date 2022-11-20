using CommerceApiClient;
using CommerceApiClient.Settings;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IHttpClientBuilder AddCommerceApiClient(this IServiceCollection services, Action<CommerceApiClientSettings> configuration)
    {
        var settings = new CommerceApiClientSettings();
        configuration?.Invoke(settings);

        services.AddSingleton(settings);

        var builder = services.AddHttpClient<ICommerceClient, CommerceClient>();
        return builder;
    }
}