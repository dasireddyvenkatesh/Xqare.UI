namespace Xqare.BusinessLayer.DTO
{
    public class VerifyEmailOtpRequest
    {
        public string Email { get; set; } = default!;
        public string OtpCode { get; set; } = default!;
        public string DeviceId { get; set; } = default!;
        public string DeviceName { get; set; } = default!;
        // Used only by registration OTP completion to set the mailbox owner's password.
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
    }
}
