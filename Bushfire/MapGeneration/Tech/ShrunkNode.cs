using BushFire.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.MapGeneration.Tech
{
    class ShrunkNode
    {
        public LandType landType { get; private set; }
        public int townId { get; private set; }
        public int pathScore;
        public int plotId { get; set; }

        public ShrunkNode(LandType landType)
        {
            plotId = -1;
            townId = -1;
            SetLandType(landType);
        }

        public void SetTownId(int townId)
        {
            this.townId = townId;
        }

        public void SetLandType(LandType landType)
        {
            this.landType = landType;          

            if (landType == LandType.WATER)
            {
                pathScore = 30;
            }
            else if (landType == LandType.PLOT)
            {
                pathScore = 150;
            }
            else if (landType == LandType.OPEN)
            {
                pathScore = 10;
            }
            else if (landType == LandType.COUNTRYROAD)
            {
                pathScore = 1;
            }
            else if (landType == LandType.CITYROAD)
            {
                pathScore = 1;
            }
            else
            {
                pathScore = 20;
            }
        }

        public bool IsRoad()
        {
            return (landType == LandType.CITYROAD || landType == LandType.COUNTRYROAD);
        }



              

    }
}
