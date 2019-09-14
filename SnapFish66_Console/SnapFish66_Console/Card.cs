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
        public byte value;
        public int index;

        public Card(string id)
        {
            ID = id;
            color = id.Substring(0, 1);
            if (ID != "unknown")
            {
                value = byte.Parse(id.Substring(1));
            }
        }


    }
}
