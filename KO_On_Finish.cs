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
        public override bool SameAs(IPart p)
        {
            return true;
        }

        public override bool WantEvent(int ID, int cascade)
        {
            return base.WantEvent(ID, cascade)
            || ID == BeforeDieEvent.ID
            || ID == AttackerDealingDamageEvent.ID;
        }

        public override void Register(GameObject go)
        {
            go.RegisterPartEvent((IPart)this, "BeginMove");
            base.Register(go);
        }

        public override bool HandleEvent(AttackerDealingDamageEvent E)
        {
            AddPlayerMessage("Execution Initilized");
            if (E.Actor.IsPlayer() && E.Object.HasEffect("Immobilized"))
            {
                AddPlayerMessage("Execute Initilized");
                E.Object.Die(ParentObject, ParentObject.it + " executes " + E.Object.it, "Executed by " + ParentObject.Its);
            }
            return base.HandleEvent(E);
        }
        public override bool HandleEvent(BeforeDieEvent E)
        {
            AddPlayerMessage("KOing Opponent");
            if (E.Killer.IsPlayer() && E.Weapon.HasPart("KO_On_Finish") && !E.Dying.HasEffect("Immobilized"))
            {
                AddPlayerMessage("KO Opponent");
                var KOdToughness = E.Dying.StatMod("Ego");
                var SaveDC = 40 - (KOdToughness * 10);

                E.Dying.hitpoints = 1;
                E.Dying.ApplyEffect(new Immobilized(SaveDC, "Toughness", "knocked unconscious", "{{red|KO'd!!!}}"));
            }
            else if (E.Killer.IsPlayer() && E.Killer.HasPart("KO_On_Finish") && !E.Dying.HasEffect("Immobilized"))
            {
                AddPlayerMessage("KO Opponent tag on parent");
                var KOdToughness = E.Dying.StatMod("Ego");
                var SaveDC = 40 - (KOdToughness * 10);

                E.Dying.hitpoints = 1;
                E.Dying.ApplyEffect(new Immobilized(SaveDC, "Toughness", "knocked unconscious", "{{red|KO'd!!!}}"));
            }
            return base.HandleEvent(E);
        }
    }
}
