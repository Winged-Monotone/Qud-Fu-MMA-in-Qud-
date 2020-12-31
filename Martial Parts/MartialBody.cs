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
    }
}
