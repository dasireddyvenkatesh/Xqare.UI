namespace Xqare.BusinessLayer.DTO
{
    public class RegisterResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = default!;
        public string? UserPublicId { get; set; }
        public string Email { get; set; }
        public bool RequiresEmailVerification { get; set; }
    }
}
