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
            Object.RegisterPartEvent(this, "AttackerGetWeaponPenModifier");
            Object.RegisterPartEvent(this, "AttackerGetWeaponPenModifier");
        }

        // public override bool FireEvent(Event E)
        // {
        //     if (E.ID == "CommandSureStrikes")
        //     {
        //         SureStrike();


        //         if (E.ID == "GetWeaponPenModifier")
        //         {
        //             var PenBonus = E.GetGameObjectParameter("Weapon");


        //             if (E.ID == "AttackerGetWeaponPenModifier")
        //             {
        //                 var PenBonus = E.GetIntParameter("PenBonus");
        //                 var CapBonus = E.GetIntParameter("CapBonus");
        //                 var SSPenBonus = PenBonus * 2;
        //                 var SSCapBonus = CapBonus * 2;



        //             }
        //         }
        //     }
        // }

        public void SureStrike()
        {
            var TargetCell = ParentObject.PickDirection();

            TextConsole _TextConsole = UI.Look._TextConsole;
            ScreenBuffer Buffer = TextConsole.ScrapBuffer;
            Core.XRLCore.Core.RenderMapToBuffer(Buffer);
            Cell cell = PickDirection();

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

            PlayWorldSound("Woosh", 1.5f, 50, true);

            var PrimaryWeapon = ParentObject.GetPrimaryWeapon();
            var PrimaryWeaponTraits = PrimaryWeapon.GetPart<MeleeWeapon>();

            var FistPenBonus = PrimaryWeaponTraits.PenBonus;

            @event = Event.New("AttackerGetWeaponPenModifier", 0, 0, 0);
            @event.SetParameter("PenBonus", FistPenBonus * 2);
            @event.SetParameter("Attacker", ParentObject);

            ParentObject.PerformMeleeAttack(Target);
        }

        public override bool AddSkill(GameObject GO)
        {
            return true;
        }

        public override bool RemoveSkill(GameObject GO)
        {
            this.SureStrikesActivatedAbilityID = base.AddMyActivatedAbility("Sure Strikes", "CommandSureStrikes", "Skill", "Deliver an attack at double penetration so long as you aren't wielding a weapon in your primary hand. If the attack lands, there's scaling chance you will throw another sure strike.", ">", null, true, false, false, false, false, false, false, -1, null);
            return true;
        }
    }
}