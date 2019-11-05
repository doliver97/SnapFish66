using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GUI
{
    /// <summary>
    /// Interaction logic for CardSelectWindow.xaml
    /// </summary>
    public partial class CardSelectWindow : Window
    {
        public List<bool> selectedCards;

        private readonly string mode;

        public CardSelectWindow(List<bool> selectedCards, string mode)
        {
            InitializeComponent();

            this.mode = mode;
            this.selectedCards = selectedCards;

            Dictionary<Image, bool> selected = new Dictionary<Image, bool>
            {
                [M2_img] = selectedCards[0],
                [M3_img] = selectedCards[1],
                [M4_img] = selectedCards[2],
                [M10_img] = selectedCards[3],
                [M11_img] = selectedCards[4],
                [P2_img] = selectedCards[5],
                [P3_img] = selectedCards[6],
                [P4_img] = selectedCards[7],
                [P10_img] = selectedCards[8],
                [P11_img] = selectedCards[9],
                [T2_img] = selectedCards[10],
                [T3_img] = selectedCards[11],
                [T4_img] = selectedCards[12],
                [T10_img] = selectedCards[13],
                [T11_img] = selectedCards[14],
                [Z2_img] = selectedCards[15],
                [Z3_img] = selectedCards[16],
                [Z4_img] = selectedCards[17],
                [Z10_img] = selectedCards[18],
                [Z11_img] = selectedCards[19]
            };

            foreach (KeyValuePair<Image, bool> entry in selected)
            {
                if(entry.Value == true)
                {
                    entry.Key.Opacity = 1;
                }
                else
                {
                    entry.Key.Opacity = 0.4;
                }
            }
        }

        public string Check()
        {
            int selectedNum = selectedCards.Count(x => x == true);

            if(mode == "Single" && selectedNum > 1)
            {
                return "Error: more than one card is selected!";
            }
            else if (mode == "Double" && (selectedNum % 2 == 1))
            {
                return "Error: odd number of cards are selected!";
            }
            else if (mode == "1to5" && (selectedNum < 1 || selectedNum > 5))
            {
                return "Error: the hand must contain 1-5 cards!";
            }

            return "OK";
        }

        private void OK_btn_Click(object sender, RoutedEventArgs e)
        {
            string errorMessage = Check();
            if (errorMessage == "OK")
            {
                Close();
            }
            else
            {
                Error_lbl.Text = errorMessage;
            }
        }

        private void M2_Click(object sender, RoutedEventArgs e)
        {
            selectedCards[0] = !selectedCards[0];

            if (selectedCards[0])
            {
                M2_img.Opacity = 1;
            }
            else
            {
                M2_img.Opacity = 0.4;
            }
        }

        private void M3_Click(object sender, RoutedEventArgs e)
        {
            selectedCards[1] = !selectedCards[1];

            if (selectedCards[1])
            {
                M3_img.Opacity = 1;
            }
            else
            {
                M3_img.Opacity = 0.4;
            }
        }

        private void M4_Click(object sender, RoutedEventArgs e)
        {
            selectedCards[2] = !selectedCards[2];

            if (selectedCards[2])
            {
                M4_img.Opacity = 1;
            }
            else
            {
                M4_img.Opacity = 0.4;
            }
        }

        private void M10_Click(object sender, RoutedEventArgs e)
        {
            selectedCards[3] = !selectedCards[3];

            if (selectedCards[3])
            {
                M10_img.Opacity = 1;
            }
            else
            {
                M10_img.Opacity = 0.4;
            }
        }

        private void M11_Click(object sender, RoutedEventArgs e)
        {
            selectedCards[4] = !selectedCards[4];

            if (selectedCards[4])
            {
                M11_img.Opacity = 1;
            }
            else
            {
                M11_img.Opacity = 0.4;
            }
        }

        private void P2_Click(object sender, RoutedEventArgs e)
        {
            selectedCards[5] = !selectedCards[5];

            if (selectedCards[5])
            {
                P2_img.Opacity = 1;
            }
            else
            {
                P2_img.Opacity = 0.4;
            }
        }

        private void P3_Click(object sender, RoutedEventArgs e)
        {
            selectedCards[6] = !selectedCards[6];

            if (selectedCards[6])
            {
                P3_img.Opacity = 1;
            }
            else
            {
                P3_img.Opacity = 0.4;
            }
        }

        private void P4_Click(object sender, RoutedEventArgs e)
        {
            selectedCards[7] = !selectedCards[7];

            if (selectedCards[7])
            {
                P4_img.Opacity = 1;
            }
            else
            {
                P4_img.Opacity = 0.4;
            }
        }

        private void P10_Click(object sender, RoutedEventArgs e)
        {
            selectedCards[8] = !selectedCards[8];

            if (selectedCards[8])
            {
                P10_img.Opacity = 1;
            }
            else
            {
                P10_img.Opacity = 0.4;
            }
        }

        private void P11_Click(object sender, RoutedEventArgs e)
        {
            selectedCards[9] = !selectedCards[9];

            if (selectedCards[9])
            {
                P11_img.Opacity = 1;
            }
            else
            {
                P11_img.Opacity = 0.4;
            }
        }

        private void T2_Click(object sender, RoutedEventArgs e)
        {
            selectedCards[10] = !selectedCards[10];

            if (selectedCards[10])
            {
                T2_img.Opacity = 1;
            }
            else
            {
                T2_img.Opacity = 0.4;
            }
        }

        private void T3_Click(object sender, RoutedEventArgs e)
        {
            selectedCards[11] = !selectedCards[11];

            if (selectedCards[11])
            {
                T3_img.Opacity = 1;
            }
            else
            {
                T3_img.Opacity = 0.4;
            }
        }

        private void T4_Click(object sender, RoutedEventArgs e)
        {
            selectedCards[12] = !selectedCards[12];

            if (selectedCards[12])
            {
                T4_img.Opacity = 1;
            }
            else
            {
                T4_img.Opacity = 0.4;
            }
        }

        private void T10_Click(object sender, RoutedEventArgs e)
        {
            selectedCards[13] = !selectedCards[13];

            if (selectedCards[13])
            {
                T10_img.Opacity = 1;
            }
            else
            {
                T10_img.Opacity = 0.4;
            }
        }

        private void T11_Click(object sender, RoutedEventArgs e)
        {
            selectedCards[14] = !selectedCards[14];

            if (selectedCards[14])
            {
                T11_img.Opacity = 1;
            }
            else
            {
                T11_img.Opacity = 0.4;
            }
        }

        private void Z2_Click(object sender, RoutedEventArgs e)
        {
            selectedCards[15] = !selectedCards[15];

            if (selectedCards[15])
            {
                Z2_img.Opacity = 1;
            }
            else
            {
                Z2_img.Opacity = 0.4;
            }
        }

        private void Z3_Click(object sender, RoutedEventArgs e)
        {
            selectedCards[16] = !selectedCards[16];

            if (selectedCards[16])
            {
                Z3_img.Opacity = 1;
            }
            else
            {
                Z3_img.Opacity = 0.4;
            }
        }

        private void Z4_Click(object sender, RoutedEventArgs e)
        {
            selectedCards[17] = !selectedCards[17];

            if (selectedCards[17])
            {
                Z4_img.Opacity = 1;
            }
            else
            {
                Z4_img.Opacity = 0.4;
            }
        }

        private void Z10_Click(object sender, RoutedEventArgs e)
        {
            selectedCards[18] = !selectedCards[18];

            if (selectedCards[18])
            {
                Z10_img.Opacity = 1;
            }
            else
            {
                Z10_img.Opacity = 0.4;
            }
        }

        private void Z11_Click(object sender, RoutedEventArgs e)
        {
            selectedCards[19] = !selectedCards[19];

            if (selectedCards[19])
            {
                Z11_img.Opacity = 1;
            }
            else
            {
                Z11_img.Opacity = 0.4;
            }
        }
    }
}
