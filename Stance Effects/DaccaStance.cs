using System;
using XRL.UI;
using XRL.Rules;
using XRL.Core;
using System.Collections.Generic;
using System.Text;
using XRL.World.Parts;
using Battlehub.UIControls;
using Rewired;
using XRL.Language;

namespace XRL.World.Effects
{
    [Serializable]

    public class DaccaStance : Effect
    {
        [NonSerialized]
        private string ForcedMoveDirection;
        public Guid GrappleCommand;
        public Guid TackleCommand;
        public Guid WrestleCommand;
        public Guid HipThrowCommand;
        public Guid SuplexCommand;
        public Guid SubdueCommand;
        public Guid RavageLimbCommand;
        public GameObject Target;
        public GameObject Grappler;

        public int CurrentFreeArmCount;
        public DaccaStance() : base()
        {
            base.DisplayName = "{{green|Way of the Death-Dacca}}";
        }

        public DaccaStance(int Duration) : this()
        {
            base.Duration = 1;
        }

        public override string GetDetails()
        {
            return "A stance about positional control, gain a pleathora of abilities that toss your enemies around.";
        }

        public override void Register(GameObject go)
        {
            go.RegisterEffectEvent((Effect)this, "MovementModeChanged");
            go.RegisterEffectEvent((Effect)this, "CanChangeMovementMode");
            go.RegisterEffectEvent((Effect)this, "EndTurn");
            go.RegisterEffectEvent((Effect)this, "IsMobile");
            go.RegisterEffectEvent((Effect)this, "LeaveCell");
            go.RegisterEffectEvent((Effect)this, "BeginTakeAction");
            go.RegisterEffectEvent((Effect)this, "wm-GrappleCommand");
            go.RegisterEffectEvent((Effect)this, "wm-TackleGrappleCommand");

            base.Register(Object);
        }

        public override void Unregister(GameObject go)
        {
            go.UnregisterEffectEvent((Effect)this, "MovementModeChanged");
            go.UnregisterEffectEvent((Effect)this, "CanChangeMovementMode");
            go.UnregisterEffectEvent((Effect)this, "EndTurn");
            go.UnregisterEffectEvent((Effect)this, "IsMobile");
            go.UnregisterEffectEvent((Effect)this, "LeaveCell");
            go.UnregisterEffectEvent((Effect)this, "BeginTakeAction");
            go.RegisterEffectEvent((Effect)this, "wm-GrappleCommand");
            go.RegisterEffectEvent((Effect)this, "wm-TackleGrappleCommand");
            base.Unregister(Object);
        }

        public void DawnPulse(Cell cell)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    cell.ParticleText("&G" + (char)(219 + Stat.Random(0, 4)), -4.9f, 2);
                }
                for (int k = 0; k < 5; k++)
                {
                    cell.ParticleText("&g" + (char)(219 + Stat.Random(0, 4)), -4.9f, 2);
                }
                for (int l = 0; l < 5; l++)
                {
                    cell.ParticleText("&y" + (char)(219 + Stat.Random(0, 4)), -4.9f, 2);
                }
            }
        }

        public override bool Apply(GameObject Object)
        {
            PlayWorldSound("swapstance", PitchVariance: 0f);
            DawnPulse(Object.CurrentCell);

            int ParentsStrengthModifier = Object.Statistics["Strength"].Modifier;

            var ObjectBody = Object.Body;
            var ObjectsHands = ObjectBody.GetPart("Hand");

            foreach (var p in ObjectsHands)
            {
                if (p.Equipped == null)
                {
                    ++CurrentFreeArmCount;
                }
            }

            int SaveDynamics = ParentsStrengthModifier * CurrentFreeArmCount;

            GrappleCommand = Object.AddActivatedAbility("Grapple", "wm-GrappleCommand", "Way of the Death-Dacca", "Take hold of an opponent, grappled opponents cannot move or make physical attacks and must make a saving-throw each round to escape your grapple. Grappling requires a free-hand and gains a bonus from each free-hand you have above the first. You lose your grip on an opponent if you move outside of their adjacent tiles or become they become somehow intangible. When grappling an enemy and sharing their cell, there is a chance an attack directed at you will be redirected to the other creature instead and you gain +2 to your AV.\n"
            + "\nCurrent Save Towards Grappling: " + "{{cyan|" + SaveDynamics + "}}", Silent: true);

            TackleCommand = Object.AddActivatedAbility("Tackle", "wmTackleCommand", "Way of the Death-Dacca", "Charge and grapple an enemy.", Silent: true);

            //__________________________________________________________________________________

            WrestleCommand = Object.AddActivatedAbility("Wrestle", "wm-WrestleCommand", "Way of the Death-Dacca", "Use your strength to reposition a grappled enemy to an adjacent tile around you, if you force the enemy towards a wall or immovable structure, you will slam the enemy into the object instead, dealing scaled damage. If you force the enemy into a tile where another creature is occupying, the creature must make a saving-throw or stumble into an adjacent tile around them, they both also take a small amount of damage. ", Silent: true);

            HipThrowCommand = Object.AddActivatedAbility("Hip-Throw", "wm-HipThrowCommand", "Way of the Death-Dacca", "Throw an opponent you are grappling with intense force into a direction around you.", Silent: true);

            SuplexCommand = Object.AddActivatedAbility("Suplex", "wm-SuplexCommand", "Way of the Death-Dacca", "While grapplng an enemy, you can perform a powerful takedown, the enemy is slammed into the tile opposite thier position to you, dealing immense damage with a chance to stun and even decapitate the enemy. [Cost 10x Combo-Meter.]", Silent: true);

            SubdueCommand = Object.AddActivatedAbility("Subdue", "wm-SubdueCommand", "Way of the Death-Dacca", "Grappled enemy must pass a toughness saving-throw or be knocked unconscious.", Silent: true);

            RavageLimbCommand = Object.AddActivatedAbility("Ravage Limb", "wm-RavageLimbCommand", "Way of the Death-Dacca", "Enemy must pass a strength-saving throw vs your own, or have a limb be permanently damaged. [Cost 10x Combo-Meter.]", Silent: true);

            StatShifter.SetStatShift("AV", 2);
            return true;
        }
        public bool IsGrappling()
        {
            var Adjacencies = Object.CurrentCell.GetAdjacentCells();

            foreach (var c in Adjacencies)
            {
                if (c.HasObjectWithEffect("Grappled"))
                {

                    return true;
                }
            }

            return false;
        }
        public bool ShouldEnable()
        {
            return XRL.UI.Options.GetOption("WingedGrapplingOptions") != "Yes" || IsGrappling();
        }

        public void CheckEnabled()
        {

            ActivatedAbilities ObjectsAbilityList = Object.GetPart<ActivatedAbilities>();
            ActivatedAbilityEntry Wrestle = ObjectsAbilityList.GetAbilityByCommand("wm-WrestleCommand");
            ActivatedAbilityEntry HipThrow = ObjectsAbilityList.GetAbilityByCommand("wm-HipThrowCommand");
            ActivatedAbilityEntry Suplex = ObjectsAbilityList.GetAbilityByCommand("wm-SuplexCommand");
            ActivatedAbilityEntry Subdue = ObjectsAbilityList.GetAbilityByCommand("wm-SubdueCommand");
            ActivatedAbilityEntry RavageLimb = ObjectsAbilityList.GetAbilityByCommand("wm-RavageLimbCommand");

            if (WrestleCommand != null)
            {
                Wrestle.Enabled = ShouldEnable();
            }
            if (HipThrowCommand != null)
            {
                HipThrow.Enabled = ShouldEnable();
            }
            if (SuplexCommand != null)
            {
                Suplex.Enabled = ShouldEnable();
            }
            if (SubdueCommand != null)
            {
                Subdue.Enabled = ShouldEnable();
            }
            if (RavageLimbCommand != null)
            {
                RavageLimb.Enabled = ShouldEnable();
            }
        }

        public override bool FireEvent(Event E)
        {
            if (E.ID == "EndTurn")
            {
                // AddPlayerMessage("Current Free-Arm Counter: " + CurrentFreeArmCount);
            }
            else if (E.ID == "wm-GrappleCommand")
            {
                // AddPlayerMessage("1: Beginning Grapple Command.");
                var TargetCell = PickDestinationCell(1, AllowVis.OnlyVisible);

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

                    // AddPlayerMessage("Target: " + Target.DisplayName);
                }

                // AddPlayerMessage("4: Checking target Viability.");

                if (Target != null)
                {
                    int GrapplersStrengthMod = Object.Statistics["Strength"].Modifier;
                    int GrapplersLevelMod = Object.Statistics["Level"].Value;
                    int SaveTarget = ((byte)GrapplersStrengthMod) + (8 + GrapplersLevelMod / 2);

                    // AddPlayerMessage("5: Setting Variables.");

                    if (!Target.MakeSave("Strength", SaveTarget, Object, null, "Strength"))
                    {
                        // AddPlayerMessage("6: Creature failed Save.");

                        Target.ApplyEffect(new Grappled(Grappler: Object));
                        XDidYToZ(what: Object, verb: "grapple", obj: Target, terminalPunctuation: "!");

                        // AddPlayerMessage("7: Effect Applied");
                    }
                    else
                    {
                        XDidYToZ(what: Object, verb: "fail to grapple", obj: Target, terminalPunctuation: "!");
                    }
                }


            }
            if (E.ID == "wm-TackleGrappleCommand")
            {

                var TackledCell = E.GetParameter<Cell>("Cell");



            }

            return base.FireEvent(E);
        }

        public override bool Render(RenderEvent E)
        {
            if (this.Duration > 0)
            {
                int num = XRLCore.CurrentFrame % 60;
                if (num > 25 && num < 35)
                {
                    E.ColorString = "&g";
                }
            }
            return base.Render(E);
        }

        public override void Remove(GameObject Object)
        {
            RemoveMyActivatedAbility(ref this.GrappleCommand);
            RemoveMyActivatedAbility(ref this.TackleCommand);
            RemoveMyActivatedAbility(ref this.HipThrowCommand);
            RemoveMyActivatedAbility(ref this.SuplexCommand);
            RemoveMyActivatedAbility(ref this.SubdueCommand);
            RemoveMyActivatedAbility(ref this.RavageLimbCommand);
            RemoveMyActivatedAbility(ref this.WrestleCommand);
            StatShifter.RemoveStatShifts();
        }

        // Grapple Action Commands

        private bool ValidTackleTarget(GameObject obj)
        {
            if (obj != null && obj.HasPart("Combat"))
            {
                return obj.FlightMatches(Object);
            }
            return false;
        }
        public int GetTackleMinimumRange()
        {
            return 2;
        }

        public int GetTackleMaximumRange()
        {
            return 3 + Object.Level / 4;
        }

        public bool Grappler_Tackle()
        {
            if (Object.OnWorldMap())
            {
                if (Object.IsPlayer())
                {
                    Popup.ShowFail("You cannot perform tackles on the world map.");
                }
                return false;
            }
            if (Object.IsFlying)
            {
                if (Object.IsPlayer())
                {
                    Popup.ShowFail("You cannot tackle while flying.");
                }
                return false;
            }
            if (Object.IsOverburdened())
            {
                if (Object.IsPlayer())
                {
                    Popup.ShowFail("You cannot tackle while overburdened.");
                }
                return false;
            }
            if (!Object.CanChangeBodyPosition("Tackling", ShowMessage: true))
            {
                return false;
            }
            if (!Object.CanChangeMovementMode("Tackling", ShowMessage: true))
            {
                return false;
            }
            int minimumRange = GetTackleMinimumRange();
            int maximumRange = GetTackleMaximumRange();
            List<Cell> list = PickLine(maximumRange + 1, AllowVis.OnlyVisible, ValidTackleTarget, IgnoreSolid: false, IgnoreLOS: true, RequireCombat: true, Snap: true);
            if (list == null || list.Count <= 0)
            {
                return false;
            }
            if (Object.IsPlayer())
            {
                list.RemoveAt(0);
            }
            int num = list.Count - 1;
            if (num < minimumRange)
            {
                if (IsPlayer())
                {
                    Popup.ShowFail("You must perform a tackle with at least " + Grammar.Cardinal(minimumRange) + " " + ((minimumRange == 1) ? "space" : "spaces") + ".");
                }
                return false;
            }
            if (num > maximumRange)
            {
                if (IsPlayer())
                {
                    Popup.ShowFail("You can't perform a tackle more than " + Grammar.Cardinal(maximumRange) + " " + ((maximumRange == 1) ? "space" : "spaces") + ".");
                }
                return false;
            }
            if (Object.AreViableHostilesAdjacent())
            {
                if (IsPlayer())
                {
                    Popup.ShowFail("You cannot tackle while in melee combat.");
                }
                return false;
            }
            GameObject gameObject = null;
            Cell cell = list[list.Count - 1];
            gameObject = ((!Object.IsPlayer()) ? Object.Target : cell.GetCombatTarget(Object, IgnoreFlight: false, IgnorePhase: true));
            if (gameObject == null)
            {
                if (IsPlayer())
                {
                    if (cell.GetCombatTarget(Object, IgnoreFlight: true, IgnorePhase: true) != null)
                    {
                        Popup.ShowFail("You cannot tackle a flying target.");
                    }
                    else
                    {
                        Popup.ShowFail("You must tackle a target!");
                    }
                }
                return false;
            }
            string text = null;
            string text2 = null;
            string colorString = null;
            string detailColor = null;
            int num2 = 10;
            Disguised disguised = Object.GetEffect("Disguised") as Disguised;
            if (disguised != null)
            {
                if (!string.IsNullOrEmpty(disguised.Tile) && Options.UseTiles)
                {
                    text2 = disguised.Tile;
                    colorString = (string.IsNullOrEmpty(disguised.TileColor) ? disguised.ColorString : disguised.TileColor);
                    detailColor = disguised.DetailColor;
                }
                else
                {
                    text = disguised.ColorString + disguised.RenderString;
                }
            }
            else if (!string.IsNullOrEmpty(Object.pRender.Tile) && Options.UseTiles)
            {
                text2 = Object.pRender.Tile;
                colorString = (string.IsNullOrEmpty(Object.pRender.TileColor) ? Object.pRender.ColorString : Object.pRender.TileColor);
                detailColor = Object.pRender.DetailColor;
            }
            else
            {
                text = Object.pRender.ColorString + Object.pRender.RenderString;
            }
            if (Visible())
            {
                if (text2 != null)
                {
                    Object.TileParticleBlip(text2, colorString, detailColor, num2, IgnoreVisibility: false, HFlip: Object.pRender.HFlip, VFlip: Object.pRender.VFlip);
                }
                else
                {
                    Object.ParticleBlip(text, num2);
                }
            }
            bool flag = false;
            bool flag2 = false;
            bool flag3 = false;
            bool flag4 = false;
            bool flag5 = false;
            Cell cell2 = Object.CurrentCell;
            string item = null;
            List<string> list2 = new List<string>(maximumRange + 2);
            int i = 0;
            for (int num3 = maximumRange + 2; i < num3; i++)
            {
                if (i >= list.Count)
                {
                    list2.Add(item);
                    continue;
                }
                Cell cell3 = list[i];
                string directionFromCell = cell2.GetDirectionFromCell(cell3);
                list2.Add(directionFromCell);
                item = directionFromCell;
                cell2 = cell3;
            }
            int j = 0;
            int count = list2.Count;
            while (true)
            {
                if (j < count)
                {
                    string text3 = list2[j];
                    Cell cellFromDirection = Object.CurrentCell.GetCellFromDirection(text3, BuiltOnly: false);
                    if (cellFromDirection != null)
                    {
                        bool flag6 = cellFromDirection.Objects.Contains(gameObject);
                        GameObject combatTarget = cellFromDirection.GetCombatTarget(Object, IgnoreFlight: false, IgnorePhase: false, IgnoreAttackable: false, AllowInanimate: true, InanimateSolidOnly: true);
                        if (combatTarget != null)
                        {
                            DidXToY("tackle", combatTarget, null, "!", null, null, combatTarget.IsPlayer() ? combatTarget : null);
                            if (Object.DistanceTo(cellFromDirection) <= 1)
                            {
                                // Object.FireEvent(Event.New("wm-TackleGrappleCommand", "Cell", cellFromDirection, "Properties", "Charging"));
                            }
                            else
                            {
                                Object.UseEnergy(1000, "Tackling");
                            }
                            Object.FireEvent(Event.New("wm-TackleGrappleCommand", "Defender", combatTarget));
                            combatTarget.FireEvent(Event.New("WasCharged", "Attacker", Object));
                            break;
                        }
                        if (flag6)
                        {
                            flag3 = true;
                        }
                        else if (flag3)
                        {
                            flag4 = true;
                            flag3 = false;
                        }
                        if (Object.DistanceTo(gameObject) == 1)
                        {
                            flag = true;
                        }
                        else if (flag)
                        {
                            flag2 = true;
                        }
                        if (j >= maximumRange)
                        {
                            flag5 = true;
                        }
                        ForcedMoveDirection = null;
                        if (Object.Move(text3, Forced: false, System: false, IgnoreGravity: false, NoStack: false, NearestAvailable: false, Type: "Tackle"))
                        {
                            if (ForcedMoveDirection != null)
                            {
                                if (ForcedMoveDirection == "U" || ForcedMoveDirection == "D" || ForcedMoveDirection == "?")
                                {
                                    goto IL_06d7;
                                }
                                if (ForcedMoveDirection != text3)
                                {
                                    int index = j + 1;
                                    for (; j < count; j++)
                                    {
                                        list2[index] = ForcedMoveDirection;
                                    }
                                }
                            }
                            num2 += 5;
                            if (Visible())
                            {
                                if (text2 != null)
                                {
                                    Object.TileParticleBlip(text2, colorString, detailColor, num2, IgnoreVisibility: false, HFlip: Object.pRender.HFlip, VFlip: Object.pRender.VFlip);
                                }
                                else
                                {
                                    Object.ParticleBlip(text, num2);
                                }
                            }
                            j++;
                            continue;
                        }
                    }
                }
                goto IL_06d7;
            IL_06d7:
                ForcedMoveDirection = null;
                if (flag4)
                {
                    DidXToY("charge", "right through", gameObject, null, "!", null, null, Object);
                }
                else if (flag2)
                {
                    DidXToY("charge", "right past", gameObject, null, "!", null, null, Object);
                }
                else if (flag3 || flag || flag5)
                {
                    DidXToY("charge", gameObject, ", but" + Object.GetVerb("fail") + " to make contact", "!", null, null, Object);
                }
                else
                {
                    DidX("charge", ", but" + Object.Is + " brought up short", "!", null, null, Object);
                }
                if (flag5)
                {
                    Object.ApplyEffect(new Dazed(1));
                }
                Object.UseEnergy(1000, "Charging");
                break;
            }
            CooldownMyActivatedAbility(TackleCommand, 20);
            return true;
        }

    }
}