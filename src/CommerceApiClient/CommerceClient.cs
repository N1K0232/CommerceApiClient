using CommerceApiClient.Settings;

namespace CommerceApiClient;

public class CommerceClient : ICommerceClient
{
    private HttpClient httpClient;
    private bool disposed = false;

    private readonly bool useInnerHttpClient;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="httpClient"></param>
    public CommerceClient(HttpClient? httpClient = null)
    {
        if (httpClient is null)
        {
            this.httpClient = new HttpClient();
            useInnerHttpClient = true;
        }
        else
        {
            this.httpClient = httpClient;
            useInnerHttpClient = false;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="settings"></param>
    public CommerceClient(CommerceApiClientSettings? settings) : this(httpClient: null)
    {
        Initialize(settings);
    }


    public void Dispose()
    {
        if (useInnerHttpClient)
        {
            Dispose(disposing: true);
        }

        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (disposing && !disposed)
        {
            httpClient.Dispose();
            disposed = true;
        }
    }

    private void ThrowIfDisposed()
    {
        if (disposed)
        {
            throw new ObjectDisposedException(GetType().FullName);
        }
    }

    private void Initialize(CommerceApiClientSettings? settings)
    {
        if (settings is null || string.IsNullOrWhiteSpace(settings?.BaseAddress))
        {
            httpClient.BaseAddress = null;
        }
        else
        {
            httpClient.BaseAddress = new Uri(settings.BaseAddress);
        }
    }
}