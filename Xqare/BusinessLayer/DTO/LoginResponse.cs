namespace Xqare.BusinessLayer.DTO
{
    public class LoginResponse
    {
        public bool IsSuccess { get; set; }

        public bool RequiresMfa { get; set; }

        public string AccessToken { get; set; } = default!;

        public string? RefreshToken { get; set; }

        public DateTime? AccessTokenExpiry { get; set; }

        public string? UserPublicId { get; set; }

        public string? Email { get; set; }

        public IEnumerable<string>? Roles { get; set; }

        public IEnumerable<string>? Permissions { get; set; }

        public string? Message { get; set; }
    }
}
