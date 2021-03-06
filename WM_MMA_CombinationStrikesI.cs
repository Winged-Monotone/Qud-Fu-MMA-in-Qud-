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
    public class WM_MMA_CombinationStrikesI : BaseSkill
    {
        public int CurrentComboICounter = 0;
        public int MaximumComboICounter = 30;
        public int ComboResetDuration;
        public int BufferDuration;
        public bool ComboBuffering;
        public bool ComboBufferingCoolingDowne;
        public bool IsComboResettable;
        public Guid ComboCounterID = Guid.Empty;
        public Guid ResetComboCounterID = Guid.Empty;
        public WM_MMA_CombinationStrikesI()
        {
            Name = "WM_MMA_CombinationStrikesI";
            DisplayName = "Combination Strikes I";
        }

        public override bool AddSkill(GameObject GO)
        {
            this.ComboCounterID = base.AddMyActivatedAbility("Combo-Counter", "CommandPlaceHolder", "Skill", "Whenever you launch an attack with either your bare hands or natural weapon.", "*", null, false, false, true);
            UpdateCounter();
            this.ResetComboCounterID = base.AddMyActivatedAbility("Reset Counter", "CommandResetCmbo", "Skill", "Reset the ComboCounter", "*", null, false, false, true);
            UpdateCounter();
            return true;
        }

        public override bool RemoveSkill(GameObject GO)
        {
            return true;
        }

        public void UpdateCounter()
        {
            if (IsPlayer())
            {
                var AA = MyActivatedAbility(this.ComboCounterID);
                if (AA != null && CurrentComboICounter <= MaximumComboICounter * 0.2)
                {
                    AA.DisplayName = "{{white|Combo x(" + (CurrentComboICounter) + ")}}";
                }
                else if (AA != null && CurrentComboICounter >= MaximumComboICounter * 0.8)
                {
                    AA.DisplayName = "{{purple|Combo x(" + (CurrentComboICounter) + ")}}";
                }
                else if (AA != null && CurrentComboICounter >= MaximumComboICounter * 0.6)
                {
                    AA.DisplayName = "{{red|Combo x(" + (CurrentComboICounter) + ")}}";
                }
                else if (AA != null && CurrentComboICounter >= MaximumComboICounter * 0.4)
                {
                    AA.DisplayName = "{{orange|Combo x(" + (CurrentComboICounter) + ")}}";
                }
                else if (AA != null && CurrentComboICounter >= MaximumComboICounter * 0.2)
                {
                    AA.DisplayName = "{{yellow|Combo x(" + (CurrentComboICounter) + ")}}";
                }
            }
            else
            {

            }
        }

        public override bool WantEvent(int ID, int cascade)
        {
            return base.WantEvent(ID, cascade)
            || ID == AttackerDealingDamageEvent.ID
            || ID == GetAttackerHitDiceEvent.ID
            || ID == ModifyAttackingSaveEvent.ID
            || ID == ModifyDefendingSaveEvent.ID;
        }

        public override void Register(GameObject Object)
        {
            Object.RegisterPartEvent(this, "AttackerCriticalHit");
            Object.RegisterPartEvent(this, "PerformMeleeAttack");
            Object.RegisterPartEvent(this, "AttackerHit");
            Object.RegisterPartEvent(this, "AttackerMeleeMiss");
            Object.RegisterPartEvent(this, "EndSegment");
            Object.RegisterPartEvent(this, "CommandResetCmbo");
            Object.RegisterPartEvent(this, "EndTurn");
            Object.RegisterPartEvent(this, "DrinkingFrom");
            base.Register(Object);
        }

        public override bool HandleEvent(ModifyAttackingSaveEvent E)
        {
            var Parent = E.Attacker == ParentObject;
            var Defender = E.Defender;

            if (Parent && ParentObject.HasPart("MartialBody") && ParentObject.HasSkill("WM_MMA_CombinationStrikesII"))
            {
                E.NaturalRoll += (CurrentComboICounter / 2);
                E.Roll += (CurrentComboICounter / 2);
            }

            return base.HandleEvent(E);
        }

        public override bool HandleEvent(GetAttackerHitDiceEvent E)
        {
            var Weapon = E.Weapon;
            var Parent = E.Attacker == ParentObject;
            var Defender = E.Defender;
            var PenBonus = E.PenetrationBonus;

            if (Parent && (E.Weapon.HasPart("MartialConditioningFistMod") || E.Weapon.Blueprint == "DefaultMartialFist" || E.Weapon.HasPart("MartialConditioningFistMod") || E.Weapon.HasPart("NaturalGear") || E.Weapon.HasPropertyOrTag("WeaponUnarmed")) && ParentObject.HasSkill("WM_MMA_CombinationStrikesIII") && Defender.HasPart("Brain") && Defender.HasPart("Combat"))
            {
                E.PenetrationBonus += (CurrentComboICounter / 3);
            }

            return base.HandleEvent(E);

        }

        public override bool HandleEvent(AttackerDealingDamageEvent E)
        {
            try
            {
                var Parent = E.Actor == ParentObject;
                var Target = E.Object;

                Body body = ParentObject.GetPart("Body") as Body;

                var ParentsAgility = ParentObject.StatMod("Ego");
                var ParentsLevel = ParentObject.Statistics["Level"].BaseValue;

                List<BodyPart> hands = body.GetPart("Hand");

                if (E.Actor == ParentObject && (E.Weapon.HasPart("MartialConditioningFistMod") || E.Weapon.Blueprint == "DefaultMartialFist" || E.Weapon.HasPart("NaturalGear") || E.Weapon.HasPropertyOrTag("WeaponUnarmed")))

                    // AddPlayerMessage("Accessing Damage System.s");

                    foreach (BodyPart hand in hands)
                    {
                        try
                        {
                            // AddPlayerMessage("Accessing Damage System. A");

                            if (Parent && Target.HasPart("Brain") && Target.HasPart("Combat"))
                            {
                                var FistDamage = E.Damage.Amount;

                                E.Damage.Amount = (int)Math.Round(E.Damage.Amount + ((CurrentComboICounter * 0.025) * E.Damage.Amount));
                            }
                            else if (Stat.Random(1, 100) < CurrentComboICounter && Parent && hand.DefaultBehavior.HasPart("MartialConditioningFistMod") && ParentObject.HasSkill("WM_MMA_CombinationStrikesII") && Target.HasPart("Brain") && Target.HasPart("Combat"))
                            {
                                E.Damage.Amount = (int)Math.Round(E.Damage.Amount + ((CurrentComboICounter * 0.050) * E.Damage.Amount));
                            }

                        }
                        catch
                        {
                            return base.HandleEvent(E);
                        }
                    }
            }
            catch
            { }
            return base.HandleEvent(E);
        }

        public override bool FireEvent(Event E)
        {
            // AddPlayerMessage("Is anything working?");
            if (E.ID == "AttackerMeleeMiss")
            {
                // AddPlayerMessage("Decrease ComboCounter starting");
                Body body = ParentObject.GetPart("Body") as Body;
                List<BodyPart> hands = body.GetPart("Hand");

                var Parent = E.GetGameObjectParameter("Attacker") == ParentObject;
                var Defender = E.GetGameObjectParameter("Defender");
                var DeezHands = E.GetGameObjectParameter("Weapon");

                if (Parent && Defender.HasPart("Brain") && Defender.HasPart("Combat") && ParentObject.HasBodyPart("Hand") && DeezHands.HasPart("MartialConditioningFistMod"))
                {
                    // AddPlayerMessage("Decrease ComboCounter");
                    if (CurrentComboICounter > 0)
                        --CurrentComboICounter;
                    UpdateCounter();
                }
            }
            else if (E.ID == "AttackerHit")
            {
                // AddPlayerMessage("Increase ComboCounter starting");
                Body body = ParentObject.GetPart("Body") as Body;
                List<BodyPart> hands = body.GetPart("Hand");

                var Penetrations = E.GetIntParameter("Penetrations");
                var Parent = E.GetGameObjectParameter("Attacker") == ParentObject;
                var Defender = E.GetGameObjectParameter("Defender");
                var DeezHands = E.GetGameObjectParameter("Weapon");
                if (Penetrations == 0)
                {
                    if (CurrentComboICounter > 0)
                    {
                        --CurrentComboICounter;
                    }
                }
                if (Parent && Defender.HasPart("Brain") && Defender.HasPart("Combat") && ParentObject.HasBodyPart("Hand") && (DeezHands.Blueprint == "DefaultMartialFist" || DeezHands.HasPart("MartialConditioningFistMod") || DeezHands.HasPart("NaturalGear") || DeezHands.HasPropertyOrTag("WeaponUnarmed")))
                {
                    // AddPlayerMessage("Increase ComboCounter");

                    if (CurrentComboICounter <= MaximumComboICounter)
                        ++CurrentComboICounter;

                    if (ComboBuffering == false)
                    {
                        ComboBuffering = true;
                    }
                    BufferDuration = 3 + Penetrations;
                    UpdateCounter();
                }

            }
            else if (E.ID == "CommandResetCmbo")
            {
                // AddPlayerMessage("Reset Combo");
                CurrentComboICounter = 0;
            }
            else if (E.ID == "EndTurn")
            {
                if (BufferDuration > 0 && ComboBuffering == true)
                {
                    // AddPlayerMessage("depricate bufferduration");
                    --BufferDuration;
                    UpdateCounter();
                    if (BufferDuration <= 0 && ComboBuffering == true)
                    {
                        // AddPlayerMessage("ResetCombos");
                        ComboBuffering = false;
                        CurrentComboICounter = 0;
                        UpdateCounter();
                    }
                }

                // AddPlayerMessage("current combo value: " + CurrentComboICounter);
                // AddPlayerMessage("current buffer value: " + BufferDuration);
                // AddPlayerMessage("current Combo Reset value: " + ComboResetDuration);

            }


            return base.FireEvent(E);
        }
    }
}

// AttackerQueryWeaponSecondaryAttackChanceMultiplier

