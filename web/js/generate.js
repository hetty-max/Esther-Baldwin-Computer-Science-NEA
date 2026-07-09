import { Grid } from './grid.js';
import {
  buildGrid,
  paintCell,
  setCellEnabled,
  setAllEnabled,
  refreshHeadersFromGrid,
  highlightCell,
  clearHighlights,
} from './render.js';
import { outwardEdges, nextOutwardState, returnTrack, connectsMutually, stateInfo } from './state.js';
import { shuffle, randInt, sameCell } from './path.js';
import { createProblemView } from './problemView.js';

const sizeSelect = document.getElementById('difficulty');
const newGridBtn = document.getElementById('newGridBtn');
const confirmBtn = document.getElementById('confirmBtn');
const randomWalkBtn = document.getElementById('randomWalkBtn');
const dfsBtn = document.getElementById('dfsBtn');
const problemViewBtn = document.getElementById('problemViewBtn');
const statusEl = document.getElementById('status');
const stepsEl = document.getElementById('steps');
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
let cancelToken = { cancelled: false };
let pv = null;

function setStatus(text, cls) {
  statusEl.textContent = text;
  statusEl.className = 'status' + (cls ? ' ' + cls : '');
}

function squareAfterStart() {
  const s = stateInfo(grid.stateAt(start.x, start.y));
  let nx = start.x;
  let ny = start.y;
  if (s.left && start.x > 0) nx = start.x - 1;
  else if (s.right && start.x < size - 1) nx = start.x + 1;
  else if (s.top && start.y > 0) ny = start.y - 1;
  else if (s.bottom && start.y < size - 1) ny = start.y + 1;
  return { x: nx, y: ny };
}

function onCellClick(x, y) {
  if (phase === 'placing-start') {
    grid.setSquare(x, y, 8);
    paintCell(cellEls, x, y, 8);
    highlightCell(cellEls, x, y, 'hl-locked');
    start = { x, y };
    phase = 'placing-end';
    setStatus('Click a different edge tile to place the End marker.');
    return;
  }
  if (phase === 'placing-end') {
    if (sameCell({ x, y }, start)) return;
    grid.setSquare(x, y, 9);
    paintCell(cellEls, x, y, 9);
    highlightCell(cellEls, x, y, 'hl-locked');
    end = { x, y };
    phase = 'editing-ends';
    setAllEnabled(cellEls, size, false, (cx, cy) => sameCell({ x: cx, y: cy }, start) || sameCell({ x: cx, y: cy }, end));
    confirmBtn.disabled = false;
    setStatus('Click Start or End to change its shape, or Confirm Puzzle to continue.');
    return;
  }
  if (phase === 'editing-ends' && (sameCell({ x, y }, start) || sameCell({ x, y }, end))) {
    const edges = outwardEdges(x, y, size);
    const next = nextOutwardState(grid.stateAt(x, y), edges);
    grid.setSquare(x, y, next);
    paintCell(cellEls, x, y, next);
  }
}

function buildNewGrid() {
  cancelToken.cancelled = true;
  cancelToken = { cancelled: false };
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

  confirmBtn.disabled = true;
  randomWalkBtn.disabled = true;
  dfsBtn.disabled = true;
  problemViewBtn.disabled = true;
  stepsEl.textContent = '';
  setStatus('Click an edge tile to place the Start marker.');
}

confirmBtn.addEventListener('click', () => {
  phase = 'confirmed';
  setAllEnabled(cellEls, size, false, () => false);
  confirmBtn.disabled = true;
  randomWalkBtn.disabled = false;
  dfsBtn.disabled = false;
  setStatus('Puzzle locked. Choose a generation algorithm.');
});

function resetInteriorTracks() {
  for (let x = 0; x < size; x++) {
    for (let y = 0; y < size; y++) {
      if (sameCell({ x, y }, start) || sameCell({ x, y }, end)) continue;
      grid.setSquare(x, y, 0);
      paintCell(cellEls, x, y, 0);
    }
  }
  clearHighlights(cellEls, size);
  highlightCell(cellEls, start.x, start.y, 'hl-locked');
  highlightCell(cellEls, end.x, end.y, 'hl-locked');
  refreshHeadersFromGrid(grid, colHeaderEls, rowHeaderEls);
}

// Backtracking path carver shared by Random Walk and Constrained DFS - the only
// differences between the two algorithms are neighbour ordering (shuffled or not)
// and whether candidates are filtered by a pre-rolled row/column ceiling.
function* carve(prev, cur, visited, opts, stepsBox) {
  stepsBox.count++;
  if (stepsBox.count > 4000) return false;

  if (sameCell(cur, end)) {
    return connectsMutually(grid, prev.x, prev.y, cur.x, cur.y);
  }

  visited[cur.x][cur.y] = true;
  yield { type: 'visit', cell: cur };

  let neighbours = grid.neighbours(cur.x, cur.y).filter((n) => !visited[n.x][n.y]);
  if (opts.headerCap) {
    neighbours = neighbours.filter(
      (n) => sameCell(n, end) || (grid.noTracksCol(n.x) < opts.headerCap.x[n.x] && grid.noTracksRow(n.y) < opts.headerCap.y[n.y])
    );
  }
  if (opts.shuffleNeighbours) shuffle(neighbours);

  for (const next of neighbours) {
    const trackValue = returnTrack(prev, cur, next);
    if (trackValue === 0) continue;
    grid.setSquare(cur.x, cur.y, trackValue);
    paintCell(cellEls, cur.x, cur.y, trackValue);
    yield { type: 'place', cell: cur };
    const solved = yield* carve(cur, next, visited, opts, stepsBox);
    if (solved) return true;
  }

  grid.setSquare(cur.x, cur.y, 0);
  paintCell(cellEls, cur.x, cur.y, 0);
  visited[cur.x][cur.y] = false;
  yield { type: 'backtrack', cell: cur };
  return false;
}

function nextFrame() {
  return new Promise((resolve) => requestAnimationFrame(() => resolve()));
}

async function runCarve(prev, cur, opts, batchSize = 5) {
  const visited = Array.from({ length: size }, () => new Array(size).fill(false));
  visited[start.x][start.y] = true;
  const stepsBox = { count: 0 };
  const gen = carve(prev, cur, visited, opts, stepsBox);

  let res = gen.next();
  let sinceFrame = 0;
  while (!res.done) {
    if (cancelToken.cancelled) return { solved: false, cancelled: true };
    const ev = res.value;
    if (ev.type === 'visit') highlightCell(cellEls, ev.cell.x, ev.cell.y, 'hl-current');
    if (ev.type === 'backtrack') highlightCell(cellEls, ev.cell.x, ev.cell.y, null);
    stepsEl.textContent = 'Steps: ' + stepsBox.count;
    sinceFrame++;
    if (sinceFrame >= batchSize) {
      sinceFrame = 0;
      await nextFrame();
    }
    res = gen.next();
  }
  return { solved: res.value, cancelled: false };
}

function onGenerateSuccess() {
  clearHighlights(cellEls, size);
  for (let x = 0; x < size; x++) {
    for (let y = 0; y < size; y++) {
      if (grid.stateAt(x, y) !== 0) highlightCell(cellEls, x, y, 'hl-success');
    }
  }
  refreshHeadersFromGrid(grid, colHeaderEls, rowHeaderEls);
  setStatus('Path found! Headers updated.', 'good');
  problemViewBtn.disabled = false;
  randomWalkBtn.disabled = false;
  dfsBtn.disabled = false;
}

randomWalkBtn.addEventListener('click', async () => {
  cancelToken.cancelled = true;
  cancelToken = { cancelled: false };
  const myToken = cancelToken;
  randomWalkBtn.disabled = true;
  dfsBtn.disabled = true;
  resetInteriorTracks();
  const cur = squareAfterStart();
  setStatus('Carving a random path...');
  const result = await runCarve(start, cur, { shuffleNeighbours: true, headerCap: null });
  if (myToken.cancelled) return;
  if (result.solved) {
    onGenerateSuccess();
  } else {
    setStatus('No valid path found. Try again.', 'bad');
    randomWalkBtn.disabled = false;
    dfsBtn.disabled = false;
  }
});

dfsBtn.addEventListener('click', async () => {
  cancelToken.cancelled = true;
  cancelToken = { cancelled: false };
  const myToken = cancelToken;
  randomWalkBtn.disabled = true;
  dfsBtn.disabled = true;
  resetInteriorTracks();
  const xheaders = [];
  const yheaders = [];
  for (let i = 0; i < size; i++) {
    xheaders.push(randInt(3, size));
    yheaders.push(randInt(3, size));
  }
  const cur = squareAfterStart();
  setStatus('Running constrained DFS...');
  const result = await runCarve(start, cur, { shuffleNeighbours: false, headerCap: { x: xheaders, y: yheaders } });
  if (myToken.cancelled) return;
  if (result.solved) {
    onGenerateSuccess();
  } else {
    setStatus('No valid path found. Try again.', 'bad');
    randomWalkBtn.disabled = false;
    dfsBtn.disabled = false;
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
