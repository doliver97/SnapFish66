using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapFish66
{
    //BUG: if a slot has an "empty" card, it conunts as card -> remove empties before calculating states
    class Step
    {
        //Whether the player has the card in hand
        private bool CardInHand(State state, string who, string cardID)
        {
            if(who == "A")
            {
                List<Card>[] places = new List<Card>[] { state.a1, state.a2, state.a3, state.a4, state.a5};

                for (int i = 0; i < places.Length; i++)
                {
                    if(places[i].Count>0 && places[i][0].ID == cardID)
                    {
                        return true;
                    }
                }

                return false;
            }
            else
            {
                List<Card>[] places = new List<Card>[] { state.b1, state.b2, state.b3, state.b4, state.b5 };

                for (int i = 0; i < places.Length; i++)
                {
                    if (places[i].Count > 0 && places[i][0].ID == cardID)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        //Count 20 or 40 automatically
        private void Saying20(State state, string who)
        {

            if (who=="A")
            {
                if ((state.adown[0].ID == "M3" && CardInHand(state, "A", "M4")) || (state.adown[0].ID == "M4" && CardInHand(state, "A", "M3")))
                {
                    state.AM20 = true;
                }
                if ((state.adown[0].ID == "P3" && CardInHand(state, "A", "P4")) || (state.adown[0].ID == "P4" && CardInHand(state, "A", "P3")))
                {
                    state.AP20 = true;
                }
                if ((state.adown[0].ID == "T3" && CardInHand(state, "A", "T4")) || (state.adown[0].ID == "T4" && CardInHand(state, "A", "T3")))
                {
                    state.AT20 = true;
                }
                if ((state.adown[0].ID == "Z3" && CardInHand(state, "A", "Z4")) || (state.adown[0].ID == "Z4" && CardInHand(state, "A", "Z3")))
                {
                    state.AZ20 = true;
                }

            }
            else
            {
                if ((state.bdown[0].ID == "M3" && CardInHand(state, "B", "M4")) || (state.bdown[0].ID == "M4" && CardInHand(state, "B", "M3")))
                {
                    state.BM20 = true;
                }
                if ((state.bdown[0].ID == "P3" && CardInHand(state, "B", "P4")) || (state.bdown[0].ID == "P4" && CardInHand(state, "B", "P3")))
                {
                    state.BP20 = true;
                }
                if ((state.bdown[0].ID == "T3" && CardInHand(state, "B", "T4")) || (state.bdown[0].ID == "T4" && CardInHand(state, "B", "T3")))
                {
                    state.BT20 = true;
                }
                if ((state.bdown[0].ID == "Z3" && CardInHand(state, "B", "Z4")) || (state.bdown[0].ID == "Z4" && CardInHand(state, "B", "Z3")))
                {
                    state.BZ20 = true;
                }
            }
        }

        //Puts down a card, and pulls if possible
        private void PutDownCard(State state, List<Card> from, List<Card> to, string who)
        {
            to = new List<Card>();
            to.Add(from[0]);

            //Before pulling
            Saying20(state, who); 

            if (state.deck.Count > 0)
            {
                from[0] = state.deck[0];
                state.deck.RemoveAt(0);
            }
            else if (state.dbottom.Count > 0)
            {
                from[0] = state.dbottom[0];
                state.dbottom.RemoveAt(0);
            }
            else
            {
                from.RemoveAt(0);
            }

            
        }

        //Compares the two cards down, moves them to Atook or Btook, and sets next
        private void HitAndTake(State state)
        {
            //TODO
        }

        //Perform a step (excluding covering)
        private bool PerformStep(State state, List<Card> from)
        {
            if (state.next == "A")
            {
                //A has no card in that slot
                if (state.adown.Count == 0)
                {
                    return false;
                }

                //B put down a card, a "answers" it
                if (state.bdown.Count > 0)
                {
                    PutDownCard(state, from, state.adown, "A");
                    HitAndTake(state);

                }
                //A starts the round
                else
                {
                    PutDownCard(state, from, state.adown, "A");
                }
            }
            else
            {
                //B has no card in that slot
                if (state.bdown.Count == 0)
                {
                    return false;
                }

                //A put down a card, b "answers" it
                if (state.adown.Count > 0)
                {
                    PutDownCard(state, from, state.bdown, "B");
                    HitAndTake(state);

                }
                //B starts the round
                else
                {
                    PutDownCard(state, from, state.bdown, "B");
                }
            }

            return true;
        }

        //Modifies the given state by the given action, returns wheteher the step is valid
        public bool Do(State state, string action) //1,2,3,4,5,cover
        {
            if(action == "1")
            {
                if(state.next=="A")
                {
                    return PerformStep(state, state.a1);
                }
                else
                {
                    return PerformStep(state, state.b1);
                }
            }
            if (action == "2")
            {
                if (state.next == "A")
                {
                    return PerformStep(state, state.a2);
                }
                else
                {
                    return PerformStep(state, state.b2);
                }
            }
            if (action == "3")
            {
                if (state.next == "A")
                {
                    return PerformStep(state, state.a3);
                }
                else
                {
                    return PerformStep(state, state.b3);
                }
            }
            if (action == "4")
            {
                if (state.next == "A")
                {
                    return PerformStep(state, state.a4);
                }
                else
                {
                    return PerformStep(state, state.b4);
                }
            }
            if (action == "5")
            {
                if (state.next == "A")
                {
                    return PerformStep(state, state.a5);
                }
                else
                {
                    return PerformStep(state, state.b5);
                }
            }
            if(action == "cover")
            {
                if(state.covered==true || state.deck.Count<=2)
                {
                    return false;
                }
                else
                {
                    state.covered = true;
                    return true;
                }
            }

            return false;
        }

    }
}
