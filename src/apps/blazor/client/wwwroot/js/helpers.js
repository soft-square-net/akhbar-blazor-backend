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

//  dotNetRef, item.Name, item.Id, item.Folder.BucketId)
window.helpers.showPickerThenFetch = async function (dotNetRef, suggestedFileName, bucketId, itemId) {
        try {
            // 1. Open picker instantly (Valid user gesture)
            const handle = await window.showSaveFilePicker({ suggestedName: suggestedFileName });
            
            // 2. Go back to C# asynchronously to prepare and fetch the content
            const base64Data = await dotNetRef.invokeMethodAsync('GetPreparedDataJson', bucketId, itemId);
            const binaryString = atob(base64Data);
            const bytes = new Uint8Array(binaryString.length);
            for (let i = 0; i < binaryString.length; i++) {
                bytes[i] = binaryString.charCodeAt(i);
            }

            // 3. Write it to the file
            const writable = await handle.createWritable();
            await writable.write(bytes);
            await writable.close();
        } catch (err) {
            if (err.name !== 'AbortError') console.error(err);
        }
    };


//////////////////////////////////////////////////////////////////////////////////
// window.downloadFileFromStream = async (fileName, contentStreamReference) => {
//         const arrayBuffer = await contentStreamReference.arrayBuffer();
//         const blob = new Blob([arrayBuffer]);
//         const url = URL.createObjectURL(blob);
//         const anchorElement = document.createElement('a');
//         anchorElement.href = url;
//         anchorElement.download = fileName ?? '';
//         document.body.appendChild(anchorElement);
//         anchorElement.click();
//         // anchorElement.remove();
//         URL.revokeObjectURL(url);
//         document.body.removeChild(anchorElement);
//     };


//   window.helpers.downloadFileFromStream = async function (suggestedName, contentStreamReference) {
//         try {
//             // 1. Open the native browser 'Save As' window
//             const options = {
//                 suggestedName: suggestedName,
//                 types: [{
//                     description: 'Text Files',
//                     accept: { 'text/plain': ['.txt'] },
//                 }],
//             };
            
//             // This will show a dialog box with a "Save" button
//             const fileHandle = await window.showSaveFilePicker(options);
            
//             // 2. Convert the Blazor C# stream into an array buffer
//             const arrayBuffer = await contentStreamReference.arrayBuffer();
            
//             // 3. Write the C# data into the selected local file path
//             const writableStream = await fileHandle.createWritable();
//             await writableStream.write(arrayBuffer);
//             await writableStream.close();
            
//         } catch (err) {
//             // Handle user cancellation or errors gracefully
//             if (err.name !== 'AbortError') {
//                 console.error("Failed to save file:", err);
//             }
//         }
//     }

//////////////////////////////////////////////////////////////////////////
// window.helpers.selectFileNameWithoutExtension = (elementId) => {
//   // MudTextField renders an inner 'input' or 'textarea'
//   const element = document.getElementById(elementId);
//   if (!element) return;

//   const input = element.querySelector('input') || element.querySelector('textarea');
//   if (!input) return;

//   const fullText = input.value;
//   const lastDotIndex = fullText.lastIndexOf('.');

//   // If there is no dot, or it starts with a dot, select everything
//   const selectionEnd = lastDotIndex > 0 ? lastDotIndex : fullText.length;

//   input.focus();
//   input.setSelectionRange(0, selectionEnd);
// };

window.helpers.simulateCtrlClick = (elementId) => {
    const element = document.getElementById(elementId);
    if (element) {
      const ctrlClickEvent = new MouseEvent("click", {
        bubbles: true,
        cancelable: true,
        ctrlKey: true,  // For Windows / Linux
        metaKey: true   // For macOS
      });
      element.dispatchEvent(ctrlClickEvent);
    }
};


window.helpers.launchApp = () => { }

