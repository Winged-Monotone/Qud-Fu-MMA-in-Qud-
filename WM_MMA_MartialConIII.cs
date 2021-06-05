using System;
using System.Collections.Generic;
using System.Text;

using XRL.Rules;
using XRL.Messages;
using XRL.UI;

namespace XRL.World.Parts.Skill
{
    [Serializable]
    public class WM_MMA_MartialConIII : BaseSkill
    {
        public WM_MMA_MartialConIII()
        {
            Name = "WM_MMA_MartialConIII";
            DisplayName = "Martial Conditioning III";
        }

        public override void Register(GameObject go)
        {
            go.RegisterPartEvent((IPart)this, "Regenerating");
            go.RegisterPartEvent((IPart)this, "AdjustSprintDuration");
        }

        public override bool WantEvent(int ID, int cascade)
        {
            if (!base.WantEvent(ID, cascade))
            {
                return ID == GetSprintDurationEvent.ID;
            }
            return true;
        }

        public override bool HandleEvent(GetSprintDurationEvent E)
        {
            E.LinearIncrease += 40;
            return base.HandleEvent(E);
        }

        public override bool FireEvent(Event E)
        {
            if (E.ID == "Regenerating")
            {
                if (ParentObject.HasEffect("Meditating"))
                {
                    int HealthRegained = E.GetIntParameter("Amount");
                    HealthRegained *= 3 + ParentObject.StatMod("Toughness", 1);
                }
            }
            // else if (E.ID == "AdjustSprintDuration")
            // {
            //     var SprintSpeedDurationBonus = E.GetIntParameter("Duration");
            //     SprintSpeedDurationBonus += 40;
            // }

            return base.FireEvent(E);
        }
        public override bool AddSkill(GameObject GO)
        {
            StatShifter.SetStatShift("AV", 2);
            StatShifter.SetStatShift("DV", 1);
            return true;
        }

        public override bool RemoveSkill(GameObject GO)
        {
            return true;
        }
    }
}
