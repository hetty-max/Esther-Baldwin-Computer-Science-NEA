using Google.OrTools.Sat;
using System;
using System.Collections.Generic;

namespace Esther_Baldwin_Computer_Science_NEA
{
    /*
     * Solves the train tracks puzzle using Google OR-Tools' CP-SAT solver.
     * Models each cell's connections (N/E/S/W) using booleans.
     *  Restricts those booleans to the 7 allowed states (NULL, NS, NW, WE, NE, WS, SE).
     *  Enforces row and column track counts.
     *  Enforces edge consistency between neighbours.
     *  Forces there to be a flow/path from START to END along connected track edges.
     *  You can plug in any row/column counts and any edge START/END positions.
     */
    public static class TracksSolver
    {
        // Map state name -> (N,E,S,W)
        private static readonly Dictionary<string, long[]> TrackPatternBits =
            new Dictionary<string, long[]>
            {
                { "NS", new long[] { 1, 0, 1, 0 } },
                { "NW", new long[] { 1, 0, 0, 1 } },
                { "WE", new long[] { 0, 1, 0, 1 } },
                { "NE", new long[] { 1, 1, 0, 0 } },
                { "WS", new long[] { 0, 0, 1, 1 } },
                { "SE", new long[] { 0, 1, 1, 0 } },
            };

        // -------- Core solver: finds up to maxSolutions distinct simple paths from START to END --------
        //
        // solutions is populated with one int[,] grid per distinct valid track layout, each in the
        // same [x, y] (column, row) / 0-6 state encoding as PlaceTrack, most callers only need
        // solutions[0], but the puzzle's row/column counts don't always pin down a unique path so
        // callers can let the user cycle through the rest.
        // moreSolutionsMayExist is true if the maxSolutions cap (or the time limit) was hit before the
        // search space was fully explored, i.e. there could be additional solutions beyond the ones found.
        public static long SolveTracks(
            int N,
            int[] rowCounts,
            int[] colCounts,
            (int r, int c) start,
            (int r, int c) end,
            string startState,   // "NS", "NW", "WE", "NE", "WS", "SE"
            string endState,
            int maxSolutions,
            out List<int[,]> solutions,
            out bool moreSolutionsMayExist)
        {
            // CP-SAT model object: holds all variables and constraints
            var model = new CpModel();

            // --- 1. VARIABLES -------------------------------------------------
            //
            // For each cell (r,c), we create:
            //   n[r,c], e[r,c], s[r,c], w[r,c] : BoolVars for whether this cell
            //                                    has a connection in each direction.
            //   track[r,c]                     : BoolVar = 1 if the cell is a "track" cell
            //                                    (i.e. one of the 6 non-NULL states).
            //
            // We do NOT create an explicit "state enum" variable; instead we encode states
            // as combinations of the four direction booleans (N,E,S,W) and use a table
            // constraint to restrict to the 7 allowed patterns.
            BoolVar[,] n = new BoolVar[N, N];
            BoolVar[,] e = new BoolVar[N, N];
            BoolVar[,] s = new BoolVar[N, N];
            BoolVar[,] w = new BoolVar[N, N];

            BoolVar[,] track = new BoolVar[N, N];

            for (int r = 0; r < N; r++)
            {
                for (int c = 0; c < N; c++)
                {
                    n[r, c] = model.NewBoolVar($"n_{r}_{c}");
                    e[r, c] = model.NewBoolVar($"e_{r}_{c}");
                    s[r, c] = model.NewBoolVar($"s_{r}_{c}");
                    w[r, c] = model.NewBoolVar($"w_{r}_{c}");
                    track[r, c] = model.NewBoolVar($"track_{r}_{c}");
                }
            }

            // --- 2. TILE SHAPES (LOCAL CONSISTENCY) ---------------------------
            //
            // Each cell's (N,E,S,W) combination must be one of the 7 allowed tile shapes:
            //   - NULL: 0,0,0,0
            //   - NS : 1,0,1,0
            //   - NW : 1,0,0,1
            //   - WE : 0,1,0,1
            //   - NE : 1,1,0,0
            //   - WS : 0,0,1,1
            //   - SE : 0,1,1,0
            //
            // We also define `track[r,c]` to be 1 iff the sum of directions is 2,
            // and 0 iff the sum of directions is 0.
            long[][] allowedPatterns =
            {
                new long[] {0,0,0,0},   // NULL
                new long[] {1,0,1,0},   // NS
                new long[] {1,0,0,1},   // NW
                new long[] {0,1,0,1},   // WE
                new long[] {1,1,0,0},   // NE
                new long[] {0,0,1,1},   // WS
                new long[] {0,1,1,0}    // SE
            };

            int sr = start.r, sc = start.c;
            int er = end.r, ec = end.c;

            for (int r = 0; r < N; r++)
            {
                for (int c = 0; c < N; c++)
                {
                    // Restrict (N,E,S,W) to one of the 7 allowed patterns using a table constraint.
                    IntVar[] vars = { n[r, c], e[r, c], s[r, c], w[r, c] };
                    var table = model.AddAllowedAssignments(vars);
                    foreach (var tuple in allowedPatterns)
                    {
                        table.AddTuple(tuple);
                    }

                    // Sum of direction booleans
                    LinearExpr sumDirs = n[r, c] + e[r, c] + s[r, c] + w[r, c];

                    // track[r,c] == 0 => sumDirs == 0
                    model.Add(sumDirs == 0).OnlyEnforceIf(track[r, c].Not());

                    // track[r,c] == 1 => sumDirs == 2
                    model.Add(sumDirs == 2).OnlyEnforceIf(track[r, c]);

                    // --- 3. PREVENT TRACKS LEAVING THE GRID (EXCEPT AT START/END) ---
                    //
                    // For non-START/END cells on the outer boundary, forbid directions that would
                    // point off the grid. START and END are allowed to connect off-grid so they
                    // can represent entry/exit from outside the puzzle.
                    bool isStart = (r == sr && c == sc);
                    bool isEnd = (r == er && c == ec);

                    if (!isStart && !isEnd)
                    {
                        if (r == 0) model.Add(n[r, c] == 0);   // cannot go north from top row
                        if (r == N - 1) model.Add(s[r, c] == 0);   // cannot go south from bottom row
                        if (c == 0) model.Add(w[r, c] == 0);   // cannot go west from leftmost col
                        if (c == N - 1) model.Add(e[r, c] == 0);   // cannot go east from rightmost col
                    }
                }
            }

            // --- 4. FIX START AND END STATES ----------------------------------
            //
            // The puzzle gives a specific track shape for START and END, e.g. "WS", "NS", etc.
            // We look up the (N,E,S,W) bit pattern and pin those four variables on each cell.
            long[] sp = TrackPatternBits[startState];
            long[] ep = TrackPatternBits[endState];

            model.Add(n[sr, sc] == sp[0]);
            model.Add(e[sr, sc] == sp[1]);
            model.Add(s[sr, sc] == sp[2]);
            model.Add(w[sr, sc] == sp[3]);

            model.Add(n[er, ec] == ep[0]);
            model.Add(e[er, ec] == ep[1]);
            model.Add(s[er, ec] == ep[2]);
            model.Add(w[er, ec] == ep[3]);

            // Explicitly mark START and END as track cells
            model.Add(track[sr, sc] == 1);
            model.Add(track[er, ec] == 1);

            // --- 5. ROW / COLUMN TRACK COUNTS --------------------------------
            //
            // These are the puzzle's numeric clues: each row and each column must contain
            // an exact number of track cells (non-NULL).
            for (int r = 0; r < N; r++)
            {
                var rowList = new List<IntVar>();
                for (int c = 0; c < N; c++)
                {
                    rowList.Add(track[r, c]);
                }
                LinearExpr sumRow = LinearExpr.Sum(rowList);
                model.Add(sumRow == rowCounts[r]);
            }

            for (int c = 0; c < N; c++)
            {
                var colList = new List<IntVar>();
                for (int r = 0; r < N; r++)
                {
                    colList.Add(track[r, c]);
                }
                LinearExpr sumCol = LinearExpr.Sum(colList);
                model.Add(sumCol == colCounts[c]);
            }

            // --- 6. NEIGHBOUR CONSISTENCY BETWEEN CELLS ----------------------
            //
            // If a cell has a north connection, the cell above must have a south connection, etc.
            // We already matched in the "direction" sense via the tile shapes; this enforces
            // that they line up between cells.
            //
            // Vertical: n[r,c] == s[r-1,c] for r>0
            for (int r = 1; r < N; r++)
            {
                for (int c = 0; c < N; c++)
                {
                    model.Add(n[r, c] == s[r - 1, c]);
                }
            }

            // Horizontal: w[r,c] == e[r,c-1] for c>0
            for (int r = 0; r < N; r++)
            {
                for (int c = 1; c < N; c++)
                {
                    model.Add(w[r, c] == e[r, c - 1]);
                }
            }

            // --- 7. EDGE VARIABLES: UNDIRECTED ADJACENCIES -------------------
            //
            // We now switch to a graph viewpoint:
            //   - Each cell is a node.
            //   - An undirected edge between two adjacent nodes is active (edgeActive == 1)
            //     iff there is track in both cells and the connection between them is "on".
            //
            // We map cell (r,c) to a unique node id using NodeId.
            int NodeId(int r, int c) => r * N + c;

            var edgeActive = new Dictionary<(int, int), BoolVar>();

            for (int r = 0; r < N; r++)
            {
                for (int c = 0; c < N; c++)
                {
                    int u = NodeId(r, c);

                    // Vertical neighbour: (r, c) and (r-1, c)
                    if (r > 0)
                    {
                        int v = NodeId(r - 1, c);
                        var key = (Math.Min(u, v), Math.Max(u, v));
                        var edgeVar = model.NewBoolVar($"edge_{key.Item1}_{key.Item2}");
                        edgeActive[key] = edgeVar;

                        // edge only if vertical connection exists and both are track
                        model.Add(edgeVar <= n[r, c]);
                        model.Add(edgeVar <= track[r, c]);
                        model.Add(edgeVar <= track[r - 1, c]);
                        // If all three are 1, edgeVar must be 1
                        model.Add(n[r, c] + track[r, c] + track[r - 1, c] - 2 <= edgeVar);
                    }

                    // Horizontal neighbour: (r, c) and (r, c-1)
                    if (c > 0)
                    {
                        int v = NodeId(r, c - 1);
                        var key = (Math.Min(u, v), Math.Max(u, v));
                        var edgeVar = model.NewBoolVar($"edge_{key.Item1}_{key.Item2}");
                        edgeActive[key] = edgeVar;

                        model.Add(edgeVar <= w[r, c]);
                        model.Add(edgeVar <= track[r, c]);
                        model.Add(edgeVar <= track[r, c - 1]);
                        model.Add(w[r, c] + track[r, c] + track[r, c - 1] - 2 <= edgeVar);
                    }
                }
            }

            // --- 8. DEGREE CONSTRAINTS: ENFORCE A PATH SHAPE -----------------
            //
            // degree[r,c] = number of in-grid neighbours this cell is connected to by track.
            // For a single simple path:
            //   - START and END should have degree 1 (endpoints).
            //   - All other track cells must have degree 2.
            //   - Non-track cells have degree 0.
            IntVar[,] degree = new IntVar[N, N];

            for (int r = 0; r < N; r++)
            {
                for (int c = 0; c < N; c++)
                {
                    int u = NodeId(r, c);
                    var incident = new List<BoolVar>();

                    void AddEdgeIfExists(int rr, int cc)
                    {
                        if (rr < 0 || rr >= N || cc < 0 || cc >= N) return;
                        int v = NodeId(rr, cc);
                        var key = (Math.Min(u, v), Math.Max(u, v));
                        if (edgeActive.TryGetValue(key, out var ev))
                            incident.Add(ev);
                    }

                    AddEdgeIfExists(r - 1, c);
                    AddEdgeIfExists(r + 1, c);
                    AddEdgeIfExists(r, c - 1);
                    AddEdgeIfExists(r, c + 1);

                    degree[r, c] = model.NewIntVar(0, 2, $"deg_{r}_{c}");

                    if (incident.Count > 0)
                        model.Add(degree[r, c] == LinearExpr.Sum(incident));
                    else
                        model.Add(degree[r, c] == 0);

                    bool isStart = (r == sr && c == sc);
                    bool isEnd = (r == er && c == ec);

                    if (isStart || isEnd)
                    {
                        // START and END: exactly one in-grid neighbour
                        model.Add(degree[r, c] == 1);
                    }
                    else
                    {
                        // Non-track cells: degree 0
                        model.Add(degree[r, c] == 0).OnlyEnforceIf(track[r, c].Not());

                        // Track cells (except START/END): degree 2
                        model.Add(degree[r, c] == 2).OnlyEnforceIf(track[r, c]);
                    }
                }
            }

            // --- 9. ORIENT THE PATH AS A TREE ROOTED AT START ----------------
            //
            // We now enforce global connectivity and acyclicity (i.e. the track graph
            // is a tree). Combined with degrees (2 for internal nodes, 1 for endpoints),
            // this means the tree is just one simple path from START to END using all track cells.
            //
            // We do this by choosing a "parent" direction for each edge, forming a rooted tree:
            //   - Each undirected edge {u,v} has two BoolVars p_uv and p_vu.
            //     If edgeActive=0, both must be 0.
            //     If edgeActive=1, exactly one of p_uv, p_vu must be 1.
            //   - For each TRACK node except START, exactly one parent comes in.
            //   - START has no parent.
            //   - Depth variables + parent constraints ensure there are no cycles.

            // parent[(u,v)] = 1 means "u is the parent of v" in the oriented tree
            var parent = new Dictionary<(int, int), BoolVar>();

            foreach (var kv in edgeActive)
            {
                var key = kv.Key;
                BoolVar edgeVar = kv.Value;
                int u = key.Item1;
                int v = key.Item2;

                // Two possible parent directions
                BoolVar p_uv = model.NewBoolVar($"p_{u}_{v}");
                BoolVar p_vu = model.NewBoolVar($"p_{v}_{u}");
                parent[(u, v)] = p_uv;
                parent[(v, u)] = p_vu;

                // Parents only allowed if the edge exists
                model.Add(p_uv <= edgeVar);
                model.Add(p_vu <= edgeVar);

                // On each active edge, exactly one direction is used as a parent edge.
                // If edgeVar = 0 -> p_uv + p_vu = 0
                // If edgeVar = 1 -> p_uv + p_vu = 1
                model.Add(p_uv + p_vu == edgeVar);
            }

            // Each TRACK node except START has exactly one parent; START has none.
            for (int r = 0; r < N; r++)
            {
                for (int c = 0; c < N; c++)
                {
                    int u = NodeId(r, c);
                    var incomingParents = new List<IntVar>();

                    void AddIncoming(int rr, int cc)
                    {
                        if (rr < 0 || rr >= N || cc < 0 || cc >= N) return;
                        int v = NodeId(rr, cc);
                        if (parent.TryGetValue((v, u), out var p_vu))
                            incomingParents.Add(p_vu);
                    }

                    AddIncoming(r - 1, c);
                    AddIncoming(r + 1, c);
                    AddIncoming(r, c - 1);
                    AddIncoming(r, c + 1);

                    LinearExpr sumParents = incomingParents.Count > 0
                        ? LinearExpr.Sum(incomingParents.ToArray())
                        : LinearExpr.Sum(Array.Empty<IntVar>());

                    bool isStart = (r == sr && c == sc);

                    if (isStart)
                    {
                        // START: by definition, root of the tree; no parent
                        model.Add(sumParents == 0);
                    }
                    else
                    {
                        // For a TRACK cell: exactly one parent
                        // For a NULL cell: 0 parents (because track[r,c]==0 => RHS=0)
                        model.Add(sumParents == track[r, c]);
                    }
                }
            }

            // --- 10. DEPTH VARIABLES TO PREVENT CYCLES -----------------------
            //
            // We create an integer depth[r,c] for each cell:
            //   - START depth = 0.
            //   - If p_uv = 1 (u is parent of v), enforce depth[v] = depth[u] + 1.
            //
            // This enforces that the "parent" relation is acyclic, so the track graph
            // becomes a tree (i.e. single connected, no loops).
            int maxDepth = N * N;
            IntVar[,] depth = new IntVar[N, N];

            for (int r = 0; r < N; r++)
            {
                for (int c = 0; c < N; c++)
                {
                    depth[r, c] = model.NewIntVar(0, maxDepth, $"depth_{r}_{c}");

                    // Non-track cells have no parent/child edges, so nothing above would otherwise
                    // pin their depth to a value - it would be a completely free variable ranging
                    // over 0..maxDepth. With enumerate_all_solutions that freedom alone produces
                    // huge numbers of "solutions" that are actually identical track layouts, so we
                    // pin it to a fixed value here.
                    model.Add(depth[r, c] == 0).OnlyEnforceIf(track[r, c].Not());
                }
            }

            // Root of the tree
            model.Add(depth[sr, sc] == 0);

            int M = maxDepth + 1; // Big-M for conditional equalities

            foreach (var kv in parent)
            {
                var key = kv.Key;
                IntVar p_uv = kv.Value;
                int u = key.Item1;
                int v = key.Item2;

                int ur = u / N, uc = u % N;
                int vr = v / N, vc = v % N;

                // If p_uv == 1, we want: depth[v] == depth[u] + 1.
                // We encode this with two inequalities and a big-M to allow slack when p_uv == 0:
                //
                // depth[v] >= depth[u] + 1 - M*(1 - p_uv)
                // depth[v] <= depth[u] + 1 + M*(1 - p_uv)
                //
                // If p_uv == 1, these become depth[v] >= depth[u]+1 and depth[v] <= depth[u]+1.
                // If p_uv == 0, they become non-binding.
                model.Add(depth[vr, vc] >= depth[ur, uc] + 1 - M * (1 - p_uv));
                model.Add(depth[vr, vc] <= depth[ur, uc] + 1 + M * (1 - p_uv));
            }

            // --- 11. SOLVE ----------------------------------------------------
            //
            // We don't set an objective: this is a pure feasibility problem. Rather than stopping at
            // the first feasible assignment, we use enumerate_all_solutions with a callback so we can
            // collect every distinct track layout up to maxSolutions, then stop the search early.
            // enumerate_all_solutions requires a single search worker.
            var collector = new SolutionCollector(N, n, e, s, w, maxSolutions);
            var solver = new CpSolver
            {
                StringParameters = "max_time_in_seconds:30,enumerate_all_solutions:true,num_search_workers:1"
            };
            var status = solver.Solve(model, collector);
            var satSteps = solver.NumBranches();

            solutions = collector.Solutions;

            // Optimal/Infeasible mean CP-SAT proved it had explored the whole search tree, so what we
            // collected (possibly none) is the complete solution set. Anything else (e.g. we called
            // StopSearch() after hitting maxSolutions, or the time limit was reached) means there could
            // be more solutions out there that were never found.
            moreSolutionsMayExist = status != CpSolverStatus.Optimal && status != CpSolverStatus.Infeasible;

            return satSteps;
        }

        // Collects one decoded grid per distinct feasible solution CP-SAT finds, stopping the search
        // once maxSolutions have been collected so an under-constrained puzzle can't run forever.
        private class SolutionCollector : CpSolverSolutionCallback
        {
            private readonly int N;
            private readonly BoolVar[,] n;
            private readonly BoolVar[,] e;
            private readonly BoolVar[,] s;
            private readonly BoolVar[,] w;
            private readonly int maxSolutions;

            public List<int[,]> Solutions { get; } = new List<int[,]>();

            public SolutionCollector(int n_, BoolVar[,] north, BoolVar[,] east, BoolVar[,] south, BoolVar[,] west, int maxSolutions)
            {
                N = n_;
                n = north;
                e = east;
                s = south;
                w = west;
                this.maxSolutions = maxSolutions;
            }

            // Decodes the solved boolean variables into the same 0-6 state values used elsewhere in
            // the project (see State.CreateState), writing them into solution[x, y]
            // (x = column, y = row) so callers can feed it straight into PlaceTrack.
            public override void OnSolutionCallback()
            {
                int CellState(int r, int c)
                {
                    long nr = Value(n[r, c]);
                    long er_ = Value(e[r, c]);
                    long sr_ = Value(s[r, c]);
                    long wr_ = Value(w[r, c]);

                    var pattern = (nr, er_, sr_, wr_);
                    return pattern switch
                    {
                        (0, 0, 0, 0) => 0,
                        (1, 0, 1, 0) => 1,
                        (1, 0, 0, 1) => 3,
                        (0, 1, 0, 1) => 2,
                        (1, 1, 0, 0) => 4,
                        (0, 0, 1, 1) => 5,
                        (0, 1, 1, 0) => 6,
                        _ => 7
                    };
                }

                var solutionGrid = new int[N, N];
                for (int r = 0; r < N; r++)
                {
                    for (int c = 0; c < N; c++)
                    {
                        solutionGrid[c, r] = CellState(r, c);
                    }
                }

                // Defence in depth: the model should no longer have any free variables that don't
                // affect the visible track layout, but if one ever creeps back in, CP-SAT would enumerate
                // it as a separate "solution" that looks identical on the grid. Skip it rather than
                // showing the user the same layout twice (and burning a slot in the maxSolutions cap on it).
                foreach (var existing in Solutions)
                {
                    if (GridsEqual(existing, solutionGrid))
                    {
                        return;
                    }
                }

                Solutions.Add(solutionGrid);

                if (Solutions.Count >= maxSolutions)
                {
                    StopSearch();
                }
            }

            private static bool GridsEqual(int[,] a, int[,] b)
            {
                for (int x = 0; x < a.GetLength(0); x++)
                {
                    for (int y = 0; y < a.GetLength(1); y++)
                    {
                        if (a[x, y] != b[x, y])
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
        }
    }
}
