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
        public char color;
        public byte value;

        //Works like an enum flag
        public int index;

        public Card(string id)
        {
            ID = id;
            color = id[0];
            if (ID != "unknown")
            {
                value = byte.Parse(id.Substring(1));
            }
        }


    }
}
