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

            if (state.adown != null && who=="A")
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
            else if(state.bdown!=null)
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

        //Helper for Draw() and TrySwitchBottom(), sets the i-th card of hand
        private void SetStateCard(State state, string who, int i, Card card)
        {
            if(who == "A")
            {
                if(i==0)
                {
                    state.a1 = card;
                }
                else if(i==1)
                {
                    state.a2 = card;
                }
                else if (i == 2)
                {
                    state.a3 = card;
                }
                else if (i == 3)
                {
                    state.a4 = card;
                }
                else if (i == 4)
                {
                    state.a5 = card;
                }
            }
            else
            {
                if (i == 0)
                {
                    state.b1 = card;
                }
                else if (i == 1)
                {
                    state.b2 = card;
                }
                else if (i == 2)
                {
                    state.b3 = card;
                }
                else if (i == 3)
                {
                    state.b4 = card;
                }
                else if (i == 4)
                {
                    state.b5 = card;
                }
            }
        }

        //Drawing a card
        private void Draw(State state, string firstToDraw)
        {
            int place1 = -1; //Index of first drawn card
            int place2 = -1; //Index of second drawn card
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
                if (bhand[i]== null)
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
                    SetStateCard(state, "A", place1, state.deck[0]);
                    state.deck.RemoveAt(0);
                }
                else if (state.dbottom != null)
                {
                    SetStateCard(state, "A", place1, state.dbottom);
                    state.dbottom = null;
                }

                if (state.deck.Count > 0)
                {
                    SetStateCard(state, "B", place2, state.deck[0]);
                    state.deck.RemoveAt(0);
                }
                else if (state.dbottom != null)
                {
                    SetStateCard(state, "B", place2, state.dbottom);
                    state.dbottom = null;
                }
            }
            else
            {
                if (state.deck.Count > 0)
                {
                    SetStateCard(state, "B", place1, state.deck[0]);
                    state.deck.RemoveAt(0);
                }
                else if (state.dbottom != null)
                {
                    SetStateCard(state, "B", place1, state.dbottom);
                    state.dbottom = null;
                }

                if (state.deck.Count > 0)
                {
                    SetStateCard(state, "A", place2, state.deck[0]);
                    state.deck.RemoveAt(0);
                }
                else if (state.dbottom != null)
                {
                    SetStateCard(state, "A", place2, state.dbottom);
                    state.dbottom = null;
                }
            }
        }

        //Puts down a card
        private void PutDownCard(State state, ref Card from, ref Card to, string who, bool first)
        {
            //Put down
            to = Card.GetCard(from.ID);
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
                state.isAnext = true;
            }
            else
            {
                state.btook.Add(state.adown);
                state.btook.Add(state.bdown);
                Draw(state, "B");
                state.isAnext = false;
            }

            state.adown = null;
            state.bdown = null;
        }

        //Perform a step (excluding covering)
        private bool PerformStep(State state, ref Card from)
        {
            if(from == null)
            {
                return false;
            }

            if (state.isAnext)
            {
                //B put down a card, a "answers" it
                if (state.bdown!=null)
                {
                    PutDownCard(state, ref from, ref state.adown, "A",false);
                    HitAndTake(state,"B");

                }
                //A starts the round
                else
                {
                    PutDownCard(state, ref from, ref state.adown, "A",true);
                    state.isAnext = false;
                }
            }
            else
            {
                //A put down a card, b "answers" it
                if (state.adown != null)
                {
                    PutDownCard(state, ref from, ref state.bdown, "B",false);
                    HitAndTake(state,"A");

                }
                //B starts the round
                else
                {
                    PutDownCard(state, ref from, ref state.bdown, "B",true);
                    state.isAnext = true;
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
                if(state.isAnext && state.bdown != null)
                {
                    return;
                }
                if (!state.isAnext && state.adown != null)
                {
                    return;
                }
                
                //Try to switch, if has trump 2
                if(state.isAnext)
                {
                    Card[] hand = new Card[] { state.a1, state.a2, state.a3, state.a4, state.a5 };

                    for (int i = 0; i< hand.Length; i++)
                    {
                        if(hand[i] != null && hand[i].value==2 && hand[i].color == state.trump)
                        {
                            Card temp = Card.GetCard(hand[i].ID);
                            SetStateCard(state, "A", i, state.dbottom);
                            state.dbottom = temp;
                        }
                    }
                }

                if (!state.isAnext)
                {
                    Card[] hand = new Card[] { state.b1, state.b2, state.b3, state.b4, state.b5 };

                    for (int i = 0; i< hand.Length; i++)
                    {
                        if (hand[i] != null && hand[i].value == 2 && hand[i].color == state.trump)
                        {
                            Card temp = Card.GetCard(hand[i].ID);
                            SetStateCard(state, "B", i, state.dbottom);
                            state.dbottom = temp;
                        }
                    }
                }
            }
        }

        //We have a trump
        private bool HasTrump(State state, Card[] hand)
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
        private bool EndgameOK(State state, byte action)
        {
            //If there are cards in the deck, it is not endgame
            if (state.deck.Count > 0)
            {
                return true;
            }

            //If coming first, we can put anything
            Card opponentCard = state.isAnext ? state.bdown : state.adown;
            if (opponentCard == null)
            {
                return true;
            }

            Card[] hand;
            if (state.isAnext)
            {
                hand = new Card[] { state.a1, state.a2, state.a3, state.a4, state.a5 };
            }
            else
            {
                hand = new Card[] { state.b1, state.b2, state.b3, state.b4, state.b5 };
            }
            
            if(hand[action] != null)
            {
                Card putCard = hand[action];
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
                if (HasTrump(state, hand))
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
        public bool Do(State state, byte action)
        {
            TrySwitchDBottom(state);

            if(!EndgameOK(state, action))
            {
                return false;
            }
            
            if (action == 0)
            {
                if (state.isAnext)
                {
                    return PerformStep(state, ref state.a1);
                }
                else
                {
                    return PerformStep(state, ref state.b1);
                }
            }
            if (action == 1)
            {
                if (state.isAnext)
                {
                    return PerformStep(state, ref state.a2);
                }
                else
                {
                    return PerformStep(state, ref state.b2);
                }
            }
            if (action == 2)
            {
                if (state.isAnext)
                {
                    return PerformStep(state, ref state.a3);
                }
                else
                {
                    return PerformStep(state, ref state.b3);
                }
            }
            if (action == 3)
            {
                if (state.isAnext)
                {
                    return PerformStep(state, ref state.a4);
                }
                else
                {
                    return PerformStep(state, ref state.b4);
                }
            }
            if (action == 4)
            {
                if (state.isAnext)
                {
                    return PerformStep(state, ref state.a5);
                }
                else
                {
                    return PerformStep(state, ref state.b5);
                }
            }
            return false;
        }

    }
}
