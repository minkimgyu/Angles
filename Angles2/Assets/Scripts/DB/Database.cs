using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System;

public class Database
{
    const float _dropSpreadOffset = 2;
    public float DropSpreadOffset { get { return _dropSpreadOffset; } }

    const int _maxSkilCount = 8;
    public int MaxSkilCount { get { return _maxSkilCount; } }

    List<BaseSkill.Name> _upgradeableSkills = new List<BaseSkill.Name>
    {
        //BaseSkill.Name.Statikk,
        //BaseSkill.Name.Knockback,
        //BaseSkill.Name.Impact,
        BaseSkill.Name.SpawnBlackhole,
        //BaseSkill.Name.SpawnStickyBomb,
        //BaseSkill.Name.SpawnBlade,

        //BaseSkill.Name.SpawnRifleShooter,
        //BaseSkill.Name.SpawnRocketShooter,

        //BaseSkill.Name.CreateShootingBuff,
        //BaseSkill.Name.CreateDashBuff,
        //BaseSkill.Name.CreateTotalDamageBuff,
        //BaseSkill.Name.CreateTotalCooltimeBuff
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
                    new StatikkUpgrader.UpgradableData(10, 0, 0, 0),
                    new StatikkUpgrader.UpgradableData(0, 0, 1, 1),
                    new StatikkUpgrader.UpgradableData(10, 0, 0, 0),
                    new StatikkUpgrader.UpgradableData(0, 0, 1, 1),
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
                    new SpawnBlackholeUpgrader.UpgradableData(3, 50, 4, 1),
                    new SpawnBlackholeUpgrader.UpgradableData(4, 50, 5, 1),
                    new SpawnBlackholeUpgrader.UpgradableData(4, 60, 5, 1.2f),
                    new SpawnBlackholeUpgrader.UpgradableData(5, 60, 6, 1.2f),
                    new SpawnBlackholeUpgrader.UpgradableData(6, 80, 7, 1.5f),
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
                    new SpawnStickyBombUpgrader.UpgradableData(3, 1, 30),
                    new SpawnStickyBombUpgrader.UpgradableData(3, 1, 40),
                    new SpawnStickyBombUpgrader.UpgradableData(3, 1.2f, 50),
                    new SpawnStickyBombUpgrader.UpgradableData(3, 1.2f, 60),
                    new SpawnStickyBombUpgrader.UpgradableData(3, 1.5f, 70),
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
                    new SpawnShooterUpgrader.UpgradableData(10, 0),
                    new SpawnShooterUpgrader.UpgradableData(10, -0.5f),
                    new SpawnShooterUpgrader.UpgradableData(10, 0),
                    new SpawnShooterUpgrader.UpgradableData(10, -0.2f),
                }
            )
        },


         {
            BaseSkill.Name.SpreadBullets,
            new SpreadBulletsUpgrader
            (
                new List<SpreadBulletsUpgrader.UpgradableData>
                {
                    new SpreadBulletsUpgrader.UpgradableData(1, 8, 3, 5),
                    new SpreadBulletsUpgrader.UpgradableData(1, 8, 3, 5),
                    new SpreadBulletsUpgrader.UpgradableData(1, 8, 2.5f, 5),
                    new SpreadBulletsUpgrader.UpgradableData(1, 8, 2.5f, 5),
                    new SpreadBulletsUpgrader.UpgradableData(1, 8, 2, 5),
                }
            )
        },
         {
            BaseSkill.Name.Shockwave,
            new ShockwaveUpgrader
            (
                new List<ShockwaveUpgrader.UpgradableData>
                {
                    new ShockwaveUpgrader.UpgradableData(1, 8),
                    new ShockwaveUpgrader.UpgradableData(1, 8),
                    new ShockwaveUpgrader.UpgradableData(1, 8),
                    new ShockwaveUpgrader.UpgradableData(1, 8),
                    new ShockwaveUpgrader.UpgradableData(1, 8),
                }
            )
        },
         {
            BaseSkill.Name.MagneticField,
            new MagneticFieldUpgrader
            (
                new List<MagneticFieldUpgrader.UpgradableData>
                {
                    new MagneticFieldUpgrader.UpgradableData(1, 8),
                    new MagneticFieldUpgrader.UpgradableData(1, 8),
                    new MagneticFieldUpgrader.UpgradableData(1, 8),
                    new MagneticFieldUpgrader.UpgradableData(1, 8),
                    new MagneticFieldUpgrader.UpgradableData(1, 8),
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
                    new SelfDestructionUpgrader.UpgradableData(1, 8, 3),
                    new SelfDestructionUpgrader.UpgradableData(1, 8, 3),
                    new SelfDestructionUpgrader.UpgradableData(1, 8, 3),
                }
            )
        },
    };

    public Dictionary<BaseSkill.Name, IUpgradeVisitor> Upgraders { get { return _upgrader; } }

    Dictionary<BaseSkill.Name, SkillData> _skillDatas = new Dictionary<BaseSkill.Name, SkillData>
    {
        { BaseSkill.Name.Statikk, new StatikkData(5, 1, 1, 10, 3, 3, new List<ITarget.Type> { ITarget.Type.Red }) },

        { BaseSkill.Name.Knockback, new KnockbackData(5, 3, 1, 20, new SerializableVector2(5.5f, 3), new SerializableVector2(1.5f, 0), new List<ITarget.Type> { ITarget.Type.Red }) },

        { BaseSkill.Name.Impact, new ImpactData(5, 0.2f, 20, 2, new List<ITarget.Type> { ITarget.Type.Red }) },

        { BaseSkill.Name.SpawnBlackhole, new SpawnBlackholeData(5, 1, 4, 5, 1, 5, new List<ITarget.Type> { ITarget.Type.Red })},

        { BaseSkill.Name.SpawnRifleShooter, new SpawnShooterData(5, BaseWeapon.Name.RifleShooter, 10, 1, BaseWeapon.Name.Bullet, new List<ITarget.Type> { ITarget.Type.Red }) },

        { BaseSkill.Name.SpawnRocketShooter, new SpawnShooterData(5, BaseWeapon.Name.RocketShooter, 10, 3, BaseWeapon.Name.Rocket, new List<ITarget.Type> { ITarget.Type.Red }) },

        { BaseSkill.Name.SpawnBlade, new SpawnBladeData(5, 1, 10, 3f, 8f, new List<ITarget.Type> { ITarget.Type.Red }) },

        { BaseSkill.Name.SpawnStickyBomb, new SpawnStickyBombData(5, 5f, 3, 20, 3, new List<ITarget.Type> { ITarget.Type.Red }) },

        { BaseSkill.Name.ContactAttack, new ContactAttackData(1, 0, new List<ITarget.Type> { ITarget.Type.Red }) },

        {
            BaseSkill.Name.UpgradeDamage,
            new UpgradeDamageData(
                3,
                new List<StatUpgrader.DamageData>
                { 
                    new StatUpgrader.DamageData(0.15f),
                    new StatUpgrader.DamageData(0.15f),
                    new StatUpgrader.DamageData(0.15f),
                }
            )
        },

        {
            BaseSkill.Name.UpgradeCooltime,
            new UpgradeCooltimeData(
                3,
                new List<StatUpgrader.CooltimeData>
                {
                    new StatUpgrader.CooltimeData(-0.15f),
                    new StatUpgrader.CooltimeData(-0.15f),
                    new StatUpgrader.CooltimeData(-0.15f),
                }
            )
        },

         {
            BaseSkill.Name.UpgradeShooting,
            new UpgradeShootingData(
                3,
                new List<StatUpgrader.ShootingData>
                {
                    new StatUpgrader.ShootingData(0.15f, -0.15f),
                    new StatUpgrader.ShootingData(0.15f, -0.15f),
                    new StatUpgrader.ShootingData(0.15f, -0.15f),
                }
            )
        },

        {
            BaseSkill.Name.UpgradeDash,
            new UpgradeShootingData(
                3,
                new List<StatUpgrader.ShootingData>
                {
                    new StatUpgrader.ShootingData(0.15f, -0.15f),
                    new StatUpgrader.ShootingData(0.15f, -0.15f),
                    new StatUpgrader.ShootingData(0.15f, -0.15f),
                }
            )
        },

        { BaseSkill.Name.MultipleShockwave, new MultipleShockwaveData(1, 0.5f, 0.7f, 3, 20f, 1.7f, 3f, new List<ITarget.Type>(){ITarget.Type.Blue}) },
        { BaseSkill.Name.SpreadBullets, new SpreadBulletsData(10, 5f, 3f, 3f, 5, new List<ITarget.Type> { ITarget.Type.Blue })},
        { BaseSkill.Name.Shockwave, new ShockwaveData(1, 20f, 5f, 3f, new List<ITarget.Type>(){ITarget.Type.Blue}) },
        { BaseSkill.Name.MagneticField, new MagneticFieldData(1, 20f, 1.5f, new List<ITarget.Type>(){ITarget.Type.Blue}) },
        { BaseSkill.Name.SelfDestruction, new SelfDestructionData(1, 20f, 5f, 3f, new List<ITarget.Type>(){ITarget.Type.Blue}) },
    };

    public Dictionary<BaseSkill.Name, SkillData> SkillDatas { get { return _skillDatas; } }

    Dictionary<BaseWeapon.Name, WeaponData> _weaponDatas = new Dictionary<BaseWeapon.Name, WeaponData>
    {
        { BaseWeapon.Name.Blade, new BladeData(1)},

        { BaseWeapon.Name.Bullet, new BulletData(0, 5)},
        { BaseWeapon.Name.Rocket, new RocketData(0, 20, 5, 5)},

        { BaseWeapon.Name.Blackhole, new BlackholeData(3, 50, 4, 1, 0.1f)},
        { BaseWeapon.Name.StickyBomb, new StickyBombData(0, 1, 3)},

        { BaseWeapon.Name.RifleShooter, new ShooterData(0, 10, 1, 10, 4.0f, 10.0f)},
        { BaseWeapon.Name.RocketShooter, new ShooterData(0, 10, 1, 10, 4.0f, 10.0f)},

    };
    public Dictionary<BaseWeapon.Name, WeaponData> WeaponDatas { get { return _weaponDatas; } }

    Dictionary<BaseLife.Name, LifeData> _lifeDatas = new Dictionary<BaseLife.Name, LifeData>
    {
        { 
            BaseLife.Name.Player, new PlayerData(
                1000, 
                ITarget.Type.Blue, 
                10, 

                0.2f,
                3,
                0.8f,

                15,
                25,
                15,

                0.5f, 
                1.5f, 
                0.5f, 

                2f,
                10f,
                2f,

                20f,
                0.2f, 
                3, 
                1, 

                0.3f,
                2f,
                1f, 

                0.15f, 
                0.3f, 
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
                    new Tuple<IInteractable.Name, float>( IInteractable.Name.Coin, 0.3f)
                }
            ), 5) 
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
                    new Tuple<IInteractable.Name, float>( IInteractable.Name.Coin, 0.3f)
                }
            ), 5)
        },

        { 
            BaseLife.Name.YellowPentagon, new PentagonData(40, ITarget.Type.Red, BaseLife.Size.Middle,

            new Dictionary<BaseSkill.Name, int>
            {
                { BaseSkill.Name.SpreadBullets, 0 }
            },

            new DropData(3,
                new List<Tuple<IInteractable.Name, float>>
                {
                    new Tuple<IInteractable.Name, float>( IInteractable.Name.Coin, 0.3f)
                }
            ), 5f, 3f, 1f)
        },

        { 
            BaseLife.Name.YellowHexagon, new HexagonData(60, ITarget.Type.Red, BaseLife.Size.Middle,

            new Dictionary<BaseSkill.Name, int>
            {
                { BaseSkill.Name.Shockwave, 0 }
            },

            new DropData(3,
                new List<Tuple<IInteractable.Name, float>>
                {
                    new Tuple<IInteractable.Name, float>( IInteractable.Name.Coin, 0.3f)
                }
            ), 5f, 3f, 1f)
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
            ), 5)
        },

        {
            BaseLife.Name.RedRectangle, new RectangleData(30, ITarget.Type.Red, BaseEnemy.Size.Small,

            new Dictionary<BaseSkill.Name, int>
            {
                { BaseSkill.Name.MagneticField, 1 }
            },

            new DropData(3,
                new List<Tuple<IInteractable.Name, float>>
                {
                    new Tuple<IInteractable.Name, float>( IInteractable.Name.Coin, 0.3f)
                }
            ), 5)
        },

        {
            BaseLife.Name.RedPentagon, new PentagonData(75, ITarget.Type.Red, BaseEnemy.Size.Middle,

            new Dictionary<BaseSkill.Name, int>
            {
                { BaseSkill.Name.SpreadBullets, 1 }
            },

            new DropData(3,
                new List<Tuple<IInteractable.Name, float>>
                {
                    new Tuple<IInteractable.Name, float>( IInteractable.Name.Coin, 0.3f)
                }
            ), 5f, 3f, 1f)
        },

        {
            BaseLife.Name.RedHexagon, new HexagonData(90, ITarget.Type.Red, BaseEnemy.Size.Middle,

            new Dictionary<BaseSkill.Name, int>
            {
                { BaseSkill.Name.Shockwave, 1 }
            },

            new DropData(3,
                new List<Tuple<IInteractable.Name, float>>
                {
                    new Tuple<IInteractable.Name, float>( IInteractable.Name.Coin, 0.3f)
                }
            ), 5f, 3f, 1f)
        },

        {
            BaseLife.Name.Lombard, new LombardData(200, ITarget.Type.Red, BaseEnemy.Size.Large,

            new Dictionary<BaseSkill.Name, int>
            {
                { BaseSkill.Name.MultipleShockwave, 0 },
                { BaseSkill.Name.MagneticField, 0 }
            },

            new DropData(3,
                new List<Tuple<IInteractable.Name, float>>
                {
                    new Tuple<IInteractable.Name, float>( IInteractable.Name.Coin, 0.3f)
                }
            ), 5f, 3f, 1f)
        },

    };
    public Dictionary<BaseLife.Name, LifeData> LifeDatas { get { return _lifeDatas; } }


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
        { IInteractable.Name.SkillBubble, new SkillBubbleData(1, 25) },
        { IInteractable.Name.Shop, new ShopData(3, 3) }
    };

    public Dictionary<IInteractable.Name, BaseInteractableObjectData> InteractableObjectDatas { get { return _interactableObjectDatas; } }
}
