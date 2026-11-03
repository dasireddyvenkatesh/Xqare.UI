namespace Xqare.BusinessLayer.DTO
{
    public sealed class SessionActionResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public int AffectedSessions { get; set; }
        public bool IsCurrentSession { get; set; }
    }
}
