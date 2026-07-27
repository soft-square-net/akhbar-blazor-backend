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
window.helpers.selectFileNameWithoutExtension = (elementId) => {
  // MudTextField renders an inner 'input' or 'textarea'
  const element = document.getElementById(elementId);
  if (!element) return;

  const input = element.querySelector('input') || element.querySelector('textarea');
  if (!input) return;

  const fullText = input.value;
  const lastDotIndex = fullText.lastIndexOf('.');

  // If there is no dot, or it starts with a dot, select everything
  const selectionEnd = lastDotIndex > 0 ? lastDotIndex : fullText.length;

  input.focus();
  input.setSelectionRange(0, selectionEnd);
};

window.helpers.launchApp = () => { }

