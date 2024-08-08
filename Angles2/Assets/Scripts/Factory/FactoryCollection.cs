using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFactory
{
    BaseViewer Create(BaseViewer.Name name);
    BaseEffect Create(BaseEffect.Name name);
    BaseWeapon Create(BaseWeapon.Name name);
    BaseSkill Create(BaseSkill.Name name);
    BaseLife Create(BaseLife.Name name);
    IInteractable Create(IInteractable.Name name);
}

public class FactoryCollection : IFactory
{
    ViewerFactory _viewerFactor;
    EffectFactory _effectFactory;
    WeaponFactory _weaponFactory;
    LifeFactory _lifeFactory;
    SkillFactory _skillFactory;
    InteractableObjectFactory _interactableObjectFactory;

    public FactoryCollection(AddressableHandler addressableHandler, Database database)
    {
        _viewerFactor = new ViewerFactory(addressableHandler.ViewerPrefabs);

        _effectFactory = new EffectFactory(addressableHandler.EffectPrefabs);

        _weaponFactory = new WeaponFactory(addressableHandler.WeaponPrefabs, database.WeaponDatas, _effectFactory.Create);

        _skillFactory = new SkillFactory(database.SkillDatas, _effectFactory.Create, _weaponFactory.Create);

        _lifeFactory = new LifeFactory(addressableHandler.LifePrefabs, database.LifeDatas, _effectFactory.Create, _skillFactory.Create);

        _interactableObjectFactory = new InteractableObjectFactory(addressableHandler.InteractableAssetDictionary, database.InteractableObjectDatas);
    }

    public BaseViewer Create(BaseViewer.Name name) { return _viewerFactor.Create(name); }
    public BaseEffect Create(BaseEffect.Name name) { return _effectFactory.Create(name); }
    public BaseWeapon Create(BaseWeapon.Name name) { return _weaponFactory.Create(name); }
    public BaseSkill Create(BaseSkill.Name name) { return _skillFactory.Create(name); }
    public BaseLife Create(BaseLife.Name name) { return _lifeFactory.Create(name); }
    public IInteractable Create(IInteractable.Name name) { return _interactableObjectFactory.Create(name); }
}
