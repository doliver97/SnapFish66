using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculate
{
    class TestFileHandler
    {
        public static string testsDirectory;

        public static string[] GetFileNames()
        {
            string workingDirectory = Directory.GetCurrentDirectory();
            string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.Parent.FullName;

            testsDirectory = projectDirectory + "\\Tesztek";

            string[] fileNames = Directory.GetFiles(testsDirectory, "*.txt");

            return fileNames;
        }

        public static string[] GetTrimmedFileNames()
        {
            string[] fullnames = GetFileNames();
            string[] trimmed = new string[fullnames.Length];
            for(int i=0;i<fullnames.Length;i++)
            {
                string[] splitted = fullnames[i].Split('\\');
                trimmed[i] = splitted[splitted.Length-1];
            }

            return trimmed;
        }

        //Adds a card to the given multi place
        private static void AddToCardPlace(ref State.CardSet place, string ID)
        {
            place += Card.GetCard(ID).value;
        }

        //Adds a card to the given single place
        private static void AddToCardPlace(ref Card place, string ID)
        {
            if (ID == "U")
            {
                place = Card.GetCard("unknown");
            }
            else
            {
                place = Card.GetCard(ID);
            }
        }

        public static State CreateState(string trimmedFileName)
        {
            string fullName = testsDirectory + "\\" + trimmedFileName;

            StreamReader sr = new StreamReader(fullName);

            State state = new State();

            //First line is DBottom
            string DBottomLine = sr.ReadLine();
            string[] DBottomSplit = DBottomLine.Split(' ');
            if(DBottomSplit.Length>1)
            {
                AddToCardPlace(ref state.dbottom, DBottomSplit[1]);
            }

            //AHand
            string AHandLine = sr.ReadLine();
            string[] AHandSplit = AHandLine.Split(' ');
            if(AHandSplit.Length>1)
            {
                AddToCardPlace(ref state.a1, AHandSplit[1]);
            }
            if (AHandSplit.Length > 2)
            {
                AddToCardPlace(ref state.a2, AHandSplit[2]);
            }
            if (AHandSplit.Length > 3)
            {
                AddToCardPlace(ref state.a3, AHandSplit[3]);
            }
            if (AHandSplit.Length > 4)
            {
                AddToCardPlace(ref state.a4, AHandSplit[4]);
            }
            if (AHandSplit.Length > 5)
            {
                AddToCardPlace(ref state.a5, AHandSplit[5]);
            }


            //Adown
            string ADownLine = sr.ReadLine();
            string[] ADownSplit = ADownLine.Split(' ');
            if (ADownSplit.Length > 1)
            {
                AddToCardPlace(ref state.adown, ADownSplit[1]);
            }


            //Atook
            string ATookLine = sr.ReadLine();
            string[] ATookSplit = ATookLine.Split(' ');
            for (int i = 1; i < ATookSplit.Length; i++)
            {
                AddToCardPlace(ref state.atook, ATookSplit[i]);
            }


            //BHand
            string BHandLine = sr.ReadLine();
            string[] BHandSplit = BHandLine.Split(' ');
            if (BHandSplit.Length > 1)
            {
                AddToCardPlace(ref state.b1, BHandSplit[1]);
            }
            if (BHandSplit.Length > 2)
            {
                AddToCardPlace(ref state.b2, BHandSplit[2]);
            }
            if (BHandSplit.Length > 3)
            {
                AddToCardPlace(ref state.b3, BHandSplit[3]);
            }
            if (BHandSplit.Length > 4)
            {
                AddToCardPlace(ref state.b4, BHandSplit[4]);
            }
            if (BHandSplit.Length > 5)
            {
                AddToCardPlace(ref state.b5, BHandSplit[5]);
            }


            //Bdown
            string BDownLine = sr.ReadLine();
            string[] BDownSplit = BDownLine.Split(' ');
            if (BDownSplit.Length > 1)
            {
                AddToCardPlace(ref state.bdown, BDownSplit[1]);
            }


            //Btook
            string BTookLine = sr.ReadLine();
            string[] BTookSplit = BTookLine.Split(' ');
            for (int i = 1; i < BTookSplit.Length; i++)
            {
                AddToCardPlace(ref state.btook, BTookSplit[i]);
            }

            //20/40
            string A20Line = sr.ReadLine();
            List<string> A20Split = A20Line.Split(' ').ToList();

            if(A20Split.Contains("M"))
            {
                state.AM20 = true;
            }
            if (A20Split.Contains("P"))
            {
                state.AP20 = true;
            }
            if (A20Split.Contains("T"))
            {
                state.AT20 = true;
            }
            if (A20Split.Contains("Z"))
            {
                state.AZ20 = true;
            }

            string B20Line = sr.ReadLine();
            List<string> B20Split = B20Line.Split(' ').ToList();

            if (B20Split.Contains("M"))
            {
                state.BM20 = true;
            }
            if (B20Split.Contains("P"))
            {
                state.BP20 = true;
            }
            if (B20Split.Contains("T"))
            {
                state.BT20 = true;
            }
            if (B20Split.Contains("Z"))
            {
                state.BZ20 = true;
            }

            //Next
            string nextLine = sr.ReadLine();
            string[] nextSplit = nextLine.Split(' ');
            state.isAnext = nextSplit[1]=="A";


            //Trump
            string trumpLine = sr.ReadLine();
            string[] trumpSplit = trumpLine.Split(' ');
            state.trump = trumpSplit[1][0];

            sr.Close();

            return state;
        }
    }
}
