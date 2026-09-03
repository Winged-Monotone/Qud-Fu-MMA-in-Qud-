using System;
using System.Collections.Generic;
using XRL.World.Anatomy;
using System.Text;
using XRL.Rules;
using XRL.UI;
using Qud.API;
using XRL.Language;
using XRL.Messages;
using ConsoleLib.Console;
using UnityEngine;
using XRL.World.Effects;

using XRL;
using XRL.World;
using XRL.World.Capabilities;
using XRL.World.Parts;
using XRL.World.Parts.Skill;

namespace XRL.World.Parts.Skill
{
    [Serializable]
    public class WM_MMA_SureStrikes : BaseSkill
    {
        public string WeaponType = "MartialConditioningFistMod";
        public int AwardSureStrikes;
        public int FistPenBonus;
        public Guid SureStrikesActivatedAbilityID;
        public GameObject eDefender;
        public WM_MMA_SureStrikes()
        {

        }
        public void UpdateCounter()
        {
            try
            {
                var MMAAccess = ParentObject.GetPart<WM_MMA_PathDawnGlider>();
                int SureStrikeBonus = MMAAccess.BonusSureStrike;

                var AA = MyActivatedAbility(this.SureStrikesActivatedAbilityID);

                if (AA != null && SureStrikeBonus <= 0)
                {
                    AA.DisplayName = "{{white|Sure Strikes}}";
                }
                else
                {
                    AA.DisplayName = "{{yellow|Sure Strikes x(" + (SureStrikeBonus) + ")}}";
                }
            }
            catch { }
        }

        public override void Register(GameObject Object, IEventRegistrar registrar)
        {
            Object.RegisterPartEvent(this, "CommandSureStrikes");
            Object.RegisterPartEvent(this, "ChainingSureStrikes");
            Object.RegisterPartEvent(this, "AttackerGetWeaponPenModifier");
            Object.RegisterPartEvent(this, "FailedChainingSureStrikes");
            Object.RegisterPartEvent(this, "AIGetOffensiveMutationList");
            Object.RegisterPartEvent(this, "EndTurn");
        }

        public override bool FireEvent(Event E)
        {
            if (E.ID == "AIGetOffensiveMutationList")
            {
                if (IsMyActivatedAbilityAIUsable(SureStrikesActivatedAbilityID, ParentObject))
                { E.AddAICommand("CommandSureStrikes"); }

            }
            else if (E.ID == "CommandSureStrikes")
            {
                // AddPlayerMessage("Firing Sure strike commandsurestrikes");

                AwardSureStrikes = 0;


                var MMAAccess = ParentObject.GetPart<WM_MMA_CombinationStrikesI>();
                var ParentAgi = ParentObject.StatMod("Agility");

                if (ParentObject.HasPart("WM_MMA_PathDawnGlider") && IsPlayer())
                {
                    var MMAComboPathDawnAccess = ParentObject.GetPart<WM_MMA_PathDawnGlider>();
                    PlayersSurestrike();
                    MMAComboPathDawnAccess.BonusSureStrike = 0;
                    UpdateCounter();
                }
                else if (IsPlayer())
                    PlayersSurestrike();


                AwardSureStrikes = ParentAgi;
                var eAttacker = _ParentObject;


                if (AwardSureStrikes <= 0)
                {
                    return base.FireEvent(E);
                }
                else
                {
                    // ChainFuntion();
                    ParentObject.CooldownActivatedAbility(SureStrikesActivatedAbilityID, 80 - (AwardSureStrikes * 10));
                }

                if (IsPlayer())
                { UpdateCounter(); }
            }
            else if (E.ID == "FailedChainingSureStrikes")
            {
                var MMAAccess = ParentObject.GetPart<WM_MMA_CombinationStrikesI>();
                var eAttacker = ParentObject;

                if (IsPlayer())
                    UpdateCounter();

                if (Stat.Random(1, 100) <= 20 + MMAAccess.CurrentComboICounter)
                {
                    // ChainFuntion();
                    ParentObject.CooldownActivatedAbility(SureStrikesActivatedAbilityID, 80 - (AwardSureStrikes * 10));
                }

                if (IsPlayer())
                    UpdateCounter();
            }
            else if (E.ID == "ChainingSureStrikes")
            {
                // AddPlayerMessage("Chaining strike fires");

                var MMAAccess = ParentObject.GetPart<WM_MMA_CombinationStrikesI>();

                var eDefender = E.GetGameObjectParameter("Defender");
                var eAttacker = E.GetGameObjectParameter("Attacker");

                ChainingSureStrike(eDefender);

                if (Stat.Random(1, 100) <= 20 + (MMAAccess.CurrentComboICounter * 2))
                {
                    // ChainFuntion();
                    ParentObject.CooldownActivatedAbility(SureStrikesActivatedAbilityID, 80 - (AwardSureStrikes * 10));
                }

                if (IsPlayer())
                    UpdateCounter();
            }
            else if (E.ID == "EndTurn")
            {
                if (IsPlayer())
                    UpdateCounter();
            }
            return base.FireEvent(E);
        }

        public void PlayersSurestrike()
        {
            var cell = PickDirection();
            ThrowSureStrike(cell);
        }

        public void ThrowSureStrike(Cell cell)
        {
            var WMMAHooks = ParentObject.GetPart<WM_MMA_CombinationStrikesI>();

            var ComboCounter = WMMAHooks.CurrentComboICounter;
            var PrimaryWeapon = ParentObject.GetPrimaryWeapon();
            var PrimaryWeaponTraits = PrimaryWeapon.GetPart<MeleeWeapon>();
            var FistPenBonus = PrimaryWeaponTraits.PenBonus;

            GameObject Target = cell.GetCombatTarget(Attacker: ParentObject);

            eDefender = Target;

            if (cell == null)
            {
                AddPlayerMessage("Must target an enemy!");
                return;
            }
            if (IsPlayer() && Target == null || Target.IsPlayer())
            {
                AddPlayerMessage("Invalid Target.");
                return;
            }

            PlayWorldSound("swiftstrikes", 0.5f, 0, true);

            Combat.PerformMeleeAttack(Attacker: ParentObject,
                                         Defender: eDefender,
                                         EnergyCost: 0, HitModifier: + 5,
                                         PenModifier: FistPenBonus + 10 + ComboCounter,
                                         PenCapModifier: 99);

            XDidYToZ(ParentObject, "throw focused percussive-strikes at", eDefender);

            UpdateCounter();
        }

        public void ChainingSureStrike(GameObject Target)
        {
            PlayWorldSound("swiftstrikes", 0.5f, 0, true);

            var WMMAHooks = ParentObject.GetPart<WM_MMA_CombinationStrikesI>();
            
            var ComboCounter = WMMAHooks.CurrentComboICounter;

            var PrimaryWeapon = ParentObject.GetPrimaryWeapon();
            var PrimaryWeaponTraits = PrimaryWeapon.GetPart<MeleeWeapon>();
            var FistPenBonus = PrimaryWeaponTraits.PenBonus;
            var HitModifier = 5;

            eDefender = Target;

            if (ParentObject.TryGetPart(out WM_MMA_PathDawnGlider WMMADGS))
            {
                HitModifier += WMMADGS.BonusSureStrike;
            }
            
            Combat.PerformMeleeAttack(Attacker: ParentObject,
                                         Defender: eDefender,
                                         EnergyCost: 0, HitModifier: + HitModifier,
                                         PenModifier: FistPenBonus + 10 + ComboCounter,
                                         PenCapModifier: 99);
            
            
            // ChainFuntion();
            
            if (IsPlayer())
                UpdateCounter();
            
        }

        // public void ChainFuntion()
        // {
        //     var MMAAccess = ParentObject.GetPart<WM_MMA_CombinationStrikesI>();
        //     var ParentAgi = ParentObject.StatMod("Agility");

        //     AwardSureStrikes = ParentAgi;
        //     var Attacker = _ParentObject;

        //     if (Stat.Random(1, 100) <= 20 + (MMAAccess.CurrentComboICounter * 2))
        //     {
        //         --AwardSureStrikes;
        //         Attacker.FireEvent(Event.New("ChainingSureStrikes", "Defender", eDefender, "Attacker", Attacker));
        //     }
        //     else
        //     {
        //         --AwardSureStrikes;
        //         Attacker.FireEvent(Event.New("FailedChainingSureStrikes", "Defender", eDefender, "Attacker", Attacker));
        //     }
        // }

        public override bool AddSkill(GameObject GO)
        {
            this.SureStrikesActivatedAbilityID = base.AddMyActivatedAbility(Name: "Sure Strikes", Command: "CommandSureStrikes", Class: "Skill", Description: "Deliver an attack at double penetration so long as you aren't wielding a weapon in your primary hand. If the attack lands, there's scaling chance you will throw another sure strike.", Icon: ">", Cooldown: 80);

            return true;
        }

        public override bool RemoveSkill(GameObject GO)
        {
            return true;
        }
    }
}