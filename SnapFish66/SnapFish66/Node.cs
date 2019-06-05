using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapFish66
{
    public class Node
    {
        public State state;
        public Node parent;

        private bool isRoot;

        public bool closed;

        private Dictionary<string, List<Node>> children = new Dictionary<string, List<Node>>();
        private Random random = new Random();

        public double EstimatedValue;

        public int depth;
        
        public List<String> VisitedSteps;
        public List<String> UnvisitedSteps;
        
        public Node(Node newparent, int ndepth)
        {
            if(newparent==null)
            {
                isRoot = true;
                depth = 0;
            }
            else
            {
                isRoot = false;
                parent = newparent;
                depth = ndepth;
            }

            //Add cover later
            VisitedSteps = new List<string>();
            UnvisitedSteps = new List<string> { "A1", "A2", "A3", "A4", "A5", "B1", "B2", "B3", "B4", "B5" };
            
        }

        private void SetClosed()
        {
            //Trivial case: end of game
            if(IsEnd(state))
            {
                closed = true;
            }
            else
            {
                //If there are no unvisited possible steps left, and every child is closed, set this as closed
                if(UnvisitedSteps.Count==0)
                {
                    closed = true;
                }
                foreach(List<Node> childlist in children.Values)
                {
                    foreach (Node child in childlist)
                    {
                        if(!child.closed)
                        {
                            closed = false;
                            return;
                        }
                    }
                }
            }
        }

        public virtual bool IsEnd(State s)
        {
            s.CalculatePoints();
            return (s.Apoints!=0 || s.Bpoints!=0);
        }
        
        public virtual Node AddRandomChild()
        {
            int ranVal = 0;

            Node child = new Node(this, depth+1);
            child.state = this.state.Copy();

            if (isRoot)
            {
                child.state = child.state.GenerateRandom();
            }

            //Set closed (has to be before IsEnd)
            SetClosed();
            

            //Do not try to generate child in end state, rather calculate the value of the state (and propagate up)
            if (IsEnd(state))
            {
                CalcEstimatedValue();
                return null;
            }

            //Do not go further if it is closed (has to be after IsEnd)
            if (closed)
            {
                return null;
            }

            //Remove invalid steps
            if (child.state.next=="A")
            {
                UnvisitedSteps.Remove("B1");
                UnvisitedSteps.Remove("B2");
                UnvisitedSteps.Remove("B3");
                UnvisitedSteps.Remove("B4");
                UnvisitedSteps.Remove("B5");
            }
            else
            {
                UnvisitedSteps.Remove("A1");
                UnvisitedSteps.Remove("A2");
                UnvisitedSteps.Remove("A3");
                UnvisitedSteps.Remove("A4");
                UnvisitedSteps.Remove("A5");
            }

            bool success = false;

            string action = "";
            
            while (!success)
            {
                //reset the wrong steps effect
                child.state = state.Copy();
                if (isRoot)
                {
                    child.state = child.state.GenerateRandom();
                }

                // Only check visiteds if there are no more unvisiteds
                if(UnvisitedSteps.Count>0)
                {
                    ranVal = random.Next(UnvisitedSteps.Count);
                    action = UnvisitedSteps[ranVal];
                    success = child.state.Step(child.state, action);

                    if(success)
                    {
                        VisitedSteps.Add(action);
                    }

                    UnvisitedSteps.Remove(action);
                }
                else
                {
                    ranVal = random.Next(VisitedSteps.Count);
                    action = VisitedSteps[ranVal];

                    //Will always be true, but needed because of side effect
                    success = child.state.Step(child.state, action);
                }
            }

            if(!children.ContainsKey(action))
            {
                children[action] = new List<Node>();
            }

            //Check if this state has already been added. If yes, return the old one.
            bool found = false;
            foreach (Node old in children[action])
            {
                if(child.state.IsSame(old.state))
                {
                    found = true;
                    child = old;
                }
            }

            if (!found)
            {
                children[action].Add(child);
                GameTree.allNodes.Add(child);
            }

            

            return child;
        }

        public virtual void CalcEstimatedValue()
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

        public virtual List<Node> GetChildrenOfAction(string action)
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
