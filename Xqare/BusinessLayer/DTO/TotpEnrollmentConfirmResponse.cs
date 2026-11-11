namespace Xqare.BusinessLayer.DTO
{
    public class TotpEnrollmentConfirmResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public IReadOnlyList<string> RecoveryCodes { get; set; } = Array.Empty<string>();
        public int? RetryAfterSeconds { get; set; }
    }
}
