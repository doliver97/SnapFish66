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
        public static string Validate()
        {
            string errorMessage = "OK";

            if (!Check2040())
            {
                errorMessage = "Error: 20/40";
                return errorMessage;
            }

            string cimp = CardInMorePlaces();
            if (cimp != "-")
            {
                errorMessage = "Error: " + cimp + " in more places!";
                return errorMessage;
            }

            string mcsp = MoreCardInSinglePlace();
            if (mcsp != "-")
            {
                errorMessage = "Error: more cards at " + mcsp;
                return errorMessage;
            }

            string cnd = CardNumberDiff();
            if (cnd != "-")
            {
                errorMessage = "Error: number of cards in " + cnd + " hand";
                return errorMessage;
            }

            return "OK";
        }

        private static bool Check2040()
        {
            //if (AM20 && BM20) return false;
            //if (AP20 && BP20) return false;
            //if (AT20 && BT20) return false;
            //if (AZ20 && BZ20) return false;
            return true;
        }

        private static string CardInMorePlaces()
        {
            //foreach (string cardID in IDs)
            //{
            //    int count = 0;
            //    count += CardInList(dbottom, cardID);
            //    count += CardInList(a1, cardID);
            //    count += CardInList(a2, cardID);
            //    count += CardInList(a3, cardID);
            //    count += CardInList(a4, cardID);
            //    count += CardInList(a5, cardID);
            //    count += CardInList(b1, cardID);
            //    count += CardInList(b2, cardID);
            //    count += CardInList(b3, cardID);
            //    count += CardInList(b4, cardID);
            //    count += CardInList(b5, cardID);
            //    count += CardInList(atook, cardID);
            //    count += CardInList(btook, cardID);
            //    count += CardInList(adown, cardID);
            //    count += CardInList(bdown, cardID);

            //    if (count > 1)
            //    {
            //        return cardID;
            //    }
            //}

            return "-";
        }

        private static string MoreCardInSinglePlace()
        {
            //if (dbottom.Count > 1)
            //{
            //    return "Talon alján";
            //}
            //if (a1.Count > 1 || a2.Count > 1 || a3.Count > 1 || a4.Count > 1 || a5.Count > 1)
            //{
            //    return "A kezében";
            //}
            //if (b1.Count > 1 || b2.Count > 1 || b3.Count > 1 || b4.Count > 1 || b5.Count > 1)
            //{
            //    return "B kezében";
            //}
            //if (adown.Count > 1)
            //{
            //    return "A letett lapja";
            //}
            //if (bdown.Count > 1)
            //{
            //    return "B letett lapja";
            //}

            return "-";
        }

        private static string CardNumberDiff()
        {
            //int Anum = a1.Count + a2.Count + a3.Count + a4.Count + a5.Count + adown.Count;
            //int Bnum = b1.Count + b2.Count + b3.Count + b4.Count + b5.Count + bdown.Count;
            //if (Anum < Bnum)
            //{
            //    return "A";
            //}
            //if (Bnum < Anum)
            //{
            //    return "B";
            //}
            return "-";
        }
    }
}
