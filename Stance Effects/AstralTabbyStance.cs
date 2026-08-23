using System;
using XRL.Core;
using XRL.Rules;

using System.Collections.Generic;
using System.Text;
using XRL.World.Parts;
using XRL.UI;
using XRL.World.Parts.Skill;


namespace XRL.World.Effects
{
    [Serializable]

    public class AstralTabbyStance : Effect
    {
        public int saveTarget;
        public int saveTargetTurnDivisor;
        public int turns;


        public AstralTabbyStance() : base()
        {
            base.DisplayName = "{{blue|Way of the Astral Tabby}}";
        }

        public AstralTabbyStance(int Duration) : this()
        {
            base.Duration = 1;
            turns = 0;
        }

        public override string GetDetails()
        {
            return "Evasive, untouchable, no matter how many flank you--you move as the void does. When flanked by multiple enemies, you add your agility modifier to your DV per unit surrounding you, successful dodges build your Combination Strike counter and you gain a +5 to movement speed, you however lose your unnarmed damage bonus.\n";
        }

        public override void Register(GameObject go, IEventRegistrar registrar)
        {
            registrar.Register("MovementModeChanged");
            registrar.Register("CanChangeMovementMode");
            registrar.Register("EndTurn");
            registrar.Register("IsMobile");
            registrar.Register("LeaveCell");
            registrar.Register("BeginTakeAction");
            base.Register(Object, registrar);
        }

        public void AstralTabbyPulse(Cell cell)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    cell.ParticleText("&B" + (char)(219 + Stat.Random(0, 4)), -4.9f, 2);
                }
                for (int k = 0; k < 5; k++)
                {
                    cell.ParticleText("&b" + (char)(219 + Stat.Random(0, 4)), -4.9f, 2);
                }
                for (int l = 0; l < 5; l++)
                {
                    cell.ParticleText("&Y" + (char)(219 + Stat.Random(0, 4)), -4.9f, 2);
                }
            }
        }

        public override bool Apply(GameObject Object)
        {
            StatShifter.SetStatShift("MoveSpeed", -10);

            PlayWorldSound("swapstance", PitchVariance: 0f);

            AstralTabbyPulse(Object.CurrentCell);
            return true;
        }

        public override bool FireEvent(Event E)
        {
            if (E.ID == "IsMobile")
            {

            }

            else if (E.ID == "BeginTakeAction")
            {

            }

            else if (E.ID == "LeaveCell")
            {

            }

            else if (E.ID == "EndTurn")
            {

            }

            else if (E.ID == "CanChangeMovementMode")
            {

            }

            else if (E.ID == "MovementModeChanged")
            {

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
                    E.ColorString = "&b";
                }
            }
            return base.Render(E);
        }

        public override void Remove(GameObject Object)
        {
            StatShifter.RemoveStatShifts();

        }

        public override bool HandleEvent(AttackerDealingDamageEvent E)
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
                                XDidYToZ(eAttacker, "tabby-claw rake", eDefender, "", "!");
                            }
                            else if (eVisceralRandom <= 20)
                            {
                                XDidYToZ(eAttacker, "perform a tabby-claw reverse claw-strike against", eDefender, "", "!");
                            }
                            else if (eVisceralRandom <= 30)
                            {
                                XDidYToZ(eAttacker, "swipe with an open claw-strike at", eDefender, "", "!");
                            }
                            else if (eVisceralRandom <= 40)
                            {
                                XDidYToZ(eAttacker, "launch a rising-knee at", eDefender, "", "!");
                            }
                            else if (eVisceralRandom <= 50)
                            {
                                XDidYToZ(eAttacker, "sweep with a whirling low-kick at", eDefender, "", "!");
                            }
                            else if (eVisceralRandom <= 60)
                            {
                                XDidYToZ(eAttacker, "drive a knife-blow into", eDefender, "", "!");
                            }
                            else if (eVisceralRandom <= 70)
                            {
                                XDidYToZ(eAttacker, "drill a push-kick into", eDefender, "", "!");
                            }
                            else if (eVisceralRandom <= 80)
                            {
                                XDidYToZ(eAttacker, "cleave with a deadly raking-claw strike into", eDefender, "", "!");
                            }
                            else if (eVisceralRandom <= 90)
                            {
                                XDidYToZ(eAttacker, "drop a strong hammer-kick against", eDefender, "", "!");
                            }
                            else if (eVisceralRandom <= 100)
                            {
                                XDidYToZ(eAttacker, "fire off flurry-of-jabs at", eDefender, "", "!");
                            }
                            else if (eVisceralRandom <= 110)
                            {
                                XDidYToZ(eAttacker, "let loose a series of claw-strikes into", eDefender, "", "!");
                            }
                            else if (eVisceralRandom <= 120)
                            {
                                XDidYToZ(eAttacker, "let your furious crescent snap-kick crush", eDefender, "", "!");
                            }
                            else if (eVisceralRandom <= 130)
                            {
                                XDidYToZ(eAttacker, "unleash a quick knife-blow into", eDefender, "", "!");
                            }
                            else if (eVisceralRandom <= 140)
                            {
                                XDidYToZ(eAttacker, "leap and tabby-claw pounce", eDefender, "", "!");
                            }
                            else if (eVisceralRandom <= 150)
                            {
                                XDidYToZ(eAttacker, "leap into the air and perform a whirling hook-kick, wrecking", eDefender, "", "!");
                            }
                        }
                    }
                }
            }

            return true;
        }
    }
}