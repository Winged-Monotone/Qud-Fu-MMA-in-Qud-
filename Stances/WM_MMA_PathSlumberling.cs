using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using XRL.Rules;
using XRL.Messages;
using XRL.UI;

using ConsoleLib.Console;

using System.Threading;
using XRL.Core;

using XRL.World;
using XRL.World.AI.GoalHandlers;
using XRL.World.Parts.Mutation;

using UnityEngine;


namespace XRL.World.Parts.Skill
{
    [Serializable]
    public class WM_MMA_PathSlumberling : BaseSkill
    {
        public Guid SlumberStanceID;
        private static List<BodyPart> DismemberableBodyParts = new List<BodyPart>(8);
        public WM_MMA_PathSlumberling()
        {
            Name = "WM_MMA_PathSlumberling";
            DisplayName = "Path of the Slumberling";
        }
        public void RageStrikePulse(Cell TargetCell)
        {
            for (int i = 0; i < 1; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    TargetCell.ParticleText("&R" + (char)(219 + Stat.Random(0, 10)), 2.9f, 5);
                }
                for (int k = 0; k < 2; k++)
                {
                    TargetCell.ParticleText("&k" + (char)(219 + Stat.Random(0, 10)), 2.9f, 5);
                }
                for (int l = 0; l < 4; l++)
                {
                    TargetCell.ParticleText("&r" + (char)(219 + Stat.Random(0, 10)), 2.9f, 5);
                }
            }
        }
        public override void Register(GameObject Object)
        {
            Object.RegisterPartEvent(this, "AttackerHit");
            Object.RegisterPartEvent(this, "AttackerAfterAttack");
            Object.RegisterPartEvent(this, "SlumberWitnessEvent");
            Object.RegisterPartEvent(this, "SlumberCleaveEvent");
            Object.RegisterPartEvent(this, "PerformMeleeAttack");
            Object.RegisterPartEvent(this, "EndTurn");
            base.Register(Object);
        }

        public override bool FireEvent(Event E)
        {
            if (E.ID == "AttackerHit" && ParentObject.HasEffect("SlumberStance"))
            {
                try
                {
                    var salthopperDamageSystem = ParentObject.GetPart<WM_MMA_PathSalthopper>();
                    Damage Damage = E.GetParameter<Damage>("Damage");
                    var Attacker = ParentObject;


                    if (salthopperDamageSystem.NegEffectsCollectiveTI.Any(Attacker.HasEffect))
                    {
                        Damage.Amount = (int)Math.Round(Damage.Amount * 1.15f);
                    }
                    if (salthopperDamageSystem.NegEffectsCollectiveTII.Any(Attacker.HasEffect))
                    {
                        Damage.Amount = (int)Math.Round(Damage.Amount * 1.55f);
                    }
                    if (salthopperDamageSystem.NegEffectsCollectiveTIII.Any(Attacker.HasEffect))
                    {
                        Damage.Amount = (int)Math.Round(Damage.Amount * 2.5f);
                    }
                    else
                    {
                        Damage.Amount = (int)Math.Round(Damage.Amount * 1.0f);
                    }
                }
                catch
                {

                }
            }
            else if (E.ID == "AttackerAfterAttack" && ParentObject.HasEffect("SlumberStance"))
            {

                // AddPlayerMessage("Execute Attacker hit on Slumberstyle");

                Damage Damage = E.GetParameter<Damage>("Damage");
                var Attacker = ParentObject;
                var Defender = E.GetGameObjectParameter("Defender");
                var Weapon = E.GetGameObjectParameter("Weapon");

                Event E2 = Event.New("SlumberCleaveEvent");
                E2.SetParameter("Attacker", ParentObject);
                E2.SetParameter("Defender", Defender);
                E2.SetParameter("Damage", Damage);

                ParentObject.FireEvent(E2);
            }
            else if (E.ID == "SlumberCleaveEvent" && ParentObject.HasEffect("SlumberStance"))
            {
                GameObject Attacker = E.GetGameObjectParameter("Attacker");
                GameObject Defender = E.GetGameObjectParameter("Defender");
                Damage Damage = E.GetParameter<Damage>("Damage");

                // AddPlayerMessage("var check 1");

                var AttackerLevels = Attacker.Statistics["Level"].BaseValue;

                // AddPlayerMessage("var check 2");

                var AttackerCell = Attacker.GetCurrentCell();
                var AttackersAdacentCells = AttackerCell.GetLocalAdjacentCells();

                var CellQuery = AttackersAdacentCells.Where(Cell => Cell.HasObjectWithPart("Brain") || Cell.HasObjectWithPart("Combat") && Cell.HasCombatObject() && !Cell.HasObject(ParentObject));


                // AddPlayerMessage("slumberstyle initiated vars");
                foreach (var c in CellQuery)
                {
                    // AddPlayerMessage("slumberstarting for each");
                    // AddPlayerMessage("{{dark red|slumbering style fires second for each!}}");
                    var Flankers = c.GetObjectsInCell();
                    RageStrikePulse(c);
                    PlayWorldSound("Hit_Default", 1f, 1f, true);
                    foreach (var obj1 in Flankers)
                    {
                        var ForceQuery = Flankers.Where(Obj => !obj1.HasPart("Brain") || obj1.HasPart("Combat"));
                        // AddPlayerMessage("slumberstarting for each 0");
                        obj1.TakeDamage(Damage.Amount / 2, null, "Slumberling way's fury.");
                        foreach (var objForced in ForceQuery)
                        {
                            if (Stat.Random(1, 100) <= 10 + AttackerLevels / 3)
                            {
                                // AddPlayerMessage("slumber pushing");
                                string KnockBack = Directions.GetRandomDirection();
                                objForced.Push(KnockBack, 2500 * (AttackerLevels / 5));
                            }
                        }
                        var FlankersBody = obj1.Body.GetParts();
                        foreach (var ob in FlankersBody)
                        {
                            var SeveringQuery = Flankers.Where(Obj => !obj1.HasPart("Brain") || obj1.HasPart("Combat"));

                            // AddPlayerMessage("slumberstarting for each 1");
                            foreach (var objpart in SeveringQuery)
                            {
                                if (Stat.Random(1, 100) <= 2 + AttackerLevels / 10)
                                {
                                    var SeveredPartsQuery = Flankers.Where(Obj => ob.IsRegenerable() || ob.IsSeverable() && ob.IsRecoverable() && ob.ParentBody != ParentObject.Body);
                                    foreach (var target in SeveredPartsQuery)
                                    {
                                        var SeveringPartsQuery = SeveredPartsQuery.Where(Obj => ob.AnyMortalParts() || ob.Mortal);

                                        foreach (var targetbodypart in SeveredPartsQuery)
                                        {
                                            if (ob.Name == "Head")
                                            {
                                                ob.Dismember();
                                                target.Die(ParentObject, ParentObject.its + " lobs " + target.its + ob.Name + ", killing it!", false);
                                            }
                                            else
                                            {
                                                ob.Dismember();
                                                target.Die(ParentObject, ParentObject.Its + " strike obliterates " + target.the + "!");
                                            }
                                        }
                                    }

                                    Event E3 = Event.New("SlumberCleaveEvent");
                                    E3.SetParameter("Attacker", ParentObject);
                                    E3.SetParameter("Defender", Defender);

                                    ParentObject.FireEvent("SlumberCleaveEvent");
                                }
                                else
                                {

                                }
                            }
                        }
                    }
                }
            }
            else if (E.ID == "SlumberWitnessEvent" && ParentObject.HasEffect("SlumberStance"))
            {
                // AddPlayerMessage("slumberstarting for each 2");

                var Attacker = E.GetGameObjectParameter("Attacker");
                var Defender = E.GetGameObjectParameter("Defender");

                var AttackerLevels = Attacker.Statistics["Level"].BaseValue;

                var CheckCells = Attacker.CurrentCell.GetLocalAdjacentCells();

                var CellQuery = CheckCells.Where(C => C.HasObjectWithTagOrProperty("Brain") || C.HasObjectWithTagOrProperty("Combat") && C.HasCombatObject() && !C.HasObject(Attacker));

                foreach (var c2 in CellQuery)
                {
                    // AddPlayerMessage("slumberstarting for each 3");
                    var FrightenedFlankers = c2.GetObjectsInCell();
                    var FrightQuery = FrightenedFlankers.Where(Obj => !Obj.MakeSave("Ego", 20 + (AttackerLevels / 3), Attacker, "Ego", "Ego", false) && Obj != Attacker && Obj.HasPart("Brain") || Obj.HasPart("Combat"));

                    foreach (var o2 in FrightenedFlankers)
                    {
                        // AddPlayerMessage("slumberstarting for each 4");
                        string text = (int)Math.Floor((double)(AttackerLevels / 2) + 3.0) + "d6";
                        int num = ParentObject.StatMod("Ego");

                        o2.pBrain.Goals.Clear();
                        o2.pBrain.PushGoal(new Flee(Attacker, 5 + (AttackerLevels / 2), false));
                        // AddPlayerMessage(o2.The + " flees in horror of " + Attacker.Its + " torrent of rage.");
                    }

                }
            }
            else if (E.ID == "PerformMeleeAttack" && ParentObject.HasEffect("SlumberStance"))
            {
                int HitBonus = E.GetIntParameter("HitBonus");

                HitBonus = +1;
            }

            // if (E.ID == "EndTurn" && ParentObject.HasEffect("SlumberStance"))
            // {
            //     AddPlayerMessage(" ");
            // }

            return base.FireEvent(E);
        }


        public override bool AddSkill(GameObject GO)
        {
            this.SlumberStanceID = base.AddMyActivatedAbility("Way of the Slumberling", "SlumberlingStanceCommand", "Skill", "Whenever you launch an attack with either your bare hands or natural weapon.", "*", null, false, false, true);

            return true;
        }

        public override bool RemoveSkill(GameObject GO)
        {
            return true;
        }
    }
}
