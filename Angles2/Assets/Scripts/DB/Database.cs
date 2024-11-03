using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System;

public class Database
{
    List<BaseSkill.Name> _upgradeableSkills = new List<BaseSkill.Name>
    {
        BaseSkill.Name.Statikk,
        BaseSkill.Name.Knockback,
        BaseSkill.Name.Impact,
        BaseSkill.Name.SpawnBlackhole,
        BaseSkill.Name.SpawnStickyBomb,
        BaseSkill.Name.SpawnBlade,

        BaseSkill.Name.SpawnRifleShooter,
        BaseSkill.Name.SpawnRocketShooter,

        BaseSkill.Name.UpgradeDamage,
        BaseSkill.Name.UpgradeCooltime,
        BaseSkill.Name.UpgradeDash,
        BaseSkill.Name.UpgradeShooting,
    };

    public List<BaseSkill.Name> UpgradeableSkills { get { return _upgradeableSkills; } }

    Dictionary<BaseSkill.Name, IUpgradeVisitor> _upgrader = new Dictionary<BaseSkill.Name, IUpgradeVisitor>
    {
        {
            BaseSkill.Name.Statikk,
            new StatikkUpgrader
            (
                new List<StatikkUpgrader.UpgradableData>
                {
                    new StatikkUpgrader.UpgradableData(10, 0, 0, 0), // 2 될 떄
                    new StatikkUpgrader.UpgradableData(0, 0, 1, 1), // 3 될 떄
                    new StatikkUpgrader.UpgradableData(10, 0, 0, 0), // 4 될 떄
                    new StatikkUpgrader.UpgradableData(0, 0, 1, 1), // 5 될 떄
                }
            )
        },
        {
            BaseSkill.Name.Knockback,
            new KnockbackUpgrader
            (
                new List<KnockbackUpgrader.UpgradableData>
                {
                    new KnockbackUpgrader.UpgradableData(0, 0.2f),
                    new KnockbackUpgrader.UpgradableData(10, 0.0f),
                    new KnockbackUpgrader.UpgradableData(0, 0.3f),
                    new KnockbackUpgrader.UpgradableData(10, 0.3f),
                }
            )
        },
        {
            BaseSkill.Name.Impact,
            new ImpactUpgrader
            (
                new List<ImpactUpgrader.UpgradableData>
                {
                    new ImpactUpgrader.UpgradableData(10, 0.2f),
                    new ImpactUpgrader.UpgradableData(10, 0.2f),
                    new ImpactUpgrader.UpgradableData(10, 0.3f),
                    new ImpactUpgrader.UpgradableData(10, 0.3f),
                }
            )
        },
        {
            BaseSkill.Name.SpawnBlackhole,
            new SpawnBlackholeUpgrader
            (
                new List<SpawnBlackholeUpgrader.UpgradableData>
                {
                    new SpawnBlackholeUpgrader.UpgradableData(1, 0.1f),
                    new SpawnBlackholeUpgrader.UpgradableData(0, 0.1f),
                    new SpawnBlackholeUpgrader.UpgradableData(1, 0.1f),
                    new SpawnBlackholeUpgrader.UpgradableData(1, 0.2f),
                }
            )
        },
         {
            BaseSkill.Name.SpawnBlade,
            new SpawnBladeUpgrader
            (
                new List<SpawnBladeUpgrader.UpgradableData>
                {
                    new SpawnBladeUpgrader.UpgradableData(10, 0, 0.1f),
                    new SpawnBladeUpgrader.UpgradableData(0, 1, 0.1f),
                    new SpawnBladeUpgrader.UpgradableData(10, 0, 0.1f),
                    new SpawnBladeUpgrader.UpgradableData(10, 1, 0.2f),
                }
            )
        },
         {
            BaseSkill.Name.SpawnStickyBomb,
            new SpawnStickyBombUpgrader
            (
                new List<SpawnStickyBombUpgrader.UpgradableData>
                {
                    new SpawnStickyBombUpgrader.UpgradableData(10, 1),
                    new SpawnStickyBombUpgrader.UpgradableData(0, 1),
                    new SpawnStickyBombUpgrader.UpgradableData(10, 0),
                    new SpawnStickyBombUpgrader.UpgradableData(10, 1),
                }
            )
        },
         {
            BaseSkill.Name.SpawnRocketShooter,
            new SpawnShooterUpgrader
            (
                new List<SpawnShooterUpgrader.UpgradableData>
                {
                    new SpawnShooterUpgrader.UpgradableData(10, 0),
                    new SpawnShooterUpgrader.UpgradableData(10, -0.5f),
                    new SpawnShooterUpgrader.UpgradableData(10, 0),
                    new SpawnShooterUpgrader.UpgradableData(10, -0.5f),
                }
            )
        },
         {
            BaseSkill.Name.SpawnRifleShooter,
            new SpawnShooterUpgrader
            (
                new List<SpawnShooterUpgrader.UpgradableData>
                {
                    new SpawnShooterUpgrader.UpgradableData(10, 0),
                    new SpawnShooterUpgrader.UpgradableData(0, -0.2f),
                    new SpawnShooterUpgrader.UpgradableData(10, 0),
                    new SpawnShooterUpgrader.UpgradableData(0, -0.3f),
                }
            )
        },

         // 기본 스텟: 4
         // 추가 업그레이드 수치: 1

         // 특수 사각형(빨간색) --> LV.2, 일반 사각형(노란색) --> LV.1
         // 데미지, 발사 속도
         {
            BaseSkill.Name.SpreadBullets,
            new SpreadBulletsUpgrader
            (
                new List<SpreadBulletsUpgrader.UpgradableData>
                {
                    new SpreadBulletsUpgrader.UpgradableData(-1, 5, 2),
                    new SpreadBulletsUpgrader.UpgradableData(-1, 5, 2),
   
                }
            )
        },
         {
            BaseSkill.Name.Shockwave,
            new ShockwaveUpgrader
            (
                new List<ShockwaveUpgrader.UpgradableData>
                {
                    new ShockwaveUpgrader.UpgradableData(10, -0.5f, 0.2f),
                    new ShockwaveUpgrader.UpgradableData(10, -0.5f, 0.3f),
                }
            )
        },
         {
            BaseSkill.Name.MagneticField,
            new MagneticFieldUpgrader
            (
                new List<MagneticFieldUpgrader.UpgradableData>
                {
                    new MagneticFieldUpgrader.UpgradableData(5, 0),
                    new MagneticFieldUpgrader.UpgradableData(5, 0),
                }
            )
        },
         {
            BaseSkill.Name.SelfDestruction,
            new SelfDestructionUpgrader
            (
                new List<SelfDestructionUpgrader.UpgradableData>
                {
                    new SelfDestructionUpgrader.UpgradableData(1, 8, 3),
                    new SelfDestructionUpgrader.UpgradableData(1, 8, 3),
                }
            )
        },
    };

    public Dictionary<BaseSkill.Name, IUpgradeVisitor> Upgraders { get { return _upgrader; } }

    Dictionary<BaseSkill.Name, SkillData> CopySkillDatas = new Dictionary<BaseSkill.Name, SkillData>
    {
        { BaseSkill.Name.Statikk, new StatikkData(5, 1, 1, 10, 3, 3, 1, new List<ITarget.Type> { ITarget.Type.Red }) },

        { BaseSkill.Name.Knockback, new KnockbackData(5, 3, 1, 20, 3, new SerializableVector2(5.5f, 3), new SerializableVector2(1.5f, 0), new List<ITarget.Type> { ITarget.Type.Red }) },

        { BaseSkill.Name.Impact, new ImpactData(5, 0.2f, 20, 2, 1, new List<ITarget.Type> { ITarget.Type.Red }) },

        { BaseSkill.Name.SpawnBlackhole, new SpawnBlackholeData(5, 0.1f, 3, new List<ITarget.Type> { ITarget.Type.Red })},

        { BaseSkill.Name.SpawnRifleShooter, new SpawnShooterData(5, BaseWeapon.Name.RifleShooter, 10, 1, BaseWeapon.Name.ShooterBullet, new List<ITarget.Type> { ITarget.Type.Red }) },

        { BaseSkill.Name.SpawnRocketShooter, new SpawnShooterData(5, BaseWeapon.Name.RocketShooter, 20, 3, BaseWeapon.Name.Rocket, new List<ITarget.Type> { ITarget.Type.Red }) },

        { BaseSkill.Name.SpawnBlade, new SpawnBladeData(5, 0.3f, 10, 3f, 8f, new List<ITarget.Type> { ITarget.Type.Red }) },

        { BaseSkill.Name.SpawnStickyBomb, new SpawnStickyBombData(5, 5f, 1, 30, 3, new List<ITarget.Type> { ITarget.Type.Red }) },

        { BaseSkill.Name.ContactAttack, new ContactAttackData(1, 10, 1, new List<ITarget.Type> { ITarget.Type.Red }) },

        {
            BaseSkill.Name.UpgradeDamage,
            new UpgradeDamageData(
                3,
                new List<StatUpgrader.DamageData>
                { 
                    new StatUpgrader.DamageData(0.1f),
                    new StatUpgrader.DamageData(0.15f),
                    new StatUpgrader.DamageData(0.25f),
                }
            )
        },

        {
            BaseSkill.Name.UpgradeCooltime,
            new UpgradeCooltimeData(
                3,
                new List<StatUpgrader.CooltimeData>
                {
                    new StatUpgrader.CooltimeData(-0.1f),
                    new StatUpgrader.CooltimeData(-0.1f),
                    new StatUpgrader.CooltimeData(-0.1f),
                }
            )
        },

         {
            BaseSkill.Name.UpgradeShooting,
            new UpgradeShootingData(
                3,
                new List<StatUpgrader.ShootingData>
                {
                    new StatUpgrader.ShootingData(0.5f, -0.3f),
                    new StatUpgrader.ShootingData(0.5f, -0.3f),
                    new StatUpgrader.ShootingData(1, -0.4f),
                }
            )
        },

        {
            BaseSkill.Name.UpgradeDash,
            new UpgradeDashData(
                3,
                new List<StatUpgrader.DashData>
                {
                    new StatUpgrader.DashData(2f, -1f),
                    new StatUpgrader.DashData(3f, -1f),
                    new StatUpgrader.DashData(5f, -1f),
                }
            )
        },

        { BaseSkill.Name.MultipleShockwave, new MultipleShockwaveData(1, 0.5f, 0.7f, 3, 30f, 1.7f, 3f, new List<ITarget.Type>(){ITarget.Type.Blue}) },
        { BaseSkill.Name.SpreadMultipleBullets, new SpreadMultipleBulletsData(1, 0.3f, 4, 10, 2f, 4f, 5f, 1f, new List<ITarget.Type> { ITarget.Type.Blue }) },

        { BaseSkill.Name.SpreadBullets, new SpreadBulletsData(3, 10, 4f, 4f, 5f, 1f, new List<ITarget.Type> { ITarget.Type.Blue })},
        { BaseSkill.Name.Shockwave, new ShockwaveData(3, 30f, 5f, 3f, new List<ITarget.Type>(){ITarget.Type.Blue}) },
        { BaseSkill.Name.MagneticField, new MagneticFieldData(3, 5f, 1f, new List<ITarget.Type>(){ITarget.Type.Blue}) },
        { BaseSkill.Name.SelfDestruction, new SelfDestructionData(3, 20f, 5f, 3f, 0.7f, new List<ITarget.Type>(){ITarget.Type.Red, ITarget.Type.Blue}) },
    };

    public Dictionary<BaseSkill.Name, SkillData> SkillDatas { get { return CopySkillDatas; } }

    Dictionary<BaseWeapon.Name, WeaponData> CopyWeaponDatas = new Dictionary<BaseWeapon.Name, WeaponData>
    {
        { BaseWeapon.Name.Blade, new BladeData(1, 1)},

        { BaseWeapon.Name.PentagonicBullet, new BulletData(5)},
        { BaseWeapon.Name.ShooterBullet, new BulletData(5)},
        { BaseWeapon.Name.PentagonBullet, new BulletData(5)},
        { BaseWeapon.Name.Rocket, new RocketData(3, 5)},

        { BaseWeapon.Name.Blackhole, new BlackholeData(100, 0.1f)},
        { BaseWeapon.Name.StickyBomb, new StickyBombData(3, 3, 1)},

        { BaseWeapon.Name.RifleShooter, new ShooterData(10, 1, 18, 4.0f, 10.0f)},
        { BaseWeapon.Name.RocketShooter, new ShooterData(10, 1, 18, 4.0f, 10.0f)},

    };
    public Dictionary<BaseWeapon.Name, WeaponData> WeaponDatas { get { return CopyWeaponDatas; } }

    Dictionary<BaseLife.Name, LifeData> CopyLifeDatas = new Dictionary<BaseLife.Name, LifeData>
    {
        { 
            BaseLife.Name.Player, new PlayerData(
                100, 
                ITarget.Type.Blue, 
                10, 

                1.1f,
                10,
                1.1f,

                8,
                10,
                8,

                0.4f, 
                1.0f, 
                0.4f, 

                2f,
                10f,
                2f,

                15f,
                0.2f, 
                3, 
                1, 

                5f,
                10f,
                5f, 

                0.15f,
                0.25f,
            new List<BaseSkill.Name> { BaseSkill.Name.ContactAttack })
        },

        { 
            BaseLife.Name.YellowTriangle, new TriangleData(10, ITarget.Type.Red, BaseLife.Size.Small,
                
            new Dictionary<BaseSkill.Name, int>
            {
                { BaseSkill.Name.MagneticField, 0 }
            },

            new DropData(3, 
                new List<Tuple<IInteractable.Name, float>>
                { 
                    new Tuple<IInteractable.Name, float>( IInteractable.Name.Coin, 0.3f),
                    new Tuple<IInteractable.Name, float>( IInteractable.Name.Heart, 0.1f),
                    //new Tuple<IInteractable.Name, float>( IInteractable.Name.SkillBubble, 0.1f)
                }
            ), 8) 
        },

        { 
            BaseLife.Name.YellowRectangle, new RectangleData(20, ITarget.Type.Red, BaseLife.Size.Small,

            new Dictionary<BaseSkill.Name, int>
            {
                { BaseSkill.Name.MagneticField, 0 }
            },

            new DropData(3,
                new List<Tuple<IInteractable.Name, float>>
                {
                    new Tuple<IInteractable.Name, float>( IInteractable.Name.Coin, 0.3f),
                    new Tuple<IInteractable.Name, float>( IInteractable.Name.Heart, 0.1f),
                    //new Tuple<IInteractable.Name, float>( IInteractable.Name.SkillBubble, 0.1f)
                }
            ), 6)
        },

        { 
            BaseLife.Name.YellowPentagon, new PentagonData(40, ITarget.Type.Red, BaseLife.Size.Medium,

            new Dictionary<BaseSkill.Name, int>
            {
                { BaseSkill.Name.SpreadBullets, 0 }
            },

            new DropData(3,
                new List<Tuple<IInteractable.Name, float>>
                {
                    new Tuple<IInteractable.Name, float>( IInteractable.Name.Coin, 0.3f),
                    new Tuple<IInteractable.Name, float>( IInteractable.Name.Heart, 0.1f),
                    //new Tuple<IInteractable.Name, float>( IInteractable.Name.SkillBubble, 0.1f)
                }
            ), 4f, 3f, 1f)
        },

        { 
            BaseLife.Name.YellowHexagon, new HexagonData(60, ITarget.Type.Red, BaseLife.Size.Medium,

            new Dictionary<BaseSkill.Name, int>
            {
                { BaseSkill.Name.Shockwave, 0 }
            },

            new DropData(3,
                new List<Tuple<IInteractable.Name, float>>
                {
                    new Tuple<IInteractable.Name, float>( IInteractable.Name.Coin, 0.3f),
                    new Tuple<IInteractable.Name, float>( IInteractable.Name.Heart, 0.1f),
                    //new Tuple<IInteractable.Name, float>( IInteractable.Name.SkillBubble, 0.1f)
                }
            ), 4f, 3f, 1f)
        },



        // 스킬 업그레이드 수정
        {
            BaseLife.Name.RedTriangle, new TriangleData(15, ITarget.Type.Red, BaseEnemy.Size.Small,

            new Dictionary<BaseSkill.Name, int>
            {
                { BaseSkill.Name.MagneticField, 1 }
            },

            new DropData(3,
                new List<Tuple<IInteractable.Name, float>>
                {
                    new Tuple<IInteractable.Name, float>( IInteractable.Name.Coin, 0.3f)
                }
            ), 11)
        },

        {
            BaseLife.Name.RedRectangle, new RectangleData(30, ITarget.Type.Red, BaseEnemy.Size.Small,

            new Dictionary<BaseSkill.Name, int>
            {
                { BaseSkill.Name.MagneticField, 1 },
                { BaseSkill.Name.SelfDestruction, 0 }
            },

            new DropData(3,
                new List<Tuple<IInteractable.Name, float>>
                {
                    new Tuple<IInteractable.Name, float>( IInteractable.Name.Coin, 0.3f)
                }
            ), 6)
        },

        {
            BaseLife.Name.RedPentagon, new PentagonData(75, ITarget.Type.Red, BaseEnemy.Size.Medium,

            new Dictionary<BaseSkill.Name, int>
            {
                { BaseSkill.Name.SpreadBullets, 1 }
            },

            new DropData(3,
                new List<Tuple<IInteractable.Name, float>>
                {
                    new Tuple<IInteractable.Name, float>( IInteractable.Name.Coin, 0.3f)
                }
            ), 4f, 3f, 1f)
        },

        {
            BaseLife.Name.RedHexagon, new HexagonData(90, ITarget.Type.Red, BaseEnemy.Size.Medium,

            new Dictionary<BaseSkill.Name, int>
            {
                { BaseSkill.Name.Shockwave, 1 }
            },

            new DropData(3,
                new List<Tuple<IInteractable.Name, float>>
                {
                    new Tuple<IInteractable.Name, float>( IInteractable.Name.Coin, 0.3f)
                }
            ), 6f, 3f, 1f)
        },

         {
            BaseLife.Name.Tricon, new TriconData(200, ITarget.Type.Red, BaseEnemy.Size.Medium,

            new Dictionary<BaseSkill.Name, int>
            {
                { BaseSkill.Name.MagneticField, 2 }
            },

            new DropData(3,
                new List<Tuple<IInteractable.Name, float>>
                {
                    new Tuple<IInteractable.Name, float>( IInteractable.Name.Coin, 0.3f)
                }
            ), 15f, 3f, 1f, 2f, 4f)
        },

         {
            BaseLife.Name.Rhombus, new RhombusData(200, ITarget.Type.Red, BaseEnemy.Size.Medium,

            new Dictionary<BaseSkill.Name, int>
            {
                { BaseSkill.Name.MultipleShockwave, 0 },
                { BaseSkill.Name.MagneticField, 2 }
            },

            new DropData(3,
                new List<Tuple<IInteractable.Name, float>>
                {
                    new Tuple<IInteractable.Name, float>( IInteractable.Name.Coin, 0.3f)
                }
            ), 6f, 3f, 1f)
        },

         {
            BaseLife.Name.Pentagonic, new PentagonicData(200, ITarget.Type.Red, BaseEnemy.Size.Medium,

            new Dictionary<BaseSkill.Name, int>
            {
                { BaseSkill.Name.SpreadMultipleBullets, 0 },
            },

            new DropData(3,
                new List<Tuple<IInteractable.Name, float>>
                {
                    new Tuple<IInteractable.Name, float>( IInteractable.Name.Coin, 0.3f)
                }
            ), 6f, 3f, 1f)
        },

    };
    public Dictionary<BaseLife.Name, LifeData> LifeDatas { get { return CopyLifeDatas; } }


    Dictionary<BaseSkill.Name, CardInfoData> _cardDatas = new Dictionary<BaseSkill.Name, CardInfoData>
    {
        { BaseSkill.Name.SpawnBlackhole, new CardInfoData(BaseSkill.Name.SpawnBlackhole, "Spawn Blackhole In My Position", 20) },
        { BaseSkill.Name.SpawnBlade, new CardInfoData(BaseSkill.Name.SpawnBlade, "Spawn Blade", 20) },
        { BaseSkill.Name.Impact, new CardInfoData(BaseSkill.Name.Impact, "Spawn Impact", 20) },
        { BaseSkill.Name.Knockback, new CardInfoData(BaseSkill.Name.Knockback, "Spawn Knockback", 20) },

        { BaseSkill.Name.SpawnRifleShooter, new CardInfoData(BaseSkill.Name.SpawnRifleShooter, "Spawn Rifle Shooter", 20) },
        { BaseSkill.Name.SpawnRocketShooter, new CardInfoData(BaseSkill.Name.SpawnRocketShooter, "Spawn Rocket Shooter", 20) },

        { BaseSkill.Name.Statikk, new CardInfoData(BaseSkill.Name.Statikk, "Spawn Statikk", 20) },
        { BaseSkill.Name.SpawnStickyBomb, new CardInfoData(BaseSkill.Name.SpawnStickyBomb, "Spawn StickyBomb", 20) },

        { BaseSkill.Name.UpgradeDash, new CardInfoData(BaseSkill.Name.UpgradeDash, "Create DashBuff", 20) },
        { BaseSkill.Name.UpgradeShooting, new CardInfoData(BaseSkill.Name.UpgradeShooting, "Create ShootingBuff", 20) },
        { BaseSkill.Name.UpgradeDamage, new CardInfoData(BaseSkill.Name.UpgradeDamage, "Create TotalDamageBuff", 20) },
        { BaseSkill.Name.UpgradeCooltime, new CardInfoData(BaseSkill.Name.UpgradeCooltime, "Create TotalCooltimeBuff", 20) },
    };
    public Dictionary<BaseSkill.Name, CardInfoData> CardDatas { get { return _cardDatas; } }


    Dictionary<IInteractable.Name, BaseInteractableObjectData> _interactableObjectDatas = new Dictionary<IInteractable.Name, BaseInteractableObjectData>
    {
        { IInteractable.Name.CardTable, new CardTableData(3) },
        { IInteractable.Name.Coin, new CoinData(5, 25) },
        { IInteractable.Name.Heart, new HeartData(20, 25) },
        { IInteractable.Name.SkillBubble, new SkillBubbleData(1, 25) },
        { IInteractable.Name.Shop, new ShopData(3, 3) }
    };

    public Dictionary<IInteractable.Name, BaseInteractableObjectData> InteractableObjectDatas { get { return _interactableObjectDatas; } }


    SaveData _defaultSaveData = new SaveData
    (
        0,
        DungeonChapter.TriconChapter,
        new Dictionary<DungeonChapter, ChapterInfo>() 
        {
            { DungeonChapter.TriconChapter, new ChapterInfo("상쾌한 시작", 0, 20, false) },
            { DungeonChapter.RhombusChapter, new ChapterInfo("고난의 시작", 0, 20, true) },
            { DungeonChapter.PentagonicChapter, new ChapterInfo("너무 어려워", 0, 20, true) },
        }
    );

    public SaveData DefaultSaveData { get { return _defaultSaveData; } }
}
