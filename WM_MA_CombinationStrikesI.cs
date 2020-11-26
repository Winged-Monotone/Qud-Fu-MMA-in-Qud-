using System;
using System.Collections.Generic;
using System.Text;
using XRL.Rules;
using XRL.Messages;
using XRL.UI;

namespace XRL.World.Parts.Skill
{
    [Serializable]
    public class WM_MMA_CombinationStrikesI : BaseSkill
    {
        public int CurrentComboICounter = 0;
        public int MaximumComboICounter = 20;
        public Guid ComboCounterID = Guid.Empty;
        public WM_MMA_CombinationStrikesI()
        {
            Name = "WM_MMA_CombinationStrikesI";
            DisplayName = "Combination Strikes I";
        }

        public override bool AddSkill(GameObject GO)
        {
            this.ComboCounterID = base.AddMyActivatedAbility("Combo-Counter", "CommandPlaceHolder", "Skill", "Whenever you launch an attack with either your bare hands or natural weapon.", "*", null, false, false, true);
            UpdateCounter();
            return true;
        }

        public override bool RemoveSkill(GameObject GO)
        {
            return true;
        }

        public void UpdateCounter()
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


        public override bool WantEvent(int ID, int cascade)
        {
            return base.WantEvent(ID, cascade) || ID == AttackerDealingDamageEvent.ID;
        }

        public override void Register(GameObject Object)
        {
            Object.RegisterPartEvent(this, "AttackerCriticalHit");
            Object.RegisterPartEvent(this, "PerformMeleeAttack");
            Object.RegisterPartEvent(this, "AttackerHit");
            Object.RegisterPartEvent(this, "AttackerMeleeMiss");
            Object.RegisterPartEvent(this, "EndSegment");
            base.Register(Object);
        }


        public override bool HandleEvent(AttackerDealingDamageEvent E)
        {
            var Parent = E.Actor == ParentObject;
            var Target = E.Object;

            Body body = ParentObject.GetPart("Body") as Body;

            var ParentsAgility = ParentObject.StatMod("Ego");
            var ParentsLevel = ParentObject.Statistics["Level"].BaseValue;

            List<BodyPart> hands = body.GetPart("Hand");

            foreach (BodyPart hand in hands)
            {
                try
                {
                    if (!hand.Name.Contains("Robo-") && hand.DefaultBehavior != null && hand.DefaultBehavior.HasPart("MartialConditioningFistMod"))
                    {
                        if (hand == null || hand.Category != 6)
                        {
                            if (Parent && Target.HasPart("Brain") && Target.HasPart("Combat"))
                            {
                                var FistDamage = E.Damage.Amount;

                                FistDamage = (int)Math.Round(FistDamage + (FistDamage / ((0.05) * (ParentsLevel))));
                            }
                        }
                    }
                }
                catch
                {
                    return true;
                }
            }

            return true;
        }

        public override bool FireEvent(Event E)
        {
            AddPlayerMessage("Is anything working?");
            if (E.ID == "AttackerMeleeMiss")
            {
                AddPlayerMessage("Decrease ComboCounter starting");
                Body body = ParentObject.GetPart("Body") as Body;
                List<BodyPart> hands = body.GetPart("Hand");

                var Parent = E.GetGameObjectParameter("Attacker") == ParentObject;
                var Defender = E.GetGameObjectParameter("Defender");
                var DeezHands = E.GetGameObjectParameter("Weapon");

                if (Parent && Defender.HasPart("Brain") && Defender.HasPart("Combat") && ParentObject.HasBodyPart("Hand") && DeezHands.Blueprint == "DefaultFist")
                {
                    AddPlayerMessage("Decrease ComboCounter");
                    if (CurrentComboICounter < 0)
                        CurrentComboICounter = 0;
                    UpdateCounter();
                }
            }
            else if (E.ID == "AttackerHit")
            {
                AddPlayerMessage("Increase ComboCounter starting");
                Body body = ParentObject.GetPart("Body") as Body;
                List<BodyPart> hands = body.GetPart("Hand");

                var Parent = E.GetGameObjectParameter("Attacker") == ParentObject;
                var Defender = E.GetGameObjectParameter("Defender");
                var DeezHands = E.GetGameObjectParameter("Weapon");

                if (Parent && Defender.HasPart("Brain") && Defender.HasPart("Combat") && ParentObject.HasBodyPart("Hand") && DeezHands.Blueprint == "DefaultFist")
                {
                    AddPlayerMessage("Increase ComboCounter");
                    ++CurrentComboICounter;
                    UpdateCounter();
                }
            }

            return base.FireEvent(E);
        }
    }
}

// AttackerQueryWeaponSecondaryAttackChanceMultiplier

