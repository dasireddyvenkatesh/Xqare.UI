using Xqare.BusinessLayer.DTO;
using Xqare.BusinessLayer.Interfaces.Common;
using Xqare.BusinessLayer.Interfaces.ContactUs;

namespace Xqare.BusinessLayer.Classes.ContactUs
{
    public class ContactUsService : IContactUsService
    {
        private readonly IApiClient _api;

        public ContactUsService(IApiClient api)
        {
            _api = api;
        }

        public async Task<ContactUsResponse> SendContactUsMessageAsync(ContactUsRequest request)
        {
            // Contact is a public endpoint. Do not attach a stale bearer token
            // or let a previously rejected refresh state block anonymous submissions.
            var response = await _api.PostAnonymousAsync<ContactUsRequest, ContactUsResponse>(
                "api/contactus",
                request);

            return response.Data;
        }
    }
}