using System;
using System.Collections.Generic;
using System.Text;
using XRL.Rules;
using XRL.Messages;
using XRL.UI;
using XRL.World.Effects;

namespace XRL.World.Parts.Skill
{
    [Serializable]
    public class WM_MMA_PathAstralTabby : BaseSkill
    {
        public bool Targeted = false;
        public Guid AstralTabbyStanceID;
        private int FlankersAboundDuration;

        public WM_MMA_PathAstralTabby()
        {
            Name = "WM_MMA_PathAstralTabby";
            DisplayName = "Path of the Astral Tabby";
        }

        public override bool AddSkill(GameObject GO)
        {
            this.AstralTabbyStanceID = base.AddMyActivatedAbility("Way of the Astral Tabby", "AstralTabbyStanceCommand", "Skill", "Whenever you launch an attack with either your bare hands or natural weapon.", "*", null, false, false, true);

            return true;
        }

        public override bool RemoveSkill(GameObject GO)
        {
            return true;
        }


        public override void Register(GameObject Object)
        {
            Object.RegisterPartEvent(this, "AttackerCriticalHit");
            Object.RegisterPartEvent(this, "AttackerHit");
            Object.RegisterPartEvent(this, "DefenderAfterAttackMissed");
            Object.RegisterPartEvent(this, "BeginTakeAction");
            Object.RegisterPartEvent(this, "AstralTabbyStanceCommand");
            Object.RegisterPartEvent(this, "AIGetOffensiveMutationList");

            base.Register(Object);
        }

        public override bool WantEvent(int ID, int cascade)
        {
            return base.WantEvent(ID, cascade)
            || ID == GetDefenderHitDiceEvent.ID;
        }
        public override bool HandleEvent(AttackerDealingDamageEvent E)
        {
            if (E.Actor == ParentObject && ParentObject.HasEffect("AstralTabbyStance"))
            { E.Damage.Amount = (E.Damage.Amount / 2); }

            return base.HandleEvent(E);
        }

        public override bool FireEvent(Event E)
        {
            if (E.ID == "DefenderAfterAttackMissed" && ParentObject.HasEffect("AstralTabbyStance"))
            {
                var Defender = E.GetGameObjectParameter("Defender");
                var Weapon = E.GetGameObjectParameter("Weapon");

                // AddPlayerMessage("Defender: " + Defender);

                if (Defender == ParentObject)
                {
                    var MMAComboAccess = ParentObject.GetPart<WM_MMA_CombinationStrikesI>();

                    MMAComboAccess.CurrentComboICounter++;
                    // AddPlayerMessage("ComboCounterAmount: " + MMAComboAccess.CurrentComboICounter);
                    MMAComboAccess.UpdateCounter();
                }
            }
            if (E.ID == "AttackerHit" && ParentObject.HasEffect("AstralTabbyStance"))
            {
                Damage Damage = E.GetParameter<Damage>("Damage");
                Damage.Amount = (int)Math.Floor(Damage.Amount * 0.75);
            }
            if (E.ID == "BeginTakeAction" && ParentObject.HasEffect("AstralTabbyStance"))
            {
                var ParentCell = ParentObject.CurrentCell.GetLocalAdjacentCells();

                foreach (var C in ParentCell)
                {
                    if (C.HasCombatObject())
                    {
                        FlankersAboundDuration = 3;

                        StatShifter.SetStatShift("DV", +1);
                    }
                }
            }
            if (E.ID == "TargetedForMissileWeapon" && ParentObject.HasEffect("AstralTabbyStance"))
            {
                GameObject Attacker = E.GetGameObjectParameter("Attacker");
                GameObject Defender = E.GetGameObjectParameter("Defender");

                if (Defender == ParentObject)
                {
                    Targeted = true;
                    StatShifter.SetStatShift("DV", +ParentObject.Statistics["Agility"].Modifier + (ParentObject.Statistics["Level"].BaseValue / 8));
                }
            }
            if (E.ID == "EndTurn" && ParentObject.HasEffect("AstralTabbyStance"))
            {
                if (FlankersAboundDuration > 0)
                {
                    --FlankersAboundDuration;
                }
                else
                {
                    StatShifter.RemoveStatShifts();
                }
                if (Targeted)
                {
                    Targeted = false;
                    StatShifter.RemoveStatShifts();
                }
            }
            return base.FireEvent(E);
        }
    }
}
