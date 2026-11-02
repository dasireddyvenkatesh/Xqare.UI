namespace Xqare.BusinessLayer.DTO
{
    public sealed class UpdateMyProfileRequest
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? Bio { get; set; }
    }
}
