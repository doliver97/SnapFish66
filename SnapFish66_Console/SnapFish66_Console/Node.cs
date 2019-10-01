using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapFish66_Console
{
    public class Node
    {
        private static readonly int maxDepth = 12;

        private static readonly object lockObject = new object();

        public State state;

        public sbyte value;
        bool maximizer;

        public byte actionBefore;

        public byte depth;

    public Node(State nstate, byte nactionBefore, byte ndepth)
        {
            state = nstate;
            actionBefore = nactionBefore;
            depth = ndepth;

            SetMaximizer();
        }

        public bool IsEnd()
        {
            state.CalculatePoints();
            return (state.Apoints!=0 || state.Bpoints!=0);
        }
        
        //Generate all possible children from this node, store in "children" variable
        public Node GenerateChild(byte action)
        {
                //If game ended, do not go further
                if(IsEnd())
                {
                    return null;
                }

                Node child = new Node(state.Copy(), action, (byte)(depth+1));
                bool success = child.state.Step(child.state, action);
                child.SetMaximizer();

                if(success)
                {
                    return child;
                }

                return null;
        }

        public void SetMaximizer()
        {
            if (state.isAnext)
            {
                maximizer = true;
                value = -3;
            }
            else
            {
                maximizer = false;
                value = 3;
            }
        }

        private sbyte Max(sbyte a, sbyte b)
        {
            if(a>b)
            {
                return a;
            }
            else
            {
                return b;
            }
        }

        private sbyte Min(sbyte a, sbyte b)
        {
            if(a>b)
            {
                return b;
            }
            else
            {
                return a;
            }
        }

        public sbyte AlphaBeta(sbyte alpha, sbyte beta)
        {
            sbyte oAlpha = alpha;
            sbyte oBeta = beta;

            //Increasing alpha and decreasing beta if we can
            if (state.AscoreFull >= 33 && alpha < -1)
            {
                alpha = -1;
            }
            else if (state.AscoreFull > 0 && alpha < -2)
            {
                alpha = -2;
            }
            if (state.BscoreFull >= 33 && beta > 1)
            {
                beta = 1;
            }
            else if (state.BscoreFull > 0 && beta > 2)
            {
                beta = 2;
            }

            GameTree.VisitedNodes[depth]++;
            
            //If found in database, we can cut it here
            if (Program.AllowReadDatabase && depth%2==0 && depth<=10)
            {
                sbyte val = (sbyte)GameTree.database.ReadFromDB(this);
                if (val != -100) // -100 means not found
                {
                    GameTree.ReadNodes[depth]++;
                    return val;
                }
            }

            sbyte retVal = 0;
            if(depth < maxDepth)
            {
                GameTree.TranspositionTable.TryGetValue(new Transposition(state, oAlpha, oBeta), out retVal);
            }
            if(retVal != 0)
            {
                return retVal;
            }

            //Trivial end case
            if (IsEnd())
            {
                if (state.Apoints > 0)
                {
                    value = state.Apoints;
                }
                else
                {
                    value = (sbyte)(-state.Bpoints);
                }
                return value;
            }
            
            for(byte i = 0; i < 5; i++)
            {
                Node child = GenerateChild(i);

                if (child != null)
                {
                    if (maximizer)
                    {
                        alpha = Max(alpha, child.AlphaBeta(alpha, beta));
                    }
                    else
                    {
                        beta = Min(beta, child.AlphaBeta(alpha, beta));
                    }

                    if (alpha >= beta)
                    {
                        break;
                    } 
                }
            }

            //Write to database
            if (Program.AllowWriteDatabase)
            {
                if (depth <= 10 && depth % 2 == 0)
                {
                    GameTree.database.AddToDB(this);
                    GameTree.SavedNodes[depth]++;
                }
            }

            if (depth < maxDepth)
            {
                if(maximizer)
                {
                    GameTree.TranspositionTable[new Transposition(state, oAlpha, oBeta)] = alpha;
                }
                else
                {
                    GameTree.TranspositionTable[new Transposition(state, oAlpha, oBeta)] = beta;
                }
            }

            if (maximizer)
            {
                return alpha;
            }
            else
            {
                return beta;
            }
        }
    }
}
