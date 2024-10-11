using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ServiceLocater
{
    static ISoundPlayable _soundPlayer;
    static NullSoundPlayer _nullSoundPlayer;

    static ISceneControllable _sceneController;
    static NullSceneController _nullSceneController;

    static ITimeController _timeController;
    static NullTimeController _nullTimeController;

    static ISaveable _saveController;
    static NULLSaveManager _nullSaveController;

    static ServiceLocater()
    {
        _nullSoundPlayer = new NullSoundPlayer();
        _nullSceneController = new NullSceneController();
        _nullTimeController = new NullTimeController();
        _nullSaveController = new NULLSaveManager();
    }

    public static void Provide(ISoundPlayable soundPlayer)
    {
        _soundPlayer = soundPlayer;
    }

    public static void Provide(ISceneControllable sceneController)
    {
        _sceneController = sceneController;
    }

    public static void Provide(ITimeController timeController)
    {
        _timeController = timeController;
    }

    public static void Provide(ISaveable saveable)
    {
        _saveController = saveable;
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

    public static ITimeController ReturnTimeController()
    {
        if (_timeController == null) return _nullTimeController;
        return _timeController;
    }

    public static ISaveable ReturnSaveManager()
    {
        if (_saveController == null) return _nullSaveController;
        return _saveController;
    }
}
