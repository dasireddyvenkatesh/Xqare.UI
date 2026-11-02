using Xqare.BusinessLayer.DTO;

namespace Xqare.BusinessLayer.Interfaces.ContactUs
{
    public interface IContactUsService
    {
        Task<ContactUsResponse> SendContactUsMessageAsync(ContactUsRequest request);
    }
}
