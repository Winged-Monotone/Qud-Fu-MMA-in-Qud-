using System;
using XRL.UI;
using XRL.Rules;
using XRL.World.Effects;
using System.Collections.Generic;
using XRL.World.Capabilities;
using ConsoleLib.Console;
using XRL.Core;

namespace XRL.World.Parts
{
    [Serializable]
    public class KO_On_Finish : IPart
    {
        public Guid ActivatedAbilityID;
        public bool ShowMercy = false;
        public override bool SameAs(IPart p)
        {
            return true;
        }

        public override bool WantEvent(int ID, int cascade)
        {
            return base.WantEvent(ID, cascade)
            || ID == KilledEvent.ID
            || ID == AfterAddSkillEvent.ID;
        }

        public override void Register(GameObject go)
        {
            go.RegisterPartEvent((IPart)this, "AttackerHit");
            go.RegisterPartEvent((IPart)this, "AttackerAfterAttack");
            go.RegisterPartEvent((IPart)this, "CommandToggleMercy");
        }

        public override bool FireEvent(Event E)
        {
            if (E.ID == "CommandToggleMercy")
            {
                ToggleMyActivatedAbility(ActivatedAbilityID);
                if (IsMyActivatedAbilityToggledOn(ActivatedAbilityID))
                {
                    ShowMercy = true;
                    AddPlayerMessage("You will pull your punches, and strike with with no intent to kill. For now, your enemies are fortunate fools. For you show mercy, where others wouldn't.");
                }
                else
                {
                    ShowMercy = false;
                }
            }
            if (E.ID == "AttackerHit")
            {
                var defender = E.GetGameObjectParameter("Defender");
                defender.RegisterPartEvent(this, "BeforeDie");
            }
            else if (E.ID == "AttackerAfterAttack")
            {
                var defender = E.GetGameObjectParameter("Defender");
                defender.UnregisterPartEvent(this, "BeforeDie");

            }
            else if (E.ID == "BeforeDie")
            {
                var Dying = E.GetGameObjectParameter("Dying");
                var Killer = E.GetGameObjectParameter("Killer");
                var Weapon = E.GetGameObjectParameter("Weapon");

                if (Killer.IsPlayer() && Weapon.HasPart("KO_On_Finish") && ShowMercy == true)
                {
                    AddPlayerMessage("KO Opponent");
                    var KOdToughness = Dying.StatMod("Toughness");
                    var SaveDC = 9999 - (KOdToughness * 10);

                    Dying.hitpoints = 1;
                    Dying.ApplyEffect(new Incapacitated(2400, SaveDC));

                    return false;
                }
                else if (Killer.IsPlayer() && Killer.HasPart("KO_On_Finish") && ShowMercy == true)
                {
                    AddPlayerMessage("KO Opponent tag on parent");
                    var KOdToughness = Dying.StatMod("Ego");
                    var SaveDC = 40 - (KOdToughness * 10);

                    Dying.hitpoints = 1;
                    Dying.ApplyEffect(new Incapacitated(2400, SaveDC));

                    return false;
                }
                if (Killer.IsPlayer() && Dying.HasEffect("Incapacitated") && ShowMercy == false)
                {
                    AddPlayerMessage("Execute Initilized");
                    Dying.Die(ParentObject, ParentObject.it + " executes " + Dying.it, "Executed by " + ParentObject.Its);
                }
            }

            return true;
        }

        public override bool HandleEvent(AfterAddSkillEvent E)
        {
            var ActorAbiltiies = E.Actor.GetActivatedAbility(ActivatedAbilityID);

            AddPlayerMessage("checking for mercy");
            if (E.Actor.IsPlayer() && ActorAbiltiies == null)
            {
                AddPlayerMessage("adding for mercy");
                this.ActivatedAbilityID = base.AddMyActivatedAbility("Mercy", "CommandToggleMercy", "Skill", "Activate to show mercy to your opponents, knocking them unconscious instead of killing them. You will not gain xp from {{red|KO'd!}} enemies.", ">", null, true, false, false, false, false, false, false, -1, null);
            }
            return base.HandleEvent(E);
        }

        // public override bool HandleEvent(KilledEvent E)
        // {
        //     AddPlayerMessage("KOing Opponent");
        //     if (E.Killer.IsPlayer() && E.Weapon.HasPart("KO_On_Finish") && !E.Dying.HasEffect("Immobilized"))
        //     {
        //         AddPlayerMessage("KO Opponent");
        //         var KOdToughness = E.Dying.StatMod("Ego");
        //         var SaveDC = 40 - (KOdToughness * 10);

        //         E.Dying.hitpoints = 1;
        //         E.Dying.ApplyEffect(new Immobilized(SaveDC, "Toughness", "knocked unconscious", "{{red|KO'd!!!}}"));
        //     }

        //     else if (E.Killer.IsPlayer() && E.Killer.HasPart("KO_On_Finish") && !E.Dying.HasEffect("Immobilized"))
        //     {
        //         AddPlayerMessage("KO Opponent tag on parent");
        //         var KOdToughness = E.Dying.StatMod("Ego");
        //         var SaveDC = 40 - (KOdToughness * 10);

        //         E.Dying.hitpoints = 1;
        //         E.Dying.ApplyEffect(new Immobilized(SaveDC, "Toughness", "knocked unconscious", "{{red|KO'd!!!}}"));
        //     }
        //     return base.HandleEvent(E);
        // }
    }
}
