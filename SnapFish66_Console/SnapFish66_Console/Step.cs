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
                Card[] places = new Card[] { state.a1, state.a2, state.a3, state.a4, state.a5};

                for (int i = 0; i < places.Length; i++)
                {
                    if(places[i]!= null && places[i].ID == cardID)
                    {
                        return true;
                    }
                }

                return false;
            }
            else
            {
                Card[] places = new Card[] { state.b1, state.b2, state.b3, state.b4, state.b5 };

                for (int i = 0; i < places.Length; i++)
                {
                    if (places[i] != null && places[i].ID == cardID)
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
                if ((state.adown.ID == "M3" && CardInHand(state, "A", "M4")) || (state.adown.ID == "M4" && CardInHand(state, "A", "M3")))
                {
                    state.AM20 = true;
                }
                if ((state.adown.ID == "P3" && CardInHand(state, "A", "P4")) || (state.adown.ID == "P4" && CardInHand(state, "A", "P3")))
                {
                    state.AP20 = true;
                }
                if ((state.adown.ID == "T3" && CardInHand(state, "A", "T4")) || (state.adown.ID == "T4" && CardInHand(state, "A", "T3")))
                {
                    state.AT20 = true;
                }
                if ((state.adown.ID == "Z3" && CardInHand(state, "A", "Z4")) || (state.adown.ID == "Z4" && CardInHand(state, "A", "Z3")))
                {
                    state.AZ20 = true;
                }

            }
            else
            {
                if ((state.bdown.ID == "M3" && CardInHand(state, "B", "M4")) || (state.bdown.ID == "M4" && CardInHand(state, "B", "M3")))
                {
                    state.BM20 = true;
                }
                if ((state.bdown.ID == "P3" && CardInHand(state, "B", "P4")) || (state.bdown.ID == "P4" && CardInHand(state, "B", "P3")))
                {
                    state.BP20 = true;
                }
                if ((state.bdown.ID == "T3" && CardInHand(state, "B", "T4")) || (state.bdown.ID == "T4" && CardInHand(state, "B", "T3")))
                {
                    state.BT20 = true;
                }
                if ((state.bdown.ID == "Z3" && CardInHand(state, "B", "Z4")) || (state.bdown.ID == "Z4" && CardInHand(state, "B", "Z3")))
                {
                    state.BZ20 = true;
                }
            }
        }

        //Drawing a card
        private void Draw(State state, string firstToDraw)
        {
            int place1 = -1;
            int place2 = -1;
            Card[] ahand = new Card[] { state.a1, state.a2, state.a3, state.a4, state.a5 };
            Card[] bhand = new Card[] { state.b1, state.b2, state.b3, state.b4, state.b5 };

            for (int i =0; i<ahand.Length; i++)
            {
                if(ahand[i]==null)
                {
                    if(firstToDraw=="A")
                    {
                        place1 = i;
                        break;
                    }
                    else
                    {
                        place2 = i;
                        break;
                    }
                }
            }

            for (int i = 0; i< bhand.Length; i++)
            {
                if (bhand[i]!= null)
                {
                    if (firstToDraw == "B")
                    {
                        place1 = i;
                        break;
                    }
                    else
                    {
                        place2 = i;
                        break;
                    }
                }
            }

            if(firstToDraw=="A")
            {
                if (state.deck.Count > 0)
                {
                    ahand[place1] = state.deck[0];
                    state.deck.RemoveAt(0);
                }
                else if (state.dbottom != null)
                {
                    ahand[place1] = state.dbottom;
                    state.dbottom = null;
                }

                if (state.deck.Count > 0)
                {
                    bhand[place2] = state.deck[0];
                    state.deck.RemoveAt(0);
                }
                else if (state.dbottom != null)
                {
                    bhand[place2] = state.dbottom;
                    state.dbottom = null;
                }
            }
            else
            {
                if (state.deck.Count > 0)
                {
                    bhand[place1] = state.deck[0];
                    state.deck.RemoveAt(0);
                }
                else if (state.dbottom != null)
                {
                    bhand[place1] = state.dbottom;
                    state.dbottom = null;
                }

                if (state.deck.Count > 0)
                {
                    ahand[place2] = state.deck[0];
                    state.deck.RemoveAt(0);
                }
                else if (state.dbottom != null)
                {
                    ahand[place2] = state.dbottom;
                    state.dbottom = null;
                }
            }
        }

        //Puts down a card
        private void PutDownCard(State state, Card from, Card to, string who, bool first)
        {
            //Put down
            to = new Card(from.ID);
            from = null;

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
                if(state.adown.color!=state.bdown.color && state.bdown.color!=state.trump)
                {
                    return "A";
                }
                // They have the same color, bigger value wins
                else if(state.adown.color==state.bdown.color)
                {
                    if(state.adown.value>state.bdown.value)
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
                if (state.adown.color != state.bdown.color && state.adown.color != state.trump)
                {
                    return "B";
                }
                // They have the same color, bigger value wins
                else if (state.adown.color == state.bdown.color)
                {
                    if (state.adown.value > state.bdown.value)
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
                state.atook.Add(state.adown);
                state.atook.Add(state.bdown);
                Draw(state, "A");
                state.next = "A";
            }
            else
            {
                state.btook.Add(state.adown);
                state.btook.Add(state.bdown);
                Draw(state, "B");
                state.next = "B";
            }

            state.adown = null;
            state.bdown = null;
        }

        //Perform a step (excluding covering)
        private bool PerformStep(State state, Card from)
        {
            if(from == null)
            {
                return false;
            }

            if (state.next == "A")
            {
                //B put down a card, a "answers" it
                if (state.bdown!=null)
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
                if (state.adown != null)
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
            if(state.dbottom != null && state.deck.Count>=3)
            {
                //Cannot switch if not coming first
                if(state.next == "A" && state.bdown != null)
                {
                    return;
                }
                if (state.next == "B" && state.adown != null)
                {
                    return;
                }
                
                //Try to switch, if has trump 2
                if(state.next =="A")
                {
                    Card[] hand = new Card[] { state.a1, state.a2, state.a3, state.a4, state.a5 };

                    for (int i = 0; i< hand.Length; i++)
                    {
                        if(hand[i] != null && hand[i].value==2 && hand[i].color == state.trump)
                        {
                            Card temp = new Card(hand[i].ID);
                            hand[i] = new Card(state.dbottom.ID);
                            state.dbottom = temp;
                        }
                    }
                }

                if (state.next == "B")
                {
                    Card[] hand = new Card[] { state.b1, state.b2, state.b3, state.b4, state.b5 };

                    for (int i = 0; i< hand.Length; i++)
                    {
                        if (hand[i] != null && hand[i].value == 2 && hand[i].color == state.trump)
                        {
                            Card temp = new Card(hand[i].ID);
                            hand[i] = new Card(state.dbottom.ID);
                            state.dbottom = temp;
                        }
                    }
                }
            }
        }

        //We have a trump
        private bool HasTrump(State state, Card opcard, Card[] hand)
        {
            foreach (Card card in hand)
            {
                if(card!= null && card.color==state.trump)
                {
                    return true;
                }
            }

            return false;
        }

        //We have card with the same color
        private bool HasSameColor(State state, Card opcard, Card[] hand)
        {
            foreach (Card card in hand)
            {
                if (card != null && card.color == opcard.color)
                {
                    return true;
                }
            }

            return false;
        }

        //We have bigger card in the same color
        private bool HasBiggerSame(State state, Card opcard, Card[] hand)
        {
            foreach (Card card in hand)
            {
                if (card!=null && card.color == opcard.color && card.value>opcard.value)
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
            Dictionary<string, Card> handDict = new Dictionary<string, Card>();
            Card opponentCard;
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

            Card[] hand;
            if (state.next == "A")
            {
                hand = new Card[] { state.a1, state.a2, state.a3, state.a4, state.a5 };
            }
            else
            {
                hand = new Card[] { state.b1, state.b2, state.b3, state.b4, state.b5 };
            }

            //If there are cards in the deck, it is not endgame
            if (state.deck.Count>0)
            {
                return true;
            }

            //If coming first, we can put anything
            if(opponentCard == null)
            {
                return true;
            }
            
            if(handDict.ContainsKey(action) && handDict[action] != null)
            {
                Card putCard = handDict[action];
                Card opCard = opponentCard;

                //If we have bigger of the same colour, then must put it down
                if (HasBiggerSame(state, opCard, hand))
                {
                    if (putCard.color == opCard.color && putCard.value > opCard.value)
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
