namespace Xqare.BusinessLayer.DTO
{
    public class VerifyMfaOtpRequest
    {
        public Guid UserId { get; set; }
        public string Code { get; set; } = default!;
        public string DeviceId { get; set; } = default!;
        public string DeviceName { get; set; } = default!;
    }
}