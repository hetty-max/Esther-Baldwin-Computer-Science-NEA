import { imageForState } from './state.js';

// Builds the DOM for an NxN puzzle grid plus its row/column header inputs, laid
// out as a CSS grid: a blank corner cell, a row of column headers, then one row
// header + N cells per row. Mirrors BaseGridForm's button grid + GenerateHeaders.
export function buildGrid(container, size, onCellClick) {
  container.innerHTML = '';
  container.classList.add('tt-grid');
  container.style.gridTemplateColumns = `var(--header-w) repeat(${size}, 1fr)`;
  container.style.gridTemplateRows = `var(--header-w) repeat(${size}, 1fr)`;

  const cellEls = Array.from({ length: size }, () => new Array(size));
  const colHeaderEls = new Array(size);
  const rowHeaderEls = new Array(size);

  const corner = document.createElement('div');
  corner.className = 'tt-corner';
  container.appendChild(corner);

  for (let x = 0; x < size; x++) {
    const h = document.createElement('input');
    h.className = 'tt-header tt-col-header';
    h.readOnly = true;
    h.value = '0';
    h.tabIndex = -1;
    container.appendChild(h);
    colHeaderEls[x] = h;
  }

  for (let y = 0; y < size; y++) {
    const rh = document.createElement('input');
    rh.className = 'tt-header tt-row-header';
    rh.readOnly = true;
    rh.value = '0';
    rh.tabIndex = -1;
    container.appendChild(rh);
    rowHeaderEls[y] = rh;

    for (let x = 0; x < size; x++) {
      const cell = document.createElement('button');
      cell.type = 'button';
      cell.className = 'tt-cell';
      cell.dataset.x = String(x);
      cell.dataset.y = String(y);
      cell.style.backgroundImage = `url(${imageForState(0)})`;
      cell.disabled = true;
      if (onCellClick) cell.addEventListener('click', () => onCellClick(x, y));
      container.appendChild(cell);
      cellEls[x][y] = cell;
    }
  }

  return { cellEls, colHeaderEls, rowHeaderEls };
}

export function paintCell(cellEls, x, y, state) {
  cellEls[x][y].style.backgroundImage = `url(${imageForState(state)})`;
}

const HIGHLIGHT_CLASSES = ['hl-current', 'hl-visited', 'hl-success', 'hl-locked', 'hl-given'];

export function highlightCell(cellEls, x, y, cls) {
  const el = cellEls[x][y];
  el.classList.remove(...HIGHLIGHT_CLASSES);
  if (cls) el.classList.add(cls);
}

export function setCellEnabled(cellEls, x, y, enabled) {
  cellEls[x][y].disabled = !enabled;
}

export function setAllEnabled(cellEls, size, enabled, predicate) {
  for (let x = 0; x < size; x++) {
    for (let y = 0; y < size; y++) {
      cellEls[x][y].disabled = predicate ? !predicate(x, y) : !enabled;
    }
  }
}

export function setHeader(headerEls, i, value) {
  headerEls[i].value = String(value);
}

export function refreshHeadersFromGrid(grid, colHeaderEls, rowHeaderEls) {
  for (let x = 0; x < grid.size; x++) setHeader(colHeaderEls, x, grid.noTracksCol(x));
  for (let y = 0; y < grid.size; y++) setHeader(rowHeaderEls, y, grid.noTracksRow(y));
}

export function paintGrid(grid, cellEls) {
  for (let x = 0; x < grid.size; x++) {
    for (let y = 0; y < grid.size; y++) {
      paintCell(cellEls, x, y, grid.stateAt(x, y));
    }
  }
}

export function clearHighlights(cellEls, size) {
  for (let x = 0; x < size; x++) {
    for (let y = 0; y < size; y++) highlightCell(cellEls, x, y, null);
  }
}
