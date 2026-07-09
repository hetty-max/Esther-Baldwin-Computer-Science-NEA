import { Grid } from './grid.js';
import { buildGrid, paintCell, setAllEnabled, refreshHeadersFromGrid, highlightCell, clearHighlights } from './render.js';
import { outwardEdges, nextOutwardState, returnTrack, stateInfo } from './state.js';
import { sameCell } from './path.js';

const sizeSelect = document.getElementById('difficulty');
const newGridBtn = document.getElementById('newGridBtn');
const lockBtn = document.getElementById('lockBtn');
const heuristicSelect = document.getElementById('heuristic');
const dfsBtn = document.getElementById('dfsBtn');
const bfsBtn = document.getElementById('bfsBtn');
const statusEl = document.getElementById('status');
const stepsEl = document.getElementById('steps');
const gridContainer = document.getElementById('grid');

let grid, cellEls, colHeaderEls, rowHeaderEls, size;
let start = null;
let end = null;
let phase = 'idle';
let cancelToken = { cancelled: false };
let colLimits = [];
let rowLimits = [];

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
    phase = 'drawing';
    setAllEnabled(cellEls, size, true);
    colHeaderEls.forEach((el) => (el.readOnly = false));
    rowHeaderEls.forEach((el) => (el.readOnly = false));
    lockBtn.disabled = false;
    setStatus('Free-paint the interior if you like, then type the row/column targets and lock the grid.');
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

  const built = buildGrid(gridContainer, size, onCellClick);
  cellEls = built.cellEls;
  colHeaderEls = built.colHeaderEls;
  rowHeaderEls = built.rowHeaderEls;
  setAllEnabled(cellEls, size, false, (x, y) => grid.isEdge(x, y));
  refreshHeadersFromGrid(grid, colHeaderEls, rowHeaderEls);

  lockBtn.disabled = true;
  dfsBtn.disabled = true;
  bfsBtn.disabled = true;
  stepsEl.textContent = '';
  setStatus('Click an edge tile to place the Start marker.');
}

lockBtn.addEventListener('click', () => {
  colLimits = colHeaderEls.map((el) => Math.max(0, parseInt(el.value, 10) || 0));
  rowLimits = rowHeaderEls.map((el) => Math.max(0, parseInt(el.value, 10) || 0));
  colHeaderEls.forEach((el) => (el.readOnly = true));
  rowHeaderEls.forEach((el) => (el.readOnly = true));
  setAllEnabled(cellEls, size, false, () => false);
  lockBtn.disabled = true;
  dfsBtn.disabled = false;
  bfsBtn.disabled = false;
  phase = 'locked';
  setStatus('Grid locked. Pick a solving algorithm.');
});

function nextFrame() {
  return new Promise((resolve) => requestAnimationFrame(() => resolve()));
}

function validateAgainstLimits() {
  for (let i = 0; i < size; i++) {
    if (grid.noTracksCol(i) !== colLimits[i]) return false;
    if (grid.noTracksRow(i) !== rowLimits[i]) return false;
  }
  return true;
}

function lossDistance(x, y) {
  return -Math.hypot(end.x - x, end.y - y);
}
function lossRowCol(x, y) {
  return (colLimits[x] - grid.noTracksCol(x)) ** 2 + (rowLimits[y] - grid.noTracksRow(y)) ** 2;
}
function lossCombo(x, y) {
  return colLimits[x] - grid.noTracksCol(x) + (rowLimits[y] - grid.noTracksRow(y)) - Math.hypot(end.x - x, end.y - y);
}

// Backtracking DFS: reaching `end` only succeeds if the finished grid's row/column
// counts exactly equal the locked targets (port of validateGrid()), otherwise this
// branch backtracks and a different route to `end` is tried. Neighbour ordering is
// purely a branch-priority heuristic - it changes speed/animation, never correctness.
function* dfsSearch(prev, cur, visited, stepsBox) {
  stepsBox.count++;
  if (stepsBox.count > 8000) return false;

  if (sameCell(cur, end)) {
    return validateAgainstLimits();
  }

  visited[cur.x][cur.y] = true;
  yield { type: 'visit', cell: cur };

  let neighbours = grid.neighbours(cur.x, cur.y).filter((n) => !visited[n.x][n.y]);
  neighbours = neighbours.filter(
    (n) => sameCell(n, end) || (grid.noTracksCol(n.x) < colLimits[n.x] && grid.noTracksRow(n.y) < rowLimits[n.y])
  );

  const mode = heuristicSelect.value;
  if (mode === 'distance') neighbours.sort((a, b) => lossDistance(b.x, b.y) - lossDistance(a.x, a.y));
  else if (mode === 'rowcol') neighbours.sort((a, b) => lossRowCol(b.x, b.y) - lossRowCol(a.x, a.y));
  else if (mode === 'combo') neighbours.sort((a, b) => lossCombo(b.x, b.y) - lossCombo(a.x, a.y));

  for (const next of neighbours) {
    const trackValue = returnTrack(prev, cur, next);
    if (trackValue === 0) continue;
    grid.setSquare(cur.x, cur.y, trackValue);
    paintCell(cellEls, cur.x, cur.y, trackValue);
    yield { type: 'place', cell: cur };
    const solved = yield* dfsSearch(cur, next, visited, stepsBox);
    if (solved) return true;
  }

  grid.setSquare(cur.x, cur.y, 0);
  paintCell(cellEls, cur.x, cur.y, 0);
  visited[cur.x][cur.y] = false;
  yield { type: 'backtrack', cell: cur };
  return false;
}

async function runDfs(batchSize = 5) {
  const visited = Array.from({ length: size }, () => new Array(size).fill(false));
  visited[start.x][start.y] = true;
  clearInteriorForSearch();
  const stepsBox = { count: 0 };
  const gen = dfsSearch(start, squareAfterStart(), visited, stepsBox);

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

function clearInteriorForSearch() {
  for (let x = 0; x < size; x++) {
    for (let y = 0; y < size; y++) {
      if (sameCell({ x, y }, start) || sameCell({ x, y }, end)) continue;
      grid.setSquare(x, y, 0);
      paintCell(cellEls, x, y, 0);
    }
  }
  clearHighlights(cellEls, size);
}

function validNewTrackInPath(path, x, y) {
  if (x < 0 || y < 0 || x >= size || y >= size) return false;
  if (path.rowCounts[y] <= 0) return false;
  if (path.colCounts[x] <= 0) return false;
  for (const c of path.cells) if (c.x === x && c.y === y) return false;
  return true;
}

function addTrackToPath(path, x, y) {
  if (!validNewTrackInPath(path, x, y)) return false;
  path.rowCounts[y] -= 1;
  path.colCounts[x] -= 1;
  path.cells.push({ x, y });
  return true;
}

function neighboursOfLast(path) {
  const last = path.cells[path.cells.length - 1];
  return grid.neighbours(last.x, last.y).filter((n) => validNewTrackInPath(path, n.x, n.y));
}

// Draws one dequeued BFS candidate onto the live grid (port of drawPathOnGrid).
// The original skips placing a shape on the very first interior cell too (an
// artifact of its loop bounds); we draw every interior cell of the candidate
// instead, which is clearer without changing what a *complete* path looks like.
function drawPath(path) {
  for (let x = 0; x < size; x++) {
    for (let y = 0; y < size; y++) {
      if (!(sameCell({ x, y }, start) || sameCell({ x, y }, end))) grid.setSquare(x, y, 0);
    }
  }
  const cells = path.cells;
  for (let i = 1; i < cells.length - 1; i++) {
    const cur = cells[i];
    if (sameCell(cur, end)) continue;
    const trackValue = returnTrack(cells[i - 1], cur, cells[i + 1]);
    grid.setSquare(cur.x, cur.y, trackValue);
  }
  for (let x = 0; x < size; x++) {
    for (let y = 0; y < size; y++) paintCell(cellEls, x, y, grid.stateAt(x, y));
  }
}

function validateFromPath(path) {
  for (let i = 0; i < size; i++) {
    if (path.rowCounts[i] !== 0 || path.colCounts[i] !== 0) return false;
  }
  return true;
}

// BFS over full path-copies. Termination requires the WHOLE header budget to be
// used up AND the path to end at `end` - not merely "reachability" - matching the
// puzzle rule that the finished track uses exactly the row/column counts implied
// by the headers (port of Solve.cs's BFS/TrainPath).
function* bfsGen(stepsBox) {
  const totalAllowed = colLimits.reduce((a, b) => a + b, 0);
  const initial = { cells: [], rowCounts: rowLimits.slice(), colCounts: colLimits.slice() };
  const second = squareAfterStart();
  addTrackToPath(initial, start.x, start.y);
  addTrackToPath(initial, second.x, second.y);

  const queue = [initial];
  let dequeues = 0;
  while (queue.length > 0) {
    dequeues++;
    if (dequeues > 20000) return null;
    const cur = queue.shift();
    yield { type: 'draw', path: cur };
    const last = cur.cells[cur.cells.length - 1];
    if (cur.cells.length === totalAllowed && sameCell(last, end)) {
      if (validateFromPath(cur)) return cur;
      continue;
    }
    for (const n of neighboursOfLast(cur)) {
      stepsBox.count++;
      yield { type: 'step' };
      const clone = { cells: cur.cells.slice(), rowCounts: cur.rowCounts.slice(), colCounts: cur.colCounts.slice() };
      addTrackToPath(clone, n.x, n.y);
      queue.push(clone);
    }
  }
  return null;
}

async function runBfs(batchSize = 1) {
  clearInteriorForSearch();
  const stepsBox = { count: 0 };
  const gen = bfsGen(stepsBox);

  let res = gen.next();
  let sinceFrame = 0;
  while (!res.done) {
    if (cancelToken.cancelled) return { solved: false, cancelled: true };
    const ev = res.value;
    if (ev.type === 'draw') drawPath(ev.path);
    stepsEl.textContent = 'Steps: ' + stepsBox.count;
    sinceFrame++;
    if (sinceFrame >= batchSize) {
      sinceFrame = 0;
      await nextFrame();
    }
    res = gen.next();
  }
  if (res.value) {
    drawPath(res.value);
    return { solved: true, cancelled: false };
  }
  return { solved: false, cancelled: false };
}

function onSolveSuccess() {
  clearHighlights(cellEls, size);
  for (let x = 0; x < size; x++) {
    for (let y = 0; y < size; y++) {
      if (grid.stateAt(x, y) !== 0) highlightCell(cellEls, x, y, 'hl-success');
    }
  }
  setStatus('Solution found!', 'good');
}

dfsBtn.addEventListener('click', async () => {
  cancelToken.cancelled = true;
  cancelToken = { cancelled: false };
  const myToken = cancelToken;
  dfsBtn.disabled = true;
  bfsBtn.disabled = true;
  setStatus('Running DFS...');
  const result = await runDfs();
  if (myToken.cancelled) return;
  dfsBtn.disabled = false;
  bfsBtn.disabled = false;
  if (result.solved) onSolveSuccess();
  else setStatus('No solution could be found for this grid.', 'bad');
});

bfsBtn.addEventListener('click', async () => {
  cancelToken.cancelled = true;
  cancelToken = { cancelled: false };
  const myToken = cancelToken;
  dfsBtn.disabled = true;
  bfsBtn.disabled = true;
  setStatus('Running BFS...');
  const result = await runBfs();
  if (myToken.cancelled) return;
  dfsBtn.disabled = false;
  bfsBtn.disabled = false;
  if (result.solved) onSolveSuccess();
  else setStatus('No solution could be found for this grid.', 'bad');
});

newGridBtn.addEventListener('click', buildNewGrid);

buildNewGrid();
