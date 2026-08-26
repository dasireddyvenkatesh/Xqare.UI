window.careerInsightsScrollSpy = (() => {
    let scrollHandler;
    let resizeHandler;
    let dotNetReference;
    let sectionSelector;
    let sectionAttribute;
    let activeSectionId;
    let animationFrame;

    const activateLink = (sectionId) => {
        document.querySelectorAll(".toc-link").forEach(link => {
            link.classList.toggle("toc-link--active", link.dataset.sectionId === sectionId);
        });
    };

    const scrollToSection = (sectionId) => {
        const target = getVisibleSections().find(section =>
            section.getAttribute(sectionAttribute) === sectionId);

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

    const getVisibleSections = () => Array.from(document.querySelectorAll(sectionSelector || ".article-section"))
        .filter(section => section.getClientRects().length > 0);

    const updateActiveSection = () => {
        const sections = getVisibleSections();
        if (!sections.length) {
            return;
        }

        // The section whose heading has most recently passed the reader's
        // viewport position is the current section. This remains reliable
        // even when several sections are visible at the same time.
        const readingLine = Math.min(window.innerHeight * 0.28, 220);
        const currentSection = sections.reduce((current, section) => {
            const top = section.getBoundingClientRect().top;
            return top <= readingLine ? section : current;
        }, null) ?? sections[0];
        const sectionId = currentSection.getAttribute(sectionAttribute);

        if (!sectionId || sectionId === activeSectionId) {
            return;
        }

        activeSectionId = sectionId;
        activateLink(sectionId);
        dotNetReference?.invokeMethodAsync("SetActiveSection", sectionId);
    };

    const scheduleActiveSectionUpdate = () => {
        if (animationFrame) {
            return;
        }

        animationFrame = requestAnimationFrame(() => {
            animationFrame = null;
            updateActiveSection();
        });
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
            this.dispose();

            dotNetReference = dotNetRef;
            sectionSelector = selector;
            sectionAttribute = attributeName;
            activeSectionId = null;
            scrollHandler = scheduleActiveSectionUpdate;
            resizeHandler = scheduleActiveSectionUpdate;

            window.addEventListener("scroll", scrollHandler, { passive: true });
            window.addEventListener("resize", resizeHandler);
            updateActiveSection();
        },
        scrollToSection(sectionId) {
            scrollToSection(sectionId);
        },
        dispose() {
            if (scrollHandler) {
                window.removeEventListener("scroll", scrollHandler);
                scrollHandler = null;
            }

            if (resizeHandler) {
                window.removeEventListener("resize", resizeHandler);
                resizeHandler = null;
            }

            if (animationFrame) {
                cancelAnimationFrame(animationFrame);
                animationFrame = null;
            }

            dotNetReference = null;
        }
    };
})();
