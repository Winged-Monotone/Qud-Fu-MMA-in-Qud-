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
            || ID == BeforeMeleeAttackEvent.ID;
        }

        public override bool HandleEvent(BeforeMeleeAttackEvent E)
        {
            var eWeapon = E.Weapon;
            var eWeaponSChance = E.Weapon.GetPart<SecondaryAttackChance>();

            var ParentsAgility = ParentObject.StatMod("Agility");
            var ParentsLevel = ParentObject.Statistics["Level"].BaseValue;

            if (E.Actor == ParentObject & (eWeapon.HasPart("MartialConditioningFistMod") || eWeapon.Blueprint == "DefaultMartialFist" || eWeapon.IsNatural() || eWeapon.HasPart("Psionic Hand")) && eWeapon.HasPart<SecondaryAttackChance>())
            {
                eWeaponSChance.Chance += (2 * ParentsAgility) + 1 + (ParentsLevel / 4);
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
