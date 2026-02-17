if (!window.helpers) window.helpers = {}
window.helpers.getWindowDimensions = function () {
    return {
        width: window.innerWidth,
        height: window.innerHeight
    };
}
window.helpers.registerResizeCallback = (dotnetHelper) => {
    window.addEventListener("resize", () => {
        dotnetHelper.invokeMethodAsync("OnBrowserResize", window.innerWidth, window.innerHeight);
    });
};

window.helpers.launchApp = () => { }

