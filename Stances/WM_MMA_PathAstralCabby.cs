using System;
using System.Collections.Generic;
using System.Text;

using XRL.Rules;
using XRL.Messages;
using XRL.UI;

namespace XRL.World.Parts.Skill
{
    [Serializable]
    public class WM_MMA_PathAstralCabby : BaseSkill
    {
        public Guid AstralCabbyStanceID;


        public WM_MMA_PathAstralCabby()
        {
            Name = "WM_MMA_PathAstralCabby";
            DisplayName = "Path of the Astral Cabby";
        }

        public override bool AddSkill(GameObject GO)
        {
            this.AstralCabbyStanceID = base.AddMyActivatedAbility("Way of the Astral Cabby", "AstralCabbyStanceCommand", "Skill", "Whenever you launch an attack with either your bare hands or natural weapon.", "*", null, false, false, true);

            return true;
        }

        public override bool RemoveSkill(GameObject GO)
        {
            return true;
        }
    }
}
