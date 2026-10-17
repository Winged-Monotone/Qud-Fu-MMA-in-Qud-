using System;
using XRL.UI;
using XRL.Rules;
using XRL.Core;
using System.Collections.Generic;
using System.Linq;
using XRL.World.Parts;
using XRL.Language;
using ConsoleLib.Console;
using XRL.World.Parts.Skill;

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
        public Guid TestGrapProbCommand;
        public GameObject Target;
        public GameObject Grappler;
        public int Passes;

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
            return "A stance about positional control, gain a plethora of abilities that toss your enemies around.";
        }

        public override void Register(GameObject go, IEventRegistrar registrar)
        {
            registrar.Register("MovementModeChanged");
            registrar.Register("CanChangeMovementMode");
            registrar.Register("EndTurn");
            registrar.Register("IsMobile");
            registrar.Register("LeaveCell");
            registrar.Register("BeginTakeAction");
            registrar.Register("wm-GrappleCommand");
            registrar.Register("wm-TackleGrappleCommand");
            registrar.Register("wm-WrestleCommand");
            registrar.Register("wm-HipThrowCommand");
            registrar.Register("wm-SuplexCommand");
            registrar.Register("wm-SubdueCommand");
            registrar.Register("wm-RavageLimbCommand");
            // registrar.Register("wm-TestGrappleProbability");

            base.Register(Object, registrar);
        }

        public void DawnPulse(Cell cell)
        {
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    cell.ParticleText("&G" + (char)(219 + Stat.Random(0, 4)), -4.9f, 2);
                }
                for (int k = 0; k < 2; k++)
                {
                    cell.ParticleText("&g" + (char)(219 + Stat.Random(0, 4)), -4.9f, 2);
                }
                for (int l = 0; l < 2; l++)
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

            // TackleCommand = Object.AddActivatedAbility("Tackle", "wmTackleCommand", "Way of the Death-Dacca", "Charge and grapple an enemy.", Silent: true);

            //__________________________________________________________________________________

            WrestleCommand = Object.AddActivatedAbility("Wrestle", "wm-WrestleCommand", "Way of the Death-Dacca", "Use your strength to reposition a grappled enemy to an adjacent tile around you, if you force the enemy towards a wall or immovable structure, you will slam the enemy into the object instead, dealing scaled damage. If you force the enemy into a tile where another creature is occupying, the creature must make a saving-throw or stumble into an adjacent tile around them, they both also take a small amount of damage. ", Silent: true);
            // Add Cooldown.

            HipThrowCommand = Object.AddActivatedAbility("Hip-Throw", "wm-HipThrowCommand", "Way of the Death-Dacca", "Throw an opponent you are grappling with intense force into a direction around you, deals heavy damage if the enemy hits a wall deals light damage upon a successful throw.", Silent: true);
            // Add Cooldown.

            SuplexCommand = Object.AddActivatedAbility("Suplex", "wm-SuplexCommand", "Way of the Death-Dacca", "While grappling an enemy, you can perform a powerful takedown, the enemy is slammed into the tile opposite thier position to you, dealing immense damage with a chance to stun and even decapitate the enemy.", Silent: true);
            // Add Cooldown.

            SubdueCommand = Object.AddActivatedAbility("Subdue", "wm-SubdueCommand", "Way of the Death-Dacca", "Grappled enemy must pass a toughness saving-throw or be knocked unconscious.", Silent: true);
            // Add Cooldown.

            RavageLimbCommand = Object.AddActivatedAbility("Ravage Limb", "wm-RavageLimbCommand", "Way of the Death-Dacca", "Enemy must pass a strength-saving throw vs your own, or have a limb be permanently damaged.", Silent: true);
            // Add Cooldown.

            // TestGrapProbCommand = Object.AddActivatedAbility("TestGrappleChance", "wm-TestGrappleProbability", "Way of the Death-Dacca", "Test grapple chances.", Silent: true);

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

        // public void GrappleTestModifier()
        // {
        //     var Grappler = Object;
        //     var GrapplersStrengthMod = (Grappler.Statistics["Strength"].Modifier);
        //     var GrapplerLevel = (Grappler.Statistics["Level"].Value);

        //     Target = PickDirection().GetCombatTarget();

        //     var TargetsStrengthMod = Target.Statistics["Strength"].Modifier;

        //     bool SaveDynamics = !Target.MakeSave(Stat: "Strength", Difficulty: 8 + (GrapplersStrengthMod + (GrapplerLevel / 4)));


        //     for (int i = 1; i <= 10; i += 1)
        //     {
        //         if (SaveDynamics)
        //         {
        //             Passes = +1;
        //         }
        //         if (i == 10)
        //         {
        //             AddPlayerMessage("You failed this save " + Passes + "/" + 10);
        //         }
        //     }

        //     Passes = 0;
        // }

        // public bool ShouldEnable()
        // {
        //     return XRL.UI.Options.GetOption("WingedGrapplingOptions") != "Yes" || IsGrappling();
        // }

        // public void CheckEnabled()
        // {

        //     ActivatedAbilities ObjectsAbilityList = Object.GetPart<ActivatedAbilities>();
        //     ActivatedAbilityEntry Wrestle = ObjectsAbilityList.GetAbilityByCommand("wm-WrestleCommand");
        //     ActivatedAbilityEntry HipThrow = ObjectsAbilityList.GetAbilityByCommand("wm-HipThrowCommand");
        //     ActivatedAbilityEntry Suplex = ObjectsAbilityList.GetAbilityByCommand("wm-SuplexCommand");
        //     ActivatedAbilityEntry Subdue = ObjectsAbilityList.GetAbilityByCommand("wm-SubdueCommand");
        //     ActivatedAbilityEntry RavageLimb = ObjectsAbilityList.GetAbilityByCommand("wm-RavageLimbCommand");

        //     if (WrestleCommand != null)
        //     {
        //         Wrestle.Enabled = ShouldEnable();
        //     }
        //     if (HipThrowCommand != null)
        //     {
        //         HipThrow.Enabled = ShouldEnable();
        //     }
        //     if (SuplexCommand != null)
        //     {
        //         Suplex.Enabled = ShouldEnable();
        //     }
        //     if (SubdueCommand != null)
        //     {
        //         Subdue.Enabled = ShouldEnable();
        //     }
        //     if (RavageLimbCommand != null)
        //     {
        //         RavageLimb.Enabled = ShouldEnable();
        //     }
        // }

        public override bool FireEvent(Event E)
        {
            if (E.ID == "EndTurn")
            {
                // AddPlayerMessage("Current Free-Arm Counter: " + CurrentFreeArmCount);
            }
            else if (E.ID == "wm-GrappleCommand")
            {
                wmGrapple();
                CooldownMyActivatedAbility(GrappleCommand, 17);

            }
            else if (E.ID == "wm-TackleGrappleCommand")
            {
                // CooldownMyActivatedAbility(TackleCommand, 25);
            }
            else if (E.ID == "wm-WrestleCommand")
            {
                wmWrestleCommand();
                CooldownMyActivatedAbility(WrestleCommand, 15);
                Object.UseEnergy(10);
            }
            else if (E.ID == "wm-HipThrowCommand")
            {
                wmHipThrow();
                CooldownMyActivatedAbility(HipThrowCommand, 15);
                Object.UseEnergy(10);
            }
            else if (E.ID == "wm-SuplexCommand")
            {
                wmSuplex();
                CooldownMyActivatedAbility(SuplexCommand, 25);
                Object.UseEnergy(50);
            }
            else if (E.ID == "wm-SubdueCommand")
            {
                wmSubdue();
                CooldownMyActivatedAbility(SubdueCommand, 12);
                Object.UseEnergy(20);
            }
            else if (E.ID == "wm-RavageLimbCommand")
            {
                wmRavage();
                CooldownMyActivatedAbility(RavageLimbCommand, 50);
                Object.UseEnergy(50);
            }
            // else if (E.ID == "wm-TestGrappleProbability")
            // {
            //     GrappleTestModifier();
            // }
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
            // RemoveMyActivatedAbility(ref this.TackleCommand);
            RemoveMyActivatedAbility(ref this.HipThrowCommand);
            RemoveMyActivatedAbility(ref this.SuplexCommand);
            RemoveMyActivatedAbility(ref this.SubdueCommand);
            RemoveMyActivatedAbility(ref this.RavageLimbCommand);
            RemoveMyActivatedAbility(ref this.WrestleCommand);
            StatShifter.RemoveStatShifts();
        }

        public void wmGrapple()
        {

            Grappler = Object;
            var GrapplersStrengthMod = (Grappler.Statistics["Strength"].Modifier);
            var GrapplerLevel = (Grappler.Statistics["Level"].Value);

            Target = PickDirection().GetCombatTarget();

            bool GrappledCheck = !Target.HasEffect<Grappled>();
            bool SaveDynamics = !Target.MakeSave(Stat: "Strength", Difficulty: 12 + (GrapplersStrengthMod + (GrapplerLevel / 4)));

            if (Target == null || !Target.PhaseAndFlightMatches(Object))
            {

                if (Object.IsPlayer())
                    AddPlayerMessage("There is nothing here to grapple.");
            }

            if (Target != null)
            {
                if (SaveDynamics && GrappledCheck)
                {

                    Target.ApplyEffect(new Grappled(Grappler: Object));
                    Target.GetAngryAt(Grappler);
                    XDidYToZ(Actor: Object, Verb: "grapple", Object: Target, EndMark: "!");

                }
                else if (GrappledCheck && SaveDynamics)
                {
                    XDidYToZ(Actor: Object, Verb: "fail to grapple", Object: Target, EndMark: "!");
                }
                else if (GrappledCheck)
                {
                    Target.RemoveEffect(new Grappled());
                    XDidYToZ(Actor: Object, Verb: "release", Object: Target, EndMark: "!");

                }
            }
        }

        //___________________________
        //_____________________________________________________
        //_____________________________________________________________________________
        //__________________________________________________________________________________________________
        //___________________________________________________________________________________________________________________________
        //______________________________________________________________________________________________________________________________________________
        //_____________________________________________________________________________________________________________________________________________________
        // __ Wrestle Method ___ 

        public void wmWrestleCommand()
        {
            var Grappler = Object;
            var GrapplersStrengthMod = (Grappler.Statistics["Strength"].Modifier);
            var GrapplerLevel = (Grappler.Statistics["Level"].Value);
            
            int DamageIntBodySmash = (GrapplersStrengthMod + GrapplerLevel) / 2;
            
            var FindTarget = Object.CurrentCell.GetAdjacentCells();
            FindGrappleTarget(FindTarget);
            
            bool SaveDynamics = !Target.MakeSave(Stat: "Strength", Difficulty: 12 + (GrapplersStrengthMod + (GrapplerLevel / 4)));
            
            if (Target == null)
            {
                AddPlayerMessage("You must select a grappled target.");
                return;
            }
            if (Target != null)
            {
                if (!Target.PhaseAndFlightMatches(Grappler))
                {
                    AddPlayerMessage("You cannot reach this creature!");
                    return;
                }
                if (SaveDynamics)
                {
                    var TargetDestination = PickDirectionS();
                    var CellDirection = Object.CurrentCell.GetCellFromDirection(TargetDestination);
                    var AimingAt = CellDirection.GetCombatTarget(Attacker: Object, InanimateSolidOnly: true, IgnoreFlight: true);
                    
                    if (AimingAt.IsValid())
                    {
                        
                        var AimingAtCurrentCell = AimingAt.CurrentCell;
                        
                        if (AimingAt.CurrentCell == Target.CurrentCell)
                        {
                            AddPlayerMessage("You must choose a different space than your target's current.");
                            return;
                        }
                        
                        AimingAt.ShatterSplatter();
                        if (AimingAt.Physics.Weight <= Target.Physics.Weight)
                        {
                            AimingAt.Move(
                                Direction: Directions.GetRandomDirection(),
                                Forced: true,
                                NearestAvailable: false,
                                EnergyCost: 1000);
                        }
                        AimingAt.TakeDamage(Amount: DamageIntBodySmash, Attributes: "Bashing", DeathReason: "colliding into another with vicious force.", ThirdPersonDeathReason: "You were splattered against another through wrestling.", Owner: Grappler, Attacker: Grappler, Source: null, Message: "from %t wrestling maneuver.");

                       if (AimingAt.IsValid())
                       {
                           GrappleJuice(AimingAt, DamageIntBodySmash, GrapplersStrengthMod);
                       }
                       else
                       {
                           GetRandomAdjacentVisibleCellAndTeleportObject(AimingAtCurrentCell);
                           Target.TakeDamage(Amount: DamageIntBodySmash, Attributes: "Bashing",
                               DeathReason: "colliding into another with vicious force.",
                               ThirdPersonDeathReason: "You were splattered against another through wrestling.",
                               Owner: Grappler, Attacker: Grappler, Source: null,
                               Message: "from %t wrestling maneuver.");
                       }

                        CombatJuice.cameraShake(0.5f);
                    }
                    else
                    {
                        XDidYToZ(Grappler, "wrestle", "", Target, "into another space", ".");
                        Target.CellTeleport(C: CellDirection, EnergyCost: 50, Forced: true, VisualEffects: false, SkipRealityDistortion: true);
                    }
                }
                else
                {
                    AddPlayerMessage(Target.the + " resist your attempt to grapple.");
                }
            }
        }

        //___________________________
        //_____________________________________________________
        //_____________________________________________________________________________
        //__________________________________________________________________________________________________
        //___________________________________________________________________________________________________________________________
        //______________________________________________________________________________________________________________________________________________
        //_____________________________________________________________________________________________________________________________________________________
        // __ HipThrow Method ___ 

        public void wmHipThrow()
        {
            var Grappler = Object;
            var GrapplersStrengthMod = (Grappler.Statistics["Strength"].Modifier);
            var GrapplerLevel = (Grappler.Statistics["Level"].Value);

            int DamageIntWallSmash = GrapplersStrengthMod + GrapplerLevel;
            int DamageIntBodySmash = (GrapplersStrengthMod + GrapplerLevel) / 2;

            var FindTarget = Object.CurrentCell.GetAdjacentCells();
            FindGrappleTarget(FindTarget);
            
            if (Target != null)
            {
                bool GrappledCheck = Target.RemoveEffect<Grappled>();
                bool SaveDynamics = !Target.MakeSave(Stat: "Strength",
                    Difficulty: 8 + (GrapplersStrengthMod + (GrapplerLevel / 4)));

                if (!GrappledCheck)
                {
                    AddPlayerMessage("You must select a grappled target.");
                }
                else if (!Target.PhaseAndFlightMatches(Grappler))
                {
                    AddPlayerMessage("You cannot reach this creature!");
                }
                else if (SaveDynamics)
                {

                    var TargetLine = PickLine(
                        Length: 3 + (GrapplerLevel / 3),
                        VisLevel: AllowVis.OnlyExplored);
                        
                    // Count is cumulative number of elements in the array. 
                    //  The last index of an array is always the amount of elements minus 1. Because indices start at 0
                    for (int index = 1, Count = TargetLine.Count; index < Count; index++)
                    {

                        Cell cell = TargetLine[index];
                        
                        var BlipTile = Target.Render.Tile;
                        var BlipColorString = Target.Render.TileColor;
                        var BlipDetailColor = Target.Render.DetailColor;
                        int BlipDuration = 2 * index;
                        
                        cell.TileParticleBlip(BlipTile, BlipColorString, BlipDetailColor, BlipDuration, false,
                            Target.Render.HFlip, Target.Render.VFlip);
                        
                        var AimingAt = cell.GetCombatTarget(Attacker: Object, InanimateSolidOnly: true, IgnoreFlight: true);
                            
                        if (AimingAt.IsValid())
                        {
                            GetRandomAdjacentVisibleCellAndTeleportObject(AimingAt.CurrentCell);
                            Target.Splatter("*");
                            Target.TakeDamage(Amount: AimingAt.IsWall()?DamageIntWallSmash:DamageIntBodySmash, Attributes: "Bashing",
                                DeathReason: "finely mashed against something.",
                                ThirdPersonDeathReason: "You were finely mashed against something.", Owner: Grappler,
                                Attacker: Grappler, Source: null, Message: "from %t wrestling maneuver.");
                            if (!Target.MakeSave(Stat: "Toughness", Difficulty: 8 + GrapplersStrengthMod, Attacker: Object, AttackerStat: "Strength"))
                            {
                                Target.ApplyEffect(new Dazed(Duration: 8));
                            }
                            CombatJuice.cameraShake(0.5f);
                            break;
                        }
                        else if (index == Count - 1)
                        {
                            
                            GetRandomAdjacentVisibleCellAndTeleportObject(cell);
                            Target.Splatter("*");
                            if (!Target.MakeSave(Stat: "Toughness", Difficulty: 8 + GrapplersStrengthMod, Attacker: Object, AttackerStat: "Strength"))
                            {
                                Target.ApplyEffect(new Dazed(Duration: 8));
                            }
                            
                            Target.Splatter("*");
                            CombatJuice.cameraShake(0.1f);
                        }
                    }
                }
                else if (!SaveDynamics)
                {
                    AddPlayerMessage(Target.DisplayNameOnlyDirect + " resist your attempt to hip-throw them.");
                }
                
            }
        }
        
        //___________________________
        //_____________________________________________________
        //_____________________________________________________________________________
        //__________________________________________________________________________________________________
        //___________________________________________________________________________________________________________________________
        //______________________________________________________________________________________________________________________________________________
        //_____________________________________________________________________________________________________________________________________________________
        // __ Suplex Method ___ 

        public void wmSuplex()
        {
            
            var Grappler = Object;
            var GrapplersStrengthMod = (Grappler.Statistics["Strength"].Modifier);
            var GrapplerLevel = (Grappler.Statistics["Level"].Value);

            int DamageIntSuplexSmash = ((1 + (GrapplerLevel / 2)) * (GrapplersStrengthMod + Stat.Random(1, 3)));
            var FindTarget = Object.CurrentCell.GetAdjacentCells();

            FindGrappleTarget(FindTarget);
            
            if (Target != null)
            {
                var TargetDir = Grappler.GetDirectionToward(Target);
                var OppositeSpace = Directions.GetOppositeDirection(TargetDir);
                var TargetCell = Object.CurrentCell.GetCellFromDirection(OppositeSpace);
                
                bool SaveDynamics = !Target.MakeSave(Stat: "Strength",
                    Difficulty: 8 + (GrapplersStrengthMod + (GrapplerLevel / 4)));

                if (Target.HasEffect<Grappled>())
                {
                    if (!Target.PhaseAndFlightMatches(Grappler))
                    {
                        AddPlayerMessage("You cannot reach this creature!");
                    }

                    if (SaveDynamics)
                    {
                        Acrobatics_Jump.PlayAnimation(Target, TargetCell);
                        if (!TargetCell.IsPassable())
                        {
                            GetRandomAdjacentVisibleCellAndTeleportObject(TargetCell);
                            TargetCell.ImplosionSplat();
                        }
                        else
                        {
                            Target.CellTeleport(C: TargetCell, EnergyCost: 10, Forced: true, VisualEffects: false);
                            TargetCell.ImplosionSplat();
                        }
                        Target.TakeDamage(
                            Amount: DamageIntSuplexSmash,
                            Attributes: "Bashing",
                            DeathReason: "crushed by a suplex.",
                            ThirdPersonDeathReason: "You were suplexed, you died.",
                            Owner: Grappler,
                            Attacker: Grappler,
                            Source: null,
                            Message: "from %t suplex maneuver.");
                        
                        CombatJuice.cameraShake(0.5f);

                        XDidYToZ(Grappler, "suplex", "", Target, "", "!");
                    }
                    else if (!SaveDynamics)
                    {
                        AddPlayerMessage(Target.DisplayNameOnlyDirect + " resist your attempt to suplex them.");
                    }
                }
            }
        }

        //___________________________
        //_____________________________________________________
        //_____________________________________________________________________________
        //__________________________________________________________________________________________________
        //___________________________________________________________________________________________________________________________
        //______________________________________________________________________________________________________________________________________________
        //_____________________________________________________________________________________________________________________________________________________
        // __ Subdue Method ___ 

        public void wmSubdue()
        {
            var Grappler = Object;
            var GrapplersStrengthMod = (Grappler.Statistics["Strength"].Modifier);
            var GrapplerLevel = (Grappler.Statistics["Level"].Value);

            var FindTarget = Object.CurrentCell.GetAdjacentCells();
            FindGrappleTarget(FindTarget);

            var TargetCon = Target.Statistics["Toughness"].Modifier;

            bool GrappledCheck = Target.HasEffect<Grappled>();

            bool SaveDynamics = !Target.MakeSave(Stat: "Strength", Difficulty: 8 + (GrapplersStrengthMod + (GrapplerLevel / 4)));

            if (Target == null)
            {
                AddPlayerMessage("You must select a grappled target.");
            }
            else if (GrappledCheck)
            {
                if (SaveDynamics)
                {
                    Target.ApplyEffect(new Incapacitated(360 - (TargetCon * 10), 24));
                }
            }
            else if (!SaveDynamics)
            {
                AddPlayerMessage(Target.DisplayNameOnlyDirect + " resist your attempt to subdue.");
            }
        }

        //___________________________
        //_____________________________________________________
        //_____________________________________________________________________________
        //__________________________________________________________________________________________________
        //___________________________________________________________________________________________________________________________
        //______________________________________________________________________________________________________________________________________________
        //_____________________________________________________________________________________________________________________________________________________
        // __ Ravage Method ___ 

        public void wmRavage()
        {
            var Grappler = Object;
            var GrapplersStrengthMod = (Grappler.Statistics["Strength"].Modifier);
            var GrapplerLevel = (Grappler.Statistics["Level"].Value);

            var FindTarget = Object.CurrentCell.GetAdjacentCells();
            FindGrappleTarget(FindTarget);

            int DamageIntRavage = (10 + GrapplerLevel / 3) * GrapplersStrengthMod;

            bool GrappledCheck = Target.HasEffect<Grappled>();
            bool SaveDynamics = !Target.MakeSave(Stat: "Strength", Difficulty: 8 + (GrapplersStrengthMod + (GrapplerLevel / 4)));

            if (Target == null)
            {
                AddPlayerMessage("You must select a grappled target.");
            }
            else if (GrappledCheck)
            {
                if (SaveDynamics)
                {
                    var TargetBodypart = Target.GetRandomConcreteBodyPart();
                    
                    if (TargetBodypart.Type == "Arm" && TargetBodypart.Type == "Hand" && TargetBodypart.Type == "Tail" && TargetBodypart.Type == "Wheel" && TargetBodypart.Type == "Foot" && TargetBodypart.Type == "Fin" && TargetBodypart.Type == "Tread" && TargetBodypart.Type == "Roots")
                    {
                        TargetBodypart.Dismember();
                        XDidYToZ(Actor: Grappler, Verb: "destroy", Object: Target, Extra: TargetBodypart.Name,
                            EndMark: "!");
                        Target.TakeDamage(
                            Amount: DamageIntRavage,
                            Attributes: "Bashing",
                            DeathReason: "torn asunder by a ravaging blow.",
                            ThirdPersonDeathReason: "You were torn asunder.",
                            Owner: Grappler,
                            Attacker: Grappler,
                            Source: null,
                            Message: "from %t ravaging blow.");
                    }
                }
                else if (!SaveDynamics)
                {
                    AddPlayerMessage(Target.DisplayNameOnlyDirect + " resist your attempt to destroy their limb.");
                }
            }
        }

        public override bool HandleEvent(AttackerDealtDamageEvent E)
        {
            var eAttacker = E.Source;
            var eDefender = E.Object;

            var eWeapon = E.Weapon;

            if (eAttacker.HasPart<WM_MMA_MartialStances>() && eWeapon.HasPart<MartialConditioningFistMod>())
            {
                var eAttackerLinkHandlers = eAttacker.GetPart<WM_MMA_MartialStances>();
                var eStanceActiveToggles = eAttackerLinkHandlers.StanceVisceraDefID;


                if (eAttacker.IsPlayer() && IsMyActivatedAbilityToggledOn(eStanceActiveToggles, Object))
                {
                    if (eAttacker == Object)
                    {
                        var eRandomizer = Stat.Random(1, 100);
                        var eVisceralRandom = Stat.Random(1, 150);

                        if (eRandomizer <= 33)
                        {
                            // if (eVisceralRandom )
                            if (eVisceralRandom <= 10)
                            {
                                XDidYToZ(eAttacker, "slam a bone-crushing elbow-strike into", eDefender, "", "!", ColorAsGoodFor: eAttacker, PossessiveObject: false);
                            }
                            else if (eVisceralRandom <= 20)
                            {
                                XDidYToZ(eAttacker, "throw a whirling round-house at", eDefender, "", "!", ColorAsGoodFor: eAttacker, PossessiveObject: false);
                            }
                            else if (eVisceralRandom <= 30)
                            {
                                XDidYToZ(eAttacker, "drive a powerful thigh-kick through", eDefender, "", "!", ColorAsGoodFor: eAttacker, PossessiveObject: false);
                            }
                            else if (eVisceralRandom <= 40)
                            {
                                XDidYToZ(eAttacker, "launch a rising-knee at", eDefender, "", "!", ColorAsGoodFor: eAttacker, PossessiveObject: false);
                            }
                            else if (eVisceralRandom <= 50)
                            {
                                XDidYToZ(eAttacker, "sweep with a whirling low-kick at", eDefender, "", "!", ColorAsGoodFor: eAttacker, PossessiveObject: false);
                            }
                            else if (eVisceralRandom <= 60)
                            {
                                XDidYToZ(eAttacker, "drive a strong sun-fist into", eDefender, "", "!", ColorAsGoodFor: eAttacker, PossessiveObject: false);
                            }
                            else if (eVisceralRandom <= 70)
                            {
                                XDidYToZ(eAttacker, "drill a push-kick into", eDefender, "", "!", ColorAsGoodFor: eAttacker, PossessiveObject: false);
                            }
                            else if (eVisceralRandom <= 80)
                            {
                                XDidYToZ(eAttacker, "cleave with a deadly hammerblow into", eDefender, "", "!", ColorAsGoodFor: eAttacker, PossessiveObject: false);
                            }
                            else if (eVisceralRandom <= 90)
                            {
                                XDidYToZ(eAttacker, "drop a strong hammer-kick against", eDefender, "", "!", ColorAsGoodFor: eAttacker, PossessiveObject: false);
                            }
                            else if (eVisceralRandom <= 100)
                            {
                                XDidYToZ(eAttacker, "fire off flurry of jabs at", eDefender, "", "!", ColorAsGoodFor: eAttacker, PossessiveObject: false);
                            }
                            else if (eVisceralRandom <= 110)
                            {
                                XDidYToZ(eAttacker, "let loose a series of rapid kicks into", eDefender, "", "!", ColorAsGoodFor: eAttacker, PossessiveObject: false);
                            }
                            else if (eVisceralRandom <= 120)
                            {
                                XDidYToZ(eAttacker, "let a furious crescent snap-kick crush", eDefender, "", "!", ColorAsGoodFor: eAttacker, PossessiveObject: false);
                            }
                            else if (eVisceralRandom <= 130)
                            {
                                XDidYToZ(eAttacker, "unleash a quick 1-inch punch into", eDefender, "", "!", ColorAsGoodFor: eAttacker, PossessiveObject: false);
                            }
                            else if (eVisceralRandom <= 140)
                            {
                                XDidYToZ(eAttacker, "launch an illusion-twist kick and slam your heel into", eDefender, "", "!", ColorAsGoodFor: eAttacker, PossessiveObject: false);
                            }
                            else if (eVisceralRandom <= 150)
                            {
                                XDidYToZ(eAttacker, "leap into the air and perform a tornado-kick, hitting", eDefender, "", "!", ColorAsGoodFor: eAttacker, PossessiveObject: false);
                            }
                        }
                    }
                }
            }

            return true;
        }

        // Method Calls
        
        public void FindGrappleTarget(List<Cell> FindTarget)
        {
            foreach (var T in FindTarget)
            {
                var O = T.GetFirstObjectWithEffect("Grappled");

                if (O.IsValid())
                {
                    Target = O;
                    break;
                }
            }
        }

        public void GrappleJuice(GameObject AimingAt, int DamageIntBodySmash, int ObjectStrengthMod)
        {
            Target.CellTeleport(C: AimingAt.CurrentCell.GetRandomLocalAdjacentCell(), EnergyCost: 1000,
                Forced: true, VisualEffects: false, SkipRealityDistortion: true);
            Target.TakeDamage(Amount: DamageIntBodySmash, Attributes: "Bashing",
                DeathReason: "colliding into another with vicious force.",
                ThirdPersonDeathReason: "You were splattered against another through wrestling.",
                Owner: Grappler, Attacker: Grappler, Source: null,
                Message: "from %t wrestling maneuver.");
            if (!Target.MakeSave(Stat: "Toughness", Difficulty: 8 + ObjectStrengthMod, Attacker: Object, AttackerStat: "Strength"))
            {
                Target.ApplyEffect(new Dazed(Duration: 8));
            }
            if (!AimingAt.MakeSave(Stat: "Toughness", Difficulty: 8 + ObjectStrengthMod, Attacker: Object, AttackerStat: "Strength"))
            {
                AimingAt.ApplyEffect(new Dazed(Duration: 8));
            }
        }

        public void GetRandomAdjacentVisibleCellAndTeleportObject(Cell eCell)
        {
            var Random = new Random();
            var RandomVisibleCell = eCell.GetAdjacentCells().OrderBy(i => Random.Next());

            foreach (var C in RandomVisibleCell)
            {
                if (C.IsVisible() && C.IsExplored() && C.IsPassable())
                {
                    Target.CellTeleport(
                        C: C,
                        EnergyCost: 1000, Forced: true, VisualEffects: false,
                        SkipRealityDistortion: true);
                    break;
                }
                else
                {
                                    
                }
            }
        }
    }
}