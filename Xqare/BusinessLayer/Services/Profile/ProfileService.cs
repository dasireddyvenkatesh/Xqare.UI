using System.Text.Json;
using Xqare.BusinessLayer.DTO;
using Xqare.BusinessLayer.Interfaces.Common;
using Xqare.BusinessLayer.Interfaces.Profile;

namespace Xqare.BusinessLayer.Services.Profile
{
    public sealed class ProfileService : IProfileService
    {
        private readonly IApiClient _api;

        public ProfileService(IApiClient api)
        {
            _api = api;
        }

        public async Task<MyProfileResponse> GetMyProfileAsync()
        {
            var response = await _api.GetAsync<MyProfileResponse>("api/profile/me");
            if (response.IsSuccess && response.Data != null)
                return response.Data;

            return new MyProfileResponse
            {
                IsSuccess = false,
                Message = ExtractMessage(response.ErrorMessage)
                    ?? "Your profile could not be loaded. Please try again."
            };
        }

        public async Task<MyProfileResponse> UpdateMyProfileAsync(UpdateMyProfileRequest request)
        {
            var response = await _api.PutAsync<UpdateMyProfileRequest, MyProfileResponse>("api/profile/me", request);
            if (response.IsSuccess && response.Data != null)
                return response.Data;

            return new MyProfileResponse
            {
                IsSuccess = false,
                Message = ExtractMessage(response.ErrorMessage)
                    ?? "Your changes could not be saved. Please try again."
            };
        }

        private static string? ExtractMessage(string? payload)
        {
            if (string.IsNullOrWhiteSpace(payload))
                return null;

            try
            {
                using var document = JsonDocument.Parse(payload);
                var root = document.RootElement;
                foreach (var name in new[] { "message", "Message", "title", "Title" })
                {
                    if (root.ValueKind == JsonValueKind.Object &&
                        root.TryGetProperty(name, out var value) &&
                        value.ValueKind == JsonValueKind.String &&
                        !string.IsNullOrWhiteSpace(value.GetString()))
                    {
                        return value.GetString();
                    }
                }

                if (root.ValueKind == JsonValueKind.Object &&
                    root.TryGetProperty("errors", out var errors) &&
                    errors.ValueKind == JsonValueKind.Object)
                {
                    var first = errors.EnumerateObject()
                        .SelectMany(x => x.Value.ValueKind == JsonValueKind.Array
                            ? x.Value.EnumerateArray().Select(v => v.GetString())
                            : Enumerable.Empty<string?>())
                        .FirstOrDefault(x => !string.IsNullOrWhiteSpace(x));
                    return first;
                }
            }
            catch (JsonException)
            {
                // Keep raw server payloads out of the visible UI.
            }

            return null;
        }
    }
}
