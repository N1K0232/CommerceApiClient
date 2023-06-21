using CommerceApi.Shared.Models.Requests;
using CommerceApi.Shared.Models.Responses;

namespace CommerceApiClient;

public interface ICommerceClient : IDisposable
{
    Task<LoginResponse?> LoginAsync(LoginRequest loginRequest);
}