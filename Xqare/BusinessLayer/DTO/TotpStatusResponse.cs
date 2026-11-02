namespace Xqare.BusinessLayer.DTO
{
    public class TotpStatusResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public bool IsEnabled { get; set; }
        public bool CanEnroll { get; set; }
        public DateTime? EnabledAt { get; set; }
        public int RecoveryCodesRemaining { get; set; }
    }
}
