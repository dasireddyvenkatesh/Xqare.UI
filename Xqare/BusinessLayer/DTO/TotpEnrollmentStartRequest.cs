namespace Xqare.BusinessLayer.DTO
{
    public class TotpEnrollmentStartRequest
    {
        public string CurrentPassword { get; set; } = string.Empty;
        public string? CurrentTotpCode { get; set; }
    }
}
