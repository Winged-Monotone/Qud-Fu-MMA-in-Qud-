using System;
using System.Collections.Generic;
using Qud.API;
using XRL.Rules;
using XRL.World;
using XRL.World.Encounters.EncounterObjectBuilders;


namespace XRL.World.Parts
{
    [Serializable]

    public class DisciplesPack : IPart
    {
        public bool bCreated;

        public override void Register(GameObject Object)
        {
            Object.RegisterPartEvent(this, "EnteredCell");
            base.Register(Object);
        }

        public override bool FireEvent(Event E)
        {
            if (E.ID == "EnteredCell")
            {
                try
                {
                    if (bCreated)
                    {
                        return true;
                    }

                    // AddPlayerMessage("Check 1");

                    bCreated = true;

                    var ParentsAdjacentCells = ParentObject.CurrentCell.GetAdjacentCells();

                    List<string> Disciples = new List<string>();
                    List<string> RaceDisciples = new List<string>();

                    int Ran = Stat.Random(1, 2);

                    // AddPlayerMessage("Check2");

                    foreach (Cell cell in ParentsAdjacentCells)
                    {
                        if (cell.IsEmpty())
                        {
                            if (ParentObject.Factions.Contains("BraversDawn"))
                            {
                                for (int i = 0; i < Ran; i++)
                                {
                                    if (Stat.Random(1, 100) <= 70)
                                        Disciples.Add("BraverDawnDisciple");
                                    else
                                    {
                                        RaceDisciples.Add(EncountersAPI.GetACreatureBlueprint());
                                    }
                                }
                            }
                            else if (ParentObject.Factions.Contains("BraversSaltBack"))
                            {
                                for (int i = 0; i < Ran; i++)
                                {
                                    if (Stat.Random(1, 100) <= 70)
                                        Disciples.Add("BraverSaltBackDisciple");
                                    else
                                    {
                                        RaceDisciples.Add(EncountersAPI.GetACreatureBlueprint());
                                    }
                                }
                            }
                            else if (ParentObject.Factions.Contains("BraversSaltHopper"))
                            {
                                for (int i = 0; i < Ran; i++)
                                {
                                    if (Stat.Random(1, 100) <= 70)
                                        Disciples.Add("BraverSaltHopDisciple");
                                    else
                                    {
                                        RaceDisciples.Add(EncountersAPI.GetACreatureBlueprint());
                                    }
                                }
                            }
                            else if (ParentObject.Factions.Contains("BraversAstralCabby"))
                            {
                                for (int i = 0; i < Ran; i++)
                                {
                                    if (Stat.Random(1, 100) <= 70)
                                        Disciples.Add("BraverAstralDisciple");
                                    else
                                    {
                                        RaceDisciples.Add(EncountersAPI.GetACreatureBlueprint());
                                    }
                                }
                            }
                            else if (ParentObject.Factions.Contains("BraversSlumber"))
                            {
                                for (int i = 0; i < Ran; i++)
                                {
                                    if (Stat.Random(1, 100) <= 70)
                                        Disciples.Add("BraverSlumberDisciple");
                                    else
                                    {
                                        RaceDisciples.Add(EncountersAPI.GetACreatureBlueprint());
                                    }
                                }
                            }
                        }
                    }

                    // AddPlayerMessage("Check3");


                    Tier1HumanoidEquipment tier1HumanoidEquipment = new Tier1HumanoidEquipment();

                    // AddPlayerMessage("Check4");
                    for (int k = 0; k < Disciples.Count; k++)
                    {
                        if (ParentsAdjacentCells.Count <= 0)
                        {
                            break;
                        }

                        // AddPlayerMessage("Check5");


                        GameObject gameObject = GameObject.create(Disciples[k]);
                        GameObject gameObjectRace = GameObject.create(RaceDisciples[k]);

                        if (ParentObject.Factions.Contains("BraversDawn"))
                        {
                            gameObjectRace.DisplayName += " and disciple of the Dawning-Fist Ryu";
                            gameObjectRace.AddSkill("WM_MMASkillTree");
                            gameObjectRace.AddSkill("WM_MMA_SureStrikes");
                            gameObjectRace.AddSkill("WM_MMA_MartialStances");
                            gameObjectRace.AddSkill("WM_MMA_MartialConI");
                            gameObjectRace.AddSkill("WM_MMA_MartialConII");
                            gameObjectRace.AddSkill("WM_MMA_MartialConIII");
                            gameObjectRace.AddSkill("WM_MMA_FlurryOfBlows");
                            gameObjectRace.AddSkill("WM_MMA_CombinationStrikesI");
                            gameObjectRace.AddSkill("WM_MMA_PathDawnGlider");
                        }
                        else if (ParentObject.Factions.Contains("BraversSaltBack"))
                        {
                            gameObjectRace.DisplayName += " and disciple of the Amethyst-Shell Dojo-Ryu";
                            gameObjectRace.AddSkill("WM_MMASkillTree");
                            gameObjectRace.AddSkill("WM_MMA_SureStrikes");
                            gameObjectRace.AddSkill("WM_MMA_MartialStances");
                            gameObjectRace.AddSkill("WM_MMA_MartialConI");
                            gameObjectRace.AddSkill("WM_MMA_MartialConII");
                            gameObjectRace.AddSkill("WM_MMA_MartialConIII");
                            gameObjectRace.AddSkill("WM_MMA_FlurryOfBlows");
                            gameObjectRace.AddSkill("WM_MMA_CombinationStrikesI");
                            gameObjectRace.AddSkill("WM_MMA_PathSaltBack");
                        }
                        else if (ParentObject.Factions.Contains("BraversSaltHopper"))
                        {
                            gameObjectRace.DisplayName += " and disciple of the Scythe-Strike Ryu";
                            gameObjectRace.AddSkill("WM_MMASkillTree");
                            gameObjectRace.AddSkill("WM_MMA_SureStrikes");
                            gameObjectRace.AddSkill("WM_MMA_MartialStances");
                            gameObjectRace.AddSkill("WM_MMA_MartialConI");
                            gameObjectRace.AddSkill("WM_MMA_MartialConII");
                            gameObjectRace.AddSkill("WM_MMA_MartialConIII");
                            gameObjectRace.AddSkill("WM_MMA_FlurryOfBlows");
                            gameObjectRace.AddSkill("WM_MMA_CombinationStrikesI");
                            gameObjectRace.AddSkill("WM_MMA_PathSalthopper");
                        }
                        else if (ParentObject.Factions.Contains("BraversAstralCabby"))
                        {
                            gameObjectRace.DisplayName += " and disciple of the Void-Claw Ryu";
                            gameObjectRace.AddSkill("WM_MMASkillTree");
                            gameObjectRace.AddSkill("WM_MMA_SureStrikes");
                            gameObjectRace.AddSkill("WM_MMA_MartialStances");
                            gameObjectRace.AddSkill("WM_MMA_MartialConI");
                            gameObjectRace.AddSkill("WM_MMA_MartialConII");
                            gameObjectRace.AddSkill("WM_MMA_MartialConIII");
                            gameObjectRace.AddSkill("WM_MMA_FlurryOfBlows");
                            gameObjectRace.AddSkill("WM_MMA_CombinationStrikesI");
                            gameObjectRace.AddSkill("WM_MMA_PathAstralTabby");
                        }
                        else if (ParentObject.Factions.Contains("BraversSlumber"))
                        {
                            gameObjectRace.DisplayName += " and disciple of the Slumbering-Fury Ryu";
                            gameObjectRace.AddSkill("WM_MMASkillTree");
                            gameObjectRace.AddSkill("WM_MMA_SureStrikes");
                            gameObjectRace.AddSkill("WM_MMA_MartialStances");
                            gameObjectRace.AddSkill("WM_MMA_MartialConI");
                            gameObjectRace.AddSkill("WM_MMA_MartialConII");
                            gameObjectRace.AddSkill("WM_MMA_MartialConIII");
                            gameObjectRace.AddSkill("WM_MMA_FlurryOfBlows");
                            gameObjectRace.AddSkill("WM_MMA_CombinationStrikesI");
                            gameObjectRace.AddSkill("WM_MMA_PathSlumberling");
                        }

                        tier1HumanoidEquipment.BuildObject(gameObject);
                        tier1HumanoidEquipment.BuildObject(gameObjectRace);

                        gameObject.pBrain.PartyLeader = ParentObject;
                        gameObjectRace.pBrain.PartyLeader = ParentObject;

                        Cell randomElement = ParentsAdjacentCells.GetRandomElement();

                        if (Stat.Random(1, 100) <= 70 && randomElement.IsEmpty())
                            randomElement.AddObject(gameObject);
                        else if (Stat.Random(1, 100) <= 70 && randomElement.IsEmpty())
                            randomElement.AddObject(gameObjectRace);
                        else if (!randomElement.IsEmpty())
                        {
                            gameObject.MakeActive();
                            gameObjectRace.MakeActive();
                            break;
                        }

                        gameObject.MakeActive();
                        gameObjectRace.MakeActive();
                    }
                    // AddPlayerMessage("Check6");
                }
                catch
                {
                }
            }
            return base.FireEvent(E);
        }
    }
}