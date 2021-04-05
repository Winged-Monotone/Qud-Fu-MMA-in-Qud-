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

using FlamingHandsMutationPart = XRL.World.Parts.Mutation.FlamingHands;
using MMA_ComboStrikeI = XRL.World.Parts.Skill.WM_MMA_CombinationStrikesI;

using HistoryKit;

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

            return base.Apply(Object);
        }

        public override void Remove(GameObject Object)
        {
            if (Object.HasEffect("Omniphase"))
            {
                Object.RemoveEffect("Omniphase");
            }
            if (Object.HasEffect("Rubbergum_Tonic"))
            {
                Object.RemoveEffect("Rubbergum_Tonic");
            }

            StatShifter.RemoveStatShifts();
        }

        public List<string> SpecialStance = new List<string>()
        {
            "SlumberStance",
            "DawnStance",
            "SaltbackStance",
            "SalthopperStance",
            "AstralTabbyStance",
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

        public override bool HandleEvent(AttackerDealingDamageEvent E)
        {
            var Parent = E.Actor;
            var Target = E.Object;
            var eDamage = E.Damage;

            var ParentsLevel = Parent.Statistics["Level"].BaseValue;

            if (Parent.HasPart("MartialBody") && Parent == Object && Parent.HasEffect("DawnStance") && eDamage.HasAttribute("Fire"))
            {
                var ComboSystem = Object.GetPart<WM_MMA_CombinationStrikesI>();

                eDamage.Amount = (int)Math.Round(eDamage.Amount + ((ComboSystem.CurrentComboICounter * 0.025) * E.Damage.Amount));
            }

            return base.HandleEvent(E);
        }

        public override bool FireEvent(Event E)
        {
            if (E.ID == "BeginTakeAction")
            {
                if (Duration > 0)
                { Duration--; }
                if (Object.HasEffect("DawnStance"))
                {

                    if (Stat.Random(1, 100) <= 10)
                    {
                        var ComboSystem = Object.GetPart<WM_MMA_CombinationStrikesI>();

                        Object.Firesplatter();


                        if (FlamingHandsMutationPart.Cast(null, ((Object.Statistics["Level"].BaseValue) / 2 + "-" + ((Object.Statistics["Level"].BaseValue) / 2 + ComboSystem.CurrentComboICounter))))
                        { AddPlayerMessage("{{orange|You belch a great stream of searing flames at your foe.}}"); }
                        else
                        {
                            return base.FireEvent(E);
                        }

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
                        Object.Heal(+Object.Statistics["Toughness"].Modifier);
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
                else if (Object.HasEffect("AstralTabbyStance"))
                {
                    if (!Object.HasEffect("Omniphase"))
                    {
                        AddPlayerMessage(Object.Its + " fists can now cross into the aether.");
                        Object.ApplyEffect(new Omniphase(DURATION_INDEFINITE));
                    }
                }
                else if (Object.HasEffect("SaltbackStance"))
                {
                    if (!Object.HasEffect("Rubbergum_Tonic"))
                    {
                        Object.ApplyEffect(new Rubbergum_Tonic(DURATION_INDEFINITE));

                        Rubbergum_Tonic RubberEffect = Object.GetEffect<Rubbergum_Tonic>();

                        var NewDescript = RubberEffect.GetDescription();
                        NewDescript = "{{w|Shellskin}}";
                        if (RubberEffect.Apply(Object))
                        {
                            var Penalty = (int)(Object.Statistics["MoveSpeed"].Value / 1.5);
                            base.StatShifter.SetStatShift("MoveSpeed", Penalty);
                        }

                    }


                }

            }

            return base.FireEvent(E);

        }

        public override string GetDetails()
        {
            return "The warm draught reveals a hidden genius in your fighting form.";
        }


    }
}