// Port of State.cs: 10 track states, each defined by which sides it opens on.
export const STATES = [
  { value: 0, name: 'Nothing', top: false, right: false, bottom: false, left: false, image: 'assets/nothing.jpg' },
  { value: 1, name: 'Vertical', top: true, right: false, bottom: true, left: false, image: 'assets/vertical.png' },
  { value: 2, name: 'Horizontal', top: false, right: true, bottom: false, left: true, image: 'assets/horizontal.png' },
  { value: 3, name: 'CurveTopLeft', top: true, right: false, bottom: false, left: true, image: 'assets/curve-top-left.png' },
  { value: 4, name: 'CurveTopRight', top: true, right: true, bottom: false, left: false, image: 'assets/curve-top-right.png' },
  { value: 5, name: 'CurveBottomLeft', top: false, right: false, bottom: true, left: true, image: 'assets/curve-bottom-left.png' },
  { value: 6, name: 'CurveBottomRight', top: false, right: true, bottom: true, left: false, image: 'assets/curve-bottom-right.png' },
  { value: 7, name: 'TrackMaybe', top: false, right: false, bottom: false, left: false, image: 'assets/maybe.png' },
  { value: 8, name: 'Start', top: false, right: false, bottom: false, left: false, image: 'assets/start.png' },
  { value: 9, name: 'End', top: false, right: false, bottom: false, left: false, image: 'assets/end.png' },
];

export function stateInfo(value) {
  return STATES[value] ?? STATES[0];
}

export function imageForState(value) {
  return stateInfo(value).image;
}

const DIRECTIONS = ['top', 'right', 'bottom', 'left'];

// Reverse lookup: which of the 6 track shapes (1-6) opens on a given unordered
// pair of sides. Replaces ReturnTrack's hardcoded switch in Generate.cs/Solve.cs -
// every one of those cases is exactly "the shape whose two openings are these two sides".
const PAIR_TO_STATE = {};
for (let v = 1; v <= 6; v++) {
  const dirs = DIRECTIONS.filter((d) => STATES[v][d]).sort();
  PAIR_TO_STATE[dirs.join('-')] = v;
}

export function stateForSides(dirA, dirB) {
  if (!dirA || !dirB || dirA === dirB) return 0;
  const key = [dirA, dirB].sort().join('-');
  return PAIR_TO_STATE[key] ?? 0;
}

// Direction (as seen from (fromX,fromY)) of the orthogonally-adjacent cell (toX,toY).
export function directionTo(fromX, fromY, toX, toY) {
  const dx = toX - fromX;
  const dy = toY - fromY;
  if (dx === 0 && dy === -1) return 'top';
  if (dx === 0 && dy === 1) return 'bottom';
  if (dx === -1 && dy === 0) return 'left';
  if (dx === 1 && dy === 0) return 'right';
  return null;
}

// Port of ReturnTrack(previous, current, next): the shape 'current' must have so the
// path bends/goes straight correctly between its neighbours 'previous' and 'next'.
export function returnTrack(prev, cur, next) {
  const dirToPrev = directionTo(cur.x, cur.y, prev.x, prev.y);
  const dirToNext = directionTo(cur.x, cur.y, next.x, next.y);
  return stateForSides(dirToPrev, dirToNext);
}

// Port of Connects(square1, square2): true if two orthogonally-adjacent cells' placed
// shapes actually agree with each other (one's opening faces the other AND vice versa).
export function connectsMutually(grid, ax, ay, bx, by) {
  const dx = bx - ax;
  const dy = by - ay;
  if (Math.abs(dx) + Math.abs(dy) !== 1) return false;
  const a = stateInfo(grid.stateAt(ax, ay));
  const b = stateInfo(grid.stateAt(bx, by));
  if (dx === 1) return a.right && b.left;
  if (dx === -1) return a.left && b.right;
  if (dy === 1) return a.bottom && b.top;
  return a.top && b.bottom;
}

// Port of the TrackConnectsOutside-driven restricted cycle used whenever the user
// clicks the start/end cell: only ever lands on a shape that opens onto one of this
// cell's outward-facing edges. `edges` = {top,right,bottom,left} booleans for which
// sides of this particular cell face the grid boundary.
export function nextOutwardState(current, edges) {
  let value = current;
  for (let attempt = 0; attempt < 7; attempt++) {
    value = value >= 7 ? 1 : value + 1;
    const s = STATES[value];
    if ((edges.top && s.top) || (edges.right && s.right) || (edges.bottom && s.bottom) || (edges.left && s.left)) {
      return value;
    }
  }
  return value;
}

// Which sides of cell (x,y) on a grid of the given size face the outer boundary.
export function outwardEdges(x, y, size) {
  return {
    top: y === 0,
    left: x === 0,
    bottom: y === size - 1,
    right: x === size - 1,
  };
}
