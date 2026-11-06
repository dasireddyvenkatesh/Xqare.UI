namespace Xqare.BusinessLayer.DTO.Admin
{
    public sealed class AdminOverviewResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public int TotalUsers { get; set; }
        public int ActiveUsers { get; set; }
        public int DisabledUsers { get; set; }
        public int NewRegistrationsLast30Days { get; set; }
        public int ActiveSessions { get; set; }
        public DateTime GeneratedAtUtc { get; set; }
    }

    public sealed class AdminUserListItemResponse
    {
        public Guid UserId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? ProfileImageUrl { get; set; }
        public bool IsActive { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public List<string> Roles { get; set; } = new();
    }

    public sealed class AdminUsersResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public List<AdminUserListItemResponse> Items { get; set; } = new();
    }

    public sealed class AdminUserRoleAssignmentResponse
    {
        public Guid RoleId { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public string RoleCode { get; set; } = string.Empty;
    }

    public sealed class AdminUserDetailsResponse
    {
        public bool IsSuccess { get; set; }
        public Guid UserId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? Bio { get; set; }
        public string? ProfileImageUrl { get; set; }
        public bool IsActive { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public int ActiveSessionCount { get; set; }
        public int TotalSessionCount { get; set; }
        public List<string> Roles { get; set; } = new();
        public List<AdminUserRoleAssignmentResponse> RoleAssignments { get; set; } = new();
    }

    public sealed class AdminUserActionResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? ErrorCode { get; set; }
        public bool? IsActive { get; set; }
        public int AffectedSessions { get; set; }
    }

    public sealed class UpdateAdminUserStatusRequest
    {
        public bool IsActive { get; set; }
    }
}
