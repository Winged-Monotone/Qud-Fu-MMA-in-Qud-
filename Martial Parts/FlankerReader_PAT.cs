using System;
using System.Collections.Generic;
using System.Text;
using XRL.Rules;
using XRL.Messages;
using XRL.UI;

namespace XRL.World.Parts
{
    [Serializable]
    public class FlankerReader_PAT : IPart
    {
        public bool Check = false;
        public bool ProximityCheck = true;
        public int CheckDuration = 2;
        public int ProximityCount = 0;
        public FlankerReader_PAT()
        {

        }

        public override void Register(GameObject Object)
        {
            Object.RegisterPartEvent(this, "BeginTakeAction");
            Object.RegisterPartEvent(this, "EndTurn");
        }

        public override bool FireEvent(Event E)
        {
            if (E.ID == "BeginTakeAction")
            {
                AddPlayerMessage("FlankerReaderLog: Completed begin take action.");
                var ParentCell = ParentObject.CurrentCell.GetLocalAdjacentCells();
                AddPlayerMessage("FlankerReaderLog: Got Creatures Cell");

                foreach (var C in ParentCell)
                {
                    AddPlayerMessage("FlankerReaderLog: For each launching.");

                    if (C.HasObjectWithEffect("AstralTabbyStance"))
                    {
                        AddPlayerMessage("FlankerReaderLog: Found astral tabby stance, commencing checks.");


                        CheckDuration = 2;
                        continue;
                    }
                }
            }
            else if (E.ID == "EndTurn")
            {
                AddPlayerMessage("FlankerReaderLog: END TURN LAUNCHING");
                var ParentCell = ParentObject.CurrentCell.GetLocalAdjacentCells();

                foreach (var C in ParentCell)
                {
                    if (!C.HasObjectWithEffect("AstralTabbyStance"))
                    {
                        ProximityCount += 1;
                    }
                }

                if (ProximityCount == 0 && CheckDuration > 0)
                {
                    --CheckDuration;
                    AddPlayerMessage("FlankerReaderLog: deprecating CheckDuration");
                }
                else if (CheckDuration <= 0)
                {
                    AddPlayerMessage("FlankerReaderLog: Removing this part.");
                    ParentObject.RemovePart(this);
                }

                AddPlayerMessage("CheckDuration : " + CheckDuration);



            }

            return base.FireEvent(E);
        }
    }
}
