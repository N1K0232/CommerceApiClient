namespace CommerceApiClient;

public class CommerceClient : ICommerceClient
{
    private HttpClient httpClient;
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

            disposed = true;
        }
    }
}