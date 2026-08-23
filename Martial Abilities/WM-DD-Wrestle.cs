
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
    public class wmWrestleAbility : Effect
    {
        public GameObject Target;
        public GameObject Grappler;
        public Cell TargetCell;
        private string ForcedMoveDirection;
        public void wmWrestleCommand()
        {

            // AddPlayerMessage("1");

            var Grappler = Object;
            var GrapplersStrengthMod = (Grappler.Statistics["Strength"].Modifier);
            var GrapplerLevel = (Grappler.Statistics["Level"].Value);


            int DamageIntWallSmash = GrapplersStrengthMod + GrapplerLevel;
            int DamageIntBodySmash = (GrapplersStrengthMod + GrapplerLevel) / 2;

            var GrapplerAdjacentcells = Grappler.CurrentCell.GetAdjacentCells();
            foreach (var C in GrapplerAdjacentcells)
            {
                Target = C.GetFirstObjectWithEffect("Grappled");
            }
            // AddPlayerMessage("2");

            bool GrappledCheck = Target.HasEffect<Grappled>();
            bool SaveDynamics = !Target.MakeSave(Stat: "Strength",
                                                    Difficulty: 8 + GrapplerLevel,
                                                    Attacker: Grappler,
                                                    AttackerStat: "Strength",
                                                    Vs: "Strength");

            // AddPlayerMessage("3");

            if (GrappledCheck && Target != null)
            {
                // AddPlayerMessage("3a");

                if (!Target.PhaseAndFlightMatches(Grappler))
                {
                    AddPlayerMessage("You cannot reach this creature!");
                }
                else if (SaveDynamics)
                {
                    var TargetDirection = PickDirectionS();
                    var TargetCell = Object.CurrentCell.GetCellFromDirection(TargetDirection);

                    if (TargetCell == Target.CurrentCell)
                    {
                        AddPlayerMessage("You must choose a different space than your target's current.");
                    }
                    else if (TargetCell.HasWall() || TargetCell.HasObjectWithPart("Furniture"))
                    {
                        var VisceralRandom = Stat.Random(1, 100);
                        var eWall = TargetCell.GetFirstObjectWithPart("Wall");
                        var eWallHPR10 = eWall.GetIntProperty("Hitpoints") / 10;

                        // AddPlayerMessage("3b");
                        if (VisceralRandom <= 33)
                        {

                            Target.Splatter("*");
                            Target.TakeDamage(Amount: DamageIntWallSmash + eWallHPR10, Attributes: "Bashing", DeathReason: "smashed against a wall.", ThirdPersonDeathReason: "You were smashed against a wall.", Owner: Grappler, Attacker: Grappler, Source: null, Message: "from %t wrestling meanuver.");
                        }
                        else if (VisceralRandom <= 66)
                        {
                            int DamageInt = GrapplersStrengthMod + GrapplerLevel;

                            Target.Splatter("*");
                            Target.TakeDamage(Amount: DamageIntWallSmash + eWallHPR10, Attributes: "Bashing", DeathReason: "finely mashed against a wall.", ThirdPersonDeathReason: "You were finely mashed against a wall.", Owner: Grappler, Attacker: Grappler, Source: null, Message: "from %t wrestling meanuver.");
                        }
                        else if (VisceralRandom <= 100)
                        {
                            int DamageInt = GrapplersStrengthMod + GrapplerLevel;

                            Target.Splatter("*");
                            Target.TakeDamage(Amount: DamageIntWallSmash + eWallHPR10, Attributes: "Bashing", DeathReason: "splattered against a wall.", ThirdPersonDeathReason: "You were splattered against a wall.", Owner: Grappler, Attacker: Grappler, Source: null, Message: "from %t wrestling meanuver.");
                        }
                        // AddPlayerMessage("3c");
                    }
                    else if (TargetCell.HasCombatObject())
                    {
                        var CreatureInCell = TargetCell.GetCombatObject();

                        bool SaveDynamics2 = !CreatureInCell.MakeSave(Stat: "Strength",
                                                        Difficulty: 8 + GrapplerLevel,
                                                        Attacker: Grappler,
                                                        AttackerStat: "Strength",
                                                        Vs: "Strength");

                        CreatureInCell.ShatterSplatter();
                        CreatureInCell.TakeDamage(Amount: DamageIntBodySmash, Attributes: "Bashing", DeathReason: "colliding into another with vicious force.", ThirdPersonDeathReason: "You were splattered against another through wrestling.", Owner: Grappler, Attacker: Grappler, Source: null, Message: "from %t wrestling meanuver.");
                        if (SaveDynamics2)
                        {
                            CreatureInCell.Move(
                                Direction: Directions.GetRandomDirection(),
                                Forced: true,
                                NearestAvailable: true,
                                EnergyCost: 50);
                            Target.TakeDamage(Amount: DamageIntBodySmash, Attributes: "Bashing", DeathReason: "colliding into another with vicious force.", ThirdPersonDeathReason: "You were splattered against another through wrestling.", Owner: Grappler, Attacker: Grappler, Source: null, Message: "from %t wrestling meanuver.");
                            Target.Move(Direction: TargetDirection,
                            Forced: true,
                                NearestAvailable: true,
                                EnergyCost: 50);
                        }
                        else
                        {
                            Target.TakeDamage(Amount: DamageIntBodySmash, Attributes: "Bashing", DeathReason: "colliding into another with vicious force.", ThirdPersonDeathReason: "You were splattered against another through wrestling.", Owner: Grappler, Attacker: Grappler, Source: null, Message: "from %t wrestling meanuver.");
                        }
                    }
                    else
                    {

                        AddPlayerMessage("You wrestle with " + Target.the + "and force them to move into a space.");
                        Target.Move(Direction: TargetDirection,
                            Forced: true,
                                NearestAvailable: true,
                                EnergyCost: 50);
                    }
                    // AddPlayerMessage("3d");
                }
                else
                {
                    AddPlayerMessage(Target.the + "resist your attempts to wrestle them into a different space.");
                }
            }
            // AddPlayerMessage("4");

        }
    }
}