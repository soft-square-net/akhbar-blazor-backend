// wwwroot/js/monacoEditor.js
function loadExternalScript(url) {
    return new Promise((resolve, reject) => {
        // Prevent duplicate script elements
        if (document.querySelector(`script[src="${url}"]`)) {
            resolve();
            return;
        }
        const script = document.createElement('script');
        script.src = url;
        script.type = 'text/javascript';
        script.onload = () => resolve();
        script.onerror = (err) => reject(err);
        document.head.appendChild(script);
    });
}

export async function initMonacoEditor(elementId, initialValue, dotnetRef, readOnly,cdnUrl, customScriptText) {
    try {
        // STEP 1: Await the online library script library first
        await loadExternalScript(cdnUrl);
        console.log("Online script library loaded successfully.");

        // STEP 2: Configure Monaco's internal loader to know where to pull modules from
        // We drop 'vs/loader.js' from the end of the URL to get the base path
        const basePath = cdnUrl.replace('/vs/loader.min.js', '/vs');
        console.log("basePath = " + basePath);

        window.require.config({
            paths: { 'vs': basePath }
        });

        // STEP 3: Wrap Monaco's load lifecycle in a Promise so we can await it
        await new Promise((resolve, reject) => {
            window.require(['vs/editor/editor.main'], function () {
                console.log("Monaco core libraries loaded. 'monaco' object is now available.");
                resolve();
            }, function (err) {
                reject(err);
            });
        });

        // STEP 4: Now that 'window.monaco' exists, safely execute your custom instantiation code
        const container = document.getElementById(elementId);
        if (!container) return null;

        const editor = monaco.editor.create(container, {
            value: initialValue || "{\n  \n}",
            language: "json",
            theme: "vs-dark",
            automaticLayout: true,
            readOnly: readOnly,
            minimap: { enabled: false },
            scrollBeyondLastLine: false,
            fontSize: 13
        });

        // Debounce updates back to Blazor when typing
        let timeout = null;
        editor.onDidChangeModelContent(() => {
            clearTimeout(timeout);
            timeout = setTimeout(() => {
                const currentValue = editor.getValue();
                dotnetRef.invokeMethodAsync("OnJsEditorContentChanged", currentValue);
            }, 300);
        });

        if (customScriptText) {
            const executeCustomScript = new Function(customScriptText);
            executeCustomScript();
        }
        return editor;


    } catch (error) {
        console.error("Failed inside monacoEditor.js sequence:", error);
    };
}

export function updateMonacoContent(editorInstance, newValue) {
    if (editorInstance && editorInstance.getValue() !== newValue) {
        editorInstance.setValue(newValue || "");
    }
}

export function disposeMonaco(editorInstance) {
    if (editorInstance) {
        editorInstance.dispose();
    }
}