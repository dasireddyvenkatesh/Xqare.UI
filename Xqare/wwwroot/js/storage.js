window.sessionStorageHelper = {
    set: (key, value) => sessionStorage.setItem(key, value),
    get: (key) => sessionStorage.getItem(key),
    remove: (key) => sessionStorage.removeItem(key)
};

// Serialize refresh-token rotation across tabs on this origin.
// Browsers without Web Locks retain the existing in-tab SemaphoreSlim protection.
window.sessionRefreshCoordinator = {
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
    }
};
