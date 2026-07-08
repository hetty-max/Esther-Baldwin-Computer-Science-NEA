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
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();

            //round the corners of the nav buttons so they read as cards, matching the info page
            UiTheme.RoundCorners(generate, 14);
            UiTheme.RoundCorners(solve, 14);
            UiTheme.RoundCorners(set, 14);
            UiTheme.RoundCorners(Info, 14);
        }

        //draws the same rounded blue border used on the info page's cards around the track piece strip
        private void Card_Paint(object sender, PaintEventArgs e) => UiTheme.PaintCardBorder(sender, e);

        private void set_Click(object sender, EventArgs e)
        {
            Set newset = new Set();
            newset.Show();
        }

        private void solve_Click(object sender, EventArgs e)
        {
            Solve newsolve = new Solve();
            newsolve.Show();
        }

        private void generate_Click(object sender, EventArgs e)
        {
            Generate newgenerate = new Generate();
            newgenerate.Show();
        }

        private void Info_Click(object sender, EventArgs e)
        {
            Info info = new Info();
            info.Show();
        }

        private void trackStrip5_Click(object sender, EventArgs e)
        {

        }
    }
}
