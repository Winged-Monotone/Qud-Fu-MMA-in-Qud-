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
        public struct NegEffConstruct
        {
            public bool IsActive;
            public Action<GameObject> Effect;
        }
        private List<string> NegEffectsCollectiveTI = new List<string>()
        {
            "Dazed",
            "Supressed",
        };
        private List<string> NegEffectsCollectiveTII = new List<string>()
        {
            "Cripple",
            "Prone",
        };
        private List<string> NegEffectsCollectiveTIII = new List<string>()
        {
            "Paralyzed",
            "Stun",
        };
        private Dictionary<string, NegEffConstruct> NegEffectsSection = new Dictionary<string, NegEffConstruct>
        {
            {
               "NegEffectsCollectiveI" , new NegEffConstruct
            {
                Effect = (ParentObject) =>
            {

            }}},
            {
                "NegEffectsCollectiveII" , new NegEffConstruct
            {
                Effect = (ParentObject) =>
            {

            }}},
            {
                "NegEffectsCollectiveIII", new NegEffConstruct
            {
                Effect = (ParentObject) =>
            {
            }
            }}};

        public WM_MMA_PathSalthopper()
        {
            Name = "WM_MMA_PathSaltHopper";
            DisplayName = "Path of the Salt-Hopper";
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
            if (E.ID == "AttackerAfterAttack" && ParentObject.HasEffect("SaltHopperStance"))
            {
                AddPlayerMessage("Execute Attacker hit on Slumberstyle");

                Damage Damage = E.GetParameter<Damage>("Damage");
                var Attacker = ParentObject;
                var Defender = E.GetGameObjectParameter("Defender");
                var Weapon = E.GetGameObjectParameter("Weapon");

                AddPlayerMessage("var check 1");

                var AttackerLevels = Attacker.Statistics["Level"].BaseValue;

                if (!NegEffectsCollectiveTI.Any(Attacker.HasEffect) && Stat.Random(1, 100) == 3 + AttackerLevels)
                {
                    if (Stat.Random(1, 100) >= 50)
                    {
                        Defender.ApplyEffect(new Dazed(10 + (ParentObject.Statistics["Level"].Value)));
                    }
                    else
                    {
                        Defender.ApplyEffect(new Supressed(10 + (ParentObject.Statistics["Level"].Value)));
                    }
                }
                if (!NegEffectsCollectiveTII.Any(Attacker.HasEffect) && Stat.Random(1, 100) == 3 + AttackerLevels && ParentObject.Statistics["Level"].Value >= 10)
                {
                    if (Stat.Random(1, 100) >= 50)
                    {
                        ParentObject.ApplyEffect(new Cripple(10 + (ParentObject.Statistics["Level"].Value)));
                    }
                    else
                    {
                        ParentObject.ApplyEffect(new Prone());
                    }
                }
                if (!NegEffectsCollectiveTII.Any(Attacker.HasEffect) && Stat.Random(1, 100) == 3 + AttackerLevels && ParentObject.Statistics["Level"].Value >= 20)
                {
                    if (Stat.Random(1, 100) >= 50)
                    {
                        ParentObject.ApplyEffect(new Paralyzed(10 + (ParentObject.Statistics["Level"].Value),
                                                                10 + (ParentObject.Statistics["Level"].Value)));
                    }
                    else
                    {
                        ParentObject.ApplyEffect(new Stun(10 + (ParentObject.Statistics["Level"].Value),
                                                            10 + (ParentObject.Statistics["Level"].Value)));
                    }
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
