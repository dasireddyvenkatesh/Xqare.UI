window.deviceHelper = {

    getDeviceId: function () {

        let deviceId = localStorage.getItem("deviceId");

        if (!deviceId) {
            deviceId = crypto.randomUUID();
            localStorage.setItem("deviceId", deviceId);
        }

        return deviceId;
    },

    getDeviceName: function () {

        const ua = navigator.userAgent;

        let browser = "Unknown";
        let os = "Unknown";

        if (ua.includes("Chrome")) browser = "Chrome";
        if (ua.includes("Firefox")) browser = "Firefox";
        if (ua.includes("Safari") && !ua.includes("Chrome")) browser = "Safari";
        if (ua.includes("Edg")) browser = "Edge";

        if (ua.includes("Windows")) os = "Windows";
        if (ua.includes("Mac")) os = "MacOS";
        if (ua.includes("Android")) os = "Android";
        if (ua.includes("iPhone") || ua.includes("iPad")) os = "iOS";

        return `${browser} on ${os}`;
    }
};