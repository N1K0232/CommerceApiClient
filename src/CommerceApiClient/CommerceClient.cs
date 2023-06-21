﻿using CommerceApi.Shared.Models.Requests;
using CommerceApi.Shared.Models.Responses;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Mime;
using System.Text.Json;

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

    public async Task<LoginResponse?> LoginAsync(LoginRequest loginRequest)
    {
        tokenSource ??= new CancellationTokenSource();

        using var response = await httpClient.PostAsJsonAsync("/api/Users/Login", loginRequest, tokenSource.Token).ConfigureAwait(false);
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (string.IsNullOrWhiteSpace(content))
            {
                return null;
            }

            var loginResponse = JsonSerializer.Deserialize<LoginResponse>(content);
            return loginResponse;
        }

        return null;
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