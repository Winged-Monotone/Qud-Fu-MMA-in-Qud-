using System;
using XRL.UI;
using XRL.Rules;
using XRL.Core;
using System.Collections.Generic;
using System.Text;
using XRL.World.Parts;
using XRL.World.Parts.Skill;

namespace XRL.World.Effects
{
    [Serializable]

    public class DawnStance : Effect
    {
        public DawnStance() : base()
        {
            base.DisplayName = "{{purple|Way of the Dawnglider}}";
        }

        public DawnStance(int Duration) : this()
        {
            base.Duration = 1;
        }

        public override string GetDetails()
        {
            return "A balanced stance, for those waiting to unleash their inner fire. Dealing successful strikes will add +1 to the 'sure-strike' command up to a maximum +10, multiplying its effect for each stack. While in this stance you gain a +3 to hit and +3 to DV\n";
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

        public void DawnPulse(Cell cell)
        {
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    cell.ParticleText("&M" + (char)(219 + Stat.Random(0, 4)), -4.9f, 2);
                }
                for (int k = 0; k < 2; k++)
                {
                    cell.ParticleText("&m" + (char)(219 + Stat.Random(0, 4)), -4.9f, 2);
                }
                for (int l = 0; l < 2; l++)
                {
                    cell.ParticleText("&o" + (char)(219 + Stat.Random(0, 4)), -4.9f, 2);
                }
            }
        }

        public override bool Apply(GameObject Object)
        {
            PlayWorldSound("swapstance", PitchVariance: 0f);
            DawnPulse(Object.CurrentCell);

            StatShifter.SetStatShift("DV", 3);
            return true;
        }

        public override bool FireEvent(Event E)
        {
            if (E.ID == "IsMobile")
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
                    E.ColorString = "&m";
                }
            }
            return base.Render(E);
        }

        public override void Remove(GameObject Object)
        {
            StatShifter.RemoveStatShifts();
        }

        public override bool WantEvent(int ID, int cascade)
        {
            return base.WantEvent(ID, cascade)
            || ID == AttackerDealtDamageEvent.ID;
        }

        public override bool HandleEvent(AttackerDealtDamageEvent E)
        {
            var eAttacker = E.Source;
            var eDefender = E.Object;

            var eWeapon = E.Weapon;

            if (E.Weapon.IsValid() && eAttacker.HasPart<WM_MMA_MartialStances>() && eWeapon.HasPart<MartialConditioningFistMod>())
            {

                // AddPlayerMessage("E.Log: Found Part");

                var eAttackerLinkHandlers = eAttacker.GetPart<WM_MMA_MartialStances>();
                var eStanceActiveToggles = eAttackerLinkHandlers.StanceVisceraDefID;

                // AddPlayerMessage("E.Log: Found Calls");

                if (eAttacker.IsPlayer() && IsMyActivatedAbilityToggledOn(eStanceActiveToggles, Object))
                {
                    // AddPlayerMessage("T: Stance is Toggle On.");

                    if (eAttacker == Object)
                    {
                        var eRandomizer = Stat.Random(1, 100);
                        var eVisceralRandom = Stat.Random(1, 150);

                        if (eRandomizer <= 33)
                        {
                            // if (eVisceralRandom )
                            if (eVisceralRandom <= 10)
                            {
                                XDidYToZ(eAttacker, "slam a bone-crushing elbow-strike into", eDefender, "", "!");
                            }
                            else if (eVisceralRandom <= 20)
                            {
                                XDidYToZ(eAttacker, "throw a whirling roundhouse at", eDefender, "", "!");
                            }
                            else if (eVisceralRandom <= 30)
                            {
                                XDidYToZ(eAttacker, "drive a powerful thigh-kick through", eDefender, "", "!");
                            }
                            else if (eVisceralRandom <= 40)
                            {
                                XDidYToZ(eAttacker, "launch a rising-knee at", eDefender, "", "!");
                            }
                            else if (eVisceralRandom <= 50)
                            {
                                XDidYToZ(eAttacker, "sweep with a mae mawashi at", eDefender, "", "!");
                            }
                            else if (eVisceralRandom <= 60)
                            {
                                XDidYToZ(eAttacker, "drive a strong sun-fist into", eDefender, "", "!");
                            }
                            else if (eVisceralRandom <= 70)
                            {
                                XDidYToZ(eAttacker, "drill a push-kick into", eDefender, "", "!");
                            }
                            else if (eVisceralRandom <= 80)
                            {
                                XDidYToZ(eAttacker, "cleave with a deadly hammerblow into", eDefender, "", "!");
                            }
                            else if (eVisceralRandom <= 90)
                            {
                                XDidYToZ(eAttacker, "drop a strong hammer-kick against", eDefender, "", "!");
                            }
                            else if (eVisceralRandom <= 100)
                            {
                                XDidYToZ(eAttacker, "fire off a flurry-of-jabs at", eDefender, "", "!");
                            }
                            else if (eVisceralRandom <= 110)
                            {
                                XDidYToZ(eAttacker, "let loose a series of mawashi-geri into", eDefender, "", "!");
                            }
                            else if (eVisceralRandom <= 120)
                            {
                                XDidYToZ(eAttacker, "let a furious crescent snap-kick crush", eDefender, "", "!");
                            }
                            else if (eVisceralRandom <= 130)
                            {
                                XDidYToZ(eAttacker, "unleash a quick 1-inch punch into", eDefender, "", "!");
                            }
                            else if (eVisceralRandom <= 140)
                            {
                                XDidYToZ(eAttacker, "pull an illusion-twist-kick and slam your heel into", eDefender, "", "!");
                            }
                            else if (eVisceralRandom <= 150)
                            {
                                XDidYToZ(eAttacker, "leap into the air and perform a tornado-kick, hitting", eDefender, "", "!");
                            }
                        }
                    }
                }
            }

            return true;
        }
    }
}