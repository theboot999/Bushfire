using BushFire.MapGeneration.Tech;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.MapGeneration.Generation.TownsAndRoadStuff
{
    class ClearingShrunkPlotErrors
    {
        public ClearingShrunkPlotErrors(ShrunkNode[,] shrunkMap, List<ShrunkPlot> shrunkPlotList)
        {
            Smooth(shrunkMap, shrunkPlotList);
        }

        private void Smooth(ShrunkNode[,] shrunkMap, List<ShrunkPlot> shrunkPlotList)
        {
            int error = 0;

            for (int i = shrunkPlotList.Count - 1; i > -1; i--)
            {
                shrunkPlotList[i].Smooth(shrunkMap);
                if (shrunkPlotList[i].kill)
                {
                    error++;
                    shrunkPlotList.RemoveAt(i);
                }
            }

            for (int i = 0; i < shrunkPlotList.Count; i++)
            {
                shrunkPlotList[i].plotNum = i;
                shrunkPlotList[i].AddDebugToMap(shrunkMap);
            }
            Debug.WriteLine(error);
            Debug.WriteLine("Total of final" + shrunkPlotList.Count);
        }
    }
}
