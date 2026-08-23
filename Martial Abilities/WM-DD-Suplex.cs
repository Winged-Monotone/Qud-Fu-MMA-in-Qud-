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

    public class wmSuplexAbility : IPart
    {
        public GameObject Target;
        public GameObject Grappler;
        public Cell TargetCell;

        public void wmSuplex()
        {
            // AddPlayerMessage("1");
            var Grappler = ParentObject;
            var GrapplersStrengthMod = (Grappler.Statistics["Strength"].Modifier);
            var GrapplerLevel = (Grappler.Statistics["Level"].Value);

            int DamageIntSuplexSmash = (20 + GrapplerLevel) * GrapplersStrengthMod;

            // var TargetDir = PickDirectionS();
            // var OppositeSpace = Directions.GetOppositeDirection(TargetDir);
            // var TargetCell = Object.CurrentCell.GetCellFromDirection(TargetDir);

            var GrapplerAdjacentcells = Grappler.CurrentCell.GetAdjacentCells();
            foreach (var C in GrapplerAdjacentcells)
            {
                Target = C.GetFirstObjectWithEffect("Grappled");
            }

            var TargetDir = Target.GetDirectionToward(Grappler);
            var OppositeSpace = Directions.GetOppositeDirection(TargetDir);


            var ShakeIntensity = Target.GetIntProperty("Hitpoints") / 10;
            var ShakeDuration = Target.GetIntProperty("Hitpoints") / 50;

            bool GrappledCheck = Target.HasEffect<Grappled>();
            bool SaveDynamics = !Target.MakeSave(Stat: "Strength",
                                                    Difficulty: 8 + GrapplersStrengthMod,
                                                    Attacker: Grappler,
                                                    AttackerStat: "Strength",
                                                    Vs: "Strength");
            // AddPlayerMessage("2");
            if (Target.HasEffect<Grappled>())
            {
                if (!Target.PhaseAndFlightMatches(Grappler))
                {
                    AddPlayerMessage("You cannot reach this creature!");
                }
                if (SaveDynamics)
                {
                    Target.Move(Direction: OppositeSpace, Forced: true, EnergyCost: 10);
                    Target.TakeDamage(
                        Amount: DamageIntSuplexSmash,
                                Attributes: "Bashing",
                                DeathReason: "crushed by a suplex.",
                                ThirdPersonDeathReason: "You were suplexed, you died.",
                                Owner: Grappler,
                                Attacker: Grappler,
                                Source: null,
                                Message: "from %t suplex meanuver.");


                }
            }
            // AddPlayerMessage("3");
        }
    }
}
