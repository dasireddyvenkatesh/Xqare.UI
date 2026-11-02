namespace Xqare.BusinessLayer.DTO
{
    public class VerifyMfaChallengeRequest
    {
        public string Code { get; set; } = string.Empty;
        public string Factor { get; set; } = "auto";
    }
}
