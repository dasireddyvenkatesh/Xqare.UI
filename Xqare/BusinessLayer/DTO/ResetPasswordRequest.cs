namespace Xqare.BusinessLayer.DTO
{
    public class ResetPasswordRequest
    {
        public string Token { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
