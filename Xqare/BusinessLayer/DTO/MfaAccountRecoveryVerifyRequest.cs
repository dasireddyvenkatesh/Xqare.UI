namespace Xqare.BusinessLayer.DTO
{
    public sealed class MfaAccountRecoveryVerifyRequest
    {
        public string Email { get; set; } = string.Empty;
        public string OtpCode { get; set; } = string.Empty;
    }

    public sealed class MfaAccountRecoveryVerifyResponse
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public string? RecoveryGrant { get; set; }
        public string? EmailProof { get; set; }
        public bool RequiresExternalVerification { get; set; }
    }
}