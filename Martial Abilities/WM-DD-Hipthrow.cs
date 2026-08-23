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

    public class wmHipThrowAbility : IPart
    {
        public GameObject Target;
        public GameObject Grappler;
        public Cell TargetCell;

        public void wmHipThrow()
        {
            AddPlayerMessage("1");
            TextConsole _TextConsole = UI.Look._TextConsole;
            ScreenBuffer Buffer = TextConsole.ScrapBuffer;
            Core.XRLCore.Core.RenderMapToBuffer(Buffer);

            List<GameObject> hit = new List<GameObject>(1);
            List<Cell> usedCells = new List<Cell>(1);

            var Grappler = ParentObject;
            var GrapplersStrengthMod = (Grappler.Statistics["Strength"].Modifier);
            var GrapplerLevel = (Grappler.Statistics["Level"].Value);

            int DamageIntWallSmash = GrapplersStrengthMod + GrapplerLevel;
            int DamageIntBodySmash = (GrapplersStrengthMod + GrapplerLevel) / 2;

            var GrapplerAdjacentcells = Grappler.CurrentCell.GetAdjacentCells();
            foreach (var C in GrapplerAdjacentcells)
            {
                Target = C.GetFirstObjectWithEffect("Grappled");
            }

            bool GrappledCheck = Target.HasEffect<Grappled>();
            bool SaveDynamics = !Target.MakeSave(Stat: "Strength",
                                                    Difficulty: 8 + GrapplerLevel,
                                                    Attacker: Grappler,
                                                    AttackerStat: "Strength",
                                                    Vs: "Strength");

            var BlipTile = Target.Render.Tile;
            var BlipColorString = Target.Render.TileColor;
            var BlipDetailColor = Target.Render.DetailColor;
            int BlipDuration = 10;
            AddPlayerMessage("2");
            if (Target != null)
            {
                if (!Target.PhaseAndFlightMatches(Grappler))
                {
                    AddPlayerMessage("You cannot reach this creature!");
                }
                else if (SaveDynamics)
                {
                    var TargetLine = PickLine(
                        Length: 3 + (GrapplerLevel / 3),
                        VisLevel: AllowVis.Any);

                    for (int index = 1; index < TargetLine.Count; index++)
                    {
                        Cell cell = TargetLine[index];
                        Buffer.Goto(cell.X, cell.Y);

                        Target.TileParticleBlip(BlipTile, BlipColorString, BlipDetailColor, BlipDuration, false, Target.Render.HFlip, Target.Render.VFlip);

                        _TextConsole.DrawBuffer(Buffer);
                        System.Threading.Thread.Sleep(18);
                        GameObject obj = cell.FindObject(o => o.ConsiderSolidFor(Grappler) || (o.HasPart("Combat") && !o.Physics.Solid));


                        if (obj != null)
                        {
                            Target.TakeDamage(
                                Amount: DamageIntWallSmash,
                                Attributes: "Bashing",
                                DeathReason: "smashed against a wall.",
                                ThirdPersonDeathReason: "You were smashed against a wall.",
                                Owner: Grappler,
                                Attacker: Grappler,
                                Source: null,
                                Message: "from %t wrestling meanuver.");
                            TargetCell = cell;
                            break;
                        }
                    }
                    AddPlayerMessage("3");
                    Target.DirectMoveTo(TargetCell, EnergyCost: 10);
                }
            }
        }
    }
}