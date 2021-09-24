using System;
using System.Collections.Generic;
using System.Text;

using XRL.Rules;
using XRL.Messages;
using XRL.UI;

namespace XRL.World.Parts.Skill
{
    [Serializable]
    public class WM_MMA_PathDeathDacca : BaseSkill
    {
        public Guid DeathDaccaStanceID;

        public WM_MMA_PathDeathDacca()
        {
            Name = "WM_MMA_PathDeathDacca";
            DisplayName = "Path of the Death Dacca's";
        }

        public override bool AddSkill(GameObject GO)
        {
            this.DeathDaccaStanceID = base.AddMyActivatedAbility("Way of the Death-Dacca", "DeathDaccaStanceCommand", "Skill", "Activate to assume the Dancing-Dacca stance.", "*", null, false, false, true);

            return true;
        }

        public override bool RemoveSkill(GameObject GO)
        {
            return true;
        }


    }
}
