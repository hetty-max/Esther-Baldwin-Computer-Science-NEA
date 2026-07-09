import { imageForState } from './state.js';

// A no-rules 2x2 demo: clicking a tile just cycles its shape 0..7, purely so a
// visitor can see what the pieces look like (port of Info.cs's AdvanceExampleSquare).
const demoContainer = document.getElementById('demoGrid');
const size = 2;
const demoState = [
  [0, 0],
  [0, 0],
];

for (let y = 0; y < size; y++) {
  for (let x = 0; x < size; x++) {
    const cell = document.createElement('button');
    cell.type = 'button';
    cell.className = 'tt-cell';
    cell.style.backgroundImage = `url(${imageForState(demoState[x][y])})`;
    cell.addEventListener('click', () => {
      demoState[x][y] = demoState[x][y] + 1 > 7 ? 0 : demoState[x][y] + 1;
      cell.style.backgroundImage = `url(${imageForState(demoState[x][y])})`;
    });
    demoContainer.appendChild(cell);
  }
}
