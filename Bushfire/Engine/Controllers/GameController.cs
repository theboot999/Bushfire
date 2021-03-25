using BushFire.Game.Tech;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Game.Controllers
{


    static class GameController
    {
        //Random
        public static Random rnd = new Random();
        public static int seed;
        public static volatile InGameState inGameState;

        public static bool editorInMenu; //This is currently used for editor will move it
        public static JobWorker jobWorker;

        public static Random GetRandomWithSeed()
        {
            return new Random(seed);
        }

        public static void AddJob(Job job)
        {
            jobWorker.AddJob(job);
        }


    }

    enum InGameState
    {
        RUNNING,
        INMENU,
        WAITINGONEXIT,
        SETTINGEXIT,
        EXIT
    }
}
