using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using XRL.Rules;
using XRL.Messages;
using XRL.UI;
using XRL.World.Effects;

namespace XRL.World.Parts.Skill
{
    [Serializable]
    public class WM_MMA_PathSalthopper : BaseSkill
    {
        public Guid SaltHopperStanceID;

        public List<string> NegEffectsCollectiveTI = new List<string>()
        {
            "Dazed",
            "PsiSupression",
        };
        public List<string> NegEffectsCollectiveTII = new List<string>()
        {
            "Cripple",
            "Prone",
        };
        public List<string> NegEffectsCollectiveTIII = new List<string>()
        {
            "Paralyzed",
            "Stun",
        };

        public WM_MMA_PathSalthopper()
        {
            Name = "WM_MMA_PathSaltHopper";
            DisplayName = "Path of the Salt-Hopper";
        }

        public override void Register(GameObject Object)
        {
            Object.RegisterPartEvent(this, "AttackerHit");
            Object.RegisterPartEvent(this, "AttackerAfterAttack");
            Object.RegisterPartEvent(this, "SlumberWitnessEvent");
            Object.RegisterPartEvent(this, "PerformMeleeAttack");
            Object.RegisterPartEvent(this, "EndTurn");
            base.Register(Object);
        }

        public override bool FireEvent(Event E)
        {
            if (E.ID == "AtttackerHit" && ParentObject.HasEffect("SaltHopperStance"))
            {
                Damage Damage = E.GetParameter<Damage>("Damage");
                var Attacker = ParentObject;

                var ComboSI = ParentObject.GetPart<WM_MMA_CombinationStrikesI>();

                if (NegEffectsCollectiveTI.Any(Attacker.HasEffect))
                {
                    Damage.Amount = (int)Math.Round(Damage.Amount * 1.15f);
                }
                if (NegEffectsCollectiveTII.Any(Attacker.HasEffect))
                {
                    Damage.Amount = (int)Math.Round(Damage.Amount * 1.55f);
                }
                if (NegEffectsCollectiveTIII.Any(Attacker.HasEffect))
                {
                    Damage.Amount = (int)Math.Round(Damage.Amount * 2.5f);
                }
                else
                {
                    Damage.Amount = (int)Math.Round(Damage.Amount * 0.75f);
                }
            }
            if (E.ID == "AttackerAfterAttack" && ParentObject.HasEffect("SaltHopperStance"))
            {

                var ComboSI = ParentObject.GetPart<WM_MMA_CombinationStrikesI>();

                ComboSI.CurrentComboICounter = 0;
                ComboSI.UpdateCounter();
                // AddPlayerMessage("Execute Attacker hit on Salthopperstyle");

                Damage Damage = E.GetParameter<Damage>("Damage");
                var Attacker = ParentObject;
                var Defender = E.GetGameObjectParameter("Defender");
                var Weapon = E.GetGameObjectParameter("Weapon");

                var AttackerLevels = Attacker.Statistics["Level"].BaseValue;

                // AddPlayerMessage("var check 1");



                if (!NegEffectsCollectiveTI.Any(Attacker.HasEffect) && Stat.Random(1, 100) <= 3 + AttackerLevels)
                {
                    if (Stat.Random(1, 100) >= 50)
                    {
                        Defender.ApplyEffect(new Dazed(10 + (ParentObject.Statistics["Level"].Value)));
                    }
                    else
                    {
                        Defender.ApplyEffect(new PsiSupression(10 + (ParentObject.Statistics["Level"].Value)));
                    }
                }
                else if (!NegEffectsCollectiveTII.Any(Attacker.HasEffect) && Stat.Random(1, 100) <= 3 + AttackerLevels && ParentObject.Statistics["Level"].Value >= 10)
                {
                    if (Stat.Random(1, 100) >= 50)
                    {
                        Defender.ApplyEffect(new Cripple(10 + (ParentObject.Statistics["Level"].Value)));
                    }
                    else
                    {
                        Defender.ApplyEffect(new Prone());
                    }
                }
                else if (!NegEffectsCollectiveTII.Any(Attacker.HasEffect) && Stat.Random(1, 100) <= 3 + AttackerLevels && ParentObject.Statistics["Level"].Value >= 20)
                {
                    if (Stat.Random(1, 100) >= 50)
                    {
                        Defender.ApplyEffect(new Paralyzed(10 + (ParentObject.Statistics["Level"].Value),
                                                                10 + (ParentObject.Statistics["Level"].Value)));
                    }
                    else
                    {
                        Defender.ApplyEffect(new Stun(10 + (ParentObject.Statistics["Level"].Value),
                                                            10 + (ParentObject.Statistics["Level"].Value)));
                    }
                }
                else if (Stat.Random(1, 100) <= 2 + (AttackerLevels / 5))
                {
                    Defender.UseEnergy(25);
                }

            }

            return base.FireEvent(E);
        }

        public override bool AddSkill(GameObject GO)
        {

            this.SaltHopperStanceID = base.AddMyActivatedAbility("Way of the Salt-Hopper", "SaltHopperStanceCommand", "Skill", "Whenever you launch an attack with either your bare hands or natural weapon.", "*", null, false, false, true);

            return true;
        }

        public override bool RemoveSkill(GameObject GO)
        {
            return true;
        }
    }
}
