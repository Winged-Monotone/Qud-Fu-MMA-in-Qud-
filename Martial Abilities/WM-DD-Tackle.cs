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

    public class wmTackleAbility : IPart
    {
        public GameObject Target;
        public GameObject Grappler;
        public Cell TargetCell;
        private string ForcedMoveDirection;
        public Guid TackleCommand;

        public int GetTackleMinimumRange()
        {
            return 2;
        }
        public int GetTackleMaximumRange()
        {
            return 3 + ParentObject.Level / 4;
        }
        private bool ValidTackleTarget(GameObject obj)
        {
            if (obj != null && obj.HasPart("Combat"))
            {
                return obj.FlightMatches(ParentObject);
            }
            return false;
        }

        // Grapple Action Commands


        public bool Grappler_Tackle()
        {
            // AddPlayerMessage("0: Execute Tackle Method");

            if (ParentObject.OnWorldMap())
            {
                if (ParentObject.IsPlayer())
                {
                    Popup.ShowFail("You cannot perform tackles on the world map.");
                }
                return false;
            }
            if (ParentObject.IsFlying)
            {
                if (ParentObject.IsPlayer())
                {
                    Popup.ShowFail("You cannot tackle while flying.");
                }
                return false;
            }
            if (ParentObject.IsOverburdened())
            {
                if (ParentObject.IsPlayer())
                {
                    Popup.ShowFail("You cannot tackle while overburdened.");
                }
                return false;
            }
            if (!ParentObject.CanChangeBodyPosition("Tackling", ShowMessage: true))
            {
                return false;
            }
            if (!ParentObject.CanChangeMovementMode("Tackling", ShowMessage: true))
            {
                return false;
            }

            // AddPlayerMessage("1: Passed exceptions. Assigning variables ...");


            int minimumRange = GetTackleMinimumRange();
            int maximumRange = GetTackleMaximumRange();
            List<Cell> list = PickLine(maximumRange + 1, AllowVis.OnlyVisible, ValidTackleTarget, IgnoreSolid: false, IgnoreLOS: true, RequireCombat: true, Snap: true);

            // AddPlayerMessage("2: Passed Variable Assigning. Getting more uull checks ...");

            if (list == null || list.Count <= 0)
            {
                // AddPlayerMessage("2a: Pick returns null");
                return false;
            }
            if (ParentObject.IsPlayer())
            {
                // AddPlayerMessage("2b: Setting the RemoveAt");
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
            if (ParentObject.AreViableHostilesAdjacent())
            {
                if (IsPlayer())
                {
                    Popup.ShowFail("You cannot tackle while in melee combat.");
                }
                return false;
            }
            // AddPlayerMessage("3: Starting Movement Sequences.");
            GameObject gameParentObject = null;
            Cell cell = list[list.Count - 1];
            AddPlayerMessage("3a");
            gameParentObject = ((!ParentObject.IsPlayer()) ? ParentObject.Target : cell.GetCombatTarget(ParentObject, IgnoreFlight: false, IgnorePhase: true));
            // AddPlayerMessage("4: Passed Movement Seq variable Assignment.");
            if (gameParentObject == null)
            {
                if (IsPlayer())
                {
                    if (cell.GetCombatTarget(ParentObject, IgnoreFlight: true, IgnorePhase: true) != null)
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
            // AddPlayerMessage("5: Continuing Movmeent Assignment.");
            string text = null;
            string text2 = null;
            string colorString = null;
            string detailColor = null;
            // AddPlayerMessage("6: Assignment of various nulls complete.");
            int num2 = 10;
            Disguised disguised = ParentObject.GetEffect<Disguised>();
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
            else if (!string.IsNullOrEmpty(ParentObject.Render.Tile) && Options.UseTiles)
            {
                text2 = ParentObject.Render.Tile;
                colorString = (string.IsNullOrEmpty(ParentObject.Render.TileColor) ? ParentObject.Render.ColorString : ParentObject.Render.TileColor);
                detailColor = ParentObject.Render.DetailColor;
            }
            else
            {
                text = ParentObject.Render.ColorString + ParentObject.Render.RenderString;
            }
            if (Visible())
            {
                if (text2 != null)
                {
                    ParentObject.TileParticleBlip(text2, colorString, detailColor, num2, IgnoreVisibility: false, HFlip: ParentObject.Render.HFlip, VFlip: ParentObject.Render.VFlip);
                }
                else
                {
                    ParentObject.ParticleBlip(text, num2);
                }
            }
            // AddPlayerMessage("7: Starting booling Assigments.");
            bool flag = false;
            bool flag2 = false;
            bool flag3 = false;
            bool flag4 = false;
            bool flag5 = false;
            Cell cell2 = ParentObject.CurrentCell;
            string item = null;
            List<string> list2 = new List<string>(maximumRange + 2);
            int i = 0;
            // AddPlayerMessage("8: Starting Tack For Statement.");
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
            // AddPlayerMessage("9");
            int j = 0;
            int count = list2.Count;
            // AddPlayerMessage("10");
            while (true)
            {
                if (j < count)
                {
                    string text3 = list2[j];
                    Cell cellFromDirection = ParentObject.CurrentCell.GetCellFromDirection(text3, BuiltOnly: false);
                    if (cellFromDirection != null)
                    {
                        bool flag6 = cellFromDirection.Objects.Contains(gameParentObject);
                        GameObject combatTarget = cellFromDirection.GetCombatTarget(ParentObject, IgnoreFlight: false, IgnorePhase: false, IgnoreAttackable: false, AllowInanimate: true, InanimateSolidOnly: true);
                        if (combatTarget != null)
                        {
                            DidXToY("tackle", combatTarget, null, "!", null, null, combatTarget.IsPlayer() ? combatTarget : null);
                            if (ParentObject.DistanceTo(cellFromDirection) <= 1)
                            {
                                // ParentObject.FireEvent(Event.New("wm-TackleGrappleCommand", "Cell", cellFromDirection, "Properties", "Charging"));
                            }
                            else
                            {
                                ParentObject.UseEnergy(1000, "Tackling");
                            }
                            ParentObject.FireEvent(Event.New("wm-TackleGrappleCommand", "Defender", combatTarget));
                            combatTarget.FireEvent(Event.New("WasCharged", "Attacker", ParentObject));
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
                        if (ParentObject.DistanceTo(gameParentObject) == 1)
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
                        if (ParentObject.Move(text3, Forced: false, System: false, IgnoreGravity: false, NoStack: false, NearestAvailable: false, Type: "Tackle"))
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
                                    ParentObject.TileParticleBlip(text2, colorString, detailColor, num2, IgnoreVisibility: false, HFlip: ParentObject.Render.HFlip, VFlip: ParentObject.Render.VFlip);
                                }
                                else
                                {
                                    ParentObject.ParticleBlip(text, num2);
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
                    DidXToY("charge", "right through", gameParentObject, null, "!", null, null, ParentObject);
                }
                else if (flag2)
                {
                    DidXToY("charge", "right past", gameParentObject, null, "!", null, null, ParentObject);
                }
                else if (flag3 || flag || flag5)
                {
                    DidXToY("charge", gameParentObject, ", but" + ParentObject.GetVerb("fail") + " to make contact", "!", null, null, ParentObject);
                }
                else
                {
                    DidX("charge", ", but" + ParentObject.Is + " brought up short", "!", null, null, ParentObject);
                }
                if (flag5)
                {
                    ParentObject.ApplyEffect(new Dazed(1));
                }
                ParentObject.UseEnergy(1000, "Charging");
                break;
            }
            // AddPlayerMessage("11");
            CooldownMyActivatedAbility(TackleCommand, 20);
            // AddPlayerMessage("11");
            return true;
        }
    }
}