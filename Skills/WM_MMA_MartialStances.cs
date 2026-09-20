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
using System.Data.SqlTypes;

namespace XRL.World.Parts.Skill
{
    [Serializable]
    public class WM_MMA_MartialStances : BaseSkill
    {
        private List<Type> StanceCollectiveE = new List<Type>()
        {
           typeof(AstralTabbyStance),
            typeof(DawnStance),
            typeof(SaltbackStance),
            typeof(SaltHopperStance),
            typeof(SlumberStance),
            typeof(DaccaStance),
        };

        public Guid DismissStanceID;
        public Guid DawnStanceID;
        public Guid SaltBackStanceID;
        public Guid SlumberStanceID;
        public Guid SaltHopperStanceID;
        public Guid AstralTabbyStanceID;
        public Guid DeathDaccaStanceID;
        public Guid StanceVisceraDefID;
        public bool InStance = false;

        public bool wmStanceVisceraToggle = true;
        public string wmSVToggleState = "Stance 'Visceral Descriptors'";

        public WM_MMA_MartialStances()
        {

        }
        public override bool WantEvent(int ID, int cascade)
        {
            return ID == ObjectEnteredCellEvent.ID
            || base.WantEvent(ID, cascade);

        }
        public override bool HandleEvent(ObjectEnteredCellEvent E)
        {
            if (E.Object.IsPlayer())
            {
                if (E.Object.OnWorldMap())
                {
                    StanceReplacement();
                }
                ;
            }
            return base.HandleEvent(E);
        }
        public override bool AddSkill(GameObject GO)
        {
            this.DismissStanceID = base.AddMyActivatedAbility("Dismiss Stance", "DismissStanceCommand", "Skill", "Dismiss your current stance.", "*", null, false, false, true);

            if (!ParentObject.HasSkill("WM_MMA_MartialStances"))
            {
                if (!ParentObject.HasSkill("WM_MMA_PathDawnGlider"))
                {
                    ParentObject.AddSkill("WM_MMA_PathDawnGlider");
                }
            }

            StanceVisceraDefID = GO.AddActivatedAbility(
                Name: wmSVToggleState,
                Command: "WM_VisceralTextSysEvent",
                Class: "Mod Option",
                Description: "Toggle the 'visceral action text' included with Qud-Fu.",
                Icon: "nD",
                DisabledMessage: "You have toggled Visceral Descriptors Off.",
                Toggleable: true,
                DefaultToggleState: true,
                AffectedByWillpower: false);

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
            GO.RemoveActivatedAbility(ref StanceVisceraDefID);
            return true;
        }

        public override void Register(GameObject Object, IEventRegistrar registrar)
        {
            Object.RegisterPartEvent(this, "DismissStanceCommand");

            Object.RegisterPartEvent(this, "DawngliderStanceCommand");
            Object.RegisterPartEvent(this, "AstralTabbyStanceCommand");
            Object.RegisterPartEvent(this, "SaltBackStanceCommand");
            Object.RegisterPartEvent(this, "SlumberlingStanceCommand");
            Object.RegisterPartEvent(this, "SaltHopperStanceCommand");
            Object.RegisterPartEvent(this, "DeathDaccaStanceCommand");

            Object.RegisterPartEvent(this, "WM_VisceralTextSysEvent");

            Object.RegisterPartEvent(this, "AIGetOffensiveMutationList");

            base.Register(Object, registrar);
        }

        public void StanceReplacement()
        {
            try
            {
                if (StanceCollectiveE.Any(ParentObject.HasEffect))
                {
                    foreach (var k in StanceCollectiveE)
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
            if (E.ID == "WM_VisceralTextSysEvent")
            {
                ToggleMyActivatedAbility(StanceVisceraDefID, who: ParentObject, Silent: false);
            }
            if (E.ID == "DismissStanceCommand")
            {
                StanceReplacement();
                NoviceStancer();
            }
            else if (E.ID == "DawngliderStanceCommand")
            {
                if (ParentObject.HasEffect("DawnStance"))
                {
                    ParentObject.FireEvent("DismissStanceCommand");
                }
                StanceReplacement();
                ParentObject.ApplyEffect(new DawnStance(Effect.DURATION_INDEFINITE));
                NoviceStancer();

            }
            else if (E.ID == "AstralTabbyStanceCommand")
            {
                if (ParentObject.HasEffect("AstralTabbyStance"))
                {
                    ParentObject.FireEvent("DismissStanceCommand");
                }
                StanceReplacement();
                ParentObject.ApplyEffect(new AstralTabbyStance(Effect.DURATION_INDEFINITE));
                NoviceStancer();
            }
            else if (E.ID == "SaltBackStanceCommand")
            {
                if (ParentObject.HasEffect("SaltbackStance"))
                {
                    ParentObject.FireEvent("DismissStanceCommand");
                }
                StanceReplacement();
                ParentObject.ApplyEffect(new SaltbackStance(Effect.DURATION_INDEFINITE));
                NoviceStancer();
            }
            else if (E.ID == "SlumberlingStanceCommand")
            {
                if (ParentObject.HasEffect("SlumberStance"))
                {
                    ParentObject.FireEvent("DismissStanceCommand");
                }
                StanceReplacement();
                ParentObject.ApplyEffect(new SlumberStance(Effect.DURATION_INDEFINITE));
                NoviceStancer();
            }
            else if (E.ID == "SaltHopperStanceCommand")
            {
                if (ParentObject.HasEffect("SaltHopperStance"))
                {
                    ParentObject.FireEvent("DismissStanceCommand");
                }
                StanceReplacement();
                ParentObject.ApplyEffect(new SaltHopperStance(Effect.DURATION_INDEFINITE));
                NoviceStancer();
            }
            else if (E.ID == "DeathDaccaStanceCommand")
            {
                if (ParentObject.HasEffect("DaccaStance"))
                {
                    ParentObject.FireEvent("DismissStanceCommand");
                }
                StanceReplacement();
                ParentObject.ApplyEffect(new DaccaStance(Effect.DURATION_INDEFINITE));
                NoviceStancer();
            }
            else if (E.ID == "AIGetOffensiveMutationList")
            {
                WM_MMA_MartialStances GetStances = ParentObject.GetPart<WM_MMA_MartialStances>();

                if (!IsMyActivatedAbilityToggledOn(AstralTabbyStanceID, ParentObject) && !ParentObject.HasEffect("AstralTabbyStance") && !InStance)
                {
                    E.AddAICommand("AstralTabbyStanceCommand");
                    InStance = true;
                }
                else if (!IsMyActivatedAbilityToggledOn(DawnStanceID, ParentObject) && !ParentObject.HasEffect("DawnStance") && !InStance)
                {
                    E.AddAICommand("DawngliderStanceCommand");
                    InStance = true;
                }
                else if (!IsMyActivatedAbilityToggledOn(SlumberStanceID, ParentObject) && !ParentObject.HasEffect("SlumberStance") && !InStance)
                {
                    E.AddAICommand("SlumberlingStanceCommand");
                    InStance = true;
                }
                else if (!IsMyActivatedAbilityToggledOn(SaltHopperStanceID, ParentObject) && !ParentObject.HasEffect("SaltHopperStance") && !InStance)
                {
                    E.AddAICommand("SaltHopperStanceCommand");
                    InStance = true;
                }
                else if (!IsMyActivatedAbilityToggledOn(SaltBackStanceID, ParentObject) && !ParentObject.HasEffect("SaltbackStance") && !InStance)
                {
                    E.AddAICommand("SaltBackStanceCommand");
                    InStance = true;
                }
                else if (!IsMyActivatedAbilityToggledOn(DeathDaccaStanceID, ParentObject) && !ParentObject.HasEffect("SaltbackStance") && !InStance)
                {
                    E.AddAICommand("DeathDaccaStanceCommand");
                    InStance = true;
                }
            }

            return base.FireEvent(E);
        }
    }
}
