using Calculate;
using System;
using System.Collections.Generic;

namespace SnapFish66_Console
{
    class Program
    {

        static void Main()
        {
            Calculator.SetStaticCards();

            //Setting mode
            //Calculate.Training();
            Calculator.Single();

            Console.WriteLine();
            Console.WriteLine("Exit with enter...");

            Console.ReadLine();
        }
    }
}
