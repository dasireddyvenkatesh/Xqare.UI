namespace Xqare.BusinessLayer.DTO
{
    public class TotpEnrollmentStartResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? Secret { get; set; }
        public string? ProvisioningUri { get; set; }
        public string? QrCodeDataUri { get; set; }
    }
}
