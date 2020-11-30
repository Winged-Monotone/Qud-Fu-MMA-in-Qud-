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
        public Guid SureStrikesActivatedAbilityID;
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
            Object.RegisterPartEvent(this, "GetWeaponPenModifier");
            Object.RegisterPartEvent(this, "AttackerHit");

        }

        public override bool FireEvent(Event E)
        {
            if (E.ID == "CommandSureStrikes")
            {
                AddPlayerMessage("Firing Sure strike commandsurestrikes");
                SureStrike();
            }
            if (E.ID == "GetWeaponPenModifier")
            {
                AddPlayerMessage("getting event Getweaponpenmodifier");
                var WeaponType = E.GetGameObjectParameter("Weapon");

                if (E.ID == "AttackerGetWeaponPenModifier" && WeaponType.HasPart("MartialConditioningFistMod"))
                {
                    AddPlayerMessage("getting event attackergetweaponpenmodifier");
                    E.SetParameter("PenBonus", E.GetIntParameter("PenBonus") * 2);
                    E.SetParameter("CapBonus", E.GetIntParameter("CapBonus") * 2);

                }
                if (E.ID == "AttackerHit")
                {
                    AddPlayerMessage("Attack hit");

                    var eAttacker = E.GetGameObjectParameter("Attacker");
                    var eDefender = E.GetGameObjectParameter("Defender");

                    var MMAAccess = ParentObject.GetPart<WM_MMA_CombinationStrikesI>();
                    var ParentAgi = ParentObject.StatMod("Agility");

                    for (int i = 0; i < ParentAgi; i++)
                    {
                        if (Stat.Random(1, 100) <= 20 + MMAAccess.CurrentComboICounter)
                        {
                            AddPlayerMessage("inititate chaining strike event");
                            eAttacker.FireEvent(Event.New("ChainingSureStrikes", "Defender", eDefender, "Attacker", eAttacker));
                        }
                    }
                }
                if (E.ID == "ChainingSureStrikes")
                {
                    AddPlayerMessage("Chaining strike fires");

                    var eDefender = E.GetGameObjectParameter("Defender");
                    var eAttacker = E.GetGameObjectParameter("Attacker");

                    ChainingSureStrike(eDefender);
                    if (E.ID == "GetWeaponPenModifier")
                    {
                        AddPlayerMessage("Getwep pen again");
                        var WeaponTypeII = E.GetGameObjectParameter("Weapon");

                        if (E.ID == "AttackerGetWeaponPenModifier" && WeaponTypeII.HasPart("MartialConditioningFistMod"))
                        {
                            AddPlayerMessage("AttackerGetpenagain");
                            E.SetParameter("PenBonus", E.GetIntParameter("PenBonus") * 2);
                            E.SetParameter("CapBonus", E.GetIntParameter("CapBonus") * 2);
                        }
                    }
                }
            }

            return base.FireEvent(E);
        }

        public void ChainingSureStrike(GameObject Target)
        {
            AddPlayerMessage("Chaining Strikes Method Fires");
            PlayWorldSound("Woosh", 1.0f, 25, true);
            ParentObject.PerformMeleeAttack(Target);
        }


        public void SureStrike()
        {
            var TargetCell = ParentObject.PickDirection();

            TextConsole _TextConsole = UI.Look._TextConsole;
            ScreenBuffer Buffer = TextConsole.ScrapBuffer;
            Core.XRLCore.Core.RenderMapToBuffer(Buffer);
            Cell cell = PickDirection();

            var PrimaryWeapon = ParentObject.GetPrimaryWeapon();

            Event @event = null;
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

            PlayWorldSound("Woosh", 1.5f, 50, true);


            // var PrimaryWeaponTraits = PrimaryWeapon.GetPart<MeleeWeapon>();

            // var FistPenBonus = PrimaryWeaponTraits.PenBonus;

            // @event = Event.New("AttackerGetWeaponPenModifier", 0, 0, 0);
            // @event.SetParameter("PenBonus", FistPenBonus * 2);
            // @event.SetParameter("Attacker", ParentObject);

            ParentObject.PerformMeleeAttack(Target);
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