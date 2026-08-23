using System;
using System.Collections.Generic;
using XRL.Core;
using XRL.Rules;
using XRL.UI;
using XRL.World;
using XRL.Liquids;
using XRL.World.Parts;
using System.Linq;
using XRL.World.Parts.Mutation;
using XRL.World.Parts.Skill;

using Mathf = UnityEngine.Mathf;

using MMA_ComboStrikeI = XRL.World.Parts.Skill.WM_MMA_CombinationStrikesI;

using HistoryKit;

namespace XRL.World.Effects
{

    [Serializable]

    public class Grappled : Effect
    {
        public const int SAVE_TARGET = 20;
        public const string SAVE_STAT = "Agility";
        public const string SAVE_VERSUS = "Immobilization[Grappled]";
        public const int SAVE_DIVISOR = 2;

        public int DVPenalty;
        public int SaveTarget;

        public GameObject Target;


        public GameObject eGrappler;

        public Grappled()
        {
            base.DisplayName = "{{R|grappled}}";
            Duration = 1;
        }

        public Grappled(int Duration)
            : this()
        {
            base.Duration = Duration;
        }

        public Grappled(GameObject Grappler) : this()
        {
            eGrappler = Grappler;
        }

        public override string GetDetails()
        {
            return "Can't move or attack unless you break the grapple.\n";
        }

        public override bool SameAs(Effect e)
        {
            Grappled grappled = e as Grappled;
            if (grappled.DVPenalty != DVPenalty)
            {
                return false;
            }
            if (grappled.SaveTarget != SaveTarget)
            {
                return false;
            }
            return base.SameAs(e);
        }

        public override bool Apply(GameObject Object)
        {
            var effect = Object.GetEffect<Grappled>();
            if (effect != null) return false;
            if (!Object.Statistics.ContainsKey("Energy")) return false;
            if (!Object.CanChangeBodyPosition(To: "Immobilized", Involuntary: true)) return false;


            // AddPlayerMessage("E1: Applying Effect");

            Object.BodyPositionChanged(Involuntary: true);

            // AddPlayerMessage("E2: Assigning Body Position Change");

            DidX("are", DisplayNameStripped, "!", ColorAsBadFor: Object);
            Object.ParticleText(ConsequentialColor(ColorAsBadFor: Object) + "*" + DisplayNameStripped + "*");

            // AddPlayerMessage("E3: Applied Effect");

            return true;
        }

        public override void Remove(GameObject Object)
        {
            Target = Object;
            if (Target.IsPlayer())
                AddPlayerMessage("You escape the grapple!");
            else
                Object.ParticleText(ConsequentialColor(ColorAsBadFor: Object) + "Escaped!");

            base.Remove(Object);
        }

        public override bool WantEvent(int ID, int cascade)
        {
            return base.WantEvent(ID, cascade) || ID == LeaveCellEvent.ID;
        }

        public override bool HandleEvent(LeaveCellEvent E)
        {
            if (E.Type == "Teleporting" || E.Dragging != null || E.Forced) return true;
            if (Object.IsPlayer()) AddPlayerMessage("You are " + DisplayNameStripped + "!");

            Object.UseEnergy(1000);
            return false;
        }

        public override void Register(GameObject Object, IEventRegistrar registrar)
        {
            registrar.Register("BeginTakeAction");
            registrar.Register("BodyPositionChanged");
            registrar.Register("CanStandUp");
            registrar.Register("EndTurn");
            registrar.Register("MovementModeChanged");
            registrar.Register("CanChangeBodyPosition");
            registrar.Register("CanChangeMovementMod");
            registrar.Register("CanMoveExtremities");
            registrar.Register("IsMobile");
            registrar.Register("CommandTakeAction");
            registrar.Register("IsMobile");
            eGrappler.RegisterEffectEvent(this, "LeftCell");
            eGrappler.RegisterEffectEvent(this, "wm-GrappleCommand");


            base.Register(Object, registrar);
        }
        public static bool MakeSave(GameObject attacker, GameObject defender)
        {
            var target = SAVE_TARGET / SAVE_DIVISOR;
            var stat = "Agility";
            if (attacker.GetStatValue("Strength") > attacker.GetStatValue("Agility"))
                stat = "Strength";

            return defender.MakeSave(SAVE_STAT, target, attacker, stat, SAVE_VERSUS);
        }
        public override bool FireEvent(Event E)
        {
            // AddPlayerMessage("E4: Event Found no Grappler or Grappled was Invalid if no messages are present after this.");
            // AddPlayerMessage("P1: Grappler: " + eGrappler);
            // AddPlayerMessage("P2: Grappler Validity: " + eGrappler.IsValid());

            if (!eGrappler.IsValid()) return eGrappler.RemoveEffect(this);
            if (E.ID == "IsMobile")
            {
                // AddPlayerMessage("E5: Has Mobility returned null.");
                return false;
            }
            else if (E.ID == "CommandTakeAction")
            {
                if (MakeSave(eGrappler, Object))
                {
                    // AddPlayerMessage("E6: Stops Here due to save being made.");

                    Object.RemoveEffect(this);
                }

                // AddPlayerMessage("P1: Grappler Validity: " + eGrappler.IsValid());
            }
            else if (E.ID == "LeftCell")
            {
                var mine = Object.CurrentCell;
                var left = E.GetParameter("Cell") as Cell;
                var dir = E.GetStringParameter("Direction");
                var enter = left.GetCellFromDirection(dir);
                if (mine == null || left == null || enter == null) return true;

                // AddPlayerMessage("E7: Left Cell Pass 1.");


                var dist = mine.PathDistanceTo(enter); // BUG? Misreports distance between zones as +1
                if (dist <= 1) return true; // TODO: re-examine this, is disconnecting right move? move until adjacent?
                if (dist > 3) Object.RemoveEffect(this);
                else
                {
                    Object.Move(mine.GetDirectionFromCell(left), Forced: true, Dragging: eGrappler, EnergyCost: 0);
                }
            }

            return true;
        }
    }
}