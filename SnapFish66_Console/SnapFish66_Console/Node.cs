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
        public State state;

        public float value;
        bool maximizer;

        public byte actionBefore;

        public List<Node> children = new List<Node>();

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
        public void GenerateChildren()
        {
            children.Clear();

            for(byte i = 0; i < 5; i++)
            {
                //If game ended, do not go further
                if(IsEnd())
                {
                    return;
                }

                Node child = new Node(state.Copy(), i, (byte)(depth+1));
                bool success = child.state.Step(child.state, i);
                child.SetMaximizer();

                if(success)
                {
                    children.Add(child);
                }
            }
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

        private float Max(float a, float b)
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

        private float Min(float a, float b)
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

        public float AlphaBeta(float alpha, float beta)
        {
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
                int val = GameTree.database.ReadFromDB(this);
                if (val != -100) // -100 means not found
                {
                    GameTree.ReadNodes[depth]++;
                    return val;
                }
            }

            GenerateChildren();

            int maxDepth = 12;

            //Trivial end case
            if(IsEnd())
            {
                if (state.Apoints > 0)
                {
                    value = state.Apoints;
                }
                else
                {
                    value = state.Bpoints * (-1);
                }
                return value;

            }
            //This node is a maximizer
            else if(maximizer)
            {
                value = -3; //Minimum value possible is -3
                foreach (Node child in children)
                {
                    if (depth < maxDepth && GameTree.TranspositionTable.ContainsKey(state))
                    {
                        value = GameTree.TranspositionTable[state];
                    }
                    else
                    {
                        value = Max(value, child.AlphaBeta(alpha, beta));
                    }

                    alpha = Max(alpha, value);
                    if (alpha >= beta)
                    {
                        break;
                    }
                }

                children = new List<Node>(); //Faster than clear

                //Write to database
                if(Program.AllowWriteDatabase)
                {
                    if(depth<=10 && depth%2==0)
                    {
                        GameTree.database.AddToDB(this);
                        GameTree.SavedNodes[depth]++;
                    }
                }
            }
            //This node is a minimizer
            else
            {
                value = 3; //Maximum value possible is +3
                foreach (Node child in children)
                {
                    if (depth < maxDepth && GameTree.TranspositionTable.ContainsKey(state))
                    {
                        value = GameTree.TranspositionTable[state];
                    }
                    else
                    {
                        value = Min(value, child.AlphaBeta(alpha, beta));
                    }

                    beta = Min(beta, value);
                    if (alpha >= beta)
                    {
                        break;
                    }
                }

                children = new List<Node>(); //Faster than clear

                //Write to database
                if (Program.AllowWriteDatabase)
                {
                    if (depth <= 10 && depth % 2 == 0)
                    {
                        GameTree.database.AddToDB(this);
                        GameTree.SavedNodes[depth]++;
                    }
                }
            }
            
            if(depth < maxDepth)
            {
                GameTree.TranspositionTable[state] = value;
            }
            
            return value;
        }
    }
}
