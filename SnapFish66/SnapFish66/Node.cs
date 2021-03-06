﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        public double AlphaBeta(double alpha, double beta, BackgroundWorker worker)
        {
            GameTree.VisitedNodes[depth]++;

            if (depth <= 8)
            {
                worker.ReportProgress(0);
            }

            //If found in database, we can cut it here
            if (Main.AllowReadDatabase && depth%2==0 && depth<=10)
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
                value = -4; //Minimum value possible is -3
                foreach (Node child in children)
                {
                    if(Main.AllowWriteDatabase)
                    {
                        value = Max(value,child.AlphaBeta(-4, 4, worker));
                    }
                    else
                    {
                        value = Max(value, child.AlphaBeta(alpha, beta, worker));
                    }
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
                        GameTree.SavedNodes[depth]++;
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
                    if(Main.AllowWriteDatabase)
                    {
                        value = Min(value, child.AlphaBeta(-4, 4, worker));
                    }
                    else
                    {
                        value = Min(value, child.AlphaBeta(alpha, beta, worker));
                    }
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
                        GameTree.SavedNodes[depth]++;
                    }
                }
                return value;
            }
        }
    }
}
