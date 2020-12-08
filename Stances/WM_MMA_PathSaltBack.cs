using System;
using System.Collections.Generic;
using System.Text;

using XRL.Rules;
using XRL.Messages;
using XRL.UI;

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
