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
        public static Dictionary<int, int> UnvisitedNodes = new Dictionary<int, int>();

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
        

        public GameTree(State s, DataGridView ndataGridView)
        {
            labelsDelegate = new SetLabelsDelegate(SetLabels);

            root = new Node(null, s, "", 0);
            nodesDataGridView = ndataGridView;

            allNodes = new List<Node> { root };
        }

        //Estimated value of action, or NaN if no such action yet
        private double EstVal(string s)
        {
            List<Node> childrenOfRoot = root.children;

            List<Node> nodesOfAction = childrenOfRoot.Where(x=>x.actionBefore==s).ToList();

            if(nodesOfAction.Count==0)
            {
                return Double.NaN;
            }

            return nodesOfAction.Average(x => x.EstimatedValue);
        }

        private void SetEstimatedValues()
        {
            a1 = EstVal("A1");
            a2 = EstVal("A2");
            a3 = EstVal("A3");
            a4 = EstVal("A4");
            a5 = EstVal("A5");
            b1 = EstVal("B1");
            b2 = EstVal("B2");
            b3 = EstVal("B3");
            b4 = EstVal("B4");
            b5 = EstVal("B5");
            cover = EstVal("cover");
        }

        public void Reset(State state)
        {
            allNodes.Clear();
            root = new Node(null, state, "", 0);
            allNodes.Add(root);
            VisitedNodes.Clear();
            UnvisitedNodes.Clear();
        }

        public void Calculate(List<Label> labels, BackgroundWorker worker)
        {
            while(!worker.CancellationPending && root.closed==false)
            {
                root.AlphaBeta(-3, 3);

                //Calculate data for labels
                SetEstimatedValues();
                SetUnvisitedNodes(); //Calculates both visited and unvisited nodes

                //Call SetLabels
                worker.ReportProgress(0);
                
            }
        }

        //Sets visited and unvisited nodes
        private void SetUnvisitedNodes()
        {
            VisitedNodes.Clear();
            UnvisitedNodes.Clear();

            foreach (Node node in allNodes)
            {
                //Add all visited nodes
                if(!VisitedNodes.ContainsKey(node.depth))
                {
                    VisitedNodes[node.depth] = 1;
                }
                else
                {
                    VisitedNodes[node.depth]++;
                }

                //Do not count the unvisited nodes after end of game
                if(!node.IsEnd())
                {
                    //Add unvisited nodes
                    if (!UnvisitedNodes.ContainsKey(node.depth))
                    {
                        UnvisitedNodes[node.depth] = node.UnvisitedSteps.Count;
                    }
                    else
                    {
                        UnvisitedNodes[node.depth]+=node.UnvisitedSteps.Count;
                    }
                }
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
            nodesDataGridView.ColumnCount = 3;
            nodesDataGridView.Columns[0].Name = "Depth";
            nodesDataGridView.Columns[1].Name = "Visited";
            nodesDataGridView.Columns[2].Name = "Unvisited";

            //VisitedNodes has more keys
            nodesDataGridView.RowCount = VisitedNodes.Keys.Count;

            for (int i = 0; i < nodesDataGridView.RowCount; i++)
            {
                nodesDataGridView.Rows[i].Cells[0].Value = i;
            }

            foreach (int key in VisitedNodes.Keys.ToList())
            {
                nodesDataGridView.Rows[key].Cells[1].Value = VisitedNodes[key];
            }

            foreach (int key in UnvisitedNodes.Keys.ToList())
            {
                nodesDataGridView.Rows[key].Cells[2].Value = UnvisitedNodes[key];
            }

            nodesDataGridView.Refresh();
        }
    }
}
