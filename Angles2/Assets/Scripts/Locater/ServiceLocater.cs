using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServiceLocater
{
    static ISoundPlayable _soundPlayer;
    static NullSoundPlayer _nullSoundPlayer;

    static ISceneControllable _sceneController;
    static NullSceneController _nullSceneController;

    static IInputable _inputController;
    static NullInputController _nullInputController;

    public static void Initialize()
    {
        _nullSoundPlayer = new NullSoundPlayer();
        _nullSceneController = new NullSceneController();
        _nullInputController = new NullInputController();
    }

    public static void Provide(ISoundPlayable soundPlayer)
    {
        _soundPlayer = soundPlayer;
    }

    public static void Provide(ISceneControllable sceneController)
    {
        _sceneController = sceneController;
    }

    public static void Provide(IInputable inputController)
    {
        _inputController = inputController;
    }

    public static ISoundPlayable ReturnSoundPlayer()
    {
        if (_soundPlayer == null) return _nullSoundPlayer;
        return _soundPlayer;
    }

    public static ISceneControllable ReturnSceneController()
    {
        if (_sceneController == null) return _nullSceneController;
        return _sceneController;
    }

    public static IInputable ReturnInputController()
    {
        if (_inputController == null) return _nullInputController;
        return _inputController;
    }
}
