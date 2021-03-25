using BushFire.Engine;
using BushFire.Game.Tech;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Game.Vehicles
{
    enum ActionState
    {
        None,
        InProgress,
        Cancelling,
        Finished
    }

    abstract class VAction
    {
        public bool ignoreDraw { get; protected set; }
        public ActionState actionState { get; protected set; }
        public Point destinationPoint;
        public VehicleState vehicleState { get; protected set; }

        public virtual void StartAction()
        {

        }

        public virtual void CancelAction()
        {
            
        }

        public virtual void Update()
        {

        }
    }
}
