using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnapFish66
{
    public class GameTree
    {
        //public static int GenerateTimeSum;

        public Node root;

        public delegate void SetLabelsDelegate(List<Label> labels, ProgressBar progressBar, Label timeLeft);
        public SetLabelsDelegate labelsDelegate;
        
        public static DateTime started;

        //Key is the depth
        public static Dictionary<int, int> VisitedNodes = new Dictionary<int, int>();

        //Estimated values of actions
        public List<double> a1;
        public List<double> a2;
        public List<double> a3;
        public List<double> a4;
        public List<double> a5;
        public List<double> b1;
        public List<double> b2;
        public List<double> b3;
        public List<double> b4;
        public List<double> b5;
        public List<double> cover;

        //Easy access to the lists above
        public List<List<double>> estimatedLists;

        //The averages of the lists above repectively, CalcAverages() sets it
        public List<double> averages;

        List<string> actionList;

        private DataGridView nodesDataGridView;

        public static List<Node> allNodes;

        public int possibleSubroots;
        public List<Node> subroots;
        

        public GameTree(State s, DataGridView ndataGridView)
        {
            labelsDelegate = new SetLabelsDelegate(SetLabels);
            started = DateTime.Now;

            a1 = new List<double>();
            a2 = new List<double>();
            a3 = new List<double>();
            a4 = new List<double>();
            a5 = new List<double>();
            b1 = new List<double>();
            b2 = new List<double>();
            b3 = new List<double>();
            b4 = new List<double>();
            b5 = new List<double>();
            cover = new List<double>();
            estimatedLists = new List<List<double>> { a1, a2, a3, a4, a5, b1, b2, b3, b4, b5, cover };
            actionList = new List<string> { "A1", "A2", "A3", "A4", "A5", "B1", "B2", "B3", "B4", "B5", "cover" };

            averages = new List<double>();
            for (int i = 0; i < estimatedLists.Count; i++)
            {
                averages.Add(double.NaN);
            }
            subroots = new List<Node>();

            int round = s.adown.Count + s.bdown.Count + s.atook.Count + s.btook.Count;

            root = new Node(null, s, "", round);
            root.SetMaximizer();
            
            nodesDataGridView = ndataGridView;

            allNodes = new List<Node> { root };
        }

        private int CalcPossibleSubroots()
        {
            int cardsInBHand = root.state.b1.Count + root.state.b2.Count + root.state.b3.Count + root.state.b4.Count + root.state.b5.Count;
            int cardsInDeck = 20 - cardsInBHand - root.state.dbottom.Count - root.state.adown.Count - root.state.bdown.Count - root.state.atook.Count - root.state.btook.Count - root.state.a1.Count - root.state.a2.Count - root.state.a3.Count - root.state.a4.Count - root.state.a5.Count;

            // count is sum unknown factorial per in hand factorial
            int count = 1;
            for (int i = cardsInBHand+1; i <= cardsInBHand+cardsInDeck; i++)
            {
                count *= i;
            }

            return count;
        }
        
        public void Reset(State state)
        {
            started = DateTime.Now;
            foreach (var item in estimatedLists)
            {
                item.Clear();
            }
            allNodes.Clear();
            subroots.Clear();
            averages.Clear();
            for (int i = 0; i < estimatedLists.Count; i++)
            {
                averages.Add(double.NaN);
            }
            root = new Node(null, state, "", 0);
            root.SetMaximizer();
            allNodes.Add(root);
            VisitedNodes.Clear();
        }

        public void Calculate(List<Label> labels, ProgressBar progressBar, BackgroundWorker worker)
        {
            possibleSubroots = CalcPossibleSubroots();
            
            started = DateTime.Now;
            
            while (!worker.CancellationPending)
            {
                if(subroots.Count<possibleSubroots)
                {
                    Node subroot = CreateNewSubroot();

                    //Add children of different actions (will be root of alphabeta)
                    List<Node> children = new List<Node>();
                    for (int i = 0; i < actionList.Count-1; i++) //without cover
                    {
                        Node child = new Node(subroot, subroot.state.Copy(), actionList[i], subroot.depth + 1);
                        bool success = child.state.Step(child.state, actionList[i]);
                        child.SetMaximizer();

                        if(success)
                        {
                            children.Add(child);
                        }
                    }

                    for (int i = 0; i < children.Count; i++)
                    {
                        children[i].AlphaBeta(-4, 4);

                        //Calculate data for labels
                        AddEstimatedValue(children[i]);
                        CalcAverages();

                        //We wont need the children of the subroot
                        children.RemoveAt(i);
                        i--;

                        //Call SetLabels
                        worker.ReportProgress(0);  
                    }
                }
                else
                {
                    break;
                }
            }
        }

        private void AddEstimatedValue(Node child)
        {
            for (int i = 0; i < actionList.Count; i++)
            {
                if(actionList[i]==child.actionBefore)
                {
                    estimatedLists[i].Add(child.value);
                    break;
                }
            }
            
        }

        private void CalcAverages()
        {
            for (int i = 0; i < estimatedLists.Count; i++)
            {
                if (estimatedLists[i].Count > 0)
                {
                    averages[i] = estimatedLists[i].Average();
                }
            }
        }

        //Calculates estimated values and unvisited nodes, and sets the labels
        public void SetLabels(List<Label> labels, ProgressBar progressBar, Label timeLeft)
        {
            //Set progressBar
            progressBar.Maximum = possibleSubroots;
            progressBar.Value = subroots.Count;

            //Set time left label
            double proportion = subroots.Count/Convert.ToDouble(possibleSubroots);
            TimeSpan left = new TimeSpan(0);
            if (proportion > 0)
            {
                double multiplier = (1 - proportion) / proportion;
                left = TimeSpan.FromTicks(Convert.ToInt64((DateTime.Now - started).Ticks * multiplier));
            }
            if(left.Days>=365)
            {
                int years = left.Days / 365;
                int days = left.Days % 365;
                timeLeft.Text = years + " years " + days + " days";
            }
            else if(left.Days>=10)
            {
                timeLeft.Text = left.Days + " days";
            }
            else if(left.Days>=1)
            {
                timeLeft.Text = left.Days + " days " + left.Hours + " hours";
            }
            else
            {
                timeLeft.Text = left.Hours/10 + "" + left.Hours%10 + ":" + left.Minutes/10 + "" + left.Minutes%10 + ":" + left.Seconds/10 + "" + left.Seconds%10;
            }


            for (int i = 0; i < labels.Count; i++)
            {
                if(!double.IsNaN(averages[i]))
                {
                    labels[i].Text = Convert.ToString(Math.Round(averages[i], 2));
                    labels[i].Refresh();
                }
                else
                {
                    labels[i].Text = "N/A";
                }
            }

            //Clear dataGridView
            nodesDataGridView.Rows.Clear();

            nodesDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            nodesDataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;

            //Set the dataGridView
            nodesDataGridView.ColumnCount = 2;
            nodesDataGridView.Columns[0].Name = "Depth";
            nodesDataGridView.Columns[1].Name = "Visited";

            nodesDataGridView.Columns[1].DefaultCellStyle.Format = "N0";

            lock (VisitedNodes)
            {
                if (VisitedNodes.Keys.Count > 0)
                {
                    nodesDataGridView.RowCount = VisitedNodes.Keys.Max() + 1;
                }

                for (int i = 0; i < nodesDataGridView.RowCount; i++)
                {
                    nodesDataGridView.Rows[i].Cells[0].Value = i;
                }

                foreach (int key in VisitedNodes.Keys)
                {
                    nodesDataGridView.Rows[key].Cells[1].Value = VisitedNodes[key];
                } 
            }

            nodesDataGridView.Refresh();
        }

        //Creates random root node by giving value to unknown cards
        private Node CreateNewSubroot()
        {
            bool found = false;
            int round = root.state.adown.Count + root.state.bdown.Count + root.state.atook.Count + root.state.btook.Count;
            Node newNode = new Node(null,root.state.GenerateRandom(),"",round);

            //DateTime begin = DateTime.Now;

            while(!found)
            {
                newNode = new Node(null, root.state.GenerateRandom(), "", round);
                found = true;
                foreach (Node n in subroots)
                {
                    if(newNode.state.IsSame(n.state))
                    {
                        found = false;
                        break;
                    }
                }
            }

            //DateTime end = DateTime.Now;

            //TimeSpan span = end - begin;

            //GenerateTimeSum += span.Milliseconds;

            //Console.WriteLine(GenerateTimeSum);

            subroots.Add(newNode);
            return newNode;
        }
    }
}