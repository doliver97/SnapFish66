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
        public string ID;
        public string color;
        public int value;

        public Card()
        {
            ID = "empty";
            color = "";
            value = 0;
        }

        public Card(string id)
        {
            ID = id;
            color = id.Substring(0, 1);
            value = int.Parse(id.Substring(1));
        }
    }
}
