using System;
using System.Collections.Generic;
using System.Text;

using XRL.Rules;
using XRL.Messages;
using XRL.UI;

namespace XRL.World.Parts.Skill
{
    [Serializable]
    public class WM_MMA_PathDawnGlider : BaseSkill
    {
        public Guid DawnStanceID;

        public WM_MMA_PathDawnGlider()
        {
            Name = "WM_MMA_PathDawnGlider";
            DisplayName = "Path of the Dawnglider";
        }

        public override bool AddSkill(GameObject GO)
        {
            this.DawnStanceID = base.AddMyActivatedAbility("Way of The Dawnglider", "DawngliderStanceCommand", "Skill", "Whenever you launch an attack with either your bare hands or natural weapon.", "*", null, false, false, true);
            return true;
        }

        public override bool RemoveSkill(GameObject GO)
        {
            return true;
        }
    }
}
