﻿using System;
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

        double value;
        //double alpha;
        //double beta;
        bool maximizer;

        public string actionBefore;

        private bool isRoot;

        public bool closed;
        
        public List<Node> children = new List<Node>();
        private Random random = new Random();

        public double EstimatedValue;

        public int depth;
        
        public List<String> VisitedSteps;
        public List<String> UnvisitedSteps;
        
        public Node(Node newparent, State nstate, string nactionBefore, int ndepth)
        {
            state = nstate;
            actionBefore = nactionBefore;

            if(newparent==null)
            {
                isRoot = true;
                depth = 0;
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

            //3 is the maximum score possible
            //alpha = -3;
            //beta = 3;
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
                    SetEstimatedValue();
                    return;
                }

                Node child = new Node(this, state.Copy(), action, depth + 1);
                bool success = child.state.Step(child.state, action);
                child.SetMaximizer();

                if(success)
                {
                    lock(GameTree.VisitedNodes)
                    {
                        if(!GameTree.VisitedNodes.ContainsKey(child.depth))
                        {
                            GameTree.VisitedNodes[child.depth] = 1;
                        }
                        else
                        {
                            GameTree.VisitedNodes[child.depth]++;
                        }
                    }
                    children.Add(child);
                }
            }
        }

        public void SetMaximizer()
        {
            if (state.next == "A")
            {
                maximizer = true;
                EstimatedValue = -3;
                value = -3;
            }
            else
            {
                maximizer = false;
                EstimatedValue = 3;
                value = 3;
            }
        }

        private void SetEstimatedValue()
        {
            if(state.Apoints!=0)
            {
                EstimatedValue = state.Apoints;
            }
            else
            {
                EstimatedValue = (-1)*state.Bpoints;
            }
        }

        //Getting estimated value of node by recurively calling it for all of its children
        public double GetEstimatedValue()
        {
            //If end or pruned
            if(children.Count == 0)
            {
                return EstimatedValue;
            }

            if(maximizer)
            {
                EstimatedValue = children.Max(x => x.GetEstimatedValue());
            }
            else
            {
                EstimatedValue = children.Min(x => x.GetEstimatedValue());
            }

            return EstimatedValue;
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
            GenerateChildren();

            //Trivial end case
            if(IsEnd())
            {
                if (state.Apoints > 0)
                {
                    EstimatedValue = state.Apoints;
                }
                else
                {
                    EstimatedValue = state.Bpoints * (-1);
                }

                return EstimatedValue;
            }
            //This node is a maximizer
            else if(maximizer)
            {
                value = -3; //Minimum value possible
                foreach (Node child in children)
                {
                    value = Max(value, child.AlphaBeta(alpha, beta));
                    alpha = Max(alpha, value);
                    if (alpha >= beta)
                    {
                        break;
                    }
                }
                return value;
            }
            //This node is a minimizer
            else
            {
                value = 3; //Maximum value possible
                foreach (Node child in children)
                {
                    value = Min(value, child.AlphaBeta(alpha, beta));
                    beta = Min(beta, value);
                    if (alpha >= beta)
                    {
                        break;
                    }
                }

                return value;
            }
        }
    }
}
