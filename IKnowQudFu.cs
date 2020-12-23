using XRL.Wish;
using XRL.World;

using static XRL.World.IComponent<XRL.World.IPart>;

[HasWishCommand]
public class IKnowQudFu
{
    [WishCommand]
    public static bool MasterMartialArts()
    {
        var PlayerCatch = game.Player;

        PlayerCatch.Body.AddSkill("WM_MMA_SkillTree");
        PlayerCatch.Body.AddSkill("WM_MMA_SureStrikes");
        PlayerCatch.Body.AddSkill("WM_MMA_MasterStanceSwitch");
        PlayerCatch.Body.AddSkill("WM_MMA_MartialStances");
        PlayerCatch.Body.AddSkill("WM_MMA_MartialConI");
        PlayerCatch.Body.AddSkill("WM_MMA_MartialConII");
        PlayerCatch.Body.AddSkill("WM_MMA_MartialConIII");
        PlayerCatch.Body.AddSkill("WM_MMA_FlurryOfBlows");
        PlayerCatch.Body.AddSkill("WM_MMA_CombinationStrikesI");
        PlayerCatch.Body.AddSkill("WM_MMA_CombinationStrikesII");
        PlayerCatch.Body.AddSkill("WM_MMA_CombinationStrikesIII");
        PlayerCatch.Body.AddSkill("WM_MMA_PathSlumberling");
        PlayerCatch.Body.AddSkill("WM_MMA_PathSaltHopper");
        PlayerCatch.Body.AddSkill("WM_MMA_PathSaltBack");
        PlayerCatch.Body.AddSkill("WM_MMA_PathDawnGlider");
        PlayerCatch.Body.AddSkill("WM_MMA_PathAstralCabby");
        PlayerCatch.Body.AddSkill("WM_MMA_Grappler");

        PlayerCatch.Body.AwardXP(100000);



        return true;
    }
}