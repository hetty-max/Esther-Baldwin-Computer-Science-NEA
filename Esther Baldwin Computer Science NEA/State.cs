using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Esther_Baldwin_Computer_Science_NEA
{
    public class State
    {
        public int state {get;}
        public string stateName {get;}
        //booleans of which directions each state connects
        public bool ConnectsTop {get;}
        public bool ConnectsRight {get;}
        public bool ConnectsBottom {get;}
        public bool ConnectsLeft {get;}

        public State(int StateValue, string StateName, bool top = false, bool right = false, bool bottom = false, bool left = false) 
        {
            state = StateValue;
            stateName = StateName;
            ConnectsTop = top;
            ConnectsRight = right;
            ConnectsBottom = bottom;
            ConnectsLeft = left;
        }

        //state value of the state
        public int GetState()
        {
            return state;
        }
        //only allow states from 0 to 9
        public bool isValid()
        {
            if (state >= 0 && state < 10)
                return true;
            else
            return false;
        }
        // creates the state corresponding to each state along with its name and which directions it connects
        public static State CreateState(int value)
        {
            switch (value)
            {
                case 0: return new State(0, "Nothing");
                case 1: return new State(1, "Vertical", top: true, bottom: true);
                case 2: return new State(2, "Horizontal", left: true, right: true);
                case 3: return new State(3, "CurveTopLeft", top: true, left: true);
                case 4: return new State(4, "CurveTopRight", top: true, right: true);
                case 5: return new State(5, "CurveBottomLeft", bottom: true, left: true);
                case 6: return new State(6, "CurveBottomRight", bottom: true, right: true);
                case 7: return new State(7, "TrackMaybe");
                case 8: return new State(8, "Start");
                case 9: return new State(9, "End");
                default: return new State(0, "Nothing");
            }
        }
    }
}
