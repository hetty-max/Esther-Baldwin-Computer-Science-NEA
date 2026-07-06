using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Esther_Baldwin_Computer_Science_NEA
{
    public class BaseGridForm : Form
    {
        protected Grid grid;
        protected Grid grid2;
        protected Button[,] gridButtons;
        protected Button[,] grid2Buttons;
        protected TextBox[] rowHeaders;
        protected TextBox[] colHeaders;
        protected int size = 8;
        protected bool firstclick = true;
        protected bool secondclick = true;
        protected int xstart = 0;
        protected int ystart = 0;
        protected int xend = 0;
        protected int yend = 0;

        
        //returns the resource corresponding to the state value
        protected Image GetImageForState(int state)
        {
            switch (state)
            {
                case 0: return Properties.Resources.Nothing;
                case 1: return Properties.Resources.StraightTrack;
                case 2: return Properties.Resources.HorizontalTrack;
                case 3: return Properties.Resources.CurveTopLeft;
                case 4: return Properties.Resources.CurveTopRight;
                case 5: return Properties.Resources.CurveBotLeft;
                case 6: return Properties.Resources.CurveBotRight;
                case 7: return Properties.Resources.TrackMaybe;
                case 8: return Properties.Resources.start;
                case 9: return Properties.Resources.end;
                default: return null;
            }
        }

        //returns whether the track connects the outside or not
        protected bool TrackConnectsOutside(int state, bool left = false, bool right = false, bool top = false, bool bottom = false)
        {
            State s = State.CreateState(state);

            if (left && s.ConnectsLeft)
                return true;
            if (right && s.ConnectsRight)
                return true;
            if (top && s.ConnectsTop)
                return true;
            if (bottom && s.ConnectsBottom)
                return true;

            return false;
        }
        
        //colours a square in the grid
        protected void HighlightSquare(int x,int y, Color colour)
        {
            var btn = gridButtons[x, y];
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 3;
            btn.FlatAppearance.BorderColor = colour;
            btn.Refresh();
        }
        //changes the starte of a square in the specified position and refreshes the image
        protected void PlaceTrack(int x , int y, int state)
        {
            grid.SetSquare(x, y, State.CreateState(state));

            gridButtons[x, y].BackgroundImage = GetImageForState(state);
            gridButtons[x, y].BackgroundImageLayout = ImageLayout.Stretch;
            gridButtons[x, y].Refresh();
        }
        //removes a track from the grid- sets it state to 0
        protected void RemoveTrack(int x, int y)
        {
            grid.SetSquare(x, y, State.CreateState(0));

            gridButtons[x, y].BackgroundImage = GetImageForState(0);
            gridButtons[x, y].BackgroundImageLayout = ImageLayout.Stretch;
            gridButtons[x, y].Refresh();
        }
        // to be implemnented in the child classes to create the form
        protected virtual void BuildGrid()
        {
            throw new NotImplementedException();
        }
        // when implemented will return if the grid has valid connections and constraints
        protected virtual bool ValidGrid(Grid grid)
        {
            throw new NotImplementedException();
        }
    }

}
