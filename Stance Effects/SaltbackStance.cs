using System;
using XRL.UI;
using XRL.Core;
using System.Collections.Generic;
using System.Text;
using XRL.World.Parts;
using XRL.Rules;

namespace XRL.World.Effects
{
    [Serializable]

    public class SaltbackStance : Effect
    {
        public int saveTarget;
        public int saveTargetTurnDivisor;
        public int turns;


        public SaltbackStance() : base()
        {
            base.DisplayName = "{{brown|Way of the Salt-Back}}";
        }

        public SaltbackStance(int Duration) : this()
        {
            base.Duration = 1;
            turns = 0;
        }

        public override string GetDetails()
        {
            return "Sacrifice attack power for defensive options, you can now deflect enemies blows with a 20% chance of adding your toughness to your AC on a sucessful block. Successful blocks count towards your Combination Strikes however you now deal 50% less damage and your mability is reduced 10. [Wearing bracers or wristblades increases this chance 10%.]\n";
        }

        public override void Register(GameObject go)
        {
            go.RegisterEffectEvent((Effect)this, "MovementModeChanged");

            base.Register(Object);
        }

        public override void Unregister(GameObject go)
        {
            go.UnregisterEffectEvent((Effect)this, "MovementModeChanged");

            base.Unregister(Object);
        }

        public void SaltBackPulse(Cell cell)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    cell.ParticleText("&Y" + (char)(219 + Stat.Random(0, 4)), -4.9f, 2);
                }
                for (int k = 0; k < 5; k++)
                {
                    cell.ParticleText("&w" + (char)(219 + Stat.Random(0, 4)), -4.9f, 2);
                }
                for (int l = 0; l < 5; l++)
                {
                    cell.ParticleText("&W" + (char)(219 + Stat.Random(0, 4)), -4.9f, 2);
                }
            }
        }

        public override bool Apply(GameObject Object)
        {
            var ParentMoveSpeed = Object.Statistics["MoveSpeed"].BaseValue;

            PlayWorldSound("swapstance", PitchVariance: 0f);
            SaltBackPulse(Object.CurrentCell);
            StatShifter.SetStatShift("MoveSpeed", -10);

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
                    E.ColorString = "&w";
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