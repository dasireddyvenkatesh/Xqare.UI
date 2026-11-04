namespace Xqare.BusinessLayer.DTO
{
    /// <summary>
    /// Registration-only email verification request with the password chosen
    /// after the user proves ownership of the mailbox.
    /// </summary>
    public sealed class VerifyRegisterEmailOtpRequest : VerifyEmailOtpRequest
    {
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
    }
}