using System;
using XRL.UI;
using XRL.Core;
using System.Collections.Generic;
using System.Text;
using XRL.World.Parts;

namespace XRL.World.Effects
{
    [Serializable]

    public class DawnStance : Effect
    {
        public int saveTarget;
        public int saveTargetTurnDivisor;
        public int turns;


        public DawnStance() : base()
        {
            base.DisplayName = "{{purple|Way of the Dawnglider}}";
        }

        public DawnStance(int Duration)
        {
            base.Duration = 1;
            turns = 0;
        }

        public override string GetDetails()
        {
            return "You are currently molting your exoskeleton.\n";
        }

        public override void Register(GameObject go)
        {
            go.RegisterEffectEvent((Effect)this, "MovementModeChanged");
            go.RegisterEffectEvent((Effect)this, "CanChangeMovementMode");
            go.RegisterEffectEvent((Effect)this, "EndTurn");
            go.RegisterEffectEvent((Effect)this, "IsMobile");
            go.RegisterEffectEvent((Effect)this, "LeaveCell");
            go.RegisterEffectEvent((Effect)this, "BeginTakeAction");
            base.Register(Object);
        }

        public override void Unregister(GameObject go)
        {
            go.UnregisterEffectEvent((Effect)this, "MovementModeChanged");
            go.UnregisterEffectEvent((Effect)this, "CanChangeMovementMode");
            go.UnregisterEffectEvent((Effect)this, "EndTurn");
            go.UnregisterEffectEvent((Effect)this, "IsMobile");
            go.UnregisterEffectEvent((Effect)this, "LeaveCell");
            go.UnregisterEffectEvent((Effect)this, "BeginTakeAction");
            base.Unregister(Object);
        }

        public override bool Apply(GameObject Object)
        {
            return true;
        }

        public override bool FireEvent(Event E)
        {
            if (E.ID == "IsMobile")
            {

            }

            else if (E.ID == "BeginTakeAction")
            {

            }

            else if (E.ID == "LeaveCell")
            {

            }

            else if (E.ID == "EndTurn")
            {

            }

            else if (E.ID == "CanChangeMovementMode")
            {

            }

            else if (E.ID == "MovementModeChanged")
            {

            }

            return base.FireEvent(E);
        }

        public override bool Render(RenderEvent E)
        {
            if (this.Duration > 0)
            {
                int num = XRLCore.CurrentFrame % 60;
                if (num > 25 && num < 35)
                {
                    E.ColorString = "&p";
                }
            }
            return base.Render(E);
        }

        public override void Remove(GameObject Object)
        {
        }

    }
}