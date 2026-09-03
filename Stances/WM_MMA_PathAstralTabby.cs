using System;
using System.Collections.Generic;
using System.Text;
using XRL.Rules;
using XRL.Messages;
using XRL.UI;
using XRL.World.Effects;
using XRL;
using XRL.World;


namespace XRL.World.Parts.Skill
{
    [Serializable]
    public class WM_MMA_PathAstralTabby : BaseSkill
    {
        public Guid AstralTabbyStanceID;
        private int FlankersAboundDuration;

        public int AdjacentHostiles = 0;

        public int AcumBonuses = 0;

        public WM_MMA_PathAstralTabby()
        {


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


        public override void Register(GameObject Object, IEventRegistrar registrar)
        {
            Object.RegisterPartEvent(this, "AttackerCriticalHit");
            Object.RegisterPartEvent(this, "AttackerHit");
            Object.RegisterPartEvent(this, "DefenderAfterAttackMissed");
            Object.RegisterPartEvent(this, "BeginTakeAction");
            Object.RegisterPartEvent(this, "AstralTabbyStanceCommand");
            Object.RegisterPartEvent(this, "AIGetOffensiveMutationList");
            Object.RegisterPartEvent(this, "ProjectileGetDefenderDV");

            base.Register(Object, registrar);
        }

        public override bool WantEvent(int ID, int cascade)
        {
            return base.WantEvent(ID, cascade)
            || ID == GetDefenderHitDiceEvent.ID
            || ID == BeginTakeActionEvent.ID
            || ID == GetMissileWeaponProjectileEvent.ID;
        }
        public override bool HandleEvent(BeginTakeActionEvent E)
        {
            RecountAdjacentHostiles();

            StatShifter.SetStatShift("DV", AdjacentHostiles, false);

            return base.HandleEvent(E);
        }

        public override bool HandleEvent(AttackerDealingDamageEvent E)
        {
            if (E.Actor == ParentObject && ParentObject.HasEffect("AstralTabbyStance"))
            { E.Damage.Amount = (E.Damage.Amount / 2); }

            return base.HandleEvent(E);
        }

        public void RecountAdjacentHostiles()
        {
            AdjacentHostiles = 0;

            foreach (var C in ParentObject.CurrentCell.GetLocalAdjacentCells())
                foreach (var Object in C.Objects)
                {
                    if (Object.IsCombatObject() && Object.IsHostileTowards(ParentObject))
                    {
                        AdjacentHostiles++;
                    }
                }
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
                    MMAComboAccess.UpdateCounter();
                }
            }
            else if (E.ID == "AttackerHit" && ParentObject.HasEffect("AstralTabbyStance"))
            {
                Damage Damage = E.GetParameter<Damage>("Damage");
                Damage.Amount = (int)Math.Floor(Damage.Amount * 0.75);
            }
            else if (E.ID == "BeginTakeAction" && ParentObject.HasEffect<AstralTabbyStance>())
            {

            }
            else if (E.ID == "TargetedForMissileWeapon" && ParentObject.HasEffect<AstralTabbyStance>())
            {

            }
            else if (E.ID == "ProjectileGetDefenderDV" && ParentObject.HasEffect<AstralTabbyStance>())
            {
                E.SetParameter("DV", +5);
            }
            return base.FireEvent(E);
        }
    }
}
