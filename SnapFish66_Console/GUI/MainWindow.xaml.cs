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
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnSet_Click(object sender, RoutedEventArgs e)
        {
            //TODO validate input

        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            //Calculator.Single(txtState.Text);
            BackgroundWorker worker = new BackgroundWorker();
            Calculator.Single(txtState.Text, worker);
            worker.ProgressChanged += new ProgressChangedEventHandler(UpdateLabels);
            worker.RunWorkerAsync();
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
        }
    }
}
