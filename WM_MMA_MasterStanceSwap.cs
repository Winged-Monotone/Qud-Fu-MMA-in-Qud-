using System;
using System.Collections.Generic;
using System.Text;

using XRL.Rules;
using XRL.Messages;
using XRL.UI;

namespace XRL.World.Parts.Skill
{
    [Serializable]
    public class WM_MMA_MasterStanceSwap : BaseSkill
    {
        public WM_MMA_MasterStanceSwap()
        {
            Name = "WM_MMA_MasterStanceSwap";
            DisplayName = "Master of Stances";
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
