import { defineConfig } from 'vite';
import { resolve, relative, dirname } from 'path';
import { globSync } from 'glob';

// 1. Find all your scoped component SCSS files
const scopedScssFiles = globSync([
    '../**/*.razor.scss', 
    '../../modules/**/*.razor.scss', 
    '../../Blazor.Shared/**/*.razor.scss'], {
    ignore: ['../**/node_modules/**', '../**/bin/**', '../**/obj/**',
        '../../modules/**/node_modules/**', '../../modules/**/bin/**', '../../modules/**/obj/**',
        '../../Blazor.Shared/**/node_modules/**', '.../../Blazor.Shared/**/bin/**', '../../Blazor.Shared/**/obj/**']
});

// 2. Map inputs dynamically for Rollup
const inputs = {
    'global_app': resolve(__dirname, 'src/app.scss'),
    'app': resolve(__dirname, 'src/app.js') // Added JavaScript entry point
};

const originalPaths = {};
scopedScssFiles.forEach(file => {
    // Create a clean key name based on its relative path
    // const relativePath = relative(resolve(__dirname, '..'), file);
    // inputs[relativePath] = resolve(__dirname, file);
    // Create a SAFE key for Rollup by replacing invalid path traversal characters

    const trueRelativePath = relative(resolve(__dirname, '..'), file);

    const safeKey = trueRelativePath.replace(/[\/\\]|^\.+/g, '-');

    inputs[safeKey] = resolve(__dirname, file);
    originalPaths[safeKey] = trueRelativePath; // Store for asset name recovery
});

export default defineConfig({
    build: {
        outDir: resolve(__dirname, '..'), // Output relative to the project root
        emptyOutDir: false,
        rollupOptions: {
            input: inputs,
            output: {
                // Handle Compiled JavaScript Outputs
                entryFileNames: (chunkInfo) => {
                    if (chunkInfo.name === 'app') {
                        return 'wwwroot/js/app.js'; // Output app.js directly here
                    }
                    return 'wwwroot/js/[name].js';
                },
                // Direct CSS outputs to their intended home locations
                // Handle CSS/SCSS Asset Outputs
                assetFileNames: (assetInfo) => {
                    if (assetInfo.name && assetInfo.name.endsWith('.css')) {
                        // Vite sets assetInfo.name based on the Rollup input key (e.g., "safeKey.css")
                        const keyName = assetInfo.name.replace('.css', '');

                        if (keyName === 'global_app') {
                            return 'wwwroot/css/app.min.css';
                        }

                        // Look up the original intended path using our map
                        if (originalPaths[keyName]) {
                            // Strip out the trailing '.scss' extension from the original path
                            let originalPath = originalPaths[keyName].replace(/\.scss\$/, '');
                            return originalPath;
                        }
                        // Fallback if not found in the dynamic map
                        // let originalName = assetInfo.name.replace('.razor.css', '.razor');
                        let originalName = assetInfo.name.replace('.razor.css', '');
                        return `wwwroot/css/${originalName.replace(/^-+/, '').replace(/^modules-+/, 'FSH.Starter.Blazor.Modules-').replace('-','.')}.css`;
                        // let originalName = assetInfo.name.replace('.razor.css', '.razor');
 
                        // if (originalName.includes('global_app')) {
                        //     return 'wwwroot/css/app.min.css';
                        // }
                        // Scoped component CSS output location
                        // return `${originalName}.css`;
                    }
                    return 'wwwroot/assets/[name]-[hash][extname]';
                }
            }
        },
        // lib: {
        //     entry: resolve(__dirname, 'src/index.js'),
        //     name: 'MyNodePackage',
        //     fileName: (format) => `my-node-bundle.${format}.js`,
        //     formats: ['es', 'umd']
        // },
        
    }
});
 