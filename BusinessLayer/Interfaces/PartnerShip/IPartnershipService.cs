using Xqare.BusinessLayer.DTO;

namespace Xqare.BusinessLayer.Interfaces.PartnerShip
{
    public interface IPartnershipService
    {
        Task<PartnershipInquiryResponse> SubmitInquiryAsync(PartnershipInquiryRequest request);
    }
}
