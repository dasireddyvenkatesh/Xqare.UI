window.xqareNavigation = window.xqareNavigation || {};

window.xqareNavigation.replaceUrl = function (url) {
    window.history.replaceState(window.history.state, document.title, url);
};
