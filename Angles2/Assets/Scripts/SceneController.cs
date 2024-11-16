using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : ISceneControllable
{
    public void ChangeScene(string sceneName)
    {
        ServiceLocater.ReturnSoundPlayer().StopBGM(); // ��� �����ش�.
        SceneManager.LoadScene(sceneName);
    }

    public string GetCurrentSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }
}
