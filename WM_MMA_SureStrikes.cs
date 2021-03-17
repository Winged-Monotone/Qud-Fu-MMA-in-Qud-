using System;
using System.Collections.Generic;
using System.Text;
using XRL.Rules;
using XRL.UI;
using Qud.API;
using XRL.Language;
using XRL.Messages;
using ConsoleLib.Console;
using UnityEngine;


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
            Name = "WM_MMA_SureStrikes";
            DisplayName = "Sure Strikes";
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

        public override void Register(GameObject Object)
        {
            Object.RegisterPartEvent(this, "CommandSureStrikes");
            Object.RegisterPartEvent(this, "ChainingSureStrikes");
            Object.RegisterPartEvent(this, "AttackerGetWeaponPenModifier");
            Object.RegisterPartEvent(this, "FailedChainingSureStrikes");
            Object.RegisterPartEvent(this, "GetWeaponPenModifier");
            Object.RegisterPartEvent(this, "EndTurn");
        }

        public void ChainFuntion()
        {
            var MMAAccess = ParentObject.GetPart<WM_MMA_CombinationStrikesI>();
            var ParentAgi = ParentObject.StatMod("Agility");

            AwardSureStrikes = ParentAgi;
            var Attacker = _ParentObject;

            if (Stat.Random(1, 100) <= 20 + (MMAAccess.CurrentComboICounter * 2))
            {
                --AwardSureStrikes;
                // AddPlayerMessage("inititate chaining strike event");
                Attacker.FireEvent(Event.New("ChainingSureStrikes", "Defender", eDefender, "Attacker", Attacker));
            }
            else
            {
                --AwardSureStrikes;
                Attacker.FireEvent(Event.New("FailedChainingSureStrikes", "Defender", eDefender, "Attacker", Attacker));
            }


        }

        public override bool FireEvent(Event E)
        {
            if (E.ID == "CommandSureStrikes")
            {
                // AddPlayerMessage("Firing Sure strike commandsurestrikes");

                AwardSureStrikes = 0;

                var MMAAccess = ParentObject.GetPart<WM_MMA_CombinationStrikesI>();
                var ParentAgi = ParentObject.StatMod("Agility");

                PlayersSurestrike();

                AwardSureStrikes = ParentAgi;
                var eAttacker = _ParentObject;


                if (AwardSureStrikes <= 0)
                {
                    return base.FireEvent(E);
                }
                else
                {
                    ChainFuntion();
                    ParentObject.CooldownActivatedAbility(SureStrikesActivatedAbilityID, 80 - (AwardSureStrikes * 10));
                }

                UpdateCounter();
            }
            if (E.ID == "FailedChainingSureStrikes")
            {
                var MMAAccess = ParentObject.GetPart<WM_MMA_CombinationStrikesI>();
                var eAttacker = _ParentObject;

                UpdateCounter();

                if (Stat.Random(1, 100) <= 20 + MMAAccess.CurrentComboICounter)
                {
                    ChainFuntion();
                    ParentObject.CooldownActivatedAbility(SureStrikesActivatedAbilityID, 80 - (AwardSureStrikes * 10));
                }

                UpdateCounter();
            }
            if (E.ID == "ChainingSureStrikes")
            {
                // AddPlayerMessage("Chaining strike fires");

                var MMAAccess = ParentObject.GetPart<WM_MMA_CombinationStrikesI>();

                var eDefender = E.GetGameObjectParameter("Defender");
                var eAttacker = E.GetGameObjectParameter("Attacker");

                ChainingSureStrike(eDefender);

                if (Stat.Random(1, 100) <= 20 + (MMAAccess.CurrentComboICounter * 2))
                {
                    ChainFuntion();
                    ParentObject.CooldownActivatedAbility(SureStrikesActivatedAbilityID, 80 - (AwardSureStrikes * 10));
                }

                UpdateCounter();
            }
            if (E.ID == "EndTurn")
            {
                UpdateCounter();
            }
            return base.FireEvent(E);
        }

        public void ChainingSureStrike(GameObject Target)
        {
            // AddPlayerMessage("Chaining Strikes Method Fires");
            PlayWorldSound("swiftstrikes", 0.5f, 0, true);

            // AddPlayerMessage("chaining eventhook?");
            Event EventHook = null;

            var PrimaryWeapon = ParentObject.GetPrimaryWeapon();

            var PrimaryWeaponTraits = PrimaryWeapon.GetPart<MeleeWeapon>();

            FistPenBonus = PrimaryWeaponTraits.PenBonus;
            PrimaryWeaponTraits.AdjustBonusCap(FistPenBonus * 2);

            // AddPlayerMessage("event changes");
            EventHook = Event.New("PerformMeleeAttack", 0, 0, 0);
            EventHook.SetParameter("PenBonus", FistPenBonus * 2);
            EventHook.SetParameter("PenCapBonus", ParentObject);
            EventHook.SetParameter("Attacker", ParentObject);
            EventHook.SetParameter("Defender", Target);

            // AddPlayerMessage("fire eventhook?");
            ParentObject.FireEvent(EventHook);

            var eAttacker = Target;
            var MMAAccess = ParentObject.GetPart<WM_MMA_CombinationStrikesI>();

            ChainFuntion();
            UpdateCounter();
        }


        public void PlayersSurestrike()
        {
            TextConsole _TextConsole = UI.Look._TextConsole;
            ScreenBuffer Buffer = TextConsole.ScrapBuffer;
            Core.XRLCore.Core.RenderMapToBuffer(Buffer);
            var cell = PickDirection();

            ThrowSureStrike(cell);
        }

        public void ThrowSureStrike(Cell cell)
        {
            // var TargetCell = ParentObject.PickDirection();

            var PrimaryWeapon = ParentObject.GetPrimaryWeapon();

            Event EventHook = null;
            GameObject Target = cell.FindObject(o => o.HasPart("Brain"));

            if (cell == null)
            {
                AddPlayerMessage("Must target an enemy!");
                return;
            }
            if (IsPlayer() && Target == null)
            {
                AddPlayerMessage("Invalid Target.");
                return;
            }
            // if (IsPlayer() && Target != null && !PrimaryWeapon.HasPart("MartialConditioningFistMod") || !PrimaryWeapon.HasPart("MartialConditioningFistMod"))
            // {
            //     AddPlayerMessage("Invalid Weapon type.");
            //     return;
            // }
            // AddPlayerMessage("passed clears, throwing attack");

            PlayWorldSound("swiftstrikes", 0.5f, 0, true);


            var PrimaryWeaponTraits = PrimaryWeapon.GetPart<MeleeWeapon>();

            var FistPenBonus = PrimaryWeaponTraits.PenBonus;

            EventHook = Event.New("PerformMeleeAttack", 0, 0, 0);
            EventHook.SetParameter("PenBonus", FistPenBonus * 2);
            EventHook.SetParameter("Attacker", ParentObject);
            EventHook.SetParameter("TargetCell", cell);
            EventHook.SetParameter("Defender", Target);

            eDefender = Target;

            AddPlayerMessage(ParentObject.it + " throws focused, percussive strikes at " + eDefender.it);

            ParentObject.FireEvent(EventHook);
            UpdateCounter();
            // ParentObject.PerformMeleeAttack(Target);
        }

        public override bool AddSkill(GameObject GO)
        {
            this.SureStrikesActivatedAbilityID = base.AddMyActivatedAbility("Sure Strikes", "CommandSureStrikes", "Skill", "Deliver an attack at double penetration so long as you aren't wielding a weapon in your primary hand. If the attack lands, there's scaling chance you will throw another sure strike.", ">", null, false, false, false, false, false, false, false, 80, null);

            return true;
        }

        public override bool RemoveSkill(GameObject GO)
        {
            return true;
        }
    }
}