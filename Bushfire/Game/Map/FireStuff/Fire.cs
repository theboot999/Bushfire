using BushFire.Engine;
using BushFire.Engine.ContentStorage;
using BushFire.Engine.Controllers;
using BushFire.Game.Controllers;
using BushFire.Game.Storage;
using BushFire.Game.Tech;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Game.Map.FireStuff
{
    class Fire
    {
        //So i think the better option
        //would be to have a global list of smoke
        //well i dont know know
        



        //So TODO:
        //add in fade in if its time0. or then just instantly reappear
        //add in red circles to represent flame temporarily
        //add in fade out
        //play around with colour sizes
        //play around with adding larger smokes if a 2 x 2 quadrant is on fire
        //coudl even go 4 x4


        //down the track add flames
        //add a wind and direction
        //finetune values for spreading and for smoke
        //perhaps the fire could slowly spread over the mini tile by using a percent coverage in a direction
        //perhaps not. but would have to look at all directions. not sure how to work that one out

        //So for smoke and flames
        //so if we suddnely move our screen over a burning fire we need to advance the state of smoke
        //however if its a new fire the smoke has to start brand new
        //we do have an is being drawn flag in our fire
        //so somehow we can use that
        //our minifire needs a timer to release smoke though
        //but ideally if we have a very big fire, like half the tile is on fire we kind of want to release smoke as bigger particles to go faster
        //so 
        //if we need to turn on draw, we need to add multiple smokes at various stages
        //but we need to keep releasing smoke, each minifire needs a few puffs
        //so really when adding smoke we need to specify its life span between 0 and 1
        //1 is almost dead
        //but our fade and drawing needs to be off the same
        //so we need to specify our maximum size what the smoke will be when its about to fade
        //and also the maximum movement from the wind
        //so our movement can be

        //calculate our windspeed and direction
        //our final location will be 

        //but to make things awkward
        //we dont want to load up our final location at the start of a new smoke release thats fresh because if the wind suddenly changes we want that to change
        //so the whole idea of initially starting the smoke is simply a 0 and 1 lerp to its final location with that wind

        //when updating a fire truck if its watering a fire(it gets the fire from the tile) it goes into a master list of jobs to do after the multithreaded update loop
        //we go through this list and then update the amount of water being thrown on that fire.  (if multiple trucks then we simply add onto the amount of water)
        //then next update loop on the fire this is calculated in the intensity.
        //after each update loop on the fire we need to zero out this water being thrown on it
        //this way we should overlap with our multithreading

        //im not sure if we need to do this too with when our fire is spreading and wants to land on another tile
        //i wouldnt do the ember spreading update in that list, only just the landing

        //At the end of the update loop
        public const int gridSize = 4;
        public const int totalPerGrid = 16;
        const float gridSquare = 32;
        const float halfGridSquare = 16;

        public int tileX { get; private set; }
        public int tileY { get; private set; }

        public MiniFire[,] miniFireList { get; private set; }


        float fuel;
        float intensity;

        public float calculatedIntensity { get; private set; }
        public bool isCompleteBurntOut;
        public bool isPartialBurntOut;

        int miniFireCount;
        public int miniFireCurrentBurning;

        private bool updateTick;
        private bool drawTick;
        private bool isBeingDrawn = false;


        private List<SmokeParticle> smokeParticleList;

        float topLeftMiniXCenter;
        float topLeftMiniYCenter;

        //so every update.  updateTick = !updateTick;

        //so every draw
        //if !isBeingDrawn
        //isBeingDrawn = updateTick;
        //isBeingDrawn





        public Fire(int tileX, int tileY)
        {
            miniFireList = new MiniFire[gridSize, gridSize];

            topLeftMiniXCenter = (tileX * GroundLayerController.tileSize) + 16;
            topLeftMiniYCenter = (tileY * GroundLayerController.tileSize) + 16;


            //Get the fuel and the intensity from the tileLogisticslayer
            fuel = 20000;
            intensity = 100f;
            this.tileX = tileX;
            this.tileY = tileY;

            if (fuel > 0f)
            {
                // {
                //      AddMiniFire(miniFirePoint.X, miniFirePoint.Y);
                //    }
            }
            else
            {
                isCompleteBurntOut = true;
            }
        }

        public void AddMiniFire(int miniX, int miniY)
        {
            if (miniFireList[miniX, miniY] == null)
            {
                miniFireList[miniX, miniY] = new MiniFire(fuel, this, miniX, miniY);
                miniFireCount++;
                miniFireCurrentBurning++;
                isPartialBurntOut = false;
            }
            WorldController.world.tileGrid[tileX, tileY].SetBurntLayer();
        }

        public int GetBurntLayerBitMaskId()
        {
            int index = 0;
            int counter = 1;

            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 4; y++)
                {
                    if (miniFireList[x,y] != null)
                    {
                        index += counter;
                    }
                    counter *= 2;
                }
            }
            return index;
        }


        private void UpdateMiniFires()
        {
            calculatedIntensity = intensity * EngineController.gameUpdateTime;

            if (miniFireCurrentBurning > 0)
            {
                for (int x = 0; x < gridSize; x++)
                {
                    for (int y = 0; y < gridSize; y++)
                    {
                        MiniFire miniFire = miniFireList[x, y];

                        if (miniFire != null && !miniFire.isBurnt)
                        {
                            miniFire.Update();


                            if (isBeingDrawn & miniFire.releaseSmoke)
                            {
                                AddSmoke(x, y);
                            }
                            if (miniFire.isBurnt)
                            {
                                miniFireCurrentBurning -= 1;
                            }
                        }
                    }
                }
            }
            else
            {
                if (miniFireCount == totalPerGrid)
                {
                    isCompleteBurntOut = true;
                }
                else
                {
                    isPartialBurntOut = true;
                }

            }
        }

        public bool IsFinishedDrawingSmoke()
        {
            if (smokeParticleList != null)
            {
                return smokeParticleList.Count == 0;
            }
            return true;
        }

        private void TurnOffDraw()
        {
            isBeingDrawn = false;
            smokeParticleList.Clear();
            smokeParticleList = null;
            //kill off any fire and smoke
        }

        private void TurnOnDraw()
        {
            smokeParticleList = new List<SmokeParticle>();
            isBeingDrawn = true;

            for (int miniX = 0; miniX < gridSize; miniX++)
            {
                for (int miniY = 0; miniY < gridSize; miniY++)
                {
                    if (miniFireList[miniX, miniY] != null)
                    {
                        AddSmoke(miniX, miniY);
                    }
                }
            }
        }

        private void AddSmoke(int miniX, int miniY)
        {
            Vector2 location = new Vector2((miniX * gridSquare) + topLeftMiniXCenter, (miniY * gridSquare) + topLeftMiniYCenter);
            SmokeParticle smokeParticle = new SmokeParticle(location, new Vector2(64, 64), Color.White, 0f);
            smokeParticleList.Add(smokeParticle); 
        }

        public void Update()
        {
            if (isBeingDrawn && updateTick != drawTick)
            {
                TurnOffDraw();
            }
            updateTick = !updateTick;
            UpdateMiniFires();
        }

        //put these in a seperate list later
        //Also only wont to kill the fires when all minifires when all the smoke is gone
        private void DrawFires(SpriteBatch spriteBatch)
        {
            if (miniFireCurrentBurning > 0)
            {
                //draw circles for the flame
                //however i think we will  have the flames as particles in our list
                Vector2 location = new Vector2((tileX * GroundLayerController.tileSize) + halfGridSquare, (tileY * GroundLayerController.tileSize) + halfGridSquare);

                for (int x = 0; x < gridSize; x++)
                {
                    for (int y = 0; y < gridSize; y++)
                    {
                        MiniFire miniFire = miniFireList[x, y];

                        if (miniFire != null)
                        {
                            miniFire.Draw(spriteBatch, new Vector2(location.X + (x * gridSquare), location.Y + (y * gridSquare)));
                        }
                    }
                }
            }
        }

        private void DrawSmoke(SpriteBatch spriteBatch)
        {
            for (int i = smokeParticleList.Count - 1; i > -1; i--)
            {
                SmokeParticle smokeParticle = smokeParticleList[i];
                smokeParticle.DrawUpdate();
                smokeParticle.Draw(spriteBatch);

                if (smokeParticle.destroy)
                {
                    smokeParticleList.RemoveAt(i);
                }
            }
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            if (!isBeingDrawn)
            {
                TurnOnDraw();
            }
            drawTick = updateTick;

            DrawFires(spriteBatch);
            DrawSmoke(spriteBatch);
            
     

        
        }


    }
}
