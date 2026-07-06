using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Esther_Baldwin_Computer_Science_NEA
{
    internal class InternalSquare : Square
    {
        public InternalSquare(int x, int y, State s)
        : base(x, y, s)
        {
            Top = Tuple.Create(x, y - 1);
            Left = Tuple.Create(x -1, y);
            Bottom = Tuple.Create(x, y + 1);
            Right = Tuple.Create(x + 1, y);
        }
        //an internal square is never allowed to have outside connection
        public override bool AllowsOutsideConnection()
        {
            return false;
        }
        //an internal square always returns 4 neighbours as it always has 4 valid neighbours
        public override List<Tuple<int, int>> GetNeighbours(int size)
        {
            return new List<Tuple<int, int>>
            {
                Top,
                Left,
                Bottom,
                Right
            };
        }
    }
}
