using System;
using System.Collections.Generic;
using System.Text;
using XRL.Rules;
using XRL.Messages;
using XRL.UI;

namespace XRL.World.Parts
{
    [Serializable]
    internal class MartialConditioningFistMod : IPart
    {
        public MartialConditioningFistMod()
        {
            Name = "MartialConditioningFistMod";
        }

        public override void Register(GameObject Object)
        {
            Object.RegisterPartEvent(this, "UpdateFistProperties");
        }

        public override bool FireEvent(Event E)
        {
            if (E.ID == "UpdateFistProperties")
            {
                AddPlayerMessage("Updating fist?");
                MeleeWeapon FistProperties = this.ParentObject.GetPart("MeleeWeapon") as MeleeWeapon;
                FistProperties.BaseDamage = (string)E.GetParameter("Dice");
                if (!ParentObject.HasPart("KO_On_Finish"))
                {
                    ParentObject.AddPart<KO_On_Finish>();

                    AddPlayerMessage("Part added to " + ParentObject);
                }

                AddPlayerMessage(FistProperties.BaseDamage);
            }
            return base.FireEvent(E);
        }
    }
}
