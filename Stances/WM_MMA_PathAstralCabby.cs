using System;
using System.Collections.Generic;
using System.Text;
using XRL.Rules;
using XRL.Messages;
using XRL.UI;

namespace XRL.World.Parts.Skill
{
    [Serializable]
    public class WM_MMA_PathAstralCabby : BaseSkill
    {
        public bool Targeted = false;
        public Guid AstralCabbyStanceID;
        private int FlankersAboundDuration;

        public WM_MMA_PathAstralCabby()
        {
            Name = "WM_MMA_PathAstralCabby";
            DisplayName = "Path of the Astral Cabby";
        }

        public override bool AddSkill(GameObject GO)
        {
            this.AstralCabbyStanceID = base.AddMyActivatedAbility("Way of the Astral Cabby", "AstralCabbyStanceCommand", "Skill", "Whenever you launch an attack with either your bare hands or natural weapon.", "*", null, false, false, true);

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
            Object.RegisterPartEvent(this, "AttackerMeleeMiss");
            Object.RegisterPartEvent(this, "BeginTakeAction");
            Object.RegisterPartEvent(this, "EndTurn");
            Object.RegisterPartEvent(this, "TargetedForMissileWeapon");

            base.Register(Object);
        }

        public override bool FireEvent(Event E)
        {
            if (E.ID == "AttackerMeleeMiss" && ParentObject.HasEffect("AstralCabbyStance"))
            {
                var Defender = E.GetGameObjectParameter("Defender");
                var Weapon = E.GetGameObjectParameter("Weapon");

                if (Defender == ParentObject)
                {
                    var MMAComboAccess = ParentObject.GetPart<WM_MMA_CombinationStrikesI>();

                    MMAComboAccess.CurrentComboICounter += 1;
                }
            }
            if (E.ID == "AttackerHit" && ParentObject.HasEffect("AstralCabbyStance"))
            {
                Damage Damage = E.GetParameter<Damage>("Damage");
                Damage.Amount = (int)Math.Floor(Damage.Amount * 0.75);
            }
            if (E.ID == "BeginTakeAction" && ParentObject.HasEffect("AstralCabbyStance"))
            {
                var ParentCell = ParentObject.CurrentCell.GetLocalAdjacentCells();

                foreach (var C in ParentCell)
                {
                    if (C.HasCombatObject())
                    {
                        FlankersAboundDuration = 7;

                        StatShifter.SetStatShift("DV", +1);
                    }
                }
            }
            if (E.ID == "TargetedForMissileWeapon" && ParentObject.HasEffect("AstralCabbyStance"))
            {
                GameObject Attacker = E.GetGameObjectParameter("Attacker");
                GameObject Defender = E.GetGameObjectParameter("Defender");

                if (Defender == ParentObject)
                {
                    Targeted = true;
                    StatShifter.SetStatShift("DV", +ParentObject.Statistics["Agility"].Modifier + (ParentObject.Statistics["Level"].BaseValue / 8));
                }
            }
            if (E.ID == "EndTurn" && ParentObject.HasEffect("AstralCabbyStance"))
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
