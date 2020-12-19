using System;
using System.Collections.Generic;
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


namespace XRL.World.Parts.Skill
{
    [Serializable]
    public class WM_MMA_PathSlumberling : BaseSkill
    {
        public Guid SlumberStanceID;
        private static List<BodyPart> DismemberableBodyParts = new List<BodyPart>(8);
        public bool wmBodyPartIsDismemberable(BodyPart Part, GameObject Actor = null, bool assumeDecapitate = false)
        {
            if (!Part.IsSeverable())
            {
                return false;
            }
            if (!assumeDecapitate && Part.SeverRequiresDecapitate())
            {
                if (Actor == null)
                {
                    return false;
                }
                if (!Actor.HasSkill("Axe_Decapitate"))
                {
                    return false;
                }
            }
            return true;
        }
        public BodyPart wmGetDismemberableBodyPart(GameObject obj, GameObject Actor = null, bool assumeDecapitate = false)
        {
            if (!obj.HasPart("Body"))
            {
                return null;
            }
            DismemberableBodyParts.Clear();
            foreach (BodyPart part in obj.Body.GetParts())
            {
                if (wmBodyPartIsDismemberable(part, Actor, assumeDecapitate))
                {
                    DismemberableBodyParts.Add(part);
                }
            }
            return DismemberableBodyParts.GetRandomElement();
        }
        public bool wmApplyFearToObject(string nDice, int Duration, GameObject GO, GameObject SourceObject, GameObject FearObject = null, bool Panicked = false, bool Mental = false)
        {
            if (FearObject == null)
            {
                FearObject = SourceObject;
            }
            if (FearObject != null)
            {
                int combatMA = Stats.GetCombatMA(GO);
                int num = Stat.RollPenetratingSuccesses(nDice, combatMA);
                if (Mental && num > 0)
                {
                    num = GO.ResistMentalIntrusion("Fear", num, SourceObject);
                }
                if (num > 0 && GO.FireEvent(new Event("ApplyFear", "SourceObject", SourceObject, "FearObject", FearObject)) && CanApplyEffectEvent.Check(GO, "Fear"))
                {
                    GO.pBrain.Goals.Clear();
                    GO.pBrain.PushGoal(new Flee(FearObject, Duration, Panicked));
                    for (int i = 0; i < num; i++)
                    {
                        GO.ParticleText("&W!");
                    }
                    IComponent<GameObject>.XDidY(GO, "become", "afraid", "!", null, null, GO);
                    GO.FireEvent(new Event("FearApplied", "SourceObject", SourceObject, "FearObject", FearObject));
                    return true;
                }
            }
            IComponent<GameObject>.XDidY(GO, "resist", "becoming afraid", "!", null, GO);
            return false;
        }
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
            Object.RegisterPartEvent(this, "AttackerAfterAttack");
            Object.RegisterPartEvent(this, "SlumberWitnessEvent");
            Object.RegisterPartEvent(this, "PerformMeleeAttack");
            Object.RegisterPartEvent(this, "EndTurn");
            base.Register(Object);
        }

        public override bool FireEvent(Event E)
        {
            if (E.ID == "AttackerAfterAttack" && ParentObject.HasEffect("SlumberStance"))
            {
                AddPlayerMessage("Execute Attacker hit on Slumberstyle");

                Damage Damage = E.GetParameter<Damage>("Damage");
                var Attacker = E.GetGameObjectParameter("Attacker");
                var Defender = E.GetGameObjectParameter("Defender");
                var Weapon = E.GetGameObjectParameter("Weapon");

                AddPlayerMessage("var check 1");

                var AttackerLevels = Attacker.Statistics["Level"].BaseValue;

                AddPlayerMessage("var check 2");

                var AttackerCell = Attacker.GetCurrentCell();
                var AttackersAdacentCells = AttackerCell.GetLocalAdjacentCells();

                AddPlayerMessage("slumberstyle initiated vars");
                foreach (var c in AttackersAdacentCells)
                {
                    AddPlayerMessage("slumberstarting for each");
                    if (c.HasObjectWithPart("Brain") || c.HasObjectWithPart("Combat") && c.HasCombatObject() && !c.HasObject(ParentObject))
                    {
                        AddPlayerMessage("{{dark red|slumbering style fires second for each!}}");
                        var Flankers = c.GetObjectsInCell();

                        foreach (var o in Flankers)
                        {
                            AddPlayerMessage("slumberstarting for each 0");
                            o.TakeDamage(Damage.Amount / 2);
                            if (Stat.Random(1, 100) <= 10 + AttackerLevels / 3)
                            {
                                AddPlayerMessage("slumber pushing");
                                string KnockBack = Directions.GetRandomDirection();
                                o.Push(KnockBack, 2500 * (AttackerLevels / 5));
                            }

                            RageStrikePulse(c);
                            PlayWorldSound("Hit_Default", 1f, 1f, true);

                            var FlankersBody = o.Body.GetParts();
                            foreach (var ob in FlankersBody)
                            {
                                AddPlayerMessage("slumberstarting for each 1");
                                if (Stat.Random(1, 100) <= 3 + AttackerLevels / 3)
                                {

                                    ob.Dismember();
                                    AddPlayerMessage("{{red|" + ParentObject.Its + " vicious strike cleaves " + Defender.The + " " + ob.Name + " from " + Defender.its + " form.}}");
                                    Attacker.FireEvent(Event.New("SlumberWitnessEvent", "Defender", Defender, "Attacker", Attacker));
                                }
                            }
                        }
                    }
                }
            }
            if (E.ID == "SlumberWitnessEvent" && ParentObject.HasEffect("SlumberStance"))
            {
                var Attacker = E.GetGameObjectParameter("Attacker");
                var Defender = E.GetGameObjectParameter("Defender");

                var AttackerLevels = Attacker.Statistics["Level"].BaseValue;

                var CheckCells = Defender.CurrentCell.GetLocalAdjacentCells();

                foreach (var c2 in CheckCells)
                {
                    AddPlayerMessage("slumberstarting for each 2");
                    if (c2.HasObjectWithTagOrProperty("Brain") || c2.HasObjectWithTagOrProperty("Combat") && c2.HasCombatObject() && !c2.HasObject(Attacker))
                    {
                        var FrightenedFlankers = c2.GetObjectsInCell();

                        foreach (var o2 in FrightenedFlankers)
                        {
                            if (!o2.MakeSave("Ego", 10 + (AttackerLevels / 3), Attacker, "Ego", "Ego", false))
                            {
                                AddPlayerMessage("slumberstarting for each 3");
                                string text = (int)Math.Floor((double)(AttackerLevels / 2) + 3.0) + "d6";
                                int num = ParentObject.StatMod("Ego");

                                wmApplyFearToObject("1d" + 6 + (AttackerLevels / 3), Attacker.StatMod("Ego") + (AttackerLevels / 3), o2, Attacker, null, false, false);
                            }
                        }
                    }
                }
            }

            if (E.ID == "PerformMeleeAttack" && ParentObject.HasEffect("SlumberStance"))
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
