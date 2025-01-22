using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISceneControllable
{
    public enum SceneName
    {
        InitScene,
        MenuScene,
        LobbyScene,
        ChapterScene,
        SurvivalScene,
        TutorialScene,
    }

    void ChangeScene(SceneName sceneName);
    SceneName GetCurrentSceneName();
}
