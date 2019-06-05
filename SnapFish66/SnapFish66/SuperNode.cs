using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapFish66
{
    class SuperNode : Node
    {
        public SuperNode(Node newparent, int ndepth) : base(newparent, ndepth)
        {
            closed = true;
        }

        public void Build(string sstr)
        {
            //TODO process string, set estimated value
        }

        public void GenerateString()
        {
            //TODO
        }

        public override Node AddRandomChild()
        {
            //No new child is possible
            return null;
        }

        public override void CalcEstimatedValue()
        {
            //We already set it
        }

        public override List<Node> GetChildrenOfAction(string action)
        {
            return base.GetChildrenOfAction(action);
        }

        public override bool IsEnd(State s)
        {
            return base.IsEnd(s);
        }
    }
}
