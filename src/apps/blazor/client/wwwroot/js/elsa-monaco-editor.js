let editorInstance = null;

export function initializeEditor() {
    // 1. Check if Monaco loader is already loaded; if not, inject it dynamically
    if (typeof window.require === 'undefined' || typeof window.require.config === 'undefined') {
        // Cache a backup of the conflicting bundler object if it exists
        if (window.require && typeof window.require.config === 'undefined') {
            window.originalBundlerRequire = window.require;
        }

        const script = document.createElement('script');
        script.src = "_content/BlazorMonaco/lib/monaco-editor/min/vs/loader.js";
        script.async = true;
        script.onload = () => configureAndCreateMonaco();
        document.body.appendChild(script);
    } else {
        configureAndCreateMonaco();
    }
}

function configureAndCreateMonaco() {
    if (typeof window.require !== 'undefined' && typeof window.require.config === 'function') {

        window.require.config({
            paths: { 'vs': '_content/BlazorMonaco/lib/monaco-editor/min/vs' }
        });


        // 3. Load the editor using the AMD loader
        window.require(['vs/editor/editor.main'], function () {
            window.require(['_content/BlazorMonaco/jsInterop.js'], function () {
                alert('Monaco Editor Loaded!!');
            });
        });
    } else {
        console.error("Monaco AMD Loader could not be claimed due to a conflicting global variable wrapper.");
    }
    //// 3. Load the editor using the AMD loader
    //require(['vs/editor/editor.main'], function () {
    //    const container = document.getElementById(containerId);
    //    if (container) {
    //        editorInstance = monaco.editor.create(container, {
    //            value: "function helloWorld() {\n\tconsole.log('Hello from Monaco!');\n}",
    //            language: 'javascript',
    //            theme: 'vs-dark',
    //            automaticLayout: true
    //        });
    //    }
    //});
}

// 4. Cleanup function to safely destroy the editor instances
export function disposeEditor() {
    if (editorInstance) {
        editorInstance.dispose();
        editorInstance = null;
    }

    // Restore original framework bundler functions to prevent breaking Elsa Studio layout
    if (window.originalBundlerRequire) {
        window.require = window.originalBundlerRequire;
    }
}
