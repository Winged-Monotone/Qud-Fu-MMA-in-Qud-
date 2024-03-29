using System;
using System.Collections.Generic;
using System.Linq;
using XRL.World.Effects;

using System.Text;
using XRL.World;
using XRL.World.Parts.Mutation;
using XRL.Rules;

using XRL.Language;
using XRL.World.Capabilities;
using UnityEngine;
using XRL.Messages;
using XRL.UI;

namespace XRL.World.Parts.Skill
{
    [Serializable]
    public class WM_MMA_MartialStances : BaseSkill
    {
        private List<string> StanceCollective = new List<string>()
        {
            "AstralTabbyStance",
            "DawnStance",
            "SaltbackStance",
            "SaltHopperStance",
            "SlumberStance",
            "DaccaStance",
        };
        public Guid DismissStanceID;
        public Guid DawnStanceID;
        public Guid SaltBackStanceID;
        public Guid SlumberStanceID;
        public Guid SaltHopperStanceID;
        public Guid AstralTabbyStanceID;
        public Guid DeathDaccaStanceID;
        public bool InStance = false;

        public WM_MMA_MartialStances()
        {
            Name = "WM_MMA_MartialStances";
            DisplayName = "Martial Stances";
        }

        public override bool AddSkill(GameObject GO)
        {
            this.DismissStanceID = base.AddMyActivatedAbility("Dismiss Stance", "DismissStanceCommand", "Skill", "Whenever you launch an attack with either your bare hands or natural weapon.", "*", null, false, false, true);

            if (!ParentObject.HasSkill("WM_MMA_MartialStances"))
            {
                if (!ParentObject.HasSkill("WM_MMA_PathDawnGlider"))
                {
                    ParentObject.AddSkill("WM_MMA_PathDawnGlider");
                }
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
            GO.RemoveActivatedAbility(ref AstralTabbyStanceID);
            GO.RemoveActivatedAbility(ref DeathDaccaStanceID);
            return true;
        }

        public override void Register(GameObject Object)
        {
            Object.RegisterPartEvent(this, "DismissStanceCommand");

            Object.RegisterPartEvent(this, "DawngliderStanceCommand");
            Object.RegisterPartEvent(this, "AstralTabbyStanceCommand");
            Object.RegisterPartEvent(this, "SaltBackStanceCommand");
            Object.RegisterPartEvent(this, "SlumberlingStanceCommand");
            Object.RegisterPartEvent(this, "SaltHopperStanceCommand");
            Object.RegisterPartEvent(this, "DeathDaccaStanceCommand");

            Object.RegisterPartEvent(this, "AIGetOffensiveMutationList");

            base.Register(Object);
        }

        public void StanceReplacement()
        {
            try
            {
                if (StanceCollective.Any(ParentObject.HasEffect))
                {
                    foreach (var k in StanceCollective)
                    {
                        ParentObject.RemoveEffect(k);
                    }
                }
            }
            catch
            {

            }
        }

        public void NoviceStancer()
        {
            if (!ParentObject.HasSkill("WM_MMA_MasterStanceSwap"))
            {
                ParentObject.UseEnergy(1000);
            }
        }

        public override bool FireEvent(Event E)
        {
            if (E.ID == "DismissStanceCommand")
            {
                StanceReplacement();
                NoviceStancer();
            }
            else if (E.ID == "DawngliderStanceCommand")
            {
                StanceReplacement();
                ParentObject.ApplyEffect(new DawnStance(Effect.DURATION_INDEFINITE));
                NoviceStancer();
            }
            else if (E.ID == "AstralTabbyStanceCommand")
            {
                StanceReplacement();
                ParentObject.ApplyEffect(new AstralTabbyStance(Effect.DURATION_INDEFINITE));
                NoviceStancer();
            }
            else if (E.ID == "SaltBackStanceCommand")
            {
                StanceReplacement();
                ParentObject.ApplyEffect(new SaltbackStance(Effect.DURATION_INDEFINITE));
                NoviceStancer();
            }
            else if (E.ID == "SlumberlingStanceCommand")
            {
                StanceReplacement();
                ParentObject.ApplyEffect(new SlumberStance(Effect.DURATION_INDEFINITE));
                NoviceStancer();
            }
            else if (E.ID == "SaltHopperStanceCommand")
            {
                StanceReplacement();
                ParentObject.ApplyEffect(new SaltHopperStance(Effect.DURATION_INDEFINITE));
                NoviceStancer();
            }
            else if (E.ID == "DeathDaccaStanceCommand")
            {
                StanceReplacement();
                ParentObject.ApplyEffect(new DaccaStance(Effect.DURATION_INDEFINITE));
                NoviceStancer();
            }
            else if (E.ID == "AIGetOffensiveMutationList")
            {
                WM_MMA_MartialStances GetStances = ParentObject.GetPart<WM_MMA_MartialStances>();

                if (IsMyActivatedAbilityToggledOn(AstralTabbyStanceID, ParentObject) == false && !ParentObject.HasEffect("AstralTabbyStance") && InStance == false)
                {
                    E.AddAICommand("AstralTabbyStanceCommand");
                    InStance = true;
                }
                else if (IsMyActivatedAbilityToggledOn(DawnStanceID, ParentObject) == false && !ParentObject.HasEffect("DawnStance") && InStance == false)
                {
                    E.AddAICommand("DawngliderStanceCommand");
                    InStance = true;
                }
                else if (IsMyActivatedAbilityToggledOn(SlumberStanceID, ParentObject) == false && !ParentObject.HasEffect("SlumberStance") && InStance == false)
                {
                    E.AddAICommand("SlumberlingStanceCommand");
                    InStance = true;
                }
                else if (IsMyActivatedAbilityToggledOn(SaltHopperStanceID, ParentObject) == false && !ParentObject.HasEffect("SaltHopperStance") && InStance == false)
                {
                    E.AddAICommand("SaltHopperStanceCommand");
                    InStance = true;
                }
                else if (IsMyActivatedAbilityToggledOn(SaltBackStanceID, ParentObject) == false && !ParentObject.HasEffect("SaltbackStance") && InStance == false)
                {
                    E.AddAICommand("SaltBackStanceCommand");
                    InStance = true;
                }
                else if (IsMyActivatedAbilityToggledOn(DeathDaccaStanceID, ParentObject) == false && !ParentObject.HasEffect("SaltbackStance") && InStance == false)
                {
                    E.AddAICommand("DeathDaccaStanceCommand");
                    InStance = true;
                }
            }

            return base.FireEvent(E);
        }
    }
}
