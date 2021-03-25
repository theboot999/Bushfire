using BushFire.Editor.Tech;
using BushFire.Engine.ContentStorage;
using BushFire.Engine.Controllers;
using BushFire.Game.Map.MapObjectComponents;
using BushFire.Game.Map.MapObjects;
using BushFire.Game.Tech;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Game.MapObjects
{
    //An intersection consists of stoplights
    //Stoplights consist of streetlights

    class Intersection
    {
        public Dictionary<Direction, StopLight> stopLightList { get; private set; }
        public int tileX { get; private set; }
        public int tileY { get; private set; }
        int id;
        float stageTimer;
        int stage;
        float nextStageMax;

        public Intersection(Random rnd, int tileX, int tileY, int id) //they should only ever start at stage 1 or 3
        {
            this.tileX = tileX;
            this.tileY = tileY;
            this.id = id;
            stage = rnd.Next(0, 2) * 2;  //should always be 0 or 2
            stageTimer = (float)rnd.Next(0, (int)WorldController.stopLightTimes[stage]); //Random the timer a bit so stoplights dont all tick the same
            nextStageMax = WorldController.stopLightTimes[stage];
        }

        public void SetStopLightList(Dictionary<Direction, StopLight> stopLightList)
        {
            this.stopLightList = stopLightList;
        }
        
        public string GetIdString()
        {
            return "Intersection #" + id.ToString();
        }
       
        private void AdvanceStage()  
        {
            stage++;
            if (stage > 3)
            {
                stage = 0;
            }
            nextStageMax = WorldController.stopLightTimes[stage];
            stageTimer = 0;

            foreach (StopLight stopLight in stopLightList.Values)
            {
                stopLight.Advance();
            }
        }

        public void Update()
        {
            //TODO: Change to game update time when we get a bit further.  Currently using this to not flash sped up gametime
            stageTimer += 0.02f * EngineController.gameUpdateTime;
           // stageTimer += 0.02f * GameController.gameUpdateTime;
            if (stageTimer > nextStageMax)
            {
                AdvanceStage();
            }
        }




    }

    enum StopLightStage
    {
        GREEN = 0,
        AMBER = 1,
        RED = 2,
        REDWAITING = 3
    }
}
