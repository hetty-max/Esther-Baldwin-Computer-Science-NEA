using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Esther_Baldwin_Computer_Science_NEA
{
    internal class ExternalSquare : Square
    {
        public ExternalSquare(int x, int y, State s, int size)
            : base(x, y, s)
        {
            //checks that each neighbour is within the grid and only create the tuples that are valid
            if (y > 0)
                Top = Tuple.Create(x, y - 1);
            else
                Top = null;
            if (x > 0)
                Left = Tuple.Create(x-1, y);
            else
                Left = null;
            if (y < size - 1)
                Bottom = Tuple.Create(x, y + 1);
            else
                Bottom = null;
            if (x < size - 1)
                Right = Tuple.Create(x + 1, y);
            else
                Right = null;

        }
        //an external square is always an edge and always connects the outside
        public override bool IsEdge()
        { 
            return true;
        }
        public override bool AllowsOutsideConnection()
        {
            return true;
        }
        //returns all non null neighbours that have just been created
        public override List<Tuple<int, int>> GetNeighbours(int size)
        {
            List<Tuple<int, int>> neighbours = new List<Tuple<int, int>>();
            if (Top != null)
                neighbours.Add(Top);
            if (Left!= null)
                neighbours.Add(Left);
            if (Bottom != null)
                neighbours.Add(Bottom);
            if (Right != null)
                neighbours.Add(Right);
            return neighbours;
        }
    }
}