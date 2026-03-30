namespace Xqare.BusinessLayer.Interfaces.Common
{
    public interface IDeviceService
    {
        Task<string> GetDeviceNameAsync();
        Task<string> GetDeviceIdAsync();
    }
}
