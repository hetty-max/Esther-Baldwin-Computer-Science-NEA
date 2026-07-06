using System;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Drawing.Printing;
using static System.Windows.Forms.AxHost;

namespace Esther_Baldwin_Computer_Science_NEA
{
    public partial class Set : BaseGridForm
    {

        public Set()
        {
            InitializeComponent();
            difficultyDropDown.SelectedIndex = 1;
            Startmessage.Visible = false;
            valid.Visible = false;
            invalid.Visible = false;
            problemview.Visible = false;
            solveproblem.Visible = false;
            checksolution.Visible = false;
        }
        private void difficultyDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        // on click of the generate button any previous searches stop 
        private void generateGrid_Click(object sender, EventArgs e)
        {
            BuildGrid();
        }
        protected override void BuildGrid()
        {
            firstclick = true;
            secondclick = true;

            valid.Visible = false;
            invalid.Visible = false;
            problemview.Visible = false;
            Startmessage.Visible = true;
            solveproblem.Visible = false;
            checksolution.Visible = false;

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
                        Font = new Font("Segoe UI", 10, FontStyle.Bold)
                    };
                    // everytime a button is clicked gridButton_Click is run for every button
                    btn.MouseClick += gridButton_Click;



                    panelGrid.Controls.Add(btn);
                    gridButtons[xcoord, ycoord] = btn;
                    if (xcoord != 0 && xcoord != size - 1 && ycoord != 0 && ycoord != size - 1)
                        btn.Enabled = false;
                }
            }
            // reset the headers
            GenerateHeaders(size);
            UpdateHeaders();
        }
        private void panelGrid_Paint(object sender, PaintEventArgs e)
        {

        }
        //re calculates the number of tracks in each row and column
        private void GenerateHeaders(int size)
        {
            int squareSize = 600 / (size + 1);
            for (int xcoord = 0; xcoord < size; xcoord++)
            {
                TextBox colHeader = new TextBox()
                {
                    ReadOnly = true,
                    Width = squareSize / 2,
                    Height = squareSize,
                    Left = (xcoord + 1) * squareSize + squareSize / 4,
                    Top = 0,
                    //counts number of tracks in each column
                    Text = (grid.NoTracksCol(xcoord)).ToString(),
                    TextAlign = HorizontalAlignment.Center,
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),

                };
                panelGrid.Controls.Add(colHeader);
                colHeaders[xcoord] = colHeader;
            }
            for (int ycoord = 0; ycoord < size; ycoord++)
            {
                TextBox rowHeader = new TextBox()
                {
                    ReadOnly = true,
                    Width = squareSize / 2,
                    Height = squareSize,
                    Left = 0,
                    Top = (ycoord + 1) * squareSize + squareSize / 4,
                    //counts number of tracks in each row
                    Text = (grid.NoTracksRow(ycoord)).ToString(),
                    TextAlign = HorizontalAlignment.Center,
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),

                };
                panelGrid.Controls.Add(rowHeader);
                rowHeaders[ycoord] = rowHeader;
            }


        }
        //recounts tracks in each row and column without having to regenerate textboces
        private void UpdateHeaders()
        {
            for (int x = 0; x < size; x++)
            {
                int colCount = 0;
                colCount = grid.NoTracksCol(x);
                colHeaders[x].Text = colCount.ToString();
            }
            for (int y = 0; y < size; y++)
            {
                int rowCount = 0;
                rowCount = grid.NoTracksRow(y);
                rowHeaders[y].Text = rowCount.ToString();
            }
        }
        private void gridButton_Click(object sender, EventArgs e)
        {
            // reads which button has been clicked and gets the coordinates of it
            Button btn = (Button)sender;
            // gets the square in grid at those coordinates
            var (x, y) = ((int, int))btn.Tag;
            Square sq = grid.GetSquare(x, y);

            //if it is the first click that square becomes the start square
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
                UpdateHeaders();
                firstclick = false;
                return;
            }
            //if it is the second click that square becomes the end square
            else if (secondclick == true)

            {
                // as long as it is not the start square
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
                    UpdateHeaders();
                    secondclick = false;

                    //enable all buttons to allow the user to set the track
                    for (int xcoord = 0; xcoord < size; xcoord++)
                    {
                        for (int ycoord = 0; ycoord < size; ycoord++)
                        {
                            gridButtons[xcoord, ycoord].Enabled = true;
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
                //if start square only tracks that connect to the outside are allowed to be placed
                int trackValue = sq.StateValue();
                int nextValue = trackValue + 1;
                if ((sq.x == xstart) && (sq.y == ystart))
                {
                    if (nextValue > 7)
                    {
                        nextValue = 0;
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
                        if (nextValue > 7) nextValue = 1;

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

                    UpdateHeaders();

                }
                //same for the end square
                if ((sq.x == xend) && (sq.y == yend))
                {
                    if (nextValue > 7)
                    {
                        nextValue = 0;
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
                        if (nextValue > 7) nextValue = 1;

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

                    UpdateHeaders();
                }
                //if not start or end square simply add one to the state value for the new state
                if ((sq.x != xstart) || (sq.y != ystart))
                {
                    if ((sq.x != xend) || (sq.y != yend))
                    {
                        int currentValue;
                        int newValue = 0;
                        if (sq.StateValue() == 8 || sq.StateValue() == 9)
                            newValue = 0;
                        else
                        {
                            currentValue = sq.StateValue();

                            newValue = currentValue + 1;
                            if (newValue > 7)
                            {
                                newValue = 0;
                            }


                        }
                        State newState = State.CreateState(newValue);
                        sq.SetState(newState);
                        btn.BackgroundImage = GetImageForState(newValue);
                        btn.BackgroundImageLayout = ImageLayout.Stretch;
                        UpdateHeaders();


                    }
                }

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

                if (x > 0 && !visited[x - 1, y] && sqState.ConnectsLeft && (grid.SquareState(x - 1, y)).ConnectsRight)
                {
                    visited[x - 1, y] = true;
                    stack.Push((x - 1, y));
                }

                if (x < size - 1 && !visited[x + 1, y] && sqState.ConnectsRight && (grid.SquareState(x + 1, y)).ConnectsLeft)
                {
                    visited[x + 1, y] = true;
                    stack.Push((x + 1, y));
                }
                if (y > 0 && !visited[x, y - 1] && sqState.ConnectsTop && (grid.SquareState(x, y - 1)).ConnectsBottom)
                {
                    visited[x, y - 1] = true;
                    stack.Push((x, y - 1));
                }

                if (y < size - 1 && !visited[x, y + 1] && sqState.ConnectsBottom && (grid.SquareState(x, y + 1)).ConnectsTop)
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
            for (int xccord = 0; xccord < size; xccord++)
            {
                for (int ycoord = 0; ycoord < size; ycoord++)
                {
                    Square sq = grid.GetSquare(xccord, ycoord);
                    if (sq.StateValue() != 0 && ((visited[sq.x, sq.y]) == false))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private void reset_Click(object sender, EventArgs e)
        {
            //resets all buttons to initial position
            panelGrid.Controls.Clear();

            grid2 = null;
            grid2Buttons = null;

            Startmessage.Visible = true;
            valid.Visible = false;
            invalid.Visible = false;
            generateblankGrid.Visible = true;
            difficultyDropDown.Visible = true;
            finish.Visible = true;

            firstclick = true;
            secondclick = true;

            problemview.Visible = false;
            checksolution.Visible = false;
            solveproblem.Visible = false;

            //rebuilds a new grid

            BuildGrid();
        }


        private void finish_Click(object sender, EventArgs e)
        {
            valid.Visible = false;
            invalid.Visible = false;
            // disables all buttons on the grid
            if (ValidGrid(grid) == true)
            {
                valid.Visible = true;
                for (int xcoord = 0; xcoord < size; xcoord++)
                {
                    for (int ycoord = 0; ycoord < size; ycoord++)
                    {
                        gridButtons[xcoord, ycoord].Enabled = false;
                    }
                }
                problemview.Visible = true;

            }
            else
            { invalid.Visible = true; }
        }

        private void invalid_TextChanged(object sender, EventArgs e)
        {

        }

        private void problemview_Click(object sender, EventArgs e)
        {
            //create a new grid and new grid buttons for the new panel
            grid2Buttons = new Button[size, size];
            panelGrid.Controls.Clear();
            panelGrid.Width = 600;
            panelGrid.Height = 600;
            grid2 = new Grid(size);

            //create a new grid and set all states to 0
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
                        Font = new Font("Segoe UI", 10, FontStyle.Bold)
                    };

                    //on mouse click call a new method as the new grid has already set start and end points so they do not need to be reset
                    btn.MouseClick += grid2Button_Click;
                    btn.Enabled = false;


                    panelGrid.Controls.Add(btn);
                    grid2Buttons[xcoord, ycoord] = btn;
                }
            }

            //set start square to the start square of the other grid
            grid2.SetSquare(xstart, ystart, grid.SquareState(xstart, ystart));
            grid2Buttons[xstart, ystart].BackgroundImage = GetImageForState(grid2.ValueSquare(xstart, ystart));
            grid2Buttons[xstart, ystart].BackgroundImageLayout = ImageLayout.Stretch;
            grid2Buttons[xstart, ystart].Refresh();
            var btnstart = grid2Buttons[xstart, ystart];
            btnstart.FlatStyle = FlatStyle.Flat;
            btnstart.FlatAppearance.BorderSize = 3;
            btnstart.FlatAppearance.BorderColor = Color.LightBlue;
            btnstart.Refresh();

            //set end square to the end square of the other grid
            grid2.SetSquare(xend, yend, grid.SquareState(xend, yend));
            grid2Buttons[xend, yend].BackgroundImage = GetImageForState(grid2.ValueSquare(xend, yend));
            grid2Buttons[xend, yend].BackgroundImageLayout = ImageLayout.Stretch;
            grid2Buttons[xend, yend].Refresh();
            var btnend = grid2Buttons[xend, yend];
            btnend.FlatStyle = FlatStyle.Flat;
            btnend.FlatAppearance.BorderSize = 3;
            btnend.FlatAppearance.BorderColor = Color.LightBlue;
            btnend.Refresh();

            //disable start and end square
            grid2Buttons[xstart, ystart].Enabled = false;
            grid2Buttons[xend, yend].Enabled = false;

            Random rnd = new Random();
            int tracksplaced = 0;
            while (tracksplaced < 2)
            {
                //get a random coordinate within the grid 
                int xcoord = rnd.Next(0, size);
                int ycoord = rnd.Next(0, size);

                //if the random coordinate is not the same in the other grid
                // would be the same if the grid does not have a track in the other grid, it is a start or end point or it has already been set to be the same
                if ((grid.SquareState(xcoord, ycoord).GetState()) == (grid2.SquareState(xcoord, ycoord)).GetState())
                {
                    continue;
                }
                else
                {
                    //set that square to the square on the first grid
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
            Application.DoEvents();

            //disable the buttons used for the first view and enable them for the problem view 
            Startmessage.Visible = false;
            valid.Visible = false;
            invalid.Visible = false;
            generateblankGrid.Visible = false;
            difficultyDropDown.Visible = false;
            finish.Visible = false;

            problemview.Enabled = true;

            problemview.Visible = false;
            checksolution.Visible = true;
            solveproblem.Visible = true;

        }
        private void grid2Button_Click(object sender, EventArgs e)
        {
            //on click of a button in the second grid change the state of the square by 1

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

        private void exit_Click(object sender, EventArgs e)
        {
            //exits
            Close();
        }

        private void checksolution_Click(object sender, EventArgs e)
        {
            //check if it is a valid grid using method before
            if (ValidGrid(grid2))
            {
                //checks if for every row and column the number of tracks in the rows and columns match the constraints 
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
                    MessageBox.Show("In correct solution try again");
                }
            }
            else
            {
                MessageBox.Show("In correct solution try again");
            }
        }

        private void solveproblem_Click(object sender, EventArgs e)
        {
            //allows the user to edit all of the buttons and so solve the puzzle
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

        private void Set_Load(object sender, EventArgs e)
        {

        }
    }

}
