using BushFire.Game.Storage;
using BushFire.MapGeneration.Tech;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Game.Map
{
    class Town
    {
        Point townCenter;
        public Vector2 worldLocation { get; private set; }
        public int id { get; private set; }
        public List<Plot> plotList { get; private set; }
        public int numberOfBuildings { get; private set; }
        public string name { get; private set; }
        public Vector2 miniMapLocation { get; private set; }

        public Town(Point shrunkenPoint, int id, string name)
        {
            townCenter = new Point(shrunkenPoint.X * 2, shrunkenPoint.Y * 2);
            worldLocation = new Vector2(townCenter.X * GroundLayerController.tileSize, townCenter.Y * GroundLayerController.tileSize);

            this.id = id;
            this.name = name;
            plotList = new List<Plot>();
            numberOfBuildings = 0;
            miniMapLocation = new Vector2(townCenter.X * 2, townCenter.Y * 2);
        }

        public void AddPlot(Plot plot)
        {
            plotList.Add(plot);
        }
               
        public Point GetShrunkPoint()
        {
            return new Point(townCenter.X / 2, townCenter.Y / 2);
        }

        public void DestroyPlotList()
        {
            plotList = null;
        }

        public void IncreaseBuildingCount()
        {
            numberOfBuildings++;
        }


    }
}
