//window.mudBlazorDialogHelper = {
//  init: (dialogId) => {
//    const dialog = document.getElementById(dialogId);
//    const header = dialog?.querySelector('.mud-dialog-title');
//    if (!header) return;

//    let pos1 = 0, pos2 = 0, pos3 = 0, pos4 = 0;
//    header.style.cursor = 'move';
//    header.onmousedown = (e) => {
//      pos3 = e.clientX;
//      pos4 = e.clientY;
//      document.onmouseup = () => { document.onmouseup = null; document.onmousemove = null; };
//      document.onmousemove = (e) => {
//        pos1 = pos3 - e.clientX;
//        pos2 = pos4 - e.clientY;
//        pos3 = e.clientX;
//        pos4 = e.clientY;
//        dialog.style.top = (dialog.offsetTop - pos2) + "px";
//        dialog.style.left = (dialog.offsetLeft - pos1) + "px";
//        dialog.style.position = 'absolute';
//      };
//    };
//  }
//};


window.mudBlazorDialogHelper = {
  init: (dialogId) => {
    const dialog = document.getElementById(dialogId);
    console.log("Dialog ID :", dialogId);
    console.log(dialog);
    if (!dialog) return;

    const header = dialog.querySelector('.mud-dialog-title');
    const container = dialog.closest('.mud-dialog-container'); // Controls the overlay
    console.log("Header :", header);
    // 1. Setup Draggable (Move)
    if (header) {
      header.style.cursor = 'move';
      header.onmousedown = (e) => {
        let startX = e.clientX, startY = e.clientY;
        document.onmousemove = (moveEvent) => {
          dialog.style.left = (dialog.offsetLeft + (moveEvent.clientX - startX)) + "px";
          dialog.style.top = (dialog.offsetTop + (moveEvent.clientY - startY)) + "px";
          startX = moveEvent.clientX;
          startY = moveEvent.clientY;
          dialog.style.position = 'absolute';
          dialog.style.margin = '0'; // Remove default centering
        };
        document.onmouseup = () => document.onmousemove = null;
      };
    }

    // 2. Setup Resizable
    dialog.style.resize = 'both';
    dialog.style.overflow = 'auto';
    dialog.style.minWidth = '200px';
    dialog.style.minHeight = '150px';
  }
};
