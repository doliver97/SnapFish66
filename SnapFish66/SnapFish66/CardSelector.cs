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
    public partial class CardSelector : Form
    {
        private List<string> selectedCards;
        public CardSelector(List<Card> cards)
        {
            InitializeComponent();
            selectedCards = new List<string>();

            SetSelectedCards(cards);
        }

        public void SetSelectedCards(List<Card> cards)
        {
            List<string> IDs = new List<string> { "M2", "M3", "M4", "M10", "M11", "P2", "P3", "P4", "P10", "P11", "T2", "T3", "T4", "T10", "T11", "Z2", "Z3", "Z4", "Z10", "Z11" };
            List<CheckBox> checkboxes = new List<CheckBox> { checkBoxM2, checkBoxM3, checkBoxM4, checkBoxM10, checkBoxM11, checkBoxP2, checkBoxP3, checkBoxP4, checkBoxP10, checkBoxP11, checkBoxT2, checkBoxT3, checkBoxT4, checkBoxT10, checkBoxT11, checkBoxZ2, checkBoxZ3, checkBoxZ4, checkBoxZ10, checkBoxZ11};

            foreach(Card card in cards)
            {
                for (int i = 0; i < IDs.Count; i++)
                {
                    if(card.ID==IDs[i])
                    {
                        //It adds the string to selectedCards list
                        checkboxes[i].Checked = true;
                    }
                }
            }
        }

        public List<string> GetSelectedCards()
        {
            return selectedCards;
        }

        private void checkBoxM2_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBoxM2.Checked)
            {
                selectedCards.Add("M2");
            }
            else
            {
                selectedCards.Remove("M2");
            }
        }

        private void checkBoxM3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxM3.Checked)
            {
                selectedCards.Add("M3");
            }
            else
            {
                selectedCards.Remove("M3");
            }
        }

        private void checkBoxM4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxM4.Checked)
            {
                selectedCards.Add("M4");
            }
            else
            {
                selectedCards.Remove("M4");
            }
        }

        private void checkBoxM10_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxM10.Checked)
            {
                selectedCards.Add("M10");
            }
            else
            {
                selectedCards.Remove("M10");
            }
        }

        private void checkBoxM11_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxM11.Checked)
            {
                selectedCards.Add("M11");
            }
            else
            {
                selectedCards.Remove("M11");
            }
        }


        private void checkBoxP2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxP2.Checked)
            {
                selectedCards.Add("P2");
            }
            else
            {
                selectedCards.Remove("P2");
            }
        }

        private void checkBoxP3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxP3.Checked)
            {
                selectedCards.Add("P3");
            }
            else
            {
                selectedCards.Remove("P3");
            }
        }

        private void checkBoxP4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxP4.Checked)
            {
                selectedCards.Add("P4");
            }
            else
            {
                selectedCards.Remove("P4");
            }
        }

        private void checkBoxP10_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxP10.Checked)
            {
                selectedCards.Add("P10");
            }
            else
            {
                selectedCards.Remove("P10");
            }
        }

        private void checkBoxP11_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxP11.Checked)
            {
                selectedCards.Add("P11");
            }
            else
            {
                selectedCards.Remove("P11");
            }
        }


        private void checkBoxT2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxT2.Checked)
            {
                selectedCards.Add("T2");
            }
            else
            {
                selectedCards.Remove("T2");
            }
        }

        private void checkBoxT3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxT3.Checked)
            {
                selectedCards.Add("T3");
            }
            else
            {
                selectedCards.Remove("T3");
            }
        }

        private void checkBoxT4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxT4.Checked)
            {
                selectedCards.Add("T4");
            }
            else
            {
                selectedCards.Remove("T4");
            }
        }

        private void checkBoxT10_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxT10.Checked)
            {
                selectedCards.Add("T10");
            }
            else
            {
                selectedCards.Remove("T10");
            }
        }

        private void checkBoxT11_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxT11.Checked)
            {
                selectedCards.Add("T11");
            }
            else
            {
                selectedCards.Remove("T11");
            }
        }


        private void checkBoxZ2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxZ2.Checked)
            {
                selectedCards.Add("Z2");
            }
            else
            {
                selectedCards.Remove("Z2");
            }
        }

        private void checkBoxZ3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxZ3.Checked)
            {
                selectedCards.Add("Z3");
            }
            else
            {
                selectedCards.Remove("Z3");
            }
        }

        private void checkBoxZ4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxZ4.Checked)
            {
                selectedCards.Add("Z4");
            }
            else
            {
                selectedCards.Remove("Z4");
            }
        }

        private void checkBoxZ10_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxZ10.Checked)
            {
                selectedCards.Add("Z10");
            }
            else
            {
                selectedCards.Remove("Z10");
            }
        }

        private void checkBoxZ11_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxZ11.Checked)
            {
                selectedCards.Add("Z11");
            }
            else
            {
                selectedCards.Remove("Z11");
            }
        }

        private void buttonEmpty_Click(object sender, EventArgs e)
        {
            selectedCards = new List<string>();
            DialogResult = DialogResult.OK;
            Close();
        }

        private void buttonUnknown_Click(object sender, EventArgs e)
        {
            selectedCards = new List<string>
            {
                "unknown"
            };
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btn_OK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
