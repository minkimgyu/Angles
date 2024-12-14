using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NullSceneController : ISceneControllable
{
    public void ChangeScene(string sceneName) { }
    public string GetCurrentSceneName() { return default; }
}
