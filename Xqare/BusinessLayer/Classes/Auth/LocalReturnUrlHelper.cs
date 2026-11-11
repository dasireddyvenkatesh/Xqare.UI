namespace Xqare.BusinessLayer.Classes.Auth
{
    /// <summary>
    /// Validates post-authentication navigation targets to prevent open redirects.
    /// Only local application paths are accepted.
    /// </summary>
    public static class LocalReturnUrlHelper
    {
        public static string GetSafeReturnUrl(string? candidate, string fallback = "/dashboard")
        {
            if (string.IsNullOrWhiteSpace(candidate) ||
                candidate.Any(char.IsControl) ||
                candidate.Contains('\\') ||
                candidate.StartsWith("//", StringComparison.Ordinal) ||
                Uri.TryCreate(candidate, UriKind.Absolute, out _))
            {
                return fallback;
            }

            var target = candidate.StartsWith("/", StringComparison.Ordinal)
                ? candidate
                : "/" + candidate;

            var path = target.Split('?', '#')[0];
            if (string.IsNullOrWhiteSpace(path) ||
                string.Equals(path, "/login", StringComparison.OrdinalIgnoreCase) ||
                path.StartsWith("/login/", StringComparison.OrdinalIgnoreCase))
            {
                return fallback;
            }

            return target;
        }
    }
}
