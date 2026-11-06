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
}