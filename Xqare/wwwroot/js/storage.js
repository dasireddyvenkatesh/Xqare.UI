window.sessionStorageHelper = {
    set: (key, value) => sessionStorage.setItem(key, value),
    get: (key) => sessionStorage.getItem(key),
    remove: (key) => sessionStorage.removeItem(key)
};

// Coordinate refresh-token rotation and logout between tabs on this UI origin.
// Access tokens are never broadcast; each tab keeps its own sessionStorage token.
window.sessionRefreshCoordinator = (() => {
    const channelName = "xqare-auth-session";
    let channel = null;
    let logoutListener = null;

    const getChannel = () => {
        if (typeof BroadcastChannel === "undefined")
            return null;

        if (!channel)
            channel = new BroadcastChannel(channelName);

        return channel;
    };

    return {
        runExclusive: async (dotNetReference, methodName, accessTokenUsed) => {
            const execute = () => dotNetReference.invokeMethodAsync(methodName, accessTokenUsed);

            if (navigator.locks && typeof navigator.locks.request === "function") {
                return await navigator.locks.request(
                    "xqare-auth-refresh",
                    { mode: "exclusive" },
                    execute
                );
            }

            return await execute();
        },

        registerLogoutListener: dotNetReference => {
            const currentChannel = getChannel();
            if (!currentChannel)
                return false;

            logoutListener = event => {
                if (event.data?.type === "logout")
                    dotNetReference.invokeMethodAsync("HandleCrossTabLogoutAsync");
            };

            currentChannel.removeEventListener("message", logoutListener);
            currentChannel.addEventListener("message", logoutListener);
            return true;
        },

        broadcastLogout: () => {
            getChannel()?.postMessage({ type: "logout" });
        }
    };
})();
