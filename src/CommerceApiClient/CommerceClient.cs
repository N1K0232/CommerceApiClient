using System.Net.Http.Headers;
using System.Net.Mime;

namespace CommerceApiClient;

public class CommerceClient : ICommerceClient
{
    private HttpClient httpClient;
    private CancellationTokenSource? tokenSource;

    private bool disposed = false;

    private readonly bool useInnerHttpClient;

    public CommerceClient(HttpClient? httpClient)
    {
        if (httpClient == null)
        {
            this.httpClient = new HttpClient();
            useInnerHttpClient = true;
        }
        else
        {
            this.httpClient = httpClient;
            useInnerHttpClient = false;
        }

        this.httpClient.DefaultRequestHeaders.Accept.Clear();
        this.httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (disposing && !disposed)
        {
            if (httpClient != null && useInnerHttpClient)
            {
                httpClient.Dispose();
                httpClient = null!;
            }

            if (tokenSource != null)
            {
                tokenSource.Dispose();
                tokenSource = null;
            }

            disposed = true;
        }
    }
}