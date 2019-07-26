using System;
using System.Collections.Generic;
using System.Text;

namespace SnapFish66_Console
{
    public static class RandomStateGenerator
    {
        public static Random r = new Random();
        
        private static int RandomEmptyPlace(char[] charray)
        {
            bool found = false;
            int rand = 0;

            while(!found)
            {
                rand = r.Next(charray.Length);
                if(charray[rand]==0)
                {
                    found = true;
                    return rand;
                }
            }

            //Should not reach this
            return -1;
        }
        
        private static bool A2040possible(char who, char card1, char card2)
        {
            bool card1taken = card1 == 'G' || card1 == 'H';
            bool card2taken = card2 == 'G' || card2 == 'H';

            //Both taken
            if (card1taken && card2taken)
            {
                return true;
            }

            //Neither is taken
            else if (!card1taken && !card2taken)
            {
                return false;
            }

            //One is taken
            else
            {
                //if A has the other, it is possible that he said, if it is unknown, we must not set 2040
                if(who == 'A' && ((card1taken && card2 =='A') || (card1=='A' && card2taken)))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        //Generates a random state ; starting state is 0
        public static string Generate(int depth)
        {
            char[] cardsPlaces = new char[20];
            char[] a2040 = new char[4] { 'X', 'X', 'X', 'X' };
            string trump;

            //If given number is odd, B put down a card first
            if (depth%2==1)
            {
                cardsPlaces[RandomEmptyPlace(cardsPlaces)] = 'F';
            }

            //First, taken cards
            int takenCards = depth;
            for (int i = 0; i < takenCards/2; i++)
            {
                int x = r.Next(2);

                //Put 2 random cards into G or H
                if(x%2==0)
                {
                    cardsPlaces[RandomEmptyPlace(cardsPlaces)] = 'G';
                    cardsPlaces[RandomEmptyPlace(cardsPlaces)] = 'G';
                }
                else
                {
                    cardsPlaces[RandomEmptyPlace(cardsPlaces)] = 'H';
                    cardsPlaces[RandomEmptyPlace(cardsPlaces)] = 'H';
                }
            }

            //Then, bottom of deck (if exists)
            if(depth < 10)
            {
                int x = RandomEmptyPlace(cardsPlaces);
                cardsPlaces[x] = 'D';

                //Set trump
                if(x < 5)
                {
                    trump = "M";
                }
                else if(x < 10)
                {
                    trump = "P";
                }
                else if (x < 15)
                {
                    trump = "T";
                }
                else
                {
                    trump = "Z";
                }
            }
            else
            {
                trump = "P";
            }

            //Then A hand
            int AhandSize = 5;
            if(depth>10)
            {
                AhandSize = (20 - depth + 1) / 2;
            }
            for (int i = 0; i < AhandSize; i++)
            {
                cardsPlaces[RandomEmptyPlace(cardsPlaces)] = 'A';
            }

            //The rest is unknown
            for (int i = 0; i < cardsPlaces.Length; i++)
            {
                if(cardsPlaces[i]==0)
                {
                    cardsPlaces[i] = 'U';
                }
            }

            //Set 20/40
            List<int> a2040cards = new List<int> { 1, 2, 6, 7, 11, 12, 16, 17 };
            for (int i = 0; i < 4; i++)
            {
                int x = r.Next(3);

                if (x == 0 && A2040possible('A', cardsPlaces[a2040cards[2 * i]], cardsPlaces[a2040cards[2 * i + 1]]))
                {
                    a2040[i] = 'A';
                }
                else if (x == 1 && A2040possible('B', cardsPlaces[a2040cards[2 * i]], cardsPlaces[a2040cards[2 * i + 1]]))
                {
                    a2040[i] = 'B';
                }
            }

            return "A" + trump + new string(cardsPlaces) + new string(a2040);
        }
    }
}
