namespace Xqare.BusinessLayer.DTO
{
    public class RegisterRequest
    {
        public string FullName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string ConfirmPassword { get; set; } = default!;
        public string DeviceId { get; set; } = default!;
        public string? DeviceName { get; set; } = default!;
        public string? UserAgent { get; set; }
    }
}
