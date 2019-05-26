using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapFish66
{
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
            //Put down
            to.Clear();
            to.Add(from[0]);

            //Before pulling
            Saying20(state, who); 

            //Pull
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

        //Calculates who takes the 2 cards
        private string Winner(State state, string first)
        {
            if(first == "A")
            {
                // B neither put the same color, nor trump
                if(state.adown[0].color!=state.bdown[0].color && state.bdown[0].color!=state.trump)
                {
                    return "A";
                }
                // They have the same color, bigger value wins
                else if(state.adown[0].color==state.bdown[0].color)
                {
                    if(state.adown[0].GetValue()>state.bdown[0].GetValue())
                    {
                        return "A";
                    }
                    else
                    {
                        return "B";
                    }
                }
                //B put trump, A did not
                else
                {
                    return "B";
                }
            }
            //B came first
            else
            {
                // A neither put the same color, nor trump
                if (state.adown[0].color != state.bdown[0].color && state.adown[0].color != state.trump)
                {
                    return "B";
                }
                // They have the same color, bigger value wins
                else if (state.adown[0].color == state.bdown[0].color)
                {
                    if (state.adown[0].GetValue() > state.bdown[0].GetValue())
                    {
                        return "A";
                    }
                    else
                    {
                        return "B";
                    }
                }
                //A put trump, B did not
                else
                {
                    return "A";
                }
            }
        }

        //Compares the two cards down, moves them to Atook or Btook, and sets next
        private void HitAndTake(State state, string first)
        {
            bool awon = false;

            if(Winner(state,first)=="A")
            {
                awon = true;
            }


            if(awon)
            {
                state.atook.Add(state.adown[0]);
                state.atook.Add(state.bdown[0]);
                state.next = "A";
            }
            else
            {
                state.btook.Add(state.adown[0]);
                state.btook.Add(state.bdown[0]);
                state.next = "B";
            }

            state.adown.Clear();
            state.bdown.Clear();
        }

        //Perform a step (excluding covering)
        private bool PerformStep(State state, List<Card> from)
        {
            if(from.Count==0)
            {
                return false;
            }

            if (state.next == "A")
            {
                //B put down a card, a "answers" it
                if (state.bdown.Count > 0)
                {
                    PutDownCard(state, from, state.adown, "A");
                    HitAndTake(state,"B");

                }
                //A starts the round
                else
                {
                    PutDownCard(state, from, state.adown, "A");
                }

                state.next = "B";
            }
            else
            {
                //A put down a card, b "answers" it
                if (state.adown.Count > 0)
                {
                    PutDownCard(state, from, state.bdown, "B");
                    HitAndTake(state,"A");

                }
                //B starts the round
                else
                {
                    PutDownCard(state, from, state.bdown, "B");
                }

                state.next = "B";
            }

            return true;
        }

        //Modifies the given state by the given action, returns wheteher the step is valid
        public bool Do(State state, string action)
        {
            if (action == "A1")
            {
                if (state.next == "A")
                {
                    return PerformStep(state, state.a1);
                }
                else
                {
                    return false;
                }
            }
            if (action == "A2")
            {
                if (state.next == "A")
                {
                    return PerformStep(state, state.a2);
                }
                else
                {
                    return false;
                }
            }
            if (action == "A3")
            {
                if (state.next == "A")
                {
                    return PerformStep(state, state.a3);
                }
                else
                {
                    return false;
                }
            }
            if (action == "A4")
            {
                if (state.next == "A")
                {
                    return PerformStep(state, state.a4);
                }
                else
                {
                    return false;
                }
            }
            if (action == "A5")
            {
                if (state.next == "A")
                {
                    return PerformStep(state, state.a5);
                }
                else
                {
                    return false;
                }
            }
            if (action == "B1")
            {
                if(state.next=="A")
                {
                    return false;
                }
                else
                {
                    return PerformStep(state, state.b1);
                }
            }
            if (action == "B2")
            {
                if (state.next == "A")
                {
                    return false;
                }
                else
                {
                    return PerformStep(state, state.b2);
                }
            }
            if (action == "B3")
            {
                if (state.next == "A")
                {
                    return false;
                }
                else
                {
                    return PerformStep(state, state.b3);
                }
            }
            if (action == "B4")
            {
                if (state.next == "A")
                {
                    return false;
                }
                else
                {
                    return PerformStep(state, state.b4);
                }
            }
            if (action == "B5")
            {
                if (state.next == "A")
                {
                    return false;
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
