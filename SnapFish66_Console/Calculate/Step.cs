﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculate
{
    static class Step
    {
        //Whether the player has the card in hand
        private static bool CardInHand(State state, string who, string cardID)
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
        private static void Saying20(State state, string who)
        {

            if (state.adown != null && who=="A")
            {
                if ((state.adown.ID == "M3" && CardInHand(state, "A", "M4")) || (state.adown.ID == "M4" && CardInHand(state, "A", "M3")))
                {
                    state.AM20 = true;
                    if(state.trump == 'M')
                    {
                        state.AscoreMarriages += 40;
                    }
                    else
                    {
                        state.AscoreMarriages += 20;
                    }
                }
                if ((state.adown.ID == "P3" && CardInHand(state, "A", "P4")) || (state.adown.ID == "P4" && CardInHand(state, "A", "P3")))
                {
                    state.AP20 = true;
                    if (state.trump == 'P')
                    {
                        state.AscoreMarriages += 40;
                    }
                    else
                    {
                        state.AscoreMarriages += 20;
                    }
                }
                if ((state.adown.ID == "T3" && CardInHand(state, "A", "T4")) || (state.adown.ID == "T4" && CardInHand(state, "A", "T3")))
                {
                    state.AT20 = true;
                    if (state.trump == 'T')
                    {
                        state.AscoreMarriages += 40;
                    }
                    else
                    {
                        state.AscoreMarriages += 20;
                    }
                }
                if ((state.adown.ID == "Z3" && CardInHand(state, "A", "Z4")) || (state.adown.ID == "Z4" && CardInHand(state, "A", "Z3")))
                {
                    state.AZ20 = true;
                    if (state.trump == 'Z')
                    {
                        state.AscoreMarriages += 40;
                    }
                    else
                    {
                        state.AscoreMarriages += 20;
                    }
                }

            }
            else if(state.bdown!=null)
            {
                if ((state.bdown.ID == "M3" && CardInHand(state, "B", "M4")) || (state.bdown.ID == "M4" && CardInHand(state, "B", "M3")))
                {
                    state.BM20 = true;
                    if (state.trump == 'M')
                    {
                        state.BscoreMarriages += 40;
                    }
                    else
                    {
                        state.BscoreMarriages += 20;
                    }
                }
                if ((state.bdown.ID == "P3" && CardInHand(state, "B", "P4")) || (state.bdown.ID == "P4" && CardInHand(state, "B", "P3")))
                {
                    state.BP20 = true;
                    if (state.trump == 'P')
                    {
                        state.BscoreMarriages += 40;
                    }
                    else
                    {
                        state.BscoreMarriages += 20;
                    }
                }
                if ((state.bdown.ID == "T3" && CardInHand(state, "B", "T4")) || (state.bdown.ID == "T4" && CardInHand(state, "B", "T3")))
                {
                    state.BT20 = true;
                    if (state.trump == 'T')
                    {
                        state.BscoreMarriages += 40;
                    }
                    else
                    {
                        state.BscoreMarriages += 20;
                    }
                }
                if ((state.bdown.ID == "Z3" && CardInHand(state, "B", "Z4")) || (state.bdown.ID == "Z4" && CardInHand(state, "B", "Z3")))
                {
                    state.BZ20 = true;
                    if (state.trump == 'Z')
                    {
                        state.BscoreMarriages += 40;
                    }
                    else
                    {
                        state.BscoreMarriages += 20;
                    }
                }
            }
        }

        //Helper for Draw() and TrySwitchBottom(), sets the i-th card of hand
        private static void SetStateCard(State state, string who, int i, Card card)
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
        private static void Draw(State state, string firstToDraw)
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
                if (state.deck > 0)
                {
                    SetStateCard(state, "A", place1, Card.PopCardFromDeck(ref state.deck));
                }
                else if (state.dbottom != null)
                {
                    SetStateCard(state, "A", place1, state.dbottom);
                    state.dbottom = null;
                }

                if (state.deck > 0)
                {
                    SetStateCard(state, "B", place2, Card.PopCardFromDeck(ref state.deck));
                }
                else if (state.dbottom != null)
                {
                    SetStateCard(state, "B", place2, state.dbottom);
                    state.dbottom = null;
                }
            }
            else
            {
                if (state.deck > 0)
                {
                    SetStateCard(state, "B", place1, Card.PopCardFromDeck(ref state.deck));
                }
                else if (state.dbottom != null)
                {
                    SetStateCard(state, "B", place1, state.dbottom);
                    state.dbottom = null;
                }

                if (state.deck > 0)
                {
                    SetStateCard(state, "A", place2, Card.PopCardFromDeck(ref state.deck));
                }
                else if (state.dbottom != null)
                {
                    SetStateCard(state, "A", place2, state.dbottom);
                    state.dbottom = null;
                }
            }
        }

        //Puts down a card
        private static void PutDownCard(State state, ref Card from, ref Card to, string who, bool first)
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
        private static string Winner(State state, string first)
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
        private static void HitAndTake(State state, string first)
        {
            bool awon = false;

            if(Winner(state,first)=="A")
            {
                awon = true;
            }

            if(awon)
            {
                state.AscoreBasic += state.adown.value;
                state.AscoreBasic += state.bdown.value;

                state.atook += state.adown.cardSetIndex;
                state.atook += state.bdown.cardSetIndex;
                state.atookCount += 2;
                Draw(state, "A");
                state.isAnext = true;
            }
            else
            {
                state.BscoreBasic += state.adown.value;
                state.BscoreBasic += state.bdown.value;

                state.btook += state.adown.cardSetIndex;
                state.btook += state.bdown.cardSetIndex;
                state.btookCount += 2;
                Draw(state, "B");
                state.isAnext = false;
            }

            state.adown = null;
            state.bdown = null;
        }

        //Perform a step (excluding covering)
        private static bool PerformStep(State state, ref Card from)
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
        private static void TrySwitchDBottom(State state)
        {
            int deck3 = 22 * 22 * 22;
            if(state.dbottom != null && state.deck / deck3 > 0) //Deck has more than 3 cards
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
        private static bool HasTrump(State state, Card[] hand)
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
        private static bool HasSameColor(Card opcard, Card[] hand)
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
        private static bool HasBiggerSame(Card opcard, Card[] hand)
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
        private static bool EndgameOK(State state, byte action)
        {
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
                if (HasBiggerSame(opCard, hand))
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
                if (HasSameColor(opCard, hand))
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
        public static bool Do(State state, byte action)
        {
            TrySwitchDBottom(state);

            if(state.deck == 0 && !EndgameOK(state, action))
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
