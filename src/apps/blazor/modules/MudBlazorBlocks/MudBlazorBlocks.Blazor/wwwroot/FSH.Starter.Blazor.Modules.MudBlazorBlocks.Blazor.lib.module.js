// MySharedUi.lib.module.js

/**
 * Dynamically loads a script or injects inline code into the document and returns a Promise.
 * @param {string} url - The URL/path of the script, or "data:" for inline content.
 * @param {boolean} header - Whether to append to <head> (true) or <body> (false).
 * @param {string} [content] - Optional inline script content.
 * @returns {Promise<void>}
 */
function loadScript(url, header = false, content = "") {
  return new Promise((resolve, reject) => {
    const script = document.createElement('script');
    script.async = true;

    // Handle inline script content
    if (url === "data:" || content) {
      script.textContent = content;

      if (header) {
        document.head.appendChild(script);
      } else {
        document.body.appendChild(script);
      }

      // CRITICAL FIX: Inline scripts execute immediately and do NOT fire onload events.
      // Resolve immediately so the Promise doesn't hang.
      console.log("Inline script injected and executed successfully.");
      resolve();
      return;
    }

    // Handle external script loading
    script.src = url;
    script.onload = () => {
      console.log(`Script loaded successfully: ${url}`);
      resolve();
    };

    script.onerror = (error) => {
      console.error(`Failed to load script: ${url}`, error);
      reject(error);
    };

    if (header) {
      document.head.appendChild(script);
    } else {
      document.body.appendChild(script);
    }
  });
}

/**
 * Dynamically loads a stylesheet into the document head and returns a Promise.
 * @param {string} url - The URL/path of the stylesheet to load.
 * @returns {Promise<void>}
 */
function loadStyleSheet(url) {
  return new Promise((resolve, reject) => {
    const link = document.createElement('link');
    link.rel = 'stylesheet';
    link.href = url;

    link.onload = () => {
      console.log(`Stylesheet loaded successfully: ${url}`);
      resolve();
    };

    link.onerror = (error) => {
      console.error(`Failed to load stylesheet: ${url}`, error);
      reject(error);
    };

    document.head.appendChild(link);
  });
}

/**
 * Executed before the Blazor WebAssembly runtime starts.
 * Returns a Promise so Blazor awaits its completion before booting up.
 */
export async function beforeStart(options, extensions) {
  console.log("Preloading module: starting asynchronous initialization...");

  try {
    // 1. Load Tailwind CSS CDN
    await loadScript("https://cdn.tailwindcss.com", true);

    // 2. Inject Tailwind configuration inline (Now fixed to resolve instantly)
    await loadScript("data:", true, `
        tailwind.config = {
            theme: {
                extend: {
                    colors: {
                        mud: {
                            primary: '#594AE2',
                            'primary-hover': '#4535C5',
                            secondary: '#FF4081',
                            surface: '#FFFFFF',
                            background: '#F5F5F7',
                            appbar: '#594AE2',
                            border: '#E0E0E0',
                            text: '#424242',
                            dark: '#1E1E2D'
                        }
                    }
                }
            }
        }
    `);

    // 3. Load Stylesheets
    await loadStyleSheet("https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css");
    await loadStyleSheet("https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500;700&display=swap");

    // 4. Load Custom RCL Script
    const scriptUrl = "/_content/FSH.Starter.Blazor.Modules.MudBlazorBlocks.Blazor/js/tailwindEditor.js";
    await loadScript(scriptUrl, true);

    console.log("Preload & Tailwind initialization finished. Proceeding with Blazor startup.");
  } catch (error) {
    console.error("An error occurred during Blazor beforeStart initialization:", error);
    throw error;
  }
}

/**
 * Executed after Blazor has successfully started.
 */
export async function afterStarted(blazor) {
  console.log("Blazor has successfully started.", blazor);
}