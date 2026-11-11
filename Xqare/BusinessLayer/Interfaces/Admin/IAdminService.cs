using Xqare.BusinessLayer.DTO.Admin;

namespace Xqare.BusinessLayer.Interfaces.Admin
{
    public interface IAdminService
    {
        Task<AdminOverviewResponse> GetOverviewAsync();
        Task<AdminUsersResponse> GetUsersAsync(
            string? search,
            bool? isActive,
            string? roleCode,
            bool? isEmailConfirmed,
            DateTime? createdFromUtc,
            DateTime? createdToUtc,
            int page,
            int pageSize);
        Task<AdminUserDetailsResponse?> GetUserDetailsAsync(Guid userId);
        Task<AdminUserActionResponse> UpdateUserStatusAsync(Guid userId, bool isActive);
    }
}