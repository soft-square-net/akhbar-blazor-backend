export function runNodeLogic() {
    console.log("Node package logic running via Vite!");
    return "Hello from Vite-built package";
}

// Expose to window for Blazor JS Interop if using UMD/IIFE, or import via JS modules
window.MyNodePackage = { runNodeLogic };