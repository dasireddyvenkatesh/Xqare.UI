using Microsoft.JSInterop;
using Xqare.BusinessLayer.Interfaces.Auth;

namespace Xqare.BusinessLayer.Classes.Auth;

public class TokenService : ITokenService
{
    private readonly IJSRuntime _js;
    private const string Key = "xq_access_token";

    // In-memory cache so we don't call JS every single time
    private string? _cachedToken;

    public TokenService(IJSRuntime js)
    {
        _js = js;
    }

    public async Task<string?> GetAccessTokenAsync()
    {
        // Return cache first — avoid JS round trip
        if (!string.IsNullOrEmpty(_cachedToken))
            return _cachedToken;

        try
        {
            _cachedToken = await _js.InvokeAsync<string?>("sessionStorageHelper.get", Key);
            return _cachedToken;
        }
        catch
        {
            // JS not ready yet (rare in WASM but safe to handle)
            return null;
        }
    }

    public async Task SetTokenAsync(string accessToken)
    {
        _cachedToken = accessToken;
        try
        {
            await _js.InvokeVoidAsync("sessionStorageHelper.set", Key, accessToken);
        }
        catch { /* ignore if JS not ready */ }
    }

    public async Task ClearAsync()
    {
        _cachedToken = null;
        try
        {
            await _js.InvokeVoidAsync("sessionStorageHelper.remove", Key);
        }
        catch { /* ignore */ }
    }
}