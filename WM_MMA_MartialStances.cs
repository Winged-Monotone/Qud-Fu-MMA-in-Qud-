using System;
using System.Collections.Generic;
using System.Text;

using XRL.Rules;
using XRL.Messages;
using XRL.UI;

namespace XRL.World.Parts.Skill
{
    [Serializable]
    public class WM_MMA_MartialStances : BaseSkill
    {
        public Guid DismissStanceID;
        public Guid DawnStanceID;
        public Guid SaltBackStanceID;
        public Guid SlumberStanceID;
        public Guid SaltHopperStanceID;
        public Guid AstralCabbyStanceID;


        public WM_MMA_MartialStances()
        {
            Name = "WM_MMA_MartialStances";
            DisplayName = "Martial Stances";
        }

        public override bool AddSkill(GameObject GO)
        {
            this.DismissStanceID = base.AddMyActivatedAbility("Dismiss Stance", "DismissStanceCommand", "Skill", "Whenever you launch an attack with either your bare hands or natural weapon.", "*", null, false, false, true);

            if (!ParentObject.HasSkill("WM_MMA_PathDawnGlider"))
            {
                ParentObject.AddSkill("WM_MMA_PathDawnGlider");
            }

            return true;
        }

        public override bool RemoveSkill(GameObject GO)
        {
            if (ParentObject.HasSkill("WM_MMA_PathDawnGlider"))
            {
                ParentObject.RemoveSkill("WM_MMA_PathDawnGlider");
            }

            GO.RemoveActivatedAbility(ref DismissStanceID);
            GO.RemoveActivatedAbility(ref DawnStanceID);
            GO.RemoveActivatedAbility(ref SaltBackStanceID);
            GO.RemoveActivatedAbility(ref SlumberStanceID);
            GO.RemoveActivatedAbility(ref SaltHopperStanceID);
            GO.RemoveActivatedAbility(ref AstralCabbyStanceID);
            return true;
        }
    }
}
