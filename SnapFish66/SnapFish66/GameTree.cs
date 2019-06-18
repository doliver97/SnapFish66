using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnapFish66
{
    public class GameTree
    {
        public Node root;

        public delegate void SetLabelsDelegate(List<Label> labels);
        public SetLabelsDelegate labelsDelegate;

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

            root = new Node(null, s, "", 0);
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

        //Estimated value of action, or NaN if no such action yet
        private double EstVal(Node subroot, string s)
        {
            List<Node> childrenOfRoot = subroot.children;

            List<Node> nodesOfAction = childrenOfRoot.Where(x=>x.actionBefore==s).ToList();

            if(nodesOfAction.Count==0)
            {
                return Double.NaN;
            }

            return nodesOfAction.Average(x => x.EstimatedValue);
        }

        public void Reset(State state)
        {
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

        public void Calculate(List<Label> labels, BackgroundWorker worker)
        {
            possibleSubroots = CalcPossibleSubroots();

            while (!worker.CancellationPending)
            {
                if(subroots.Count<possibleSubroots)
                {
                    Node subroot = CreateNewSubroot();

                    subroot.AlphaBeta(-4, 4);

                    //Calculate data for labels
                    AddEstimatedValues(subroot);
                    CalcAverages();

                    //We wont need the children of the subroot
                    subroot.children.Clear();

                    //Call SetLabels
                    worker.ReportProgress(0); 
                }
            }
        }

        private void AddEstimatedValues(Node subroot)
        {
            subroot.GetEstimatedValue();

            double val = 0;
            for (int i = 0; i < actionList.Count; i++)
            {
                val = EstVal(subroot, actionList[i]);
                if (val != 4 && val != -4)
                {
                    estimatedLists[i].Add(val);
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
        public void SetLabels(List<Label> labels)
        {
            for (int i = 0; i < labels.Count; i++)
            {
                if(!double.IsNaN(averages[i]))
                {
                    labels[i].Text = Convert.ToString(Math.Round(averages[i], 2));
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
            Node newNode = new Node(null,root.state.GenerateRandom(),"",0);

            while(!found)
            {
                newNode = new Node(null, root.state.GenerateRandom(), "", 0);
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

            subroots.Add(newNode);
            return newNode;
        }
    }
}