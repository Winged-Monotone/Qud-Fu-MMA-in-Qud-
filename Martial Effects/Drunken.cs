using System;
using System.Collections.Generic;
using XRL.Core;
using XRL.Rules;
using XRL.UI;
using XRL.World;
using XRL.Liquids;
using XRL.World.Parts;
using System.Linq;
using XRL.World.Parts.Mutation;
using XRL.World.Parts.Skill;

using MutationPart = XRL.World.Parts.Mutation.FlamingHands;



namespace XRL.World.Effects
{
    [Serializable]
    public class Drunken : Effect
    {
        public int JumpBonus;

        public Drunken()
        {
            base.DisplayName = "{{M|Drunken}}";
        }

        public Drunken(int Duration)
            : this()
        {
            base.Duration = Duration;
        }

        public override bool Apply(GameObject Object)
        {
            if (Object.HasEffect("Drunken"))
            {
                Drunken Drunken = Object.GetEffect("Drunken") as Drunken;
                if (Duration > Drunken.Duration)
                {
                    Drunken.Duration = Duration;
                }
                return true;
            }

            return true;
        }


        public List<string> SpecialStance = new List<string>()
        {
            "SlumberStance",
            "DawnStance",
            "SaltbackStance",
            "SalthopperStance",
            "AstralCabbyStance",
        };
        public void GetDrunkenEffects()
        {
            if (!SpecialStance.Any(Object.HasEffect))
            {

            }
        }
        public override void Register(GameObject Object)
        {

            Object.RegisterEffectEvent(this, "BeginTakeAction");
            Object.RegisterEffectEvent(this, "CanChangeBodyPosition");
            Object.RegisterEffectEvent(this, "CanChangeMovementMod");
            Object.RegisterEffectEvent(this, "CanMoveExtremities");
            Object.RegisterEffectEvent(this, "IsMobile");
            base.Register(Object);
        }

        public override void Unregister(GameObject Object)
        {

            Object.UnregisterEffectEvent(this, "BeginTakeAction");
            Object.UnregisterEffectEvent(this, "CanChangeBodyPosition");
            Object.UnregisterEffectEvent(this, "CanChangeMovementMod");
            Object.UnregisterEffectEvent(this, "CanMoveExtremities");
            Object.UnregisterEffectEvent(this, "IsMobile");
            base.Unregister(Object);
        }

        public override bool FireEvent(Event E)
        {
            if (E.ID == "BeginTakeAction")
            {
                if (Object.HasEffect("DawnStance"))
                {

                    if (Stat.Random(1, 100) <= 10)
                    {
                        Object.Firesplatter();
                        MutationPart.Cast(null, "5-6");
                        if (!Object.HasEffect("Blaze_Tonic"))
                        {
                            Object.ApplyEffect(new Blaze_Tonic());

                        }
                    }
                }
                else if (Object.HasEffect("SlumberStance"))
                {
                    if (Object.HasEffect<Asleep>())
                    {
                        Object.Heal(Object.Statistics["Toughness"].Modifier);
                    }
                }
                else if (Object.HasEffect("SaltHopperStance"))
                {
                    try
                    {
                        var JumpHigh = Object.GetPart<Acrobatics_Jump>();
                        JumpBonus = Object.Statistics["Agility"].Modifier;
                        JumpHigh.MaxDistance = +JumpBonus;
                    }
                    catch
                    {

                    }
                }
                // else if (Object.HasEffect("SaltbackStance"))
                // {

                // }
                // else if (Object.HasEffect("AstralCabbyStance"))
                // {

                // }

            }

            return base.FireEvent(E);

        }

        public override string GetDetails()
        {
            return "The warm draught reveals a hidden genius in your fighting form.";
        }


    }
}