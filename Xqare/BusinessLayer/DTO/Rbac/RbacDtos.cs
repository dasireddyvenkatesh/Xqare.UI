namespace Xqare.BusinessLayer.DTO.Rbac
{
    public sealed class RoleResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public bool IsSystem { get; set; }
        public int AssignedUsers { get; set; }
        public int PermissionCount { get; set; }
    }

    public sealed class RbacRolesResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<RoleResponse> Roles { get; set; } = new();
    }

    public sealed class RbacOperationResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? ErrorCode { get; set; }
    }

    public sealed class CreateRoleRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
    }

    public sealed class UpdateRoleRequest
    {
        public string Name { get; set; } = string.Empty;
    }
    public sealed class RoleAssignmentRequest
    {
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
    }

    public sealed class PermissionResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Module { get; set; } = string.Empty;
    }

    public sealed class RolePermissionsResponse
    {
        public Guid RoleId { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public string RoleCode { get; set; } = string.Empty;
        public List<PermissionResponse> Permissions { get; set; } = new();
    }

    public sealed class RolePermissionsResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public RolePermissionsResponse? Role { get; set; }
    }

    public sealed class PermissionsResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<PermissionResponse> Permissions { get; set; } = new();
    }

}
