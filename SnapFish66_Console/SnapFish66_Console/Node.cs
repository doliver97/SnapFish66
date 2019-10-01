﻿using System;
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
        private static object lockObject = new object();

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
            Transposition transposition = new Transposition(state, alpha, beta);

            float oAlpha = alpha;
            float oBeta = beta;

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

            int maxDepth = 10;

            float retVal = 0;
            if(depth < maxDepth)
            {
                GameTree.TranspositionTable.TryGetValue(transposition, out retVal);
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
                    value = state.Bpoints * (-1);
                }
                return value;
            }

            GenerateChildren();

            //This node is a maximizer
            if(maximizer)
            {
                //If not in table
                foreach (Node child in children)
                {
                    alpha = Max(alpha, child.AlphaBeta(alpha, beta));
                    if (alpha >= beta)
                    {
                        break;
                    }
                }
            }
            //This node is a minimizer
            else
            {
                //If not in table
                foreach (Node child in children)
                {
                    beta = Min(beta, child.AlphaBeta(alpha, beta));
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
