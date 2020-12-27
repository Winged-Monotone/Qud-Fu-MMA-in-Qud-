// XRL.World.Effects.Paralyzed
using System;
using XRL.Core;
using XRL.Rules;
using XRL.UI;
using XRL.World;


namespace XRL.World.Effects
{
    [Serializable]
    public class PsiSupression : Effect
    {

        public PsiSupression()
        {
            base.DisplayName = "{{M|psi-supression}}";
        }

        public PsiSupression(int Duration)
            : this()
        {
            base.Duration = Duration;
        }

        public override int GetEffectType()
        {
            return 117440516;
        }

        public override string GetDetails()
        {
            return "An enemies' strike has rendered your thoughtflow weakened."
            + "- ";
        }

        public override bool Apply(GameObject Object)
        {
            StatShifter.SetStatShift("Ego", -2);
            if (Object.HasEffect("PsiSupression"))
            {
                PsiSupression PsiSupression = Object.GetEffect("PsiSupression") as PsiSupression;
                if (Duration > PsiSupression.Duration)
                {
                    PsiSupression.Duration = Duration;
                }
                return true;
            }
            return false;
        }

        public override void Remove(GameObject Object)
        {
            StatShifter.RemoveStatShifts();
            base.Remove(Object);
        }


        public override bool Render(RenderEvent E)
        {
            int num = XRLCore.CurrentFrame % 60;
            if (num > 15 && num < 30)
            {
                E.Tile = null;
                E.RenderString = "X";
                E.ColorString = "&M^m";
            }
            return true;
        }

    }
}