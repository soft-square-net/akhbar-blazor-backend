
window.Bos = window.Bos || {};
// window.Bos.DragedItem = null;

// window.Bos.CurrentDropZone = null;

window.Bos.Helpers = {
    GetPosFromIndex: (index, colSize, lnkWidth, lnkHeight) => {
        return {
            x: (index / colSize) * lnkWidt,
            y: (index % colSize) * lnkHeight,
        }
    },
    GePosColRow: (x, y, lnkWidth, lnkHeight) => {
        return {
            col: (x / lnkWidth),
            row: (y % lnkHeight),
        }
    },
    GeIndexFromPos: (x, y, lnkWidth, lnkHeight) => {
        let cr = GePosColRow(x, y, lnkWidth, lnkHeight);
        return cr.x * cr.y;
    }
}
pos = { x: null, y: null };
diff = { x: null, y: null };
selectedItem = null;
window.Bos.Dragging = (() =>
({
    colsize: 1,
    width: 0,
    height: 0,
    itemSelector: '.bos-desktop .drop-zone .dragable',
    itemsEle: document.querySelector('.drop-zone'),
    itemsList: [],
    mouseDown: false,
    resetTransition: false,
    transitionTime: 400,
    init: async (id, dotnetHelper) => { 
        Bos.Dragging.itemsEle = document.getElementById(id);
        Bos.Dragging.width = Bos.Dragging.itemsEle.offsetWidth;
        Bos.Dragging.height = Bos.Dragging.itemsEle.offsetHeight;
        Bos.Dragging.itemsList = document.querySelectorAll(Bos.Dragging.itemSelector); Bos.Dragging.itemsList = Array.prototype.slice.call(Bos.Dragging.itemsList); Bos.Dragging.itemsList = Bos.Dragging.itemsList.filter(item => item.getAttribute('selected') !== 'yes');
        await Bos.Dragging.positionItems();
        Bos.Dragging.itemsList.forEach(function(item) {
            item.addEventListener('mousedown', function(e) {
                console.log("Mouse button pressed down!");
                if (!pos.x || Bos.Dragging.resetTransition) return;
                Bos.Dragging.mouseDown = true, selectedItem = item;
                diff.y = pos.y - item.offsetTop, diff.x = pos.x - item.offsetLeft;
                let offsetY = pos.y - diff.y, offsetX = pos.x - diff.x;
                item.style.top = offsetY + 'px';
                item.style.left = offsetX + 'px';
                item.style.zIndex = '1000';
                item.setAttribute('selected', 'yes');
            });
            item.addEventListener('mouseup', function(e) {
                console.log("Mouse button pressed up!");
                Bos.Dragging.mouseDown = false;
                Bos.Dragging.positionItemsInOrder();
            });
        });
    },
    positionItems: (insertIndex = null) => { 
        let indexCounter = 0;
        let currentCol = 0;
        Bos.Dragging.colsize = parseInt(Bos.Dragging.height / 102);
        Bos.Dragging.itemsList.forEach(function(item) {
            let itemRow = indexCounter % Bos.Dragging.colsize;
            if (itemRow == 0 && indexCounter > 0) {
                currentCol++;
            }
            if (insertIndex === indexCounter + 1) {
                indexCounter++;
            }

            item.style.left = (60 * currentCol) + (currentCol * 10) + 'px';
            item.style.top = (92 * itemRow) + (itemRow * 10) + 'px';
            item.setAttribute('order', indexCounter + 1);
            indexCounter++;
        });
    },
    mousemove: function(e) {
        if (!Bos.Dragging.itemsEle) return;
        pos.x = e.clientX - Bos.Dragging.itemsEle.offsetLeft, pos.y = e.clientY - (Bos.Dragging.itemsEle.offsetTop - window.scrollY); //ScrollY, so that a vertical scroll bar does not mess everything up
        if (!Bos.Dragging.mouseDown) return;
        let offsetY = pos.y - diff.y, offsetX = pos.x - diff.x;
        selectedItem.style.top = offsetY + 'px';
        selectedItem.style.left = offsetX + 'px';
        Bos.Dragging.itemsList = document.querySelectorAll(Bos.Dragging.itemSelector); Bos.Dragging.itemsList = Array.prototype.slice.call(Bos.Dragging.itemsList); Bos.Dragging.itemsList = Bos.Dragging.itemsList.filter(item => item.getAttribute('selected') !== 'yes');
        let orderOfSelectedItem = Number(selectedItem.getAttribute('order'));
        let currentCol = parseInt(pos.x / 70);
        //Test for new position
        if (orderOfSelectedItem !== 1) {
            
            let beforeItem = document.querySelector(`${Bos.Dragging.itemSelector}[order*="${orderOfSelectedItem - 1}"]`);
            if (beforeItem) { 
                let beforeMiddle = pos.y < beforeItem.offsetTop + (beforeItem.clientHeight / 2);
                if (beforeMiddle) {
                    Bos.Dragging.positionItems(orderOfSelectedItem - 1);
                    selectedItem.setAttribute('order', orderOfSelectedItem - 1);
                    return;
                }
            }
        };
        if (orderOfSelectedItem !== document.querySelectorAll(Bos.Dragging.itemSelector).length) {
            let afterItem = document.querySelector(`${Bos.Dragging.itemSelector}[order*="${orderOfSelectedItem + 1}"]`);
            if (afterItem) { 
                let afterMiddle = pos.y > afterItem.offsetTop + (afterItem.clientHeight / 2);
                if (afterMiddle) {
                    Bos.Dragging.positionItems(orderOfSelectedItem + 1);
                    selectedItem.setAttribute('order', orderOfSelectedItem + 1);
                    return;
                }
            }
        };
    },
    positionItemsInOrder: function(e) {
        let itemsList = document.querySelectorAll(Bos.Dragging.itemSelector); itemsList = Array.prototype.slice.call(itemsList); 
        itemsList = itemsList.sort(function(a, b) {
            return Number(a.getAttribute('order')) > Number(b.getAttribute('order')) ? 1 : -1;
        });
        let currentCol = 0;
        itemsList.forEach(function(item, index) {
            let itemRow = index % Bos.Dragging.colsize;
            if (itemRow == 0 && index > 0) {
                currentCol++;
            }
            if (item.getAttribute('selected') === 'yes') {
                item.removeAttribute('selected');
                // item.style.left = '0';
                item.style.left = (60 * currentCol) + (currentCol * 10) + 'px';
                setTimeout(function() {
                    item.style.zIndex = '0';
                }, Bos.Dragging.transitionTime);
            };
            // item.style.top = (92 * index) + (index * 10) + 'px';
            item.style.top = (92 * itemRow) + (itemRow * 10) + 'px';
            item.setAttribute('order', index + 1);
        });
        Bos.Dragging.resetTransition = true;
        //When transition is over
        setTimeout(function() {
            while (Bos.Dragging.itemsEle.firstChild) {
                Bos.Dragging.itemsEle.removeChild(Bos.Dragging.itemsEle.lastChild);
            };
            itemsList.forEach(function(item) {
                Bos.Dragging.itemsEle.append(item);
            });
            Bos.Dragging.resetTransition = false;
        }, Bos.Dragging.transitionTime);
    },
})
)();


addEventListener('mousemove', window.Bos.Dragging.mousemove); 