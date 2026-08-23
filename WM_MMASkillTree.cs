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
using XRL.World.Anatomy;


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
            if (!ParentObject.HasSkill("WM_MMA_FlurryOfBlows"))
            {
                ParentObject.AddSkill("WM_MMA_FlurryOfBlows");
            }
            return true;
        }

        public override bool RemoveSkill(GameObject GO)
        {
            return true;
        }

        public override void Register(GameObject Object, IEventRegistrar registrar)
        {
            Object.RegisterPartEvent(this, "BeginTakeAction");
            Object.RegisterPartEvent(this, "DrinkingFrom");
        }

        public int GetLevelValueBaseDamage(int Level)
        {
            int exDamageMIN = (1 + Level / 2);
            int exDamageMAX = (1 + Level / 2) + (2 + (Level / 2));

            var MinMax = Stat.Random(exDamageMIN, exDamageMAX);

            return MinMax;
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

        public override bool WantEvent(int ID, int cascade)
        {

            return ID == AttackerDealingDamageEvent.ID
            || base.WantEvent(ID, cascade);

        }

        public override bool HandleEvent(AttackerDealingDamageEvent E)
        {
            var eActor = E.Actor;
            var Defender = E.Object;

            var eWeapon = E.Weapon;
            if (eWeapon != null)
            {
                var eDamage = E.Damage.Amount;


                var ObjectDamageLevel = GetLevelValueBaseDamage(this.ParentObject.Statistics["Level"].BaseValue);

                if (eWeapon.HasPart<MartialConditioningFistMod>() || eWeapon.HasPropertyOrTag("MMAHooks"))
                {
                    E.Damage.Amount = ObjectDamageLevel;
                }
            }
            else
            {

            }

            return base.HandleEvent(E);
        }
    }
}
