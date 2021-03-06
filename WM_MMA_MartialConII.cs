using System;
using System.Collections.Generic;
using System.Text;
using XRL.Rules;
using XRL.Messages;
using XRL.UI;

namespace XRL.World.Parts.Skill
{
    [Serializable]
    public class WM_MMA_MartialConII : BaseSkill
    {
        public WM_MMA_MartialConII()
        {
            Name = "WM_MMA_MartialConII";
            DisplayName = "Martial Conditioning II";
        }

        public override bool FireEvent(Event E)
        {
            int parentLevel = ParentObject.Statistics["Level"].BaseValue;

            if (E.ID == "ModifyDefendingSave" && E.GetStringParameter("Vs", (string)null).Contains("Stun"))
            {
                int roll = E.GetIntParameter("Roll", 0) + (4) + (parentLevel / 10);
                E.SetParameter("Roll", roll);
            }
            else if (E.ID == "ModifyDefendingSave" && E.GetStringParameter("Vs", (string)null).Contains("Dazed"))
            {
                int roll = E.GetIntParameter("Roll", 0) + (4) + (parentLevel / 10);
                E.SetParameter("Roll", roll);
            }
            else if (E.ID == "ModifyDefendingSave" && E.GetStringParameter("Vs", (string)null).Contains("ApplyProne"))
            {
                int roll = E.GetIntParameter("Roll", 0) + (4) + (parentLevel / 10);
                E.SetParameter("Roll", roll);
            }
            return base.FireEvent(E);
        }
        public override void Register(GameObject go)
        {
            go.RegisterPartEvent((IPart)this, "ModifyDefendingSave");
        }
        public override bool AddSkill(GameObject GO)
        {
            StatShifter.SetStatShift("Speed", 10);
            return true;
        }

        public override bool RemoveSkill(GameObject GO)
        {
            return true;
        }
    }
}
