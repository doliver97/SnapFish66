using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


        private double EstVal(string s)
        {
            List<Node> nodes = root.GetChildrenOfAction(s);
            return nodes.Average(x => x.EstimatedValue);
        }

        public void SetEstimatedValues()
        {
            a1 = EstVal("a1");
            a2 = EstVal("a2");
            a3 = EstVal("a3");
            a4 = EstVal("a4");
            a5 = EstVal("a5");
            b1 = EstVal("b1");
            b2 = EstVal("b2");
            b3 = EstVal("b3");
            b4 = EstVal("b4");
            b5 = EstVal("b5");
            cover = EstVal("cover");
        }

        public void Calculate()
        {
            while (Main.running)
            {
                CalcOneRound();
            }
        }

        private void CalcOneRound()
        {
            Node actual = root;

            while(actual!=null)
            {
                actual = actual.AddRandomChild();
            }
        }
    }
}
