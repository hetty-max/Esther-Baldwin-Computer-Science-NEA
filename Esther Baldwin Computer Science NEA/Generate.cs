using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.DirectoryServices.ActiveDirectory;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Windows.Forms.AxHost;

namespace Esther_Baldwin_Computer_Science_NEA
{
    public partial class Generate : BaseGridForm
    {
        // cancel search is used to stop the dfs in the middle of a recursion
        // this ensures that early termination can happen e.g. for resets
        // steps is used to count the number of steps each recursion takes to complete
        // a limit on the number of steps is used so the puzzle cannot reccurse infinitely
        private bool cancelSearch = false;
        private int steps;


        public Generate()
        {
            InitializeComponent();
            randWalk.Enabled = false;
            ConfirmPuzzle.Enabled = true;
            cancelSearch = false;
            dfs.Enabled = false;
            checksolution.Visible = false;
            solveproblem.Visible = false;
            problemview.Enabled = false;
        }

        private void Generate_Load(object sender, EventArgs e)
        {

        }

        // on click of the generate button any previous searches stop 
        private void GenerateButton_Click(object sender, EventArgs e)
        {

            cancelSearch = false;
            BuildGrid();
        }
        protected override void BuildGrid()
        {
            cancelSearch = true;
            firstclick = true;
            secondclick = true;

            // sets size of grid depending on the input of the user default is 8
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

            //find width of square using selected size
            int squareSize = 600 / (size + 1);
            gridButtons = new Button[size, size];
            rowHeaders = new TextBox[size];
            colHeaders = new TextBox[size];

            //sets each button equally spaced on the grid sets the state to zero so the grid is empty 
            for (int ycoord = 0; ycoord < size; ycoord++)
            {
                for (int xcoord = 0; xcoord < size; xcoord++)
                {
                    State s = State.CreateState(0);
                    grid.SetSquare(xcoord, ycoord, s);
                    Button btn = new Button
                    {
                        Width = squareSize - 2,
                        Height = squareSize - 2,
                        Left = (xcoord + 1) * squareSize,
                        Top = (ycoord + 1) * squareSize,
                        BackgroundImage = GetImageForState(grid.ValueSquare(xcoord, ycoord)),
                        BackgroundImageLayout = ImageLayout.Stretch,
                        Tag = (xcoord, ycoord),
                        Font = new System.Drawing.Font("Segoe UI", 10, FontStyle.Bold)
                    };
                    // everytime a button is clicked gridButton_Click is run for every button
                    btn.MouseClick += gridButton_Click;

                    panelGrid.Controls.Add(btn);
                    gridButtons[xcoord, ycoord] = btn;
                    if (xcoord != 0 && xcoord != size - 1 && ycoord != 0 && ycoord != size - 1)
                        btn.Enabled = false;
                }
            }
            GenerateHeaders(size);
        }
        private void gridButton_Click(object sender, EventArgs e)
        {
            // reads which button has been clicked and gets the coordinates of it
            Button btn = (Button)sender;
            var (x, y) = ((int, int))btn.Tag;
            // gets the square in grid at those coordinates
            Square sq = grid.GetSquare(x, y);

            // if its the first click that becomes the start square
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

            // if its the second click it becomes the end square
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

                    // disable all squares apart from start and end
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

                    return;
                }
                else
                    return;
            }
            else
            {
                // only allow tracks to be placed on the start square that connect to the outside
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
                        //only 7 track states possible (not start and end state) so to not get stuck in an infite loop attempts neccessary
                        int attempts = 0;
                        while (!valid && attempts < 7)
                        {
                            nextValue++;
                            if (nextValue > 7)
                                nextValue = 1;

                            valid = TrackConnectsOutside(nextValue, left: sq.x == 0, right: sq.x == size - 1, top: sq.y == 0, bottom: sq.y == size - 1);
                            attempts++;

                        }
                        State finalState = State.CreateState(nextValue);
                        sq.SetState(finalState);
                        btn.BackgroundImage = GetImageForState(nextValue);
                        btn.BackgroundImageLayout = ImageLayout.Stretch;
                    }
                }
                // only allow tracks to be placed on the end square that connect to the outside
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

                        //only 7 track states possible (not start and end state) so to not get stuck in an infite loop attempts neccessary
                        int attempts = 0;
                        while (!valid && attempts < 7)
                        {
                            nextValue++;
                            if (nextValue > 7)
                                nextValue = 1;

                            valid = TrackConnectsOutside(nextValue, left: sq.x == 0, right: sq.x == size - 1, top: sq.y == 0, bottom: sq.y == size - 1);

                            attempts++;

                        }
                        State finalState = State.CreateState(nextValue);
                        sq.SetState(finalState);
                        btn.BackgroundImage = GetImageForState(nextValue);
                        btn.BackgroundImageLayout = ImageLayout.Stretch;
                    }
                }
            }



        }

        private bool DFS(Grid grid, Square callingSquare, Square currentSquare, bool[,] visited, int[] xheaders, int[] yheaders)
        {
            steps++;

            int x = currentSquare.x;
            int y = currentSquare.y;
            // stop if end point
            if (cancelSearch)
                return false;
            if (steps > 2000)
                return false;
            if (x == xend && y == yend)
            {
                //check penultimate track placement to check it connects to the end square
                if (Connects(callingSquare, grid.GetSquare(xend, yend)))
                {
                    HighlightSquare(xend, yend, Color.LightGreen);
                    for (int xcoord = 0; xcoord < size; xcoord++)
                    {
                        for (int ycoord = 0; ycoord < size; ycoord++)
                        {
                            HighlightSquare(xcoord, ycoord 
                                , Color.LightGreen);
                        }
                    }
                    UpdateHeaders();
                    return true;
                }
                return false;

            }

            //mark current square as visited
            visited[x, y] = true;
            // put a maybe track on that point
            PlaceTrack(x, y, 7);

            HighlightSquare(x, y, Color.Yellow);



            //Get Neighbours of current square
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

                if (nextx == -1 || nexty == -1)
                    continue;
  
                if (visited[nextx, nexty])
                    continue;

                int trackvalue = ReturnTrack(callingSquare, currentSquare, grid.GetSquare(nextx, nexty));

                //checks it does not violate row and column constraints
                if (!validsquare(nextx, nexty, xheaders, yheaders))
                    continue;

                validNeighbours.Add(neighbour);


            }

            foreach (Tuple<int, int> neighbour in validNeighbours)
            {
                if (cancelSearch)
                    return false;

                int nextx = neighbour.Item1;
                int nexty = neighbour.Item2;

                Square nextSquare = grid.GetSquare(nextx, nexty);

                //calculate the orientation of the track using the last current and next square
                //place that track at specified position
                int trackvalue = ReturnTrack(callingSquare, currentSquare, grid.GetSquare(nextx, nexty));
                PlaceTrack(x, y, trackvalue);

                //call DFS of next square
                if (DFS(grid, currentSquare, nextSquare, visited, xheaders, yheaders))
                    return true;

            }
            //if no neighbours left backtrack
            //remove the track placed and mark as not visited
            RemoveTrack(x, y);
            visited[x, y] = false;
            HighlightSquare(x, y, Color.White);
            System.Windows.Forms.Application.DoEvents();
            return false;
        }

        // used to set the headers to be the number of tracks in rows and columns
        private void GenerateHeaders(int size)
        {
            int squareSize = 600 / (size + 1);
            // creates a textbox for each header above each row and column
            for (int x = 0; x < size; x++)
            {
                
                TextBox colHeader = new TextBox()
                {
                    ReadOnly = true,
                    Width = squareSize / 2,
                    Height = squareSize,
                    Left = (x + 1) * squareSize + squareSize / 4,
                    Top = 0,
                    // counts number of tracks in each column
                    Text = (grid.NoTracksCol(x)).ToString(),
                    TextAlign = HorizontalAlignment.Center,
                    Font = new System.Drawing.Font("Segoe UI", 10, FontStyle.Bold),

                };
                panelGrid.Controls.Add(colHeader);
                //adds it to an array
                colHeaders[x] = colHeader;
            }
            for (int y = 0; y < size; y++)
            {
                TextBox rowHeader = new TextBox()
                {
                    ReadOnly = true,
                    Width = squareSize / 2,
                    Height = squareSize,
                    Left = 0,
                    Top = (y + 1) * squareSize + squareSize / 4,
                    // counts number of tracks in each row
                    Text = (grid.NoTracksRow(y)).ToString(),
                    TextAlign = HorizontalAlignment.Center,
                    Font = new System.Drawing.Font("Segoe UI", 10, FontStyle.Bold),

                };
                panelGrid.Controls.Add(rowHeader);
                // adds it to an array
                rowHeaders[y] = rowHeader;
            }


        }

        //checks if the new square entered is valid
        private bool validsquare(int x, int y, int[] xheaders, int[] yheaders)
        {
            if (colHeaders == null || rowHeaders == null)
                return false;

            //gets the limit for the column and row

            int colLimit = xheaders[x];
            int rowLimit = yheaders[y];

            // counts number of tracks in that row and column 
            int colCount = grid.NoTracksCol(x);
            int rowCount = grid.NoTracksRow(y);

            //checks that that is lower than the limits
            return (colCount < colLimit && rowCount < colLimit);

        }
        private void UpdateHeaders()
        {
            // counts number of tracks in each row and column and sets those as the headers
            for (int xcoord = 0; xcoord < size; xcoord++)
            {
                int colCount = 0;
                colCount = grid.NoTracksCol(xcoord);
                colHeaders[xcoord].Text = colCount.ToString();
            }
            for (int ycoord = 0; ycoord < size; ycoord++)
            {
                int rowCount = 0;
                rowCount = grid.NoTracksRow(ycoord);
                rowHeaders[ycoord].Text = rowCount.ToString();
            }
        }

        // generates random headers and returns them as a integer array
        private int[] RandHeaders(int size)
        {
            int[] header = new int[size];
            Random random = new Random();

            for (int i = 0; i < size; i++)
            {
                int randint = random.Next(3, size);
                header[i] = randint;
            }
            return header;

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



        private void dfs_Click_1(object sender, EventArgs e)
        {
            //initialises the dfs generate method
            int nextsquarex = 0;
            int nextsquarey = 0;
            cancelSearch = false;
            steps = 0;

            //generates random constraints for the puzzle using the random headers function
            int[] xheaders = RandHeaders(size);
            int[] yheaders = RandHeaders(size);

            // disable all buttons that do not need to be used
           
            ConfirmPuzzle.Enabled = false;
            cancelSearch = false;

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
            //set the current square as the square one step away from the start
            Square currentsq = grid.GetSquare(nextsquarex, nextsquarey);

            // a boolean solved stores the result of the DFS to determine whether a result has been found from the recursion
            // DFS is run using the start square, current square and the generated headers
            bool solved = DFS(grid, startsq, currentsq, visited, xheaders, yheaders);

            // if the DFS finds a solution a puzzle has been generated
            if (solved)
            {
                gridButtons[xend, yend].BackColor = Color.LightGreen;
                UpdateHeaders();
                System.Windows.Forms.Application.DoEvents();
                // enable problem view so the problem can be viewed now it is generated
                problemview.Enabled = true;
            }
            else
            //no valid solution has been found
            {
                MessageBox.Show("No valid path found. Try again.");
            }

        }
        //another method I use for generating a puzzle
        private bool RandomRecursion(Grid grid, Square previoussq, Square currentsq, bool[,] visited, Random random)
        {
            //increment steps to ensure the problem does not recurse infinitely
            steps++;
            //checks for premature termination- reset or too many recursion
            if (cancelSearch)
                return false;
            else if (steps > 2000)
                return false;

            //if end square
            if (currentsq.x == xend && currentsq.y == yend)
            {
                //check there is a valid connection between penultimate square and end point 
                if (Connects(previoussq, grid.GetSquare(xend, yend)))
                {
                    //end found
                    HighlightSquare(currentsq.x, currentsq.y, Color.LightGreen);
                    UpdateHeaders();
                    for (int xcoord = 0; xcoord < size; xcoord++)
                    {
                        for (int ycoord = 0; ycoord < size; ycoord++)
                        {
                            HighlightSquare(xcoord, ycoord, Color.LightGreen);
                        }
                    }
                    return true;
                }
                return false;
            }

            //set current square to visited
            //Get neighbours from the square and randomise them
            visited[currentsq.x, currentsq.y] = true;
            List<Tuple<int, int>> neighbours = currentsq.GetNeighbours(size).OrderBy(n => random.Next()).ToList();

            //iterates over every neighbour of the square
            foreach (var n in neighbours)
            {
                int neighbourx = n.Item1;
                int neighboury = n.Item2;
                // check neighbour has not been visited and is withing the constraints of the grid
                if (neighbourx < 0 || neighboury < 0 || neighbourx >= size || neighboury >= size || visited[neighbourx, neighboury])
                    continue;

                Square nextsq = grid.GetSquare(neighbourx, neighboury);

                //get the value of track that can be placed on the current square
                int trackValue = ReturnTrack(previoussq, currentsq, nextsq);

                //if empty no valid connection so skip the square
                if (trackValue == 0)
                    continue;

                //place the track
                PlaceTrack(currentsq.x, currentsq.y, trackValue);
                HighlightSquare(currentsq.x, currentsq.y, Color.Yellow);

                //call another recursion using the current square and next square
                if (RandomRecursion(grid, currentsq, nextsq, visited, random))
                    return true;
                // if no neighbours left backtrack
                RemoveTrack(currentsq.x, currentsq.y);
            }
            visited[currentsq.x, currentsq.y] = false;
            HighlightSquare(currentsq.x, currentsq.y, Color.White);
            return false;
        }

        //returns whether two squares connect to one and other
        private bool Connects(Square square1, Square square2)
        {
            //looks at the difference in x and y values between the two squares
            int xdiff = square2.x - square1.x;
            int ydiff = square2.y - square1.y;

            // if the difference is not 1 or -1 the squares cannot be adjacent so they dont connect
            if (Math.Abs(xdiff) + Math.Abs(ydiff) != 1)
                return false;
            State state1 = square1.GetState();
            State state2 = square2.GetState();

            //checks the direction they connect is the same as the difference in coordinates
            if (xdiff == 1 && state1.ConnectsRight && state2.ConnectsLeft)
                return true;
            if (xdiff == -1 && state1.ConnectsLeft && state2.ConnectsRight)
                return true;
            if (ydiff == 1 && state1.ConnectsBottom && state2.ConnectsTop)
                return true;
            if (ydiff == -1 && state1.ConnectsTop && state2.ConnectsBottom)
                return true;
            return false;
        }
        private void ConfirmPuzzle_Click(object sender, EventArgs e)
        {
            //before any generation is done the start and end points must be confirmed
            //this disables all buttons so the generation cannot be distirbed
            GenerateGridButton.Enabled = false;
            dfs.Enabled = true;
            randWalk.Enabled = true;
            for (int xcoord = 0; xcoord < size; xcoord++)
            {
                for (int ycoord = 0; ycoord < size; ycoord++)
                {
                    gridButtons[xcoord, ycoord].Enabled = false;
                }
            }
        }

        private void RandomRecursion_Click_1(object sender, EventArgs e)
        {
            //initialises the random recursion
            int nextsquarex = 0;
            int nextsquarey = 0;
            cancelSearch = false;
            steps = 0;

            ConfirmPuzzle.Enabled = false;
            problemview.Enabled = true;

            //no squares have been visited yet so have a new empty array
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
            //gets nextsquare from grid
            Square currentsq = grid.GetSquare(nextsquarex, nextsquarey);
            Random random = new Random();

            //stores the result of the random recursion to be able to tell if the puzzle has been solved or not
            bool solved = RandomRecursion(grid, startsq, currentsq, visited, random);
            if (solved)
            {
                gridButtons[xend, yend].BackColor = Color.LightGreen;
                UpdateHeaders();
                System.Windows.Forms.Application.DoEvents();
            }
            else
            {
                MessageBox.Show("No valid path found. Try again.");
            }
        }

        private void reset_Click(object sender, EventArgs e)
        {
            //suspends all generation by setting cancelsearch to true
            // returns all ui components to their original positions
            panelGrid.Controls.Clear();

            grid2 = null;
            grid2Buttons = null;

            cancelSearch = true;

            randWalk.Enabled = false;
            ConfirmPuzzle.Enabled = true;
            dfs.Enabled = false;
            problemview.Enabled = false;
            firstclick = true;
            secondclick = true;

            randWalk.Visible = true;
            ConfirmPuzzle.Visible = true;
            dfs.Visible = true;
            GenerateGridButton.Enabled = true;
            GenerateGridButton.Visible = true;
            problemview.Visible = true;
            checksolution.Visible = false;
            solveproblem.Visible = false;

            //generates a new grid
            BuildGrid();
            cancelSearch = false;
        }





        private void problemview_Click(object sender, EventArgs e)
        {
            //hide all no longer useful buttons and show the ones we will now be using
            checksolution.Visible = true;
            solveproblem.Visible = true;
            randWalk.Visible = false;
            ConfirmPuzzle.Visible = false;
            dfs.Visible = false;
            GenerateGridButton.Visible = false;
            problemview.Visible = false;
            difficultyDropDown.Visible = false;

            //clear grid for a new grid to be generated
            panelGrid.Controls.Clear();
            panelGrid.Width = 600;
            panelGrid.Height = 600;
            grid2Buttons = new Button[size, size];
            grid2 = new Grid(size);

            //build new grid in the same way as before
            //but keep the headers from the previous grid
            int squareSize = 600 / (size + 1);
            for (int ycoord = 0; ycoord < size; ycoord++)
            {

                for (int xcoord = 0; xcoord < size; xcoord++)
                {
                    State s = State.CreateState(0);
                    grid2.SetSquare(xcoord, ycoord, s);
                    Button btn = new Button
                    {
                        Width = squareSize - 2,
                        Height = squareSize - 2,
                        Left = (xcoord + 1) * squareSize,
                        Top = (ycoord + 1) * squareSize,
                        BackgroundImage = GetImageForState(grid2.ValueSquare(xcoord, ycoord)),
                        BackgroundImageLayout = ImageLayout.Stretch,
                        Tag = (xcoord, ycoord),
                        Font = new System.Drawing.Font("Segoe UI", 10, FontStyle.Bold)
                    };

                    //calls a new click function as there is no first click second click in this 
                    btn.MouseClick += grid2Button_Click;
                    btn.Enabled = false;


                    panelGrid.Controls.Add(btn);
                    grid2Buttons[xcoord, ycoord] = btn;
                }
            }

            //sets the start square as the start square found in the previous grid
            //disables it so it cannot be changed
            grid2.SetSquare(xstart, ystart, grid.SquareState(xstart, ystart));
            grid2Buttons[xstart, ystart].BackgroundImage = GetImageForState(grid2.ValueSquare(xstart, ystart));
            grid2Buttons[xstart, ystart].BackgroundImageLayout = ImageLayout.Stretch;
            grid2Buttons[xstart, ystart].Refresh();
            var btnstart = grid2Buttons[xstart, ystart];
            btnstart.FlatStyle = FlatStyle.Flat;
            btnstart.FlatAppearance.BorderSize = 3;
            btnstart.FlatAppearance.BorderColor = Color.LightBlue;
            btnstart.Refresh();

            //sets the end square as the start square found in the previous grid
            //disables it so it cannot be changed
            grid2.SetSquare(xend, yend, grid.SquareState(xend, yend));
            grid2Buttons[xend, yend].BackgroundImage = GetImageForState(grid2.ValueSquare(xend, yend));
            grid2Buttons[xend, yend].BackgroundImageLayout = ImageLayout.Stretch;
            grid2Buttons[xend, yend].Refresh();
            var btnend = grid2Buttons[xend, yend];
            btnend.FlatStyle = FlatStyle.Flat;
            btnend.FlatAppearance.BorderSize = 3;
            btnend.FlatAppearance.BorderColor = Color.LightBlue;
            btnend.Refresh();

            grid2Buttons[xstart, ystart].Enabled = false;
            grid2Buttons[xend, yend].Enabled = false;

            //places 2 random tracks from grid 1 into grid 2
            Random rnd = new Random();
            int tracksplaced = 0;
            while (tracksplaced < 2)
            {
                //gets random x and y coordinates
                int xcoord = rnd.Next(0, size);
                int ycoord = rnd.Next(0, size);

                //if the state is not the same on the both- it would be the same on the start and end squae
                // would also be the same if the track was 0 or if a track had already been copied from grid 1 to grid2
                if ((grid.SquareState(xcoord, ycoord).GetState()) == (grid2.SquareState(xcoord, ycoord)).GetState())
                {
                    continue;
                }
                else
                {
                    //set the random point to have the same state as in grid1
                    grid2.SetSquare(xcoord, ycoord, grid.SquareState(xcoord, ycoord));
                    grid2Buttons[xcoord, ycoord].BackgroundImage = GetImageForState(grid2.ValueSquare(xcoord, ycoord));
                    grid2Buttons[xcoord, ycoord].BackgroundImageLayout = ImageLayout.Stretch;
                    grid2Buttons[xcoord, ycoord].Refresh();
                    grid2Buttons[xcoord, ycoord].Enabled = false;
                    var btn = grid2Buttons[xcoord, ycoord];
                    btn.FlatStyle = FlatStyle.Flat;
                    btn.FlatAppearance.BorderSize = 3;
                    btn.FlatAppearance.BorderColor = Color.LightBlue;
                    btn.Refresh();
                    tracksplaced += 1;
                }
            }
            GenerateHeaders(size);
            System.Windows.Forms.Application.DoEvents();
        }
        private void grid2Button_Click(object sender, EventArgs e)
        {
            //very similar to gridbuttonclick but it performs the same funciton every time
            // simply cycles through the states
            Button btn = (Button)sender;
            var (x, y) = ((int, int))btn.Tag;
            Square sq = grid2.GetSquare(x, y);
            bool valid = false;
            int trackValue = sq.StateValue();
            int nextValue = trackValue + 1;
            if (nextValue > 7)
            {
                nextValue = 0;
            }
            State finalState = State.CreateState(nextValue);
            sq.SetState(finalState);
            btn.BackgroundImage = GetImageForState(nextValue);
            btn.BackgroundImageLayout = ImageLayout.Stretch;
        }


        private void solveproblem_Click(object sender, EventArgs e)
        {
            //enables all buttons that are not preplaced tracks so the user can solve the puzzle
            for (int ycoord = 0; ycoord < size; ycoord++)
            {

                for (int xcoord = 0; xcoord < size; xcoord++)
                {
                    Square square = grid2.GetSquare(xcoord, ycoord);
                    if (square.StateValue() != 0)
                    {
                        grid2Buttons[xcoord, ycoord].Enabled = false;
                    }
                    else
                    {
                        grid2Buttons[xcoord, ycoord].Enabled = true;
                    }
                }
            }
        }

        private void checksolution_Click(object sender, EventArgs e)
        {
            //if there is a valid path from start to end with no unconnected tracks
            if (ValidGrid(grid2))
            {
                //and all of the row and column constraints are obeyed
                //the solution is correct
                bool validrowcol = true;
                for (int xcoord = 0; xcoord < size; xcoord++)
                {
                    if (Convert.ToInt32(rowHeaders[xcoord].Text) != grid2.NoTracksRow(xcoord))
                    {
                        validrowcol = false;
                    }
                }
                for (int ycoord = 0; ycoord < size; ycoord++)
                {
                    if (Convert.ToInt32(rowHeaders[ycoord].Text) != grid2.NoTracksRow(ycoord))
                    {
                        validrowcol = false;
                    }
                }
                //display the solution is correct
                if (validrowcol)
                {
                    for (int xcoord = 0; xcoord < size; xcoord++)
                    {
                        for (int ycoord = 0; ycoord < size; ycoord++)
                        {
                            grid2Buttons[xcoord, ycoord].Enabled = false;
                            var btn = grid2Buttons[xcoord, ycoord];
                            btn.FlatStyle = FlatStyle.Flat;
                            btn.FlatAppearance.BorderSize = 3;
                            btn.FlatAppearance.BorderColor = Color.LightGreen;
                            btn.Refresh();
                        }
                    }

                    MessageBox.Show("Correct solution! ");
                }
                else
                {
                    MessageBox.Show("Incorrect solution try again");
                }
            }
            else
            {
                MessageBox.Show("Incorrect solution try again");
            }
            
        }
        protected override bool ValidGrid(Grid grid)
        {
            // uses a stack to iterate over the path from start to finish
            //checks the connections to ensure all are connected
            bool reachedEnd = false;
            bool[,] visited = new bool[size, size];
            Stack<(int x, int y)> stack = new Stack<(int, int)>();

            stack.Push((xstart, ystart));
            visited[xstart, ystart] = true;

            while (stack.Count > 0)
            {
                //gets state of top thing on the stack
                var (x, y) = stack.Pop();
                State sqState = grid.SquareState(x, y);
           
                if ((x == xend && y == yend))
                    reachedEnd = true;

                //checks direction that it connects and goes to the next square that it is connected to
                //ensuring all connections are correct
                if (x > 0 && !visited[x - 1, y] && sqState.ConnectsLeft && (grid.SquareState(x-1, y)).ConnectsRight)
                {
                    visited[x - 1, y] = true;
                    stack.Push((x - 1, y));
                }

                if (x < size - 1 && !visited[x + 1, y] && sqState.ConnectsRight && (grid.SquareState(x + 1, y)).ConnectsLeft)
                {
                    visited[x + 1, y] = true;
                    stack.Push((x + 1, y));
                }
                if (y > 0 && !visited[x, y - 1] && sqState.ConnectsTop && (grid.SquareState(x, y -1)).ConnectsBottom) 
                {
                    visited[x, y - 1] = true;
                    stack.Push((x, y - 1));
                }

                if (y < size - 1 && !visited[x, y + 1] && sqState.ConnectsBottom && (grid.SquareState(x, y +1)).ConnectsTop)
                {
                    visited[x, y + 1] = true;
                    stack.Push((x, y + 1));
                }
            }
            //makes sure there is no unaccounted for loops not originally seen
            return reachedEnd && UnconnectedTracks(visited, grid);

        }
        private bool UnconnectedTracks(bool[,] visited, Grid grid)
        {
            //iterates over grid and sees if there are any tracks that have not been visited
            // all tracks in the path will have been visited
            for (int xcoord = 0; xcoord < size; xcoord++)
            {
                for (int ycoord = 0; ycoord < size; ycoord++)
                {
                    Square sq = grid.GetSquare(xcoord, ycoord);
                    if (sq.StateValue() != 0 && ((visited[sq.x, sq.y]) == false))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private void Exit(object sender, EventArgs e)
        {
            //closes current form
            Close();
        }
    }
}
