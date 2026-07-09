import { Grid } from './grid.js';
import { buildGrid, paintCell, paintGrid, highlightCell, setCellEnabled, refreshHeadersFromGrid } from './render.js';
import { validPath, randInt } from './path.js';

// Shared "problem view" hand-off used by both Generate and Set: takes a fully
// solved grid, builds a second grid seeded with the start/end shapes plus exactly
// 2 random track pieces from the solution as givens, and lets a second "solver"
// fill in the rest before checking their answer against the original solution's
// row/column counts and topology (port of problemview_Click / checksolution_Click,
// identical in Generate.cs and Set.cs).
export function createProblemView({ solvedGrid, start, end, container }) {
  const size = solvedGrid.size;
  const grid2 = new Grid(size);

  grid2.setSquare(start.x, start.y, solvedGrid.stateAt(start.x, start.y));
  grid2.setSquare(end.x, end.y, solvedGrid.stateAt(end.x, end.y));

  const locked = new Set([`${start.x},${start.y}`, `${end.x},${end.y}`]);
  const given = [start, end];

  let placed = 0;
  let attempts = 0;
  while (placed < 2 && attempts < 5000) {
    attempts++;
    const x = randInt(0, size);
    const y = randInt(0, size);
    const key = `${x},${y}`;
    if (locked.has(key) || solvedGrid.stateAt(x, y) === 0) continue;
    grid2.setSquare(x, y, solvedGrid.stateAt(x, y));
    locked.add(key);
    given.push({ x, y });
    placed++;
  }

  let interactive = false;

  const { cellEls, colHeaderEls, rowHeaderEls } = buildGrid(container, size, (x, y) => {
    if (!interactive || locked.has(`${x},${y}`)) return;
    const next = grid2.stateAt(x, y) + 1 > 7 ? 0 : grid2.stateAt(x, y) + 1;
    grid2.setSquare(x, y, next);
    paintCell(cellEls, x, y, next);
  });

  paintGrid(grid2, cellEls);
  for (const { x, y } of given) {
    setCellEnabled(cellEls, x, y, false);
    highlightCell(cellEls, x, y, 'hl-given');
  }
  // headers shown are the target counts to reach (the true solution's counts)
  refreshHeadersFromGrid(solvedGrid, colHeaderEls, rowHeaderEls);

  function enablePlay() {
    interactive = true;
    for (let x = 0; x < size; x++) {
      for (let y = 0; y < size; y++) {
        if (!locked.has(`${x},${y}`)) setCellEnabled(cellEls, x, y, true);
      }
    }
  }

  function checkSolution() {
    const topologyOk = validPath(grid2, start, end);
    let headersOk = true;
    for (let i = 0; i < size; i++) {
      if (grid2.noTracksCol(i) !== solvedGrid.noTracksCol(i)) headersOk = false;
      if (grid2.noTracksRow(i) !== solvedGrid.noTracksRow(i)) headersOk = false;
    }
    const correct = topologyOk && headersOk;
    if (correct) {
      for (let x = 0; x < size; x++) {
        for (let y = 0; y < size; y++) {
          highlightCell(cellEls, x, y, 'hl-success');
          setCellEnabled(cellEls, x, y, false);
        }
      }
    }
    return correct;
  }

  return { grid2, cellEls, colHeaderEls, rowHeaderEls, enablePlay, checkSolution };
}
