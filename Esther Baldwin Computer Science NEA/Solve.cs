using Microsoft.VisualBasic.ApplicationServices;

namespace Esther_Baldwin_Computer_Science_NEA
{
    public partial class Solve : BaseGridForm
    {
        // cancel search is used to stop the dfs in the middle of a recursion
        // this ensures that early termination can happen e.g. for resets
        // steps is used to count the number of steps each recursion takes to complete
        // a limit on the number of steps is used so the puzzle cannot reccurse infinitely
        private bool cancelSearch = false;
        private int numsteps = 0;

        public Solve()
        {
            InitializeComponent();
            generategrid.Enabled = true;
            solveprob.Enabled = false;
            bfs.Enabled = false;
            dfs.Enabled = false;
            steps.Visible = false;
        }
                                                                                                                                                                                

        private void GernerateGrid_Click(object sender, EventArgs e)
        {
            cancelSearch = false;
            BuildGrid();
        }

        protected override void BuildGrid()
        {
            //cancel any running solves
            cancelSearch = true;

            firstclick = true;
            secondclick = true;
            solveprob.Enabled = true;

            //size of grid determined by the difficulty selected in the difficulty index
            switch (difficultyDropDown.SelectedIndex)
            {
                case 0: size = 6; break;
                case 1: size = 8; break;
                case 2: size = 10; break;
            }
            grid = new Grid(size);

            panelGrid.Controls.Clear();
            panelGrid.Width = 600;
            panelGrid.Height = 600;

            //size of square determined by the size of the grid
            int squareSize = 600 / (size + 1);
            gridButtons = new Button[size, size];
            rowHeaders = new TextBox[size];
            colHeaders = new TextBox[size];


            //for each square in the grid a button is placed in the interactive grid 
            for (int ycoord = 0; ycoord < size; ycoord++)
            {
                for (int xcoord = 0; xcoord < size; xcoord++)
                {
                    State sqstate = State.CreateState(0);
                    grid.SetSquare(xcoord, ycoord, sqstate);
                    Button btn = new Button
                    {
                        Width = squareSize - 2,
                        Height = squareSize -2,
                        Left = (xcoord + 1) *squareSize,
                        Top = (ycoord + 1) * squareSize,
                        Tag = (xcoord, ycoord),
                        //image is found based on the state
                        BackgroundImage = GetImageForState(grid.ValueSquare(xcoord, ycoord)),
                        BackgroundImageLayout = ImageLayout.Stretch,
                        Font = new Font("Segoe UI", 10, FontStyle.Bold)
                    };

                    //everytime a button is clicked the gridbutton click function is called
                    btn.MouseClick += gridButton_Click;

                    panelGrid.Controls.Add(btn);
                    gridButtons[xcoord, ycoord] = btn;

                    //if not in the outer squares of the grid the button is disabled 
                    if (xcoord != 0 && xcoord != size - 1 && ycoord != 0 && ycoord != size - 1)
                        btn.Enabled = false;
                }

            }
            GenerateHeaders(size);
        }
        private void gridButton_Click(object sender, EventArgs e)
        {
            //gets the coordinates of the button that has been clicked and uses that to find the square of it
            Button btn = (Button)sender;
            var (x, y) = ((int, int))btn.Tag;
            Square sq = grid.GetSquare(x, y);

            //if it is the first click this becomes the start square
            if (firstclick == true)
            {

                State newState = State.CreateState(8);
                sq.SetState(newState);
                btn.BackgroundImage = GetImageForState(8);
                btn.BackgroundImageLayout = ImageLayout.Stretch;
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderColor = Color.PowderBlue;
                btn.FlatAppearance.BorderSize = 3;
                xstart = sq.x;
                ystart = sq.y;
                firstclick = false;
                return;
            }

            //if it is the secondclick it becomes the end square 
            else if (secondclick == true)

            {
                if (!(sq.x == xstart && sq.y == ystart))
                {
                    State newState = State.CreateState(9);
                    sq.SetState(newState);
                    btn.BackgroundImage = GetImageForState(9);
                    btn.BackgroundImageLayout = ImageLayout.Stretch;
                    btn.FlatStyle = FlatStyle.Flat;
                    btn.FlatAppearance.BorderColor = Color.SteelBlue;
                    btn.FlatAppearance.BorderSize = 3;
                    xend = sq.x;
                    yend = sq.y;
                    secondclick = false;

                    //disables all buttons in the grid except for start and end
                    for (int xcoord = 0; xcoord < size; xcoord++)
                    {
                        for (int ycoord = 0; ycoord < size; ycoord++)
                        {
                            if (((xcoord == xstart && ycoord == ystart) || (xcoord == xend && ycoord == yend)) == false)
                            {
                                gridButtons[xcoord, ycoord].Enabled = false;
                            }
                        }
                    }

                    Startmessage.Visible = false;
                    return;
                }
                else
                    return;
            }
            else
            {
                //if it is the start square only allow tracks to be placed that connect to the outside
                if ((sq.x == xstart && sq.y == ystart))
                {
                    int trackValue = sq.StateValue();
                    int nextValue = trackValue + 1;
                    if ((sq.x == xstart) && (sq.y == ystart))
                    {
                        if (nextValue > 7)
                        {
                            nextValue = 1;
                        }
                        bool valid = false;
                        if (xstart == 0)
                        {
                            if (ystart == 0)
                            {
                                valid = TrackConnectsOutside(nextValue, left: true, top: true);
                            }
                            else if (ystart == size - 1)
                            {
                                valid = TrackConnectsOutside(nextValue, left: true, bottom: true);
                            }
                            else
                            {
                                valid = TrackConnectsOutside(nextValue, left: true);
                            }
                        }
                        else if (xstart == size - 1)
                        {
                            if (ystart == 0)
                            {
                                valid = TrackConnectsOutside(nextValue, right: true, top: true);
                            }
                            else if (ystart == size - 1)
                            {
                                valid = TrackConnectsOutside(nextValue, right: true, bottom: true);
                            }
                            else
                            {
                                valid = TrackConnectsOutside(nextValue, right: true);
                            }
                        }
                        else
                        {
                            if (ystart == 0)
                            {
                                valid = TrackConnectsOutside(nextValue, top: true);
                            }
                            else if (ystart == size - 1)
                            {
                                valid = TrackConnectsOutside(nextValue, bottom: true);
                            }
                            else
                            {
                                return;
                            }
                        }
                        int attempts = 0;
                        while (!valid && attempts < 7)
                        {
                            nextValue++;
                            if (nextValue > 7)
                                nextValue = 1;

                            valid = TrackConnectsOutside(nextValue,
                                left: sq.x == 0,
                                right: sq.x == size - 1,
                                top: sq.y == 0,
                                bottom: sq.y == size - 1);

                            attempts++;

                        }
                        State finalState = State.CreateState(nextValue);
                        sq.SetState(finalState);
                        btn.BackgroundImage = GetImageForState(nextValue);
                        btn.BackgroundImageLayout = ImageLayout.Stretch;
                    }
                }
                //same with the end square only allow tracks to be placed that connect to the outside square
                else if ((sq.x == xend && sq.y == yend))
                {
                    int trackValue = sq.StateValue();
                    int nextValue = trackValue + 1;
                    if ((sq.x == xend) && (sq.y == yend))
                    {
                        if (nextValue > 7)
                        {
                            nextValue = 1;
                        }
                        bool valid = false;
                        if (xend == 0)
                        {
                            if (yend == 0)
                            {
                                valid = TrackConnectsOutside(nextValue, left: true, top: true);
                            }
                            else if (yend == size - 1)
                            {
                                valid = TrackConnectsOutside(nextValue, left: true, bottom: true);
                            }
                            else
                            {
                                valid = TrackConnectsOutside(nextValue, left: true);
                            }
                        }
                        else if (xend == size - 1)
                        {
                            if (yend == 0)
                            {
                                valid = TrackConnectsOutside(nextValue, right: true, top: true);
                            }
                            else if (yend == size - 1)
                            {
                                valid = TrackConnectsOutside(nextValue, right: true, bottom: true);
                            }
                            else
                            {
                                valid = TrackConnectsOutside(nextValue, right: true);
                            }
                        }
                        else
                        {
                            if (yend == 0)
                            {
                                valid = TrackConnectsOutside(nextValue, top: true);
                            }
                            else if (yend == size - 1)
                            {
                                valid = TrackConnectsOutside(nextValue, bottom: true);
                            }
                            else
                            {
                                return;
                            }
                        }
                        int attempts = 0;
                        while (!valid && attempts < 7)
                        {
                            nextValue++;
                            if (nextValue > 7)
                                nextValue = 1;

                            valid = TrackConnectsOutside(nextValue,
                                left: sq.x == 0,
                                right: sq.x == size - 1,
                                top: sq.y == 0,
                                bottom: sq.y == size - 1);

                            attempts++;

                        }
                        State finalState = State.CreateState(nextValue);
                        sq.SetState(finalState);
                        btn.BackgroundImage = GetImageForState(nextValue);
                        btn.BackgroundImageLayout = ImageLayout.Stretch;
                    }
                }

                else
                {
                    //otherwise state value is just incremented by 1 until greater than 7 when it goes back to zero in a loop
                    int trackValue = sq.StateValue();
                    int nextValue = trackValue + 1;

                    if (nextValue > 7)
                        nextValue = 0;

                    //change the image on the button to the correct corresponding image
                    State finalState = State.CreateState(nextValue);
                    sq.SetState(finalState);
                    btn.BackgroundImage = GetImageForState(nextValue);
                    btn.BackgroundImageLayout = ImageLayout.Stretch;
                }
            }
        }

        private void solveprob_Click(object sender, EventArgs e)
        {
            generategrid.Enabled = false;
            solveprob.Enabled = false;
            bfs.Enabled = true;
            dfs.Enabled = true;

            //disable editing of all headers
            for (int i = 0; i < size; i++)
            {
                rowHeaders[i].ReadOnly = true;
                colHeaders[i].ReadOnly = true;
            }
            //dsiable all gridButtons
            for (int xcoord = 0; xcoord < size; xcoord++)
            {
                for (int ycoord = 0; ycoord < size; ycoord++)
                {
                    Button btn = gridButtons[xcoord, ycoord];
                    btn.Enabled = false;
                }
            }
        }
        //re calculates the number of tracks in each row and column 
        private void GenerateHeaders(int size)
        {
            int squareSize = 600 / (size + 1);
            // creates a textbox for each header above each row and column
            for (int x = 0; x < size; x++)
            {
                TextBox colHeader = new TextBox()
                {
                    Width = squareSize / 2,
                    Height = squareSize,
                    Left = (x + 1) * squareSize + squareSize / 4,
                    Top = 0,
                    //number of tracks in each column
                    Text = (grid.NoTracksCol(x)).ToString(),
                    TextAlign = HorizontalAlignment.Center,
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    ReadOnly = false,

                };
                panelGrid.Controls.Add(colHeader);
                colHeaders[x] = colHeader;
            }
            for (int y = 0; y < size; y++)
            {
                TextBox rowHeader = new TextBox()
                {
                    Width = squareSize / 2,
                    Height = squareSize,
                    Left = 0,
                    Top = (y + 1) * squareSize + squareSize / 4,
                    //number of tracks in each row
                    Text = (grid.NoTracksRow(y)).ToString(),
                    TextAlign = HorizontalAlignment.Center,
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    ReadOnly = false,

                };
                panelGrid.Controls.Add(rowHeader);
                rowHeaders[y] = rowHeader;
            }
        }
        private void dfs_Click(object sender, EventArgs e)
        {
            //on click of dfs disable all reursion buttons
            cancelSearch = false;
            bfs.Enabled = false;
            dfs.Enabled = false;
            steps.Visible = true;

            //set steps to zero
            numsteps = 0;
            steps.Text = "Steps : 0";
            int nextsquarex = 0;
            int nextsquarey = 0;

            //initialise visited grid 
            bool[,] visited = new bool[size, size];
            visited[xstart, ystart] = true;

            Square startsq = grid.GetSquare(xstart, ystart);

            //get next square looking at where the start square is and what it is connected to
            if (startsq.GetState().ConnectsLeft && xstart > 0)
            {
                nextsquarex = xstart - 1;
                nextsquarey = ystart;
            }
            else if (startsq.GetState().ConnectsRight && xstart < size - 1)
            {
                nextsquarex = xstart + 1;
                nextsquarey = ystart;
            }
            else if (startsq.GetState().ConnectsTop && ystart > 0)
            {
                nextsquarex = xstart;
                nextsquarey = ystart - 1;
            }
            else if (startsq.GetState().ConnectsBottom && ystart < size - 1)
            {
                nextsquarex = xstart;
                nextsquarey = ystart + 1;
            }
            Square currentsq = grid.GetSquare(nextsquarex, nextsquarey);

            //set a boolean to store the result of a DFS if the end was reached
            bool solved = DFS(grid, startsq, currentsq, visited);
            if(!solved && !cancelSearch)
            {
                MessageBox.Show("No Solution could be found for this grid");
            }
            gridButtons[xend, yend].BackColor = Color.LightGreen;
        }



        private bool BFS(Grid grid, Square startSquare, Square currentSquare)
        {
            // Breadth First Search to find a valid train track path
            var gridSize = grid.Size;

            //store the allowed number of tracks in each row and column
            int[] rowCounts = new int[gridSize];
            int[] colCounts = new int[gridSize];
            var totalAllowedTracks = 0;

            //read row and column constraints from UI and calculate total number of tracks allowed
            for (int i = 0; i < gridSize; i++)
            {
                if (int.TryParse(colHeaders[i].Text, out int colLimit))
                {
                    colCounts[i] = colLimit;
                    totalAllowedTracks += colLimit;
                }
                if (int.TryParse(rowHeaders[i].Text, out int rowLimit))
                    rowCounts[i] = rowLimit;
            }

            // a queue of trainTracks
            //whether a solution has been found
            bool solved = false;
            TrainPath solutionPath;

            //each item in the queue represents a possible path
            Queue<TrainPath> Q = new Queue<TrainPath>();
            //create the initial path containing start and end square
            TrainPath initalTrack = new TrainPath(grid, rowCounts, colCounts);
            initalTrack.addTrack(startSquare);
            initalTrack.addTrack(currentSquare);

            Q.Enqueue(initalTrack);
            while (Q.Count > 0)
            {
                if (cancelSearch)
                    return false;
                /// where we have got to with our path
                // clear the grid and draw the 
                // current path under consideration
                TrainPath currentPath = Q.Dequeue();
                drawPathOnGrid(currentPath);

                /// how many tracks we have laid?
                int noTracksLaid = currentPath.Size();
                Square currentCell = currentPath.GetLast();

                // if we've laid all of our tracks and
                // currentCell is the end one, 
                // then we should be done
                if (noTracksLaid == totalAllowedTracks && currentCell.x == xend && currentCell.y == yend)
                {
                    Q.Clear();
                    solved = validateGrid();
                }
                else
                {
                    // otherwise loop over the neighbours of current cell
                    foreach (Tuple<int, int> w in currentPath.getValidNeighboursOfLast())
                    {
                        numsteps++;
                        steps.Text = "Steps: " + numsteps.ToString();
                        steps.Refresh();
                        Application.DoEvents();

                        // clone the train track 
                        var newPath = new TrainPath(currentPath);
                        Square sq = grid.GetSquare(w.Item1, w.Item2);
                        newPath.addTrack(sq);
                        Q.Enqueue(newPath);
                    }
                }
            }

            return solved;
        }


        private void drawPathOnGrid(TrainPath track)
        {
            /// clear the grid first
            /// not start and end points
            /// loop through the items in this trainTrack
            /// draw correct track on the grid
            bool[,] visited = new bool[size, size];
            visited[xstart, ystart] = true;
            visited[xend, yend] = true;
            HighlightSquare(xstart, ystart, Color.LightGreen);
            HighlightSquare(xend, yend, Color.LightGreen);


            int counter = 0;
            int gridSize = track.GetGridSize();

            //determine correct track orientation
            Square callingSquare = new ExternalSquare(0, 0, State.CreateState(0), gridSize);
            Square currentSquare = new ExternalSquare(0, 0, State.CreateState(0), gridSize);
            
            //iterate through each square in path
            foreach (Square nextSquare in track)
            {
                //skip first two squares
                //dont place a track on the end square
                counter += 1;
                if (counter > 2 && !(currentSquare.x == xend && currentSquare.y == yend))
                {
                    int trackvalue = ReturnTrack(callingSquare, currentSquare, nextSquare);
                    PlaceTrack(currentSquare.x, currentSquare.y, trackvalue);
                    visited[currentSquare.x, currentSquare.y] = true;
                }
                //move along path
                callingSquare = currentSquare;
                currentSquare = nextSquare;
            }
            //clear all squares not part of the current path
            for (int loopX = 0; loopX < size; loopX++)
            {
                for (int loopY = 0; loopY < size; loopY++)
                {
                    if (!visited[loopX, loopY])
                    {
                        HighlightSquare(loopX, loopY, Color.White);
                        RemoveTrack(loopX, loopY);
                    }
                }
            }
        }



        private bool DFS(Grid grid, Square callingSquare, Square currentSquare, bool[,] visited)
        {
            // Depth First Search
            int x = currentSquare.x;
            int y = currentSquare.y;

            // if the end has been reached check if the grid is valid
            if (x == xend && y == yend)
            {
                HighlightSquare(xend, yend, Color.LightGreen);
                bool solvedGrid = validateGrid();
                // return true from DFS if grid is valid
                return solvedGrid;
            }

            visited[x, y] = true;

            PlaceTrack(x, y, 7);
            HighlightSquare(x, y, Color.Yellow);
            //Thread.Sleep(10);
            // Application.DoEvents();

            //Get neighbours of current square
            List<Tuple<int, int>> NeighboursCurrent = currentSquare.GetNeighbours(size);
            List<Tuple<int, int>> validNeighbours = new List<Tuple<int, int>>();

            //Check if each neighbour is valid
            // looking at the coordinates of the grid
            // and if it has been visited
            foreach (Tuple<int, int> neighbour in NeighboursCurrent)
            {
                int nextx = neighbour.Item1;
                int nexty = neighbour.Item2;

                //if endpoint it automatically is a valid neighbour
                if (nextx == xend && nexty == yend)
                {
                    validNeighbours.Add(neighbour);
                    continue;
                }
                //ensure it is within the bounds 
                if (nextx == -1 || nexty == -1)
                    continue;

                //check not visited
                if (visited[nextx, nexty])
                    continue;

                int trackvalue = ReturnTrack(callingSquare, currentSquare, grid.GetSquare(nextx, nexty));

                //checks it does not violate row and column constraints
                if (!validsquare(nextx, nexty, trackvalue))
                    continue;

                validNeighbours.Add(neighbour);
            }

            //calculate first loss function using the distance from the end square
            double LossFunctionDistance(int i, int j)
            {
                double lossValue = 0;
                lossValue -= Math.Sqrt((xend - i) * (xend - i) + (yend - j) * (yend - j));
                return lossValue;
            }
            //calculate loss function from space left in each row and column
            double LossFunctionRowCol(int i, int j)
            {
                double lossValue = 0;
                if (int.TryParse(colHeaders[i].Text, out int colLimit))
                    lossValue += Math.Pow(colLimit - grid.NoTracksCol(i), 2);

                if (int.TryParse(rowHeaders[j].Text, out int rowLimit))
                    lossValue += Math.Pow(rowLimit - grid.NoTracksRow(j), 2);
                return lossValue;
            }
            //combine both loss functions
            double LossFunctionCombo(int i, int j)
            {
                double lossValue = 0;
                if (int.TryParse(colHeaders[i].Text, out int colLimit))
                    lossValue += colLimit - grid.NoTracksCol(i);

                if (int.TryParse(rowHeaders[j].Text, out int rowLimit))
                    lossValue += rowLimit - grid.NoTracksRow(j);

                lossValue -= Math.Sqrt((xend - i) * (xend - i) + (yend - j) * (yend - j));
                return lossValue;
            }

            //sort the neighbours by the optimisation algorithm picked or none
            switch (OptimisationPick.SelectedIndex)
            {
                case 0:
                    validNeighbours.Sort((a, b) => LossFunctionDistance(b.Item1, b.Item2).CompareTo(LossFunctionDistance(a.Item1, a.Item2)));
                    break;
                case 1:
                    validNeighbours.Sort((a, b) => LossFunctionRowCol(b.Item1, b.Item2).CompareTo(LossFunctionRowCol(a.Item1, a.Item2)));
                    break;
                case 2:
                    validNeighbours.Sort((a, b) => LossFunctionCombo(b.Item1, b.Item2).CompareTo(LossFunctionCombo(a.Item1, a.Item2)));
                    break;
                default:
                    break;
            }


            foreach (Tuple<int, int> neighbour in validNeighbours)
            {
                if (cancelSearch)
                    return false;
                numsteps++;

                steps.Text = "Steps: " + numsteps.ToString();
                steps.Refresh();
                Application.DoEvents();

                int nextx = neighbour.Item1;
                int nexty = neighbour.Item2;

                Square nextSquare = grid.GetSquare(nextx, nexty);

                //calculate the orientation of the track using the last current and next square
                //place that track at specified position

                int trackvalue = ReturnTrack(callingSquare, currentSquare, grid.GetSquare(nextx, nexty));
                PlaceTrack(x, y, trackvalue);

                //if end is found in the next square return true
                if (DFS(grid, currentSquare, nextSquare, visited))
                    return true;
            }
            //if no neighbours backtrack
            //remove the track placed and mark as not visited
            RemoveTrack(x, y);
            visited[x, y] = false;
            HighlightSquare(x, y, Color.White);
            Application.DoEvents();
            return false;
        }

        //checks if the new square entered is valid
        private bool validsquare(int x, int y, int prospectiveTrack = 1)
        {
            if (colHeaders == null || rowHeaders == null)
                return false;
            //gets the limit for the column and row

            if (!int.TryParse(colHeaders[x].Text, out int colLimit))
                return false;

            if (!int.TryParse(rowHeaders[y].Text, out int rowLimit))
                return false;

            // counts number of tracks in that row and column
            int colCount = grid.NoTracksCol(x);
            int rowCount = grid.NoTracksRow(y);

            //checks that that is lower than the limits
            return (colCount < colLimit) && (rowCount < rowLimit);
        }


        private int ReturnTrack(Square previoussq, Square currentsq, Square nextsq)
        {
            //returns the value of the state that needs to be placed on the current square
            //uses the coordinates of the previous and current square to find this
            if (currentsq.Bottom != null && currentsq.Bottom.Item1 == previoussq.x && currentsq.Bottom.Item2 == previoussq.y)
            {
                if (currentsq.Right != null && currentsq.Right.Item1 == nextsq.x && currentsq.Right.Item2 == nextsq.y)
                {
                    return 6;
                }
                else if (currentsq.Left != null && currentsq.Left.Item1 == nextsq.x && currentsq.Left.Item2 == nextsq.y)
                {
                    return 5;
                }
                else if (currentsq.Top != null && currentsq.Top.Item1 == nextsq.x && currentsq.Top.Item2 == nextsq.y)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }

            }
            else if (currentsq.Left != null && currentsq.Left.Item1 == previoussq.x && currentsq.Left.Item2 == previoussq.y)
            {
                if (currentsq.Right != null && currentsq.Right.Item1 == nextsq.x && currentsq.Right.Item2 == nextsq.y)
                {
                    return 2;
                }
                else if (currentsq.Top != null && currentsq.Top.Item1 == nextsq.x && currentsq.Top.Item2 == nextsq.y)
                {
                    return 3;
                }
                else if (currentsq.Bottom != null && currentsq.Bottom.Item1 == nextsq.x && currentsq.Bottom.Item2 == nextsq.y)
                {
                    return 5;
                }
                else
                {
                    return 0;
                }
            }
            else if (currentsq.Top != null && currentsq.Top.Item1 == previoussq.x && currentsq.Top.Item2 == previoussq.y)
            {

                if (currentsq.Right != null && currentsq.Right.Item1 == nextsq.x && currentsq.Right.Item2 == nextsq.y)
                {
                    return 4;
                }
                else if (currentsq.Left != null && currentsq.Left.Item1 == nextsq.x && currentsq.Left.Item2 == nextsq.y)
                {
                    return 3;
                }
                else if (currentsq.Bottom != null && currentsq.Bottom.Item1 == nextsq.x && currentsq.Bottom.Item2 == nextsq.y)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            else if (currentsq.Right != null && currentsq.Right.Item1 == previoussq.x && currentsq.Right.Item2 == previoussq.y)
            {
                if (currentsq.Top != null && currentsq.Top.Item1 == nextsq.x && currentsq.Top.Item2 == nextsq.y)
                {
                    return 4;
                }
                else if (currentsq.Left != null && currentsq.Left.Item1 == nextsq.x && currentsq.Left.Item2 == nextsq.y)
                {
                    return 2;
                }
                else if (currentsq.Bottom != null && currentsq.Bottom.Item1 == nextsq.x && currentsq.Bottom.Item2 == nextsq.y)
                {
                    return 6;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }



        }


        private bool validateGrid()
        {
            bool solvedGrid = true;
            if (colHeaders == null || rowHeaders == null)
                return false;

            //check each header in turn to check that the number of tracks in the column corresponds to the header
            for (int x = 0; solvedGrid && x < size; x++)
            {
                if (!int.TryParse(colHeaders[x].Text, out int colHeader))
                    return false;
                int colCount = grid.NoTracksCol(x);
                //set solved grid to false if one does not match
                solvedGrid &= (colHeader == colCount);
            }
            //check each header in turn to check that the number of tracks in the row corresponds to the header
            for (int y = 0; solvedGrid && y < size; y++)
            {
                if (!int.TryParse(rowHeaders[y].Text, out int rowHeader))
                    return false;
                int rowCount = grid.NoTracksRow(y);
                //set solved grid to false if one does not match
                solvedGrid &= (rowHeader == rowCount);
            }
            if (solvedGrid)
            {
                for (int x = 0; x < size; x++)
                {
                    for (int y = 0; y < size; y++)
                    {
                        HighlightSquare(x, y, Color.LightGreen);
                    }
                }
            }
            return solvedGrid;
        }

        private void panelGrid_Paint(object sender, PaintEventArgs e)
        {

        }

        private void exit_Click(object sender, EventArgs e)
        {
            //exit
            Application.Exit();
        }

        private void Reset_Click(object sender, EventArgs e)
        {
            //reset grid back to its orignial position
            // cancel all current solves
            cancelSearch = true;
            numsteps = 0;
            steps.Text = "Steps: 0";
            steps.Visible = false;
            generategrid.Enabled = true;
            bfs.Enabled = false;
            dfs.Enabled = false;

            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    RemoveTrack(x, y);
                    gridButtons[x, y].BackColor = Color.White;
                }
            }
            //build a new grid
            BuildGrid();
        }
        private void bfs_Click_1(object sender, EventArgs e)
        {
            //enable solving
            //set steps to 0 
            cancelSearch = false;
            bfs.Enabled = false;
            dfs.Enabled = false;
            steps.Visible = true;
            numsteps = 0;
            steps.Text = "Steps : 0";
            int nextsquarex = 0;
            int nextsquarey = 0;

            //get next square looking at where the start square is and what it is connected to
            Square startsq = grid.GetSquare(xstart, ystart);
            if (startsq.GetState().ConnectsLeft && xstart > 0)
            {
                nextsquarex = xstart - 1;
                nextsquarey = ystart;
            }
            else if (startsq.GetState().ConnectsRight && xstart < size - 1)
            {
                nextsquarex = xstart + 1;
                nextsquarey = ystart;
            }
            else if (startsq.GetState().ConnectsTop && ystart > 0)
            {
                nextsquarex = xstart;
                nextsquarey = ystart - 1;
            }
            else if (startsq.GetState().ConnectsBottom && ystart < size - 1)
            {
                nextsquarex = xstart;
                nextsquarey = ystart + 1;
            }
            Square currentsq = grid.GetSquare(nextsquarex, nextsquarey);

            //set result of BFS to solved to identify if the puzzle has been solved or not
            bool solved = BFS(grid, startsq, currentsq);
            if (!solved && !cancelSearch)
            {
                MessageBox.Show("No Solution could be found for this grid");
            }
        }

        private void Exit(object sender, EventArgs e)
        {
            Close();
        }
    }

}
