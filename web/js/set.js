import { Grid } from './grid.js';
import { buildGrid, paintCell, setAllEnabled, refreshHeadersFromGrid, highlightCell, clearHighlights } from './render.js';
import { outwardEdges, nextOutwardState } from './state.js';
import { validPath, sameCell } from './path.js';
import { createProblemView } from './problemView.js';

const sizeSelect = document.getElementById('difficulty');
const newGridBtn = document.getElementById('newGridBtn');
const finishBtn = document.getElementById('finishBtn');
const problemViewBtn = document.getElementById('problemViewBtn');
const statusEl = document.getElementById('status');
const gridContainer = document.getElementById('grid');
const pvSection = document.getElementById('problemViewSection');
const pvContainer = document.getElementById('problemGrid');
const solveProblemBtn = document.getElementById('solveProblemBtn');
const checkSolutionBtn = document.getElementById('checkSolutionBtn');
const pvStatus = document.getElementById('pvStatus');

let grid, cellEls, colHeaderEls, rowHeaderEls, size;
let start = null;
let end = null;
let phase = 'idle';
let pv = null;

function setStatus(text, cls) {
  statusEl.textContent = text;
  statusEl.className = 'status' + (cls ? ' ' + cls : '');
}

function onCellClick(x, y) {
  if (phase === 'placing-start') {
    grid.setSquare(x, y, 8);
    paintCell(cellEls, x, y, 8);
    highlightCell(cellEls, x, y, 'hl-locked');
    start = { x, y };
    phase = 'placing-end';
    refreshHeadersFromGrid(grid, colHeaderEls, rowHeaderEls);
    setStatus('Click a different edge tile to place the End marker.');
    return;
  }
  if (phase === 'placing-end') {
    if (sameCell({ x, y }, start)) return;
    grid.setSquare(x, y, 9);
    paintCell(cellEls, x, y, 9);
    highlightCell(cellEls, x, y, 'hl-locked');
    end = { x, y };
    phase = 'drawing';
    setAllEnabled(cellEls, size, true);
    refreshHeadersFromGrid(grid, colHeaderEls, rowHeaderEls);
    finishBtn.disabled = false;
    setStatus('Click any tile to cycle its track piece, then click Finish.');
    return;
  }
  if (phase === 'drawing') {
    if (sameCell({ x, y }, start) || sameCell({ x, y }, end)) {
      const edges = outwardEdges(x, y, size);
      const next = nextOutwardState(grid.stateAt(x, y), edges);
      grid.setSquare(x, y, next);
      paintCell(cellEls, x, y, next);
    } else {
      const next = grid.stateAt(x, y) + 1 > 7 ? 0 : grid.stateAt(x, y) + 1;
      grid.setSquare(x, y, next);
      paintCell(cellEls, x, y, next);
    }
    refreshHeadersFromGrid(grid, colHeaderEls, rowHeaderEls);
  }
}

function buildNewGrid() {
  size = parseInt(sizeSelect.value, 10);
  grid = new Grid(size);
  start = null;
  end = null;
  phase = 'placing-start';
  pv = null;
  pvSection.hidden = true;

  const built = buildGrid(gridContainer, size, onCellClick);
  cellEls = built.cellEls;
  colHeaderEls = built.colHeaderEls;
  rowHeaderEls = built.rowHeaderEls;
  setAllEnabled(cellEls, size, false, (x, y) => grid.isEdge(x, y));
  refreshHeadersFromGrid(grid, colHeaderEls, rowHeaderEls);

  finishBtn.disabled = true;
  problemViewBtn.disabled = true;
  setStatus('Click an edge tile to place the Start marker.');
}

finishBtn.addEventListener('click', () => {
  const ok = validPath(grid, start, end);
  if (ok) {
    setStatus('Valid! Single connected path from Start to End.', 'good');
    setAllEnabled(cellEls, size, false, () => false);
    finishBtn.disabled = true;
    problemViewBtn.disabled = false;
    clearHighlights(cellEls, size);
    for (let x = 0; x < size; x++) {
      for (let y = 0; y < size; y++) {
        if (grid.stateAt(x, y) !== 0) highlightCell(cellEls, x, y, 'hl-success');
      }
    }
  } else {
    setStatus('Invalid - not a single connected path. Keep adjusting and try again.', 'bad');
  }
});

problemViewBtn.addEventListener('click', () => {
  pvSection.hidden = false;
  pv = createProblemView({ solvedGrid: grid, start, end, container: pvContainer });
  solveProblemBtn.disabled = false;
  checkSolutionBtn.disabled = true;
  pvStatus.textContent = '';
  pvSection.scrollIntoView({ behavior: 'smooth', block: 'start' });
});

solveProblemBtn.addEventListener('click', () => {
  pv.enablePlay();
  solveProblemBtn.disabled = true;
  checkSolutionBtn.disabled = false;
});

checkSolutionBtn.addEventListener('click', () => {
  const correct = pv.checkSolution();
  pvStatus.textContent = correct ? 'Correct solution!' : 'Incorrect solution, try again.';
  pvStatus.className = 'status ' + (correct ? 'good' : 'bad');
});

newGridBtn.addEventListener('click', buildNewGrid);

buildNewGrid();
