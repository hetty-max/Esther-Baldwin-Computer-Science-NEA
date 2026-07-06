using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Esther_Baldwin_Computer_Science_NEA
{
    public abstract class Square
    {
        public int x { get; }
        public int y { get; }
        protected  State state;
        //All are the coordinates of the squares adjacent to this square
        public Tuple<int, int> Top;
        public Tuple<int, int>Right;
        public Tuple<int, int>Bottom;
        public Tuple<int, int> Left;
        protected Square(int X, int Y, State s)
        {
            x = X;
            y = Y;
            state = s;
        }
        // gets state of square
        public State GetState()
        {
            return state;
        }
        //implements differently whether external or internal square
        public virtual bool IsEdge()
        {
            return false;
        }
        //returns int value of the state
        public int StateValue()
        {
            return state.GetState();
        }
        //only allowed in external squares
        public abstract bool AllowsOutsideConnection();

        //returns list of the coordinates of neighbours
        public abstract List<Tuple<int, int>> GetNeighbours(int size);
        public Tuple <int,int> GetCoords()
        {
            Tuple<int, int> output = new Tuple<int, int>(x, y);
            return output;
        }
        //set state of the square
        public virtual void SetState(State s)
        {
            if (s != null && s.isValid())
            {
                state = s;
            }
        }
       
    }

    //uses genereic class IEnumerable so that derived classes can be iterated over
    public abstract class TrackList<T> : IEnumerable<T>
    {
        //list of the items in the track
        public List<T> items;
        public TrackList()
        {
            items = new List<T>();
        }
        // Copy constructor creates another tracklist- needed in bfs where tracks are cloned
        public TrackList(TrackList<T> other)
        {
            items = new List<T>(other.items);
        }

        //returns the most recently added item in the track
        public T GetLast() => items.Last();

        // enables foreach loops
        public IEnumerator<T> GetEnumerator() => items.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    //inherits from tracklist to be able to implement foreach and list behavior
    internal class TrainPath : TrackList<Square>
    {
        // how many tracks are allowed in each row and column
        private readonly int[] rowCounts;
        private readonly int[] colCounts;
        private readonly int gridSize;

        //number of track squares placed
        public int Size() => items.Count;

        public TrainPath(Grid trackOwner, int[] rowCounts, int[] colCounts)
        {
            items = new List<Square>();

            gridSize = trackOwner.Size;
            this.rowCounts = new int[gridSize];
            this.colCounts = new int[gridSize];
            for (int i = 0; i < gridSize; i++)
            {
                //constraints must be less than grid size
                this.rowCounts[i] = Math.Min(rowCounts[i], gridSize);
                this.colCounts[i] = Math.Min(colCounts[i], gridSize);
            }
        }
        public int GetGridSize()
        {
            return gridSize;
        }
        // copy constructor used in BFS when branching
        public TrainPath(TrainPath other) : base(other)
        {
            gridSize = other.gridSize;
            rowCounts = new int[gridSize];
            colCounts = new int[gridSize];
            for (int i = 0; i < gridSize; i++)
            {
                rowCounts[i] = other.rowCounts[i];
                colCounts[i] = other.colCounts[i];
            }
        }

        public bool validNewTrack(Square trackToAdd)
        {
            return validNewTrack(trackToAdd.x, trackToAdd.y);
        }

        //same check as above but without needing a full Square instance for the candidate coordinates
        public bool validNewTrack(int x, int y)
        {
            // check the rowCounts
            // check the colCounts
            // of the trackToAdd (r,c)
            bool validTrack = (x >= 0) && (y >= 0);
            validTrack &= (x < gridSize) && (y < gridSize);
            validTrack &= (0 < rowCounts[y]);
            validTrack &= (0 < colCounts[x]);
            // valid tracks cannot appear in the current track!!!
            if (validTrack)
            {
                foreach (Square track in items)
                {
                    if ((track.x == x) && (track.y == y))
                    {
                        validTrack = false;
                        break;
                    }
                }
            }
            return validTrack;
        }

        //adds a new square to the path
        public bool addTrack(Square trackToAdd)
        {
            bool success = false;
            if (validNewTrack(trackToAdd))
            {
                // reduce the rowCounts	
                // reduce the colCounts	
                // of the trackToAdd (r,c)
                rowCounts[trackToAdd.y] -= 1;
                colCounts[trackToAdd.x] -= 1;
                items.Add(trackToAdd);
                success = true;
            }
            return success;
        }

        //returns neighbours of end of path
        public List<Tuple<int, int>> getValidNeighboursOfLast()
        {
            Square lastSquare = this.GetLast();
            List<Tuple<int, int>> NeighboursCurrent = lastSquare.GetNeighbours(gridSize);
            List<Tuple<int, int>> validNeighbours = new List<Tuple<int, int>>();
            // valid neighbours are in the grid and adding them does not exceed row/col counts
            foreach (Tuple<int, int> neighbour in NeighboursCurrent)
            {
                if (!validNewTrack(neighbour.Item1, neighbour.Item2))
                {
                    continue;
                }
                validNeighbours.Add(neighbour);
            }
            // valid neighbours can not appear in the current track either
            return validNeighbours;
        }

    }


}
