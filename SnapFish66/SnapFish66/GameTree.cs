using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnapFish66
{
    class GameTree
    {
        Node root;

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
        

        public GameTree(State s)
        {
            root = new Node(null)
            {
                state = s
            };
        }

        //Estimated value of action, or NaN if no such action yet
        private double EstVal(string s)
        {
            List<Node> nodes = root.GetChildrenOfAction(s);

            if(nodes.Count==0)
            {
                return Double.NaN;
            }

            return nodes.Average(x => x.EstimatedValue);
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

        public void Calculate(List<Label> labels)
        {
            //while (Main.running)
            for(int i=0; i<10000;i++)
            {
                CalcOneRound();
            }
            SetLabels(labels);
        }

        private void CalcOneRound()
        {
            Node actual = root;
            
            while(actual!=null)
            {
                actual = actual.AddRandomChild();
            }
        }

        //Calculates estimated values and sets the labels
        private void SetLabels(List<Label> labels)
        {
            SetEstimatedValues();

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
                if(values[i]!=Double.NaN)
                {
                    labels[i].Text = Convert.ToString(Math.Round(values[i],2));
                }
                else
                {
                    labels[i].Text = "N/A";
                }
            }
        }
    }
}
