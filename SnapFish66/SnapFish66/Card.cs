using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnapFish66
{
    public class Card
    {
        public Image image;
        public string ID;
        public string color;

        public Card()
        {
            image = new Bitmap(Properties.Resources.Empty);
            ID = "empty";
            color = "";
        }

        public Card(string id, Image img)
        {
            ID = id;
            image = img;
            color = id.Substring(0, 1);
        }

        public bool IsSame(Card other)
        {
            return this.ID == other.ID;
        }

        public int GetValue()
        {
            if(ID=="M2" || ID == "P2" || ID == "T2" || ID == "Z2")
            {
                return 2;
            }
            if (ID == "M3" || ID == "P3" || ID == "T3" || ID == "Z3")
            {
                return 3;
            }
            if (ID == "M4" || ID == "P4" || ID == "T4" || ID == "Z4")
            {
                return 4;
            }
            if (ID == "M10" || ID == "P10" || ID == "T10" || ID == "Z10")
            {
                return 10;
            }
            if (ID == "M11" || ID == "P11" || ID == "T11" || ID == "Z11")
            {
                return 11;
            }

            return 0;
        }
    }
}
