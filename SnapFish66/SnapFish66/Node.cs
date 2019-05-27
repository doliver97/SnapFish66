using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapFish66
{
    class Node
    {
        public State state;
        public Node parent;

        private bool isRoot;

        private Dictionary<string, List<Node>> children = new Dictionary<string, List<Node>>();
        private Random random = new Random();

        public double EstimatedValue;

        private List<String> PossibleSteps = new List<string> { "A1","A2","A3","A4","A5","B1","B2","B3","B4","B5","cover"};

        public Node(Node newparent)
        {
            if(newparent==null)
            {
                isRoot = true;
            }
            else
            {
                isRoot = false;
                parent = newparent;
            }
        }

        public bool IsEnd(State s)
        {
            s.CalculatePoints();
            return (s.Apoints!=0 || s.Bpoints!=0);
        }

        public Node AddRandomChild()
        {
            int ranVal = 0;

            Node child = new Node(this)
            {
                state = state.Copy()
            };

            if (isRoot)
            {
                child.state = child.state.GenerateRandom();
            }

            //Do not try to generate child in end state, rather calculate the value of the state (and propagate up)
            if(IsEnd(state))
            {
                CalcEstimatedValue();
                return null;
            }

            bool success = false;

            while(!success)
            {
                //reset the wrong steps effect
                child.state = state.Copy(); 
                if (isRoot)
                {
                    child.state = child.state.GenerateRandom();
                }

                ranVal = random.Next(PossibleSteps.Count);
                success = child.state.Step(child.state,PossibleSteps[ranVal]);
            }

            if(!children.ContainsKey(PossibleSteps[ranVal]))
            {
                children[PossibleSteps[ranVal]] = new List<Node>();
            }

            children[PossibleSteps[ranVal]].Add(child);

            return child;
        }

        public void CalcEstimatedValue()
        {
            //Trivial case
            if(IsEnd(state))
            {
                if(state.Apoints>0)
                {
                    EstimatedValue = state.Apoints;
                }
                else
                {
                    EstimatedValue = state.Bpoints * (-1);
                }
                
            }
            //Find best step, with highest average points
            else if(state.next=="A")
            {
                EstimatedValue = HighestInDict();
                

            }
            //Find best step, with lowest average points
            else if(state.next=="B")
            {
                EstimatedValue = LowestInDict();
            }

            //Propagate value up the tree
            if (parent != null)
            {
                parent.CalcEstimatedValue();
            }
        }

        private double HighestInDict()
        {
            double max = -10;
            foreach (var item in children.Values)
            {
                if(item.Average(x=>x.EstimatedValue)> max)
                {
                    max = item.Average(x => x.EstimatedValue);
                }
            }

            return max;
        }

        private double LowestInDict()
        {
            double min = 10;
            foreach (var item in children.Values)
            {
                if (item.Average(x => x.EstimatedValue) < min)
                {
                    min = item.Average(x => x.EstimatedValue);
                }
            }

            return min;
        }

        public List<Node> GetChildrenOfAction(string action)
        {
            if(children.ContainsKey(action))
            {
                return children[action];
            }
            else
            {
                return new List<Node>();
            }
        }
    }
}
