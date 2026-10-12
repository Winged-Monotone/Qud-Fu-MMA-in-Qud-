using System;
using System.Collections.Generic;
using System.Text;

using XRL.Rules;
using XRL.Messages;
using XRL.UI;

namespace XRL.World.Parts.Skill
{
    [Serializable]
    public class WM_MMA_MartialConI : BaseSkill
    {
        public WM_MMA_MartialConI()
        {

        }

        public override bool AddSkill(GameObject GO)
        {
           var EquipmentList = ParentObject.GetEquippedObjects();
           foreach (var E in EquipmentList)
           {
               if (E.IsNatural())
               {
                   E.RemoveProperty("UndesirableWeapon");
               }
           }
            return true;
        }

        public override bool RemoveSkill(GameObject GO)
        {
            return true;
        }
    }
}
