using Microsoft.JSInterop;
using Xqare.BusinessLayer.Interfaces.Common;

namespace Xqare.BusinessLayer.Classes.Common
{
    public class DeviceService : IDeviceService
    {
        private readonly IJSRuntime _js;

        public DeviceService(IJSRuntime js)
        {
            _js = js;
        }

        public async Task<string> GetDeviceNameAsync()
        {
            return await _js.InvokeAsync<string>("deviceHelper.getDeviceName");
        }

        public async Task<string> GetDeviceIdAsync()
        {
            return await _js.InvokeAsync<string>("deviceHelper.getDeviceId");
        }
    }
}
