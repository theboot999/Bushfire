using BushFire.Game.Tech.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Game.Tech
{
    abstract class Job
    {
        //so we create the job in the vehicle
        //which holds a reference to the job
        //every game tick it checks if the job is done
        //if its done
        //it grabs the job and grabs what it needs from it
        //then removes the reference to the job


        public volatile bool completed;
        public volatile bool isCancel;

        public Job()
        {
            completed = false;
        }

        //can cancel the job if it has not been started yet
        public void CancelJob()
        {
            isCancel = true;
        }


        public virtual void Start(Reusables reusables)
        {
         
        }
    }
}
