namespace Xqare.BusinessLayer.DTO
{
    public sealed class MfaAccountRecoveryEnrollmentStartRequest
    {
        public string RecoveryGrant { get; set; } = string.Empty;
    }

    public sealed class MfaAccountRecoveryCompleteRequest
    {
        public string RecoveryGrant { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
    }

    public sealed class MfaRecoveryEnrollmentStartResponse
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public string? Secret { get; set; }
        public string? ProvisioningUri { get; set; }
        public string? QrCodeDataUri { get; set; }
    }

    public sealed class MfaRecoveryCompleteResponse
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public IReadOnlyList<string> RecoveryCodes { get; set; } = Array.Empty<string>();
    }
}