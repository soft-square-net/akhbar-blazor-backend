import { GridBlockItem } from './TailwindGridBlocksDesigner/GridBlockItem';
import { AppState } from './TailwindGridBlocksDesigner/AppState'; 
import { initDemoData } from './TailwindGridBlocksDesigner/InitDemoData';
import {
  renderCanvas,
  countBlocks,
  createBlockDomElement,
  handleMouseMove,
  handleMouseUp
} from './TailwindGridBlocksDesigner/CanvasRenderEngine';

import { renderInspector } from './TailwindGridBlocksDesigner/PropertyInspector';
// --- PALETTE & BUTTON HANDLERS ---
function setupAddInteractions() {
  document.querySelectorAll('.palette-item').forEach(item => {
    item.addEventListener('dragstart', (e) => {
      const type = item.dataset.type;
      const newBlock = new GridBlockItem(type, type === 'Container' ? 12 : 6, 1);
      AppState.draggedBlock = newBlock;
      e.dataTransfer.setData('text/plain', newBlock.id);
    });
  });

  const addNewBlockToTarget = (type) => {
    const newBlock = new GridBlockItem(type, type === 'Container' ? 12 : 6, 1);
    if (AppState.selectedBlock && AppState.selectedBlock.type === 'Container') {
      AppState.selectedBlock.children.push(newBlock);
    } else {
      AppState.rootContainer.children.push(newBlock);
    }
    AppState.selectedBlock = newBlock;
    AppState.recalculateAllContainerRowSpans();
    renderCanvas();
  };

  document.querySelectorAll('.btn-quick-add, .btn-top-add').forEach(btn => {
    btn.addEventListener('click', () => addNewBlockToTarget(btn.dataset.type));
  });

  const rootGridEl = document.getElementById('root-grid');
  rootGridEl.addEventListener('dragover', (e) => e.preventDefault());

  rootGridEl.addEventListener('drop', (e) => {
    e.preventDefault();
    if (e.target !== rootGridEl) return;

    const dragged = AppState.draggedBlock;
    if (!dragged) return;

    const { item } = AppState.findBlockById(dragged.id);
    if (!item) {
      AppState.rootContainer.children.push(dragged);
      AppState.selectedBlock = dragged;
    }
    AppState.recalculateAllContainerRowSpans();
    renderCanvas();
  });
}

// --- EXPORT HTML MODAL ---
function setupExportModal() {
  const modal = document.getElementById('export-modal');
  const codeBlock = document.getElementById('export-code-block');

  document.getElementById('btn-export').addEventListener('click', () => {
    AppState.recalculateAllContainerRowSpans();
    const generatedHtml = AppState.rootContainer.children.map(c => c.renderToHtml()).join('\n\n');
    const fullTemplate = `<div class="wysiwyg-grid-layout" style="display: grid; grid-template-columns: repeat(12, minmax(0, 1fr)); grid-auto-flow: dense; gap: 16px; width: 100%;">\n${generatedHtml}\n</div>`;

    codeBlock.innerText = fullTemplate;
    modal.classList.remove('hidden');
    setTimeout(() => modal.classList.remove('opacity-0'), 10);
  });

  document.getElementById('btn-close-modal').addEventListener('click', () => {
    modal.classList.add('opacity-0');
    setTimeout(() => modal.classList.add('hidden'), 200);
  });

  document.getElementById('btn-copy-code').addEventListener('click', () => {
    const code = codeBlock.innerText;
    const textarea = document.createElement('textarea');
    textarea.value = code;
    document.body.appendChild(textarea);
    textarea.select();
    document.execCommand('copy');
    document.body.removeChild(textarea);

    const btn = document.getElementById('btn-copy-code');
    btn.innerHTML = `<i class="fa-solid fa-check"></i> Copied!`;
    setTimeout(() => {
      btn.innerHTML = `<i class="fa-regular fa-copy"></i> Copy HTML`;
    }, 2000);
  });
}

// Initialize Window
window.onload = function () {
  initDemoData();
  renderCanvas();
  setupAddInteractions();
  setupExportModal();

  document.getElementById('btn-clear').addEventListener('click', () => {
    initDemoData();
    renderCanvas();
  });

  document.getElementById('tailwind-interactive-button').style.width = "500px";
};

// Initialize Window
export function InitializeTailwingGridBlockDesigner (dotNetRef) {
  initDemoData();
  renderCanvas();
  setupAddInteractions();
  setupExportModal();

  document.getElementById('btn-clear').addEventListener('click', () => {
    initDemoData();
    renderCanvas();
  });

  document.getElementById('tailwind-interactive-button').style.width = "500px";
};

export default InitializeTailwingGridBlockDesigner;