using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapFish66_Console
{
    public class Card
    {
        //Works like an enum
        public static readonly Card M2 = new Card("M2");
        public static readonly Card M3 = new Card("M3");
        public static readonly Card M4 = new Card("M4");
        public static readonly Card M10 = new Card("M10");
        public static readonly Card M11 = new Card("M11");
        public static readonly Card P2 = new Card("P2");
        public static readonly Card P3 = new Card("P3");
        public static readonly Card P4 = new Card("P4");
        public static readonly Card P10 = new Card("P10");
        public static readonly Card P11 = new Card("P11");
        public static readonly Card T2 = new Card("T2");
        public static readonly Card T3 = new Card("T3");
        public static readonly Card T4 = new Card("T4");
        public static readonly Card T10 = new Card("T10");
        public static readonly Card T11 = new Card("T11");
        public static readonly Card Z2 = new Card("Z2");
        public static readonly Card Z3 = new Card("Z3");
        public static readonly Card Z4 = new Card("Z4");
        public static readonly Card Z10 = new Card("Z10");
        public static readonly Card Z11 = new Card("Z11");
        public static readonly Card unknown = new Card("unknown");

        public static Dictionary<string, Card> dictionary = new Dictionary<string, Card>
        {
            {"M2", M2},
            {"M3", M3},
            {"M4", M4},
            {"M10", M10},
            {"M11", M11},
            {"P2", P2},
            {"P3", P3},
            {"P4", P4},
            {"P10", P10},
            {"P11", P11},
            {"T2", T2},
            {"T3", T3},
            {"T4", T4},
            {"T10", T10},
            {"T11", T11},
            {"Z2", Z2},
            {"Z3", Z3},
            {"Z4", Z4},
            {"Z10", Z10},
            {"Z11", Z11},
            {"unknown", unknown}
        };

        private static readonly Card[] cardsForDeck = new Card[]
        {
            null,
            M2, M3, M4, M10, M11,
            P2, P3, P4, P10, P11,
            T2, T3, T4, T10, T11,
            Z2, Z3, Z4, Z10, Z11,
            unknown
        };

        public string ID;
        public char color;
        public byte value;
        public int cardSetIndex;
        public int deckIndex;

        //Private!!!
        private Card(string id)
        {
            ID = id;
            color = id[0];
            if (ID != "unknown")
            {
                value = byte.Parse(id.Substring(1));
                deckIndex = 21;
            }

            //It runs only 21 times at the beginning
            if (id == "M2") deckIndex = 1;
            if (id == "M3") deckIndex = 2;
            if (id == "M4") deckIndex = 3;
            if (id == "M10") deckIndex = 4;
            if (id == "M11") deckIndex = 5;
            if (id == "P2") deckIndex = 6;
            if (id == "P3") deckIndex = 7;
            if (id == "P4") deckIndex = 8;
            if (id == "P10") deckIndex = 9;
            if (id == "P11") deckIndex = 10;
            if (id == "T2") deckIndex = 11;
            if (id == "T3") deckIndex = 12;
            if (id == "T4") deckIndex = 13;
            if (id == "T10") deckIndex = 14;
            if (id == "T11") deckIndex = 15;
            if (id == "Z2") deckIndex = 16;
            if (id == "Z3") deckIndex = 17;
            if (id == "Z4") deckIndex = 18;
            if (id == "Z10") deckIndex = 19;
            if (id == "Z11") deckIndex = 20;

            cardSetIndex = 1 << (deckIndex - 1);
        }

        public static Card GetCard(string id)
        {
            return dictionary[id];
        }

        public static void PushCardToDeck(ref long deck, Card card)
        {
            deck *= 22;
            deck += card.deckIndex;
        }

        public static Card PopCardFromDeck(ref long deck)
        {
            long i = deck % 22;
            deck /= 22;
            return cardsForDeck[i];
        }
    }
}
