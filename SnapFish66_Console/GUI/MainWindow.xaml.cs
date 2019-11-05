using Calculate;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<string> picturePaths = new List<string>()
        {
            "images/M2.jpg",
            "images/M3.jpg",
            "images/M4.jpg",
            "images/M10.jpg",
            "images/M11.jpg",
            "images/P2.jpg",
            "images/P3.jpg",
            "images/P4.jpg",
            "images/P10.jpg",
            "images/P11.jpg",
            "images/T2.jpg",
            "images/T3.jpg",
            "images/T4.jpg",
            "images/T10.jpg",
            "images/T11.jpg",
            "images/Z2.jpg",
            "images/Z3.jpg",
            "images/Z4.jpg",
            "images/Z10.jpg",
            "images/Z11.jpg"
        };

        private BitmapImage empty = new BitmapImage(new Uri("images/Empty.png", UriKind.Relative));
        private BitmapImage back = new BitmapImage(new Uri("images/back.png", UriKind.Relative));

        private BackgroundWorker worker;

        public MainWindow()
        {
            InitializeComponent();
            SetCards();
        }

        private void SetCards()
        {
            string checkString = Validator.Validate(txtState.Text);
            if (checkString != "OK")
            {
                error_lbl.Text = checkString;
                return;
            }
            else
            {
                error_lbl.Text = "";
            }

            List<Image> aPlaces = new List<Image>() { a1_pic, a2_pic, a3_pic, a4_pic, a5_pic };
            List<Image> bPlaces = new List<Image>() { b1_pic, b2_pic, b3_pic, b4_pic, b5_pic };

            int aCount = 0;
            int bCount = 0;

            foreach (Image place in aPlaces)
            {
                place.Source = empty;
            }
            foreach (Image place in bPlaces)
            {
                place.Source = empty;
            }
            bottom_pic.Source = empty;
            f_pic.Source = empty;
            atook_pic.Source = empty;
            btook_pic.Source = empty;

            for (int i = 0; i < 20; i++)
            {
                Uri uriSource = new Uri(picturePaths[i], UriKind.Relative);

                if (txtState.Text[i + 2] == 'A')
                {
                    aPlaces[aCount].Source = new BitmapImage(uriSource);
                    aCount++;
                }
                else if (txtState.Text[i + 2] == 'B')
                {
                    bPlaces[bCount].Source = new BitmapImage(uriSource);
                    bCount++;
                }
                else if (txtState.Text[i + 2] == 'D')
                {
                    bottom_pic.Source = new BitmapImage(uriSource);
                }
                else if (txtState.Text[i + 2] == 'F')
                {
                    f_pic.Source = new BitmapImage(uriSource);
                }
                else if(txtState.Text[i + 2] == 'G')
                {
                    atook_pic.Source = back;
                }
                else if (txtState.Text[i + 2] == 'H')
                {
                    btook_pic.Source = back;
                }
            }

            // Fill B hand with unknown
            for (int i = 0; i < 5; i++)
            {
                bPlaces[i].Source = back;
            }

            // If hand is not full, fill with empty
            for (int i = aCount; i < 5; i++)
            {
                aPlaces[i].Source = empty;
                bPlaces[i].Source = empty;
            }

            // B has a card put down
            if (txtState.Text.Contains("F") && aCount>0)
            {
                bPlaces[aCount - 1].Source = empty;
            }

            //Set 20/40
            AM20_img.Opacity = 0.4;
            AP20_img.Opacity = 0.4;
            AT20_img.Opacity = 0.4;
            AZ20_img.Opacity = 0.4;
            BM20_img.Opacity = 0.4;
            BP20_img.Opacity = 0.4;
            BT20_img.Opacity = 0.4;
            BZ20_img.Opacity = 0.4;
            if (txtState.Text[22] == 'A')
            {
                AM20_img.Opacity = 1;
            }
            if (txtState.Text[22] == 'B')
            {
                BM20_img.Opacity = 1;
            }
            if (txtState.Text[23] == 'A')
            {
                AP20_img.Opacity = 1;
            }
            if (txtState.Text[23] == 'B')
            {
                BP20_img.Opacity = 1;
            }
            if (txtState.Text[24] == 'A')
            {
                AT20_img.Opacity = 1;
            }
            if (txtState.Text[24] == 'B')
            {
                BT20_img.Opacity = 1;
            }
            if (txtState.Text[25] == 'A')
            {
                AZ20_img.Opacity = 1;
            }
            if (txtState.Text[25] == 'B')
            {
                BZ20_img.Opacity = 1;
            }
        }

        private void btnSet_Click(object sender, RoutedEventArgs e)
        {
            SetCards();
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            if ((string)btnStart.Content == "Start")
            {
                if (Validator.Validate(txtState.Text) == "OK")
                {
                    worker = new BackgroundWorker();
                    Calculator.Single(txtState.Text, worker);
                    worker.ProgressChanged += new ProgressChangedEventHandler(UpdateLabels);
                    worker.RunWorkerAsync();
                    btnStart.Content = "Stop";
                }
            }
            else if ((string)btnStart.Content == "Stop")
            {
                worker.CancelAsync();
            }
            else if ((string)btnStart.Content == "Reset")
            {
                a1_label.Text = "0.00";
                a2_label.Text = "0.00";
                a3_label.Text = "0.00";
                a4_label.Text = "0.00";
                a5_label.Text = "0.00";
                progress_label.Text = "0 / 0";
                btnStart.Content = "Start";
            }
        }

        private void UpdateLabels(object sender, ProgressChangedEventArgs e)
        {
            ReturnObject returnObject = (ReturnObject)e.UserState;

            a1_label.Text = returnObject.a1Value.ToString();
            a2_label.Text = returnObject.a2Value.ToString();
            a3_label.Text = returnObject.a3Value.ToString();
            a4_label.Text = returnObject.a4Value.ToString();
            a5_label.Text = returnObject.a5Value.ToString();

            progress_label.Text = returnObject.calculatedGames + " / " + returnObject.allGames;

            if (returnObject.isFinished)
            {
                btnStart.Content = "Reset";
            }
        }

        private void setACards()
        {
            string checkString = Validator.Validate(txtState.Text);
            if (checkString != "OK")
            {
                error_lbl.Text = checkString;
            }
            else
            {
                error_lbl.Text = "";
            }

            List<bool> x = new List<bool>();
            for (int i = 0; i < 20; i++)
            {
                if (txtState.Text[i + 2] == 'A')
                {
                    x.Add(true);
                }
                else
                {
                    x.Add(false);
                }
            }
            CardSelectWindow selectionWindow = new CardSelectWindow(x, "1to5");
            selectionWindow.ShowDialog();

            //The first character should remain A
            txtState.Text = txtState.Text.Replace('A', 'U');
            char[] arr = txtState.Text.ToCharArray();
            arr[0] = 'A';
            txtState.Text = new string(arr);

            x = selectionWindow.selectedCards;
            for (int i = 0; i < x.Count; i++)
            {
                if (x[i] == true)
                {
                    char[] arr2 = txtState.Text.ToCharArray();
                    arr2[i + 2] = 'A';
                    txtState.Text = new string(arr2);
                }
            }

            SetCards();
        }

        private void bottom_pic_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            string checkString = Validator.Validate(txtState.Text);
            if (checkString != "OK")
            {
                error_lbl.Text = checkString;
            }
            else
            {
                error_lbl.Text = "";
            }

            List<bool> x = new List<bool>();
            for (int i = 0; i < 20; i++)
            {
                if (txtState.Text[i + 2] == 'D')
                {
                    x.Add(true);
                }
                else
                {
                    x.Add(false);
                }
            }
            CardSelectWindow selectionWindow = new CardSelectWindow(x, "Single");
            selectionWindow.ShowDialog();

            txtState.Text = txtState.Text.Replace('D', 'U');
            x = selectionWindow.selectedCards;
            for (int i = 0; i < x.Count; i++)
            {
                if (x[i] == true)
                {
                    char[] arr = txtState.Text.ToCharArray();
                    arr[i + 2] = 'D';
                    txtState.Text = new string(arr);

                    //Setting trump color
                    if (i < 5)
                    {
                        char[] arr2 = txtState.Text.ToCharArray();
                        arr2[1] = 'M';
                        txtState.Text = new string(arr2);
                    }
                    else if (i < 10)
                    {
                        char[] arr2 = txtState.Text.ToCharArray();
                        arr2[1] = 'P';
                        txtState.Text = new string(arr2);
                    }
                    else if (i < 15)
                    {
                        char[] arr2 = txtState.Text.ToCharArray();
                        arr2[1] = 'T';
                        txtState.Text = new string(arr2);
                    }
                    else
                    {
                        char[] arr2 = txtState.Text.ToCharArray();
                        arr2[1] = 'Z';
                        txtState.Text = new string(arr2);
                    }
                }

            }

            SetCards();
        }

        private void f_pic_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            string checkString = Validator.Validate(txtState.Text);
            if (checkString != "OK")
            {
                error_lbl.Text = checkString;
            }
            else
            {
                error_lbl.Text = "";
            }

            List<bool> x = new List<bool>();
            for (int i = 0; i < 20; i++)
            {
                if (txtState.Text[i + 2] == 'F')
                {
                    x.Add(true);
                }
                else
                {
                    x.Add(false);
                }
            }
            CardSelectWindow selectionWindow = new CardSelectWindow(x, "Single");
            selectionWindow.ShowDialog();

            txtState.Text = txtState.Text.Replace('F', 'U');
            x = selectionWindow.selectedCards;
            for (int i = 0; i < x.Count; i++)
            {
                if (x[i] == true)
                {
                    char[] arr = txtState.Text.ToCharArray();
                    arr[i + 2] = 'F';
                    txtState.Text = new string(arr);
                }
            }

            SetCards();
        }

        private void a1_pic_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            setACards();
        }

        private void a2_pic_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            setACards();
        }

        private void a3_pic_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            setACards();
        }

        private void a4_pic_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            setACards();
        }

        private void a5_pic_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            setACards();
        }

        private void atook_pic_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            string checkString = Validator.Validate(txtState.Text);
            if (checkString != "OK")
            {
                error_lbl.Text = checkString;
            }
            else
            {
                error_lbl.Text = "";
            }

            List<bool> x = new List<bool>();
            for (int i = 0; i < 20; i++)
            {
                if (txtState.Text[i + 2] == 'G')
                {
                    x.Add(true);
                }
                else
                {
                    x.Add(false);
                }
            }
            CardSelectWindow selectionWindow = new CardSelectWindow(x, "Double");
            selectionWindow.ShowDialog();

            txtState.Text = txtState.Text.Replace('G', 'U');
            x = selectionWindow.selectedCards;
            for (int i = 0; i < x.Count; i++)
            {
                if (x[i] == true)
                {
                    char[] arr = txtState.Text.ToCharArray();
                    arr[i + 2] = 'G';
                    txtState.Text = new string(arr);
                }
            }

            SetCards();
        }

        private void btook_pic_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            string checkString = Validator.Validate(txtState.Text);
            if (checkString != "OK")
            {
                error_lbl.Text = checkString;
            }
            else
            {
                error_lbl.Text = "";
            }

            List<bool> x = new List<bool>();
            for (int i = 0; i < 20; i++)
            {
                if (txtState.Text[i + 2] == 'H')
                {
                    x.Add(true);
                }
                else
                {
                    x.Add(false);
                }
            }
            CardSelectWindow selectionWindow = new CardSelectWindow(x, "Double");
            selectionWindow.ShowDialog();

            txtState.Text = txtState.Text.Replace('H', 'U');
            x = selectionWindow.selectedCards;
            for (int i = 0; i < x.Count; i++)
            {
                if (x[i] == true)
                {
                    char[] arr = txtState.Text.ToCharArray();
                    arr[i + 2] = 'H';
                    txtState.Text = new string(arr);
                }
            }

            SetCards();
        }

        private void AM20_Click(object sender, RoutedEventArgs e)
        {
            char[] arr = txtState.Text.ToCharArray();
            if(arr[22] != 'A')
            {
                arr[22] = 'A';
            }
            else
            {
                arr[22] = 'X';
            }
            txtState.Text = new string(arr);
            SetCards();
        }

        private void AP20_Click(object sender, RoutedEventArgs e)
        {
            char[] arr = txtState.Text.ToCharArray();
            if (arr[23] != 'A')
            {
                arr[23] = 'A';
            }
            else
            {
                arr[23] = 'X';
            }
            txtState.Text = new string(arr);
            SetCards();
        }

        private void AT20_Click(object sender, RoutedEventArgs e)
        {
            char[] arr = txtState.Text.ToCharArray();
            if (arr[24] != 'A')
            {
                arr[24] = 'A';
            }
            else
            {
                arr[24] = 'X';
            }
            txtState.Text = new string(arr);
            SetCards();
        }

        private void AZ20_Click(object sender, RoutedEventArgs e)
        {
            char[] arr = txtState.Text.ToCharArray();
            if (arr[25] != 'A')
            {
                arr[25] = 'A';
            }
            else
            {
                arr[25] = 'X';
            }
            txtState.Text = new string(arr);
            SetCards();
        }

        private void BM20_Click(object sender, RoutedEventArgs e)
        {
            char[] arr = txtState.Text.ToCharArray();
            if (arr[22] != 'B')
            {
                arr[22] = 'B';
            }
            else
            {
                arr[22] = 'X';
            }
            txtState.Text = new string(arr);
            SetCards();
        }

        private void BP20_Click(object sender, RoutedEventArgs e)
        {
            char[] arr = txtState.Text.ToCharArray();
            if (arr[23] != 'B')
            {
                arr[23] = 'B';
            }
            else
            {
                arr[23] = 'X';
            }
            txtState.Text = new string(arr);
            SetCards();
        }

        private void BT20_Click(object sender, RoutedEventArgs e)
        {
            char[] arr = txtState.Text.ToCharArray();
            if (arr[24] != 'B')
            {
                arr[24] = 'B';
            }
            else
            {
                arr[24] = 'X';
            }
            txtState.Text = new string(arr);
            SetCards();
        }

        private void BZ20_Click(object sender, RoutedEventArgs e)
        {
            char[] arr = txtState.Text.ToCharArray();
            if (arr[25] != 'B')
            {
                arr[25] = 'B';
            }
            else
            {
                arr[25] = 'X';
            }
            txtState.Text = new string(arr);
            SetCards();
        }
    }
}
