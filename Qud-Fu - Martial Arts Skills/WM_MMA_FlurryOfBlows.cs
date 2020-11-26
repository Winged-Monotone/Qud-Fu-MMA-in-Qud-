using System;
using System.Collections.Generic;
using System.Text;

using XRL.Rules;
using XRL.Messages;
using XRL.UI;

namespace XRL.World.Parts.Skill
{
    [Serializable]
    public class WM_MMA_FlurryOfBlows : BaseSkill
    {
        public WM_MMA_FlurryOfBlows()
        {
            Name = "WM_MMA_FlurryOfBlows";
            DisplayName = "Chaining Strikes";
        }

        public override void Register(GameObject Object)
        {
            Object.RegisterPartEvent(this, "AttackerQueryWeaponSecondaryAttackChanceMultiplier");
            base.Register(Object);
        }

        public override bool FireEvent(Event E)
        {
            if (E.ID == "AttackerQueryWeaponSecondaryAttackChanceMultiplier")
            {
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
                            BodyPart bodyPart = E.GetParameter("BodyPart") as BodyPart;
                            if (bodyPart == null || bodyPart.Category != 6)
                            {
                                E.SetParameter("Chance", E.GetIntParameter("Chance") + (5 + ParentsAgility) + (5 * (ParentsLevel / 4)));
                            }
                        }
                    }
                    catch
                    {

                    }
                }
            }

            return base.FireEvent(E);
        }

        public override bool AddSkill(GameObject GO)
        {
            return true;
        }

        public override bool RemoveSkill(GameObject GO)
        {
            return true;
        }
    }
}
