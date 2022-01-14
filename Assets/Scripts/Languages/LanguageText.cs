using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class LanguageText : ScriptableObject
{
    public string PlayButtonInitSceneKey;
    public string AchievementsButtonInitSceneKey;
    public string CreditsButtonInitSceneKey;
    public string ExitButtonInitSceneKey;
    public string InfoWhenNoAchievementsInitSceneKey;
    public string ButtonBackInitSceneKey;
    public string WhenAchievementIsNotDiscoveredYetInitSceneKey;
    public string LockedTextForAchievementsInitSceneKey;
    public string CreditsTitleInitSceneKey;
    public string AchievementsTitleInitSceneKey;

    public string ContinueButtonCinemaSceneKey;


    public string DarkMatterInfoTextLaboratoryScenesKey;
    public string MenuButtonTextLaboratoryScenesKey;
    public string ButtonExitTextLaboratoryScenesKey;
    public string ButtonRestartTextLaboratoryScenesKey;
    public string ButtonContinueTextLaboratoryScenesKey;
    public string GoDownOrDieInfoTextForPCLaboratoryScenesKey;
    public string GoUpOrDieInfoTextForPCLaboratoryScenesKey;
    public string GoDownOrDieInfoTextForAndroidLaboratoryScenesKey;
    public string GoUpOrDieInfoTextForAndroidLaboratoryScenesKey;
    public string TitleMenuPauseTextLaboratoryScenesKey;
    public string TitleMenuGameOverTextLaboratoryScenesKey;
    public List<AchievementsInfoText> _listAchievementsInfoText;

    public LanguageText()
    {
    }

    public string GameDesignTitleUsersInitSceneKey;
    public string ProgrammersTitleUsersInitSceneKey;
    public string ArtTitleUsersInitSceneKey;
    public string SoundTitleUsersInitSceneKey;
    public string JumpInfoTextLaboratoryScenesKey;


    public string GetAchievementTitleTextOfKey(string achievementNameKey)
    {
        return _listAchievementsInfoText.Single(x => x.Key == achievementNameKey).NameText;
    }

    public string GetAchievementDescriptionTextOfKey(string achievementInfoNameKey)
    {
        return _listAchievementsInfoText.Single(x => x.Key == achievementInfoNameKey).DescriptionText;
    }
}