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
            Name = "WM_MMA_MartialConI";
            DisplayName = "Martial Conditioning I";
        }

        public override bool AddSkill(GameObject GO)
        {
            StatShifter.SetStatShift("Speed", -10);
            return true;
        }

        public override bool RemoveSkill(GameObject GO)
        {
            return true;
        }
    }
}
