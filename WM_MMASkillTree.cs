using System;
using System.Collections.Generic;
using System.Text;
using XRL.Rules;
using XRL.Messages;
using XRL.UI;
using XRL.World.Effects;
using XRL.World.Parts;
using static XRL.World.GameObjectFactory;
using ConsoleLib.Console;
using System.Linq;
using System.Security;
using XRL.Core;
using XRL.World;


using ObjectPart = XRL.World.GameObjectFactory;

namespace XRL.World.Parts.Skill
{
    [Serializable]
    public class WM_MMASkillTree : BaseSkill
    {
        public int CombinationStrikesCurrentCount = 0;
        public int CombinationStrikesMaximumCount = 0;
        public int ComboCounter = 0;
        public string BaseDamageMod;
        public int BasePenMod;

        public WM_MMASkillTree()
        {
            Name = "WM_MMASkillTree";
            DisplayName = "Martial Disciplines";
        }

        public override bool AddSkill(GameObject GO)
        {
            if (!ParentObject.HasPart("KO_On_Finish"))
            {
                // AddPlayerMessage("AddingKOPart from Skill Tree");
                ParentObject.AddPart<KO_On_Finish>();
                // AddPlayerMessage("Part KO added to " + ParentObject);
            }
            if (!ParentObject.HasPart("MartialBody"))
            {
                // AddPlayerMessage("Adding MartialbodPart from Skill Tree");
                ParentObject.AddPart<MartialBody>();
                // AddPlayerMessage("Part MB added to " + ParentObject);
            }
            if (!ParentObject.HasSkill("WM_MMA_MartialConI"))
            {
                ParentObject.AddSkill("WM_MMA_MartialConI");
            }
            return true;
        }

        public override bool RemoveSkill(GameObject GO)
        {
            return true;
        }

        public override void Register(GameObject Object)
        {
            Object.RegisterPartEvent(this, "BeginTakeAction");
            Object.RegisterPartEvent(this, "DrinkingFrom");
        }

        public string GetLevelValueBaseDamage(int Level)
        {
            if (ParentObject != null)
            {
                BaseDamageMod = "1d" + (2 + (Level / 4));
            }

            return BaseDamageMod;
        }

        public int GetLevelValuePenetration(int Level)
        {
            if (Level == 1)
            {
                BasePenMod = 4;
            }
            if (Level == 2)
            {
                BasePenMod = 4;
            }
            if (Level == 3)
            {
                BasePenMod = 5;
            }
            if (Level == 4)
            {
                BasePenMod = 5;
            }
            if (Level == 5)
            {
                BasePenMod = 6;
            }
            if (Level == 6)
            {
                BasePenMod = 6;
            }
            if (Level == 7)
            {
                BasePenMod = 7;
            }
            if (Level == 8)
            {
                BasePenMod = 7;
            }
            if (Level == 9)
            {
                BasePenMod = 8;
            }
            if (Level >= 10)
            {
                BasePenMod = 8;
            }
            return BasePenMod;
        }

        public void UpdateFistDamage(GameObject parent)
        {
            Body body = parent.GetPart<Body>();
            List<BodyPart> hands = body.GetPart("Hand");

            foreach (BodyPart hand in hands)
            {
                try
                {
                    if (!hand.Name.Contains("Robo-") && hand.DefaultBehavior != null && hand.DefaultBehavior.HasPart("MartialConditioningFistMod"))
                    {
                        var ObjectDamageLevel = GetLevelValueBaseDamage(this.ParentObject.Statistics["Level"].BaseValue);

                        hand.DefaultBehavior.FireEvent(Event.New("UpdateFistProperties", "Dice", ObjectDamageLevel));
                    }
                    if (hand.DefaultBehavior.HasPropertyOrTag("UndesireableWeapon"))
                    {
                        GameObject ConditionFist = GameObject.create("DefaultMartialFist");

                        // hand.DefaultBehaviorBlueprint.Remove(1, 1);
                        hand.DefaultBehavior.UnequipAndRemove();
                        // hand.DefaultBehaviorBlueprint = "DefaultMartialFist";
                        hand.DefaultBehavior = ConditionFist;
                    }
                }
                catch
                {

                }
            }
            return;
        }

        public override bool FireEvent(Event E)
        {
            if (E.ID == "BeginTakeAction")
            {
                UpdateFistDamage(this.ParentObject);
                return true;

            }
            // else if (E.ID == "DrinkingFrom" && (E.GetParameter("Container") as GameObject).LiquidVolume.ContainsLiquid("wine"))
            // {
            //     AddPlayerMessage("Drunken");
            //     ParentObject.ApplyEffect(new Drunken(10));
            // }
            return base.FireEvent(E);
        }


    }
}
