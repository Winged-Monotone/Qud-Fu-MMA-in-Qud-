using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using XRL.Rules;
using XRL.Messages;
using XRL.UI;


namespace XRL.World.Parts.Skill
{
    [Serializable]
    public class WM_MMA_PathDawnGlider : BaseSkill
    {
        public Guid DawnStanceID;
        public int BonusSureStrike;
        public int FlankersAboundBnsDuration;

        public WM_MMA_PathDawnGlider()
        {
            Name = "WM_MMA_PathDawnGlider";
            DisplayName = "Path of the Dawnglider";
        }

        public override void Register(GameObject Object)
        {
            Object.RegisterPartEvent(this, "AttackerHit");
            Object.RegisterPartEvent(this, "CommandSureStrikes");
            Object.RegisterPartEvent(this, "PerformMeleeAttack");
            Object.RegisterPartEvent(this, "BeginTakeAction");
            Object.RegisterPartEvent(this, "EndTurn");
            base.Register(Object);
        }

        public override bool FireEvent(Event E)
        {
            if (E.ID == "AttackerHit" && ParentObject.HasEffect("DawnStance"))
            {
                if (ParentObject.HasPart("WM_MMA_SureStrikes"))
                {
                    var MMAComboSSAccess = ParentObject.GetPart<WM_MMA_SureStrikes>();

                    //Handles damage scaling.

                    if (BonusSureStrike <= 10)
                    { ++BonusSureStrike; }
                    MMAComboSSAccess.UpdateCounter();
                }
                else
                    try
                    {
                        var salthopperDamageSystem = ParentObject.GetPart<WM_MMA_PathSaltHopper>();
                        Damage Damage = E.GetParameter<Damage>("Damage");
                        var Attacker = ParentObject;


                        if (salthopperDamageSystem.NegEffectsCollectiveTI.Any(Attacker.HasEffect))
                        {
                            Damage.Amount = (int)Math.Round(Damage.Amount * 1.15f);
                        }
                        else if (salthopperDamageSystem.NegEffectsCollectiveTII.Any(Attacker.HasEffect))
                        {
                            Damage.Amount = (int)Math.Round(Damage.Amount * 1.45f);
                        }
                        else if (salthopperDamageSystem.NegEffectsCollectiveTIII.Any(Attacker.HasEffect))
                        {
                            Damage.Amount = (int)Math.Round(Damage.Amount * 2.5f);
                        }
                        else
                        {
                            Damage.Amount = (int)Math.Round(Damage.Amount * 1.0f);
                        }
                    }
                    catch
                    {

                    }
            }
            else if (E.ID == "CommandSureStrikes" && ParentObject.HasEffect("DawnStance"))
            {
                try
                {
                    var MMAComboAccess = ParentObject.GetPart<WM_MMA_SureStrikes>();

                    MMAComboAccess.FistPenBonus = +BonusSureStrike;
                    BonusSureStrike = 0;
                    MMAComboAccess.UpdateCounter();
                }
                catch
                {
                    BonusSureStrike = 0;
                }
            }
            else if (E.ID == "CommandSureStrikes")
            {
                try
                {
                    var MMAComboAccess = ParentObject.GetPart<WM_MMA_SureStrikes>();

                    MMAComboAccess.FistPenBonus = +BonusSureStrike;
                    BonusSureStrike = 0;
                    MMAComboAccess.UpdateCounter();
                }
                catch
                {
                    BonusSureStrike = 0;
                }
            }
            else if (E.ID == "PerformMeleeAttack" && ParentObject.HasEffect("DawnStance"))
            {
                int HitBonus = E.GetIntParameter("HitBonus");

                HitBonus = +1;
            }
            if (E.ID == "GetDefenderHitDice" && ParentObject.HasEffect("DawnStance") && ParentObject.HasSkill("WM_MMA_PathSaltBack"))
            {
                // AddPlayerMessage("dawnSaltBack Defender Block Begins");
                // GameObject Attacker = E.GetGameObjectParameter("Attacker");
                var Owner = ParentObject;

                var SaltBackBlockSystem = ParentObject.GetPart<WM_MMA_PathSaltBack>();

                Body body = Owner.GetPart("Body") as Body;
                List<BodyPart> hands = body.GetPart("Hand");
                var hand = body.GetPrimaryWeaponOfTypeOnBodyPartOfType("DefaultMartialFist", "Hand");

                int FistShieldAV = ParentObject.StatMod("Toughness", 1) + (ParentObject.Statistics["Level"].BaseValue / 4);
                if (SaltBackBlockSystem.SpecialFistCollective.Any(Owner.HasEquippedObject))
                {
                    SaltBackBlockSystem.PSBArmorBonus = 1;
                }

                if (Owner.GetShield() != null)
                {
                    // AddPlayerMessage("SaltBackHalf Shield Returned Null");
                    return true;
                }
                if (E.HasParameter("ShieldBlocked"))
                {
                    // AddPlayerMessage("SaltBackHalf Blocked ParameterSet");
                    return true;
                }
                if (!Owner.CanMoveExtremities(null, false, false, false))
                {
                    // AddPlayerMessage("SaltBackHalf CanMove Check");
                    return true;
                }
                // AddPlayerMessage("SaltBackHalf Block Attempt Random Int");
                if (Stat.Random(1, 100) <= 15 + (5 * (ParentObject.Statistics["Level"].BaseValue / 5)))
                {
                    // AddPlayerMessage("SaltBackHalf SaltBack Status");

                    E.SetParameter("ShieldBlocked", true);

                    // AddPlayerMessage("SaltBackHalf Damage");

                    if (Owner.IsPlayer())
                    {
                        IComponent<GameObject>.AddPlayerMessage("You deflect an attack with your " + ParentObject.Equipped + "!" + "(" + FistShieldAV + " AV)", 'g');
                    }
                    else
                    {
                        Owner.ParticleText(string.Concat(new object[]
                        {
                            "{{",
                            IComponent<GameObject>.ConsequentialColor(Owner, null),
                            "|Block! (+",
                            FistShieldAV +
                            " AV)}}"
                        }), ' ', false, 1.5f, -8f);
                    }
                    E.SetParameter("AV", E.GetIntParameter("AV", 0) + FistShieldAV);
                }
            }
            if (E.ID == "BeginTakeAction" && ParentObject.HasEffect("DawnStance") && ParentObject.HasSkill("WM_MMA_PathAstralTabby"))
            {
                var ParentCell = ParentObject.CurrentCell.GetLocalAdjacentCells();

                foreach (var C in ParentCell)
                {
                    if (C.HasCombatObject())
                    {
                        FlankersAboundBnsDuration = 7;

                        StatShifter.SetStatShift("Speed", -2);
                    }
                }
            }
            if (E.ID == "EndTurn" && ParentObject.HasEffect("DawnStance"))
            {
                if (FlankersAboundBnsDuration > 0)
                {
                    --FlankersAboundBnsDuration;
                }
                else
                {
                    StatShifter.RemoveStatShifts();
                }

                var MMAComboAccess = ParentObject.GetPart<WM_MMA_SureStrikes>();
                MMAComboAccess.UpdateCounter();

            }
            return base.FireEvent(E);
        }

        public override bool AddSkill(GameObject GO)
        {
            this.DawnStanceID = base.AddMyActivatedAbility("Way of The Dawnglider", "DawngliderStanceCommand", "Skill", "Whenever you launch an attack with either your bare hands or natural weapon.", "*", null, false, false, true);
            return true;
        }

        public override bool RemoveSkill(GameObject GO)
        {
            return true;
        }
    }
}
