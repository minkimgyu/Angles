using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Skill;

abstract public class BaseFactory
{
    public virtual AudioSource Create(ISoundPlayable.SoundName name) { return default; }
    public virtual BaseViewer Create(BaseViewer.Name name) { return default; }
    public virtual BaseEffect Create(BaseEffect.Name name) { return default; }
    public virtual BaseWeapon Create(BaseWeapon.Name name) { return default; }
    public virtual BaseSkill Create(BaseSkill.Name name) { return default; }
    public virtual BaseLife Create(BaseLife.Name name) { return default; }
    public virtual IInteractable Create(IInteractable.Name name) { return default; }
    public virtual ILevel Create(GameMode.Level level) { return default; }
}

public class InGameFactory
{
    public enum Type
    {
        Viewer,
        Effect,
        Weapon,
        Skill,
        Life,
        Interactable,
        Level,
    }

    Dictionary<Type, BaseFactory> _factories = new Dictionary<Type, BaseFactory>();

    public InGameFactory(AddressableHandler addressableHandler, BaseFactory stageFactory)
    {
        //_factories.Add(Type.Viewer, new ViewerFactory(addressableHandler.ViewerPrefabAsset));
        // --> 로비 UI 전용 팩토리 하나 만들기

        _factories.Add(Type.Viewer, new InGameViewerFactory(addressableHandler.ViewerPrefabAsset));

        _factories.Add(Type.Effect, new EffectFactory(addressableHandler.EffectPrefabAsset));

        _factories.Add(Type.Weapon, new WeaponFactory(addressableHandler.WeaponPrefabAsset, addressableHandler.Database.WeaponDatas, _factories[Type.Effect]));

        _factories.Add(Type.Skill, new SkillFactory(addressableHandler.Database.SkillDatas, addressableHandler.Database.Upgraders, _factories[Type.Effect], _factories[Type.Weapon]));

        _factories.Add(Type.Life, new LifeFactory(addressableHandler.LifePrefabAsset, addressableHandler.Database.LifeDatas, addressableHandler.Database.ChapterDropDatas, _factories[Type.Effect], _factories[Type.Skill]));

        _factories.Add(Type.Interactable, new InteractableObjectFactory(addressableHandler.InteractableAsset, addressableHandler.Database.InteractableObjectDatas));

        _factories.Add(Type.Level, stageFactory); // new ChapterStageFactory(addressableHandler.ChapterMapAsset, addressableHandler.ChapterMapLevelDesignAsset));
    }

    public BaseFactory GetFactory(Type type) { return _factories[type]; }
}