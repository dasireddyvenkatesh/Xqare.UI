namespace Xqare.BusinessLayer.DTO
{
    public sealed class ActiveSessionsResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<ActiveSessionDto> Sessions { get; set; } = new();
    }
}
