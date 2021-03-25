using BushFire.Game.Map;
using BushFire.MapGeneration.Containers;
using BushFire.MapGeneration.Tech;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.MapGeneration.Generation.RoadStuff
{
    class ExpandShrunkPlot
    {
        public ExpandShrunkPlot(List<ShrunkPlot> shrunkPlotList, List<Town> townList, LoadingInfo loadingInfo)
        {
            float percentDone = 0;
            float percentJump = 100f / shrunkPlotList.Count;


            foreach (ShrunkPlot shrunkPlot in shrunkPlotList)
            {
                percentDone += percentJump;
                loadingInfo.UpdateLoading(LoadingType.ExpandingTowns, percentDone);
                int id = shrunkPlot.townId;
                townList[id].AddPlot(new Plot(shrunkPlot.pointOne, shrunkPlot.pointTwo, shrunkPlot.plotNum, shrunkPlot.roadFaceDirection));
            }

        }



    }
}
