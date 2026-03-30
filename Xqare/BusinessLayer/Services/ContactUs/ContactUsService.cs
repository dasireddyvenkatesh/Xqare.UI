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

            var response = await _api.PostAsync<ContactUsRequest, ContactUsResponse>("api/contactus", request);

            return response.Data;

        }
    }
}
