using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapFish66_Console
{
    public class State
    {
        ////This enum represents the cards
        ////The first digit represents the colour: 2-M 4-P 6-T 8-Z
        ////The number modulo 20 is the value of the card
        //public enum Card
        //{
        //    NONE = 0,
        //    M2 = 22,
        //    M3 = 23,
        //    M4 = 24,
        //    M10 = 30,
        //    M11 = 31,
        //    P2 = 42,
        //    P3 = 43,
        //    P4 = 44,
        //    P10 = 50,
        //    P11 = 51,
        //    T2 = 62,
        //    T3 = 63,
        //    T4 = 64,
        //    T10 = 70,
        //    T11 = 71,
        //    Z2 = 82,
        //    Z3 = 83,
        //    Z4 = 84,
        //    Z10 = 90,
        //    Z11 = 91
        //};


        public List<Card> deck = new List<Card>();
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
        public List<Card> atook = new List<Card>();
        public List<Card> btook = new List<Card>();
        public Card adown;
        public Card bdown;

        public bool AM20;
        public bool AP20;
        public bool AT20;
        public bool AZ20;
        public bool BM20;
        public bool BP20;
        public bool BT20;
        public bool BZ20;

        public bool covered = false;
        
        //TODO be an enum
        public string next = "";

        //TODO delete if possible
        public List<string> IDs = new List<string> { "M2", "M3", "M4", "M10", "M11", "P2", "P3", "P4", "P10", "P11", "T2", "T3", "T4", "T10", "T11", "Z2", "Z3", "Z4", "Z10", "Z11" };

        public char trump = '-';

        //Collect 66
        public byte Ascore = 0;
        public byte Bscore = 0;

        //Points: {0-3}
        public byte Apoints = 0;
        public byte Bpoints = 0;
        
        private List<Card> GetUnknownCards()
        {
            List<Card> cards = new List<Card>();
            foreach (var c in Program.cards)
            {
                cards.Add(c);
            }

            foreach(var c in Program.cards)
            {
                List<Card>[] MultiCardPlaces = new List<Card>[] {atook, btook};

                for (int i = 0; i < MultiCardPlaces.Length; i++)
                {
                    if(CardInList(MultiCardPlaces[i],c.ID)>0)
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

        public State()
        {
            CalculatePoints();
        }

        //Makes a new valid state by giving value to unkonwn cards
        public State GenerateRandom()
        {
            State newstate = Copy();
            
            List<Card> remaining = GetUnknownCards();

            Card[] singlePlaces = new Card[] { dbottom, adown, bdown, a1, a2, a3, a4, a5, b1, b2, b3, b4, b5 };
            Card[] newSinglePlaces = new Card[] { newstate.dbottom, newstate.adown, newstate.bdown, newstate.a1, newstate.a2, newstate.a3, newstate.a4, newstate.a5, newstate.b1, newstate.b2, newstate.b3, newstate.b4, newstate.b5 };

            Random rand = new Random();
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

            newstate.deck.Clear();
            while(remaining.Count>0)
            {
                int r = rand.Next(remaining.Count);
                newstate.deck.Add(remaining[r]);
                remaining.RemoveAt(r);
            }

            newstate.CalculatePoints();

            return newstate;
        }

        //private bool Check2040()
        //{
        //    if (AM20 && BM20) return false;
        //    if (AP20 && BP20) return false;
        //    if (AT20 && BT20) return false;
        //    if (AZ20 && BZ20) return false;
        //    return true;
        //}

        //How many cards are in the list with given ID
        private int CardInList(List<Card> cards, string id)
        {
            return cards.Count(x => x.ID == id);
        }
        
        private void CountScores()
        {
            
            Ascore = (byte)atook.Sum(x => x.value);
            Bscore = (byte)btook.Sum(x => x.value);

            if(Ascore!=0)
            {
                if(AM20)
                {
                    if(trump == 'M')
                    {
                        Ascore += 40;
                    }
                    else
                    {
                        Ascore += 20;
                    }
                }
                if (AP20)
                {
                    if (trump == 'P')
                    {
                        Ascore += 40;
                    }
                    else
                    {
                        Ascore += 20;
                    }
                }
                if (AT20)
                {
                    if (trump == 'T')
                    {
                        Ascore += 40;
                    }
                    else
                    {
                        Ascore += 20;
                    }
                }
                if (AZ20)
                {
                    if (trump == 'Z')
                    {
                        Ascore += 40;
                    }
                    else
                    {
                        Ascore += 20;
                    }
                }
            }

            if (Bscore != 0)
            {
                if (BM20)
                {
                    if (trump == 'M')
                    {
                        Bscore += 40;
                    }
                    else
                    {
                        Bscore += 20;
                    }
                }
                if (BP20)
                {
                    if (trump == 'P')
                    {
                        Bscore += 40;
                    }
                    else
                    {
                        Bscore += 20;
                    }
                }
                if (BT20)
                {
                    if (trump == 'T')
                    {
                        Bscore += 40;
                    }
                    else
                    {
                        Bscore += 20;
                    }
                }
                if (BZ20)
                {
                    if (trump == 'Z')
                    {
                        Bscore += 40;
                    }
                    else
                    {
                        Bscore += 20;
                    }
                }
            }


        }

        public void CalculatePoints()
        {
            CountScores();

            if(Ascore>=66)
            {
                if(Bscore==0)
                {
                    Apoints = 3;
                }
                else if(Bscore<33)
                {
                    Apoints = 2;
                }
                else
                {
                    Apoints = 1;
                }
            }
            else if (Bscore >= 66)
            {
                if (Ascore == 0)
                {
                    Bpoints = 3;
                }
                else if (Ascore < 33)
                {
                    Bpoints = 2;
                }
                else
                {
                    Bpoints = 1;
                }
            }
            //No more cards in play, the player who won the last cards (and would come next) won
            else if(atook.Count+btook.Count==20)
            {
                if(next == "A")
                {
                    Apoints = 1;
                }
                else
                {
                    Bpoints = 1;
                }
            }
        }

        public bool Step(State st,string action)
        {

            Step step = new Step();
            return step.Do(st, action);
        }

        private List<Card> CopyCardList(List<Card> list)
        {
            List<Card> copy = new List<Card>();
            foreach (var item in list)
            {
                copy.Add(item);
            }

            return copy;
        }

        public State Copy()
        {
            State copy = new State();

            copy.deck = CopyCardList(deck);
            if(dbottom!=null)
            {
                copy.dbottom = new Card(dbottom.ID);
            }
            if(a1!=null)
            {
                copy.a1 = new Card(a1.ID);
            }
            if (a2 != null)
            {
                copy.a2 = new Card(a2.ID);
            }
            if (a3 != null)
            {
                copy.a3 = new Card(a3.ID);
            }
            if (a4 != null)
            {
                copy.a4 = new Card(a4.ID);
            }
            if (a5 != null)
            {
                copy.a5 = new Card(a5.ID);
            }
            if (b1 != null)
            {
                copy.b1 = new Card(b1.ID);
            }
            if (b2 != null)
            {
                copy.b2 = new Card(b2.ID);
            }
            if (b3 != null)
            {
                copy.b3 = new Card(b3.ID);
            }
            if (b4 != null)
            {
                copy.b4 = new Card(b4.ID);
            }
            if (b5 != null)
            {
                copy.b5 = new Card(b5.ID);
            }
            copy.atook = CopyCardList(atook);
            copy.btook = CopyCardList(btook);
            if (adown != null)
            {
                copy.adown = new Card(adown.ID);
            }
            if (bdown != null)
            {
                copy.bdown = new Card(bdown.ID);
            }

            copy.AM20 = AM20;
            copy.AP20 = AP20;
            copy.AT20 = AT20;
            copy.AZ20 = AZ20;
            copy.BM20 = BM20;
            copy.BP20 = BP20;
            copy.BT20 = BT20;
            copy.BZ20 = BZ20;

            copy.covered = covered;
            copy.next = next;
            copy.trump = trump;

            copy.CountScores();

            return copy;
        }

        //Order does matter
        private bool IsSameDeck(List<Card> one, List<Card> other)
        {
            for (int i = 0; i < one.Count; i++)
            {
                if(one[i].ID != other[i].ID)
                {
                    return false;
                }
            }

            return true;
        }

        //Order does not matter
        private bool IsSameCardSet(List<Card> one, List<Card> other)
        {
            if(one.Count!=other.Count)
            {
                return false;
            }

            for (int i = 0; i < one.Count; i++)
            {
                bool found = false;

                for (int j = 0; j < other.Count; j++)
                {
                    if(one[i]==null && other[j]==null)
                    {
                        found = true;
                    }
                    else if(one[i]!=null && other[j] != null && one[i].ID == other[j].ID)
                    {
                        found = true;
                    }
                }

                if(!found)
                {
                    return false;
                }
            }

            return true;
        }

        public bool IsSame(State other)
        {
            List<Card> aHand = new List<Card>{a1,a2,a3,a4,a5};
            List<Card> bHand = new List<Card>{b1,b2,b3,b4,b5};
            List<Card> oaHand = new List<Card>{other.a1,other.a2,other.a3,other.a4,other.a5};
            List<Card> obHand = new List<Card>{other.b1,other.b2,other.b3, other.b4,other.b5};

            List<List<Card>> MultiPlaces = new List<List<Card>> { aHand, bHand, atook, btook};
            List<List<Card>> OtherMultiPlaces = new List<List<Card>> { oaHand, obHand, other.atook, other.btook};

            for (int i = 0; i < MultiPlaces.Count; i++)
            {
                if(!IsSameCardSet(MultiPlaces[i],OtherMultiPlaces[i]))
                {
                    return false;
                }
            }

            Card[] SinglePlaces = new Card[] { dbottom, adown, bdown};
            Card[] OtherSinglePlaces = new Card[] { other.dbottom, other.adown, other.bdown };

            for (int i = 0; i < SinglePlaces.Length; i++)
            {
                if(SinglePlaces[i]==null && OtherSinglePlaces[i]==null)
                {
                    continue;
                }
                else if((SinglePlaces[i]==null && OtherSinglePlaces[i]!= null) || (SinglePlaces[i]!=null && OtherSinglePlaces==null))
                {
                    return false;
                }
                else if(SinglePlaces[i].ID!=OtherSinglePlaces[i].ID)
                {
                    return false;
                }
            }

            if(!IsSameDeck(deck,other.deck))
            {
                return false;
            }

            if(next!=other.next)
            {
                return false;
            }

            if(trump!=other.trump)
            {
                return false;
            }

            if(AM20!=other.AM20 || AP20!=other.AP20 || AT20!=other.AT20 || AZ20!=other.AZ20 || BM20!=other.BM20 || BP20!=other.BP20 || BT20!=other.BT20 || BZ20!=other.BZ20)
            {
                return false;
            }

            if(covered!=other.covered)
            {
                return false;
            }

            return true;
        }

    }
}
