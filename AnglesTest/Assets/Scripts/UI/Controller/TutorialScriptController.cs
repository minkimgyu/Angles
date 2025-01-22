using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TutorialScriptModel
{
    TutorialScriptViewer _viewer;

    public TutorialScriptModel(TutorialScriptViewer viewer)
    {
        _viewer = viewer;
    }

    public Tuple<float, int> FadeInOutFinger
    {
        set
        {
            _viewer.FadeInOutLeftFinger(value.Item1, value.Item2);
        }
    }

    public Tuple<Vector2, float, float> DragRightFinger
    {
        set
        {
            _viewer.DragRightFinger(value.Item1, value.Item2, value.Item3);
        }
    }

    public Tuple<string, string> ChangeScript
    {
        set
        {
            _viewer.ChangeScript(value.Item1, value.Item2);
        }
    }

    public Tuple<bool, float> ActivateScript
    {
        set
        {
            _viewer.ActivateScript(value.Item1, value.Item2);
        }
    }
}


public class TutorialScriptController : MonoBehaviour
{
    [SerializeField] TutorialScriptViewer _viewer;
    TutorialScriptModel _model;

    public void Initialize()
    {
        _viewer.Initialize();
        _model = new TutorialScriptModel(_viewer);
    }

    public void FadeInOutFinger(float fadeDuration, int fadeCount)
    {
        _model.FadeInOutFinger = new Tuple<float, int>(fadeDuration, fadeCount);
    }

    public void DragRightFinger(Vector2 drawPosition, float moveDuration, float fadeDuration)
    {
        _model.DragRightFinger = new Tuple<Vector2, float, float>(drawPosition, moveDuration, fadeDuration);
    }

    public void ChangeScript(string title, string content)
    {
        _model.ChangeScript = new Tuple<string, string>(title, content);
    }

    public void ActivateScript(bool activate, float delay)
    {
        _model.ActivateScript = new Tuple<bool, float>(activate, delay);
    }
}
