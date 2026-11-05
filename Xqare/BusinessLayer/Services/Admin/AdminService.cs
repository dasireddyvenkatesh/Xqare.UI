using System.Text.Json;
using Xqare.BusinessLayer.DTO.Admin;
using Xqare.BusinessLayer.Interfaces.Admin;
using Xqare.BusinessLayer.Interfaces.Common;

namespace Xqare.BusinessLayer.Services.Admin
{
    public sealed class AdminService : IAdminService
    {
        private readonly IApiClient _api;

        public AdminService(IApiClient api)
        {
            _api = api;
        }

        public async Task<AdminOverviewResponse> GetOverviewAsync()
        {
            var response = await _api.GetAsync<AdminOverviewResponse>("api/admin/overview");
            return response.Data ?? new AdminOverviewResponse
            {
                IsSuccess = false,
                Message = ExtractMessage(response.ErrorMessage) ?? "The administration overview could not be loaded."
            };
        }

        public async Task<AdminUsersResponse> GetUsersAsync(string? search, bool? isActive, int page, int pageSize)
        {
            var query = $"api/admin/users?page={page}&pageSize={pageSize}";
            if (!string.IsNullOrWhiteSpace(search))
                query += $"&search={Uri.EscapeDataString(search.Trim())}";
            if (isActive.HasValue)
                query += $"&isActive={isActive.Value.ToString().ToLowerInvariant()}";

            var response = await _api.GetAsync<AdminUsersResponse>(query);
            return response.Data ?? new AdminUsersResponse
            {
                IsSuccess = false,
                Message = ExtractMessage(response.ErrorMessage) ?? "Users could not be loaded."
            };
        }

        public async Task<AdminUserDetailsResponse?> GetUserDetailsAsync(Guid userId)
        {
            var response = await _api.GetAsync<AdminUserDetailsResponse>($"api/admin/users/{userId:D}");
            return response.IsSuccess ? response.Data : null;
        }

        public async Task<AdminUserActionResponse> UpdateUserStatusAsync(Guid userId, bool isActive)
        {
            var response = await _api.PutAsync<UpdateAdminUserStatusRequest, AdminUserActionResponse>(
                $"api/admin/users/{userId:D}/status",
                new UpdateAdminUserStatusRequest { IsActive = isActive });

            return response.Data ?? new AdminUserActionResponse
            {
                IsSuccess = false,
                Message = ExtractMessage(response.ErrorMessage) ?? "The account status could not be updated."
            };
        }

        private static string? ExtractMessage(string? payload)
        {
            if (string.IsNullOrWhiteSpace(payload))
                return null;

            try
            {
                using var document = JsonDocument.Parse(payload);
                foreach (var name in new[] { "message", "Message", "title", "Title" })
                {
                    if (document.RootElement.ValueKind == JsonValueKind.Object &&
                        document.RootElement.TryGetProperty(name, out var value) &&
                        value.ValueKind == JsonValueKind.String &&
                        !string.IsNullOrWhiteSpace(value.GetString()))
                        return value.GetString();
                }
            }
            catch (JsonException)
            {
                // Raw server payloads are never displayed in the UI.
            }

            return null;
        }
    }
}
