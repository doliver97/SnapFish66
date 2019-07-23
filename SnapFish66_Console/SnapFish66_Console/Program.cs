using System;
using System.Collections.Generic;

namespace SnapFish66_Console
{
    class Program
    {
        public static bool AllowReadDatabase { get; internal set; }
        public static bool AllowWriteDatabase { get; internal set; }

        public static List<Card> cards = new List<Card>();

        public static void SetStaticCards()
        {
            cards.Add(new Card("M2"));
            cards.Add(new Card("M3"));
            cards.Add(new Card("M4"));
            cards.Add(new Card("M10"));
            cards.Add(new Card("M11"));
            cards.Add(new Card("P2"));
            cards.Add(new Card("P3"));
            cards.Add(new Card("P4"));
            cards.Add(new Card("P10"));
            cards.Add(new Card("P11"));
            cards.Add(new Card("T2"));
            cards.Add(new Card("T3"));
            cards.Add(new Card("T4"));
            cards.Add(new Card("T10"));
            cards.Add(new Card("T11"));
            cards.Add(new Card("Z2"));
            cards.Add(new Card("Z3"));
            cards.Add(new Card("Z4"));
            cards.Add(new Card("Z10"));
            cards.Add(new Card("Z11"));
        }

        private static State SetStateFromString(string s)
        {
            //State string [26 characters]
            //[0] - next player (A,B)
            //[1] - trump (M,P,T,Z)
            //[2-21] - position of each card (M2, M3, ...)
            //   U : unknown
            //   D : bottom of deck
            //   A : in A's hand (Ordering is alpabetical)
            //   B : in B's hand
            //   E : A put down
            //   F : B put down
            //   G : taken by A
            //   H : taken by B
            //[22-25] - 20/40 for each color (A,B,X)

            State state = new State();

            state.next = s[0].ToString();
            state.trump = s[1].ToString();

            //Card[] singlePlaces = new Card[] { state.dbottom, state.adown, state.bdown};


            //List<Card> ahand = new List<Card>();
            //List<Card> bhand = new List<Card>();
            //List<Card>[] multiPlaces = new List<Card>[] { ahand, bhand, state.deck, state.atook, state.btook};

            int ahand = 0;
            int bhand = 0;
            int unknown = 0;
            
            for (int i = 0; i < cards.Count; i++)
            {
                if(s[i+2]=='D')
                {
                    state.dbottom = new Card(cards[i].ID);
                }
                else if (s[i + 2] == 'A')
                {
                    if(ahand==0)
                    {
                        state.a1 = new Card(cards[i].ID);
                        ahand = 1;
                    }
                    else if (ahand == 1)
                    {
                        state.a2 = new Card(cards[i].ID);
                        ahand = 2;
                    }
                    else if (ahand == 2)
                    {
                        state.a3 = new Card(cards[i].ID);
                        ahand = 3;
                    }
                    else if (ahand == 3)
                    {
                        state.a4 = new Card(cards[i].ID);
                        ahand = 4;
                    }
                    else if (ahand == 4)
                    {
                        state.a5 = new Card(cards[i].ID);
                        ahand = 5;
                    }
                }
                else if (s[i + 2] == 'B')
                {
                    if (bhand == 0)
                    {
                        state.b1 = new Card(cards[i].ID);
                        bhand = 1;
                    }
                    else if (bhand == 1)
                    {
                        state.b2 = new Card(cards[i].ID);
                        bhand = 2;
                    }
                    else if (bhand == 2)
                    {
                        state.b3 = new Card(cards[i].ID);
                        bhand = 3;
                    }
                    else if (bhand == 3)
                    {
                        state.b4 = new Card(cards[i].ID);
                        bhand = 4;
                    }
                    else if (bhand == 4)
                    {
                        state.b5 = new Card(cards[i].ID);
                        bhand = 5;
                    }
                }
                else if(s[i+2] == 'E')
                {
                    state.adown = new Card(cards[i].ID);
                }
                else if(s[i+2]=='F')
                {
                    state.bdown = new Card(cards[i].ID);
                }
                else if (s[i+2] == 'G')
                {
                    state.atook.Add(new Card(cards[i].ID));
                }
                else if (s[i + 2] == 'H')
                {
                    state.btook.Add(new Card(cards[i].ID));
                }
                else if(s[i+2] == 'U')
                {
                    unknown++;
                }
            }

            //Setting unkonwn cards
            int adown = state.adown != null ? 1 : 0;
            int bdown = state.bdown != null ? 1 : 0;
            int bHandUnknown = ahand + adown - bhand - bdown;

            for (int i = 0; i < bHandUnknown; i++)
            {
                if(i==0)
                {
                    state.b1 = new Card("unknown");
                }
                else if(i==1)
                {
                    state.b2 = new Card("unknown");
                }
                else if (i == 2)
                {
                    state.b3 = new Card("unknown");
                }
                else if (i == 3)
                {
                    state.b4 = new Card("unknown");
                }
                else if (i == 4)
                {
                    state.b5 = new Card("unknown");
                }
            }

            for(int i =0; i< unknown-bHandUnknown;i++)
            {
                state.deck.Add(new Card("unknown"));
            }

            if(s[22]=='A')
            {
                state.AM20 = true;
            }
            if (s[22] == 'B')
            {
                state.BM20 = true;
            }
            if (s[23] == 'A')
            {
                state.AP20 = true;
            }
            if (s[23] == 'B')
            {
                state.BP20 = true;
            }
            if (s[24] == 'A')
            {
                state.AT20 = true;
            }
            if (s[24] == 'B')
            {
                state.BT20 = true;
            }
            if (s[25] == 'A')
            {
                state.AZ20 = true;
            }
            if (s[25] == 'B')
            {
                state.BZ20 = true;
            }

            return state;
        }

        static void Main(string[] args)
        {
            AllowReadDatabase = false;
            AllowWriteDatabase = false;

            SetStaticCards();

            //State s = SetStateFromString("APGGAGUUAHGAGHUAHUGHAUXXXX");
            //State s = SetStateFromString("APUUUUADAAAAUUUUUUUUUUXXXX"); //Best starting hand
            State s = SetStateFromString("APAUAUUDUUHGHGUUAAAGUGXXXX"); //test 3deck
            //State s = SetStateFromString("APHHHUADAAAAHHUUUUHHUHXXXX"); // Test 8 , result must be +2 for all
            //State s = SetStateFromString("AMHGGUHAHHGGGHGAUUHGGAXXXX"); //Test 5, result must be -1 for all

            GameTree tree = new GameTree(s);

            tree.Calculate(); //TODO write back

            //TEST (this is now a D1 test)
            //char[] tst = new char[] { 'A', 'A', 'A', 'A', 'A', 'G', 'G', 'G', 'G', 'H', 'H', 'H', 'H', 'G', 'G', 'U', 'U', 'U', 'U'};
            //Random r = new Random();
            //for (int i = 0; i < 1000; i++)
            //{
            //    //Generate random state
            //    for (int j = 0; j < tst.Length; j++)
            //    {
            //        int x = r.Next(tst.Length);
            //        char temp = tst[j];
            //        tst[j] = tst[x];
            //        tst[x] = temp;
            //    }

            //    string str = "AMU" + new string(tst) + "XXXX";

            //    s = SetStateFromString(str);
            //    tree = new GameTree(s);
            //    tree.Calculate();
            //}
            //TEST END

            Console.WriteLine();
            Console.WriteLine("Exit with enter...");

            Console.ReadLine();
        }
    }
}
