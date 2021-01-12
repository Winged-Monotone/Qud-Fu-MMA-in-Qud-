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
    public class MartialBody : IPart
    {
        public Guid ActivatedAbilityID;
        public override bool SameAs(IPart p)
        {
            return true;
        }

        public override bool WantEvent(int ID, int cascade)
        {
            return base.WantEvent(ID, cascade)
            || ID == InventoryActionEvent.ID;
        }

        public override void Register(GameObject Object)
        {
            Object.RegisterPartEvent(this, "DrinkingFrom");
            base.Register(Object);
        }

        public override bool HandleEvent(InventoryActionEvent E)
        {

            return base.HandleEvent(E);
        }

        public override bool FireEvent(Event E)
        {
            // AddPlayerMessage("Wine Inventory Action Event");
            if (E.ID == "DrinkingFrom" && (E.GetParameter("Container") as GameObject).LiquidVolume.ContainsLiquid("wine") && !ParentObject.HasEffect("Drunken"))
            {
                // AddPlayerMessage("Drunken");
                ParentObject.ApplyEffect(new Drunken(25 + (10 * ParentObject.Statistics["Level"].BaseValue / 5)));
            }

            return base.FireEvent(E);
        }
    }
}
