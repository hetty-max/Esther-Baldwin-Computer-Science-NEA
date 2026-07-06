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
        }

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
    }
}
