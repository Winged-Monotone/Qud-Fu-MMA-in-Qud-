using System;
using System.Collections.Generic;
using XRL.Core;
using XRL.Rules;
using XRL.UI;
using XRL.World;
using XRL.Liquids;
using XRL.World.Parts;
using System.Linq;



namespace XRL.World.Effects
{
    [Serializable]
    public class Drunken : Effect
    {

        public Drunken()
        {
            base.DisplayName = "{{M|Drunken}}";
        }

        public Drunken(int Duration)
            : this()
        {
            base.Duration = Duration;
        }

        public override bool Apply(GameObject Object)
        {
            if (Object.HasEffect("Drunken"))
            {
                Drunken Drunken = Object.GetEffect("Drunken") as Drunken;
                if (Duration > Drunken.Duration)
                {
                    Drunken.Duration = Duration;
                }
                return true;
            }
            return true;
        }


        public List<string> SpecialStance = new List<string>()
        {
            "SlumberStance",
            "DawnStance",
            "SaltbackStance",
            "SalthopperStance",
            "AstralCabbyStance",
        };
        public void GetDrunkenEffects()
        {
            if (!SpecialStance.Any(Object.HasEffect))
            {



            }
        }

        public override bool FireEvent(Event E)
        {


            return base.FireEvent(E);

        }

        public override string GetDetails()
        {
            return "The warm draught reveals a hidden genius in your fighting form.";
        }


    }
}