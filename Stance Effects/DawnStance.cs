using System;
using XRL.UI;
using XRL.Rules;
using XRL.Core;
using System.Collections.Generic;
using System.Text;
using XRL.World.Parts;

namespace XRL.World.Effects
{
    [Serializable]

    public class DawnStance : Effect
    {
        public DawnStance() : base()
        {
            base.DisplayName = "{{purple|Way of the Dawnglider}}";
        }

        public DawnStance(int Duration) : this()
        {
            base.Duration = 1;
        }

        public override string GetDetails()
        {
            return "A balanced stance, for those waiting to unleash their inner fire. Dealing successful strikes will add +1 to the 'sure-strike,' command up to a maximum +10. While in this stance you gain a +1 to hit and + 2 to DV\n";
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

        public void DawnPulse(Cell cell)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    cell.ParticleText("&M" + (char)(219 + Stat.Random(0, 4)), -4.9f, 2);
                }
                for (int k = 0; k < 5; k++)
                {
                    cell.ParticleText("&m" + (char)(219 + Stat.Random(0, 4)), -4.9f, 2);
                }
                for (int l = 0; l < 5; l++)
                {
                    cell.ParticleText("&o" + (char)(219 + Stat.Random(0, 4)), -4.9f, 2);
                }
            }
        }

        public override bool Apply(GameObject Object)
        {
            PlayWorldSound("swapstance", PitchVariance: 0f);
            DawnPulse(Object.CurrentCell);

            StatShifter.SetStatShift("DV", 1);
            return true;
        }

        public override bool FireEvent(Event E)
        {
            if (E.ID == "IsMobile")
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
                    E.ColorString = "&m";
                }
            }
            return base.Render(E);
        }

        public override void Remove(GameObject Object)
        {
            StatShifter.RemoveStatShifts();
        }



    }
}