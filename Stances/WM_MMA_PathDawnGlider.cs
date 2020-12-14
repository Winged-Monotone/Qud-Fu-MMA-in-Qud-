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
        public int BonusSureStrike;


        public WM_MMA_PathDawnGlider()
        {
            Name = "WM_MMA_PathDawnGlider";
            DisplayName = "Path of the Dawnglider";
        }

        public override void Register(GameObject Object)
        {
            Object.RegisterPartEvent(this, "AttackerHit");
            Object.RegisterPartEvent(this, "CommandSureStrikes");
            Object.RegisterPartEvent(this, "PerformMeleeAttack");
            Object.RegisterPartEvent(this, "EndTurn");
            base.Register(Object);
        }

        public override bool FireEvent(Event E)
        {
            if (E.ID == "AttackerHit")
            {
                if (ParentObject.HasEffect("DawnStance"))
                {
                    if (BonusSureStrike < 10)
                    { ++BonusSureStrike; }
                }
            }
            else if (E.ID == "CommandSureStrikes")
            {

                var MMAComboAccess = ParentObject.GetPart<WM_MMA_SureStrikes>();

                MMAComboAccess.FistPenBonus = +BonusSureStrike;
                BonusSureStrike = 0;
            }
            else if (E.ID == "PerformMeleeAttack")
            {
                int HitBonus = E.GetIntParameter("HitBonus");

                HitBonus = +2;
            }
            if (E.ID == "EndTurn")
            {
                AddPlayerMessage("SureStrike Stat: " + BonusSureStrike);
            }
            return base.FireEvent(E);
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
