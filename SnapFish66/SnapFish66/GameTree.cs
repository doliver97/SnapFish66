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
        public double a1;
        public double a2;
        public double a3;
        public double a4;
        public double a5;
        public double b1;
        public double b2;
        public double b3;
        public double b4;
        public double b5;
        public double cover;
        
        private DataGridView nodesDataGridView;

        public static List<Node> allNodes;

        public int possibleSubroots;
        public List<Node> subroots;
        

        public GameTree(State s, DataGridView ndataGridView)
        {
            labelsDelegate = new SetLabelsDelegate(SetLabels);

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
            allNodes.Clear();
            subroots.Clear();
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

                    subroot.AlphaBeta(-3, 3);
                    
                    //Calculate data for labels
                    CalculateEstimatedValues();

                    //Call SetLabels
                    worker.ReportProgress(0); 
                }
            }
        }
        
        private void CalculateEstimatedValues()
        {
            a1 = 0;
            a2 = 0;
            a3 = 0;
            a4 = 0;
            a5 = 0;
            b1 = 0;
            b2 = 0;
            b3 = 0;
            b4 = 0;
            b5 = 0;

            foreach (Node subroot in subroots)
            {
                subroot.GetEstimatedValue();
                a1 += EstVal(subroot, "A1") / subroots.Count;
                a2 += EstVal(subroot, "A2") / subroots.Count;
                a3 += EstVal(subroot, "A3") / subroots.Count;
                a4 += EstVal(subroot, "A4") / subroots.Count;
                a5 += EstVal(subroot, "A5") / subroots.Count;
                b1 += EstVal(subroot, "B1") / subroots.Count;
                b2 += EstVal(subroot, "B2") / subroots.Count;
                b3 += EstVal(subroot, "B3") / subroots.Count;
                b4 += EstVal(subroot, "B4") / subroots.Count;
                b5 += EstVal(subroot, "B5") / subroots.Count;
            }
        }

        //Calculates estimated values and unvisited nodes, and sets the labels
        public void SetLabels(List<Label> labels)
        {
            List<double> values = new List<double>
            {
                a1,
                a2,
                a3,
                a4,
                a5,
                b1,
                b2,
                b3,
                b4,
                b5,
                cover
            };

            for (int i = 0; i < labels.Count; i++)
            {
                if(!double.IsNaN(values[i]))
                {
                    labels[i].Text = Convert.ToString(Math.Round(values[i], 2));
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