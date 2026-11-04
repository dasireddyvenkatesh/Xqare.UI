namespace Xqare.BusinessLayer.DTO
{
    public class VerifyEmailOtpRequest
    {
        public string Email { get; set; } = default!;
        public string OtpCode { get; set; } = default!;
        public string DeviceId { get; set; } = default!;
        public string DeviceName { get; set; } = default!;
    }
}
