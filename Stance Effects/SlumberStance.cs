using System;
using XRL.UI;
using XRL.Core;
using System.Collections.Generic;
using System.Text;
using XRL.World.Parts;



namespace XRL.World.Effects
{
    [Serializable]

    public class SlumberStance : Effect
    {
        public Guid SlumberID;
        public int ShockwaveCooldown;



        public SlumberStance() : base()
        {
            base.DisplayName = "{{dark red|Way of the Slumberling}}";
        }

        public SlumberStance(int Duration) : this()
        {
            base.Duration = 1;
        }

        public override string GetDetails()
        {
            return "Vicious sweeping attacks that harm any in  the practitioners way while throwing away one's defensive abilities. At the cost of half your AV and DV, attacks while unnarmed now deal damage to nearby foes, for every successful hit on an opponent, enemies flanking you must make a penetration save or be dealt 50% of the damage.\n";
        }

        public override void Register(GameObject go)
        {
            go.RegisterEffectEvent((Effect)this, "MovementModeChanged");
            go.RegisterEffectEvent((Effect)this, "CanChangeMovementMode");
            go.RegisterEffectEvent((Effect)this, "EndTurn");
            go.RegisterEffectEvent((Effect)this, "SlumberSleepCommand");
            go.RegisterEffectEvent((Effect)this, "LeaveCell");
            go.RegisterEffectEvent((Effect)this, "BeginTakeAction");
            base.Register(Object);
        }

        public override void Unregister(GameObject go)
        {
            go.UnregisterEffectEvent((Effect)this, "MovementModeChanged");
            go.UnregisterEffectEvent((Effect)this, "CanChangeMovementMode");
            go.UnregisterEffectEvent((Effect)this, "EndTurn");
            go.UnregisterEffectEvent((Effect)this, "SlumberSleepCommand");
            go.UnregisterEffectEvent((Effect)this, "LeaveCell");
            go.UnregisterEffectEvent((Effect)this, "BeginTakeAction");
            base.Unregister(Object);
        }

        public override bool FireEvent(Event E)
        {
            if (E.ID == "SlumberSleepCommand")
            {
                if (!Object.HasEffect("Asleep"))
                {
                    Object.ApplyEffect(new Asleep(1200 - (Object.Statistics["Toughness"].Modifier * 10), forced: true, quicksleep: false, voluntary: true));
                    AddPlayerMessage("You fall into a deep slumber ...");
                }
            }

            return base.FireEvent(E);
        }

        public void RageSplat()
        {
            Cell currentCell = Object.GetCurrentCell();
            if (currentCell != null && currentCell.InActiveZone)
            {
                for (int i = 0; i < 360; i++)
                {
                    float num = (float)XRL.Rules.Stat.RandomCosmetic(1, 10) / 3f;
                    XRLCore.ParticleManager.Add("&R^r*", currentCell.X, currentCell.Y, (float)Math.Sin((double)i * 0.017) / num, (float)Math.Cos((double)i * 0.017) / num);
                }
            }
        }

        public override bool Apply(GameObject Object)
        {
            AddPlayerMessage("{{dark red|With a bloodchilling roar, you unleash your slumbering fury!}}");
            RageSplat();

            var ParentAV = Object.Statistics["AV"].Value;
            var ParentDV = Object.Statistics["DV"].Value;

            PlayWorldSound("swapstance", PitchVariance: 0.5f);

            StatShifter.SetStatShift("AV", -(int)Math.Round(ParentAV * 0.5));
            StatShifter.SetStatShift("DV", -(int)Math.Round(ParentDV * 0.5));

            this.SlumberID = base.AddMyActivatedAbility("Slumber", "SlumberSleepCommand", "Power", "Fall into a deep slumber, regenerating your hitpoints.", "Z", null, false, false, true);

            return true;
        }

        public override bool Render(RenderEvent E)
        {
            if (this.Duration > 0)
            {
                int num = XRLCore.CurrentFrame % 60;
                if (num > 25 && num < 35)
                {
                    E.ColorString = "&r";
                }
            }
            return base.Render(E);
        }

        public override void Remove(GameObject Object)
        {
            this.RemoveMyActivatedAbility(ref SlumberID, Object);
            StatShifter.RemoveStatShifts();
        }

    }
}