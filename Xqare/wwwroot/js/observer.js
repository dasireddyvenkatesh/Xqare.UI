window.observeElement = (element, dotnetRef) => {

    const observer = new IntersectionObserver(entries => {

        if (entries[0].isIntersecting) {
            dotnetRef.invokeMethodAsync("Trigger");
            observer.disconnect();
        }

    }, { threshold: 0.3 });

    observer.observe(element);
};
