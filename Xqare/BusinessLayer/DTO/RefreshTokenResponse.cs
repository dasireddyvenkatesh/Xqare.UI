namespace Xqare.BusinessLayer.DTO
{
    public class RefreshTokenResponse
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public string AccessToken { get; set; } = default!;
        public DateTime? AccessTokenExpiryUtc { get; set; }
    }
}
