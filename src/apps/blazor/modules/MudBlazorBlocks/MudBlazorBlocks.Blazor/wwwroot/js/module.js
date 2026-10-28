//#region src/TailwindGridBlocksDesigner/GridBlockItem.js
var GridBlockItem$1 = class {
	constructor(type = "Paragraph", colSpan = 6, rowSpan = 1, content = "") {
		this.id = "block_" + Math.random().toString(36).substr(2, 9);
		this.type = type;
		this.colSpan = colSpan;
		this.rowSpan = rowSpan;
		this.content = content || this.getDefaultContent(type);
		this.tagName = type === "Heading" ? "h2" : "p";
		this.imageUrl = "https://images.unsplash.com/photo-1618005182384-a83a8bd57fbe?auto=format&fit=crop&w=600&q=80";
		this.customCss = "";
		this.children = [];
	}
	getDefaultContent(type) {
		switch (type) {
			case "Heading": return "WYSIWYG CSS Grid Block";
			case "Paragraph": return "Toolbars float on top of blocks on hover/focus without causing layout shift or height expansion!";
			case "CustomHtml": return "<button style=\"background:#594AE2; color:white; padding:8px 16px; border:none; border-radius:6px; font-weight:bold; cursor:pointer;\">Custom Action Button</button>";
			default: return "";
		}
	}
	renderToHtml() {
		const style = `grid-column: span ${this.colSpan}; grid-row: span ${this.rowSpan}; ${this.customCss}`.trim();
		if (this.type === "Container") return `<div style="display: grid; grid-template-columns: repeat(12, minmax(0, 1fr)); grid-auto-flow: dense; gap: 12px; ${style}">\n  ${this.children.map((c) => c.renderToHtml()).join("\n  ")}\n</div>`;
		switch (this.type) {
			case "Heading": return `<${this.tagName} style="margin:0; ${style}">${this.content}</${this.tagName}>`;
			case "Paragraph": return `<p style="margin:0; ${style}">${this.content}</p>`;
			case "Image": return `<img src="${this.imageUrl}" alt="Grid Image" style="width:100%; height:100%; object-fit:cover; border-radius:6px; ${style}" />`;
			case "CustomHtml": return `<div style="${style}">${this.content}</div>`;
			default: return "";
		}
	}
};
//#endregion
//#region src/TailwindGridBlocksDesigner/AppState.js
var AppState$1 = {
	rootContainer: new GridBlockItem("Container", 12, 1),
	selectedBlock: null,
	draggedBlock: null,
	resizingData: null,
	findBlockById(id, current = this.rootContainer) {
		if (current.id === id) return {
			item: current,
			parent: null
		};
		for (let child of current.children) {
			if (child.id === id) return {
				item: child,
				parent: current
			};
			if (child.type === "Container") {
				const found = this.findBlockById(id, child);
				if (found.item) return found;
			}
		}
		return {
			item: null,
			parent: null
		};
	},
	removeBlock(id) {
		const { item, parent } = this.findBlockById(id);
		if (item && parent) {
			const index = parent.children.indexOf(item);
			if (index > -1) {
				parent.children.splice(index, 1);
				if (this.selectedBlock?.id === id) this.selectedBlock = null;
				this.recalculateAllContainerRowSpans();
				return true;
			}
		}
		return false;
	},
	recalculateContainerRowSpan(container) {
		if (!container || container.type !== "Container") return;
		container.children.forEach((child) => {
			if (child.type === "Container") this.recalculateContainerRowSpan(child);
		});
		if (container.children.length === 0) {
			container.rowSpan = 1;
			return;
		}
		let currentCol = 0;
		let estimatedRows = 0;
		let maxRowInCurrentLine = 0;
		container.children.forEach((child) => {
			const childCols = Math.min(12, Math.max(1, child.colSpan || 1));
			const childRows = Math.max(1, child.rowSpan || 1);
			if (currentCol + childCols > 12) {
				estimatedRows += maxRowInCurrentLine;
				currentCol = childCols;
				maxRowInCurrentLine = childRows;
			} else {
				currentCol += childCols;
				maxRowInCurrentLine = Math.max(maxRowInCurrentLine, childRows);
			}
		});
		estimatedRows += maxRowInCurrentLine;
		container.rowSpan = Math.max(1, estimatedRows);
	},
	recalculateAllContainerRowSpans() {
		this.recalculateContainerRowSpan(this.rootContainer);
	}
};
//#endregion
//#region src/TailwindGridBlocksDesigner/InitDemoData.js
var initDemoData = function() {
	AppState$1.rootContainer.children = [];
	const header = new GridBlockItem$1("Heading", 12, 1, "Absolute Overlay Toolbars & Auto-Scaling RowSpans");
	header.tagName = "h1";
	AppState$1.rootContainer.children.push(header);
	const gridSection = new GridBlockItem$1("Container", 12, 1);
	const subHeading = new GridBlockItem$1("Heading", 7, 1, "Dynamic Container Resizing & Zero Shift");
	subHeading.tagName = "h3";
	const paragraph = new GridBlockItem$1("Paragraph", 7, 2, "Toolbars now float directly over block content using absolute positioning. Hovering or selecting a block will never cause layout shifts or height expansion!");
	const imgBlock = new GridBlockItem$1("Image", 5, 3);
	gridSection.children.push(subHeading, paragraph, imgBlock);
	AppState$1.rootContainer.children.push(gridSection);
	const customBlock = new GridBlockItem$1("CustomHtml", 12, 1);
	AppState$1.rootContainer.children.push(customBlock);
	AppState$1.selectedBlock = header;
	AppState$1.recalculateAllContainerRowSpans();
};
//#endregion
//#region src/TailwindGridBlocksDesigner/PropertyInspector.js
var renderInspector = function() {
	const container = document.getElementById("inspector-content");
	const badge = document.getElementById("selected-type-badge");
	const item = AppState.selectedBlock;
	if (!item) {
		badge.innerText = "None";
		badge.className = "text-[10px] font-bold px-2 py-0.5 rounded bg-gray-200 text-gray-600 uppercase";
		container.innerHTML = `
                    <div class="text-center py-12 text-gray-400 text-sm">
                        <i class="fa-regular fa-hand-pointer text-3xl mb-2 block text-gray-300"></i>
                        Select any block on the canvas to inspect properties.
                    </div>`;
		return;
	}
	badge.innerText = item.type;
	badge.className = "text-[10px] font-bold px-2 py-0.5 rounded bg-indigo-100 text-mud-primary uppercase";
	container.innerHTML = `
                <div>
                    <div class="flex justify-between text-xs font-semibold text-gray-700 mb-1">
                        <label>Column Span (1 - 12)</label>
                        <span class="text-mud-primary font-mono font-bold">${item.colSpan} Cols</span>
                    </div>
                    <div class="flex gap-2 items-center">
                        <input type="range" id="input-colspan-range" min="1" max="12" value="${item.colSpan}" class="w-full accent-mud-primary cursor-pointer">
                        <input type="number" id="input-colspan-num" min="1" max="12" value="${item.colSpan}" class="w-14 text-xs p-1 border border-gray-300 rounded text-center">
                    </div>
                </div>

                <div>
                    <div class="flex justify-between text-xs font-semibold text-gray-700 mb-1">
                        <label>Row Span (1 - 10)</label>
                        <span class="text-mud-primary font-mono font-bold">${item.rowSpan} Rows</span>
                    </div>
                    <div class="flex gap-2 items-center">
                        <input type="range" id="input-rowspan-range" min="1" max="10" value="${item.rowSpan}" class="w-full accent-mud-primary cursor-pointer">
                        <input type="number" id="input-rowspan-num" min="1" max="10" value="${item.rowSpan}" class="w-14 text-xs p-1 border border-gray-300 rounded text-center">
                    </div>
                </div>

                ${item.type === "Heading" ? `
                <div>
                    <label class="block text-xs font-semibold text-gray-700 mb-1">Heading Tag</label>
                    <select id="input-tag" class="w-full text-xs p-2 border border-gray-300 rounded-md focus:ring-2 focus:ring-mud-primary focus:outline-none">
                        <option value="h1" ${item.tagName === "h1" ? "selected" : ""}>H1 Title</option>
                        <option value="h2" ${item.tagName === "h2" ? "selected" : ""}>H2 Subtitle</option>
                        <option value="h3" ${item.tagName === "h3" ? "selected" : ""}>H3 Section Header</option>
                    </select>
                </div>
                ` : ""}

                ${item.type === "Heading" || item.type === "Paragraph" || item.type === "CustomHtml" ? `
                <div>
                    <label class="block text-xs font-semibold text-gray-700 mb-1">Text Content</label>
                    <textarea id="input-content" rows="4" class="w-full text-xs p-2.5 border border-gray-300 rounded-md focus:ring-2 focus:ring-mud-primary focus:outline-none font-sans">${item.content}</textarea>
                </div>
                ` : ""}

                ${item.type === "Image" ? `
                <div>
                    <label class="block text-xs font-semibold text-gray-700 mb-1">Image URL</label>
                    <input type="text" id="input-imageurl" value="${item.imageUrl}" class="w-full text-xs p-2 border border-gray-300 rounded-md focus:ring-2 focus:ring-mud-primary focus:outline-none">
                </div>
                ` : ""}

                <div>
                    <label class="block text-xs font-semibold text-gray-700 mb-1">Custom CSS Styles</label>
                    <input type="text" id="input-css" value="${item.customCss}" placeholder="e.g. color: #2563eb; background: #fff;" class="w-full text-xs p-2 border border-gray-300 rounded-md font-mono focus:ring-2 focus:ring-mud-primary focus:outline-none">
                </div>

                <div class="pt-4 border-t border-gray-100 flex justify-between">
                    <button id="btn-inspector-delete" class="text-xs px-3 py-1.5 bg-red-50 text-red-600 hover:bg-red-100 font-semibold rounded transition flex items-center gap-1">
                        <i class="fa-regular fa-trash-can"></i> Delete Block
                    </button>
                </div>
            `;
	const updateColSpan = (val) => {
		item.colSpan = Math.max(1, Math.min(12, parseInt(val) || 1));
		AppState.recalculateAllContainerRowSpans();
		renderCanvas();
	};
	const updateRowSpan = (val) => {
		item.rowSpan = Math.max(1, Math.min(10, parseInt(val) || 1));
		AppState.recalculateAllContainerRowSpans();
		renderCanvas();
	};
	document.getElementById("input-colspan-range")?.addEventListener("input", (e) => updateColSpan(e.target.value));
	document.getElementById("input-colspan-num")?.addEventListener("change", (e) => updateColSpan(e.target.value));
	document.getElementById("input-rowspan-range")?.addEventListener("input", (e) => updateRowSpan(e.target.value));
	document.getElementById("input-rowspan-num")?.addEventListener("change", (e) => updateRowSpan(e.target.value));
	document.getElementById("input-tag")?.addEventListener("change", (e) => {
		item.tagName = e.target.value;
		renderCanvas();
	});
	document.getElementById("input-content")?.addEventListener("input", (e) => {
		item.content = e.target.value;
		renderCanvas();
	});
	document.getElementById("input-imageurl")?.addEventListener("change", (e) => {
		item.imageUrl = e.target.value;
		renderCanvas();
	});
	document.getElementById("input-css")?.addEventListener("input", (e) => {
		item.customCss = e.target.value;
		renderCanvas();
	});
	document.getElementById("btn-inspector-delete")?.addEventListener("click", () => {
		AppState.removeBlock(item.id);
		renderCanvas();
	});
};
//#endregion
//#region src/TailwindGridBlocksDesigner/CanvasRenderEngine.js
var renderCanvas$1 = function() {
	AppState$1.recalculateAllContainerRowSpans();
	const rootGridEl = document.getElementById("root-grid");
	rootGridEl.innerHTML = "";
	AppState$1.rootContainer.children.forEach((child) => {
		rootGridEl.appendChild(createBlockDomElement(child, AppState$1.rootContainer));
	});
	document.getElementById("canvas-stats").innerText = `Blocks: ${countBlocks(AppState$1.rootContainer)}`;
	renderInspector();
};
var countBlocks = function(container) {
	let count = container.children.length;
	container.children.forEach((c) => {
		if (c.type === "Container") count += countBlocks(c);
	});
	return count;
};
var createBlockDomElement = function(block, parentBlock) {
	const wrapper = document.createElement("div");
	wrapper.className = `grid-block-wrapper ${AppState$1.selectedBlock?.id === block.id ? "selected" : ""}`;
	wrapper.style.gridColumn = `span ${block.colSpan}`;
	wrapper.style.gridRow = `span ${block.rowSpan}`;
	wrapper.dataset.id = block.id;
	wrapper.tabIndex = 0;
	wrapper.draggable = true;
	const toolbar = document.createElement("div");
	toolbar.className = "block-toolbar";
	toolbar.innerHTML = `
                <div class="flex items-center gap-1.5 cursor-grab drag-handle flex-1 min-w-0">
                    <i class="fa-solid fa-grip-vertical text-gray-400 hover:text-gray-600"></i>
                    <i class="fa-solid ${{
		Container: "fa-border-all text-mud-primary",
		Heading: "fa-heading text-blue-500",
		Paragraph: "fa-paragraph text-emerald-500",
		Image: "fa-image text-amber-500",
		CustomHtml: "fa-code text-purple-500"
	}[block.type] || "fa-cube"}"></i>
                    <span class="font-bold text-[11px] text-gray-700 truncate">${block.type}</span>
                    <span class="text-[10px] text-gray-400 font-mono">(${block.colSpan}x${block.rowSpan})</span>
                </div>
                <div class="flex items-center gap-1 shrink-0">
                    ${block.type === "Container" ? `<button class="btn-add-child p-1 hover:text-mud-primary transition text-gray-600" title="Add Child Block"><i class="fa-solid fa-plus text-xs"></i></button>` : ""}
                    <button class="btn-delete p-1 hover:text-red-500 transition text-gray-600" title="Delete Block"><i class="fa-regular fa-trash-can text-xs"></i></button>
                </div>
            `;
	const body = document.createElement("div");
	body.className = "block-content-body p-3 relative pointer-events-none";
	if (block.type === "Container") {
		body.className = "block-content-body p-2 relative";
		const innerGrid = document.createElement("div");
		innerGrid.className = "grid-container-12 border border-dashed border-indigo-200 rounded p-2 bg-indigo-50/20 h-full";
		innerGrid.dataset.containerId = block.id;
		if (block.children.length === 0) innerGrid.innerHTML = `<div class="col-span-12 flex items-center justify-center text-xs text-indigo-400 font-medium py-6 pointer-events-none">Empty Container Grid (Hover & click '+' to add blocks)</div>`;
		else block.children.forEach((child) => {
			innerGrid.appendChild(createBlockDomElement(child, block));
		});
		body.appendChild(innerGrid);
	} else if (block.type === "Heading") {
		const tag = block.tagName || "h2";
		const el = document.createElement(tag);
		el.className = "font-bold text-gray-800 tracking-tight leading-snug";
		el.innerText = block.content;
		body.appendChild(el);
	} else if (block.type === "Paragraph") {
		const p = document.createElement("p");
		p.className = "text-sm text-gray-600 leading-relaxed";
		p.innerText = block.content;
		body.appendChild(p);
	} else if (block.type === "Image") {
		const img = document.createElement("img");
		img.src = block.imageUrl;
		img.className = "w-full h-full object-cover rounded shadow-sm max-h-[220px]";
		body.appendChild(img);
	} else if (block.type === "CustomHtml") body.innerHTML = block.content;
	wrapper.appendChild(toolbar);
	wrapper.appendChild(body);
	const handleR = document.createElement("div");
	handleR.className = "resize-handle handle-r";
	handleR.dataset.dir = "r";
	const handleB = document.createElement("div");
	handleB.className = "resize-handle handle-b";
	handleB.dataset.dir = "b";
	const handleSE = document.createElement("div");
	handleSE.className = "resize-handle handle-se";
	handleSE.dataset.dir = "se";
	wrapper.appendChild(handleR);
	wrapper.appendChild(handleB);
	wrapper.appendChild(handleSE);
	wrapper.addEventListener("click", (e) => {
		e.stopPropagation();
		AppState$1.selectedBlock = block;
		renderCanvas$1();
	});
	toolbar.querySelector(".btn-delete")?.addEventListener("click", (e) => {
		e.stopPropagation();
		AppState$1.removeBlock(block.id);
		renderCanvas$1();
	});
	toolbar.querySelector(".btn-add-child")?.addEventListener("click", (e) => {
		e.stopPropagation();
		const newChild = new GridBlockItem("Paragraph", 12, 1);
		block.children.push(newChild);
		AppState$1.selectedBlock = newChild;
		AppState$1.recalculateAllContainerRowSpans();
		renderCanvas$1();
	});
	wrapper.addEventListener("dragstart", (e) => {
		e.stopPropagation();
		AppState$1.draggedBlock = block;
		wrapper.classList.add("dragging");
		e.dataTransfer.setData("text/plain", block.id);
		e.dataTransfer.effectAllowed = "move";
	});
	wrapper.addEventListener("dragend", () => {
		wrapper.classList.remove("dragging");
		AppState$1.draggedBlock = null;
		document.querySelectorAll(".drop-target-over").forEach((el) => el.classList.remove("drop-target-over"));
	});
	wrapper.addEventListener("dragover", (e) => {
		e.preventDefault();
		e.stopPropagation();
		if (!AppState$1.draggedBlock || AppState$1.draggedBlock.id === block.id) return;
		wrapper.classList.add("drop-target-over");
	});
	wrapper.addEventListener("dragleave", (e) => {
		e.stopPropagation();
		wrapper.classList.remove("drop-target-over");
	});
	wrapper.addEventListener("drop", (e) => {
		e.preventDefault();
		e.stopPropagation();
		wrapper.classList.remove("drop-target-over");
		const dragged = AppState$1.draggedBlock;
		if (!dragged || dragged.id === block.id) return;
		const { parent: oldParent } = AppState$1.findBlockById(dragged.id);
		if (oldParent) {
			const oldIndex = oldParent.children.indexOf(dragged);
			if (oldIndex > -1) oldParent.children.splice(oldIndex, 1);
		}
		const targetParent = parentBlock;
		const targetIndex = targetParent.children.indexOf(block);
		targetParent.children.splice(targetIndex, 0, dragged);
		AppState$1.recalculateAllContainerRowSpans();
		renderCanvas$1();
	});
	const initResize = (dir, e) => {
		e.preventDefault();
		e.stopPropagation();
		const colWidthPx = wrapper.parentElement.getBoundingClientRect().width / 12;
		AppState$1.resizingData = {
			block,
			dir,
			startX: e.clientX,
			startY: e.clientY,
			startColSpan: block.colSpan,
			startRowSpan: block.rowSpan,
			colWidthPx,
			rowHeightPx: 65,
			wrapper
		};
		const indicator = document.createElement("div");
		indicator.className = "resize-indicator";
		indicator.id = "active-resize-indicator";
		indicator.innerText = `${block.colSpan} Col x ${block.rowSpan} Row`;
		wrapper.appendChild(indicator);
		document.addEventListener("mousemove", handleMouseMove);
		document.addEventListener("mouseup", handleMouseUp);
	};
	handleR.addEventListener("mousedown", (e) => initResize("r", e));
	handleB.addEventListener("mousedown", (e) => initResize("b", e));
	handleSE.addEventListener("mousedown", (e) => initResize("se", e));
	return wrapper;
};
var handleMouseMove = function(e) {
	if (!AppState$1.resizingData) return;
	const { block, dir, startX, startY, startColSpan, startRowSpan, colWidthPx, rowHeightPx, wrapper } = AppState$1.resizingData;
	if (dir === "r" || dir === "se") {
		const deltaX = e.clientX - startX;
		const colsChange = Math.round(deltaX / colWidthPx);
		block.colSpan = Math.max(1, Math.min(12, startColSpan + colsChange));
	}
	if (dir === "b" || dir === "se") {
		const deltaY = e.clientY - startY;
		const rowsChange = Math.round(deltaY / rowHeightPx);
		block.rowSpan = Math.max(1, Math.min(10, startRowSpan + rowsChange));
	}
	wrapper.style.gridColumn = `span ${block.colSpan}`;
	wrapper.style.gridRow = `span ${block.rowSpan}`;
	const badge = wrapper.querySelector(".drag-handle .font-mono");
	if (badge) badge.innerText = `(${block.colSpan}x${block.rowSpan})`;
	const indicator = document.getElementById("active-resize-indicator");
	if (indicator) indicator.innerText = `${block.colSpan} Col x ${block.rowSpan} Row`;
	AppState$1.recalculateAllContainerRowSpans();
};
var handleMouseUp = function() {
	if (AppState$1.resizingData) {
		document.removeEventListener("mousemove", handleMouseMove);
		document.removeEventListener("mouseup", handleMouseUp);
		AppState$1.resizingData = null;
		renderCanvas$1();
	}
};
//#endregion
//#region src/module.js
function setupAddInteractions() {
	document.querySelectorAll(".palette-item").forEach((item) => {
		item.addEventListener("dragstart", (e) => {
			const type = item.dataset.type;
			const newBlock = new GridBlockItem$1(type, type === "Container" ? 12 : 6, 1);
			AppState$1.draggedBlock = newBlock;
			e.dataTransfer.setData("text/plain", newBlock.id);
		});
	});
	const addNewBlockToTarget = (type) => {
		const newBlock = new GridBlockItem$1(type, type === "Container" ? 12 : 6, 1);
		if (AppState$1.selectedBlock && AppState$1.selectedBlock.type === "Container") AppState$1.selectedBlock.children.push(newBlock);
		else AppState$1.rootContainer.children.push(newBlock);
		AppState$1.selectedBlock = newBlock;
		AppState$1.recalculateAllContainerRowSpans();
		renderCanvas$1();
	};
	document.querySelectorAll(".btn-quick-add, .btn-top-add").forEach((btn) => {
		btn.addEventListener("click", () => addNewBlockToTarget(btn.dataset.type));
	});
	const rootGridEl = document.getElementById("root-grid");
	rootGridEl.addEventListener("dragover", (e) => e.preventDefault());
	rootGridEl.addEventListener("drop", (e) => {
		e.preventDefault();
		if (e.target !== rootGridEl) return;
		const dragged = AppState$1.draggedBlock;
		if (!dragged) return;
		const { item } = AppState$1.findBlockById(dragged.id);
		if (!item) {
			AppState$1.rootContainer.children.push(dragged);
			AppState$1.selectedBlock = dragged;
		}
		AppState$1.recalculateAllContainerRowSpans();
		renderCanvas$1();
	});
}
function setupExportModal() {
	const modal = document.getElementById("export-modal");
	const codeBlock = document.getElementById("export-code-block");
	document.getElementById("btn-export").addEventListener("click", () => {
		AppState$1.recalculateAllContainerRowSpans();
		const fullTemplate = `<div class="wysiwyg-grid-layout" style="display: grid; grid-template-columns: repeat(12, minmax(0, 1fr)); grid-auto-flow: dense; gap: 16px; width: 100%;">\n${AppState$1.rootContainer.children.map((c) => c.renderToHtml()).join("\n\n")}\n</div>`;
		codeBlock.innerText = fullTemplate;
		modal.classList.remove("hidden");
		setTimeout(() => modal.classList.remove("opacity-0"), 10);
	});
	document.getElementById("btn-close-modal").addEventListener("click", () => {
		modal.classList.add("opacity-0");
		setTimeout(() => modal.classList.add("hidden"), 200);
	});
	document.getElementById("btn-copy-code").addEventListener("click", () => {
		const code = codeBlock.innerText;
		const textarea = document.createElement("textarea");
		textarea.value = code;
		document.body.appendChild(textarea);
		textarea.select();
		document.execCommand("copy");
		document.body.removeChild(textarea);
		const btn = document.getElementById("btn-copy-code");
		btn.innerHTML = `<i class="fa-solid fa-check"></i> Copied!`;
		setTimeout(() => {
			btn.innerHTML = `<i class="fa-regular fa-copy"></i> Copy HTML`;
		}, 2e3);
	});
}
window.onload = function() {
	initDemoData();
	renderCanvas$1();
	setupAddInteractions();
	setupExportModal();
	document.getElementById("btn-clear").addEventListener("click", () => {
		initDemoData();
		renderCanvas$1();
	});
	document.getElementById("tailwind-interactive-button").style.width = "500px";
};
//#endregion
