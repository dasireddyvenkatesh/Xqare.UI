window.sessionStorageHelper = {
    set: (key, value) => sessionStorage.setItem(key, value),
    get: (key) => sessionStorage.getItem(key),
    remove: (key) => sessionStorage.removeItem(key),

    // A non-sensitive hint survives tab closure. It only indicates that this
    // browser previously established a session; the HttpOnly cookie and API
    // remain authoritative when the session is restored.
    hasSessionHint: () => localStorage.getItem("xq_session_hint_v1") === "1",
    setSessionHint: () => localStorage.setItem("xq_session_hint_v1", "1"),
    clearSessionHint: () => localStorage.removeItem("xq_session_hint_v1")
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

        runExclusiveAction: async (dotNetReference, methodName) => {
            const execute = () => dotNetReference.invokeMethodAsync(methodName);

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
                if (event.data?.type === "logout") {
                    dotNetReference.invokeMethodAsync("HandleCrossTabLogoutAsync");
                } else if (event.data?.type === "session-established") {
                    dotNetReference.invokeMethodAsync("HandleCrossTabSessionEstablishedAsync");
                }
            };

            currentChannel.removeEventListener("message", logoutListener);
            currentChannel.addEventListener("message", logoutListener);
            return true;
        },

        broadcastLogout: () => {
            getChannel()?.postMessage({ type: "logout" });
        },

        broadcastSessionEstablished: () => {
            getChannel()?.postMessage({ type: "session-established" });
        }
    };
})();
