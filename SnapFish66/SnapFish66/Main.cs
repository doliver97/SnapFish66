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
        public static List<Card> cards;

        public State state;

        public Card emptyCard = new Card("empty", Properties.Resources.Empty);

        public static bool running = false;

        public List<Label> labels;


        public Main()
        {
            InitializeComponent();
            cards = new List<Card>();
            InitCards();
            state = new State();
            state.next = "A";
            InitCardPlaces();

            labels = new List<Label>
            {
                A1_l,
                A2_l,
                A3_l,
                A4_l,
                A5_l,
                B1_l,
                B2_l,
                B3_l,
                B4_l,
                B5_l,
                cover_l
            };
        }

        private void InitCardPlaces()
        {

            A1_pb.Image = Properties.Resources.Empty;
            A2_pb.Image = Properties.Resources.Empty;
            A3_pb.Image = Properties.Resources.Empty;
            A4_pb.Image = Properties.Resources.Empty;
            A5_pb.Image = Properties.Resources.Empty;
            B1_pb.Image = Properties.Resources.Empty;
            B2_pb.Image = Properties.Resources.Empty;
            B3_pb.Image = Properties.Resources.Empty;
            B4_pb.Image = Properties.Resources.Empty;
            B5_pb.Image = Properties.Resources.Empty;
            Deck_pb.Image = Properties.Resources.back;
            Dbottom_pb.Image = Properties.Resources.Empty;
            Atook_pb.Image = Properties.Resources.Empty;
            Btook_pb.Image = Properties.Resources.Empty;
            Adown_pb.Image = Properties.Resources.Empty;
            Bdown_pb.Image = Properties.Resources.Empty;
        }

        private void InitCards()
        {
            cards = new List<Card>();

            List<string> IDs = new List<string> { "M2", "M3", "M4", "M10", "M11", "P2", "P3", "P4", "P10", "P11", "T2", "T3", "T4", "T10", "T11", "Z2", "Z3", "Z4", "Z10", "Z11"};
            List<Bitmap> images = new List<Bitmap> { Properties.Resources.M2, Properties.Resources.M3, Properties.Resources.M4, Properties.Resources.M10,Properties.Resources.M11, Properties.Resources.P2, Properties.Resources.P3, Properties.Resources.P4, Properties.Resources.P10, Properties.Resources.P11, Properties.Resources.T2, Properties.Resources.T3, Properties.Resources.T4, Properties.Resources.T10, Properties.Resources.T11, Properties.Resources.Z2, Properties.Resources.Z3, Properties.Resources.Z4, Properties.Resources.Z10, Properties.Resources.Z11 };

            for (int i = 0; i < IDs.Count; i++)
            {
                cards.Add(new Card(IDs[i],images[i]));
            }
        }

        private void AddCards(List<Card> target)
        {
            target.Clear();
            CardSelector cs = new CardSelector();
            cs.ShowDialog();
            List<string> selected = cs.GetSelectedCards();

            if(selected.Count>0 && selected[0]=="unknown")
            {
                Card u = new Card("unknown",Properties.Resources.back);
                target.Add(u);
                return;
            }

            foreach (string id in selected)
            {
                target.Add(cards.Find(x => x.ID == id));
            }
        }
        

        

        private void Main_Load(object sender, EventArgs e)
        {

        }

        private void M_rb_CheckedChanged(object sender, EventArgs e)
        {
            if (M_rb.Checked)
            {
                P_rb.Checked = false;
                T_rb.Checked = false;
                Z_rb.Checked = false;

                state.trump = "M";
            }
        }

        private void P_rb_CheckedChanged(object sender, EventArgs e)
        {
            if (P_rb.Checked)
            {
                M_rb.Checked = false;
                T_rb.Checked = false;
                Z_rb.Checked = false;

                state.trump = "P";
            }
        }

        private void T_rb_CheckedChanged(object sender, EventArgs e)
        {
            if (T_rb.Checked)
            {
                M_rb.Checked = false;
                P_rb.Checked = false;
                Z_rb.Checked = false;

                state.trump = "T";
            }
        }

        private void Z_rb_CheckedChanged(object sender, EventArgs e)
        {
            if (Z_rb.Checked)
            {
                M_rb.Checked = false;
                P_rb.Checked = false;
                T_rb.Checked = false;

                state.trump = "Z";
            }
        }

        private void Check_btn_Click(object sender, EventArgs e)
        {
            string message = state.Check();
            state.CalculatePoints();
            Ascore_l.Text = state.Ascore.ToString();
            Bscore_l.Text = state.Bscore.ToString();

            OK_l.Text = message;
        }

        private void B_rb_CheckedChanged(object sender, EventArgs e)
        {
            if(B_rb.Checked)
            {
                A_rb.Checked = false;
                state.next = "B";
            }
        }

        private void A_rb_CheckedChanged(object sender, EventArgs e)
        {
            if (A_rb.Checked)
            {
                B_rb.Checked = false;
                state.next = "A";
            }
        }

        private void BM20_cb_CheckedChanged(object sender, EventArgs e)
        {
            state.BM20 = BM20_cb.Checked;
        }

        private void BP20_cb_CheckedChanged(object sender, EventArgs e)
        {
            state.BP20 = BP20_cb.Checked;
        }

        private void BT20_cb_CheckedChanged(object sender, EventArgs e)
        {
            state.BT20 = BT20_cb.Checked;
        }

        private void BZ20_cb_CheckedChanged(object sender, EventArgs e)
        {
            state.BZ20 = BZ20_cb.Checked;
        }

        private void AM20_cb_CheckedChanged(object sender, EventArgs e)
        {
            state.AM20 = AM20_cb.Checked;
        }

        private void AP20_cb_CheckedChanged(object sender, EventArgs e)
        {
            state.AP20 = AP20_cb.Checked;
        }

        private void AT20_cb_CheckedChanged(object sender, EventArgs e)
        {
            state.AT20 = AT20_cb.Checked;
        }

        private void AZ20_cb_CheckedChanged(object sender, EventArgs e)
        {
            state.AZ20 = AZ20_cb.Checked;
        }

        

        private void Deck_pb_Click(object sender, EventArgs e)
        {
            AddCards(state.deck);
            if (state.deck.Count > 0)
            {
                //Deck_pb.Image = state.deck[0].image;
                Deck_pb.Image = Properties.Resources.back;
            }
            else
            {
                Deck_pb.Image = Properties.Resources.Empty;
            }
        }

        private void B1_pb_Click(object sender, EventArgs e)
        {
            AddCards(state.b1);
            if (state.b1.Count > 0)
            {
                B1_pb.Image = state.b1[0].image;
            }
            else
            {
                B1_pb.Image = Properties.Resources.Empty;
            }
        }

        private void B2_pb_Click(object sender, EventArgs e)
        {
            AddCards(state.b2);
            if (state.b2.Count > 0)
            {
                B2_pb.Image = state.b2[0].image;
            }
            else
            {
                B2_pb.Image = Properties.Resources.Empty;
            }
        }

        private void B3_pb_Click(object sender, EventArgs e)
        {
            AddCards(state.b3);
            if (state.b3.Count > 0)
            {
                B3_pb.Image = state.b3[0].image;
            }
            else
            {
                B3_pb.Image = Properties.Resources.Empty;
            }
        }

        private void B4_pb_Click(object sender, EventArgs e)
        {
            AddCards(state.b4);
            if (state.b4.Count > 0)
            {
                B4_pb.Image = state.b4[0].image;
            }
            else
            {
                B4_pb.Image = Properties.Resources.Empty;
            }
        }

        private void B5_pb_Click(object sender, EventArgs e)
        {
            AddCards(state.b5);
            if (state.b5.Count > 0)
            {
                B5_pb.Image = state.b5[0].image;
            }
            else
            {
                B5_pb.Image = Properties.Resources.Empty;
            }
        }

        private void Btook_pb_Click(object sender, EventArgs e)
        {
            AddCards(state.btook);
            if (state.btook.Count > 0)
            {
                Btook_pb.Image = Properties.Resources.back;
            }
            else
            {
                Btook_pb.Image = Properties.Resources.Empty;
            }
        }

        private void Bdown_pb_Click(object sender, EventArgs e)
        {
            AddCards(state.bdown);
            if (state.bdown.Count > 0)
            {
                Bdown_pb.Image = state.bdown[0].image;
            }
            else
            {
                Bdown_pb.Image = Properties.Resources.Empty;
            }
        }

        private void Adown_pb_Click(object sender, EventArgs e)
        {
            AddCards(state.adown);
            if (state.adown.Count > 0)
            {
                Adown_pb.Image = state.adown[0].image;
            }
            else
            {
                Adown_pb.Image = Properties.Resources.Empty;
            }
        }

        private void A1_pb_Click(object sender, EventArgs e)
        {
            AddCards(state.a1);
            if (state.a1.Count > 0)
            {
                A1_pb.Image = state.a1[0].image;
            }
            else
            {
                A1_pb.Image = Properties.Resources.Empty;
            }


        }

        private void A2_pb_Click(object sender, EventArgs e)
        {
            AddCards(state.a2);
            if (state.a2.Count > 0)
            {
                A2_pb.Image = state.a2[0].image;
            }
            else
            {
                A2_pb.Image = Properties.Resources.Empty;
            }
        }

        private void A3_pb_Click(object sender, EventArgs e)
        {
            AddCards(state.a3);
            if (state.a3.Count > 0)
            {
                A3_pb.Image = state.a3[0].image;
            }
            else
            {
                A3_pb.Image = Properties.Resources.Empty;
            }
        }

        private void A4_pb_Click(object sender, EventArgs e)
        {
            AddCards(state.a4);
            if (state.a4.Count > 0)
            {
                A4_pb.Image = state.a4[0].image;
            }
            else
            {
                A4_pb.Image = Properties.Resources.Empty;
            }
        }

        private void A5_pb_Click(object sender, EventArgs e)
        {
            AddCards(state.a5);
            if (state.a5.Count > 0)
            {
                A5_pb.Image = state.a5[0].image;
            }
            else
            {
                A5_pb.Image = Properties.Resources.Empty;
            }
        }

        private void Atook_pb_Click(object sender, EventArgs e)
        {
            AddCards(state.atook);
            if (state.atook.Count > 0)
            {
                Atook_pb.Image = Properties.Resources.back;
            }
            else
            {
                Atook_pb.Image = Properties.Resources.Empty;
            }
        }

        private void Dbottom_pb_Click(object sender, EventArgs e)
        {
            AddCards(state.dbottom);
            if (state.dbottom.Count > 0)
            {
                Dbottom_pb.Image = state.dbottom[0].image;

                //Set trump radiobutton automatically
                if (state.dbottom[0].ID[0] == 'M')
                {
                    M_rb.Checked = true;
                }
                if (state.dbottom[0].ID[0] == 'P')
                {
                    P_rb.Checked = true;
                }
                if (state.dbottom[0].ID[0] == 'T')
                {
                    T_rb.Checked = true;
                }
                if (state.dbottom[0].ID[0] == 'Z')
                {
                    Z_rb.Checked = true;
                }
            }
            else
            {
                Dbottom_pb.Image = Properties.Resources.Empty;
            }
        }

        private void covered_cb_CheckedChanged(object sender, EventArgs e)
        {
            state.covered = covered_cb.Checked;
        }

        private void Start_btn_Click(object sender, EventArgs e)
        {
            if(running)
            {
                Start_btn.Text = "START";
                running = false;
            }
            else
            {
                Start_btn.Text = "STOP";
                running = true;
                GameTree tree = new GameTree(state, progressBar, NodesDataGridView);
                tree.Calculate(labels);
            }
        }

        private void Test_btn_Click(object sender, EventArgs e)
        {
            if(testFileSelector.SelectedItem!=null)
            {
                State s = TestFileHandler.CreateState(cards,(string)testFileSelector.SelectedItem);
                SetState(s);
            }

        }

        private void testFileSelector_Click(object sender, EventArgs e)
        {
            testFileSelector.Items.Clear();
            testFileSelector.Items.AddRange(TestFileHandler.GetTrimmedFileNames());
        }

        private void SetImage(PictureBox pb, List<Card> place)
        {
            if(place.Count>0)
            {
                pb.Image = place[0].image;
            }
            else
            {
                pb.Image = Properties.Resources.Empty;
            }
        }

        public void SetState(State newState)
        {
            state = newState;

            //Set dbottom
            if (state.dbottom.Count > 0)
            {
                Dbottom_pb.Image = state.dbottom[0].image;

                //Set trump radiobutton automatically
                if (state.dbottom[0].ID[0] == 'M')
                {
                    M_rb.Checked = true;
                    state.trump = "M";
                }
                if (state.dbottom[0].ID[0] == 'P')
                {
                    P_rb.Checked = true;
                    state.trump = "P";
                }
                if (state.dbottom[0].ID[0] == 'T')
                {
                    T_rb.Checked = true;
                    state.trump = "T";
                }
                if (state.dbottom[0].ID[0] == 'Z')
                {
                    Z_rb.Checked = true;
                    state.trump = "Z";
                }
            }
            else
            {
                Dbottom_pb.Image = Properties.Resources.Empty;
            }

            //Set the images of everything else

            Deck_pb.Image = Properties.Resources.back; //now constant

            SetImage(A1_pb,state.a1);
            SetImage(A2_pb, state.a2);
            SetImage(A3_pb, state.a3);
            SetImage(A4_pb, state.a4);
            SetImage(A5_pb, state.a5);
            SetImage(B1_pb, state.b1);
            SetImage(B2_pb, state.b2);
            SetImage(B3_pb, state.b3);
            SetImage(B4_pb, state.b4);
            SetImage(B5_pb, state.b5);
            SetImage(Adown_pb, state.adown);
            SetImage(Bdown_pb, state.bdown);

            if(state.atook.Count>0)
            {
                Atook_pb.Image = Properties.Resources.back;
            }
            else
            {
                Atook_pb.Image = Properties.Resources.Empty;
            }

            if (state.btook.Count > 0)
            {
                Btook_pb.Image = Properties.Resources.back;
            }
            else
            {
                Btook_pb.Image = Properties.Resources.Empty;
            }

            //Set 20/40
            AM20_cb.Checked = state.AM20;
            AP20_cb.Checked = state.AP20;
            AT20_cb.Checked = state.AT20;
            AZ20_cb.Checked = state.AZ20;
            BM20_cb.Checked = state.BM20;
            BP20_cb.Checked = state.BP20;
            BT20_cb.Checked = state.BT20;
            BZ20_cb.Checked = state.BZ20;


            //Set next radiobutton
            if (state.next=="A")
            {
                A_rb.Checked = true;
            }
            else
            {
                B_rb.Checked = true;
            }
        }
    }
}
