using System;
using System.Collections.Generic;
using System.Text;

using XRL.Rules;
using XRL.Messages;
using XRL.UI;

namespace XRL.World.Parts.Skill
{
    [Serializable]
    public class WM_MMA_CombinationStrikesII : BaseSkill
    {
        public WM_MMA_CombinationStrikesII()
        {
            Name = "WM_MMA_CombinationStrikesII";
            DisplayName = "Martial Conditioning II";
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
