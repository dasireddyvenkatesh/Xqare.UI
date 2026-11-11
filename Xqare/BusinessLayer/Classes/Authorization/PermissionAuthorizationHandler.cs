using Microsoft.AspNetCore.Authorization;

namespace Xqare.BusinessLayer.Classes.Authorization
{
    /// <summary>
    /// UI-only permission presentation check. The API remains authoritative and
    /// independently validates permissions and the active Super Admin assignment.
    /// </summary>
    public sealed class PermissionAuthorizationHandler
        : AuthorizationHandler<PermissionRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionRequirement requirement)
        {
            if (context.User.Identity?.IsAuthenticated != true)
                return Task.CompletedTask;

            // The API validates the current Super Admin assignment against the database.
            // This claim-based bypass only keeps the UI experience consistent.
            if (context.User.IsInRole("SUPERADMIN") ||
                context.User.Claims.Any(claim =>
                    claim.Type == "permission" &&
                    string.Equals(claim.Value, requirement.PermissionCode,
                        StringComparison.OrdinalIgnoreCase)))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}