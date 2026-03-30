using System.Net;

namespace Xqare.BusinessLayer.DTO
{
    public class ApiResponse<T>
    {
        public bool IsSuccess { get; set; }

        public HttpStatusCode StatusCode { get; set; }

        public string RawJson { get; set; } = default!;

        public string? ErrorMessage { get; set; }

        public T Data { get; set; } = default!;
    }
}
