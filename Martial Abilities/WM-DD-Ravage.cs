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

    public class wmRavageAbility : IPart
    {
        public GameObject Target;
        public GameObject Grappler;
        public Cell TargetCell;


        public void wmRavage()
        {
            // AddPlayerMessage("1");
            var Grappler = ParentObject;
            var GrapplersStrengthMod = (Grappler.Statistics["Strength"].Modifier);
            var GrapplerLevel = (Grappler.Statistics["Level"].Value);
            // AddPlayerMessage("2");
            var GrapplerAdjacentcells = Grappler.CurrentCell.GetAdjacentCells();
            foreach (var C in GrapplerAdjacentcells)
            {
                Target = C.GetFirstObjectWithEffect("Grappled");
            }
            // AddPlayerMessage("3");
            int DamageIntRavage = (10 + GrapplerLevel) * GrapplersStrengthMod;

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
                    var TargetBodypart = Target.GetRandomConcreteBodyPart();
                    TargetBodypart.Dismember();
                    XDidYToZ(Actor: Grappler, Verb: "destroy", Object: Target, Extra: TargetBodypart.Name, EndMark: "!");
                    Target.TakeDamage(
                        Amount: DamageIntRavage,
                                Attributes: "Bashing",
                                DeathReason: "crushed by a suplex.",
                                ThirdPersonDeathReason: "You were suplexed, you died.",
                                Owner: Grappler,
                                Attacker: Grappler,
                                Source: null,
                                Message: "from %t suplex meanuver.");
                }
            }
            // AddPlayerMessage("5");
        }
    }
}