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

        public override bool HandleEvent(InventoryActionEvent E)
        {
            AddPlayerMessage("Wine Inventory Action Event");
            if (E.Command == "Drink" && E.Item != null)
            {
                AddPlayerMessage("Drink Found?");
                LiquidVolume lv = E.Item.GetPart<LiquidVolume>();
                if (lv != null)
                {
                    AddPlayerMessage("Liquid Var Set");
                    if (lv.ContainsLiquid("wine"))
                    {
                        E.Actor.ApplyEffect(new Drunken(10));
                        AddPlayerMessage("Drunken Applied");
                    }
                }
            }

            return base.HandleEvent(E);
        }
    }
}
