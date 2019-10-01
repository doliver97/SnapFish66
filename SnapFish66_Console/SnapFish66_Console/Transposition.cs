using System;
using System.Collections.Generic;
using System.Text;

namespace SnapFish66_Console
{
    public class Transposition : IEquatable<Transposition>
    {
        public State state;
        public float alpha;
        public float beta;

        public Transposition(State s, float a, float b)
        {
            state = s;
            alpha = a;
            beta = b;
        }

        public bool Equals(Transposition other)
        {
            if (state.Equals(other.state) && alpha == other.alpha && beta == other.beta)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return state.GetHashCode();
        }
    }
}
