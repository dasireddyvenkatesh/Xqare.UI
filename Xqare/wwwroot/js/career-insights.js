window.careerInsightsScrollSpy = (() => {
    let observer;

    const activateLink = (sectionId) => {
        document.querySelectorAll(".toc-link").forEach(link => {
            link.classList.toggle("toc-link--active", link.dataset.sectionId === sectionId);
        });
    };

    const scrollToSection = (sectionId) => {
        const target = document.getElementById(sectionId);

        if (!target) {
            return;
        }

        target.scrollIntoView({
            behavior: "smooth",
            block: "start"
        });

        activateLink(sectionId);
        history.replaceState(null, "", `${location.pathname}${location.search}#${sectionId}`);
    };

    document.addEventListener("click", event => {
        const link = event.target.closest(".toc-link[data-section-id]");

        if (!link) {
            return;
        }

        const sectionId = link.dataset.sectionId;

        if (!sectionId || !document.getElementById(sectionId)) {
            return;
        }

        event.preventDefault();
        scrollToSection(sectionId);
    });

    return {
        register(dotNetRef, selector, attributeName) {
            if (observer) {
                observer.disconnect();
            }

            const sections = Array.from(document.querySelectorAll(selector));
            if (!sections.length) {
                return;
            }

            observer = new IntersectionObserver((entries) => {
                const visible = entries
                    .filter(entry => entry.isIntersecting)
                    .sort((a, b) => a.boundingClientRect.top - b.boundingClientRect.top)[0];

                if (visible) {
                    const sectionId = visible.target.getAttribute(attributeName);
                    activateLink(sectionId);
                    dotNetRef.invokeMethodAsync("SetActiveSection", sectionId);
                }
            }, {
                rootMargin: "-12% 0px -70% 0px",
                threshold: [0, 0.15, 0.35, 0.65]
            });

            sections.forEach(section => observer.observe(section));
        },
        scrollToSection(sectionId) {
            scrollToSection(sectionId);
        },
        dispose() {
            if (observer) {
                observer.disconnect();
                observer = null;
            }
        }
    };
})();
