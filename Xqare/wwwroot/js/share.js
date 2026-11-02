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