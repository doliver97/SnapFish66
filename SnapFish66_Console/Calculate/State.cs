using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculate
{
    public class State : IEquatable<State>
    {
        private static readonly Random rand = new Random();

        //This enum represents the card list
        public enum CardSet : long
        {
            NONE = 0b00000000_00000000_00000000_00000000,
            M2 =  0b00000000_00000000_00000000_00000001,
            M3 =  0b00000000_00000000_00000000_00000010,
            M4 =  0b00000000_00000000_00000000_00000100,
            M10 = 0b00000000_00000000_00000000_00001000,
            M11 = 0b00000000_00000000_00000000_00010000,
            P2 =  0b00000000_00000000_00000000_00100000,
            P3 =  0b00000000_00000000_00000000_01000000,
            P4 =  0b00000000_00000000_00000000_10000000,
            P10 = 0b00000000_00000000_00000001_00000000,
            P11 = 0b00000000_00000000_00000010_00000000,
            T2 =  0b00000000_00000000_00000100_00000000,
            T3 =  0b00000000_00000000_00001000_00000000,
            T4 =  0b00000000_00000000_00010000_00000000,
            T10 = 0b00000000_00000000_00100000_00000000,
            T11 = 0b00000000_00000000_01000000_00000000,
            Z2 =  0b00000000_00000000_10000000_00000000,
            Z3 =  0b00000000_00000001_00000000_00000000,
            Z4 =  0b00000000_00000010_00000000_00000000,
            Z10 = 0b00000000_00000100_00000000_00000000,
            Z11 = 0b00000000_00001000_00000000_00000000
        };

        //This could be a CardSet
        public long deck;
        public Card dbottom;
        public Card a1;
        public Card a2;
        public Card a3;
        public Card a4;
        public Card a5;
        public Card b1;
        public Card b2;
        public Card b3;
        public Card b4;
        public Card b5;
        public CardSet atook = CardSet.NONE;
        public CardSet btook = CardSet.NONE;
        public Card adown;
        public Card bdown;

        public CardSet aHand;
        public CardSet bHand;

        public int atookCount = 0;
        public int btookCount = 0;

        public bool AM20;
        public bool AP20;
        public bool AT20;
        public bool AZ20;
        public bool BM20;
        public bool BP20;
        public bool BT20;
        public bool BZ20;

        public bool covered = false;
        
        public bool isAnext = false;

        public char trump = '-';

        //Collect 66
        public byte AscoreBasic = 0;
        public byte BscoreBasic = 0;
        public byte AscoreMarriages = 0;
        public byte BscoreMarriages = 0;
        public byte AscoreFull = 0;
        public byte BscoreFull = 0;

        //Points: {0-3}
        public sbyte Apoints = 0;
        public sbyte Bpoints = 0;

        public bool isEnd;
        
        private List<Card> GetUnknownCards()
        {
            List<Card> cards = new List<Card>();
            foreach (var c in Calculator.cards)
            {
                cards.Add(c);
            }

            foreach(Card c in Calculator.cards)
            {
                CardSet[] MultiCardPlaces = new CardSet[] {atook, btook};

                for (int i = 0; i < MultiCardPlaces.Length; i++)
                {
                    if(MultiCardPlaces[i].HasFlag((CardSet)c.cardSetIndex))
                    {
                        cards.RemoveAll(x => x.ID == c.ID);
                    }
                }

                Card[] SimpleCardPlaces = { dbottom, adown, bdown, a1, a2, a3, a4, a5, b1, b2, b3, b4, b5 };

                for (int i = 0; i < SimpleCardPlaces.Length; i++)
                {
                    if (SimpleCardPlaces[i]!=null && SimpleCardPlaces[i].ID == c.ID)
                    {
                        cards.RemoveAll(x => x.ID == c.ID);
                    }
                }
            }

            return cards;
        }

        //Makes a new valid state by giving value to unkonwn cards
        public State GenerateRandom()
        {
            State newstate = Copy();
            
            List<Card> remaining = GetUnknownCards();

            Card[] singlePlaces = new Card[] { dbottom, adown, bdown, a1, a2, a3, a4, a5, b1, b2, b3, b4, b5 };
            Card[] newSinglePlaces = new Card[] { newstate.dbottom, newstate.adown, newstate.bdown, newstate.a1, newstate.a2, newstate.a3, newstate.a4, newstate.a5, newstate.b1, newstate.b2, newstate.b3, newstate.b4, newstate.b5 };

            for (int i = 0; i < singlePlaces.Length; i++)
            {
                if(singlePlaces[i]!=null && singlePlaces[i].ID=="unknown")
                {
                    int r = rand.Next(remaining.Count);
                    newSinglePlaces[i] = remaining[r];
                    remaining.RemoveAt(r);
                }
            }

            newstate.dbottom = newSinglePlaces[0];
            newstate.adown = newSinglePlaces[1];
            newstate.bdown = newSinglePlaces[2];
            newstate.a1 = newSinglePlaces[3];
            newstate.a2 = newSinglePlaces[4];
            newstate.a3 = newSinglePlaces[5];
            newstate.a4 = newSinglePlaces[6];
            newstate.a5 = newSinglePlaces[7];
            newstate.b1 = newSinglePlaces[8];
            newstate.b2 = newSinglePlaces[9];
            newstate.b3 = newSinglePlaces[10];
            newstate.b4 = newSinglePlaces[11];
            newstate.b5 = newSinglePlaces[12];

            newstate.deck = 0;
            while(remaining.Count>0)
            {
                int r = rand.Next(remaining.Count);
                Card.PushCardToDeck(ref newstate.deck, remaining[r]);
                remaining.RemoveAt(r);
            }

            newstate.CalculatePoints();

            return newstate;
        }

        private void SetHands()
        {
            aHand = CardSet.NONE;
            if (a1 != null) aHand += a1.cardSetIndex;
            if (a2 != null) aHand += a2.cardSetIndex;
            if (a3 != null) aHand += a3.cardSetIndex;
            if (a4 != null) aHand += a4.cardSetIndex;
            if (a5 != null) aHand += a5.cardSetIndex;

            bHand = CardSet.NONE;
            if (b1 != null) bHand += b1.cardSetIndex;
            if (b2 != null) bHand += b2.cardSetIndex;
            if (b3 != null) bHand += b3.cardSetIndex;
            if (b4 != null) bHand += b4.cardSetIndex;
            if (b5 != null) bHand += b5.cardSetIndex;
        }

        internal byte HigherSamePosition(Card[] hand)
        {
            Card down;
            if(isAnext)
            {
                down = bdown;
            }
            else
            {
                down = adown;
            }

            for (byte i = 0; i < hand.Length; i++)
            {
                if (hand[i] != null && hand[i].color == down.color && hand[i].value > down.value)
                {
                    return i;
                }
            }
            return 255;
        }

        internal byte SmallestPosition(Card[] hand)
        {
            byte position = 0;
            byte value = 255;

            for (byte i = 0; i < hand.Length; i++)
            {
                if (hand[i] != null && hand[i].value < value)
                {
                    position = i;
                    value = hand[i].value;
                }
            }

            return position;
        }

        internal byte TrumpPosition(Card[] hand)
        {
            for (byte i = 0; i < hand.Length; i++)
            {
                if(hand[i]!= null && hand[i].color == trump)
                {
                    return i;
                }
            }
            return 255;
        }

        internal byte NonTrump11Position(Card[] hand)
        {
            for (byte i = 0; i < hand.Length; i++)
            {
                if (hand[i] != null && hand[i].value == 11 && hand[i].color != trump)
                {
                    return i;
                }
            }
            return 255;
        }

        internal byte MarriagePosition(Card[] hand)
        {
            Card[] kings = new Card[] { Card.M4, Card.P4, Card.T4, Card.Z4 };
            Card[] queens = new Card[] { Card.M3, Card.P3, Card.T3, Card.Z3 };

            for (byte handCard = 0; handCard < hand.Length; handCard++)
            {
                for (byte king = 0; king < kings.Length; king++)
                {
                    if (hand[handCard]!= null && hand[handCard] == kings[king])
                    {
                        for (byte handCard2 = 0; handCard2 < hand.Length; handCard2++)
                        {
                            if (hand[handCard2] != null && hand[handCard2] == queens[king])
                            {
                                return handCard;
                            }
                        }
                    }
                }
            }

            return 0;
        }

        public void CalculateFullScore()
        {
            AscoreFull = AscoreBasic;
            if(AscoreBasic!=0)
            {
                AscoreFull += AscoreMarriages;
            }

            BscoreFull = BscoreBasic;
            if (BscoreBasic != 0)
            {
                BscoreFull += BscoreMarriages;
            }
        }

        //Call only from root
        public void InitPoints()
        {
            //it could be faster, but runs only once
            List<Card> atook2 = new List<Card>();
            List<Card> btook2 = new List<Card>();

            foreach (Card c in Card.dictionary.Values)
            {
                if (atook.HasFlag((CardSet)c.cardSetIndex))
                {
                    atook2.Add(c);
                }
                if (btook.HasFlag((CardSet)c.cardSetIndex))
                {
                    btook2.Add(c);
                }
            }

            //3x faster than lambda
            foreach (Card c in atook2)
            {
                AscoreBasic += c.value;
            }
            foreach (Card c in btook2)
            {
                BscoreBasic += c.value;
            }



            if (AM20)
            {
                if (trump == 'M')
                {
                    AscoreMarriages += 40;
                }
                else
                {
                    AscoreMarriages += 20;
                }
            }
            if (AP20)
            {
                if (trump == 'P')
                {
                    AscoreMarriages += 40;
                }
                else
                {
                    AscoreMarriages += 20;
                }
            }
            if (AT20)
            {
                if (trump == 'T')
                {
                    AscoreMarriages += 40;
                }
                else
                {
                    AscoreMarriages += 20;
                }
            }
            if (AZ20)
            {
                if (trump == 'Z')
                {
                    AscoreMarriages += 40;
                }
                else
                {
                    AscoreMarriages += 20;
                }
            }

            if (BM20)
            {
                if (trump == 'M')
                {
                    BscoreMarriages += 40;
                }
                else
                {
                    BscoreMarriages += 20;
                }
            }
            if (BP20)
            {
                if (trump == 'P')
                {
                    BscoreMarriages += 40;
                }
                else
                {
                    BscoreMarriages += 20;
                }
            }
            if (BT20)
            {
                if (trump == 'T')
                {
                    BscoreMarriages += 40;
                }
                else
                {
                    BscoreMarriages += 20;
                }
            }
            if (BZ20)
            {
                if (trump == 'Z')
                {
                    BscoreMarriages += 40;
                }
                else
                {
                    BscoreMarriages += 20;
                }
            }

            CalculateFullScore();
        }

        //Also sets isEnd flag
        public void CalculatePoints()
        {
            if(AscoreFull>=66)
            {
                isEnd = true;
                if(BscoreFull==0)
                {
                    Apoints = 3;
                }
                else if(BscoreFull<33)
                {
                    Apoints = 2;
                }
                else
                {
                    Apoints = 1;
                }
            }
            else if (BscoreFull >= 66)
            {
                isEnd = true;
                if (AscoreFull == 0)
                {
                    Bpoints = 3;
                }
                else if (AscoreFull < 33)
                {
                    Bpoints = 2;
                }
                else
                {
                    Bpoints = 1;
                }
            }
            //No more cards in play, the player who won the last cards (and would come next) won
            else if(atookCount + btookCount==20)
            {
                isEnd = true;
                if(isAnext)
                {
                    Apoints = 1;
                }
                else
                {
                    Bpoints = 1;
                }
            }
        }

        public bool StepOne(State s, byte action)
        {
            bool success = Step.Do(s, action);
            if(success)
            {
                CalculatePoints();
            }
            return success;
        }

        public State Copy()
        {
            State copy = new State
            {
                deck = deck,

                dbottom = dbottom,
                a1 = a1,
                a2 = a2,
                a3 = a3,
                a4 = a4,
                a5 = a5,
                b1 = b1,
                b2 = b2,
                b3 = b3,
                b4 = b4,
                b5 = b5,

                atook = atook,
                btook = btook,

                atookCount = atookCount,
                btookCount = btookCount,

                adown = adown,
                bdown = bdown,

                AM20 = AM20,
                AP20 = AP20,
                AT20 = AT20,
                AZ20 = AZ20,
                BM20 = BM20,
                BP20 = BP20,
                BT20 = BT20,
                BZ20 = BZ20,

                covered = covered,
                isAnext = isAnext,
                trump = trump,

                AscoreBasic = AscoreBasic,
                BscoreBasic = BscoreBasic,
                AscoreMarriages = AscoreMarriages,
                BscoreMarriages = BscoreMarriages
            };

            copy.CalculateFullScore();

            return copy;
        }

        public bool Equals(State other)
        {
            if(other==null)
            {
                return false;
            }

            if (atook != other.atook || btook != other.btook)
            {
                return false;
            }

            if (deck != other.deck)
            {
                return false;
            }

            if (isAnext != other.isAnext)
            {
                return false;
            }

            if (trump != other.trump)
            {
                return false;
            }

            if (AM20 != other.AM20 || AP20 != other.AP20 || AT20 != other.AT20 || AZ20 != other.AZ20 || BM20 != other.BM20 || BP20 != other.BP20 || BT20 != other.BT20 || BZ20 != other.BZ20)
            {
                return false;
            }

            if (covered != other.covered)
            {
                return false;
            }
            
            SetHands();
            other.SetHands();

            if (aHand != other.aHand || bHand != other.bHand)
            {
                return false;
            }

            Card[] SinglePlaces = new Card[] { dbottom, adown, bdown};
            Card[] OtherSinglePlaces = new Card[] { other.dbottom, other.adown, other.bdown };

            for (int i = 0; i < SinglePlaces.Length; i++)
            {
                if(SinglePlaces[i]==null && OtherSinglePlaces[i]==null)
                {
                    continue;
                }
                else if((SinglePlaces[i]==null && OtherSinglePlaces[i]!= null) || (SinglePlaces[i]!=null && OtherSinglePlaces[i]==null))
                {
                    return false;
                }
                else if(SinglePlaces[i].ID!=OtherSinglePlaces[i].ID)
                {
                    return false;
                }
            }

            return true;
        }

        public override int GetHashCode()
        {
            unchecked //Let it overflow
            {
                int hash = 17;
                hash = hash * 23 + deck.GetHashCode();
                hash = hash * 23 + atook.GetHashCode();
                hash = hash * 23 + btook.GetHashCode();
                return hash;
            }
        }
    }
}
