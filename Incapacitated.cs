// XRL.World.Effects.Paralyzed
using System;
using XRL.Core;
using XRL.Rules;
using XRL.UI;
using XRL.World;


namespace XRL.World.Effects
{
    [Serializable]
    public class Incapacitated : Effect
    {
        public int DVPenalty;

        public int SaveTarget;

        public Incapacitated()
        {
            base.DisplayName = "{{R|incapacitated}}";
        }

        public Incapacitated(int Duration, int SaveTarget)
            : this()
        {
            this.SaveTarget = SaveTarget;
            base.Duration = Duration;
        }

        public override int GetEffectType()
        {
            return 117440516;
        }

        public override bool SameAs(Effect e)
        {
            Incapacitated incapacitated = e as Incapacitated;
            if (incapacitated.DVPenalty != DVPenalty)
            {
                return false;
            }
            if (incapacitated.SaveTarget != SaveTarget)
            {
                return false;
            }
            return base.SameAs(e);
        }

        public override string GetDetails()
        {
            if (DVPenalty < 0)
            {
                return "Unconscious, they cannot move or attack.\n" + DVPenalty + " DV";
            }
            return "Can't move or attack.\nDV set to 0.";
        }

        public override bool Apply(GameObject Object)
        {
            if (Object.HasEffect("Incapacitated"))
            {
                Incapacitated incapacitated = Object.GetEffect("Incapacitated") as Incapacitated;
                if (Duration > incapacitated.Duration)
                {
                    incapacitated.Duration = Duration;
                }
                return true;
            }
            if (Object.FireEvent("ApplyIncapacitate"))
            {
                ApplyStats();
                DidX("has been", "incapacitated", "!", null, null, Object);
                Object.ParticleText("&R*KO'd!!!*");
                return true;
            }
            return false;
        }

        public override void Remove(GameObject Object)
        {
            UnapplyStats();
            base.Remove(Object);
        }

        private void ApplyStats()
        {
            int combatDV = Stats.GetCombatDV(base.Object);
            if (combatDV > 0)
            {
                DVPenalty += combatDV;
                base.StatShifter.SetStatShift(base.Object, "DV", -DVPenalty);
            }
            else
            {
                DVPenalty = 0;
            }
        }

        private void UnapplyStats()
        {
            DVPenalty = 0;
            base.StatShifter.RemoveStatShifts(base.Object);
        }

        public override void Register(GameObject Object)
        {
            Object.RegisterEffectEvent(this, "AfterDeepCopyWithoutEffects");
            Object.RegisterEffectEvent(this, "BeforeDeepCopyWithoutEffects");
            Object.RegisterEffectEvent(this, "BeginTakeAction");
            Object.RegisterEffectEvent(this, "CanChangeBodyPosition");
            Object.RegisterEffectEvent(this, "CanChangeMovementMod");
            Object.RegisterEffectEvent(this, "CanMoveExtremities");
            Object.RegisterEffectEvent(this, "IsMobile");
            base.Register(Object);
        }

        public override void Unregister(GameObject Object)
        {
            Object.UnregisterEffectEvent(this, "AfterDeepCopyWithoutEffects");
            Object.UnregisterEffectEvent(this, "BeforeDeepCopyWithoutEffects");
            Object.UnregisterEffectEvent(this, "BeginTakeAction");
            Object.UnregisterEffectEvent(this, "CanChangeBodyPosition");
            Object.UnregisterEffectEvent(this, "CanChangeMovementMod");
            Object.UnregisterEffectEvent(this, "CanMoveExtremities");
            Object.UnregisterEffectEvent(this, "IsConversationallyResponsive");
            Object.UnregisterEffectEvent(this, "IsMobile");
            base.Unregister(Object);
        }

        public override bool Render(RenderEvent E)
        {
            int num = XRLCore.CurrentFrame % 60;
            if (num > 15 && num < 30)
            {
                E.Tile = null;
                E.RenderString = "KO";
                E.ColorString = "&R^r";
            }
            return true;
        }

        public override bool FireEvent(Event E)
        {
            if (E.ID == "BeginTakeAction")
            {
                if (Duration > 0)
                {
                    if (base.Object.IsPlayer())
                    {
                        IComponent<GameObject>.AddPlayerMessage("You are {{r|incapacitated}}.");
                    }
                    Duration--;
                    return false;
                }
            }
            else if (E.ID == "IsMobile" || E.ID == "IsConversationallyResponsive")
            {
                if (Duration > 0)
                {
                    return false;
                }
            }
            else if (E.ID == "CanChangeBodyPosition" || E.ID == "CanChangeMovementMode" || E.ID == "CanMoveExtremities")
            {
                if (Duration > 0 && !E.HasFlag("Involuntary"))
                {
                    if (E.HasFlag("ShowMessage") && base.Object.IsPlayer())
                    {
                        Popup.Show("You are {{R|incapacitated}}!");
                    }
                    return false;
                }
            }
            else if (E.ID == "BeforeDeepCopyWithoutEffects")
            {
                UnapplyStats();
            }
            else if (E.ID == "AfterDeepCopyWithoutEffects")
            {
                ApplyStats();
            }
            return base.FireEvent(E);
        }
    }
}