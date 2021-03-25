using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Game.Tech.Jobs
{
    class Reusables
    {
      //  public List<AStarNode> nodePool;
        public Dictionary<int, AStarNode> checkedNodeList;
        public Dictionary<int, AStarNode> toCheckList;

        public Reusables()
        {
         //   nodePool = new List<AStarNode>();
            checkedNodeList = new Dictionary<int, AStarNode>();
            toCheckList = new Dictionary<int, AStarNode>();
        }

        public void Clear()
        {
            checkedNodeList.Clear();
            toCheckList.Clear();

            
           // nodePool.Clear();
        //    nodePool = new List<AStarNode>();
        }
    }
}
