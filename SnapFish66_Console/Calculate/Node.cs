using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculate
{
    public class Node
    {
        private static readonly int maxDepth = 12;
        private static readonly int mostLikelyTreshold = 10;

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
        
        //Generate all possible children from this node, store in "children" variable
        public Node GenerateChild(byte action)
        {
                //If game ended, do not go further
                if(state.isEnd)
                {
                    return null;
                }

                Node child = new Node(state.Copy(), action, (byte)(depth+1));
                bool success = child.state.StepOne(child.state, action);
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
            //if (Calculator.AllowReadDatabase && depth%2==0 && depth<=10)
            //{
            //    sbyte val = (sbyte)GameTree.database.ReadFromDB(this);
            //    if (val != -100) // -100 means not found
            //    {
            //        GameTree.ReadNodes[depth]++;
            //        return val;
            //    }
            //}

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
            if (state.isEnd)
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

            byte mostLikely = CalculateMostLikely();

            for (byte i = 0; i < 5; i++)
            {
                byte childIdx = i;
                if(i==0)
                {
                    childIdx = mostLikely;
                }
                else if(i == mostLikely)
                {
                    childIdx = 0;
                }

                Node child = GenerateChild(childIdx);

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
            //if (Calculator.AllowWriteDatabase)
            //{
            //    if (depth <= 10 && depth % 2 == 0)
            //    {
            //        GameTree.database.AddToDB(this);
            //        GameTree.SavedNodes[depth]++;
            //    }
            //}

            //Write to transposition table
            if (depth < maxDepth && GC.GetTotalMemory(false)<1000000000)
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

        private byte CalculateMostLikely()
        {
            if(depth > mostLikelyTreshold)
            {
                return 0;
            }

            Card[] hand;
            if(state.isAnext)
            {
                hand = new Card[] { state.a1, state.a2, state.a3, state.a4, state.a5};
            }
            else
            {
                hand = new Card[] { state.a1, state.a2, state.a3, state.a4, state.a5 };
            }

            //coming first
            if(state.adown == null && state.bdown == null)
            {
                //Put down king of 20/40
                byte marriagePos = state.MarriagePosition(hand);
                if(marriagePos!=255)
                {
                    return marriagePos;
                }

                //Put nontrump 11
                byte nonTrump11 = state.NonTrump11Position(hand);
                if (nonTrump11 != 255)
                {
                    return nonTrump11;
                }

                //Put smallest
                return state.SmallestPosition(hand);
            }
            //coming second
            else
            {
                //Hit with same color
                byte higherSamePos = state.HigherSamePosition(hand);
                if(higherSamePos != 255)
                {
                    return higherSamePos;
                }

                //Hit any 10 or 11 with trump
                byte trumpBig = state.TrumpPosition(hand);
                if(trumpBig != 255)
                {
                    return trumpBig;
                }

                //Put smallest
                return state.SmallestPosition(hand);
            }
        }


    }
}
