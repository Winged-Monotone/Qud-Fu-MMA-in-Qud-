namespace XRL.World.Effects
{
    public class StopDeath : Effect
    {
        public override bool WantEvent(int ID, int Cascade)
        {
            return base.WantEvent(ID, Cascade)
                || ID == BeforeDieEvent.ID;
        }

        public override bool HandleEvent(BeforeDieEvent E)
        {
            if (E.Dying == Object)
            {
                Object.ApplyEffect(new Incapacitated(Duration: 600 - (Object.StatMod("Willpower") + Object.StatMod("Toughness") * 10), 30));
                return false;
            }
            
            return base.HandleEvent(E);
        }
    }
}