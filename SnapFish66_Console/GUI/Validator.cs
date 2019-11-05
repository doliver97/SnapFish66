using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI
{
    public static class Validator
    {
        //returns with the error message 
        public static string Validate(string text)
        {
            bool formatOK = ValidStringFormat(text);
            if (!formatOK)
            {
                return "Error: invalid string format";
            }
            string cnd = CheckPlaces(text);
            if (cnd != "-")
            {
                return "Error: " + cnd;
            }

            return "OK";
        }

        private static string CheckPlaces(string text)
        {
            int aCount = 0;
            for (int i = 2; i < 22; i++)
            {
                if(text[i] == 'A')
                {
                    aCount++;
                }
            }
            int dCount = text.Count(x => x == 'D');
            int fCount = text.Count(x => x == 'F');

            int gCount = text.Count(x => x == 'G');
            int hCount = text.Count(x => x == 'H');
            int takenCount = gCount + hCount;

            if(fCount > 1)
            {
                return "F in multiple places in text";
            }

            if(dCount > 1)
            {
                return "D in multiple places in text";
            }

            if(gCount % 2 == 1)
            {
                return "Odd number of cards taken by player";
            }

            if (hCount % 2 == 1)
            {
                return "Odd number of cards taken by the opponent";
            }

            if (dCount == 0 && takenCount<10)
            {
                return "Bottom card of deck not found";
            }

            if(dCount == 1 && takenCount >= 10)
            {
                return "Bottom card of deck found while in endgame";
            }

            if(aCount > 5)
            {
                return "Player has too many cards";
            }

            if(aCount == 0)
            {
                return "Player has no cards";
            }

            if(dCount == 1)
            {
                int dIndex = 0;
                char dColor;
                for (int i = 0; i < text.Length; i++)
                {
                    if(text[i] == 'D')
                    {
                        dIndex = i;
                    }
                }

                if(dIndex < 7)
                {
                    dColor = 'M';
                }
                else if (dIndex < 12)
                {
                    dColor = 'P';
                }
                else if (dIndex < 17)
                {
                    dColor = 'T';
                }
                else
                {
                    dColor = 'Z';
                }

                if(dColor != text[1])
                {
                    return "Trump and color of bottom card does not match";
                }
            }

            if(takenCount <= 10 && aCount<5)
            {
                return "Player must have 5 cards";
            }

            if (takenCount>10 && aCount > (20-takenCount)/2)
            {
                return "Player must have " + ((20 - takenCount)/2) + " cards";
            }

            return "-";
        }

        private static bool ValidStringFormat(string text)
        {
            if(text.Length != 26)
            {
                return false;
            }

            if(text[0] != 'A')
            {
                return false;
            }

            if((text[1] != 'M') && (text[1] != 'P') && (text[1] != 'T') && (text[1] != 'Z'))
            {
                return false;
            }

            for (int i = 2; i < 22; i++)
            {
                if((text[i] != 'A') && (text[i] != 'B') && (text[i] != 'U') && (text[i] != 'D') && (text[i] != 'F') && (text[i] != 'G') && (text[i] != 'H'))
                {
                    return false;
                }
            }

            for (int i = 22; i < 26; i++)
            {
                if ((text[i] != 'X') && (text[i] != 'A') && (text[i] != 'B'))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
