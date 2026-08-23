using System;
using XRL.UI;
using XRL.Core;
using System.Collections.Generic;
using System.Text;
using XRL.World.Parts;
using XRL.Rules;
using XRL.World.Parts.Skill;

namespace XRL.World.Effects
{
    [Serializable]

    public class SlumberStance : Effect
    {
        public Guid SlumberID;
        public int ShockwaveCooldown;



        public SlumberStance() : base()
        {
            base.DisplayName = "{{dark red|Way of the Slumberling}}";
        }

        public SlumberStance(int Duration) : this()
        {
            base.Duration = 1;
        }

        public override string GetDetails()
        {
            return "Vicious sweeping attacks that harm any in the practitioners way while throwing away one's defensive abilities. At the cost of half your AV and DV, attacks while unnarmed now deal damage to nearby foes, for every successful hit on an opponent, enemies flanking you must make a penetration save or be dealt 50% of the damage.\n";
        }

        public override void Register(GameObject go, IEventRegistrar registrar)
        {
            registrar.Register("MovementModeChanged");
            registrar.Register("CanChangeMovementMode");
            registrar.Register("EndTurn");
            registrar.Register("SlumberSleepCommand");
            registrar.Register("LeaveCell");
            registrar.Register("BeginTakeAction");
            base.Register(Object, registrar);
        }

        public override bool FireEvent(Event E)
        {
            if (E.ID == "SlumberSleepCommand")
            {
                if (!Object.HasEffect("Asleep"))
                {
                    Object.ApplyEffect(new Asleep(1200 - (Object.Statistics["Toughness"].Modifier * 10), forced: true, quicksleep: false, Voluntary: true));
                    AddPlayerMessage("You fall into a deep slumber ...");
                }
            }

            return base.FireEvent(E);
        }

        public void RageSplat()
        {
            Cell currentCell = Object.GetCurrentCell();
            if (currentCell != null && currentCell.InActiveZone)
            {
                for (int i = 0; i < 360; i++)
                {
                    float num = (float)XRL.Rules.Stat.RandomCosmetic(1, 10) / 3f;
                    XRLCore.ParticleManager.Add("&R^r*", currentCell.X, currentCell.Y, (float)Math.Sin((double)i * 0.017) / num, (float)Math.Cos((double)i * 0.017) / num);
                }
            }
        }

        public override bool Apply(GameObject Object)
        {
            if (Stat.Random(1, 100) < 50)
                AddPlayerMessage("{{dark red|With a bloodchilling roar, you unleash your slumbering fury!}}");

            RageSplat();

            var ParentAV = Object.Statistics["AV"].Value;
            var ParentDV = Object.Statistics["DV"].Value;

            PlayWorldSound("swapstance", PitchVariance: 0.5f);

            StatShifter.SetStatShift("AV", -(int)Math.Round(ParentAV * 0.5));
            StatShifter.SetStatShift("DV", -(int)Math.Round(ParentDV * 0.5));

            this.SlumberID = base.AddMyActivatedAbility("Slumber", "SlumberSleepCommand", "Power", "Fall into a deep slumber, regenerating your hitpoints.", "Z", null, false, false, false);

            return true;
        }

        public override bool Render(RenderEvent E)
        {
            if (this.Duration > 0)
            {
                int num = XRLCore.CurrentFrame % 60;
                if (num > 25 && num < 35)
                {
                    E.ColorString = "&r";
                }
            }
            return base.Render(E);
        }

        public override void Remove(GameObject Object)
        {
            this.RemoveMyActivatedAbility(ref SlumberID, Object);
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
                                XDidYToZ(eAttacker, "unleash a wild slumber-clawed strike into", eDefender, "tearing viscera from them", "!", ColorAsGoodFor: eAttacker, PossessiveObject: false);
                            }
                            else if (eVisceralRandom <= 20)
                            {
                                XDidYToZ(eAttacker, "clobber", eDefender, "with a hammerblow", "!", ColorAsGoodFor: eAttacker, PossessiveObject: false);
                            }
                            else if (eVisceralRandom <= 30)
                            {
                                XDidYToZ(eAttacker, "drive a knife-blow into", eDefender, "cleaving flesh from them", "!", ColorAsGoodFor: eAttacker, PossessiveObject: false);
                            }
                            else if (eVisceralRandom <= 40)
                            {
                                XDidYToZ(eAttacker, "rake", eDefender, "with a pulled-claw strike, tearing flesh from them", "!", ColorAsGoodFor: eAttacker, PossessiveObject: false);
                            }
                            else if (eVisceralRandom <= 50)
                            {
                                XDidYToZ(eAttacker, "sweep with a whirling roundhouse at", eDefender, "", "!", ColorAsGoodFor: eAttacker, PossessiveObject: false);
                            }
                            else if (eVisceralRandom <= 60)
                            {
                                XDidYToZ(eAttacker, "drive your fist through", eDefender, "impaling them", "!", ColorAsGoodFor: eAttacker, PossessiveObject: false);
                            }
                            else if (eVisceralRandom <= 70)
                            {
                                XDidYToZ(eAttacker, "impale", eDefender, "with a powerful mule-kick", "!", ColorAsGoodFor: eAttacker, PossessiveObject: false);
                            }
                            else if (eVisceralRandom <= 80)
                            {
                                XDidYToZ(eAttacker, "cleave with a deadly hammerblow into", eDefender, "", "!", ColorAsGoodFor: eAttacker, PossessiveObject: false);
                            }
                            else if (eVisceralRandom <= 90)
                            {
                                XDidYToZ(eAttacker, "split", eDefender, "with a deadly axe-kick", "!", ColorAsGoodFor: eAttacker, PossessiveObject: false);
                            }
                            else if (eVisceralRandom <= 100)
                            {
                                XDidYToZ(eAttacker, "unleash a flurry of clawed-strikes at", eDefender, "tearing them limb from limb", "!", ColorAsGoodFor: eAttacker, PossessiveObject: false);
                            }
                            else if (eVisceralRandom <= 110)
                            {
                                XDidYToZ(eAttacker, "you perform a pulled-claw strike against", eDefender, "tearing viscera from them", "!", ColorAsGoodFor: eAttacker, PossessiveObject: false);
                            }
                            else if (eVisceralRandom <= 120)
                            {
                                XDidYToZ(eAttacker, "shatter", eDefender, "innards with a vicious sidekick", "!", ColorAsGoodFor: eAttacker, PossessiveObject: true);
                            }
                            else if (eVisceralRandom <= 130)
                            {
                                XDidYToZ(eAttacker, "thumb-jab", eDefender, "", "!", ColorAsGoodFor: eAttacker, PossessiveObject: false);
                            }
                            else if (eVisceralRandom <= 140)
                            {
                                XDidYToZ(eAttacker, "pull an illusion-twist kick and slam your heel into", eDefender, "", "!", ColorAsGoodFor: eAttacker, PossessiveObject: false);
                            }
                            else if (eVisceralRandom <= 150)
                            {
                                XDidYToZ(eAttacker, "leap into the air and performs a tornado kick, hitting", eDefender, "", "!", ColorAsGoodFor: eAttacker, PossessiveObject: false);
                            }
                        }
                    }
                }
            }

            return true;
        }

    }
}