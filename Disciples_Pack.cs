using System;
using System.Collections.Generic;
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
                                    Disciples.Add("BraverDawnDisciple");
                                }
                            }
                            else if (ParentObject.Factions.Contains("BraversSaltBack"))
                            {
                                for (int i = 0; i < Ran; i++)
                                {
                                    Disciples.Add("BraverSaltBackDisciple");
                                }
                            }
                            else if (ParentObject.Factions.Contains("BraversSaltHopper"))
                            {
                                for (int i = 0; i < Ran; i++)
                                {
                                    Disciples.Add("BraverSaltHopDisciple");
                                }
                            }
                            else if (ParentObject.Factions.Contains("BraversAstralCabby"))
                            {
                                for (int i = 0; i < Ran; i++)
                                {
                                    Disciples.Add("BraverAstralDisciple");
                                }
                            }
                            else if (ParentObject.Factions.Contains("BraversSlumber"))
                            {
                                for (int i = 0; i < Ran; i++)
                                {
                                    Disciples.Add("BraverSlumberDisciple");
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
                        tier1HumanoidEquipment.BuildObject(gameObject);
                        gameObject.pBrain.PartyLeader = ParentObject;
                        Cell randomElement = ParentsAdjacentCells.GetRandomElement();
                        randomElement.AddObject(gameObject);
                        gameObject.MakeActive();
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