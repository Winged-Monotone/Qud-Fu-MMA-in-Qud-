using System;
using System.Collections.Generic;
using XRL;
using XRL.Rules;
using XRL.World;
using XRL.World.Parts;
using System.Text;
using XRL.Messages;
using XRL.UI;
using XRL.World.Effects;
using ConsoleLib.Console;
using System.Linq;
using System.Security;
using XRL.Core;

namespace XRL.World.Parts
{
    [Serializable]
    public class AutumnEntropyParticleOnCreate : IActivePart
    {
        public AutumnEntropyParticleOnCreate()
        {
            Name = "AutumnEntropyParticleOnCreate";
        }
        public override void Register(GameObject Object)
        {
            Object.RegisterPartEvent(this, "AutumnEntropyParticleEvent");
            base.Register(Object);
        }
        public override bool WantEvent(int ID, int cascade)
        {
            return base.WantEvent(ID, cascade)
            || ID == AfterObjectCreatedEvent.ID;
        }

        public override bool HandleEvent(AfterObjectCreatedEvent E)
        {
            AddPlayerMessage("Event Firing: AfterObjectCreatedEvent");
            GameObject ObjectedCreated = E.Object;

            Event @Event = Event.New("AutumnEntropyParticleEvent");
            @Event.SetParameter("SplatterOrigin", ObjectedCreated);

            AddPlayerMessage("Event tick 1");
            TurnTick(10);
            ObjectedCreated.FireEvent(@Event);

            AddPlayerMessage("Event tick 2");
            return base.HandleEvent(E);
        }

        public override bool FireEvent(Event E)
        {
            if (E.ID == "AutumnEntropyParticleEvent")
            {
                AddPlayerMessage("Event tick 3");
                GameObject Parent = E.GetGameObjectParameter("SplatterOrigin");
                Parent.Splatter("&B!");
                Parent.ParticlePulse("@");

                AddPlayerMessage("Event tick 4");
            }
            return base.FireEvent(E);
        }
    }
}
