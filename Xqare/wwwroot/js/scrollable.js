window.headerScrollHandler = (dotnetHelper) => {
    window.addEventListener("scroll", () => {
        if (window.scrollY > 10) {
            dotnetHelper.invokeMethodAsync("OnScroll", true);
        } else {
            dotnetHelper.invokeMethodAsync("OnScroll", false);
        }
    });
};