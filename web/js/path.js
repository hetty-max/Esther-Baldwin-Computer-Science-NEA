import { connectsMutually } from './state.js';

// Port of ValidGrid/UnconnectedTracks (identical in Generate.cs and Set.cs): a stack
// based traversal from `start`, following only mutually-agreeing connections, then
// requiring `end` was reached AND every non-empty cell was visited. Because every
// track shape has at most 2 openings, "one component containing everything, rooted
// at start, reaching end" is exactly equivalent to "a single simple path start->end
// with no branches, dead ends or stray loops" - no extra bookkeeping needed.
export function validPath(grid, start, end) {
  const size = grid.size;
  const visited = Array.from({ length: size }, () => new Array(size).fill(false));
  const stack = [start];
  visited[start.x][start.y] = true;
  let reachedEnd = false;

  while (stack.length > 0) {
    const { x, y } = stack.pop();
    if (x === end.x && y === end.y) reachedEnd = true;

    for (const n of grid.neighbours(x, y)) {
      if (!visited[n.x][n.y] && connectsMutually(grid, x, y, n.x, n.y)) {
        visited[n.x][n.y] = true;
        stack.push(n);
      }
    }
  }

  if (!reachedEnd) return false;

  for (let x = 0; x < size; x++) {
    for (let y = 0; y < size; y++) {
      if (grid.stateAt(x, y) !== 0 && !visited[x][y]) return false;
    }
  }
  return true;
}

export function sameCell(a, b) {
  return a.x === b.x && a.y === b.y;
}

export function shuffle(array) {
  for (let i = array.length - 1; i > 0; i--) {
    const j = Math.floor(Math.random() * (i + 1));
    [array[i], array[j]] = [array[j], array[i]];
  }
  return array;
}

export function randInt(minInclusive, maxExclusive) {
  return minInclusive + Math.floor(Math.random() * (maxExclusive - minInclusive));
}

export function sleep(ms) {
  return new Promise((resolve) => setTimeout(resolve, ms));
}
