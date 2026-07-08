using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Esther_Baldwin_Computer_Science_NEA
{
    // shared visual language (blue colour scheme, rounded cards/buttons) used by the menu and info pages
    public static class UiTheme
    {
        public static readonly Color CardBorderColor = Color.FromArgb(174, 214, 241);

        //draws a soft rounded border around whichever panel raised the Paint event, e.g. an info card
        public static void PaintCardBorder(object sender, PaintEventArgs e, int radius = 14)
        {
            if (sender is not Control control)
                return;

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle bounds = new Rectangle(0, 0, control.Width - 1, control.Height - 1);
            using GraphicsPath path = RoundedRectanglePath(bounds, radius);
            using Pen pen = new Pen(CardBorderColor, 2);
            e.Graphics.DrawPath(pen, path);
        }

        //clips a control to a rounded-rectangle region so flat WinForms buttons/panels read as rounded cards
        public static void RoundCorners(Control control, int radius)
        {
            Rectangle bounds = new Rectangle(0, 0, control.Width, control.Height);
            using GraphicsPath path = RoundedRectanglePath(bounds, radius);
            control.Region = new Region(path);
        }

        private static GraphicsPath RoundedRectanglePath(Rectangle bounds, int radius)
        {
            int diameter = radius * 2;
            var path = new GraphicsPath();

            path.AddArc(bounds.X, bounds.Y, diameter, diameter, 180, 90);
            path.AddArc(bounds.Right - diameter, bounds.Y, diameter, diameter, 270, 90);
            path.AddArc(bounds.Right - diameter, bounds.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();

            return path;
        }
    }
}
