using System;
using System.Collections.Generic;
using System.Text;

using XRL.Rules;
using XRL.Messages;
using XRL.UI;

namespace XRL.World.Parts.Skill
{
    [Serializable]
    public class WM_MMA_PathSaltHopper : BaseSkill
    {
        public Guid SaltHopperStanceID;

        public WM_MMA_PathSaltHopper()
        {
            Name = "WM_MMA_PathSaltHopper";
            DisplayName = "Path of the Salt-Hopper";
        }

        public override bool AddSkill(GameObject GO)
        {

            this.SaltHopperStanceID = base.AddMyActivatedAbility("Way of the Salt Hopper", "SaltHopperStanceCommand", "Skill", "Whenever you launch an attack with either your bare hands or natural weapon.", "*", null, false, false, true);

            return true;
        }

        public override bool RemoveSkill(GameObject GO)
        {
            return true;
        }
    }
}
