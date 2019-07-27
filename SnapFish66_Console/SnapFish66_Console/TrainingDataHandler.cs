using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace SnapFish66_Console
{
    class TrainingDataHandler
    {
        private static StreamWriter sw;

        public static void Init()
        {
            sw = new StreamWriter("TrainingData.csv");
        }

        public static void Write(string stateString, List<double> values)
        {
            foreach (char ch in stateString)
            {
                sw.Write(ch + ",");
            }
            for (int i = 0; i < values.Count; i++)
            {
                //Change separator from , to .
                NumberFormatInfo nfi = new NumberFormatInfo
                {
                    NumberDecimalSeparator = "."
                };
                sw.Write(values[i].ToString(nfi));
                if(i!=values.Count-1)
                {
                    sw.Write(",");
                }
            }
            sw.WriteLine();
        }

        public static void Close()
        {
            sw.Close();
        }
    }
}
