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
        private Node parent;

        public double value;
        bool maximizer;

        public string actionBefore;

        public List<Node> children = new List<Node>();
        private Random random = new Random();

        public int depth;
        
        public List<String> VisitedSteps;
        public List<String> UnvisitedSteps;
        
        public Node(Node newparent, State nstate, string nactionBefore, int ndepth)
        {
            state = nstate;
            actionBefore = nactionBefore;
            depth = ndepth;

            if(newparent!=null)
            {
                parent = newparent;
            }

            VisitedSteps = new List<string>();
            UnvisitedSteps = new List<string> { "A1", "A2", "A3", "A4", "A5", "B1", "B2", "B3", "B4", "B5" };

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

            foreach (string action in UnvisitedSteps)
            {
                //If game ended, do not go further
                if(IsEnd())
                {
                    return;
                }

                Node child = new Node(this, state.Copy(), action, depth + 1);
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
            if (state.next == "A")
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

        private double Max(double a, double b)
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

        private double Min(double a, double b)
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

        public double AlphaBeta(double alpha, double beta)
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
