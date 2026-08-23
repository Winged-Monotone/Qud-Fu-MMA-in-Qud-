using System;
using XRL.UI;
using XRL.Rules;
using XRL.Core;
using System.Collections.Generic;
using XRL.World.Parts;
using XRL.Language;
using ConsoleLib.Console;
using XRL.World.Effects;


namespace XRL.World.Parts
{
    [Serializable]

    public class wmSubdueAbility : IPart
    {
        public GameObject Target;
        public GameObject Grappler;
        public Cell TargetCell;

        public void wmSubdue()
        {
            // AddPlayerMessage("1");
            var Grappler = ParentObject;
            var GrapplersStrengthMod = (Grappler.Statistics["Strength"].Modifier);
            var GrapplerLevel = (Grappler.Statistics["Level"].Value);

            var GrapplerAdjacentcells = Grappler.CurrentCell.GetAdjacentCells();

            // AddPlayerMessage("2");
            foreach (var C in GrapplerAdjacentcells)
            {
                Target = C.GetFirstObjectWithEffect("Grappled");
            }
            // AddPlayerMessage("3");
            var TargetCon = Target.Statistics["Constitution"].Modifier;

            bool GrappledCheck = Target.HasEffect<Grappled>();
            bool SaveDynamics = !Target.MakeSave(Stat: "Constitution",
                                                    Difficulty: 8 + GrapplersStrengthMod + GrapplerLevel / 4,
                                                    Attacker: Grappler,
                                                    AttackerStat: "Strength",
                                                    Vs: "Strength");
            // AddPlayerMessage("4");
            if (GrappledCheck)
            {
                if (SaveDynamics)
                {
                    Target.ApplyEffect(new Incapacitated(360 - (TargetCon * 10), 24));
                }
            }
            // AddPlayerMessage("5");
        }
    }
}