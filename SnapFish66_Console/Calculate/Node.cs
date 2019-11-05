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
        private static readonly int maxTTableDepth = 10;
        private static readonly int maxDBDepth = 0;
        private static readonly int mostLikelyTreshold = 10;

        public static List<string> IDs = new List<string> { "M2", "M3", "M4", "M10", "M11", "P2", "P3", "P4", "P10", "P11", "T2", "T3", "T4", "T10", "T11", "Z2", "Z3", "Z4", "Z10", "Z11" };

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

            //GameTree.VisitedNodes[depth]++;

            //If found in database, we can cut it here
            if (Calculator.AllowReadDatabase && depth <= maxDBDepth)
            {
                sbyte val = (sbyte)GameTree.database.ReadFromDB(StringifyNode(oAlpha, oBeta));
                if (val != -100) // -100 means not found
                {
                    //GameTree.ReadNodes[depth]++;
                    return val;
                }
            }

            sbyte retVal = 0;
            if(depth < maxTTableDepth)
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
            if (Calculator.AllowWriteDatabase)
            {
                if (depth <= maxDBDepth)
                {
                    sbyte value = maximizer ? alpha : beta;
                    GameTree.database.AddToDB(StringifyNode(oAlpha, oBeta), value);
                    GameTree.SavedNodes[depth]++;
                }
            }

            if (depth < maxTTableDepth && GC.GetTotalMemory(false) < 1000000000)
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

        //Helper for stringify
        private void SetChar(ref char[] key, Card place, char val, int i)
        {
            if (place != null && place.ID == IDs[i])
            {
                key[i + 2] = val;
            }
        }

        private string StringifyNode(sbyte alpha, sbyte beta)
        {
            char[] key = new char[28];

            key[0] = state.trump;
            key[1] = state.isAnext ? 'A' : 'B';

            //for each card
            for (int i = 0; i < IDs.Count; i++)
            {
                //deck
                long deck = state.deck;
                int j = 0;
                while (deck > 0)
                {
                    if (Card.PopCardFromDeck(ref deck).ID == IDs[i])
                    {
                        key[i + 2] = Convert.ToChar(j + '0');
                    }

                    j++;
                }

                //dbottom
                SetChar(ref key, state.dbottom, 'D', i);

                //hands
                SetChar(ref key, state.a1, 'A', i);
                SetChar(ref key, state.a2, 'A', i);
                SetChar(ref key, state.a3, 'A', i);
                SetChar(ref key, state.a4, 'A', i);
                SetChar(ref key, state.a5, 'A', i);
                SetChar(ref key, state.b1, 'B', i);
                SetChar(ref key, state.b2, 'B', i);
                SetChar(ref key, state.b3, 'B', i);
                SetChar(ref key, state.b4, 'B', i);
                SetChar(ref key, state.b5, 'B', i);

                //put down
                SetChar(ref key, state.adown, 'E', i);
                SetChar(ref key, state.bdown, 'F', i);

                //taken
                if (state.atook.HasFlag((State.CardSet)Card.GetCard(IDs[i]).cardSetIndex))
                {
                    key[i + 2] = 'G';
                }
                if (state.btook.HasFlag((State.CardSet)Card.GetCard(IDs[i]).cardSetIndex))
                {
                    key[i + 2] = 'H';
                }
            }

            //20/40
            if (state.AM20)
            {
                key[22] = 'A';
            }
            else if (state.BM20)
            {
                key[22] = 'B';
            }
            else
            {
                key[22] = 'X';
            }

            if (state.AP20)
            {
                key[23] = 'A';
            }
            else if (state.BP20)
            {
                key[23] = 'B';
            }
            else
            {
                key[23] = 'X';
            }

            if (state.AT20)
            {
                key[24] = 'A';
            }
            else if (state.BT20)
            {
                key[24] = 'B';
            }
            else
            {
                key[24] = 'X';
            }

            if (state.AZ20)
            {
                key[25] = 'A';
            }
            else if (state.BZ20)
            {
                key[25] = 'B';
            }
            else
            {
                key[25] = 'X';
            }

            key[26] = Convert.ToChar('0' + (alpha + 3));
            key[27] = Convert.ToChar('0' + (beta + 3));

            return new string(key);
        }

    }
}
