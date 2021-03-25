using BushFire.Engine;
using BushFire.Engine.Controllers;
using BushFire.Game.Map.FireStuff;
using BushFire.Game.Tech;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Game.Map
{
    class WorldFire
    {
        //once we tested the adding
        //we need to use threaded actions


        //so we have 10 lists. how do we find which one to put it in
        //we can itterate throguh, however as things come out they could need smoothing
        //deal with that soon

        int arrayCount = 10;
        int addingCount = 0;
        int currentUpdateCount = 0;
        List<Fire>[] fireArray;
        List<ThreadedFireSpread> threadedFireSpreadList = new List<ThreadedFireSpread>();



        public WorldFire()
        {

            fireArray = new List<Fire>[arrayCount];

            for (int i = 0; i < arrayCount; i++)
            {
                fireArray[i] = new List<Fire>();
            }


        }

     /*   private void debugAdd()
        {
            for (int i = 0; i < 10000; i++)
            {
                AddFire(new Fire(3000, 10, 100, 100));
            }
        }*/

        //technically we should only be adding the fire to the world at the end of the update turn
        //we can fix that later
        public void AddFire(Fire fire)
        {
            fireArray[addingCount].Add(fire);
            addingCount++;
            if (addingCount > arrayCount - 1)
            {
                addingCount = 0;
            }
            WorldController.world.tileGrid[fire.tileX, fire.tileY].fire = fire;
        }

        //can only spread a maxium of 4 places (Gridsize)
        public void SpreadMiniFire(int tileX, int tileY, int miniX, int miniY, int directionX, int directionY)
        {
            //also need to do a check if there is a fire in current place

            miniX += directionX;
            miniY += directionY;

            if (miniX >= Fire.gridSize)
            {
                miniX -= Fire.gridSize;
                tileX += 1;
            }
            else if (miniX < 0)
            {
                miniX += Fire.gridSize;
                tileX -= 1;
            }
            if (miniY >= Fire.gridSize)
            {
                miniY -= Fire.gridSize;
                tileY += 1;
            }
            else if (miniY < 0)
            {
                miniY += Fire.gridSize;
                tileY -= 1;
            }

            //Todo do ou
            //if (WorldController.IsInWorldBounds(tileX, tileY) && !WorldController.world.tileGrid[tileX, tileY].isCompleteBurnt)
            if (WorldController.IsInWorldBounds(tileX, tileY) && !WorldController.world.tileGrid[tileX, tileY].IsFullyBurnt())
            {
                threadedFireSpreadList.Add(new ThreadedFireSpread(tileX, tileY, miniX, miniY));  
            }        
        }




        //This is multitides faster removing from a list than using removeAt
        //We are setting the one we are deleting to the last one in the list
        //then removing the last one
        //List doesnt require a reshuffle
        private void RemoveFireFromUpdateList(int index, List<Fire> fireList)
        {
            int last = fireList.Count - 1;
            if (index != last)
            {
                fireList[index] = fireList[last];
                fireList.RemoveAt(last);
            }
            else
            {
                fireList.RemoveAt(index);
            }
        }

        private void SetTileToBurnt(Fire fire)
        {
            WorldController.world.tileGrid[fire.tileX, fire.tileY].SetTileToFullyBurnt();
        }

        bool first;


        public void UpdateThreadedFireSpreading()
        {
            for (int i = threadedFireSpreadList.Count - 1; i > -1; i--)
            {
                ThreadedFireSpread fireSpread = threadedFireSpreadList[i];

                Fire fire = WorldController.world.tileGrid[fireSpread.tileX, fireSpread.tileY].fire;

                if (fire == null)
                {
                    fire = new Fire(fireSpread.tileX, fireSpread.tileY);
                    AddFire(fire); //need to add the fire before the mini fire for drawing
                    fire.AddMiniFire(fireSpread.miniX, fireSpread.miniY);
                }
                else
                {
                    if (fire.isPartialBurntOut)
                    {
                        //readd it to our list
                        AddFire(fire); //need to add the fire before the mini fire for drawing
                    }
                    fire.AddMiniFire(fireSpread.miniX, fireSpread.miniY);

                    //if its not in the update cycle because its partially burnt
                }

                threadedFireSpreadList.RemoveAt(threadedFireSpreadList.Count - 1);
            }
        }



        //Okay so the issue is
        //if its finished but not completely burnt
        //we dont want to be updating it
        //however it could start up again


        public void Update()
        {

            //     EngineController.debugMiniFires = 0;
            //first = true;
            if (!first)
            {
                first = true;

                Fire fire = new Fire(184, 184);
                AddFire(fire);  //need to add the fire to the map before adding the minifire
                fire.AddMiniFire(0, 0);
            }

            List<Fire> fireList = fireArray[currentUpdateCount];
            EngineController.debugFires = fireList.Count;

            for (int i = fireList.Count - 1; i > -1; i--)
            {
                Fire fire = fireList[i];
                fire.Update();

                if (fire.isCompleteBurntOut && fire.IsFinishedDrawingSmoke())
                {
                    RemoveFireFromUpdateList(i, fireList);
                    SetTileToBurnt(fire);
                }
                else if (fire.isPartialBurntOut && fire.IsFinishedDrawingSmoke())
                {
                    RemoveFireFromUpdateList(i, fireList);
                }
            }


            currentUpdateCount++;
            if (currentUpdateCount > arrayCount - 1)
            {
                currentUpdateCount = 0;
            }
        }
    }
}
