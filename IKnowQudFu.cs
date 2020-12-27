// // XRL.World.Capabilities.Wishing
// using ConsoleLib.Console;
// using Genkit;
// using HistoryKit;
// using Qud.API;
// using Sheeter;
// using System;
// using System.Collections.Generic;
// using System.Diagnostics;
// using System.IO;
// using System.Linq;
// using System.Reflection;
// using System.Text;
// using UnityEngine;
// using XRL.Annals;
// using XRL.Core;
// using XRL.Language;
// using XRL.Messages;
// using XRL.Rules;
// using XRL.UI;
// using XRL.Wish;
// using XRL.World.AI.GoalHandlers;
// using XRL.World.Effects;
// using XRL.World.Parts;
// using XRL.World.Parts.Mutation;
// using XRL.World.Parts.Skill;
// using XRL.World.QuestManagers;
// using XRL.World.Skills.Cooking;
// using XRL.World.Tinkering;
// using XRL.World.ZoneBuilders;


// using static XRL.World.IComponent<XRL.World.IPart>;



// namespace MMAWishes
// {
//     [HasWishCommand]
//     class MMAWishHandler
//     {
//         [WishCommand(Command = "IKnowQudFu")]
//         public static void MasterMartialArts()
//         {
//             AddPlayerMessage("Show me.");

//             XRLCore.Core.Game.Player.Body.GetPart<XRL.World.Parts.Skills>().AddSkill("WM_MMA_SkillTree").;
//             XRL.World.Capabilities.Wishing.AddSkill("WM_MMA_SureStrikes");
//             XRL.World.Capabilities.Wishing.AddSkill("WM_MMA_MasterStanceSwitch");
//             XRL.World.Capabilities.Wishing.AddSkill("WM_MMA_MartialStances");
//             XRL.World.Capabilities.Wishing.AddSkill("WM_MMA_MartialConI");
//             XRL.World.Capabilities.Wishing.AddSkill("WM_MMA_MartialConII");
//             XRL.World.Capabilities.Wishing.AddSkill("WM_MMA_MartialConIII");
//             XRL.World.Capabilities.Wishing.AddSkill("WM_MMA_FlurryOfBlows");
//             XRL.World.Capabilities.Wishing.AddSkill("WM_MMA_CombinationStrikesI");
//             XRL.World.Capabilities.Wishing.AddSkill("WM_MMA_CombinationStrikesII");
//             XRL.World.Capabilities.Wishing.AddSkill("WM_MMA_CombinationStrikesIII");
//             XRL.World.Capabilities.Wishing.AddSkill("WM_MMA_PathSlumberling");
//             XRL.World.Capabilities.Wishing.AddSkill("WM_MMA_PathSaltHopper");
//             XRL.World.Capabilities.Wishing.AddSkill("WM_MMA_PathSaltBack");
//             XRL.World.Capabilities.Wishing.AddSkill("WM_MMA_PathDawnGlider");
//             XRL.World.Capabilities.Wishing.AddSkill("WM_MMA_PathAstralCabby");
//             XRL.World.Capabilities.Wishing.AddSkill("WM_MMA_Grappler");

//             XRL.World.Capabilities.Wishing.AwardXP(100000);

//         }
//     }
// }