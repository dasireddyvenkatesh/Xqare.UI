namespace Xqare.BusinessLayer.DTO
{
    public class OtpOperationResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = "The request could not be completed.";
        public bool CanVerifyExistingOtp { get; set; }
        public int? RetryAfterSeconds { get; set; }
    }
}
