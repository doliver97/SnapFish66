using System;
using System.Collections.Generic;

namespace SnapFish66_Console
{
    class Program
    {
        private static readonly Random r = new Random();

        public static bool AllowReadDatabase { get; internal set; }
        public static bool AllowWriteDatabase { get; internal set; }

        public static List<Card> cards = new List<Card>();

        public static void SetStaticCards()
        {
            cards.Add(Card.M2);
            cards.Add(Card.M3);
            cards.Add(Card.M4);
            cards.Add(Card.M10);
            cards.Add(Card.M11);
            cards.Add(Card.P2);
            cards.Add(Card.P3);
            cards.Add(Card.P4);
            cards.Add(Card.P10);
            cards.Add(Card.P11);
            cards.Add(Card.T2);
            cards.Add(Card.T3);
            cards.Add(Card.T4);
            cards.Add(Card.T10);
            cards.Add(Card.T11);
            cards.Add(Card.Z2);
            cards.Add(Card.Z3);
            cards.Add(Card.Z4);
            cards.Add(Card.Z10);
            cards.Add(Card.Z11);
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
            state.trump = s[1];

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
                    state.dbottom = Card.GetCard(cards[i].ID);
                }
                else if (s[i + 2] == 'A')
                {
                    if(ahand==0)
                    {
                        state.a1 = Card.GetCard(cards[i].ID);
                        ahand = 1;
                    }
                    else if (ahand == 1)
                    {
                        state.a2 = Card.GetCard(cards[i].ID);
                        ahand = 2;
                    }
                    else if (ahand == 2)
                    {
                        state.a3 = Card.GetCard(cards[i].ID);
                        ahand = 3;
                    }
                    else if (ahand == 3)
                    {
                        state.a4 = Card.GetCard(cards[i].ID);
                        ahand = 4;
                    }
                    else if (ahand == 4)
                    {
                        state.a5 = Card.GetCard(cards[i].ID);
                        ahand = 5;
                    }
                }
                else if (s[i + 2] == 'B')
                {
                    if (bhand == 0)
                    {
                        state.b1 = Card.GetCard(cards[i].ID);
                        bhand = 1;
                    }
                    else if (bhand == 1)
                    {
                        state.b2 = Card.GetCard(cards[i].ID);
                        bhand = 2;
                    }
                    else if (bhand == 2)
                    {
                        state.b3 = Card.GetCard(cards[i].ID);
                        bhand = 3;
                    }
                    else if (bhand == 3)
                    {
                        state.b4 = Card.GetCard(cards[i].ID);
                        bhand = 4;
                    }
                    else if (bhand == 4)
                    {
                        state.b5 = Card.GetCard(cards[i].ID);
                        bhand = 5;
                    }
                }
                else if(s[i+2] == 'E')
                {
                    state.adown = Card.GetCard(cards[i].ID);
                }
                else if(s[i+2]=='F')
                {
                    state.bdown = Card.GetCard(cards[i].ID);
                }
                else if (s[i+2] == 'G')
                {
                    state.atook.Add(Card.GetCard(cards[i].ID));
                }
                else if (s[i + 2] == 'H')
                {
                    state.btook.Add(Card.GetCard(cards[i].ID));
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
                    state.b1 = Card.GetCard("unknown");
                }
                else if(i==1)
                {
                    state.b2 = Card.GetCard("unknown");
                }
                else if (i == 2)
                {
                    state.b3 = Card.GetCard("unknown");
                }
                else if (i == 3)
                {
                    state.b4 = Card.GetCard("unknown");
                }
                else if (i == 4)
                {
                    state.b5 = Card.GetCard("unknown");
                }
            }

            for(int i =0; i< unknown-bHandUnknown;i++)
            {
                state.deck.Add(Card.GetCard("unknown"));
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

        // Generating training data
        private static void Training()
        {
            TrainingDataHandler.Init();

            int dataCount = 100000;
            for (int i = 0; i < dataCount; i++)
            {
                Console.Clear();
                Console.WriteLine((i + 1) + "/" + dataCount);

                string stateString = RandomStateGenerator.Generate(r.Next(8, 16)); // between 8 and (exclusive)16 is ideal
                State s = SetStateFromString(stateString);

                //If game has ended, do not calculate, find new instead
                s.CalculatePoints();
                if (s.Apoints != 0 || s.Bpoints != 0)
                {
                    i--;
                    continue;
                }

                GameTree tree = new GameTree(s);
                tree.Calculate();

                List<float> values = tree.averages.GetRange(0, 5);
                //Do some formatting
                for (int j = 0; j < values.Count; j++)
                {
                    if (float.IsNaN(values[j]))
                    {
                        values[j] = 0;
                    }
                    values[j] = (float)Math.Round(values[j], 2);
                }

                TrainingDataHandler.Write(stateString, values);
            }

            TrainingDataHandler.Close();
        }

        // Calculating a single problem
        private static void Single()
        {
            //State s = SetStateFromString("APDUAGUUAGGAUHHAHUGHAUXXXX");
            //State s = SetStateFromString("APUUUUADAAAAUUUUUUUUUUXXXX"); //Best starting hand
            //State s = SetStateFromString("APAUAUUDUUHGHGUUAAAGUGXXXX"); //test 3deck
            State s = SetStateFromString("APAUAUUDUUHGHGUUAAAUUUXXXX");  // A 5 deck state
            //State s = SetStateFromString("APHHHUADAAAAHHUUUUHHUHXXXX"); // Test 8 , result must be +2 for all
            //State s = SetStateFromString("AMHGGUHAHHGGGHGAUUHGGAXXXX"); //Test 5, result must be -1 for all

            GameTree tree = new GameTree(s);
            tree.Calculate();
        }

        static void Main(string[] args)
        {
            AllowReadDatabase = false;
            AllowWriteDatabase = false;

            SetStaticCards();

            //Setting mode
            //Training();
            Single();

            Console.WriteLine();
            Console.WriteLine("Exit with enter...");

            Console.ReadLine();
        }
    }
}
