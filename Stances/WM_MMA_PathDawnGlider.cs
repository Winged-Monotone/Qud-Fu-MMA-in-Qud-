using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using XRL.Rules;
using XRL.Messages;
using XRL.UI;


namespace XRL.World.Parts.Skill
{
    [Serializable]
    public class WM_MMA_PathDawnGlider : BaseSkill
    {
        public Guid DawnStanceID;
        public int BonusSureStrike;


        public WM_MMA_PathDawnGlider()
        {
            Name = "WM_MMA_PathDawnGlider";
            DisplayName = "Path of the Dawnglider";
        }

        public override void Register(GameObject Object)
        {
            Object.RegisterPartEvent(this, "AttackerHit");
            Object.RegisterPartEvent(this, "CommandSureStrikes");
            Object.RegisterPartEvent(this, "PerformMeleeAttack");
            Object.RegisterPartEvent(this, "EndTurn");
            base.Register(Object);
        }

        public override bool FireEvent(Event E)
        {
            if (E.ID == "AttackerHit" && ParentObject.HasEffect("DawnStance"))
            {
                if (ParentObject.HasPart("WM_MMA_SureStrikes"))
                {
                    var MMAComboAccess = ParentObject.GetPart<WM_MMA_SureStrikes>();

                    //Handles damage scaling.

                    if (BonusSureStrike < 10)
                    { ++BonusSureStrike; }
                    MMAComboAccess.UpdateCounter();
                }
                else
                    try
                    {
                        var salthopperDamageSystem = ParentObject.GetPart<WM_MMA_PathSalthopper>();
                        Damage Damage = E.GetParameter<Damage>("Damage");
                        var Attacker = ParentObject;


                        if (salthopperDamageSystem.NegEffectsCollectiveTI.Any(Attacker.HasEffect))
                        {
                            Damage.Amount = (int)Math.Round(Damage.Amount * 1.15f);
                        }
                        else if (salthopperDamageSystem.NegEffectsCollectiveTII.Any(Attacker.HasEffect))
                        {
                            Damage.Amount = (int)Math.Round(Damage.Amount * 1.45f);
                        }
                        else if (salthopperDamageSystem.NegEffectsCollectiveTIII.Any(Attacker.HasEffect))
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
            else if (E.ID == "CommandSureStrikes" && ParentObject.HasEffect("DawnStance"))
            {
                try
                {
                    var MMAComboAccess = ParentObject.GetPart<WM_MMA_SureStrikes>();

                    MMAComboAccess.FistPenBonus = +BonusSureStrike;
                    BonusSureStrike = 0;
                    MMAComboAccess.UpdateCounter();
                }
                catch
                {

                }
            }
            else if (E.ID == "PerformMeleeAttack" && ParentObject.HasEffect("DawnStance"))
            {
                int HitBonus = E.GetIntParameter("HitBonus");

                HitBonus = +2;
            }
            if (E.ID == "EndTurn" && ParentObject.HasEffect("DawnStance"))
            {
                // AddPlayerMessage("SureStrike Stat: " + BonusSureStrike);
            }
            return base.FireEvent(E);
        }

        public override bool AddSkill(GameObject GO)
        {
            this.DawnStanceID = base.AddMyActivatedAbility("Way of The Dawnglider", "DawngliderStanceCommand", "Skill", "Whenever you launch an attack with either your bare hands or natural weapon.", "*", null, false, false, true);
            return true;
        }

        public override bool RemoveSkill(GameObject GO)
        {
            return true;
        }
    }
}
