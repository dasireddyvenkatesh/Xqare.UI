using Xqare.BusinessLayer.DTO;
using Xqare.BusinessLayer.Interfaces.PartnerShip;
using Xqare.BusinessLayer.Constants.Messages;

namespace Xqare.BusinessLayer.Classes.PartnerShip
{
    public class PartnershipService : IPartnershipService
    {
        public async Task<PartnershipInquiryResponse> SubmitInquiryAsync(PartnershipInquiryRequest request)
        {

            await Task.Delay(1000);


            return new PartnershipInquiryResponse
            {
                IsSuccess = true,
                Message = PartnershipMessages.Success
            };
        }
    }
}
