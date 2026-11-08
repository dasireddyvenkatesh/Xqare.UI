using Microsoft.JSInterop;
using Xqare.BusinessLayer.Interfaces.Auth;

namespace Xqare.BusinessLayer.Classes.Auth;

public class TokenService : ITokenService
{
    private readonly IJSRuntime _js;
    private const string Key = "xq_access_token_v2";

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
        catch (Exception ex)
        {
            // Storage access can be unavailable in restricted browser contexts.
            Console.Error.WriteLine($"Unable to read the tab access token from sessionStorage: {ex.Message}");
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
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unable to persist the tab access token in sessionStorage: {ex.Message}");
        }

        try
        {
            // Persist only a boolean-like hint, never the access or refresh token.
            await _js.InvokeVoidAsync("sessionStorageHelper.setSessionHint");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unable to persist the browser session hint: {ex.Message}");
        }
    }

    public async Task ClearAsync()
    {
        _cachedToken = null;
        try
        {
            await _js.InvokeVoidAsync("sessionStorageHelper.remove", Key);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unable to clear the tab access token from sessionStorage: {ex.Message}");
        }

        try
        {
            await _js.InvokeVoidAsync("sessionStorageHelper.clearSessionHint");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unable to clear the browser session hint: {ex.Message}");
        }
    }

    public async Task<bool> HasSessionHintAsync()
    {
        try
        {
            return await _js.InvokeAsync<bool>("sessionStorageHelper.hasSessionHint");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unable to read the browser session hint: {ex.Message}");
            return false;
        }
    }
}