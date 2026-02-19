window.bos = window.bos || {};
window.bos.gridDrop = {
    _dragItem: null,
    _element: null,
    _hoveredRow: null,
    _hoveredCell: null,
    _hoveredIndex: null,
    _isDragging: false,
    _isDragOverFirst: true,
    init: function(gridId, dotNetHelper) { },
    ondrag: function(e, target) {
        // console.log(target, "drag fired");
        this._dragItem = target;
    },
    ondragstart: function(e, target) {
        // console.log(event.target, "dragstart fired");
        this._dragItem = target;
    },
    ondragover: function(e, target) {
        // e.preventDefault();
        if (this._isDragOverFirst) {
            // First time dragging over the grid
            this._isDragOverFirst = false;
        }
    },
    ondragenter: function(e, target) {
        console.log(target, "Enter Fierd");
        target.classList.add("highlighted")
    },
    ondragleave: function(e, target) {
        
        console.log("Leave Fierd");
        target.classList.remove("highlighted")
    },
    ondrop: function(e, target) {
        // e.preventDefault();
        console.log("Drop Fierd");
        target.classList.remove("highlighted")
        target.append(this._dragItem);
    },
    ondragend: function(e, target) {
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