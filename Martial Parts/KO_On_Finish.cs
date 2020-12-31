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
            go.RegisterPartEvent((IPart)this, "DealDamage");
            go.RegisterPartEvent((IPart)this, "AttackerAfterAttack");
            go.RegisterPartEvent((IPart)this, "CommandToggleMercy");
        }

        public override bool FireEvent(Event E)
        {
            if (E.ID == "CommandToggleMercy")
            {
                // AddPlayerMessage("CommandToggleMercy");
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
            // if (E.ID == "AttackerHit")
            // {
            //     AddPlayerMessage("mercy attacker addon hit");
            //     var defender = E.GetGameObjectParameter("Defender");
            //     defender.RegisterPartEvent(this, "DealDamage");
            // }
            // else if (E.ID == "AttackerAfterAttack")
            // {
            //     AddPlayerMessage("mercy attacker remove after attack");

            //     var defender = E.GetGameObjectParameter("Defender");
            //     defender.UnregisterPartEvent(this, "DealDamage");

            // }
            else if (E.ID == "DealDamage")
            {
                var attacker = E.GetGameObjectParameter("Attacker");
                var weapon = E.GetGameObjectParameter("Weapon");
                var defender = E.GetGameObjectParameter("Defender");
                var damage = E.GetParameter<Damage>("Damage");
                if (attacker.IsPlayer() && (weapon.HasPart("KO_On_Finish") || attacker.HasPart("KO_On_Finish")) && ShowMercy == true)
                {
                    // AddPlayerMessage("mercy start mercy");

                    if (damage.Amount >= defender.hitpoints)
                    {
                        // AddPlayerMessage("mercy recalculates damage");

                        damage.Amount = (int)Math.Min(1, defender.hitpoints - 1); //Set damage equal to what we need to drop them to 1 HP
                    }
                    // AddPlayerMessage("KO in deal damage event");
                    var KOdToughness = defender.StatMod("Toughness");
                    var SaveDC = 40 - (KOdToughness * 10);

                    defender.ApplyEffect(new Incapacitated(2400, SaveDC));

                    return true;
                }
            }

            return base.FireEvent(E);
        }

        public override bool HandleEvent(AfterAddSkillEvent E)
        {
            var ActorAbilities = E.Actor.GetActivatedAbility(ActivatedAbilityID);

            // AddPlayerMessage("checking for mercy");
            if (E.Actor.IsPlayer() && ActorAbilities == null)
            {
                // AddPlayerMessage("adding for mercy");
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
