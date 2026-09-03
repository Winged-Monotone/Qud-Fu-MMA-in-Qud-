using System;

namespace XRL.World.Parts
{
    public class Multithrow : IPart
    {
        [NonSerialized]
        private int StarStack = 0;
        
        public override bool WantEvent(int ID, int Cascade)
        {
            return base.WantEvent(ID, Cascade);
        }

        public override bool HandleEvent(GetThrowProfileEvent E)
        {
            
            return base.HandleEvent(E);
        }

        public override void Register(GameObject Object, IEventRegistrar Registrar)
        {
            Registrar.Register("BeforeThrown");
            base.Register(Object, Registrar);
        }

        public override bool FireEvent(Event E)
        {
            if (E.ID == "BeforeThrown")
            {
                var eThrower = E.GetGameObjectParameter("Thrower");
                var eTargetCell = E.GetParameter("TargetCell") as Cell;
                var eApparentTarget = E.GetGameObjectParameter("ApparentTarget");
                var ePhase = E.GetIntParameter("Phase");

                if (StarStack++ < 5)
                {
                    TryMultithrow(eThrower, eTargetCell, eApparentTarget, ePhase);
                }

            }
            return base.FireEvent(E);
        }
        
        public bool TryMultithrow(GameObject Thrower, Cell TargetCell, GameObject ApparentTarget, int Phase)
        {
            if (!Thrower.IsValid() || TargetCell != null)
            {
                return false;
            }

            return Thrower.PerformThrow(ParentObject, TargetCell, ApparentTarget, Phase: Phase);
        }
    }
}