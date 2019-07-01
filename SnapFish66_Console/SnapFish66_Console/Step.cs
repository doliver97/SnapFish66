using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapFish66_Console
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

        //Drawing a card
        private void Draw(State state, string firstToDraw)
        {
            List<Card> place1 = new List<Card>();
            List<Card> place2 = new List<Card>();
            List<List<Card>> ahand = new List<List<Card>> { state.a1, state.a2, state.a3, state.a4, state.a5 };
            List<List<Card>> bhand = new List<List<Card>> { state.b1, state.b2, state.b3, state.b4, state.b5 };

            foreach (List<Card> place in ahand)
            {
                if(place.Count==0)
                {
                    if(firstToDraw=="A")
                    {
                        place1 = place;
                        break;
                    }
                    else
                    {
                        place2 = place;
                        break;
                    }
                }
            }

            foreach (List<Card> place in bhand)
            {
                if (place.Count == 0)
                {
                    if (firstToDraw == "B")
                    {
                        place1 = place;
                        break;
                    }
                    else
                    {
                        place2 = place;
                        break;
                    }
                }
            }


            if (state.deck.Count > 0)
            {
                place1.Add(state.deck[0]);
                state.deck.RemoveAt(0);
            }
            else if (state.dbottom.Count > 0)
            {
                place1.Add(state.dbottom[0]);
                state.dbottom.RemoveAt(0);
            }

            if (state.deck.Count > 0)
            {
                place2.Add(state.deck[0]);
                state.deck.RemoveAt(0);
            }
            else if (state.dbottom.Count > 0)
            {
                place2.Add(state.dbottom[0]);
                state.dbottom.RemoveAt(0);
            }
        }

        //Puts down a card
        private void PutDownCard(State state, List<Card> from, List<Card> to, string who, bool first)
        {
            //Put down
            to.Clear();
            to.Add(from[0]);
            from.Clear();

            //Before drawing cards
            if (first)
            { 
                Saying20(state, who);
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

        //Compares the two cards down, moves them to Atook or Btook, draws cards, and sets next
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
                Draw(state, "A");
                state.next = "A";
            }
            else
            {
                state.btook.Add(state.adown[0]);
                state.btook.Add(state.bdown[0]);
                Draw(state, "B");
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
                    PutDownCard(state, from, state.adown, "A",false);
                    HitAndTake(state,"B");

                }
                //A starts the round
                else
                {
                    PutDownCard(state, from, state.adown, "A",true);
                    state.next = "B";
                }
            }
            else
            {
                //A put down a card, b "answers" it
                if (state.adown.Count > 0)
                {
                    PutDownCard(state, from, state.bdown, "B",false);
                    HitAndTake(state,"A");

                }
                //B starts the round
                else
                {
                    PutDownCard(state, from, state.bdown, "B",true);
                    state.next = "A";
                }
            }

            return true;
        }

        //Tries to switch the dbottom with card in hand
        private void TrySwitchDBottom(State state)
        {
            if(state.dbottom.Count>0 && state.deck.Count>=3)
            {
                //Cannot switch if not coming first
                if(state.next == "A" && state.bdown.Count>0)
                {
                    return;
                }
                if (state.next == "B" && state.adown.Count > 0)
                {
                    return;
                }
                
                //Try to switch, if has trump 2
                if(state.next =="A")
                {
                    List<List<Card>> hand = new List<List<Card>> { state.a1, state.a2, state.a3, state.a4, state.a5 };

                    foreach (var position in hand)
                    {
                        if(position.Count>0 && position[0].GetValue()==2 && position[0].color == state.trump)
                        {
                            Card temp = position[0];
                            position[0] = state.dbottom[0];
                            state.dbottom[0] = temp;
                        }
                    }
                }

                if (state.next == "B")
                {
                    List<List<Card>> hand = new List<List<Card>> { state.b1, state.b2, state.b3, state.b4, state.b5 };

                    foreach (var position in hand)
                    {
                        if (position.Count > 0 && position[0].GetValue() == 2 && position[0].color == state.trump)
                        {
                            Card temp = position[0];
                            position[0] = state.dbottom[0];
                            state.dbottom[0] = temp;
                        }
                    }
                }
            }
        }

        //We have a trump
        private bool HasTrump(State state, Card opcard, List<List<Card>> hand)
        {
            foreach (List<Card> card in hand)
            {
                if(card.Count>0 && card[0].color==state.trump)
                {
                    return true;
                }
            }

            return false;
        }

        //We have card with the same color
        private bool HasSameColor(State state, Card opcard, List<List<Card>> hand)
        {
            foreach (List<Card> card in hand)
            {
                if (card.Count>0 && card[0].color == opcard.color)
                {
                    return true;
                }
            }

            return false;
        }

        //We have bigger card in the same color
        private bool HasBiggerSame(State state, Card opcard, List<List<Card>> hand)
        {
            foreach (List<Card> card in hand)
            {
                if (card.Count>0 && card[0].color == opcard.color && card[0].GetValue()>opcard.GetValue())
                {
                    return true;
                }
            }

            return false;
        }

        //Endgame (same color and hitting is obligatory)
        private bool EndgameOK(State state, string action)
        {
            //Helpers
            Dictionary<string, List<Card>> handDict = new Dictionary<string, List<Card>>();
            List<Card> opponentCard = new List<Card>();
            if(state.next == "A")
            {
                opponentCard = state.bdown;
                handDict.Add("A1", state.a1);
                handDict.Add("A2", state.a2);
                handDict.Add("A3", state.a3);
                handDict.Add("A4", state.a4);
                handDict.Add("A5", state.a5);
            }
            else
            {
                opponentCard = state.adown;
                handDict.Add("B1", state.b1);
                handDict.Add("B2", state.b2);
                handDict.Add("B3", state.b3);
                handDict.Add("B4", state.b4);
                handDict.Add("B5", state.b5);
            }

            List<List<Card>> hand;
            if (state.next == "A")
            {
                hand = new List<List<Card>> { state.a1, state.a2, state.a3, state.a4, state.a5 };
            }
            else
            {
                hand = new List<List<Card>> { state.b1, state.b2, state.b3, state.b4, state.b5 };
            }

            //If there are cards in the deck, it is not endgame
            if (state.deck.Count>0)
            {
                return true;
            }

            //If coming first, we can put anything
            if(opponentCard.Count==0)
            {
                return true;
            }
            
            if(handDict.ContainsKey(action) && handDict[action].Count > 0)
            {
                Card putCard = handDict[action][0];
                Card opCard = opponentCard[0];

                //If we have bigger of the same colour, then must put it down
                if (HasBiggerSame(state, opCard, hand))
                {
                    if (putCard.color == opCard.color && putCard.GetValue() > opCard.GetValue())
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }


                //Else if we have same color, we must put it down
                if (HasSameColor(state, opCard, hand))
                {
                    if (putCard.color == opCard.color)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

                //Else if we have a trump, we must put it down
                if (HasTrump(state, opCard, hand))
                {
                    if(putCard.color == state.trump)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

                //If dont have neither the same color, nor trump, we can put anything
                return true;
            }
            //Wrong action
            else
            {
                return false;
            }

        }

        //Modifies the given state by the given action, returns whether the step is valid
        public bool Do(State state, string action)
        {
            TrySwitchDBottom(state);

            if(!EndgameOK(state, action))
            {
                return false;
            }

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
