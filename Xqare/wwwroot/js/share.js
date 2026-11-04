window.shareArticle = async function (url, title) {
    if (navigator.share) {
        try {
            await navigator.share({
                title: title,
                url: url
            });
        } catch (error) {
            // User cancelled the share dialog.
        }
    } else {
        try {
            await navigator.clipboard.writeText(url);
        } catch (error) {
            console.error("Could not copy URL:", error);
        }
    }
};
window.xqareNavigation = window.xqareNavigation || {};

window.xqareNavigation.replaceUrl = function (url) {
    window.history.replaceState(window.history.state, document.title, url);
};

// Serialize refresh-cookie rotation across tabs sharing the same frontend origin.
// The lock is held until the .NET refresh operation completes and the browser has
// processed its Set-Cookie response. No token is read from or written to browser storage here.
window.xqareSession = window.xqareSession || {};

window.xqareSession.withRefreshLock = async function (dotNetReference, accessTokenUsed) {
    const refresh = () => dotNetReference.invokeMethodAsync(
        "RefreshTokenUnderBrowserLock",
        accessTokenUsed
    );

    if (!navigator.locks || typeof navigator.locks.request !== "function") {
        return await refresh();
    }

    return await navigator.locks.request(
        "xqare-auth-refresh",
        { mode: "exclusive" },
        refresh
    );
};
