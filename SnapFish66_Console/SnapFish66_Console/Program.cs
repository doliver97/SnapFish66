using Calculate;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;

namespace SnapFish66_Console
{
    class Program
    {
        static void Main()
        {
            //string s = "APDUAGUUAGGAUHHAHUGHAUXXXX";
            //string s = "APUUUUADAAAAUUUUUUUUUUXXXX"; //Best starting hand
            string s = "APAUAUUDUUHGHGUUAAAGUGXXXX"; //test 3deck
            //string s = "AMAAAUADUUHGHUUGAUUHUHXXXX"; //other 3deck
            //string s = "APAUAUGDUUHGUUUUAAAUUHAXAB";  // A 5 deck state
            //string s = "APAUAUUDUUUGUGUUAAAUUUXXXX";  // A 7 deck state
            //string s = "APAUAUUDUUUUUUUUAAAUUUXXXX";  // A 9 deck state
            //string s = "APHHHUADAAAAHHUUUUHHUHXXXX"; // Test 8 , result must be +2 for all
            //string s = "AMHGGUHAHHGGGHGAUUHGGAXXXX"; //Test 5, result must be -1 for all
            //string s = "APHGHGADAUUUUUAUUFUAUAXXXX"; //A has 20 -> CONFIRMED: bigger card of 20 is always best option!
            //string s = "APHHHUADAAAAHHUUFUHHUHXXXX";

            //Setting mode
            //Calculate.Training();

            BackgroundWorker worker = new BackgroundWorker();
            Calculator.Single(s, worker);
            worker.ProgressChanged += ConsoleWriter.WriteDataToConsole;
            worker.RunWorkerAsync();

            while(worker.IsBusy)
            {
                Thread.Sleep(1000);
            }
        }
    }
}
