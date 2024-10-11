using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class BaseFactory
{
    public virtual AudioSource Create(ISoundPlayable.SoundName name) { return default; }
    public virtual BaseViewer Create(BaseViewer.Name name) { return default; }
    public virtual BaseEffect Create(BaseEffect.Name name) { return default; }
    public virtual BaseWeapon Create(BaseWeapon.Name name) { return default; }
    public virtual BaseSkill Create(BaseSkill.Name name) { return default; }
    public virtual BaseLife Create(BaseLife.Name name) { return default; }
    public virtual IInteractable Create(IInteractable.Name name) { return default; }
}

public class FactoryCollection
{
    public enum Type
    {
        Viewer,
        Effect,
        Weapon,
        Skill,
        Life,
        Interactable,
    }

    Dictionary<Type, BaseFactory> _factories = new Dictionary<Type, BaseFactory>();

    public FactoryCollection(AddressableHandler addressableHandler, Database database)
    {
        _factories.Add(Type.Viewer, new ViewerFactory(addressableHandler.ViewerPrefabAsset));

        _factories.Add(Type.Effect, new EffectFactory(addressableHandler.EffectPrefabAsset));

        _factories.Add(Type.Weapon, new WeaponFactory(addressableHandler.WeaponPrefabAsset, database.WeaponDatas, _factories[Type.Effect]));

        _factories.Add(Type.Skill, new SkillFactory(database.SkillDatas, database.Upgraders, _factories[Type.Effect], _factories[Type.Weapon]));

        _factories.Add(Type.Life, new LifeFactory(addressableHandler.LifePrefabAsset, database.LifeDatas, _factories[Type.Effect], _factories[Type.Skill]));

        _factories.Add(Type.Interactable, new InteractableObjectFactory(addressableHandler.InteractableAsset, database.InteractableObjectDatas));
    }

    public BaseFactory ReturnFactory(Type type) { return _factories[type]; }
}