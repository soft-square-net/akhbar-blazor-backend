window.bos = window.bos || {};
window.bos.gridDrop = {
    _dragItem: null,
    _element: null,
    _hoveredRow: null,
    _hoveredCell: null,
    _hoveredIndex: null,
    _isDragging: false,
    _isDragOverFirst: true,
    // init: function(gridId, dotNetHelper) {
    init: function(target, dragable) {
        console.log("Initializing Grid Drop Target", target);
        console.log("Initializing Grid Drop dragable", dragable);
        document.querySelectorAll(dragable).forEach(el => {
            el.addEventListener("drag", this.ondrag)
            el.addEventListener("dragstart", this.ondragstart)
        });
        document.querySelectorAll(target).forEach(el => {
            el.addEventListener("dragover", this.ondragover)
            el.addEventListener("dragenter", this.ondragenter)
            el.addEventListener("dragleave", this.ondragleave)
            el.addEventListener("drop", this.ondrop)
            el.addEventListener("dragend", this.ondragend)
        });
    },
    ondrag: function(e) {
        // console.log(target, "drag fired");
        this._dragItem = target;
    },
    ondragstart: function(e) {
        // console.log(event.target, "dragstart fired");
        this._dragItem = target;
    },
    ondragover: function(e) {
        e.preventDefault();
        if (this._isDragOverFirst) {
            // First time dragging over the grid
            console.log("dragover first fired")
            this._isDragOverFirst = false;
        }
    },
    ondragenter: function(e) {
        console.log(target, "Enter Fierd");
        e.target.classList.add("highlighted")
    },
    ondragleave: function(e) {
        
        console.log("Leave Fierd");
        e.target.classList.remove("highlighted")
    },
    ondrop: function(e) {
        // e.preventDefault();
        console.log("Drop Fierd");
        e.target.classList.remove("highlighted")
        e.target.append(this._dragItem);
    },
    ondragend: function(e) {
        console.log("Drop Fierd");
        this._isDragOverFirst = true;
        this._dragItem = null;
    },
    createGridDropTarget: function(gridId, dotNetHelper) { },
    // Getters //
    getDragItem: () => this._dragItem,
    getElement: () => this._element,
    getHoveredRow: () => this._hoveredRow,
    getHoveredCell: () => this._hoveredCell,
    getHoveredIndex: () => this._hoveredIndex,
    isDragging: () => this._isDragging
};

window.Bos = window.Bos || {};
window.Bos.DragedItem = null;
window.Bos.Dragable = {
    init: function(id, dotnetHelper) { 
        let target = document.getElementById(id);
        target.addEventListener("drag", () => {
            dotnetHelper.invokeMethodAsync("OnDrag", "dragData");
        });
        target.addEventListener("dragstart", (e) => {
            e.dataTransfer.setData('text/plain', target.id)
            window.Bos.DragedItem = target;
            setTimeout(() => target.classList.add('dragging'), 0);
            dotnetHelper.invokeMethodAsync("OnDragStart", "dragstartData");
        });
    }
};
window.Bos.CurrentDropZone = null;
window.Bos.DropZone = {
    init: function(id, dotnetHelper) { 
        let target = document.getElementById(id);
        target.addEventListener("dragover", (e) => {
            e.preventDefault();
            dotnetHelper.invokeMethodAsync("OnDragOver", "dragoverData");
        });
        target.addEventListener("dragenter", (e) => {
            e.preventDefault();
            console.log("Adding PlaceHolder", target.parentElement)
            target.classList.add("highlighted");
            if (target === Bos.DragedItem) return;
            // Add animation class to push down
            target.classList.add('push-down');
            dotnetHelper.invokeMethodAsync("OnDragEnter", `${target.getAttribute('index') || ""}`);
        }); 
        target.addEventListener("dragleave", (e) => {
            target.classList.remove("highlighted")
             target.classList.remove('push-down');
            if (window.Bos.CurrentDropZone != null) {
                target.parentElement.remove(window.Bos.CurrentDropZone);
                window.Bos.CurrentDropZone = null;
            }
            dotnetHelper.invokeMethodAsync("OnDragLeave", "dradleaveData");
        }); 
        target.addEventListener("drop", (e) => {
            e.preventDefault();
            target.classList.remove("highlighted")

            

            dotnetHelper.invokeMethodAsync("OnDrop", "dropData");
            if (window.Bos.DragedItem) target.append(window.Bos.DragedItem);
        }); 
        target.addEventListener("dragend", (e) => {
            Bos.DragedItem.classList.remove('dragging');

            target.parentElement.querySelectorAll('.push-down').forEach(el => {
                el.classList.remove('push-down');
            });
            
             Bos.DragedItem = null;
            dotnetHelper.invokeMethodAsync("OnDragEnd", "dragendData");
        });
    }
};