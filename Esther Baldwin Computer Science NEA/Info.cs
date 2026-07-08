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

        //draws a soft rounded blue border around each info card panel
        private void Card_Paint(object sender, PaintEventArgs e) => UiTheme.PaintCardBorder(sender, e);

        // the card heights needed to fit their text depend on the actual font metrics/DPI of the
        // machine this runs on (WinForms' AutoScaleMode.Font scaling and AutoSize labels don't always
        // agree on exactly how tall that text ends up), so rather than hard-code pixel heights that
        // only fit on one machine, the cards are sized here to their real rendered content once the
        // labels have laid themselves out - this can never clip regardless of DPI or font settings
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            const int cardPadding = 16;
            const int cardGap = 20;

            whatIsCard.Height = whatIsBody.Bottom + cardPadding;
            howToCard.Height = howToBody.Bottom + cardPadding;

            int topRowBottom = Math.Max(whatIsCard.Bottom, Math.Max(howToCard.Bottom, practiceCard.Bottom));

            algorithmsCard.Top = topRowBottom + cardGap;
            algorithmsCard.Height = algorithmsBody.Bottom + cardPadding;

            exitButton.Top = algorithmsCard.Bottom + 30;

            ClientSize = new Size(ClientSize.Width, exitButton.Bottom + 40);
        }

        //advances the example square at (row, col) to its next state and updates its button image
        private void AdvanceExampleSquare(int row, int col, Button btn)
        {
            int nextValue = exgrid[row, col] + 1;
            if (nextValue > 7)
            {
                nextValue = 0;
            }
            btn.BackgroundImage = BaseGridForm.GetImageForState(nextValue);
            btn.BackgroundImageLayout = ImageLayout.Stretch;
            exgrid[row, col] = nextValue;
        }

        private void gridButton1_Click(object sender, EventArgs e) => AdvanceExampleSquare(0, 0, gridButton1);

        private void gridButton2_Click(object sender, EventArgs e) => AdvanceExampleSquare(0, 1, gridButton2);

        private void gridButton3_Click(object sender, EventArgs e) => AdvanceExampleSquare(1, 0, gridButton3);

        private void gridButton4_Click(object sender, EventArgs e) => AdvanceExampleSquare(1, 1, gridButton4);

        private void Exit_Click(object sender, EventArgs e)
        {
            //exits
            Close();
        }
    }
}
