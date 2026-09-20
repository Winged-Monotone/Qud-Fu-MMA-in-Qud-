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
using XRL.Wish;
using XRL.World;
using XRL.World.Anatomy;


using ObjectPart = XRL.World.GameObjectFactory;

namespace XRL.World.Parts.Skill
{
    [HasWishCommand]
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
                ParentObject.AddPart<KO_On_Finish>();
            }
            if (!ParentObject.HasPart("MartialBody"))
            {
                ParentObject.AddPart<MartialBody>();
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
        
        public override void Register(GameObject Object, IEventRegistrar registrar)
        {
            registrar.Register("UpdateFistProperties");
            registrar.Register(AfterLevelGainedEvent.ID);
            registrar.Register(RegenerateDefaultEquipmentEvent.ID);
            
            base.Register(Object, registrar);
        }
        
        [WishCommand("fistbeharder")]
        public static void UpdateFistProperties()
        {
            if (ThePlayer != null && ThePlayer.Body.IsValid)
                The.Player.FireEvent("UpdateFistProperties");
            
        }

        public override bool HandleEvent(AfterLevelGainedEvent E)
        {
            if (E.Actor == ParentObject)
            {
                UpdateFistProperties();
            }
            
            return base.HandleEvent(E);
        }
        
        public override bool HandleEvent(RegenerateDefaultEquipmentEvent E)
        {
                UpdateFistProperties();
            return base.HandleEvent(E);
        }

        public override bool FireEvent(Event E)
        {
            if (E.ID == "UpdateFistProperties")
            {
                if (ParentObject.Body != null)
                {
                    foreach (var Limb in ParentObject.Body.LoopParts())
                    {
                        if (Limb.DefaultBehavior.IsValid() && Limb.DefaultBehavior.Blueprint == "DefaultFist" &&
                            Limb.DefaultBehavior.TryGetPart(out MeleeWeapon MW))
                        {
                            MW.PenBonus = WM_MMASkillTree.GetLevelValuePenetration(ParentObject.Level);
                            MW.BaseDamage = WM_MMASkillTree.GetLevelValueBaseDamage(ParentObject.Level);

                            if (!ParentObject.HasPart("KO_On_Finish"))
                            {
                                ParentObject.AddPart<KO_On_Finish>();

                                // AddPlayerMessage("Part added to " + ParentObject);
                            }
                        }
                    }
                }

                // AddPlayerMessage(FistProperties.BaseDamage);
            }
            return base.FireEvent(E);
        }

        public override bool RemoveSkill(GameObject GO)
        {
            return true;
        }
        
        public static string GetLevelValueBaseDamage(int Level)
        {
            return (1 + (Level/10)) + "d" + (1 + (Level / 5));
        }

        public static int GetLevelValuePenetration(int Level)
        {
            if (Level == 1)
            {
                return 1;
            }
            if (Level == 3)
            {
                return 1;
            }
            if (Level == 5)
            {
                return 1;
            }
            if (Level == 7)
            {
                return 1;
            }
            if (Level == 10)
            {
                return 2;
            }
            if (Level == 13)
            {
                return 2;
            }
            if (Level == 16)
            {
                return 2;
            }
            if (Level == 20)
            {
                return 2;
            }
            if (Level == 24)
            {
                return 2;
            }
            if (Level == 27)
            {
                return 2;
            }
            if (Level >= 30)
            {
                return 2 + (Level/10);
            }
            
            return 1;
        }

        public override bool HandleEvent(AttackerDealingDamageEvent E)
        {
            var eActor = E.Actor;
            var Defender = E.Object;
            
            var eWeapon = E.Weapon;
            if (eWeapon != null)
            {
                var eDamage = E.Damage.Amount;
                var eComboCounter = ParentObject.GetPart<WM_MMA_CombinationStrikesI>();
                
                if (eActor == ParentObject && eWeapon.HasPropertyOrTag("MMAHooks") || eWeapon.HasPropertyOrTag("MMAThrowable"))
                {
                    E.Damage.Amount += ParentObject.Level * (int)(eComboCounter.CurrentComboICounter*0.5f);
                }
            }
            
            return base.HandleEvent(E);
        }
        
        public static bool GetWeaponEventBooleanChecks(GameObject MMAPartIOwner, GameObject Weapon, GameObject Defender)
        {
            return (Defender.HasPart("Combat")
                     && MMAPartIOwner.HasBodyPart("Hand")
                     && Weapon.Blueprint == "DefaultMartialFist"
                    || Weapon.HasPart("MartialConditioningFistMod")
                    || Weapon.HasPart("NaturalEquipment")
                    || Weapon.HasPart("NaturalWeapon")
                    || Weapon.HasPropertyOrTag("WeaponUnarmed"));
        }
    }
}
