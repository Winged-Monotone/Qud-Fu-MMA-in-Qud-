using System;
using XRL.UI;
using XRL.Core;
using XRL.Rules;

using System.Collections.Generic;
using System.Text;
using XRL.World.Parts;
using XRL.World.Parts.Skill;

namespace XRL.World.Effects
{
    [Serializable]

    public class SaltHopperStance : Effect
    {
        public int saveTarget;
        public int saveTargetTurnDivisor;
        public int turns;


        public SaltHopperStance() : base()
        {
            base.DisplayName = "{{orange|Way of the Salt-Hopper}}";
        }

        public SaltHopperStance(int Duration) : this()
        {
            base.Duration = 1;
            turns = 0;
        }

        public override string GetDetails()
        {
            return "Sudden, high precision strikes that target sensitive areas on the foe; melee damage switches to agility, cannot build combinations strikes and only perform one attack per turn, however each blow has a 10% chance of adding either Immobilize, Confuse, Stun, Daze or Crippled/Hobbled to your enemy.\n";
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

        public void SaltHopperPulse(Cell cell)
        {
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    cell.ParticleText("&o" + (char)(219 + Stat.Random(0, 4)), -4.9f, 2);
                }
                for (int k = 0; k < 2; k++)
                {
                    cell.ParticleText("&O" + (char)(219 + Stat.Random(0, 4)), -4.9f, 2);
                }
                for (int l = 0; l < 2; l++)
                {
                    cell.ParticleText("&Y" + (char)(219 + Stat.Random(0, 4)), -4.9f, 2);
                }
            }
        }


        public override bool Apply(GameObject Object)
        {
            PlayWorldSound("swapstance", PitchVariance: 0f);
            SaltHopperPulse(Object.CurrentCell);
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

            return base.FireEvent(E);
        }

        public override bool Render(RenderEvent E)
        {
            if (this.Duration > 0)
            {
                int num = XRLCore.CurrentFrame % 60;
                if (num > 25 && num < 22)
                {
                    E.ColorString = "&o";
                }
            }
            return base.Render(E);
        }

        public override void Remove(GameObject Object)
        {
            StatShifter.RemoveStatShifts();

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
                                XDidYToZ(eAttacker, "use a pin-point mantis blow on", eDefender, "", "!");
                            }
                            else if (eVisceralRandom <= 20)
                            {
                                XDidYToZ(eAttacker, "knife-blow", eDefender, "", "!");
                            }
                            else if (eVisceralRandom <= 30)
                            {
                                XDidYToZ(eAttacker, "finger strike the pressure points of", eDefender, "", "!");
                            }
                            else if (eVisceralRandom <= 40)
                            {
                                XDidYToZ(eAttacker, "needle-blow", eDefender, "", "!");
                            }
                            else if (eVisceralRandom <= 50)
                            {
                                XDidYToZ(eAttacker, "two-finger knuckle-strike", eDefender, "", "!");
                            }
                            else if (eVisceralRandom <= 60)
                            {
                                XDidYToZ(eAttacker, "drive a strong ridgehand blow into", eDefender, "", "!");
                            }
                            else if (eVisceralRandom <= 70)
                            {
                                XDidYToZ(eAttacker, "drill a push-kick into", eDefender, "", "!");
                            }
                            else if (eVisceralRandom <= 80)
                            {
                                XDidYToZ(eAttacker, "cleave with a chestting-strike at", eDefender, "", "!");
                            }
                            else if (eVisceralRandom <= 90)
                            {
                                XDidYToZ(eAttacker, "drop a strong hammer-kick against", eDefender, "", "!");
                            }
                            else if (eVisceralRandom <= 100)
                            {
                                XDidYToZ(eAttacker, "fire off a flurry of knuckled-strikes into", eDefender, "", "!");
                            }
                            else if (eVisceralRandom <= 110)
                            {
                                XDidYToZ(eAttacker, "let loose a series of rapid kicks into", eDefender, "", "!");
                            }
                            else if (eVisceralRandom <= 120)
                            {
                                XDidYToZ(eAttacker, "let loose a furious crescent snap-kick, crushing", eDefender, "", "!");
                            }
                            else if (eVisceralRandom <= 130)
                            {
                                XDidYToZ(eAttacker, "unleash a quick mantis-strike", eDefender, "", "!");
                            }
                            else if (eVisceralRandom <= 140)
                            {
                                XDidYToZ(eAttacker, "hop and deliver successive front kicks into", eDefender, "", "!");
                            }
                            else if (eVisceralRandom <= 150)
                            {
                                XDidYToZ(eAttacker, "leap into the air and palm-strike", eDefender, "", "!");
                            }
                        }
                    }
                }
            }

            return true;
        }
    }
}