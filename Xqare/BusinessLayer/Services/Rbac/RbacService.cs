using System.Text.Json;
using Xqare.BusinessLayer.DTO.Rbac;
using Xqare.BusinessLayer.Interfaces.Common;
using Xqare.BusinessLayer.Interfaces.Rbac;

namespace Xqare.BusinessLayer.Services.Rbac
{
    public sealed class RbacService : IRbacService
    {
        private readonly IApiClient _api;

        public RbacService(IApiClient api)
        {
            _api = api;
        }

        public async Task<RbacRolesResult> GetRolesAsync()
        {
            var response = await _api.GetAsync<List<RoleResponse>>("api/admin/rbac/roles");
            return response.Data is not null
                ? new RbacRolesResult { IsSuccess = true, Roles = response.Data }
                : new RbacRolesResult
                {
                    IsSuccess = false,
                    Message = ExtractMessage(response.ErrorMessage) ?? "Roles could not be loaded."
                };
        }

        public async Task<RbacRolesResult> GetAssignableRolesAsync()
        {
            var response = await _api.GetAsync<List<RoleResponse>>("api/admin/rbac/roles/assignable");
            return response.Data is not null
                ? new RbacRolesResult { IsSuccess = true, Roles = response.Data }
                : new RbacRolesResult
                {
                    IsSuccess = false,
                    Message = ExtractMessage(response.ErrorMessage) ?? "Assignable roles could not be loaded."
                };
        }

        public async Task<PermissionsResult> GetPermissionsAsync()
        {
            var response = await _api.GetAsync<List<PermissionResponse>>("api/admin/rbac/permissions");
            return response.Data is not null
                ? new PermissionsResult { IsSuccess = true, Permissions = response.Data }
                : new PermissionsResult
                {
                    IsSuccess = false,
                    Message = ExtractMessage(response.ErrorMessage) ?? "Permissions could not be loaded."
                };
        }

        public async Task<RolePermissionsResult> GetRolePermissionsAsync(Guid roleId)
        {
            var response = await _api.GetAsync<RolePermissionsResponse>(
                $"api/admin/rbac/roles/{roleId:D}/permissions");

            return response.Data is not null
                ? new RolePermissionsResult { IsSuccess = true, Role = response.Data }
                : new RolePermissionsResult
                {
                    IsSuccess = false,
                    Message = ExtractMessage(response.ErrorMessage) ?? "Role permissions could not be loaded."
                };
        }

        public async Task<RbacOperationResponse> CreateRoleAsync(CreateRoleRequest request)
        {
            var response = await _api.PostAsync<CreateRoleRequest, RbacOperationResponse>(
                "api/admin/rbac/roles", request);
            return response.Data ?? Failure(response.ErrorMessage, "The role could not be created.");
        }

        public async Task<RbacOperationResponse> UpdateRoleAsync(Guid roleId, UpdateRoleRequest request)
        {
            var response = await _api.PutAsync<UpdateRoleRequest, RbacOperationResponse>(
                $"api/admin/rbac/roles/{roleId:D}", request);
            return response.Data ?? Failure(response.ErrorMessage, "The role could not be updated.");
        }

        public async Task<RbacOperationResponse> DeleteRoleAsync(Guid roleId)
        {
            var response = await _api.DeleteAsync<RbacOperationResponse>(
                $"api/admin/rbac/roles/{roleId:D}");
            return response.Data ?? Failure(response.ErrorMessage, "The role could not be deleted.");
        }

        public async Task<RbacOperationResponse> AssignRoleAsync(RoleAssignmentRequest request)
        {
            var response = await _api.PostAsync<RoleAssignmentRequest, RbacOperationResponse>(
                "api/admin/rbac/assignments", request);
            return response.Data ?? Failure(response.ErrorMessage, "The role could not be assigned.");
        }

        public async Task<RbacOperationResponse> RemoveRoleAsync(RoleAssignmentRequest request)
        {
            var response = await _api.DeleteAsync<RoleAssignmentRequest, RbacOperationResponse>(
                "api/admin/rbac/assignments", request);
            return response.Data ?? Failure(response.ErrorMessage, "The role assignment could not be removed.");
        }

        public async Task<RbacOperationResponse> SetRolePermissionsAsync(Guid roleId, List<string> permissionCodes)
        {
            var request = new { PermissionCodes = permissionCodes };
            var response = await _api.PutAsync<object, RbacOperationResponse>(
                $"api/admin/rbac/roles/{roleId:D}/permissions", request);
            return response.Data ?? Failure(response.ErrorMessage, "Role permissions could not be updated.");
        }

        private static RbacOperationResponse Failure(string? payload, string fallback)
            => new()
            {
                IsSuccess = false,
                Message = ExtractMessage(payload) ?? fallback
            };

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