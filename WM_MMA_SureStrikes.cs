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
        public Guid SureStrikesActivatedAbilityID;

        public GameObject eDefender;
        public WM_MMA_SureStrikes()
        {
            Name = "WM_MMA_SureStrikes";
            DisplayName = "Sure Strikes";
        }

        public override void Register(GameObject Object)
        {
            Object.RegisterPartEvent(this, "CommandSureStrikes");
            Object.RegisterPartEvent(this, "ChainingSureStrikes");
            Object.RegisterPartEvent(this, "AttackerGetWeaponPenModifier");
            Object.RegisterPartEvent(this, "FailedChainingSureStrikes");
            Object.RegisterPartEvent(this, "GetWeaponPenModifier");
            Object.RegisterPartEvent(this, "AttackerHit");
        }

        public override bool FireEvent(Event E)
        {
            if (E.ID == "CommandSureStrikes")
            {
                AddPlayerMessage("Firing Sure strike commandsurestrikes");

                AwardSureStrikes = 0;

                var MMAAccess = ParentObject.GetPart<WM_MMA_CombinationStrikesI>();
                var ParentAgi = ParentObject.StatMod("Agility");

                SureStrike();

                AwardSureStrikes = ParentAgi;
                var eAttacker = _ParentObject;

                if (AwardSureStrikes <= 0)
                {
                    return base.FireEvent(E);
                }
                else
                {
                    if (Stat.Random(1, 100) <= 20 + (MMAAccess.CurrentComboICounter * 2))
                    {
                        --AwardSureStrikes;
                        AddPlayerMessage("inititate chaining strike event");
                        eAttacker.FireEvent(Event.New("ChainingSureStrikes", "Defender", eDefender, "Attacker", eAttacker));
                    }
                    else
                    {
                        --AwardSureStrikes;
                        eAttacker.FireEvent(Event.New("FailedChainingSureStrikes", "Defender", eDefender, "Attacker", eAttacker));

                    }
                }
            }
            if (E.ID == "FailedChainingSureStrikes")
            {
                var MMAAccess = ParentObject.GetPart<WM_MMA_CombinationStrikesI>();
                var eAttacker = _ParentObject;

                if (Stat.Random(1, 100) <= 20 + MMAAccess.CurrentComboICounter)
                {
                    --AwardSureStrikes;
                    AddPlayerMessage("initiate chaining strike event");
                    eAttacker.FireEvent(Event.New("ChainingSureStrikes", "Defender", eDefender, "Attacker", eAttacker));
                }
                else
                {
                    --AwardSureStrikes;
                    eAttacker.FireEvent(Event.New("FailedChainingSureStrikes", "Defender", eDefender, "Attacker", eAttacker));

                }
            }
            if (E.ID == "ChainingSureStrikes")
            {
                AddPlayerMessage("Chaining strike fires");

                var MMAAccess = ParentObject.GetPart<WM_MMA_CombinationStrikesI>();

                var eDefender = E.GetGameObjectParameter("Defender");
                var eAttacker = E.GetGameObjectParameter("Attacker");

                ChainingSureStrike(eDefender);

                if (Stat.Random(1, 100) <= 20 + (MMAAccess.CurrentComboICounter * 2))
                {
                    --AwardSureStrikes;
                    AddPlayerMessage("inititate chaining strike event");
                    eAttacker.FireEvent(Event.New("ChainingSureStrikes", "Defender", eDefender, "Attacker", eAttacker));
                }
                else
                {
                    --AwardSureStrikes;
                    eAttacker.FireEvent(Event.New("FailedChainingSureStrikes", "Defender", eDefender, "Attacker", eAttacker));
                }
            }
            return base.FireEvent(E);
        }

        public void ChainingSureStrike(GameObject Target)
        {
            AddPlayerMessage("Chaining Strikes Method Fires");
            PlayWorldSound("refract", 100f, 0, true);

            AddPlayerMessage("chaining eventhook?");
            Event EventHook = null;

            var PrimaryWeapon = ParentObject.GetPrimaryWeapon();

            var PrimaryWeaponTraits = PrimaryWeapon.GetPart<MeleeWeapon>();

            var FistPenBonus = PrimaryWeaponTraits.PenBonus;
            PrimaryWeaponTraits.AdjustBonusCap(FistPenBonus * 2);

            AddPlayerMessage("event changes");
            EventHook = Event.New("PerformMeleeAttack", 0, 0, 0);
            EventHook.SetParameter("PenBonus", FistPenBonus * 2);
            EventHook.SetParameter("PenCapBonus", ParentObject);
            EventHook.SetParameter("Attacker", ParentObject);
            EventHook.SetParameter("Defender", Target);

            AddPlayerMessage("fire eventhook?");
            ParentObject.FireEvent(EventHook);

            var eAttacker = Target;
            var MMAAccess = ParentObject.GetPart<WM_MMA_CombinationStrikesI>();

            if (Stat.Random(1, 100) <= 20 + (MMAAccess.CurrentComboICounter * 2))
            {
                --AwardSureStrikes;
                AddPlayerMessage("inititate chaining strike event");
                eAttacker.FireEvent(Event.New("ChainingSureStrikes", "Defender", eDefender, "Attacker", eAttacker));
            }
            else
            {
                --AwardSureStrikes;
                eAttacker.FireEvent(Event.New("FailedChainingSureStrikes", "Defender", eDefender, "Attacker", eAttacker));
            }
        }


        public void SureStrike()
        {
            var TargetCell = ParentObject.PickDirection();

            TextConsole _TextConsole = UI.Look._TextConsole;
            ScreenBuffer Buffer = TextConsole.ScrapBuffer;
            Core.XRLCore.Core.RenderMapToBuffer(Buffer);
            Cell cell = PickDirection();

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
            if (IsPlayer() && Target != null && PrimaryWeapon.Blueprint != "DefaultFist")
            {
                AddPlayerMessage("Invalid Weapon type.");
                return;
            }
            AddPlayerMessage("passed clears, throwing attack");

            PlayWorldSound("refract", 100f, 0, true);


            var PrimaryWeaponTraits = PrimaryWeapon.GetPart<MeleeWeapon>();

            var FistPenBonus = PrimaryWeaponTraits.PenBonus;

            EventHook = Event.New("PerformMeleeAttack", 0, 0, 0);
            EventHook.SetParameter("PenBonus", FistPenBonus * 2);
            EventHook.SetParameter("Attacker", ParentObject);
            EventHook.SetParameter("TargetCell", cell);
            EventHook.SetParameter("Defender", Target);

            eDefender = Target;

            ParentObject.FireEvent(EventHook);
            // ParentObject.PerformMeleeAttack(Target);
        }

        public override bool AddSkill(GameObject GO)
        {
            this.SureStrikesActivatedAbilityID = base.AddMyActivatedAbility("Sure Strikes", "CommandSureStrikes", "Skill", "Deliver an attack at double penetration so long as you aren't wielding a weapon in your primary hand. If the attack lands, there's scaling chance you will throw another sure strike.", ">", null, false, false, false, false, false, false, false, 20, null);

            return true;
        }

        public override bool RemoveSkill(GameObject GO)
        {
            return true;
        }
    }
}