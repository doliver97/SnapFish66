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
        
        public static List<byte> UnvisitedSteps = new List<byte> { 0, 1, 2, 3, 4}; //TODO kell ez ha úgyis egyesével megyünk rekurzívan?

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

            foreach (byte action in UnvisitedSteps)
            {
                //If game ended, do not go further
                if(IsEnd())
                {
                    return;
                }

                Node child = new Node(state.Copy(), action, (byte)(depth+1));
                bool success = child.state.Step(child.state, action);
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
            if (state.Ascore >= 33 && alpha < -1)
            {
                alpha = -1;
            }
            else if (state.Ascore > 0 && alpha < -2)
            {
                alpha = -2;
            }
            if (state.Bscore >= 33 && beta > 1)
            {
                beta = 1;
            }
            else if (state.Bscore > 0 && beta > 2)
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
                    if(Program.AllowWriteDatabase && depth<=10 && depth%2==0)
                    {
                        value = Max(value,child.AlphaBeta(-3,3));
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
                return value;
            }
            //This node is a minimizer
            else
            {
                value = 3; //Maximum value possible is +3
                foreach (Node child in children)
                {
                    if(Program.AllowWriteDatabase && depth<=10 && depth%2==0)
                    {
                        value = Min(value, child.AlphaBeta(-3, 3));
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
                return value;
            }
        }
    }
}
