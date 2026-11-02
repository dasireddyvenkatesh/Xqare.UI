using Xqare.BusinessLayer.DTO;

namespace Xqare.BusinessLayer.Interfaces.Profile
{
    public interface IProfileService
    {
        Task<MyProfileResponse> GetMyProfileAsync();
        Task<MyProfileResponse> UpdateMyProfileAsync(UpdateMyProfileRequest request);
    }
}
