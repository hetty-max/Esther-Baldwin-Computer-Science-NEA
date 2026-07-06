using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Esther_Baldwin_Computer_Science_NEA
{
    public class Grid
    {
        private int size;
        // 2D array represents the grid and each of the squares in it
        private Square[,] squaresArray;
        public Grid(int size)
        {
            {
                this.size = size;
                squaresArray = new Square[size, size];
                CreateSquares();
            }

        }
        public int Size => size;
        //updates the state of one square  
        public void SetSquare(int x, int y, State state)
        {
            if (x >= 0 && y >= 0 && x < Size && y < Size)
                squaresArray[x, y].SetState(state);

        }
        // internal method used for initiating grid 
        private void CreateSquares()
        {
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    bool isEdge = x == 0 || y == 0 || x == size - 1 ||y == size - 1;

                    if (isEdge)
                    {
                        squaresArray[x,y] = new ExternalSquare(x, y,  State.CreateState(0), size);
                    }
                    else
                    {
                        squaresArray[ x, y] = new InternalSquare(x, y,State.CreateState(0));
                    }
                }
            }
        }
        // returns square at specified position
        public Square GetSquare(int r, int c)
        {
            return squaresArray[r, c];
        }
        //Gets the value of the state of the square at that position
        public int ValueSquare(int r, int c)
        {
            return ((squaresArray[r, c].StateValue()));
        }
        //gets the state of the square at specified position
        public State SquareState(int r, int c)
        {
            return (squaresArray[r, c].GetState());
        }
        // gets number of tracks in specified column
        public int NoTracksCol(int x)
        {
            int total =0 ;
            for (int  i = 0; i < Size; i++)
            {
                if (ValueSquare(x, i) != 0)
                {
                    total++;
                }
            }
            return total;
        }
        // gets number of tracks in specified row
        public int NoTracksRow(int y)
        {
            int total = 0;
            for (int i = 0; i < Size; i++)
            {
                if (ValueSquare(i, y) != 0)
                {
                    total++;
                }
            }
            return total;
        }

    }
        
    
}
