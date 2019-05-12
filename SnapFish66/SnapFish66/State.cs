﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapFish66
{
    public class State
    {
        public List<Card> deck;
        public List<Card> dbottom;
        public List<Card> a1;
        public List<Card> a2;
        public List<Card> a3;
        public List<Card> a4;
        public List<Card> a5;
        public List<Card> b1;
        public List<Card> b2;
        public List<Card> b3;
        public List<Card> b4;
        public List<Card> b5;
        public List<Card> atook;
        public List<Card> btook;
        public List<Card> adown;
        public List<Card> bdown;

        public bool AM20;
        public bool AP20;
        public bool AT20;
        public bool AZ20;
        public bool BM20;
        public bool BP20;
        public bool BT20;
        public bool BZ20;

        public string ErrorMessage = "OK";

        List<string> IDs = new List<string> { "M2", "M3", "M4", "M10", "M11", "P2", "P3", "P4", "P10", "P11", "T2", "T3", "T4", "T10", "T11", "Z2", "Z3", "Z4", "Z10", "Z11" };

        public string trump = "";

        //Collect 66
        private int Ascore = 0;
        private int Bscore = 0;

        //Points: {0-3}
        public int Apoints = 0;
        public int Bpoints = 0;

        private bool Check2040()
        {
            if (AM20 && BM20) return false;
            if (AP20 && BP20) return false;
            if (AT20 && BT20) return false;
            if (AZ20 && BZ20) return false;
            return true;
        }

        private int CardInList(List<Card> cards, string id)
        {
            return cards.Count(x => x.ID == id);
        }

        private string CardInMorePlaces()
        {
            foreach (string cardID in IDs)
            {
                int count = 0;
                count += CardInList(dbottom, cardID);
                count += CardInList(a1, cardID);
                count += CardInList(a2, cardID);
                count += CardInList(a3, cardID);
                count += CardInList(a4, cardID);
                count += CardInList(a5, cardID);
                count += CardInList(b1, cardID);
                count += CardInList(b2, cardID);
                count += CardInList(b3, cardID);
                count += CardInList(b4, cardID);
                count += CardInList(b5, cardID);
                count += CardInList(atook, cardID);
                count += CardInList(btook, cardID);
                count += CardInList(adown, cardID);
                count += CardInList(bdown, cardID);

                if(count>1)
                {
                    return cardID;
                }
            }

            return "-";
        }

        private string MoreCardInSinglePlace()
        {
            if(dbottom.Count>1)
            {
                return "Talon alján";
            }
            if (a1.Count > 1 || a2.Count>1 || a3.Count>1 || a4.Count>1 || a5.Count>1)
            {
                return "A kezében";
            }
            if (b1.Count > 1 || b2.Count > 1 || b3.Count > 1 || b4.Count > 1 || b5.Count > 1)
            {
                return "B kezében";
            }
            if(adown.Count>1)
            {
                return "A letett lapja";
            }
            if (bdown.Count > 1)
            {
                return "B letett lapja";
            }

            return "-";
        }

        public bool Check()
        {
            if(!Check2040())
            {
                ErrorMessage = "Hiba: 20/40";
                return false;
            }

            string cimp = CardInMorePlaces();
            if (cimp!="-")
            {
                ErrorMessage = "Hiba: " + cimp + " több helyen!";
                return false;
            }

            string mcsp = MoreCardInSinglePlace();
            {
                if(mcsp!="-")
                {
                    ErrorMessage = "Hiba: több lap " + mcsp;
                    return false;
                }
            }
            return true;
        }

        private void CountScores()
        {
            
            Ascore += atook.Sum(x => x.GetValue());
            Bscore += btook.Sum(x => x.GetValue());

            if(Ascore!=0)
            {
                if(AM20)
                {
                    if(trump == "M")
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
                    if (trump == "P")
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
                    if (trump == "T")
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
                    if (trump == "Z")
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
                    if (trump == "M")
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
                    if (trump == "P")
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
                    if (trump == "T")
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
                    if (trump == "Z")
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

            if (Bscore >= 66)
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
        }
    }
}
