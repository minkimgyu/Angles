using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class Database : Singleton<Database>
{
    public const int _maxSkilCount = 8;
    public int MaxSkilCount { get { return _maxSkilCount; } }

    public List<BaseSkill.Name> _upgradeableSkills = new List<BaseSkill.Name>
    {
        BaseSkill.Name.Statikk,
        BaseSkill.Name.Knockback,
        BaseSkill.Name.Impact,
        BaseSkill.Name.SpawnBlackhole,
        BaseSkill.Name.SpawnStickyBomb,
        BaseSkill.Name.SpawnShooter,
        BaseSkill.Name.SpawnBlade
    };

    public List<BaseSkill.Name> UpgradeableSkills { get { return _upgradeableSkills; } }


    Dictionary<BaseSkill.Name, BaseSkillData> _skillDatas = new Dictionary<BaseSkill.Name, BaseSkillData>
    {
        { BaseSkill.Name.Statikk, new StatikkData(5, 1, 1, 101, 3, 3, new List<ITarget.Type> { ITarget.Type.Red }) },
        { BaseSkill.Name.Knockback, new KnockbackData(5, 3, 1, 100, new SerializableVector2(5.5f, 3), 
            new SerializableVector2(1.5f, 0), new List<ITarget.Type> { ITarget.Type.Red }) },
        { BaseSkill.Name.Impact, new ImpactData(5, 1, 100, 5, new List<ITarget.Type> { ITarget.Type.Red }) },

        { BaseSkill.Name.SpawnBlackhole, new SpawnBlackholeData(5, 1, new List<ITarget.Type> { ITarget.Type.Red }) },
        { BaseSkill.Name.SpawnShooter, new SpawnShooterData(5, new List<ITarget.Type> { ITarget.Type.Red }) },
        { BaseSkill.Name.SpawnBlade, new SpawnBladeData(5, 1, 3f, new List<ITarget.Type> { ITarget.Type.Red }) },
        { BaseSkill.Name.SpawnStickyBomb, new SpawnStickyBombData(5, 5f, 3, new List<ITarget.Type> { ITarget.Type.Red }) },

        { BaseSkill.Name.ContactAttack, new ContactAttackData(1, 30, new List<ITarget.Type> { ITarget.Type.Red }) },

        { BaseSkill.Name.SpreadBullets, new SpreadBulletsData(1, 20f, 5f, 3f, 3f, 5, new List<ITarget.Type> { ITarget.Type.Blue })},
        { BaseSkill.Name.Shockwave, new ShockwaveData(1, 20f, 5f, 3f, new List<ITarget.Type>(){ITarget.Type.Blue}) },
        { BaseSkill.Name.MagneticField, new MagneticFieldData(1, 20f, 5f, 3f, new List<ITarget.Type>(){ITarget.Type.Blue}) },
        { BaseSkill.Name.SelfDestruction, new SelfDestructionData(1, 20f, 5f, 3f, new List<ITarget.Type>(){ITarget.Type.Blue}) },
    };

    public Dictionary<BaseSkill.Name, BaseSkillData> SkillDatas { get { return _skillDatas; } }



    Dictionary<BaseWeapon.Name, BaseWeaponData> _weaponData = new Dictionary<BaseWeapon.Name, BaseWeaponData>
    {
        {BaseWeapon.Name.Blackhole, new BlackholeData(100, 10, 6, 0.1f, 10) },
        {BaseWeapon.Name.Blade, new BladeData(100, 10, 3, 1) },
        {BaseWeapon.Name.Bullet, new BulletData(100, 10, 3) },
        {BaseWeapon.Name.Rocket, new RocketData(100, 10, 3, 105, 3) },
        {BaseWeapon.Name.Shooter, new ShooterData(100, 9, 3, 1, 2.0f) },
        {BaseWeapon.Name.StickyBomb, new StickyBombData(100, 3, 3) },
    };
    public Dictionary<BaseWeapon.Name, BaseWeaponData> WeaponData { get { return _weaponData; } }


    Dictionary<BaseLife.Name, BaseLifeData> _lifeDatas = new Dictionary<BaseLife.Name, BaseLifeData>
    {
        { BaseLife.Name.Player, new PlayerData(100, ITarget.Type.Blue, 10, 15, 0.5f, 15, 0.5f, 0.5f, 3, 1, 1.5f, 0.15f, 0.3f, new List<BaseSkill.Name> { BaseSkill.Name.ContactAttack }) },
        { BaseLife.Name.Triangle, new TriangleData(2000, ITarget.Type.Red, 5, new List<BaseSkill.Name> { BaseSkill.Name.MagneticField }) },
        { BaseLife.Name.Rectangle, new RectangleData(2000, ITarget.Type.Red, 5, new List<BaseSkill.Name> { BaseSkill.Name.MagneticField }) },
        { BaseLife.Name.Pentagon, new PentagonData(2000, ITarget.Type.Red, 5, new List<BaseSkill.Name> { BaseSkill.Name.SpreadBullets }, 4f, 2f) },
        { BaseLife.Name.Hexagon, new HexagonData(2000, ITarget.Type.Red, 5, new List<BaseSkill.Name> { BaseSkill.Name.Shockwave }, 4f, 2f) },

    };
    public Dictionary<BaseLife.Name, BaseLifeData> LifeDatas { get { return _lifeDatas; } }


    Dictionary<BaseSkill.Name, CardInfoData> _cardDatas = new Dictionary<BaseSkill.Name, CardInfoData>
    {
        { BaseSkill.Name.SpawnBlackhole, new CardInfoData(BaseSkill.Name.SpawnBlackhole, "Spawn Blackhole In My Position") },
        { BaseSkill.Name.SpawnBlade, new CardInfoData(BaseSkill.Name.SpawnBlade, "Spawn Blade") },
        { BaseSkill.Name.Impact, new CardInfoData(BaseSkill.Name.Impact, "Spawn Impact") },
        { BaseSkill.Name.Knockback, new CardInfoData(BaseSkill.Name.Knockback, "Spawn Knockback") },
        { BaseSkill.Name.SpawnShooter, new CardInfoData(BaseSkill.Name.SpawnShooter, "Spawn Shooter") },
        { BaseSkill.Name.Statikk, new CardInfoData(BaseSkill.Name.Statikk, "Spawn Statikk") },
        { BaseSkill.Name.SpawnStickyBomb, new CardInfoData(BaseSkill.Name.SpawnStickyBomb, "Spawn StickyBomb") },
    };
    public Dictionary<BaseSkill.Name, CardInfoData> CardDatas { get { return _cardDatas; } }


    Dictionary<BaseInteractableObjectData.Name, BaseInteractableObjectData> _interactableObjectDatas;
    public Dictionary<BaseInteractableObjectData.Name, BaseInteractableObjectData> InteractableObjectDatas { get { return _interactableObjectDatas; } }
}
