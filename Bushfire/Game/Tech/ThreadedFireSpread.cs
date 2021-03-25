using BushFire.Game.Map.FireStuff;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Game.Tech
{
    class ThreadedFireSpread
    {

        public int tileX;
        public int tileY;
        public int miniX;
        public int miniY;


        public ThreadedFireSpread(int tileX, int tileY, int miniX, int miniY)
        {
            this.tileX = tileX;
            this.tileY = tileY;
            this.miniX = miniX;
            this.miniY = miniY;
        }



    }

}
