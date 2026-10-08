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

const editors = {};

export async function initMonacoEditor(elementId, initialValue, dotnetRef, readOnly,cdnUrl, customScriptText,jsonSchemaJson) {
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
        if (!container) {
            console.warn(`[Monaco] Target container element #${elementId} not found.`);
            return;
        }

        if (!window.monaco) {
            console.error("[Monaco] Monaco library is not loaded.");
            return;
        }

        if (editors[elementId]) {
            editors[elementId].dispose();
            delete editors[elementId];
        }

        // --- CONFIGURE JSON SCHEMA VALIDATION & INTELLISENSE ---
        if (jsonSchemaJson) {
            try {
                const parsedSchema = typeof jsonSchemaJson === "string"
                    ? JSON.parse(jsonSchemaJson)
                    : jsonSchemaJson;

                monaco.languages.json.jsonDefaults.setDiagnosticsOptions({
                    validate: true,
                    allowComments: false,
                    schemas: [
                        {
                            uri: "http://myschema/dynamic-input-schema.json", // Unique virtual URI
                            fileMatch: ["*"], // Match all JSON documents loaded in this editor
                            schema: parsedSchema
                        }
                    ]
                });
            } catch (e) {
                console.error("[Monaco] Invalid JSON Schema provided:", e);
            }
        }

        const editor = monaco.editor.create(container, {
            value: initialValue || "{\n  \n}",
            language: "json",
            theme: "vs-dark",
            automaticLayout: true, // Recalculates dimensions on DOM container resize
            readOnly: readOnly,
            minimap: { enabled: false },
            scrollBeyondLastLine: false,
            fontSize: 13
        });

        editors[elementId] = editor;

        // --- KEYBINDINGS REGISTRATION ---

        // 1. Escape key -> Triggers OnEscapePressed in Blazor
        editor.addCommand(monaco.KeyCode.Escape, () => {
            dotnetRef.invokeMethodAsync("OnEscapePressed");
        });

        // 2. Ctrl + S (or Cmd + S on Mac) -> Triggers OnSavePressed in Blazor
        editor.addCommand(monaco.KeyMod.CtrlCmd | monaco.KeyCode.KeyS, () => {
            dotnetRef.invokeMethodAsync("OnSavePressed");
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

// Forces Monaco to re-calculate width & height when expanding to full-screen
export function resizeMonacoEditor(elementId) {
    const editor = editors[elementId];
    if (editor) {
        editor.layout();
    }
}

export function updateMonacoContent(elementId, newValue) {
    const editor = editors[elementId];
    if (editor && editor.getValue() !== newValue) {
        editor.setValue(newValue || "");
    }
}

export function disposeMonaco(elementId) {
    if (editors[elementId]) {
        editors[elementId].dispose();
        delete editors[elementId];
    }
}

export function focusMonacoEditor(elementId) {
    const editor = editors[elementId];
    if (editor) {
        editor.focus();

        // Scroll element into view smoothly if off-screen
        const container = document.getElementById(elementId);
        if (container) {
            container.scrollIntoView({ behavior: 'smooth', block: 'center' });
        }
    }
}
