using System;
using System.Collections.Generic;
using System.Text;

// keep in mind you can reference another class through 'using' directly so long as your know the direct route to that particular class
using ShieldPart = XRL.World.Parts.Shield;

using XRL.Rules;
using XRL.Messages;
using XRL.UI;

using XRL.World.Parts;
using XRL.World.PartBuilders;
using XRL.Core;


namespace XRL.World.Parts.Skill
{
    [Serializable]
    public class WM_MMA_PathSaltBack : BaseSkill
    {
        public Guid SaltBackStanceID;

        public WM_MMA_PathSaltBack()
        {
            Name = "WM_MMA_PathSaltBack";
            DisplayName = "Path of the Salt-Back";
        }

        public override void Register(GameObject Object)
        {
            Object.RegisterPartEvent(this, "AttackerHit");
            Object.RegisterPartEvent(this, "GetDefenderHitDice");
            Object.RegisterPartEvent(this, "PerformMeleeAttack");
            Object.RegisterPartEvent(this, "EndTurn");
            base.Register(Object);
        }

        public override bool FireEvent(Event E)
        {
            if (E.ID == "AttackerHit")
            {
                AddPlayerMessage("SaltBackHalf Damage");
                var Damage = E.GetIntParameter("Damage");

                Damage = (Damage / 2);
            }
            if (E.ID == "GetDefenderHitDice" && ParentObject.HasEffect("SaltbackPath"))
            {
                AddPlayerMessage("SaltBack Defender Block Begins");
                // GameObject Attacker = E.GetGameObjectParameter("Attacker");
                var Owner = ParentObject;


                Body body = Owner.GetPart("Body") as Body;
                List<BodyPart> hands = body.GetPart("Hand");
                var hand = body.GetPrimaryWeaponOfTypeOnBodyPartOfType("DefaultFist", "Hand");

                if (!Owner.HasPart("Shield"))
                {
                    AddPlayerMessage("Adding Shield");
                    Owner.AddPart<ShieldPart>();
                }

                ShieldPart FistShield = hand.RequirePart<ShieldPart>();

                FistShield.AV = ParentObject.StatMod("Toughness", 1) + (ParentObject.Statistics["Level"].BaseValue / 2);
                FistShield.Blocks = (ParentObject.StatMod("Toughness", 1));

                if (FistShield == null)
                {
                    AddPlayerMessage("SaltBackHalf Shield Returned Null");
                    return true;
                }
                if (E.HasParameter("ShieldBlocked"))
                {
                    AddPlayerMessage("SaltBackHalf Blocked ParameterSet");
                    return true;
                }
                if (!Owner.CanMoveExtremities(null, false, false, false))
                {
                    AddPlayerMessage("SaltBackHalf CanMove Check");
                    return true;
                }
                AddPlayerMessage("SaltBackHalf Block Attempt Random Int");
                if (Stat.Random(1, 100) <= 15 + (5 * (ParentObject.Statistics["Level"].BaseValue / 5)))
                {
                    AddPlayerMessage("SaltBackHalf SaltBack Status");
                    var MMAComboAccess = ParentObject.GetPart<WM_MMA_CombinationStrikesI>();
                    E.SetParameter("ShieldBlocked", true);
                    ++MMAComboAccess.CurrentComboICounter;
                    AddPlayerMessage("SaltBackHalf Damage");
                    if (Owner.IsPlayer())
                    {
                        IComponent<GameObject>.AddPlayerMessage("You deflect an attack with your " + base.ParentObject.DisplayName + "!" + "(" + FistShield.AV + " AV)", 'g');
                    }
                    else
                    {
                        Owner.ParticleText(string.Concat(new object[]
                        {
                            "{{",
                            IComponent<GameObject>.ConsequentialColor(Owner, null),
                            "|Block! (+",
                            FistShield.AV,
                            " AV)}}"
                        }), ' ', false, 1.5f, -8f);
                    }
                    E.SetParameter("AV", E.GetIntParameter("AV", 0) + FistShield.AV);
                }
            }
            return base.FireEvent(E);
        }

        public override bool AddSkill(GameObject GO)
        {


            this.SaltBackStanceID = base.AddMyActivatedAbility("Way of the Salt-Back", "SaltBackStanceCommand", "Skill", "Whenever you launch an attack with either your bare hands or natural weapon.", "*", null, false, false, true);
            return true;
        }

        public override bool RemoveSkill(GameObject GO)
        {
            return true;
        }
    }
}
