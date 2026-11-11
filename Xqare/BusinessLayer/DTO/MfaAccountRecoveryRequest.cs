namespace Xqare.BusinessLayer.DTO
{
    public sealed class MfaAccountRecoveryRequest
    {
        public string Email { get; set; } = string.Empty;
        public string? Password { get; set; }
    }
}