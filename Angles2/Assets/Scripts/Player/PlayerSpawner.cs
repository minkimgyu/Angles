using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class PlayerSpawner
{
    FactoryCollection _factoryCollection;
    InputController _inputController;

    Dictionary<SkinData.Key, Sprite> _skinIconAsset;

    Dictionary<SkinData.Key, SkinData> _skinDatas;
    Dictionary<SkinData.Key, List<IStatModifier>> _skinModifiers;

    Dictionary<StatData.Key, StatData> _statDatas;
    Dictionary<StatData.Key, IStatModifier> _statModifiers;

    public PlayerSpawner(
        FactoryCollection factoryCollection,
        InputController inputController,
        Dictionary<SkinData.Key, Sprite> skinIconAsset,

        Dictionary<SkinData.Key, SkinData> skinDatas,
        Dictionary<SkinData.Key, List<IStatModifier>> skinModifiers,

        Dictionary<StatData.Key, StatData> statDatas,
        Dictionary<StatData.Key, IStatModifier> statModifiers)
    {
        _factoryCollection = factoryCollection;
        _inputController = inputController;

        _skinIconAsset = skinIconAsset;

        _skinDatas = skinDatas;
        _skinModifiers = skinModifiers;

        _statDatas = statDatas;
        _statModifiers = statModifiers;
    }

    void ApplySkin(Player player)
    {
        ISaveable saveable = ServiceLocater.ReturnSaveManager();
        SaveData saveData = saveable.GetSaveData();

        if (_skinIconAsset.ContainsKey(saveData._skin) == false) return;
        player.ApplySkinSprite(_skinIconAsset[saveData._skin]);

        if (_skinModifiers.ContainsKey(saveData._skin) == false) return;

        List<IStatModifier> modifiers = _skinModifiers[saveData._skin];
        for (int i = 0; i < modifiers.Count; i++)
        {
            player.Upgrade(modifiers[i]);
        }
    }

    void ApplyStat(Player player)
    {
        ISaveable saveable = ServiceLocater.ReturnSaveManager();
        SaveData saveData = saveable.GetSaveData();

        foreach (var stat in saveData._statLevelInfos)
        {
            if(_statModifiers.ContainsKey(stat.Key) == false) continue;

            int level = stat.Value;
            IStatModifier modifier = _statModifiers[stat.Key];
            for (int i = 0; i < level; i++)
            {
                player.Upgrade(modifier, i);
            }
        }
    }

    public void Spawn(Vector3 spawnPos)
    {
        Player player = (Player)_factoryCollection.ReturnFactory(FactoryCollection.Type.Life).Create(BaseLife.Name.Player);
        player.transform.position = spawnPos;

        ApplySkin(player);
        ApplyStat(player);

        EventBusManager.Instance.SubEventBus.Register(SubEventBus.State.SetPlayerInvincible, new SetPlayerInvincibleCommand(() => player.SetInvincible()));

        _inputController.Initialize();
        if (_inputController != null)
        {
            _inputController.AddEvent(InputController.Side.Left, InputController.Type.OnInputStart, player.OnLeftInputStart);
            _inputController.AddEvent(InputController.Side.Left, InputController.Type.OnInput, player.OnLeftInput);
            _inputController.AddEvent(InputController.Side.Left, InputController.Type.OnInputEnd, player.OnLeftInputEnd);

            _inputController.AddEvent(InputController.Side.Right, InputController.Type.OnInputStart, player.OnRightInputStart);
            _inputController.AddEvent(InputController.Side.Right, InputController.Type.OnInput, player.OnRightInput);
            _inputController.AddEvent(InputController.Side.Right, InputController.Type.OnInputEnd, player.OnRightInputEnd);

            _inputController.AddEvent(InputController.Side.Right, InputController.Type.OnDoubleTab, player.OnRightDoubleTab);
        }

        IFollowable followable = player.GetComponent<IFollowable>();
        if (followable != null) EventBusManager.Instance.SubEventBus.Publish(SubEventBus.State.AddFollableCamera, followable);

        BaseFactory viewerFactory = _factoryCollection.ReturnFactory(FactoryCollection.Type.Viewer);

        BaseViewer hpViewer = viewerFactory.Create(BaseViewer.Name.HpViewer);
        hpViewer.Initialize();
        hpViewer.SetFollower(followable);
        player.AddObserverEvent(hpViewer.UpdateViewer);

        BaseViewer directionViewer = viewerFactory.Create(BaseViewer.Name.DirectionViewer);
        directionViewer.Initialize();
        directionViewer.SetFollower(followable);
    }
}