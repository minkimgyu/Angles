using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class BaseFactory
{
    public virtual BaseViewer Create(BaseViewer.Name name) { return default; }
    public virtual BaseEffect Create(BaseEffect.Name name) { return default; }
    public virtual BaseWeapon Create(BaseWeapon.Name name) { return default; }
    public virtual BaseSkill Create(BaseSkill.Name name) { return default; }
    public virtual BaseLife Create(BaseLife.Name name) { return default; }
    public virtual IInteractable Create(IInteractable.Name name) { return default; }
    public virtual BaseBuff Create(BaseBuff.Name name) { return default; }
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
        Buff
    }

    Dictionary<Type, BaseFactory> _factories = new Dictionary<Type, BaseFactory>();

    public FactoryCollection(AddressableHandler addressableHandler, Database database)
    {
        _factories.Add(Type.Buff, new BuffFactory(database.BuffDatas));

        _factories.Add(Type.Viewer, new ViewerFactory(addressableHandler.ViewerPrefabs));

        _factories.Add(Type.Effect, new EffectFactory(addressableHandler.EffectPrefabs));

        _factories.Add(Type.Weapon, new WeaponFactory(addressableHandler.WeaponPrefabs, _factories[Type.Effect]));

        _factories.Add(Type.Skill, new SkillFactory(database.SkillDatas, _factories[Type.Effect], _factories[Type.Weapon], _factories[Type.Buff]));

        _factories.Add(Type.Life, new LifeFactory(addressableHandler.LifePrefabs, database.LifeDatas, _factories[Type.Effect], _factories[Type.Skill]));

        _factories.Add(Type.Interactable, new InteractableObjectFactory(addressableHandler.InteractableAssetDictionary, database.InteractableObjectDatas));

    }

    public BaseFactory ReturnFactory(Type type) { return _factories[type]; }
}