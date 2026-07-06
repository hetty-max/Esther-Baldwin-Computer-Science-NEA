using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Esther_Baldwin_Computer_Science_NEA
{
    public partial class Info : Form
    {
        //creates a 2x2 2D grid for the practice grid
        private int[,] exgrid = new int[2, 2];
        public Info()
        {
            InitializeComponent();

            exgrid[0, 0] = 0;
            exgrid[1, 0] = 0;
            exgrid[0, 1] = 0;
            exgrid[1, 1] = 0;
        }
        //returns the image at that state
        private Image GetImageForState(int state)
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
        //changes the state of that button in the grid
        private void gridButton1_Click(object sender, EventArgs e)
        {
            int trackValue = exgrid[0, 0];
            int nextValue = trackValue + 1;
            if (nextValue > 7)
            {
                nextValue = 0;
            }
            State finalState = State.CreateState(nextValue);
            gridButton1.BackgroundImage = GetImageForState(nextValue);
            gridButton1.BackgroundImageLayout = ImageLayout.Stretch;
            exgrid[0, 0] = nextValue;
        }

        private void gridButton2_Click(object sender, EventArgs e)
        {
            int trackValue = exgrid[0, 1];
            int nextValue = trackValue + 1;
            if (nextValue > 7)
            {
                nextValue = 0;
            }
            State finalState = State.CreateState(nextValue);
            gridButton2.BackgroundImage = GetImageForState(nextValue);
            gridButton2.BackgroundImageLayout = ImageLayout.Stretch;
            exgrid[0, 1] = nextValue;

        }

        private void gridButton3_Click(object sender, EventArgs e)
        {
            int trackValue = exgrid[1, 0];
            int nextValue = trackValue + 1;
            if (nextValue > 7)
            {
                nextValue = 0;
            }
            State finalState = State.CreateState(nextValue);
            gridButton3.BackgroundImage = GetImageForState(nextValue);
            gridButton3.BackgroundImageLayout = ImageLayout.Stretch;
            exgrid[1, 0] = nextValue;
        }

        private void gridButton4_Click(object sender, EventArgs e)
        {
            int trackValue = exgrid[1, 1];
            int nextValue = trackValue + 1;
            if (nextValue > 7)
            {
                nextValue = 0;
            }
            State finalState = State.CreateState(nextValue);
            gridButton4.BackgroundImage = GetImageForState(nextValue);
            gridButton4.BackgroundImageLayout = ImageLayout.Stretch;
            exgrid[1, 1] = nextValue;
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            //exits
            Close();
        }
    }
}
