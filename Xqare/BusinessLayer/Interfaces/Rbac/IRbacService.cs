using Xqare.BusinessLayer.DTO.Rbac;

namespace Xqare.BusinessLayer.Interfaces.Rbac
{
    public interface IRbacService
    {
        Task<RbacRolesResult> GetRolesAsync();
        Task<RolePermissionsResult> GetRolePermissionsAsync(Guid roleId);
        Task<PermissionsResult> GetPermissionsAsync();
        Task<RbacOperationResponse> CreateRoleAsync(CreateRoleRequest request);
        Task<RbacOperationResponse> UpdateRoleAsync(Guid roleId, UpdateRoleRequest request);
        Task<RbacOperationResponse> DeleteRoleAsync(Guid roleId);
    }
}