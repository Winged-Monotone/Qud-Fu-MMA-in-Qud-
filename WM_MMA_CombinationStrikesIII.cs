using System;
using System.Collections.Generic;
using System.Text;

using XRL.Rules;
using XRL.Messages;
using XRL.UI;

namespace XRL.World.Parts.Skill
{
    [Serializable]
    public class WM_MMA_CombinationStrikesIII : BaseSkill
    {
        public WM_MMA_CombinationStrikesIII()
        {
            Name = "WM_MMA_CombinationStrikesIII";
            DisplayName = "Martial Conditioning III";
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
