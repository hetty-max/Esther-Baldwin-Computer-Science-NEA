// Port of Grid.cs. A single Square class hierarchy isn't needed in JS - a bounds
// check gives the same neighbour behaviour as InternalSquare (always 4 neighbours)
// vs ExternalSquare (drops whichever neighbours would fall off the grid).
export class Grid {
  constructor(size) {
    this.size = size;
    this.cells = Array.from({ length: size }, () => new Array(size).fill(0));
  }

  inBounds(x, y) {
    return x >= 0 && y >= 0 && x < this.size && y < this.size;
  }

  isEdge(x, y) {
    return x === 0 || y === 0 || x === this.size - 1 || y === this.size - 1;
  }

  setSquare(x, y, value) {
    if (this.inBounds(x, y)) this.cells[x][y] = value;
  }

  stateAt(x, y) {
    return this.cells[x][y];
  }

  neighbours(x, y) {
    const out = [];
    if (this.inBounds(x, y - 1)) out.push({ x, y: y - 1 });
    if (this.inBounds(x - 1, y)) out.push({ x: x - 1, y });
    if (this.inBounds(x, y + 1)) out.push({ x, y: y + 1 });
    if (this.inBounds(x + 1, y)) out.push({ x: x + 1, y });
    return out;
  }

  noTracksCol(x) {
    let total = 0;
    for (let y = 0; y < this.size; y++) if (this.cells[x][y] !== 0) total++;
    return total;
  }

  noTracksRow(y) {
    let total = 0;
    for (let x = 0; x < this.size; x++) if (this.cells[x][y] !== 0) total++;
    return total;
  }

  clone() {
    const g = new Grid(this.size);
    for (let x = 0; x < this.size; x++) g.cells[x] = this.cells[x].slice();
    return g;
  }
}
