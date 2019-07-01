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

        static void Main(string[] args)
        {
            SetStaticCards();
            Console.WriteLine("Hello World!");
        }
    }
}
