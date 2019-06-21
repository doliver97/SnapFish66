using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapFish66
{
    public class Node
    {
        public State state;
        private Node parent;

        public double value;
        bool maximizer;

        public string actionBefore;

        private bool isRoot;

        public bool closed;
        
        public List<Node> children = new List<Node>();
        private Random random = new Random();

        public int depth;
        
        public List<String> VisitedSteps;
        public List<String> UnvisitedSteps;
        
        public Node(Node newparent, State nstate, string nactionBefore, int ndepth)
        {
            state = nstate;
            actionBefore = nactionBefore;

            if(newparent==null)
            {
                depth = ndepth;
                isRoot = true;
            }
            else
            {
                isRoot = false;
                parent = newparent;
                depth = ndepth;
            }

            VisitedSteps = new List<string>();
            UnvisitedSteps = new List<string> { "A1", "A2", "A3", "A4", "A5", "B1", "B2", "B3", "B4", "B5" };

            SetMaximizer();
        }

        private void SetClosed()
        {
            //Trivial case: end of game
            if(IsEnd())
            {
                closed = true;
            }
            else
            {
                //If there are no unvisited possible steps left, and every child is closed, set this as closed
                if(UnvisitedSteps.Count==0)
                {
                    closed = true;
                }
                foreach (Node child in children)
                {
                    if (!child.closed)
                    {
                        closed = false;
                        return;
                    }
                }

            }
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
                value = -4;
            }
            else
            {
                maximizer = false;
                value = 4;
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
            lock (GameTree.VisitedNodes)
            {
                if (!GameTree.VisitedNodes.ContainsKey(depth))
                {
                    GameTree.VisitedNodes[depth] = 1;
                }
                else
                {
                    GameTree.VisitedNodes[depth]++;
                }
            }

            //If found in database, we can cut it here
            if(Main.AllowReadDatabase)
            {
                int val = GameTree.database.ReadFromDB(this);
                if (val != -100) // -100 means not found
                {
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
                value = -4; //Minimum value possible is -3
                foreach (Node child in children)
                {
                    value = Max(value, child.AlphaBeta(alpha, beta));
                    alpha = Max(alpha, value);
                    if (alpha >= beta)
                    {
                        break;
                    }
                }
                children.Clear();

                //Write to database
                if(Main.AllowWriteDatabase)
                {
                    if(depth<=10 && depth%2==0)
                    {
                        GameTree.database.AddToDB(this);
                    }
                }
                return value;
            }
            //This node is a minimizer
            else
            {
                value = 4; //Maximum value possible is +3
                foreach (Node child in children)
                {
                    value = Min(value, child.AlphaBeta(alpha, beta));
                    beta = Min(beta, value);
                    if (alpha >= beta)
                    {
                        break;
                    }
                }
                children.Clear();

                //Write to database
                if (Main.AllowWriteDatabase)
                {
                    if (depth <= 10 && depth % 2 == 0)
                    {
                        GameTree.database.AddToDB(this);
                    }
                }
                return value;
            }
        }
    }
}
