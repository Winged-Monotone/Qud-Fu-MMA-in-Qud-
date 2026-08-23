using System;
using XRL.UI;
using XRL.Rules;
using XRL.Core;
using System.Collections.Generic;
using XRL.World.Parts;
using XRL.Language;
using ConsoleLib.Console;
using XRL.World.Effects;




namespace XRL.World.Effects
{
    [Serializable]

    public class wmGrappleAbility : Effect
    {
        public GameObject Target;
        public GameObject Grappler;
        public Cell TargetCell;
        public void wmGrapple()
        {
            // AddPlayerMessage("0 : Execute Grappling.");

            var Grappler = Object;
            var GrapplersStrengthMod = (Grappler.Statistics["Strength"].Modifier);
            var GrapplerLevel = (Grappler.Statistics["Level"].Value);

            Target = PickDirection().GetCombatTarget();

            AddPlayerMessage("Combat target : " + Target.DisplayName);


            bool GrappledCheck = Target.HasEffect<Grappled>();
            bool SaveDynamics = !Target.MakeSave(Stat: "Strength",
                                                    Difficulty: 8 + GrapplerLevel,
                                                    Attacker: Grappler,
                                                    AttackerStat: "Strength",
                                                    Vs: "Strength");

            // AddPlayerMessage("1: Beginning Grapple Command.");

            if (TargetCell.GetCombatObject() == null && Target.PhaseAndFlightMatches(Object))
            {
                // AddPlayerMessage("2: Destination Selected, But Nothing to Grapple.");

                if (Object.IsPlayer())
                    AddPlayerMessage("There is nothing here to grapple.");
            }
            else
            {
                // AddPlayerMessage("3: Assigning Combat Target");

                Target = TargetCell.GetCombatObject();

                AddPlayerMessage("Target: " + Target.DisplayName);
            }

            // AddPlayerMessage("4: Checking target Viability.");

            if (Target != null)
            {

                // AddPlayerMessage("5: Setting Variables.");

                if (!SaveDynamics && !GrappledCheck)
                {
                    // AddPlayerMessage("6: Creature failed Save.");

                    Target.ApplyEffect(new Grappled(Grappler: Object));
                    XDidYToZ(Actor: Object, Verb: "grapple", Object: Target, EndMark: "!");

                    // AddPlayerMessage("7: Effect Applied");
                }
                else if (!GrappledCheck && SaveDynamics)
                {
                    // AddPlayerMessage("8: Failed");
                    XDidYToZ(Actor: Object, Verb: "fail to grapple", Object: Target, EndMark: "!");
                }
                else if (GrappledCheck)
                {
                    Target.RemoveEffect(new Grappled());
                    XDidYToZ(Actor: Object, Verb: "release", Object: Target, EndMark: "!");

                }

                // AddPlayerMessage("7: End Grapple Effect");
            }
        }


    }
}