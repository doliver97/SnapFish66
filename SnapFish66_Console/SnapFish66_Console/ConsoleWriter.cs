using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Calculate;

namespace SnapFish66_Console
{
    public static class ConsoleWriter
    {
        public static object lockObject = new object();

        public static void WriteDataToConsole(object sender, ProgressChangedEventArgs e)
        {
            lock (lockObject)
            {
                ReturnObject returnObject = (ReturnObject)e.UserState;

                Console.Clear();
                Console.Write("Card permutations: ");
                Console.WriteLine(returnObject.calculatedGames + "/" + returnObject.allGames);

                Console.WriteLine();
                Console.WriteLine("Estimated values for cards:");

                if (returnObject.a1Name != "")
                {
                    Console.WriteLine(returnObject.a1Name + " : " + returnObject.a1Value);
                }
                if (returnObject.a2Name != "")
                {
                    Console.WriteLine(returnObject.a2Name + " : " + returnObject.a2Value);
                }
                if (returnObject.a3Name != "")
                {
                    Console.WriteLine(returnObject.a3Name + " : " + returnObject.a3Value);
                }
                if (returnObject.a4Name != "")
                {
                    Console.WriteLine(returnObject.a4Name + " : " + returnObject.a4Value);
                }
                if (returnObject.a5Name != "")
                {
                    Console.WriteLine(returnObject.a5Name + " : " + returnObject.a5Value);
                }
            }
        }
    }
}
