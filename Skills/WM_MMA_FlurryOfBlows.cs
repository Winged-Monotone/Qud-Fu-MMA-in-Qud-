using System;
using System.Collections.Generic;
using System.Text;

using XRL.Rules;
using XRL.Messages;
using XRL.UI;

using XRL.World.Anatomy;

namespace XRL.World.Parts.Skill
{
    [Serializable]
    public class WM_MMA_FlurryOfBlows : BaseSkill
    {
        public WM_MMA_FlurryOfBlows()
        {
           
        }

        public override bool WantEvent(int ID, int cascade)
        {
            return base.WantEvent(ID, cascade)
            || ID == GetMeleeAttackChanceEvent.ID;
        }

        public override bool HandleEvent(GetMeleeAttackChanceEvent E)
        {
            var eWeapon = E.Weapon;

            var ParentsAgility = ParentObject.StatMod("Agility");
            var ParentsLevel = ParentObject.Statistics["Level"].BaseValue;

            if (E.Actor == ParentObject 
                && (eWeapon.HasPart("MartialConditioningFistMod") 
                    || eWeapon.Blueprint == "DefaultFist" 
                    || eWeapon.IsNatural() 
                    || eWeapon.HasPart("Psionic Hand")))
            {
                E.Chance += (2 * ParentsAgility) + 1 + (ParentsLevel / 4);
            }

            return base.HandleEvent(E);
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
