using BushFire.Game.Controllers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Engine.Controllers
{
    class RandomController
    {
        public static float GetRandomFloat(double min, double max)
        {
            return (float)(GameController.rnd.NextDouble() * (max - min) + min);    
        }




        public static bool GetRandomBool()
        {
            return GameController.rnd.Next(0, 2) == 0;
        }

        public static Color GetRandomColour()
        {
            int r = GameController.rnd.Next(0, 255);
            int g = GameController.rnd.Next(0, 255);
            int b = GameController.rnd.Next(0, 255);
            return new Color(r, g, b);
        }

        public static Color GetRandomGrey()
        {
            int r = GameController.rnd.Next(0, 255);
            return new Color(r, r, r);
        }

        public static Color GetRandomWhite()
        {
            int r = GameController.rnd.Next(220, 255);
            return new Color(r, r, r);
        }
    }
}
