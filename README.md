For my Computer Science A-Level NEA I created a C# WinForms application to generate, solve and allow users to set their own Train Tracks puzzles.
A train tracks puzzle is a newspaper logic puzzle in which a single track must be laid from village A to village B using only straight and corner pieces. 
The number of track segments in each row and column must satisfy the row and column constraints of that row or column header.

During my project I was not allowed to use any AI tools to enhance my project however, once it was over I used AI to enhance what I had done already. I used it to add the google SAT solver to solve the puzzles which I had wanted to do in my original project but run out of time.

This is my reflection on the original project:

I chose this project as it allowed me to use and grow my skills in areas I had not developed as much before.
Specifically, I found graph traversal, constraint satisfaction and UI design interesting.
Furthermore, the puzzle is simple to state but the constraints make the generation and solving non-trivial which allowed me to implement, compare and optimis several distinct algotithms rather than relying on a single 'correct' solution.


## What the program does:
- Generates a random, guaranted- solvable puzzle on an adjustable grid using either a random walk algorithm or a constrained Depth First Search over randomly generated row and column headers.
-  Solves any generated or user-supplied puzzle with a choice of two algorithms and some optisations.
    - Depth First Search, implements using a stack and backtracks whenever a placement violates a row or column constraint
    - This Depth First search is then optimised using loss algorithms algorithms looking at the distance each square is away from the goal and the number of squares that are left for that column to be fully satisfied.
    - The user can choose to look at these things individually when picking the next square or combine them when picking the next square.
    - Breadth First Search, implements using a queue which is guaranteed to find the shortest path but which is a lot slower than depth first search.
- Lets a user set their own puzzle by hand, with placement rules enforced live (start/end restricted to the grid edge, only valid connecting pieces accepted) and a stack-based validator that confirms the entered solution forms a single connected path with no orphaned track.
- Visualises the chosen algorithm solving the puzzle in real time, with a running step count, so the difference in search strategy is something the user can actually see rather than just being told about.
- Includes a difficulty-scaled, adjustable grid size and a built-in tutorial/help panel, so the puzzle is approachable to players unfamiliar with it.
<img width="643" height="635" alt="Screenshot 2026-02-09 200950" src="https://github.com/user-attachments/assets/7fcf7c89-f0d7-487d-8010-91c07bb0209d" />

## How it's put together

I modelled the grid as a 2D array of `Square` objects rather than a primitive array of integers, because the puzzle logic depends heavily on adjacency and edge-behaviour that reads far more clearly as object relationships than as index arithmetic scattered through the algorithm code. `Square` is abstract, with `InternalSquare` and `ExternalSquare` overriding `GetNeighbours()` and `IsEdge()` — internal squares always expose four neighbours, external squares fewer, and only external squares can host the start/end points and connect outward. Pushing this distinction into the type system meant the solving and generating algorithms could call the same interface everywhere and stay agnostic to grid position, which kept them considerably easier to reason about and debug.
Each square holds a `State`, representing either "empty" or a specific track orientation, along with four booleans indicating which sides that orientation connects to. Paths themselves are represented with a generic `trackList<T>`, implementing `IEnumerable<T>`, specialised as `TrainPath : trackList<Square>`. I used this so that a partial path could be pushed onto a stack or queue and later restored with its exact constraint state (row/column counts at that point) intact — this was essential for BFS in particular, since popping a path off the queue has to reconstruct exactly where that path had got to before continuing the search correctly.
The UI is built around a shared `BaseGridForm`, with the grid itself rendered as WinForms `Button` controls — each button stores its coordinates in `Tag` and updates its image and colour to reflect the underlying square state. I would have liked to make `BaseGridForm` abstract, since conceptually it is a template for the three modes (Generate/Solve/Set), but the WinForms designer cannot operate on an abstract form, so I settled for virtual methods overridden per mode instead.


### Core classes

| Class | Responsibility |
|---|---|
| `Grid` | Owns the 2D array of squares; provides get/set access and row/column track counts |
| `Square` (abstract), `InternalSquare`, `ExternalSquare` | A single cell, its state, and its valid neighbours |
| `State` | Track orientation for a square, including which sides it connects to |
| `trackList<T>` / `TrainPath` | Iterable, stack/queue-friendly representation of a partial or complete track |
| `Generate` | Puzzle generation algorithms (random walk, constrained DFS) |
| `Solve` | Puzzle solving algorithms (DFS, optimised DFS, BFS) |
| `Set` | Manual puzzle entry and stack-based validation |
| `Menu`, `BaseGridForm`, `Info` | UI: mode selection, shared grid behaviour, and the tutorial/help panel |

<img width="981" height="674" alt="Screenshot 2026-02-10 153506" src="https://github.com/user-attachments/assets/f1776153-0281-43c8-b918-2c484d82e3c3" />


## Evaluation

Nearly every objective I set at the start of the project was fully met, verified against the graphical interface, the algorithm behaviour, and direct user feedback. The two exceptions were around guaranteeing unique solutions for generated and set puzzles. Once I had implemented the generator, I realised that Train Tracks puzzles very rarely have a unique solution by construction, so enforcing uniqueness would have meant rejecting the large majority of otherwise valid puzzles. I made the deliberate decision to relax that requirement to solvability alone, since a program that only rarely produces a puzzle is a worse outcome than one that occasionally produces a puzzle with more than one valid solution — a trade-off I'm confident was the right call given what I learned about the puzzle domain, even though it meant not fully meeting the objective as originally written.

Feedback on the finished program was positive, particularly on the generation of unlimited puzzles, the real-time visualisation of the solving algorithms, and the ability to directly compare optimisation strategies. The most useful piece of criticism I received was on the UI: a click-and-hold interaction for cycling through track states would be more intuitive than the current click-per-state approach, and it's worth questioning whether the three modes need to be separate forms at all. Both points are reflected below rather than dismissed, since I think they'd meaningfully improve the experience.
<img width="634" height="624" alt="Screenshot 2026-02-12 150253" src="https://github.com/user-attachments/assets/9b2f5454-ff61-4bc6-a941-5e3e0ff1e6ab" />
<img width="981" height="727" alt="Screenshot 2026-02-12 150148" src="https://github.com/user-attachments/assets/bfa571a3-e6ae-462a-be8b-73d5badd182e" />

### What I'd do differently

- Implement a click-and-hold or drag interaction for placing tracks, rather than the current click-per-state cycle.
- Investigate a constraint-solver approach (I looked briefly at Google OR-Tools) as an alternative to hand-rolled DFS/BFS. I chose not to pursue this given the time available and that I already had two non-trivial algorithms to implement and optimise, but I think it would be a strong extension.
- Consider merging the three operational modes into a single form, balancing the clarity of separate modes against the friction of switching between them.
- Add an optional timer, though this would require standardising puzzles across users (e.g. a shared daily puzzle) to make comparison meaningful, which is a bigger change than it first appears.
