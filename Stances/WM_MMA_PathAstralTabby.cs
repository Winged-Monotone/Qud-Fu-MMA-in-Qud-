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

        private int FlankerAccumBonus;

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
            Object.RegisterPartEvent(this, "EndTurn");

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
                // AddPlayerMessage("Log: Beginning Take Action");
                var ParentCell = ParentObject.CurrentCell.GetLocalAdjacentCells();
                // AddPlayerMessage("Log: Acquired Cells");
                foreach (var C in ParentCell)
                {
                    // AddPlayerMessage("Log: Starting Foreach");
                    if (C.HasObjectWithPart("Combat") && !C.HasObjectWithPart("FlankerReader_PAT"))
                    {
                        // AddPlayerMessage("Found Combat Targets");
                        var ObjectCheck = C.GetFirstObjectWithPart("Combat");
                        // AddPlayerMessage("Log: Getting Combat Targets");

                        if (!ObjectCheck.HasPart("FlankerReader_PAT"))
                        {
                            // AddPlayerMessage("Log: Adding FlankerCheckParts");
                            ObjectCheck.AddPart<FlankerReader_PAT>();


                            // AddPlayerMessage("Log: Added Flanker Part");
                            var ConfirmedObject = ObjectCheck.GetPart<FlankerReader_PAT>();
                            ConfirmedObject.Check = true;


                            // AddPlayerMessage("Log: Confriming Object Flanker Pass");
                            if (ConfirmedObject.Check != false)
                            {
                                FlankersAboundDuration = 3;
                                ParentObject.Statistics["DV"]._Value += 1;
                                Targeted = true;
                                FlankerAccumBonus += 1;
                                ConfirmedObject.Check = false;
                            }
                        }
                        // AddPlayerMessage("Log: Completed begin take action.");
                        break;
                    }
                    else
                    if (Targeted == true)
                    {
                        Targeted = false;
                    }
                }
            }
            else if (E.ID == "TargetedForMissileWeapon" && ParentObject.HasEffect<AstralTabbyStance>())
            {
                GameObject Attacker = E.GetGameObjectParameter("Attacker");
                GameObject Defender = E.GetGameObjectParameter("Defender");

                if (Defender == ParentObject)
                {
                    Targeted = true;
                    StatShifter.SetStatShift("DV", +ParentObject.Statistics["Agility"].Modifier + (ParentObject.Statistics["Level"].BaseValue / 8));
                }
            }
            else if (E.ID == "EndTurn" && ParentObject.HasEffect<AstralTabbyStance>())
            {
                // AddPlayerMessage("Log: Firing Endturn.");
                if (FlankersAboundDuration > 0 && Targeted == false)
                {
                    // AddPlayerMessage("Log: Duration Countdown.");
                    --FlankersAboundDuration;
                }
                else if (Targeted == false && FlankersAboundDuration <= 0)
                {
                    // AddPlayerMessage("Log: Resetting Stats");

                    ParentObject.Statistics["DV"]._Value -= FlankerAccumBonus;
                    FlankerAccumBonus = 0;
                    Targeted = false;
                }
                else if (Targeted == true && StatShifter != null)
                {
                    StatShifter.RemoveStatShifts();
                }

                AddPlayerMessage("Log: Targeted: " + Targeted);
                AddPlayerMessage("FlankersAboundDuration: " + FlankersAboundDuration);

            }
            return base.FireEvent(E);
        }
    }
}
