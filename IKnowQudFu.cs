// XRL.World.Capabilities.Wishing
using ConsoleLib.Console;
using Genkit;
using HistoryKit;
using Qud.API;
using Sheeter;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using XRL.Annals;
using XRL.Core;
using XRL.Language;
using XRL.Messages;
using XRL.Rules;
using XRL.UI;
using XRL.Wish;
using XRL.World.AI.GoalHandlers;
using XRL.World.Effects;
using XRL.World.Parts;
using XRL.World.Parts.Mutation;
using XRL.World.Parts.Skill;
using XRL.World.QuestManagers;
using XRL.World.Skills.Cooking;
using XRL.World.Tinkering;
using XRL.World.ZoneBuilders;


using static XRL.World.IComponent<XRL.World.IPart>;



namespace MMAWishes
{
    [HasWishCommand]
    class MMAWishHandler
    {
        private static void AddSkill(BaseSkill Skill)
        {
            XRLCore.Core.Game.Player.Body.GetPart<XRL.World.Parts.Skills>().AddSkill(Skill);
        }

        [WishCommand(Command = "IKnowQudFu")]

        public static void MasterMartialArts()
        {

            AddPlayerMessage("Show me.");

            AddSkill(new WM_MMASkillTree());
            AddSkill(new WM_MMA_SureStrikes());
            AddSkill(new WM_MMA_MasterStanceSwap());
            AddSkill(new WM_MMA_MartialStances());
            AddSkill(new WM_MMA_MartialConI());
            AddSkill(new WM_MMA_MartialConII());
            AddSkill(new WM_MMA_MartialConIII());
            AddSkill(new WM_MMA_FlurryOfBlows());
            AddSkill(new WM_MMA_CombinationStrikesI());
            AddSkill(new WM_MMA_CombinationStrikesII());
            AddSkill(new WM_MMA_CombinationStrikesIII());
            AddSkill(new WM_MMA_PathSlumberling());
            AddSkill(new WM_MMA_PathSaltHopper());
            AddSkill(new WM_MMA_PathSaltBack());
            AddSkill(new WM_MMA_PathDawnGlider());
            AddSkill(new WM_MMA_PathAstralTabby());
            AddSkill(new WM_MMA_PathDeathDacca());

        }
    }
}