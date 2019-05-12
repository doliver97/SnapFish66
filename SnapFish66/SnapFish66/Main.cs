using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnapFish66
{
    public partial class Main : Form
    {
        public List<Card> cards;

        public State state;


        public Main()
        {
            InitializeComponent();
            InitCards();
            state = new State();
        }

        private void InitCards()
        {
            List<string> IDs = new List<string> { "M2", "M3", "M4", "M10", "M11", "P2", "P3", "P4", "P10", "P11", "T2", "T3", "T4", "T10", "T11", "Z2", "Z3", "Z4", "Z10", "Z11"};
            List<Bitmap> images = new List<Bitmap> { Properties.Resources.M2, Properties.Resources.M3, Properties.Resources.M4, Properties.Resources.M10,Properties.Resources.M11, Properties.Resources.P2, Properties.Resources.P3, Properties.Resources.P4, Properties.Resources.P10, Properties.Resources.P11, Properties.Resources.T2, Properties.Resources.T3, Properties.Resources.T4, Properties.Resources.T10, Properties.Resources.T11, Properties.Resources.Z2, Properties.Resources.Z3, Properties.Resources.Z4, Properties.Resources.Z10, Properties.Resources.Z11 };

            for (int i = 0; i < IDs.Count; i++)
            {
                cards.Add(new Card(IDs[i],images[i]));
            }
        }
        

        private void Dbottom_pb_Click(object sender, EventArgs e)
        {

        }

        private void Main_Load(object sender, EventArgs e)
        {

        }
        
    }
}
