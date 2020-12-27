using System;
using XRL.UI;
using XRL.Core;
using XRL.Rules;

using System.Collections.Generic;
using System.Text;
using XRL.World.Parts;

namespace XRL.World.Effects
{
    [Serializable]

    public class SaltHopperStance : Effect
    {
        public int saveTarget;
        public int saveTargetTurnDivisor;
        public int turns;


        public SaltHopperStance() : base()
        {
            base.DisplayName = "{{orange|Way of the Salt-Hopper}}";
        }

        public SaltHopperStance(int Duration) : this()
        {
            base.Duration = 1;
            turns = 0;
        }

        public override string GetDetails()
        {
            return "Sudden, high precision strikes that target sensitive areas on the foe; melee damage switches to agility, cannot build combinations strikes and only perform one attack per turn, however each blow has a 10% chance of adding either Immobilize, Confuse, Stun, Daze or Crippled/Hobbled to your enemy.\n";
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


        // public void PsychicPulse()
        // {
        //     for (int i = 0; i < 4; i++)
        //     {
        //         for (int j = 0; j < 5; j++)
        //         {
        //             ParticleText("&B" + (char)(219 + Stat.Random(0, 4)), 4.9f, 5);
        //         }
        //         for (int k = 0; k < 5; k++)
        //         {
        //             ParticleText("&b" + (char)(219 + Stat.Random(0, 4)), 4.9f, 5);
        //         }
        //         for (int l = 0; l < 5; l++)
        //         {
        //             ParticleText("&W" + (char)(219 + Stat.Random(0, 4)), 4.9f, 5);
        //         }
        //     }
        // }


        public void SaltHopperPulse(Cell cell)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    cell.ParticleText("&o" + (char)(219 + Stat.Random(0, 4)), -4.9f, 5);
                }
                for (int k = 0; k < 5; k++)
                {
                    cell.ParticleText("&O" + (char)(219 + Stat.Random(0, 4)), -4.9f, 5);
                }
                for (int l = 0; l < 5; l++)
                {
                    cell.ParticleText("&Y" + (char)(219 + Stat.Random(0, 4)), -4.9f, 5);
                }
            }
        }


        public override bool Apply(GameObject Object)
        {
            SaltHopperPulse(Object.CurrentCell);
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

            return base.FireEvent(E);
        }

        public override bool Render(RenderEvent E)
        {
            if (this.Duration > 0)
            {
                int num = XRLCore.CurrentFrame % 60;
                if (num > 25 && num < 22)
                {
                    E.ColorString = "&o";
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