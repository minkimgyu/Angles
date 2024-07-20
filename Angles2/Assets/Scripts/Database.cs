using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class Database : Singleton<Database>
{
    //private void Start()
    //{
    //    // Lables »ç¿ë
    //    Addressables.LoadAssetsAsync(type.ToString(), (Object obj) = >
    //    {
    //        if (!m_dicObject.ContainsKey(type))
    //            m_dicObject.Add(type, new Dictionary<string, Object>());

    //        m_dicObject[type].Add(obj.name, obj);

    //    });

    //}

    Dictionary<BaseSkill.Name, BaseSkillData> _skillDatas = new Dictionary<BaseSkill.Name, BaseSkillData>
    {
        { BaseSkill.Name.Statikk, new StatikkData(1, 101, 3, 3, new List<ITarget.Type> { ITarget.Type.Red }) },

        { BaseSkill.Name.Knockback, new KnockbackData(1, 100, new SerializableVector2(5.5f, 3), 
            new SerializableVector2(1.5f, 0), new List<ITarget.Type> { ITarget.Type.Red }) },

        { BaseSkill.Name.Impact, new ImpactData(1, 100, 5, new List<ITarget.Type> { ITarget.Type.Red }) },

        { BaseSkill.Name.SpawnBlackhole, new SpawnBlackholeData(1, new List<ITarget.Type> { ITarget.Type.Red }) },
        { BaseSkill.Name.SpawnShooter, new SpawnShooterData(1, new List<ITarget.Type> { ITarget.Type.Red }) },
        { BaseSkill.Name.SpawnBlade, new SpawnBladeData(1, 3f, new List<ITarget.Type> { ITarget.Type.Red }) },
        { BaseSkill.Name.SpawnStickyBomb, new SpawnStickyBombData(1, new List<ITarget.Type> { ITarget.Type.Red }) },

        { BaseSkill.Name.SpreadBullets, new SpreadBulletsData(1f, 20f, 5f, 3f, 3f, 5, new List<ITarget.Type> { ITarget.Type.Blue })},

        { BaseSkill.Name.Shockwave, new ShockwaveData(1f, 20f, 5f, 3f, new List<ITarget.Type>(){ITarget.Type.Red}) },
        { BaseSkill.Name.MagneticField, new MagneticFieldData(1f, 20f, 5f, 3f, new List<ITarget.Type>(){ITarget.Type.Red}) },
        { BaseSkill.Name.SelfDestruction, new SelfDestructionData(1f, 20f, 5f, 3f, new List<ITarget.Type>(){ITarget.Type.Red}) },
    };

    public Dictionary<BaseSkill.Name, BaseSkillData> SkillDatas { get { return _skillDatas; } }




    Dictionary<BaseWeapon.Name, BaseWeaponData> _weaponData = new Dictionary<BaseWeapon.Name, BaseWeaponData>
    {
        //{BaseWeapon.Name, },
    };
    public Dictionary<BaseWeapon.Name, BaseWeaponData> WeaponData { get { return _weaponData; } }


    Dictionary<BaseLife.Name, BaseLifeData> _lifeDatas = new Dictionary<BaseLife.Name, BaseLifeData>
    {
        { BaseLife.Name.Player, new PlayerData(100, ITarget.Type.Blue, 10, 15, 0.5f, 15, 0.5f, 0.5f, 3, 1, 1.5f, 0.15f, 0.3f) },

        { BaseLife.Name.Triangle, new TriangleData(100, ITarget.Type.Blue, 5, new List<BaseSkill.Name> { BaseSkill.Name.MagneticField }) },

        { BaseLife.Name.Rectangle, new RectangleData(100, ITarget.Type.Blue, 5, new List<BaseSkill.Name> { BaseSkill.Name.MagneticField }) },

        { BaseLife.Name.Pentagon, new PentagonData(100, ITarget.Type.Blue, 5, new List<BaseSkill.Name> { BaseSkill.Name.SpreadBullets }, 4f, 2f) },

        { BaseLife.Name.Hexagon, new HexagonData(100, ITarget.Type.Blue, 5, new List<BaseSkill.Name> { BaseSkill.Name.Shockwave }, 4f, 2f) },

    };
    public Dictionary<BaseLife.Name, BaseLifeData> LifeDatas { get { return _lifeDatas; } }




    Dictionary<CardData.Name, CardData> _cardDatas = new Dictionary<CardData.Name, CardData>
    {
        //{ CardData.Name.KnockbackCard, new CardData() },
        //{ CardData.Name.KnockbackCard, new CardData() },
        //{ CardData.Name.KnockbackCard, new CardData() },
        //{ CardData.Name.KnockbackCard, new CardData() },
        //{ CardData.Name.KnockbackCard, new CardData() },
    };
    public Dictionary<CardData.Name, CardData> CardDatas { get { return _cardDatas; } }




    Dictionary<BaseInteractableObjectData.Name, BaseInteractableObjectData> _interactableObjectDatas;
    public Dictionary<BaseInteractableObjectData.Name, BaseInteractableObjectData> InteractableObjectDatas { get { return _interactableObjectDatas; } }
}
